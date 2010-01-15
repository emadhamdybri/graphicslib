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
        public static Character Read(DirectoryInfo dir)
        {
            if (!dir.Exists)
                return null;

            Character character = new Character();

            List<Component> models = new List<Component>();
            foreach(FileInfo file in dir.GetFiles("*.md3"))
                models.Add(ReadComponent(file));

            character.Componenets = models.ToArray();
            return character;
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

                List<MD3Tag> tags = new List<MD3Tag>();
                for (int i = 0; i < tagCount; i++)
                    tags.Add((MD3Tag)BinUtils.ReadObject(fs, typeof(MD3Tag)));

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
                    info.Name = BinUtils.FixString(frame.Name);
                    info.Max = convertVectorF(frame.MaxBounds);
                    info.Min = convertVectorF(frame.MinBounds);
                    info.Origin = convertVectorF(frame.LocalOrigin);
                    info.Radius = frame.Radius;
                    modelFrames.Add(info);
                }
                model.Frames = modelFrames.ToArray();


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
