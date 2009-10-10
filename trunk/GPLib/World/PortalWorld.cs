using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using OpenTK;
using Math3D;

namespace World
{
    public class CellVert
    {
        public Vector3 Bottom;
        public float Top;

        public float GetTopZ(bool incremental)
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

    public class CellID
    {
        public static CellID Empty = new CellID(string.Empty,string.Empty);

        public string CellName = string.Empty;
        public string GroupName = string.Empty;

        public CellID() { }
        public CellID ( string cell, string group )
        {
            CellName = cell;
            GroupName = group;
        }
    }

    public class CellMaterialInfo
    {
        public static CellMaterialInfo Empty = new CellMaterialInfo();

        public float Rotation = 0f;

        public string Material = string.Empty;

        public Vector2 UVScale = Vector2.One;
        public Vector2 UVShift = Vector2.Zero;

        public Vector2 GetFinalUV(float u, float v)
        {
            return new Vector2((u + UVShift.X) * UVScale.X, (v + UVShift.Y) * UVScale.Y);
        }
    }

    public class PortalDestination
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public Cell Cell = null;

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public CellGroup Group = null;

        public CellMaterialInfo Material = new CellMaterialInfo();

        public bool Visable = false;

        public CellID DestinationCell = new CellID();

        public PortalMapAttributes DestinationAttributes = new PortalMapAttributes();

        public PortalDestination() { }

        public PortalDestination(PortalDestination d)
        {
            DestinationCell.CellName = string.Copy(d.DestinationCell.CellName);
            DestinationCell.GroupName = string.Copy(d.DestinationCell.GroupName);
        }
    }

    public class CellWallGeometry
    {
        public bool Vizable = true;

        public float[] LowerZ = new float[2];
        public float[] UpperZ = new float[2];

        public CellMaterialInfo Material =  new CellMaterialInfo();

        public CellID Bottom = CellID.Empty;
        public CellID Top = CellID.Empty;
    }

    public class CellEdge
    {
        public bool Vizable = true;
        public int Start, End;
        public CellEdgeType EdgeType = CellEdgeType.Unknown;

        public List<PortalDestination> Destinations = new List<PortalDestination>();
        public List<CellWallGeometry> Geometry = new List<CellWallGeometry>();

        public Vector2 Normal = new Vector2();
        public Vector2 Slope = new Vector2();

        public List<PortalMapAttribute> EdgeAttributes = new List<PortalMapAttribute>();

        public CellEdge()
        {
            Start = -1;
            End = -1;
        }

        public CellEdge(CellEdge e)
        {
            Start = e.Start;
            End = e.End;
            EdgeType = e.EdgeType;
            foreach (PortalDestination dest in e.Destinations)
                Destinations.Add(new PortalDestination(dest));
            Normal = new Vector2(e.Normal);
        }
    }

    public class Cell
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public CellGroup Group = null;

        public CellID ID = new CellID(new Random().Next().ToString(), string.Empty);
            
        public List<CellVert> Verts = new List<CellVert>();
        public List<CellEdge> Edges = new List<CellEdge>();

        public Vector3 FloorNormal = new Vector3(0, 0, 1);
        public Vector3 RoofNormal = new Vector3(0, 0, -1);

        public bool HeightIsIncremental = true;

        public bool RoofVizable = true;
        public bool FloorVizable = true;

        public CellMaterialInfo FloorMaterial =  new CellMaterialInfo();
        public CellMaterialInfo RoofMaterial =  new CellMaterialInfo();

        public PortalMapAttributes CellAttributes = new PortalMapAttributes();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        BoundingBox Bounds = BoundingBox.Empty;
        [System.Xml.Serialization.XmlIgnoreAttribute]
        Plane FloorPlane = Plane.Empty;
        [System.Xml.Serialization.XmlIgnoreAttribute]
        Plane RoofPlane = Plane.Empty;
        
        public void Invaldate ()
        {
            Bounds = BoundingBox.Empty;
            FloorPlane = Plane.Empty;
            RoofPlane = Plane.Empty;
        }

        public override string ToString()
        {
            return ID.CellName;
        }

        public Cell()
        { }

