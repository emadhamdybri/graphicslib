using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace MD3
{
    public enum Gender
    {
        Unknown,
        Female,
        Male,
    }

    public class AnimationSequence
    {
        public string Name = string.Empty;
        public int StartFrame = -1;
        public int EndFrame = -1;
        public int LoopPoint = -1;
        public float FPS = 30f;

        public override string ToString()
        {
            return Name;
        }
    }

    public class Skin
    {
        public string Name = string.Empty;
        public Dictionary<string,Dictionary<string, string>> Surfaces = new Dictionary<string,Dictionary<string, string>>();

        public Skin ( string n )
        {
            Name = n;
        }
    }

    internal class ConnectedComponent
    {
        internal Component Part;
        internal Dictionary<Tag, List<ConnectedComponent>> Children = new Dictionary<Tag, List<ConnectedComponent>>();
    }

    public class LODLevel
    {
        public Component[] Componenets = null;
    }

    public class Character
    {
        public Component[] Componenets = null;

        public LODLevel[] LODs = null;

        public String Name = string.Empty;

        public Gender Gender = Gender.Unknown;
        public Vector3 HeadOffset = Vector3.Zero;
        public string Footsetps = string.Empty;

        public Dictionary<string,List<AnimationSequence>> Sequences;

        public List<Skin> Skins = new List<Skin>();

        protected Dictionary<string, Dictionary<string,AnimationSequence>> SequenceCache;

        internal ConnectedComponent RootNode;

        public Skin GetSkin ( string name )
        {
            foreach (Skin skin in Skins)
            {
                if (skin.Name == name)
                    return skin;
            }

            Skin s = new Skin(name);
            Skins.Add(s);
            return s;
        }

        public bool SkinExists ( string name )
        {
            foreach (Skin skin in Skins)
            {
                if (skin.Name == name)
                    return true;
            }

            return false;
        }

        public Skin SkinFromSurfs ()
        {
            if (SkinExists("from_surfs"))
                return GetSkin("from_surfs");

            Skin skin = new Skin("from_surfs");

            foreach (Component component in Componenets)
            {
                skin.Surfaces.Add(component.Name, new Dictionary<string, string>());

                Dictionary<string,string> comps = skin.Surfaces[component.Name];

                foreach (Mesh mesh in component.Meshes)
                {
                    if (mesh.ShaderFiles.Length > 0)
                        comps.Add(mesh.Name, mesh.ShaderFiles[0]);
                    else
                        comps.Add(mesh.Name, mesh.Name + ".tga");
                }
            }

            Skins.Add(skin);
            return skin;
        }

        public Component FindComponent ( string name )
        {
            foreach ( Component comp in Componenets )
            {
                if (comp.FileName == name)
                    return comp;
            }

            return null;
        }

        public AnimationSequence FindSequence ( string part, string name )
        {
            if (SequenceCache == null)
            {
                SequenceCache = new Dictionary<string, Dictionary<string,AnimationSequence>>();
                foreach (KeyValuePair<string,List<AnimationSequence>> partSeq in Sequences)
                {
                    if (!SequenceCache.ContainsKey(partSeq.Key))
                        SequenceCache.Add(partSeq.Key, new Dictionary<string, AnimationSequence>());

                    Dictionary<string, AnimationSequence> seqs = SequenceCache[partSeq.Key];
                    foreach(AnimationSequence seq in partSeq.Value)
                    {
                        if (!seqs.ContainsKey(seq.Name))
                            seqs.Add(seq.Name, seq);
                    }
                }
            }

            if (!SequenceCache.ContainsKey(part))
                return null;

            if (!SequenceCache[part].ContainsKey(name))
                return null;

            return SequenceCache[part][name];
        }
    }
}
