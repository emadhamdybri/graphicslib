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

    public class FrameMatrix
    {
        public Matrix4 Matrix = Matrix4.Identity;
        public Matrix4 Rotation = Matrix4.Identity;
        public Vector3 Position = Vector3.Zero;
    }

    public class Tag
    {
        public string Name = string.Empty;
        public FrameMatrix[] Frames;

        public static Tag Empty = new Tag();
    }

    public class Triangle
    {
        public int[] Verts;
    }

    public class Frame
    {
        public Vector3[] Positions;
        public Vector3[] Normals;
    }

    public class Mesh
    {
        public string Name = string.Empty;
        public string[] ShaderFiles;

        public Triangle[] Triangles;
        public Frame[] Frames;
        public Vector2[] UVs;
    }

    public enum ComponentType
    {
        Legs,
        Torso,
        Head,
        Other
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

        public ComponentType PartType = ComponentType.Other;

        public string FileName = string.Empty;

        public FrameInfo[] Frames;
        public Tag[] Tags;
        public Mesh[] Meshes;

        public Tag FindTag ( string name )
        {
            foreach (Tag tag in Tags)
            {
                if (tag.Name == name)
                    return tag;
            }

            return null;
        }

        public Tag SearchTag ( string name )
        {
            foreach (Tag tag in Tags)
            {
                if (tag.Name.ToLower().Contains(name.ToLower()))
                    return tag;
            }

            return null;
        }

        public Mesh FindMesh ( string name )
        {
            foreach (Mesh mesh in Meshes)
            {
                if (mesh.Name == name)
                    return mesh;
            }

            return null;
        }
    }
}