        public Cell(Cell cell)
        {
            ID.CellName = cell.ID.CellName;
            ID.GroupName = cell.ID.GroupName;

            CellAttributes = cell.CellAttributes;

            Verts = cell.Verts;
            Edges = cell.Edges;
            HeightIsIncremental = cell.HeightIsIncremental;

            FloorNormal = cell.FloorNormal;
            RoofNormal = cell.RoofNormal;

            RoofVizable = cell.RoofVizable;
            FloorVizable = cell.FloorVizable;

            RoofMaterial = cell.RoofMaterial;
            FloorMaterial = cell.FloorMaterial;
        }

        public bool HasEdge(Vector2 e1, Vector2 e2)
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

        public int MatchingIndex(Vector3 inVert)
        {
            for (int i = 0; i < Verts.Count; i++)
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

        public float EdgeDistance(CellEdge edge)
        {
            CellVert sp = Verts[edge.Start];
            CellVert ep = Verts[edge.End];

            return (float)Math.Sqrt((ep.Bottom.X - sp.Bottom.X) * (ep.Bottom.X - sp.Bottom.X) + (ep.Bottom.Y - sp.Bottom.Y) * (ep.Bottom.Y - sp.Bottom.Y));
        }

        public float EdgeDistance(int edge)
        {
            return EdgeDistance(Edges[edge]);
        }

        public Vector3 FloorPoint(int index)
        {
            return Verts[index].Bottom;
        }

        public Vector3 RoofPoint(int index)
        {
            return new Vector3(Verts[index].Bottom.X, Verts[index].Bottom.Y, RoofZ(index));
        }

        public Vector3 RoofPoint(CellVert vert)
        {
            return new Vector3(vert.Bottom.X, vert.Bottom.Y, vert.GetTopZ(HeightIsIncremental));
        }

        public float RoofZ(int index)
        {
            return Verts[index].GetTopZ(HeightIsIncremental);
        }

        public float RoofZ(CellVert vert)
        {
            return vert.GetTopZ(HeightIsIncremental);
        }

        public static float GetZInPlane(Plane plane, float x, float y)
        {
            return (plane.D - plane.Normal.X * x - plane.Normal.Y * y) / plane.Normal.Z;
        }

        public Plane GetFloorPlane()
        {
            if (FloorPlane == Plane.Empty)
                FloorPlane = new Plane(FloorNormal, Vector3.Dot(FloorNormal, Verts[0].Bottom));
            return FloorPlane;
        }

        public Plane GetRoofPlane()
        {
            if (RoofPlane == Plane.Empty)
                RoofPlane = new Plane(RoofNormal, Vector3.Dot(RoofNormal, RoofPoint(0)));
            return RoofPlane;
        }

        public BoundingBox GetBoundingBox ()
        {
            if (Bounds == BoundingBox.Empty)
            {
                List<Vector3> verts = new List<Vector3>();

                foreach (CellVert vert in Verts)
                {
                    verts.Add(vert.Bottom);
                    verts.Add(RoofPoint(vert));
                }
                Bounds = BoundingBox.CreateFromPoints(verts);
            }
            return Bounds;
        }

        public bool PointIn ( Vector3 point )
        {
            if (GetBoundingBox().Contains(point) == ContainmentType.Disjoint)
                return false;

            // check the XY see if we are outside
            foreach ( CellEdge edge in Edges )
            {
                Vector2 v = new Vector2(Verts[edge.Start].Bottom.X - point.X, Verts[edge.Start].Bottom.Y - point.Y);
                float dot = Vector2.Dot(v, edge.Normal);
                if (dot > 0)
                    return false;
            }

            if (GetFloorPlane().Intersects(point) == PlaneIntersectionType.Back)
                return false;

            if (GetRoofPlane().IntersectsPoint(point) == PlaneIntersectionType.Back)
                return false;

            return true;
        }
    }

    public class CellGroup
    {
        public string Name = string.Empty;
        public List<Cell> Cells = new List<Cell>();
        public PortalMapAttributes GroupAttributes = new PortalMapAttributes();

        protected bool NameExists(string name)
        {
            foreach (Cell cell in Cells)
            {
                if (cell.ID.CellName == name)
                    return true;
            }
            return false;
        }

