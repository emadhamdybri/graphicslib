using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Drawables.Textures;
using Math3D;

namespace MD3
{
    public delegate string ProcessSurfacePathHandler ( string name );

    internal class AnimationTracker
    {
        AnimationSequence sequence;
        double lastTime = -1;
        int thisFrame = 0;
        int nextFrame = 0;
        double param;

        public delegate void SequenceEvent ( AnimationTracker sender, string name );
        public event SequenceEvent AnimationLooped;
        public event SequenceEvent AnimationEnded;
        public event SequenceEvent FrameChanged;

        bool animated = false;

        public int ThisFrame 
        {
            get { return thisFrame; }
        }

        public int NextFrame
        {
            get { return nextFrame; }
        }

        public float Paramater
        {
            get { return (float)param; }
        }

        Stopwatch timer;

        public AnimationTracker ( AnimationSequence seq )
        {
            sequence = seq;
            timer = new Stopwatch();
            timer.Start();
            thisFrame = seq.StartFrame;
            if (seq.StartFrame != seq.EndFrame)
            {
                nextFrame = thisFrame + 1;
                animated = true;
            }
            else
                nextFrame = thisFrame;

            param = 0;
        }

        public void Update ()
        {
            if (!animated)
            {
                thisFrame = sequence.EndFrame;
                nextFrame = thisFrame;
                param = 0;
                return;
            }

            double now = timer.ElapsedMilliseconds / 1000.0;

            if (thisFrame < sequence.StartFrame || thisFrame > sequence.EndFrame || lastTime < 0) // starting over
            {
                thisFrame = sequence.StartFrame;
                nextFrame = thisFrame + 1;
                param = 0;
                lastTime = now;
            }
            else
            {
                float frameTime = 1.0f/(sequence.FPS * CharacterInstance.FPSScale);

                param = (now - lastTime) / frameTime;

                if (param >= 1)
                {
                    param = 0;
                    thisFrame = nextFrame;
                    nextFrame++;
                    if (nextFrame > sequence.EndFrame)
                        nextFrame = sequence.LoopPoint;  
                
                    if (thisFrame == sequence.EndFrame) // we hit the end of the loop
                    {
                        if (sequence.LoopPoint == sequence.EndFrame)
                        {
                            if (AnimationEnded != null)
                                AnimationEnded(this, sequence.Name);
                        }
                        else
                        {
                            if (AnimationLooped != null)
                                AnimationLooped(this, sequence.Name);
                        }
                    }

                    lastTime = now;
                    if (FrameChanged != null)
                        FrameChanged(this, sequence.Name);
                }
            }
        }
    }

    public class CharacterInstance
    {
        public static float FPSScale = 1.0f;

        public delegate void SequenceEvent ( CharacterInstance sender, string name, ComponentType part );
        public delegate void SequenceFrameEvent(CharacterInstance sender, int frame, ComponentType part);

        public event SequenceEvent AnimationLooped;
        public event SequenceEvent AnimationEnded;
        public event SequenceFrameEvent FrameChanged;

        protected Character character;
        internal AnimationTracker legSequence;
        internal AnimationTracker torsoSequence;

        protected Dictionary<Tag,Matrix4> TagMatrixOffsets = new Dictionary<Tag,Matrix4>();

        protected List<Mesh> HiddenMeshes = new List<Mesh>();

        protected Dictionary<Mesh, Texture> skinTextures;
        protected string skinName = string.Empty;

        public static ProcessSurfacePathHandler ProcessSurfacePath = null;

        public static bool DrawTags = false;

        public Matrix4 LegTorsoMatrix = Matrix4.Identity;
        public Matrix4 TorsoHeadMatrix = Matrix4.Identity;

        public bool InterpolateMeshes = false;
        public bool InterpolateNormals = false;
        public bool InterpolateTagPosition = false;
        public bool InterpolateTagRotation = false;

        Stopwatch frameTimer = new Stopwatch();

        double lastFrameTime = 0;

        public double FrameTime
        {
            get { return lastFrameTime; }
        }

        public CharacterInstance(Character c)
        {
            character = c;
            frameTimer.Start();
        }

