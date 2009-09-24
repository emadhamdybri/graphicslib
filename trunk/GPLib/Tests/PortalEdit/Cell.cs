using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Math;

namespace PortalEdit
{
    public class CellVert
    {
        public Vector3 bottom;
        public float top;
    }

    public enum CellEdgeType
    {
        eWall,
        ePortal,
    }

    public class CellEdge
    {
        public int start, end;
        public CellEdgeType type = CellEdgeType.eWall;
    }

    public class Cell
    {
        public String name;
        public List<CellVert> verts = new List<CellVert>();
        public List<CellEdge> edges = new List<CellEdge>();

        public object tag;
    }
}