        public string NewCellName()
        {
            int count = Cells.Count;
            while (NameExists(count.ToString()))
                count++;

            return count.ToString();
        }

        public Cell HasEdge(Vector2 e1, Vector2 e2)
        {
            foreach (Cell cell in Cells)
            {
                if (cell.HasEdge(e1, e2))
                    return cell;
            }

            return null;
        }

        public Cell FindCell(string name)
        {
            foreach (Cell cell in Cells)
            {
                if (cell.ID.CellName == name)
                    return cell;
            }

            return null;
        }

        public Cell FindCell(CellID id)
        {
            foreach (Cell cell in Cells)
            {
                if (cell.ID.CellName == id.CellName)
                    return cell;
            }

            return null;
        }
    }

    public class PortalMapAttribute
    {
        public PortalMapAttribute() { }
        public PortalMapAttribute(string n, string v)
        {
            Name = n;
            Value = v;
        }

        public string Name = string.Empty;
        public string Value = string.Empty;
    }

    public class PortalMapAttributes
    {
        public List<PortalMapAttribute> AttributeList = new List<PortalMapAttribute>();

        public void Clear()
        {
            AttributeList.Clear();
        }
        public PortalMapAttribute[] Find(string name)
        {
            List<PortalMapAttribute> foundItems = new List<PortalMapAttribute>();

            foreach (PortalMapAttribute item in AttributeList)
            {
                if (item.Name == name)
                    foundItems.Add(item);
            }

            return foundItems.ToArray();
        }

        public void Add(string name, string value)
        {
            foreach (PortalMapAttribute item in AttributeList)
            {
                if (item.Name == name && item.Value == value)
                    return;
            }
            AttributeList.Add(new PortalMapAttribute(name, value));
        }

        public void Remove(string name)
        {
            List<PortalMapAttribute> foundItems = new List<PortalMapAttribute>();

            foreach (PortalMapAttribute item in AttributeList)
            {
                if (item.Name == name)
                    foundItems.Add(item);
            }

            foreach (PortalMapAttribute item in foundItems)
                AttributeList.Remove(item);
        }

        public void Remove(string name, string value)
        {
            List<PortalMapAttribute> foundItems = new List<PortalMapAttribute>();

            foreach (PortalMapAttribute item in AttributeList)
            {
                if (item.Name == name && item.Value == value)
                    foundItems.Add(item);
            }

            foreach (PortalMapAttribute item in foundItems)
                AttributeList.Remove(item);
        }
    }

    public class ObjectInstance
    {
        public String ObjectType = String.Empty;
        public String Name = string.Empty;
        public Vector3 Postion = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;

        public List<CellID> cells;
        public PortalMapAttributes ObjectAttributes = new PortalMapAttributes();
    }

    public class PortalWorld
    {
        public List<CellGroup> CellGroups = new List<CellGroup>();
        public PortalMapAttributes MapAttributes = new PortalMapAttributes();
        public List<ObjectInstance> MapObjects = new List<ObjectInstance>();

        public List<ObjectInstance> FindObjects ( string type )
        {
            List<ObjectInstance> objs = new List<ObjectInstance>();
            foreach (ObjectInstance obj in MapObjects)
            {
                if (obj.ObjectType == type)
                    objs.Add(obj);
            }

            return objs;
        }

        public List<ObjectInstance> FindObjects(string type, string name)
        {
            List<ObjectInstance> objs = new List<ObjectInstance>();
            foreach (ObjectInstance obj in MapObjects)
            {
                if (obj.ObjectType == type && obj.Name == name)
                    objs.Add(obj);
            }

            return objs;
        }

        public bool Valid()
        {
            foreach (CellGroup group in CellGroups)
            {
                if (group.Cells.Count > 0)
                    return true;
            }
            return false;
        }

        protected bool NameExists(string name)
        {
            foreach (CellGroup group in CellGroups)
            {
                if (group.Name == name)
                    return true;
            }
            return false;
        }

        public string NewGroupName()
        {
            int count = CellGroups.Count;
            while (NameExists(count.ToString()))
                count++;

            return count.ToString();
        }

