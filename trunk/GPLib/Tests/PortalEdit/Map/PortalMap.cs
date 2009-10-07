using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using OpenTK;

namespace PortalEdit
{
    public class PortalMapAttribute
    {
        public PortalMapAttribute (){}
        public PortalMapAttribute (string n, string v)
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

        public void Clear ()
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

        public PortalMapAttributes ObjectAttributes = new PortalMapAttributes();
    }

    public class PortalMap
    {
        public List<CellGroup> CellGroups = new List<CellGroup>();
        public PortalMapAttributes MapAttributes = new PortalMapAttributes();
        public List<ObjectInstance> MapObjects = new List<ObjectInstance>();

        public bool Valid ()
        {
            foreach(CellGroup group in CellGroups)
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

        public void AddCell ( Cell cell, CellGroup group )
        {
            cell.Group = group;
            cell.GroupName = group.Name;
            group.Cells.Add(cell);
        }

        public List<Cell> CellsThatContainEdge ( Vector2 p1, Vector2 p2 )
        {
            return CellsThatContainEdge(p1, p2, null);
        }

        public List<Cell> CellsThatContainEdge ( Vector2 p1, Vector2 p2, Cell ignoreCell )
        {
            List<Cell> foundCells = new List<Cell>();
            foreach(CellGroup group in CellGroups)
            {
                foreach(Cell cell in group.Cells)
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
            cell.tag = null;
            CellGroup group = cell.Group;
            if (group == null)
                group = FindGroup(cell.GroupName);

            if (group != null)
                group.Cells.Remove(cell);
        }

        public CellGroup FindGroup ( string name )
        {
            foreach (CellGroup group in CellGroups)
            {
                if (group.Name == name)
                    return group;
            }
            return null;
        }

        public Cell FindCell ( string name, string groupName )
        {
            CellGroup group = FindGroup(groupName);
            if (group != null)
                return group.FindCell(name);

            return null;
        }

        public void RebindCells ( )
        {
            foreach (CellGroup group in CellGroups)
            {
                // link up all the portals
                foreach (Cell cell in group.Cells)
                {
                    cell.Group = group;
                    cell.GroupName = group.Name;

                    foreach (CellEdge edge in cell.Edges)
                    {
                        if (edge.EdgeType == CellEdgeType.Portal)
                        {
                            foreach(PortalDestination dest in edge.Destinations)
                            {
                                dest.Group = FindGroup(dest.GroupName);
                                if (dest.Group != null)
                                    dest.Cell = dest.Group.FindCell(dest.CellName);
                                else
                                    dest.Cell = null;
                            }
                        }
                    }
                }
            }
        }

        public static PortalMap Read ( FileInfo file )
        {
            if (!file.Exists)
                return null;

            XmlSerializer XML = new XmlSerializer(typeof(PortalMap));

            PortalMap map = null;
            try
            {
                map = (PortalMap)XML.Deserialize(file.OpenRead());
            }
            catch (System.Exception ex)
            {
                GZipStream compressionStream = new GZipStream(file.OpenRead(),CompressionMode.Decompress);
                map = (PortalMap)XML.Deserialize(compressionStream);
            }

            if (map != null)
                map.RebindCells();
            
            return map;
        }

        public bool Write ( FileInfo file )
        {
            return Write(file, false);
        }

        public bool Write(FileInfo file, bool compress)
        {
            XmlSerializer XML = new XmlSerializer(typeof(PortalMap));

            PortalMap writeMap = new PortalMap();
            writeMap.MapAttributes = MapAttributes;

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
