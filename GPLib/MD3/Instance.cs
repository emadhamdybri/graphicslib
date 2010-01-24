using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Drawables.Textures;
using System.Drawing;

namespace MD3
{
    public delegate string ProcessSurfacePathHandler ( string name );

    public class CharacterInstance
    {
        protected Character character;
        protected AnimationSequence sequence;

        protected Dictionary<Tag,Matrix4> TagMatrixOffsets = new Dictionary<Tag,Matrix4>();

        protected List<Mesh> HiddenMeshes = new List<Mesh>();

        protected Dictionary<Mesh, Texture> skinTextures;
        protected string skinName = string.Empty;

        public static ProcessSurfacePathHandler ProcessSurfacePath = null;

        public static bool DrawTags = false;

        int lastFrame = -1;
        float lastTime = -1;

        int thisFrame = 0;

        public CharacterInstance(Character c)
        {
            character = c;
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
                                    else
                                    {
                                        int i = 0;
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
            if (character.RootNode == null)
                return false;

            if (skinTextures == null)
                BindSkin();

            DrawCompoenent(character.RootNode);
            return true;
        }

        internal void DrawCompoenent(ConnectedComponent component)
        {
            foreach (Mesh mesh in component.Part.Meshes)
                DrawMesh(mesh);

            foreach (KeyValuePair<Tag,List<ConnectedComponent>> child in component.Children)
            {
                if (child.Key.Frames.Length > 0)
                {
                    GL.PushMatrix();

                    int realFrame = 0;
                    if (thisFrame < child.Key.Frames.Length)
                        realFrame = thisFrame;

                    FrameMatrix mat = child.Key.Frames[realFrame];
                    GL.MultMatrix(ref mat.Matrix);

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
                        if (destTag == null)
                            continue;
                        GL.PushMatrix();
                        Matrix4 m = destTag.Frames[0].Matrix;
                        if (thisFrame < destTag.Frames.Length)
                            m = destTag.Frames[thisFrame].Matrix;
                        GL.MultMatrix(ref m);
                        DrawCompoenent(c);
                        GL.PopMatrix();
                    }
                    GL.PopMatrix();
                }
            }
        }

        protected void DrawMesh ( Mesh mesh )
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

            Frame frame;
            if (thisFrame >= mesh.Frames.Length)
                frame = mesh.Frames[0];
            else
                frame = mesh.Frames[thisFrame];

            foreach(Triangle triangle in mesh.Triangles)
            {
                foreach (int index in triangle.Verts)
                {
                    GL.TexCoord2(mesh.UVs[index]);
                    GL.Normal3(frame.Verts[index].Normal);
                    GL.Vertex3(frame.Verts[index].Position);
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

        public bool SetSequence(string name)
        {
            sequence = character.FindSequence(name);

            return sequence != null;
        }

        public void IncremntFullFrame ( )
        {
            thisFrame++;
            if (thisFrame > character.RootNode.Part.Frames.Length)
                thisFrame = 0;
        }
    }
}
