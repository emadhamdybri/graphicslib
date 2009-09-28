using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
using Math3D;

namespace PortalEdit
{
    public class CellVert
    {
        public Vector3 Bottom;
        public float Top;

        public float GetTopZ ( bool incremental )
        {
            if (incremental)
                return Top + Bottom.Z;
            return Top;
        }
    }

    public enum CellEdgeType
    {
        Wall,
        Portal,
    }

    public class PortalDestination
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public Cell Cell = null;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public CellGroup Group = null;

        public String CellName = string.Empty;
        public String GroupName = string.Empty;
    }

    public class CellEdge
    {
        public int Start, End;
        public CellEdgeType EdgeType = CellEdgeType.Wall;

        public PortalDestination Destination = new PortalDestination();

        public Vector2 Normal;
    }

    public class Cell
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public CellGroup Group = null;

        public String GroupName = string.Empty;

        public String Name = new Random().Next().ToString();
        public List<CellVert> Verts = new List<CellVert>();
        public List<CellEdge> Edges = new List<CellEdge>();

        public bool HeightIsIncremental = true;

        public override string ToString()
        {
            return Name;
        }

        public Cell()
        {}

        public Cell ( Cell cell )
        {
            Name = cell.Name;
            Verts = cell.Verts;
            Edges = cell.Edges;
            HeightIsIncremental = cell.HeightIsIncremental;
        }

        public bool HasEdge ( Vector2 e1, Vector2 e2 )
        {
            foreach (CellEdge edge in Edges)
            {
                Vector2 p1 = new Vector2(Verts[edge.Start].Bottom.X, Verts[edge.Start].Bottom.Y);
                Vector2 p2 = new Vector2(Verts[edge.End].Bottom.X, Verts[edge.End].Bottom.Y);

                if (VectorHelper2.Equal(e1, p1) && VectorHelper2.Equal(e2, p2))
                    return true;

                if (VectorHelper2.Equal(e1, p2) && VectorHelper2.Equal(e2, p1))
                    return true;
            }
            return false;
        }

        public int MatchingIndex ( Vector3 inVert )
        {
            for ( int i = 0; i < Verts.Count; i++)
            {
                if (FloatHelper.Equals(inVert.X, Verts[i].Bottom.X) && FloatHelper.Equals(inVert.Y, Verts[i].Bottom.Y))
                    return i;
            }
            return -1;
        }

        public CellVert MatchingVert(Vector3 inVert)
        {
            for (int i = 0; i < Verts.Count; i++)
            {
                if (FloatHelper.Equals(inVert.X, Verts[i].Bottom.X) && FloatHelper.Equals(inVert.Y, Verts[i].Bottom.Y))
                    return Verts[i];
            }
            return null;
        }

        public object tag;
    }

    public class CellGroup
    {
        public String Name = string.Empty;
        public List<Cell> Cells = new List<Cell>();

        protected bool NameExists ( String name )
        {
            foreach (Cell cell in Cells)
            {
                if (cell.Name == name)
                    return true;
            }
            return false;
        }

        public String NewCellName ( )
        {
            int count = Cells.Count;
            while (NameExists(count.ToString()))
                count++;

            return count.ToString();
        }

        public Cell HasEdge ( Vector2 e1, Vector2 e2 )
        {
            foreach (Cell cell in Cells)
            {
                if (cell.HasEdge(e1, e2))
                    return cell;
            }

            return null;
        }

        public Cell FindCell ( String name )
        {
            foreach (Cell cell in Cells)
            {
                if (cell.Name == name)
                    return cell;
            }

            return null;
        }
    }
}
