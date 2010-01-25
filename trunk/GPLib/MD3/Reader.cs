using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

using OpenTK;
using Utilities.FileIO;

namespace MD3
{
    public class Reader
    {
        public static bool CCWWinding = true;

        public static Character Read(DirectoryInfo dir)
        {
            return Read(dir, true);
        }

        public static Character Read(DirectoryInfo dir, bool useLOD)
        {
            if (!dir.Exists)
                return null;

            Character character = new Character();

            List<List<Component>> LODs = new List<List<Component>>();
            foreach (FileInfo file in dir.GetFiles("*.md3"))
            {
                Component compnent = ReadComponent(file);
                if (compnent == null)
                    continue;

                int LOD = 0;

                string componentName = string.Empty;

                if (file.Name.Contains("_"))
                {
                    string[] nugs = Path.GetFileNameWithoutExtension(file.Name).Split("_".ToCharArray());
                    if (nugs.Length > 0 && char.IsNumber(nugs[nugs.Length - 1][0]))
                        LOD = int.Parse(nugs[nugs.Length - 1]);

                    componentName = nugs[0].ToLower();
                }
                else
                    componentName = Path.GetFileNameWithoutExtension(file.Name).ToLower();

                if (componentName == "lower")
                    compnent.PartType = ComponentType.Legs;
                else if (componentName == "upper")
                    compnent.PartType = ComponentType.Torso;
                else if (componentName == "head")
                    compnent.PartType = ComponentType.Head;
                else
                    compnent.PartType = ComponentType.Other;

                if (!useLOD && LOD > 0)
                    continue;

                if (LOD+1 > LODs.Count)
                {
                    for (int i = LODs.Count; i < LOD + 1; i++)
                        LODs.Add(new List<Component>());
                }
                LODs[LOD].Add(compnent);
            }
            if (LODs.Count == 0)
                return null;
 
            character.Componenets = LODs[0].ToArray();

            character.LODs = new LODLevel[LODs.Count];

            int j = 0;
            foreach(List<Component> lod in LODs)
            {
                character.LODs[j] = new LODLevel();
                character.LODs[j].Componenets = lod.ToArray();
                j++;
            }

            foreach (FileInfo file in dir.GetFiles("*.cfg"))
                ReadAnimationConfig(character,file);

            foreach (FileInfo file in dir.GetFiles("*.skin"))
                ReadSkin(character, file);

            buildCharacterTree(character);

            return character;
        }

        internal static void linkTreeChildren ( Component parrent, ConnectedComponent node, Dictionary<string, List<Component>> componentsWithTags )
        {
           foreach (Tag tag in node.Part.Tags)
           {
               if (tag.Name != "tag_floor")
               {
                   if (componentsWithTags.ContainsKey(tag.Name))
                   {
                       if (!node.Children.ContainsKey(tag))
                           node.Children.Add(tag,new List<ConnectedComponent>());

                       List<Component> linkedComponents = componentsWithTags[tag.Name];
                       foreach (Component c in linkedComponents)
                       {
                           if (c == node.Part || c == parrent)
                               continue;

                           ConnectedComponent child = new ConnectedComponent();
                           child.Part = c;
                           node.Children[tag].Add(child);
                           linkTreeChildren(node.Part,child, componentsWithTags);
                       }
                   }
               }
           }
        }

        protected static void buildCharacterTree(Character character)
        {
            Component root = null;
            Dictionary<string, List<Component>> componentsWithTags = new Dictionary<string, List<Component>>();

            foreach(Component part in character.Componenets)
            {
                if (part.FileName == "lower")
                    root = part;

                foreach (Tag tag in part.Tags)
                {
                    if (!componentsWithTags.ContainsKey(tag.Name))
                        componentsWithTags.Add(tag.Name, new List<Component>());

                    componentsWithTags[tag.Name].Add(part);
                }
            }

            if (root == null)
                return;

            character.RootNode = new ConnectedComponent();
            character.RootNode.Part = root;

            linkTreeChildren(null,character.RootNode, componentsWithTags);
        }

