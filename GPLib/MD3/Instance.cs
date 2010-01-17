using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Drawables.Textures;

namespace MD3
{
    public class CharacterInstance
    {
        protected Character character;
        protected AnimationSequence sequence;

        protected Dictionary<Tag,Matrix4> TagMatrixOffsets = new Dictionary<Tag,Matrix4>();

        protected List<Mesh> HiddenMeshes = new List<Mesh>();

        protected Dictionary<Mesh, Texture> skinTextures;
        protected string skinName = string.Empty;

        int lastFrame = -1;
        float lastTime = -1;

        public CharacterInstance(Character c)
        {
            character = c;
        }

        public void BindSkin ()
        {
            skinTextures = new Dictionary<Mesh, Texture>();

            Skin skin;
            if (character.SkinExists(skinName))
                skin = character.GetSkin(skinName);
            else if (character.SkinExists("default"))
                skin = character.GetSkin("default");
            else if (character.SkinExists(string.Empty))
                skin = character.GetSkin(string.Empty);
            else
            {
                // skin from surfaces
            }

        }

        public bool Draw()
        {
            if (skinTextures == null)
                BindSkin();

            return false;
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
    }
}
