using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Utilities.FileIO;

using OpenTK;
using OpenTK.Graphics;

namespace Cal3d
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CMFHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] magic;
        public Int32 version;
        public Int32 submeshes;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CMFSubMeshInfo
    {
        public Int32 material;
        public Int32 verts;
        public Int32 faces;
        public Int32 lodSteps;
        public Int32 springs;
        public Int32 maps;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CMFVertexInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] position;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] normal;
        public Int32 collapseID;
        public Int32 faceCollapseCount;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CMFMapInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 2)]
        public float[] uv;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CMFInfluenceInfo
    {
        public int boneID;
        [MarshalAs(UnmanagedType.R4)]
        public float weight;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CMFSpringInfo
    {
        public Int32 vert1;
        public Int32 vert2;
        [MarshalAs(UnmanagedType.R4)]
        public float coefficient;
        [MarshalAs(UnmanagedType.R4)]
        public float idleLength;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CMFFaceInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int32[] verts;
    }

    class Cal3dMeshInfluence
    {
        public int Bone;
        public float Weight;
    }

    class Cal3dMeshVert
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Normal = Vector3.Zero;
        public int CollapseVert = -1;
        public int CollapseFaceCount = -0;

        public List<Vector2> UVs = new List<Vector2>();
        public List<Cal3dMeshInfluence> Influences = new List<Cal3dMeshInfluence>();

        public float Weight = 0;
    }

    class Cal3dMeshSpring
    {
        public List<int> Anchors = new List<int>();
        public float Coefficient = 0;
        public float Lenght = 0;
    }

    class Cal3dMeshFace
    {
        public List<int> Verts = new List<int>();
    }

    class Cal3dMesh
    {
        public int Material = -1;
        public int LODSteps = 0;

        public List<Cal3dMeshVert> Verts = new List<Cal3dMeshVert>();
        public List<Cal3dMeshSpring> Springs = new List<Cal3dMeshSpring>();
        public List<Cal3dMeshFace> Faces = new List<Cal3dMeshFace>();
    }

    class CMFFile
    {
        public List<Cal3dMesh> Meshes = new List<Cal3dMesh>();

        public bool Read ( FileInfo file )
        {
            Meshes.Clear();

            FileStream fs = file.OpenRead();

            CMFHeader header = (CMFHeader)BinUtils.ReadObject(fs, typeof(CMFHeader));

            string magic = new string(header.magic);
            if (magic != "CMF\0")
            {
                fs.Close();
                return false;
            }

            // read each mesh
            for (int sm = 0; sm < header.submeshes; sm++)
            {
                CMFSubMeshInfo smInfo = (CMFSubMeshInfo)BinUtils.ReadObject(fs, typeof(CMFSubMeshInfo));
                Cal3dMesh mesh = new Cal3dMesh();

                mesh.Material = smInfo.material;
                mesh.LODSteps = smInfo.lodSteps;

                //read each vertex
                for ( int v = 0; v < smInfo.verts; v++)
                {
                    Cal3dMeshVert vert = new Cal3dMeshVert();

                    CMFVertexInfo vertInfo = (CMFVertexInfo)BinUtils.ReadObject(fs, typeof(CMFVertexInfo));
                    vert.CollapseFaceCount = vertInfo.faceCollapseCount;
                    vert.CollapseVert = vertInfo.collapseID;
                    vert.Position = new Vector3(vertInfo.position[0], vertInfo.position[1], vertInfo.position[2]);
                    vert.Normal = new Vector3(vertInfo.normal[0], vertInfo.normal[1], vertInfo.normal[2]);

                    for (int m = 0; m < smInfo.maps; m++)
                    {
                        CMFMapInfo mapInfo = (CMFMapInfo)BinUtils.ReadObject(fs, typeof(CMFMapInfo));
                        vert.UVs.Add(new Vector2(mapInfo.uv[0], mapInfo.uv[1]));
                    }

                    int influences = (int)BinUtils.ReadObject(fs, typeof(Int32));
                    for ( int i = 0; i < influences; i++ )
                    {
                        CMFInfluenceInfo influenceInfo = (CMFInfluenceInfo)BinUtils.ReadObject(fs, typeof(CMFInfluenceInfo));
                        Cal3dMeshInfluence inf = new Cal3dMeshInfluence();
                        inf.Bone = influenceInfo.boneID;
                        inf.Weight = influenceInfo.weight;
                        vert.Influences.Add(inf);
                    }

                    if (smInfo.springs > 0)
                    {
                        vert.Weight = (float)BinUtils.ReadObject(fs, typeof(float));
                    }
                    mesh.Verts.Add(vert);
                }

                for (int s = 0; s < smInfo.springs; s++)
                {
                    CMFSpringInfo springInfo = (CMFSpringInfo)BinUtils.ReadObject(fs, typeof(CMFSpringInfo));
                    Cal3dMeshSpring spring = new Cal3dMeshSpring();
                    spring.Anchors.Add(springInfo.vert1);
                    spring.Anchors.Add(springInfo.vert2);
                    spring.Coefficient = springInfo.coefficient;
                    spring.Lenght = springInfo.idleLength;
                    mesh.Springs.Add(spring);
                }

                for (int f = 0; f < smInfo.faces; f++)
                {
                    CMFFaceInfo faceInfo = (CMFFaceInfo)BinUtils.ReadObject(fs, typeof(CMFFaceInfo));
                    Cal3dMeshFace face = new Cal3dMeshFace();
                    face.Verts.Add(faceInfo.verts[0]);
                    face.Verts.Add(faceInfo.verts[1]);
                    face.Verts.Add(faceInfo.verts[2]);
                    mesh.Faces.Add(face);
                }
                Meshes.Add(mesh);
            }
            fs.Close();
            return true;
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CSFHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] magic;
        public Int32 version;
        public Int32 bones;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CSFBoneInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] translation;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
        public float[] rotation;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] localTranslation;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
        public float[] localRotation;
        public Int32 parent;
        public Int32 children;
    }

    class Cal3dBone
    {
        public string Name = string.Empty;
        public Vector3 Translation = Vector3.Zero;
        public Quaternion Rotation = Quaternion.Identity;
        public Vector3 VertTranslation = Vector3.Zero;
        public Quaternion VertRotation = Quaternion.Identity;
        public int Parrent = -1;
        public List<int> Children = new List<int>();
    }

    class CSFFile
    {
        public List<Cal3dBone> Bones = new List<Cal3dBone>();

        public bool Read ( FileInfo file )
        {
            FileStream fs = file.OpenRead();
            Bones.Clear();

            CSFHeader header = (CSFHeader)BinUtils.ReadObject(fs, typeof(CSFHeader));

            string magic = new string(header.magic);
            if (magic != "CSF\0")
            {
                fs.Close();
                return false;
            }

            for (int b = 0; b < header.bones; b++)
            {
                Cal3dBone bone = new Cal3dBone();

                Int32 nameSize = (Int32)BinUtils.ReadObject(fs, typeof(Int32));
                byte[] name = new byte[nameSize];
                fs.Read(name, 0, name.Length);

                bone.Name = System.Text.Encoding.ASCII.GetString(name);
                CSFBoneInfo info = (CSFBoneInfo)BinUtils.ReadObject(fs, typeof(CSFBoneInfo));

                bone.Translation = new Vector3(info.translation[0], info.translation[1], info.translation[2]);
                bone.VertTranslation = new Vector3(info.localTranslation[0], info.localTranslation[1], info.localTranslation[2]);

                bone.Rotation = new Quaternion(info.rotation[0], info.rotation[1], info.rotation[2], info.rotation[3]);
                bone.VertRotation = new Quaternion(info.localRotation[0], info.localRotation[1], info.localRotation[2], info.localRotation[3]);
                bone.Parrent = info.parent;
                bone.Rotation.Normalize();
                bone.VertRotation.Normalize();

                for( int c = 0; c < info.children; c++)
                    bone.Children.Add((int)BinUtils.ReadObject(fs, typeof(Int32)));

                Bones.Add(bone);
            }

            fs.Close();
            return true;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CAFHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] magic;
        public Int32 version;
        [MarshalAs(UnmanagedType.R4)]
        public float durration;

        public Int32 tracks;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CAFTrackInfo
    {
        public Int32 bone;
        public Int32 keyframes;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CAFKeyframeInfo
    {
        [MarshalAs(UnmanagedType.R4)]
        public float time;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] translation;
        
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
        public float[] rotation;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CAFCompressedTrackInfo
    {
        public Int32 bone;
        public Int32 keyframes;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] minimum;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] scale;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    struct CAFCompressedKeyframeInfo
    {
        public UInt16 time;

        public UInt32 translation;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public UInt16[] rotation;
    }

    class Cal3dKeyframe
    {
        public float Time = -1;
        public Vector3 Translation = Vector3.Zero;
        public Quaternion Rotation = Quaternion.Identity;
    }

    class CAFFile
    {
        public Dictionary<int, List<Cal3dKeyframe>> Keyframes = new Dictionary<int, List<Cal3dKeyframe>>();

        public bool Read(FileInfo file)
        {
            FileStream fs = file.OpenRead();
            Keyframes.Clear();

            CAFHeader header = (CAFHeader)BinUtils.ReadObject(fs, typeof(CAFHeader));

            string magic = new string(header.magic);
            if (magic != "CAF\0")
            {
                fs.Close();
                return false;
            }

            bool compressed = false;
            if (header.version >= 1200)
            {
                Int32 flags = (Int32)BinUtils.ReadObject(fs, typeof(Int32));
                compressed = flags > 0;
            }

            if (compressed)
            {
                CAFCompressedTrackInfo trackInfo = (CAFCompressedTrackInfo)BinUtils.ReadObject(fs, typeof(CAFCompressedTrackInfo));

                if (!Keyframes.ContainsKey(trackInfo.bone))
                    Keyframes.Add(trackInfo.bone, new List<Cal3dKeyframe>());

                List<Cal3dKeyframe> boneFrames = Keyframes[trackInfo.bone];

                for (int k = 0; k < trackInfo.keyframes; k++)
                {
                    CAFCompressedKeyframeInfo keyframeInfo = (CAFCompressedKeyframeInfo)BinUtils.ReadObject(fs, typeof(CAFCompressedKeyframeInfo));
                    Cal3dKeyframe keyframe = new Cal3dKeyframe();

                    keyframe.Time = ((float)keyframeInfo.time / 65535f) * header.durration;
                    // TODO read in the compressed trans and rotation
                    boneFrames.Add(keyframe);
                }
            }
            else
            {
                for ( int t = 0; t < header.tracks; t++ )
                {
                    CAFTrackInfo trackInfo = (CAFTrackInfo)BinUtils.ReadObject(fs, typeof(CAFTrackInfo));

                    if (!Keyframes.ContainsKey(trackInfo.bone))
                        Keyframes.Add(trackInfo.bone, new List<Cal3dKeyframe>());

                    List<Cal3dKeyframe> boneFrames = Keyframes[trackInfo.bone];

                    for( int k = 0; k < trackInfo.keyframes; k++ )
                    {
                        CAFKeyframeInfo keyframeInfo = (CAFKeyframeInfo)BinUtils.ReadObject(fs, typeof(CAFKeyframeInfo));
                        Cal3dKeyframe keyframe = new Cal3dKeyframe();

                        keyframe.Time = keyframeInfo.time;
                        keyframe.Translation = new Vector3(keyframeInfo.translation[0], keyframeInfo.translation[1], keyframeInfo.translation[2]);
                        keyframe.Rotation = new Quaternion(keyframeInfo.rotation[0], keyframeInfo.rotation[1], keyframeInfo.rotation[2], keyframeInfo.rotation[3]);
                        keyframe.Rotation.Normalize();

                        boneFrames.Add(keyframe);
                    }
                }
            }

            fs.Close();
            return true;

        }
    }
}
