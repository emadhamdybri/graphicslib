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
    public delegate string ProcessSurfacePathHandler ( ModelTree model, string name );

    internal class AnimationTracker
    {
        public AnimationSequence Sequence;
        double lastTime = -1;
        int thisFrame = 0;
        int nextFrame = 0;
        double param;

        public Component Part = null;

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
            Sequence = seq;
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

        public void Reset ()
        {
            if (!animated)
                return;
            lastTime = -1;
        }

        public void Update ()
        {
            if (!animated)
            {
                thisFrame = Sequence.EndFrame;
                nextFrame = thisFrame;
                param = 0;
                return;
            }

            double now = timer.ElapsedMilliseconds / 1000.0;

            if (thisFrame < Sequence.StartFrame || thisFrame > Sequence.EndFrame || lastTime < 0) // starting over
            {
                thisFrame = Sequence.StartFrame;
                nextFrame = thisFrame + 1;
                param = 0;
                lastTime = now;
            }
            else
            {
                float frameTime = 1.0f / (Sequence.FPS * CharacterInstance.FPSScale);

                param = (now - lastTime) / frameTime;

                if (param >= 1)
                {
                    param = 0;
                    thisFrame = nextFrame;
                    nextFrame++;
                    if (nextFrame > Sequence.EndFrame)
                        nextFrame = Sequence.LoopPoint;

                    if (thisFrame == Sequence.EndFrame) // we hit the end of the loop
                    {
                        if (Sequence.LoopPoint == Sequence.EndFrame)
                        {
                            if (AnimationEnded != null)
                                AnimationEnded(this, Sequence.Name);
                        }
                        else
                        {
                            if (AnimationLooped != null)
                                AnimationLooped(this, Sequence.Name);
                        }
                    }

                    lastTime = now;
                    if (FrameChanged != null)
                        FrameChanged(this, Sequence.Name);
                }
            }
        }
    }

    public class AnimatedInstance
    {
        public static float FPSScale = 1.0f;

        public delegate void SequenceEvent(AnimatedInstance sender, string name, ComponentType part);
        public delegate void SequenceFrameEvent(AnimatedInstance sender, int frame, ComponentType part);

        public event SequenceEvent AnimationLooped;
        public event SequenceEvent AnimationEnded;
        public event SequenceFrameEvent FrameChanged;
        
        protected ModelTree model;
       
        protected Dictionary<Tag, Matrix4> TagMatrixOffsets = new Dictionary<Tag, Matrix4>();
        protected List<Mesh> HiddenMeshes = new List<Mesh>();
       
        protected Dictionary<Mesh, Texture> skinTextures;
        protected string skinName = string.Empty;
      
        public static ProcessSurfacePathHandler ProcessSurfacePath = null;

        public static bool DrawTags = false;
        
        public bool InterpolateMeshes = false;
        public bool InterpolateNormals = false;
        public bool InterpolateTagPosition = false;
        public bool InterpolateTagRotation = false;

        Stopwatch frameTimer = new Stopwatch();

        internal Dictionary<Component,AnimationTracker> ActiveTrackers = new Dictionary<Component,AnimationTracker>();

        protected Dictionary<Component, Dictionary<Tag, ModelTree>> AttachedItems = new Dictionary<Component, Dictionary<Tag, ModelTree>>();

        double lastFrameTime = 0;

        bool InUpdate = false;

        protected List<KeyValuePair<Component, AnimationSequence>> animsToReplace = new List<KeyValuePair<Component, AnimationSequence>>();

        public double FrameTime
        {
            get { return lastFrameTime; }
        }

        public AnimatedInstance ( ModelTree tree )
        {
            model = tree;
            frameTimer.Start();
        }

        public void AttatchItem(Component root, Tag tag, ModelTree item )
        {
            if (!AttachedItems.ContainsKey(root))
                AttachedItems.Add(root, new Dictionary<Tag, ModelTree>());

            if (AttachedItems[root].ContainsKey(tag))
                AttachedItems[root][tag] = item;
            else
                AttachedItems[root].Add(tag, item);

            SetSkin(skinName);
        }

        public void BindSkin ()
        {
            BindSkin(model);
        }

        public void BindSkin (ModelTree tree)
        {
            if (skinTextures == null)
                skinTextures = new Dictionary<Mesh, Texture>();

            Skin skin = null;
            if (tree.SkinExists(skinName))
                skin = model.GetSkin(skinName);
            else if (tree.SkinExists("default"))
                skin = model.GetSkin("default");
            else if (tree.SkinExists(string.Empty))
                skin = tree.GetSkin(string.Empty);
            else if (tree.Skins.Count > 0)
                skin = tree.Skins[0];
            else
                skin = tree.SkinFromSurfs();

            if (skin == null)
                return;

            foreach (KeyValuePair<string,Dictionary<string,string>> SkinComponent in skin.Surfaces)
            {
                Component comp = tree.FindComponent(SkinComponent.Key);
                if (comp != null)
                {
                    foreach (KeyValuePair<string,string> meshTextures in SkinComponent.Value)
                    {
                        Mesh mesh = comp.FindMesh(meshTextures.Key);
                        if (mesh != null)
                        {
                            string path = meshTextures.Value;
                            if (ProcessSurfacePath != null)
                                path = ProcessSurfacePath(tree,path);
                            Texture tex = TextureSystem.system.GetTexture(path);
                            if (tex != null)
                                skinTextures.Add(mesh, tex);
                            else
                            {
                                string name = Path.GetFileNameWithoutExtension(path);
                                FileInfo file = null;
                                if (name[name.Length - 1] == 'a')
                                {
                                    name = name.TrimEnd("a".ToCharArray());
                                    file = new FileInfo(Path.Combine(Path.GetDirectoryName(path), name + Path.GetExtension(path)));
                                    if (file.Exists)
                                        tex = TextureSystem.system.GetTexture(file.FullName);
                                    if (tex != null)
                                        skinTextures.Add(mesh, tex);
                                }
                                else
                                {
                                    name = Path.GetFileNameWithoutExtension(path) + ".tga";
                                    file = new FileInfo(Path.Combine(Path.GetDirectoryName(path), name));

                                    if (file.Exists)
                                        tex = TextureSystem.system.GetTexture(file.FullName);
                                    if (tex != null)
                                        skinTextures.Add(mesh, tex);
                                    else
                                    {
                                        name = Path.GetFileNameWithoutExtension(path) + ".jpg";
                                        file = new FileInfo(Path.Combine(Path.GetDirectoryName(path), name));

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
        }
     
        public bool Draw()
        {
            double current = frameTimer.ElapsedMilliseconds / 1000.0;

            UpdateSequence();

            if (model.RootNode == null)
                return false;

            if (skinTextures == null)
                BindSkin();

            DrawCompoenent(model.RootNode);

            double now = frameTimer.ElapsedMilliseconds / 1000.0;
            lastFrameTime = now - current;
            return true;
        }

        protected virtual void UpdateSequence()
        {
            InUpdate = true;
            foreach (KeyValuePair<Component,AnimationTracker> tracker in ActiveTrackers)
                tracker.Value.Update();

            InUpdate = false;

            foreach (KeyValuePair<Component, AnimationSequence> replaceItems in animsToReplace)
                SetAnimationTracker(replaceItems.Key, replaceItems.Value);

            animsToReplace.Clear();
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

        protected bool PartAnimated ( Component component )
        {
            if (!ActiveTrackers.ContainsKey(component))
                return false;

            return ActiveTrackers[component].ThisFrame != ActiveTrackers[component].NextFrame;
        }

        internal Matrix4 GetTagMatrix(Component component, AnimationTracker tracker, FrameMatrix[] frames)
        {
            if (frames.Length == 1)
                return frames[0].Matrix;

            if (tracker == null || (!InterpolateTagPosition && !InterpolateTagRotation) || !PartAnimated(component))
                return frames[getFrame(tracker)].Matrix;

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
            AnimationTracker tracker = null;
            if (ActiveTrackers.ContainsKey(component.Part))
                tracker = ActiveTrackers[component.Part];

            int thisFrame = getFrame(tracker);
            foreach (Mesh mesh in component.Part.Meshes)
                DrawMesh(mesh, tracker);

            if (AttachedItems.ContainsKey(component.Part))
            {
                Dictionary<Tag, ModelTree> attachedItems = AttachedItems[component.Part];

                foreach(Tag tag in component.Part.Tags)
                {
                    if (attachedItems.ContainsKey(tag))
                    {
                        GL.PushMatrix();

                        ModelTree attatchedItem = attachedItems[tag];

                        Tag attachTag = attatchedItem.FindTagInRoot(tag.Name);

                        Matrix4 m = GetTagMatrix(component.Part, tracker, tag.Frames);
                        GL.MultMatrix(ref m);

                        Matrix4 attatchMat = Matrix4.Identity;
                        if (attachTag != null)
                            attatchMat = GetTagMatrix(attatchedItem.GetRootComponent(), tracker, attachTag.Frames);

                        GL.MultMatrix(ref attatchMat);

                        DrawCompoenent(attatchedItem.RootNode);
                        GL.PopMatrix();
                    }
                }
            }

            foreach (KeyValuePair<Tag,List<ConnectedComponent>> child in component.Children)
            {
                if (child.Key.Frames.Length > 0)
                {
                    GL.PushMatrix();

                    Matrix4 m = GetTagMatrix(component.Part, tracker, child.Key.Frames);
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

                        if (TagMatrixOffsets.ContainsKey(child.Key))
                        {
                            Matrix4 offsetMat = TagMatrixOffsets[child.Key];
                            GL.MultMatrix(ref offsetMat);
                        }
                     
                        Matrix4 destMat = Matrix4.Identity;
                        if (destTag != null)
                        {
                            AnimationTracker destTracker = null;
                            if (ActiveTrackers.ContainsKey(c.Part))
                                destTracker = ActiveTrackers[c.Part];
                            destMat = GetTagMatrix(c.Part, destTracker, destTag.Frames);
                        }

                        GL.MultMatrix(ref destMat);
                        DrawCompoenent(c);
                        GL.PopMatrix();
                    }

                    GL.PopMatrix();
                }
            }
        }

        internal void DrawMesh ( Mesh mesh, AnimationTracker tracker )
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

            int thisFrame = getFrame(tracker);
            int nextFrame = getNextFrame(tracker);
            float param = getParam(tracker);

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
            foreach( Component c in model.Componenets )
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
            foreach (Component c in model.Componenets)
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
            skinTextures = null;
            BindSkin();

            foreach(KeyValuePair<Component,Dictionary<Tag,ModelTree>> attach in AttachedItems)
            {
                foreach(KeyValuePair<Tag,ModelTree> item in attach.Value)
                    BindSkin(item.Value);
            }
        }

        public bool SetAnimationTracker ( Component component, AnimationSequence sequence )
        {
            if (InUpdate)
            {
                animsToReplace.Add(new KeyValuePair<Component, AnimationSequence>(component, sequence));
                return true;
            }

            if (ActiveTrackers.ContainsKey(component))
            {
                if (ActiveTrackers[component].Sequence == sequence)
                {
                    ActiveTrackers[component].Reset();
                    return true;
                }

                ActiveTrackers.Remove(component);
            }

            if (sequence == null)
                return false;

            AnimationTracker tracker = new AnimationTracker(sequence);
            tracker.Part = component;
            tracker.AnimationEnded += new AnimationTracker.SequenceEvent(Sequence_AnimationEnded);
            tracker.AnimationLooped += new AnimationTracker.SequenceEvent(Sequence_AnimationLooped);
            if (FrameChanged != null)
                tracker.FrameChanged += new AnimationTracker.SequenceEvent(Sequence_FrameChanged);

            ActiveTrackers.Add(component, tracker);
            return true;
        }

        void Sequence_FrameChanged(AnimationTracker sender, string name)
        {
            if (FrameChanged == null)
                return;
            FrameChanged(this, sender.ThisFrame, sender.Part.PartType);
        }

        void Sequence_AnimationLooped(AnimationTracker sender, string name)
        {
            if (AnimationLooped != null)
                AnimationLooped(this, name, sender.Part.PartType);
        }

        void Sequence_AnimationEnded(AnimationTracker sender, string name)
        {
            if (AnimationEnded != null)
                AnimationEnded(this, name, sender.Part.PartType);
        }
    }

    public class CharacterInstance : AnimatedInstance
    {
        internal Component legComponent;
        internal Component torsoComponent;

        public Matrix4 LegTorsoMatrix
        {
            get 
            {
                if (LegTorsoTag == null || !TagMatrixOffsets.ContainsKey(LegTorsoTag))
                    return Matrix4.Identity;

                return TagMatrixOffsets[LegTorsoTag];
            }

            set 
            {
                if (LegTorsoTag == null)
                    return;
                if (TagMatrixOffsets.ContainsKey(LegTorsoTag))
                    TagMatrixOffsets[LegTorsoTag] = value;
                else
                    TagMatrixOffsets.Add(LegTorsoTag, value);
            }
        }

        public Matrix4 TorsoHeadMatrix
        {
            get
            {
                if (TorsoHeadTag == null || !TagMatrixOffsets.ContainsKey(TorsoHeadTag))
                    return Matrix4.Identity;

                return TagMatrixOffsets[TorsoHeadTag];
            }

            set
            {
                if (TorsoHeadTag == null)
                    return;
                if (TagMatrixOffsets.ContainsKey(TorsoHeadTag))
                    TagMatrixOffsets[TorsoHeadTag] = value;
                else
                    TagMatrixOffsets.Add(TorsoHeadTag, value);
            }
        }

        protected Tag LegTorsoTag = null;
        protected Tag TorsoHeadTag = null;

        public CharacterInstance(Character c) : base(c)
        {
            Component head = null;

            foreach(Component component in c.Componenets)
            {
                if (component.PartType == ComponentType.Torso)
                    torsoComponent = component;
               
                if (component.PartType == ComponentType.Legs)
                    legComponent = component;
               
                if (component.PartType == ComponentType.Head)
                    head = component;
            }

            if (torsoComponent != null)
            {
                foreach(Tag tag in legComponent.Tags)
                {
                    if (torsoComponent.FindTag(tag.Name) != null)
                    {
                        LegTorsoTag = tag;
                        break;
                    }
                }

                if (LegTorsoTag == null)
                {
                    LegTorsoTag= legComponent.SearchTag("torso");
                    if (LegTorsoTag == null)
                        LegTorsoTag = legComponent.SearchTag("upper");
                }
            }

            if (head != null)
            {
                foreach (Tag tag in torsoComponent.Tags)
                {
                    if (head.FindTag(tag.Name) != null)
                    {
                        TorsoHeadTag = tag;
                        break;
                    }
                }

                if (TorsoHeadTag == null)
                    TorsoHeadTag = legComponent.SearchTag("head");
            }
        }

        public void AttatchWeapon ( ModelTree tree )
        {
            Component attatchComponent = null;
            Tag attachTag = null;

            if (torsoComponent != null)
            {
                attachTag = torsoComponent.SearchTag("weapon");
                if (attachTag != null)
                    attatchComponent = torsoComponent;
            }

            if (attatchComponent == null && legComponent != null)
            {
                attachTag = legComponent.SearchTag("weapon");
                if (attachTag != null)
                    attatchComponent = legComponent;
            }

            if (attatchComponent != null && attachTag != null)
                AttatchItem(attatchComponent, attachTag, tree);
        }
      
        public bool SetTorsoSequence(string name)
        {
            if (torsoComponent == null)
                return false;

            return SetAnimationTracker(torsoComponent,model.FindSequence("torso", name));
        }

        public bool SetLegSequence(string name)
        {
            if (legComponent == null)
                return false;

            return SetAnimationTracker(legComponent, model.FindSequence("legs", name));
        }
    }
}
