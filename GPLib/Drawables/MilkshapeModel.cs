/*
    Open Combat/Projekt 2501
    Copyright (C) 2010  Jeffery Allen Myers

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;

using OpenTK;

using Utilities.FileIO;

namespace MilkhapeModel
{
    /// <summary> 
    /// MS3D File header 
    /// </summary> 
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct MS3DHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public char[] ID;
        public int version;
    }

    /// <summary> 
    /// MS3D Vertex information 
    /// </summary> 
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct MS3DVertex
    {
        public byte flags;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] vertex;
        public char boneID;
        public byte refCount;
    }

    /// <summary> 
    /// MS3D Triangle information 
    /// </summary> 
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct MS3DTriangle
    {
        public short flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public short[] vertexIndices;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 9)]
        public float[] vertexNormals; //[3],[3] 
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] s;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] t;
        public byte smoothingGroup;
        public byte groupIndex;
    }

    /// <summary> 
    /// MS3D Material information 
    /// </summary> 
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct MS3DMaterial
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] name;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
        public float[] ambient;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
        public float[] diffuse;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
        public float[] specular;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)]
        public float[] emissive;
        [MarshalAs(UnmanagedType.R4)]
        public float shininess; // 0.0f - 128.0f 
        [MarshalAs(UnmanagedType.R4)]
        public float transparency; // 0.0f - 1.0f 
        public char mode; // 0, 1, 2 is unused now 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public char[] texture;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public char[] alphamap;
    }

    /// <summary> 
    /// MS3D Joint information 
    /// </summary> 
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct MS3DJoint
    {
        public byte flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public char[] parentName;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] rotation;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] translation;
        public short numRotationKeyframes;
        public short numTranslationKeyframes;
    }

    /// <summary> 
    /// MS3D Keyframe data 
    /// </summary> 
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct MS3DKeyframe
    {
        [MarshalAs(UnmanagedType.R4)]
        public float time;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] parameter;
    } 

    class MilkshapeVert
    {
        public int BoneID;
        public Vector3 Location;
    }

    class MilkshapeTriangle
    {
        public Vector3[] Normals = new Vector3[3];
        public Vector2[] UVs = new Vector2[3];
        public int[] Verts = new int[3];
    }

    class MilkshapeGroup
    {
        public String Name;
        public List<int> Triangles = new List<int>();
        public int MaterialIndex = -1;
    }

    class MilkshapeMaterial
    {
        public Color Ambient;
        public Color Diffuse;
        public Color Specular;
        public Color Emissive;
        public float Shine;
        public String TextureName;
    }

    class MilkshapeKeyframe
    {
        public int Joint = 0;
        public float time;
        public Vector3 Paramater = Vector3.Zero;
    }

    class MilkshapeJoint
    {
        public String Name = string.Empty;
        public String ParentName = string.Empty;

        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Translation = Vector3.Zero;

        public List<MilkshapeKeyframe> TranslationFrames = new List<MilkshapeKeyframe>();
        public List<MilkshapeKeyframe> RotationFrames = new List<MilkshapeKeyframe>();
    }

    class MilkshapeModel
    {
        public string ID = string.Empty;
        public string Vers = string.Empty;

        public List<MilkshapeVert> Verts = new List<MilkshapeVert>();
        public List<MilkshapeTriangle> Triangles = new List<MilkshapeTriangle>();
        public List<MilkshapeGroup> Groups = new List<MilkshapeGroup>();
        public List<MilkshapeMaterial> Materials = new List<MilkshapeMaterial>();
        public List<MilkshapeJoint> Joints = new List<MilkshapeJoint>();

        public float AnimationFPS = -1;

        public bool Read ( FileInfo file )
        {
            if (!file.Exists)
                return false;

            DirectoryInfo fileFolder = file.Directory;

            Stream fs = file.OpenRead();

            long len = fs.Length;
            MS3DHeader header = (MS3DHeader)BinUtils.ReadObject(fs, typeof(MS3DHeader));
                
            ID = new string(header.ID);
            Vers = header.version.ToString();

            int verts = BinUtils.GetNextStreamCount(fs);

            for (int i = 0; i < verts; i++ )
            {
                MS3DVertex Vertex = (MS3DVertex)BinUtils.ReadObject(fs, typeof(MS3DVertex));
                MilkshapeVert vert = new MilkshapeVert();
                vert.BoneID = Vertex.boneID;
                vert.Location = new Vector3(Vertex.vertex[0], Vertex.vertex[1], Vertex.vertex[2]);
                Verts.Add(vert);
            }

            int triangles = BinUtils.GetNextStreamCount(fs);

            for (int i = 0; i < triangles; i++)
            {
                MS3DTriangle Triangle = (MS3DTriangle)BinUtils.ReadObject(fs, typeof(MS3DTriangle));
                MilkshapeTriangle tri = new MilkshapeTriangle();

                bool normalsFirst = true;
                if (normalsFirst)
                {
                    tri.Normals[0] = new Vector3(Triangle.vertexNormals[0], Triangle.vertexNormals[1], Triangle.vertexNormals[2]);
                    tri.Normals[1] = new Vector3(Triangle.vertexNormals[3], Triangle.vertexNormals[4], Triangle.vertexNormals[5]);
                    tri.Normals[2] = new Vector3(Triangle.vertexNormals[6], Triangle.vertexNormals[7], Triangle.vertexNormals[8]);
                }
                else
                {
                    tri.Normals[0] = new Vector3(Triangle.vertexNormals[0], Triangle.vertexNormals[3], Triangle.vertexNormals[6]);
                    tri.Normals[1] = new Vector3(Triangle.vertexNormals[1], Triangle.vertexNormals[4], Triangle.vertexNormals[7]);
                    tri.Normals[2] = new Vector3(Triangle.vertexNormals[2], Triangle.vertexNormals[5], Triangle.vertexNormals[8]);
                }
              

                tri.UVs[0] = new Vector2(Triangle.s[0], Triangle.t[0]);
                tri.UVs[1] = new Vector2(Triangle.s[1], Triangle.t[1]);
                tri.UVs[2] = new Vector2(Triangle.s[2], Triangle.t[2]);

                tri.Verts[0] = Triangle.vertexIndices[0];
                tri.Verts[1] = Triangle.vertexIndices[1];
                tri.Verts[2] = Triangle.vertexIndices[2];

                Triangles.Add(tri);
            }

            int groups = BinUtils.GetNextStreamCount(fs);
            for (int i = 0; i < groups; i++)
            {
                MilkshapeGroup group = new MilkshapeGroup();

                BinUtils.ReadObject(fs, typeof(byte));
 
                byte[]b = new byte[32];
                fs.Read(b, 0, b.Length); // name
                group.Name = FixString(System.Text.Encoding.UTF8.GetString(b));

                short indexCount = (short)BinUtils.ReadObject(fs, typeof(short));

                for (int t = 0; t < indexCount; t++ )
                    group.Triangles.Add((short)BinUtils.ReadObject(fs, typeof(short)));

                group.MaterialIndex = (char)BinUtils.ReadObject(fs, typeof(char));
                if (group.MaterialIndex > 128)
                    group.MaterialIndex = -1;

                Groups.Add(group);
            }

            int materials = BinUtils.GetNextStreamCount(fs);
            for (int i = 0; i < materials; i++)
            {
                MilkshapeMaterial material = new MilkshapeMaterial();

                MS3DMaterial Mat = (MS3DMaterial)BinUtils.ReadObject(fs, typeof(MS3DMaterial));

                material.Ambient = ConvertColor(Mat.ambient);
                material.Diffuse = ConvertColor(Mat.diffuse);
                material.Specular = ConvertColor(Mat.specular);
                material.Emissive = ConvertColor(Mat.emissive);
                material.Shine = Mat.shininess;
                material.TextureName = FixString(new String(Mat.texture));

                Materials.Add(material);
            }

            AnimationFPS = (float)BinUtils.ReadObject(fs, typeof(float));
            float currentTime = (float)BinUtils.ReadObject(fs, typeof(float));
            int frames = (int)BinUtils.ReadObject(fs, typeof(int));

            int joints = BinUtils.GetNextStreamCount(fs);

            for (int i = 0; i < joints; i++)
            {
                MilkshapeJoint joint = new MilkshapeJoint();

                MS3DJoint Joint = (MS3DJoint)BinUtils.ReadObject(fs, typeof(MS3DJoint));

                joint.Name = FixString(new String(Joint.name));
                joint.ParentName = FixString(new String(Joint.parentName));
                joint.Translation = new Vector3 (Joint.translation[0],Joint.translation[1],Joint.translation[2]);
                joint.Rotation = new Vector3 (Joint.rotation[0],Joint.rotation[1],Joint.rotation[2]);

                for (int t = 0; t < Joint.numRotationKeyframes; t++ )
                {
                    MS3DKeyframe Frame = (MS3DKeyframe)BinUtils.ReadObject(fs, typeof(MS3DKeyframe));

                    MilkshapeKeyframe frame = new MilkshapeKeyframe();
                    frame.Paramater = new Vector3(Frame.parameter[0], Frame.parameter[1], Frame.parameter[2]);
                    frame.time = Frame.time;
                    joint.RotationFrames.Add(frame);
                }

                for (int t = 0; t < Joint.numTranslationKeyframes; t++ )
                {
                    MS3DKeyframe Frame = (MS3DKeyframe)BinUtils.ReadObject(fs, typeof(MS3DKeyframe));

                    MilkshapeKeyframe frame = new MilkshapeKeyframe();
                    frame.Paramater = new Vector3(Frame.parameter[0], Frame.parameter[1], Frame.parameter[2]);
                    frame.time = Frame.time;
                    joint.TranslationFrames.Add(frame);
                }

                Joints.Add(joint);
            }

            fs.Close();
            return true;
        }

        String FixString ( String input )
        {
            return input.TrimEnd("\0".ToCharArray());
        }

        Color ConvertColor ( float[] color)
        {
            return Color.FromArgb(255, (int)(255 * color[0]), (int)(255 * color[1]), (int)(255 * color[2]));
        }
    }
}
