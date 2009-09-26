using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
using Math3D;

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

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public Cell destination = null;

        public String destinationName = string.Empty;

        public Vector2 normal;
    }

    public class Cell
    {
        public String name = new Random().Next().ToString();
        public List<CellVert> verts = new List<CellVert>();
        public List<CellEdge> edges = new List<CellEdge>();

        public override string ToString()
        {
            return name;
        }

        public Cell()
        {}

        public Cell ( Cell cell )
        {
            name = cell.name;
            verts = cell.verts;
            edges = cell.edges;
        }

        public bool HasEdge ( Vector2 e1, Vector2 e2 )
        {
            foreach (CellEdge edge in edges)
            {
                Vector2 p1 = new Vector2(verts[edge.start].bottom.X, verts[edge.start].bottom.Y);
                Vector2 p2 = new Vector2(verts[edge.end].bottom.X, verts[edge.end].bottom.Y);

                if (VectorHelper2.Equal(e1, p1) && VectorHelper2.Equal(e2, p2))
                    return true;

                if (VectorHelper2.Equal(e1, p2) && VectorHelper2.Equal(e2, p1))
                    return true;
            }
            return false;
        }

        public object tag;
    }
}
