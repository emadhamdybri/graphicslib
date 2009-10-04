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

            if (Top > Bottom.Z)
                return Top;

            return Bottom.Z;
        }

        public CellVert()
        {
            Bottom = new Vector3();
            Top = 10;
        }

        public CellVert(CellVert v)
        {
            Bottom = new Vector3(v.Bottom);
            Top = v.Top;
        }
    }

    public enum CellEdgeType
    {
        Unknown,
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

        public PortalMapAttributes DestinationAttributes = new PortalMapAttributes();

        public PortalDestination(){}

        public PortalDestination ( PortalDestination d )
        {
            CellName = String.Copy(d.CellName);
            GroupName = String.Copy(d.GroupName);
        }
    }

    public class CellWallGeometry
    {
        public bool Vizable = true;

        public float[] LowerZ = new float[2];
        public float[] UpperZ = new float[2];

        public String Material = String.Empty;

        public Vector2 UVScale = Vector2.One;
        public Vector2 UVShift = Vector2.Zero;

        public String BottomCell = string.Empty;
        public String BottomGroup = string.Empty;
        public String TopCell = string.Empty;
        public String TopGroup = string.Empty;
    }

    public class CellEdge
    {
        public bool Vizable = true;
        public int Start, End;
        public CellEdgeType EdgeType = CellEdgeType.Unknown;

        public List<PortalDestination> Destinations = new List<PortalDestination>();
        public List<CellWallGeometry> Geometry = new List<CellWallGeometry>();

        public Vector2 Normal = new Vector2();

        public List<PortalMapAttribute> EdgeAttributes = new List<PortalMapAttribute>();

        public CellEdge()
        {
            Start = -1;
            End = -1;
        }

        public CellEdge( CellEdge e)
        {
            Start = e.Start;
            End = e.End;
            EdgeType = e.EdgeType;
            foreach(PortalDestination dest in e.Destinations)
                Destinations.Add(new PortalDestination(dest));
            Normal = new Vector2(e.Normal);
        }
    }

    public class Cell
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public CellGroup Group = null;

        public String GroupName = string.Empty;

        public String Name = new Random().Next().ToString();
        public List<CellVert> Verts = new List<CellVert>();
        public List<CellEdge> Edges = new List<CellEdge>();

        public Vector3 FloorNormal = new Vector3(0, 0, 1);
        public Vector3 RoofNormal = new Vector3(0, 0, -1);

        public bool HeightIsIncremental = true;

        public bool RoofVizable = true;
        public bool FloorVizable = true;

        public PortalMapAttributes CellAttributes = new PortalMapAttributes();

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

        public CellVert MatchingVert(CellVert inVert)
        {
            return MatchingVert(inVert.Bottom);
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

        public Vector3 FloorPoint ( int index )
        {
            return Verts[index].Bottom;
        }

        public Vector3 RoofPoint(int index)
        {
            return new Vector3(Verts[index].Bottom.X, Verts[index].Bottom.Y, RoofZ(index));
        }

        public float RoofZ(int index)
        {
            return Verts[index].GetTopZ(HeightIsIncremental);
        }

        public static float GetZInPlane(Plane plane, float x, float y)
        {
            return (plane.D - plane.Normal.X * x - plane.Normal.Y * y) / plane.Normal.Z;
        }

        public Plane GetFloorPlane()
        {
            return new Plane(FloorNormal, Vector3.Dot(FloorNormal, Verts[0].Bottom));
        }

        public Plane GetRoofPlane()
        {
            return new Plane(RoofNormal, Vector3.Dot(RoofNormal, RoofPoint(0)));
        }

        public object tag;
    }

    public class CellGroup
    {
        public String Name = string.Empty;
        public List<Cell> Cells = new List<Cell>();
        public PortalMapAttributes GroupAttributes = new PortalMapAttributes();

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