        public static void ReadSkin(Character character, FileInfo file)
        {
            if (!file.Exists)
                return;
            
            string name = string.Empty;

            string component = string.Empty;

            string[] nugs = Path.GetFileNameWithoutExtension(file.Name).Split("_".ToCharArray(), 2);
            if (nugs.Length > 0 )
                component = nugs[0].ToLower();
            
            if (nugs.Length >1)
                name = nugs[1].ToLower();

            Skin skin = character.GetSkin(name);
            FileStream fs = file.OpenRead();
            StreamReader sr = new StreamReader(fs);


            if (!skin.Surfaces.ContainsKey(component))
                skin.Surfaces.Add(component, new Dictionary<string, string>());

            Dictionary<string, string> skinMeshes = skin.Surfaces[component];

            string line = sr.ReadLine();
            while (line != null)
            {
                if (line.Length > 0)
                {
                    nugs = line.Split(",".ToCharArray(),2);
                    if (nugs.Length > 1)
                    {
                        if (skinMeshes.ContainsKey(nugs[0].ToLower()))
                            skinMeshes[nugs[0].ToLower()] = nugs[1].ToLower();
                        else
                            skinMeshes.Add(nugs[0].ToLower(), nugs[1].ToLower());
                    }
                }
                line = sr.ReadLine();
            }

            sr.Close();
            fs.Close();
        }