        public void BindSkin ()
        {
            skinTextures = new Dictionary<Mesh, Texture>();

            Skin skin = null;
            if (character.SkinExists(skinName))
                skin = character.GetSkin(skinName);
            else if (character.SkinExists("default"))
                skin = character.GetSkin("default");
            else if (character.SkinExists(string.Empty))
                skin = character.GetSkin(string.Empty);
            else if (character.Skins.Count > 0)
                skin = character.Skins[0];
            else
                skin = character.SkinFromSurfs();

            if (skin == null)
                return;

            foreach (KeyValuePair<string,Dictionary<string,string>> SkinComponent in skin.Surfaces)
            {
                Component comp = character.FindComponent(SkinComponent.Key);
                if (comp != null)
                {
                    foreach (KeyValuePair<string,string> meshTextures in SkinComponent.Value)
                    {
                        Mesh mesh = comp.FindMesh(meshTextures.Key);
                        if (mesh != null)
                        {
                            string path = meshTextures.Value;
                            if (ProcessSurfacePath != null)
                                path = ProcessSurfacePath(path);
                            Texture tex = TextureSystem.system.GetTexture(path);
                            if (tex != null)
                                skinTextures.Add(mesh, tex);
                            else
                            {
                                string name = Path.GetFileNameWithoutExtension(path);
                                if (name[name.Length-1] == 'a')
                                {
                                    name = name.TrimEnd("a".ToCharArray());
                                    FileInfo file = new FileInfo(Path.Combine(Path.GetDirectoryName(path), name + Path.GetExtension(path)));
                                    if (file.Exists)
                                        tex = TextureSystem.system.GetTexture(file.FullName);
                                    if (tex != null)
                                        skinTextures.Add(mesh, tex);
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool Draw()
        {
            double current = frameTimer.ElapsedMilliseconds / 1000.0;

            UpdateSequence();

            if (character.RootNode == null)
                return false;

            if (skinTextures == null)
                BindSkin();

            DrawCompoenent(character.RootNode);

            double now = frameTimer.ElapsedMilliseconds / 1000.0;
            lastFrameTime = now - current;
            return true;
        }

        void UpdateSequence()
        {
            if (legSequence != null)
                legSequence.Update();

            if (torsoSequence != null)
                torsoSequence.Update();
        }

        internal int getFrame ( AnimationTracker tracker )
        {
            if (tracker == null)
                return 0;

            return tracker.ThisFrame;
        }

        internal int getNextFrame(AnimationTracker tracker)
        {
            if (tracker == null)
                return 0;

            return tracker.NextFrame;
        }

        internal float getParam(AnimationTracker tracker)
        {
            if (tracker == null)
                return 0;

            return tracker.Paramater;
        }

        internal AnimationTracker getTracker(ComponentType part)
        {
            if (part == ComponentType.Head || part == ComponentType.Other)
                return null;

            if (part == ComponentType.Legs)
                return legSequence;
            else
                return torsoSequence;
        }

        protected bool PartAnimated ( ComponentType part )
        {
            AnimationTracker tracker = getTracker(part);
            if (tracker == null)
                return false;

            return tracker.ThisFrame != tracker.NextFrame;
        }

        protected Matrix4 GetTagMatrix(ComponentType part, FrameMatrix[] frames)
        {
            if (frames.Length == 1)
                return frames[0].Matrix;

            if ((!InterpolateTagPosition && !InterpolateTagRotation) || !PartAnimated(part))
                return frames[getFrame(getTracker(part))].Matrix;

            AnimationTracker tracker = getTracker(part);
            if (frames.Length <= tracker.ThisFrame)
                return frames[0].Matrix;

            Vector3 newTrans = frames[tracker.ThisFrame].Position + ((frames[tracker.NextFrame].Position - frames[tracker.ThisFrame].Position) * tracker.Paramater);
           
            if (!InterpolateTagRotation)
                return frames[tracker.ThisFrame].Rotation * Matrix4.CreateTranslation(newTrans);

            Quaternion thisRot = QuaternionHelper.FromMatrix(frames[tracker.ThisFrame].Rotation);
            Quaternion nextRot = QuaternionHelper.FromMatrix(frames[tracker.NextFrame].Rotation);

            Quaternion frameQuat = Quaternion.Slerp(thisRot, nextRot, tracker.Paramater);
            frameQuat.Normalize();
            return MatrixHelper4.FromQuaternion(frameQuat) * Matrix4.CreateTranslation(newTrans);
        }

        internal void DrawCompoenent(ConnectedComponent component)
        {
            int thisFrame = getFrame(getTracker(component.Part.PartType));
            foreach (Mesh mesh in component.Part.Meshes)
                DrawMesh(mesh, component.Part.PartType);

            foreach (KeyValuePair<Tag,List<ConnectedComponent>> child in component.Children)
            {
                if (child.Key.Frames.Length > 0)
                {
                    GL.PushMatrix();

                    int realFrame = 0;
                    if (thisFrame < child.Key.Frames.Length)
                        realFrame = thisFrame;

                    Matrix4 m = GetTagMatrix(component.Part.PartType, child.Key.Frames);
                    GL.MultMatrix(ref m);

                    if (DrawTags)
                    {
                        float tagSize = 2.0f;
                        GL.Disable(EnableCap.Texture2D);
                        GL.Disable(EnableCap.Lighting);

                        GL.Begin(BeginMode.Lines);
                            GL.Color4(Color.Red);
                            GL.Vertex3(0,0,0);
                            GL.Vertex3(tagSize, 0, 0);
                            GL.Color4(Color.Green);
                            GL.Vertex3(0,0,0);
                            GL.Vertex3(0, tagSize, 0);
                            GL.Color4(Color.Blue);
                            GL.Vertex3(0,0,0);
                            GL.Vertex3(0, 0, tagSize);
                        GL.End();
                        GL.Color4(Color.White);

                        GL.Enable(EnableCap.Lighting);
                    }

                    foreach (ConnectedComponent c in child.Value)
                    {
                        Tag destTag = c.Part.FindTag(child.Key.Name);
                        GL.PushMatrix();

                        if (component.Part.PartType == ComponentType.Legs && c.Part.PartType == ComponentType.Torso)
                            GL.MultMatrix(ref LegTorsoMatrix);
                        else if (component.Part.PartType == ComponentType.Torso && c.Part.PartType == ComponentType.Head)
                            GL.MultMatrix(ref TorsoHeadMatrix);

                        Matrix4 destMat = Matrix4.Identity;
                        if (destTag != null)
                            destMat = GetTagMatrix(c.Part.PartType, destTag.Frames);

                        GL.MultMatrix(ref destMat);
                        DrawCompoenent(c);
                        GL.PopMatrix();
                    }
                    GL.PopMatrix();
                }
            }
        }

        protected void DrawMesh ( Mesh mesh, ComponentType part )
        {
            if (HiddenMeshes.Contains(mesh) || mesh.Frames.Length < 1)
                return;

            Texture tex = null;
            if (skinTextures != null && skinTextures.ContainsKey(mesh))
                tex = skinTextures[mesh];
            else
                return;

            tex.Bind();

            GL.Color4(Color.White);
            GL.Begin(BeginMode.Triangles);

            int thisFrame = getFrame(getTracker(part));
            int nextFrame = getNextFrame(getTracker(part));
            float param = getParam(getTracker(part));

            Vector3[] theseVerts;
            Vector3[] theseNorms;

            if (!InterpolateMeshes || thisFrame == nextFrame || thisFrame >= mesh.Frames.Length)
            {
                if (thisFrame >= mesh.Frames.Length)
                {
                    theseVerts = mesh.Frames[0].Positions;
                    theseNorms = mesh.Frames[0].Normals;
                }
                else
                {
                    theseVerts = mesh.Frames[thisFrame].Positions;
                    theseNorms = mesh.Frames[thisFrame].Normals;
                }
            }
            else
            {
                theseVerts = new Vector3[mesh.Frames[thisFrame].Positions.Length];
                theseNorms = new Vector3[mesh.Frames[thisFrame].Normals.Length];

                for(int i = 0; i < theseVerts.Length; i++ )
                {
                    theseVerts[i] = mesh.Frames[thisFrame].Positions[i] + ((mesh.Frames[nextFrame].Positions[i] - mesh.Frames[thisFrame].Positions[i]) *param);

                    if (InterpolateNormals)
                    {
                        theseNorms[i] = mesh.Frames[thisFrame].Normals[i] + ((mesh.Frames[nextFrame].Normals[i] - mesh.Frames[thisFrame].Normals[i]) * param);
                        theseNorms[i].NormalizeFast();
                    }
                    else
                        theseNorms[i] = mesh.Frames[thisFrame].Normals[i];
                }
            }

            foreach(Triangle triangle in mesh.Triangles)
            {
                foreach (int index in triangle.Verts)
                {
                    GL.TexCoord2(mesh.UVs[index]);
                    GL.Normal3(theseNorms[index]);
                    GL.Vertex3(theseVerts[index]);
                }
            }

            GL.End();
        }

        public void HideMesh ( string name )
        {
            foreach( Component c in character.Componenets )
            {
                foreach (Mesh m in c.Meshes)
                {
                    if (m.Name == name)
                    {
                        if (!HiddenMeshes.Contains(m))
                            HiddenMeshes.Add(m);
                    }
                }
            }
        }

        public void ShowMesh(string name)
        {
            foreach (Component c in character.Componenets)
            {
                foreach (Mesh m in c.Meshes)
                {
                    if (m.Name == name)
                    {
                        if (HiddenMeshes.Contains(m))
                            HiddenMeshes.Remove(m);
                    }
                }
            }
        }

        public void SetSkin ( string name )
        {
            skinName = name;
            BindSkin();
        }

        public bool SetTorsoSequence(string name)
        {
            AnimationSequence sequence = character.FindSequence("torso",name);
            if (sequence == null)
            {
                torsoSequence = null;
                return false;
            }

            torsoSequence = new AnimationTracker(sequence);
            torsoSequence.AnimationEnded += new AnimationTracker.SequenceEvent(torsoSequence_AnimationEnded);
            torsoSequence.AnimationLooped += new AnimationTracker.SequenceEvent(torsoSequence_AnimationLooped);
            if (FrameChanged != null)
                torsoSequence.FrameChanged += new AnimationTracker.SequenceEvent(torsoSequence_FrameChanged);
            return true;
        }

        void torsoSequence_FrameChanged(AnimationTracker sender, string name)
        {
            if (FrameChanged != null)
                FrameChanged(this, sender.ThisFrame, ComponentType.Torso);
        }

        void torsoSequence_AnimationLooped(AnimationTracker sender, string name)
        {
            if (AnimationLooped != null)
                AnimationLooped(this, name, ComponentType.Torso);
        }

        void torsoSequence_AnimationEnded(AnimationTracker sender, string name)
        {
            if (AnimationEnded != null)
                AnimationEnded(this, name, ComponentType.Torso);
        }

        public bool SetLegSequence(string name)
        {
            AnimationSequence sequence = character.FindSequence("legs", name);
            if (sequence == null)
            {
                legSequence = null;
                return false;
            }

            legSequence = new AnimationTracker(sequence);
            legSequence.AnimationEnded += new AnimationTracker.SequenceEvent(legSequence_AnimationEnded);
            legSequence.AnimationLooped += new AnimationTracker.SequenceEvent(legSequence_AnimationLooped);
            if (FrameChanged != null)
                legSequence.FrameChanged += new AnimationTracker.SequenceEvent(legSequence_FrameChanged);
            return true;
        }

        void legSequence_FrameChanged(AnimationTracker sender, string name)
        {
            if (FrameChanged != null)
                FrameChanged(this, sender.ThisFrame, ComponentType.Legs);
        }

        void legSequence_AnimationLooped(AnimationTracker sender, string name)
        {
            if (AnimationLooped != null)
                AnimationLooped(this, name, ComponentType.Legs);
        }

        void legSequence_AnimationEnded(AnimationTracker sender, string name)
        {
            if (AnimationEnded != null)
                AnimationEnded(this, name, ComponentType.Legs);
        }
    }
}