        public void AddCell(Cell cell, CellGroup group)
        {
            cell.Group = group;
            cell.ID.GroupName = group.Name;
            group.Cells.Add(cell);
        }

        public List<Cell> CellsThatContainEdge(Vector2 p1, Vector2 p2)
        {
            return CellsThatContainEdge(p1, p2, null);
        }

        public List<Cell> CellsThatContainEdge(Vector2 p1, Vector2 p2, Cell ignoreCell)
        {
            List<Cell> foundCells = new List<Cell>();
            foreach (CellGroup group in CellGroups)
            {
                foreach (Cell cell in group.Cells)
                {
                    if (cell != ignoreCell)
                    {
                        if (cell.HasEdge(p1, p2))
                            foundCells.Add(cell);
                    }
                }
            }

            return foundCells;
        }

        public void RemoveCell(Cell cell)
        {
            CellGroup group = cell.Group;
            if (group == null)
                group = FindGroup(cell.ID);

            if (group != null)
                group.Cells.Remove(cell);
        }

        public CellGroup FindGroup(CellID id)
        {
            foreach (CellGroup group in CellGroups)
            {
                if (group.Name == id.GroupName)
                    return group;
            }
            return null;
        }

        public CellGroup FindGroup(string name)
        {
            foreach (CellGroup group in CellGroups)
            {
                if (group.Name == name)
                    return group;
            }
            return null;
        }

        public Cell FindCell ( CellID id )
        {
            return FindCell(id.CellName, id.GroupName);
        }

        public Cell FindCell(string name, string groupName)
        {
            CellGroup group = FindGroup(groupName);
            if (group != null)
                return group.FindCell(name);

            return null;
        }

        public List<CellID> FindCells ( Vector3 point )
        {
            List<CellID> cells = new List<CellID>();

            foreach (CellGroup group in CellGroups)
            {
                // link up all the portals
                foreach (Cell cell in group.Cells)
                {
                    if (cell.PointIn(point))
                        cells.Add(cell.ID);
                 }
            }
            return cells;
        }

        public void RebindCells()
        {
            foreach (CellGroup group in CellGroups)
            {
                // link up all the portals
                foreach (Cell cell in group.Cells)
                {
                    cell.Group = group;
                    cell.ID.GroupName = group.Name;

                    foreach (CellEdge edge in cell.Edges)
                    {
                        if (edge.EdgeType == CellEdgeType.Portal)
                        {
                            foreach (PortalDestination dest in edge.Destinations)
                            {
                                dest.Cell = FindCell(dest.DestinationCell);
                                dest.Group = dest.Cell.Group;
                            }
                        }
                    }
                }
            }
        }

        public static PortalWorld Read(FileInfo file)
        {
            if (!file.Exists)
                return null;

            XmlSerializer XML = new XmlSerializer(typeof(PortalWorld));

            PortalWorld map = null;
            try
            {
                map = (PortalWorld)XML.Deserialize(file.OpenRead());
            }
            catch (System.Exception ex)
            {
                GZipStream compressionStream = new GZipStream(file.OpenRead(), CompressionMode.Decompress);
                map = (PortalWorld)XML.Deserialize(compressionStream);
            }

            if (map != null)
                map.RebindCells();

            return map;
        }

        public bool Write(FileInfo file)
        {
            return Write(file, false);
        }

        public bool Write(FileInfo file, bool compress)
        {
            XmlSerializer XML = new XmlSerializer(typeof(PortalWorld));

            PortalWorld writeMap = new PortalWorld();
            writeMap.MapAttributes = MapAttributes;
            writeMap.MapObjects = MapObjects;

            foreach (CellGroup group in CellGroups)
            {
                CellGroup newGroup = new CellGroup();
                newGroup.Name = group.Name;
                writeMap.CellGroups.Add(newGroup);
                foreach (Cell cell in group.Cells)
                    newGroup.Cells.Add(new Cell(cell));
            }

            try
            {
                if (compress)
                    XML.Serialize(new GZipStream(file.OpenWrite(), CompressionMode.Compress), writeMap);
                else
                    XML.Serialize(file.OpenWrite(), writeMap);
            }
            catch (System.Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
