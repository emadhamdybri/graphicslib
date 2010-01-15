using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace MD3
{
    public class FrameInfo
    {
        public String Name = string.Empty;
        public Vector3 Max = Vector3.Zero;
        public Vector3 Min = Vector3.Zero;
        public Vector3 Origin = Vector3.Zero;
        public float Radius = 0;
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

    }
}
