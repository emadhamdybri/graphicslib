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

    }

    public class PortalCell
    {
        String name = String.Empty;

        public List<PortalFace> faces = new List<PortalFace>();
        public List<Portal> portals = new List<Portal>();
    }

    class PortalWorld
    {
    }
}