        public static void ReadAnimationConfig ( Character character, FileInfo file )
        {
            if (!file.Exists)
                return;

            FileStream fs = file.OpenRead();
            StreamReader sr = new StreamReader(fs);

            string line = sr.ReadLine();

            if (character.Sequences == null)
                character.Sequences = new Dictionary<string, List<AnimationSequence>>();

            int count = 0;

            int torsoStart = -1;
            int legsOffset = -1;

            while (line != null)
            {
                string[] nugs = line.Split(" \t".ToCharArray());
                if (nugs.Length > 0)
                {
                    string tag = nugs[0].ToLower();
                    if (tag == "sex" && nugs.Length > 1)
                    {
                        string gender = nugs[1].ToLower();
                        if (gender == "f")
                            character.Gender = Gender.Female;
                        else if (gender == "m")
                            character.Gender = Gender.Male;
                        else
                            character.Gender = Gender.Unknown;
                    }
                    else if (tag == "headoffset" && nugs.Length > 3)
                        character.HeadOffset = new Vector3(float.Parse(nugs[1]), float.Parse(nugs[2]), float.Parse(nugs[3]));
                    else if (tag == "footsteps" && nugs.Length > 1)
                        character.Footsetps = nugs[1];
                    else if (tag.Length > 0 && char.IsNumber(tag[0]) && nugs.Length > 3)
                    {
                        AnimationSequence seq = new AnimationSequence();
                        seq.StartFrame = int.Parse(tag);
                        int length = int.Parse(nugs[1]);

                        seq.EndFrame = seq.StartFrame + length - 1;

                        int loop = int.Parse(nugs[2]);
                        if (loop == 0 || length == 1)
                            seq.LoopPoint = seq.EndFrame;
                        else
                            seq.LoopPoint = seq.EndFrame - (loop-1);

                        float fps = float.Parse(nugs[3]);
                        if (fps > 0)
                            seq.FPS = fps;

                        string name = string.Empty;
                        string part = "both";
                        if (nugs.Length > 6)
                            name = nugs[6];

                        if (name != string.Empty)
                        {
                            string[] seqParts = name.Split("_".ToCharArray());
                            if (seqParts.Length > 1)
                            {
                                part = seqParts[0].ToLower();
                                name = seqParts[1].ToLower();
                            }
                            else
                               name = seqParts[0];
                        }
                        else
                        {
                            if (count < 6)
                            {
                                part = "both";
                                switch (count)
                                {
                                    case 0:
                                        name = "death1";
                                        break;
                                    case 1:
                                        name = "dead1";
                                        break;
                                    case 2:
                                        name = "death2";
                                        break;
                                    case 3:
                                        name = "dead2";
                                        break;
                                    case 4:
                                        name = "death3";
                                        break;
                                    case 5:
                                        name = "dead3";
                                        break; 
                                }
                            }
                            else if (count < 13)
                            {
                                part = "torso";
                                switch (count)
                                {
                                    case 6:
                                        name = "gesture";
                                        break;
                                    case 7:
                                        name = "attack";
                                        break;
                                    case 8:
                                        name = "attak2";
                                        break;
                                    case 9:
                                        name = "drop";
                                        break;
                                    case 10:
                                        name = "raise";
                                        break;
                                    case 11:
                                        name = "stand";
                                        break;
                                    case 12:
                                        name = "stand2";
                                        break;
                                }
                            }
                            else
                            {
                                part = "legs";

                                switch (count)
                                {
                                    case 13:
                                        name = "walkcr";
                                        break;
                                    case 14:
                                        name = "walk";
                                        break;
                                    case 15:
                                        name = "run";
                                        break;
                                    case 16:
                                        name = "back";
                                        break;
                                    case 17:
                                        name = "swim";
                                        break;
                                    case 18:
                                        name = "jump";
                                        break;
                                    case 19:
                                        name = "land";
                                        break;
                                    case 20:
                                        name = "jumpb";
                                        break;
                                    case 21:
                                        name = "landb";
                                        break;
                                    case 22:
                                        name = "idle";
                                        break;
                                    case 23:
                                        name = "idlecr";
                                        break;
                                    case 24:
                                        name = "turn";
                                        break;
                                }
                            }
                        }

                        if (torsoStart < 0 && part == "torso")
                            torsoStart = seq.StartFrame;

                        if (legsOffset < 0 && part == "legs")
                        {
                            legsOffset = seq.StartFrame - torsoStart;
                        }

                        if (part == "legs")
                        {
                            seq.StartFrame -= legsOffset;
                            seq.LoopPoint -= legsOffset;
                            seq.EndFrame -= legsOffset;
                        }

                        seq.Name = name;
                        if (part == "both")
                        {
                            if (!character.Sequences.ContainsKey("torso"))
                                character.Sequences.Add("torso", new List<AnimationSequence>());
                            if (!character.Sequences.ContainsKey("legs"))
                                character.Sequences.Add("legs", new List<AnimationSequence>());

                            character.Sequences["torso"].Add(seq);
                            character.Sequences["legs"].Add(seq);
                        }
                        else
                        {
                            if (!character.Sequences.ContainsKey(part))
                                character.Sequences.Add(part, new List<AnimationSequence>());

                            character.Sequences[part].Add(seq);
                        }
                        count++;
                    }
                }
                line = sr.ReadLine();
            }

            sr.Close();
            fs.Close();
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MD3Frame
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] MinBounds; //First corner of the bounding box.
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] MaxBounds; //Second corner of the bounding box.
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] LocalOrigin; //	Local origin, usually (0, 0, 0).
            [MarshalAs(UnmanagedType.R4)]
            public float Radius; //Radius of bounding sphere.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] Name; //Name of Frame. ASCII character string, NUL-terminated (C-style)
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MD3SurfaceHeader
        {
            public UInt32 Ident; // Magic number. As a string of 4 octets, reads "IDP3"; as unsigned little-endian 860898377 (0x33504449);
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] Name; //Name of Tag object. ASCII character string, NUL-terminated (C-style)
            public UInt32 Flags; // ???

            public Int32 FrameCount; // Number of animation frames. This should match NUM_FRAMES in the MD3 header.
            public Int32 ShaderCount; // Number of Shader objects defined in this Surface, with a limit of MD3_MAX_SHADERS. Current value of MD3_MAX_SHADERS is 256.
            public Int32 VertCount; // Number of Vertex objects defined in this Surface, up to MD3_MAX_VERTS. Current value of MD3_MAX_VERTS is 4096.
            public Int32 TriangleCount; // Number of Triangle objects defined in this Surface, maximum of MD3_MAX_TRIANGLES. Current value of MD3_MAX_TRIANGLES is 8192.

            public Int32 TriangleOffset; // Relative offset from SURFACE_START where the list of Triangle objects starts.
            public Int32 ShaderOffset; // Relative offset from SURFACE_START where the list of Shader objects starts.
            public Int32 UVOffset; // Relative offset from SURFACE_START where the list of ST objects (s-t texture coordinates) starts.
            public Int32 VertOffset; // Relative offset from SURFACE_START where the list of Vertex objects (X-Y-Z-N vertices) starts.
            public Int32 EndOffset; // Relative offset from SURFACE_START to where the Surface object ends.
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MD3Tag
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] Name; //Name of Tag object. ASCII character string, NUL-terminated (C-style)
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] Origin; //Coordinates of Tag object.
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] R1; //   3x3 rotation matrix row associated with the Tag.
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] R2; //	3x3 rotation matrix row associated with the Tag.
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
            public float[] R3; //	3x3 rotation matrix row associated with the Tag.
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MD3Shader
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] Name; //Name of Tag object. ASCII character string, NUL-terminated (C-style)
            public Int32 ShaderInex; //Shader index number. No idea how this is allocated, but presumably in sequential order of definition.
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MD3Triangle
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] Indexes; //List of offset values into the list of Vertex objects that constitute the corners of the Triangle object. Vertex numbers are used instead of actual coordinates, as the coordinates are implicit in the Vertex object. The triangles have clockwise winding.
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MD3TexCoord
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 2)]
            public float[] UV; //s and t texture coordinates, normalized to the range [0, 1]. Values outside the range indicate wraparounds/repeats. Unlike UV coordinates, the origin for texture coordinates is located in the upper left corner (similar to the coordinate system used for computer screens) whereas, in UV mapping, it is placed in the lower left corner. As such, the t value must be flipped to correspond with UV coordinates. See also Left-hand coordinates
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        public struct MD3Vertex
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int16[] Coordinate; //List of offset values into the list of Vertex objects that constitute the corners of the Triangle object. Vertex numbers are used instead of actual coordinates, as the coordinates are implicit in the Vertex object. The triangles have clockwise winding.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] Normal; //Zenith and azimuth angles of normal vector. 255 corresponds to 2 pi. See spherical coordinates.
        }

        public class MD3Surface
        {
            public MD3SurfaceHeader Header;
            public List<MD3Shader> Shaders = new List<MD3Shader>();
            public List<MD3Triangle> Triangles = new List<MD3Triangle>();
            public List<MD3TexCoord> UVs = new List<MD3TexCoord>();
            public List<List<MD3Vertex>> Frames = new List<List<MD3Vertex>>();
        }

        protected static bool runStreamToOffset ( Stream fs, int offset )
        {
            if (fs.Position == (Int64)offset)
                return true;

            if (offset > fs.Length)
                return false;

            fs.Seek(offset, SeekOrigin.Begin);
            return true;
        }

        protected static Vector3 convertNormal ( MD3Vertex vert )
        {
            Vector3 norm = new Vector3();
            float lat = vert.Normal[0] * (2f*(float)Math.PI)/255f;
            float lng = vert.Normal[1] * (2f * (float)Math.PI) / 255f;
            norm.X = (float)Math.Cos(lat) * (float)Math.Sin(lng);
            norm.Y = (float)Math.Sin(lat) * (float)Math.Sin(lng);
            norm.Z = (float)Math.Cos(lng);

            norm.Normalize();
            return norm;
        }

        protected static Vector3 convertCoordinate(MD3Vertex vert)
        {
            float factor = 1f/64f;

            return new Vector3(vert.Coordinate[0] * factor, vert.Coordinate[1] * factor, vert.Coordinate[2] * factor);
        }

        protected static Vector3 convertVectorF( float[] vec )
        {
            Vector3 v = new Vector3(0,0,0);
            if (vec.Length > 0)
                v.X = vec[0];
            if (vec.Length > 1)
                v.Y = vec[1];
            if (vec.Length > 2)
                v.Z = vec[2];
            return v;
        }

        public static Component ReadComponent(FileInfo file)
        {
            if (!file.Exists)
                return null;

            FileStream fs = file.OpenRead();

            try
            {
                UInt32 magic = (UInt32)BinUtils.ReadObject(fs, typeof(UInt32));

                if (magic != 860898377)
                    return null;

                Int32 MD3Version = (Int32)BinUtils.ReadObject(fs, typeof(Int32));
                Component model = new Component(BinUtils.ReadString(fs, 64));

                string filename = Path.GetFileNameWithoutExtension(file.FullName);
                if (filename.Contains("_"))
                    filename = filename.Split("_".ToCharArray())[0];

                model.FileName = filename.ToLower();

                UInt32 flags = (UInt32)BinUtils.ReadObject(fs, typeof(UInt32));

                Int32 frameCount = (Int32)BinUtils.ReadObject(fs, typeof(Int32));
                Int32 tagCount = (Int32)BinUtils.ReadObject(fs, typeof(Int32));
                Int32 surfaceCount = (Int32)BinUtils.ReadObject(fs, typeof(Int32));
                Int32 skinCount = (Int32)BinUtils.ReadObject(fs, typeof(Int32));

                Int32 frameOffset = (Int32)BinUtils.ReadObject(fs, typeof(Int32));
                Int32 tagOffset = (Int32)BinUtils.ReadObject(fs, typeof(Int32));
                Int32 surfaceOffset = (Int32)BinUtils.ReadObject(fs, typeof(Int32));
                Int32 EOFOffset = (Int32)BinUtils.ReadObject(fs, typeof(Int32));

                if (!runStreamToOffset(fs, frameOffset))
                {
                    fs.Close();
                    return null;
                }

                List<MD3Frame> frames = new List<MD3Frame>();
                for (int i = 0; i < frameCount; i++ )
                    frames.Add((MD3Frame)BinUtils.ReadObject(fs, typeof(MD3Frame)));

                if (!runStreamToOffset(fs, tagOffset))
                {
                    fs.Close();
                    return null;
                }

                List<List<MD3Tag>> tagFrames = new List<List<MD3Tag>>();
                for (int i = 0; i < tagCount; i++)
                    tagFrames.Add(new List<MD3Tag>());

                for (int f = 0; f < frameCount; f++)
                {
                    for ( int i = 0; i < tagCount; i++)
                        tagFrames[i].Add((MD3Tag)BinUtils.ReadObject(fs, typeof(MD3Tag)));
                }

                if (!runStreamToOffset(fs, surfaceOffset))
                {
                    fs.Close();
                    return null;
                }

                List<MD3Surface> Surfaces = new List<MD3Surface>();
                for (int i = 0; i < surfaceCount; i++)
                {
                    MD3Surface surface = new MD3Surface();

                    Int64 pos = fs.Position;

                    surface.Header = (MD3SurfaceHeader)BinUtils.ReadObject(fs, typeof(MD3SurfaceHeader));

                    if (!runStreamToOffset(fs, (Int32)pos + surface.Header.ShaderOffset))
                    {
                        fs.Close();
                        return null;
                    }

                    for (int s = 0; s < surface.Header.ShaderCount; s++ )
                        surface.Shaders.Add((MD3Shader)BinUtils.ReadObject(fs, typeof(MD3Shader)));

                    if (!runStreamToOffset(fs, (Int32)pos + surface.Header.TriangleOffset))
                    {
                        fs.Close();
                        return null;
                    }

                    for (int t = 0; t < surface.Header.TriangleCount; t++)
                        surface.Triangles.Add((MD3Triangle)BinUtils.ReadObject(fs, typeof(MD3Triangle)));

                    if (!runStreamToOffset(fs, (Int32)pos + surface.Header.UVOffset))
                    {
                        fs.Close();
                        return null;
                    }

                    for (int u = 0; u < surface.Header.VertCount; u++)
                        surface.UVs.Add((MD3TexCoord)BinUtils.ReadObject(fs, typeof(MD3TexCoord)));

                    if (!runStreamToOffset(fs, (Int32)pos + surface.Header.VertOffset))
                    {
                        fs.Close();
                        return null;
                    }

                    for (int f = 0; f < surface.Header.FrameCount; f++)
                    {
                        List<MD3Vertex> frameVerts = new List<MD3Vertex>();
                        surface.Frames.Add(frameVerts);

                        for (int v = 0; v < surface.Header.VertCount; v++)
                            frameVerts.Add((MD3Vertex)BinUtils.ReadObject(fs, typeof(MD3Vertex)));
                    }
                    Surfaces.Add(surface);
                }
                fs.Close();

                List<FrameInfo> modelFrames = new List<FrameInfo>();
                foreach(MD3Frame frame in frames)
                {
                    FrameInfo info = new FrameInfo();
                    info.Name = BinUtils.FixString(frame.Name).ToLower();
                    info.Max = convertVectorF(frame.MaxBounds);
                    info.Min = convertVectorF(frame.MinBounds);
                    info.Origin = convertVectorF(frame.LocalOrigin);
                    info.Radius = frame.Radius;
                    modelFrames.Add(info);
                }
                model.Frames = modelFrames.ToArray();

                List<Tag> modelTags = new List<Tag>();
                foreach (List<MD3Tag> tagList in tagFrames)
                {
                    if (tagList.Count > 0)
                    {
                        Tag tag = new Tag();
                        tag.Name = BinUtils.FixString(tagList[0].Name).ToLower();
                        List<FrameMatrix> frameMats = new List<FrameMatrix>();
                        foreach (MD3Tag t in tagList)
                        {
                            FrameMatrix mat = new FrameMatrix();
                            mat.Matrix = new Matrix4(t.R1[0], t.R1[1], t.R1[2], 0, t.R2[0], t.R2[1], t.R2[2], 0, t.R3[0], t.R3[1], t.R3[2], 0, t.Origin[0], t.Origin[1], t.Origin[2], 1.0f);
                            mat.Inverse = Matrix4.Invert(mat.Matrix);
                            frameMats.Add(mat);
                        }
                        tag.Frames = frameMats.ToArray();
                        modelTags.Add(tag);
                    }
                }
                model.Tags = modelTags.ToArray();

                List<Mesh> meshes = new List<Mesh>();
                foreach (MD3Surface surface in Surfaces)
                {
                    Mesh mesh = new Mesh();
                    mesh.Name = BinUtils.FixString(surface.Header.Name).ToLower();

                    List<Triangle> tris = new List<Triangle>();
                    foreach (MD3Triangle triangle in surface.Triangles)
                    {
                        Triangle tri = new Triangle();
                        tri.Verts = triangle.Indexes;
                        if (CCWWinding)
                            Array.Reverse(tri.Verts);
                        tris.Add(tri);
                    }

                    mesh.Triangles = tris.ToArray();

                    List<Frame> fList = new List<Frame>();
                    foreach (List<MD3Vertex> frameVerts in surface.Frames)
                    {
                        Frame frame = new Frame();
                        List<Vertex> vList = new List<Vertex>();
                        foreach (MD3Vertex v in frameVerts)
                        {
                            Vertex vert = new Vertex();
                            vert.Position = convertCoordinate(v);
                            vert.Normal = convertNormal(v);
                            vList.Add(vert);
                        }
                        frame.Verts = vList.ToArray();
                        fList.Add(frame);
                    }
                    mesh.Frames = fList.ToArray();

                    List<Vector2> uvs = new List<Vector2>();
                    foreach(MD3TexCoord coord in surface.UVs)
                        uvs.Add(new Vector2(coord.UV[0], coord.UV[1]));

                    mesh.UVs = uvs.ToArray();

                    List<String> shaders = new List<String>();
                    foreach(MD3Shader shader in surface.Shaders)
                        shaders.Add(BinUtils.FixString(shader.Name));

                    mesh.ShaderFiles = shaders.ToArray();

                    meshes.Add(mesh);
                }
                model.Meshes = meshes.ToArray();

                return model;
            }
            catch (System.Exception ex)
            {
                fs.Close();
                return null;
            }
        }
    }
}
