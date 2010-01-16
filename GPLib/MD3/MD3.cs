﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace MD3
{
    public class FrameInfo
    {
        public string Name = string.Empty;
        public Vector3 Max = Vector3.Zero;
        public Vector3 Min = Vector3.Zero;
        public Vector3 Origin = Vector3.Zero;
        public float Radius = 0;
    }

    public class Tag
    {
        public string Name = string.Empty;
        public Matrix4[] Frames;
    }

    public class Triangle
    {
        public int[] Verts;
    }

    public class Vertex
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Normal = Vector3.Zero;
    }

    public class Frame
    {
        public Vertex[] Verts;
    }

    public class Mesh
    {
        public string Name = string.Empty;
        public string[] ShaderFiles;

        public Triangle[] Triangles;
        public Frame[] Frames;
        public Vector2[] UVs;
    }

    public class Component
    {
        public string Name
        {
            get { return name; }
        }
        string name = string.Empty;

        public Component ( string n )
        {
            name = n;
        }

        public FrameInfo[] Frames;
        public Tag[] Tags;
    }
}
