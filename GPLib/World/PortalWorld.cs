using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Math;
using Math3D;

namespace World
{
    public class Portal
    {
        String destination = String.Empty;
    }

    public class PortalFace
    {
        public List<Vector3> verts;
        public List<Vector3> normals;
    }

    public class PortalCell
    {
        public BoundingBox overallBBox = new BoundingBox();

        public String name = String.Empty;

        public List<PortalFace> faces = new List<PortalFace>();
        public List<Portal> portals = new List<Portal>();
    }

    class PortalWorld
    {
    }
}
