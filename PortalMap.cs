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
    public class PortalMap
    {
        public List<CellGroup> cellGroups = new List<CellGroup>();

        protected bool NameExists(String name)
        {
            foreach (CellGroup group in cellGroups)
            {
                if (group.Name == name)
                    return true;
            }
            return false;
        }

        public String NewGroupName()
        {
            int count = cellGroups.Count;
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
            foreach(CellGroup group in cellGroups)
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

        public CellGroup FindGroup ( String name )
        {
            foreach (CellGroup group in cellGroups)
            {
                if (group.Name == name)
                    return group;
            }
            return null;
        }

        public Cell FindCell ( String name, String groupName )
        {
            CellGroup group = FindGroup(groupName);
            if (group != null)
                return group.FindCell(name);

            return null;
        }

        public void RebindCells ( )
        {
            foreach (CellGroup group in cellGroups)
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
                            edge.Destination.Group = FindGroup(edge.Destination.GroupName);
                            if (edge.Destination.Group != null)
                                edge.Destination.Cell = edge.Destination.Group.FindCell(edge.Destination.CellName);
                            else
                                edge.Destination.Cell = null;
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
            foreach (CellGroup group in cellGroups)
            {
                CellGroup newGroup = new CellGroup();
                newGroup.Name = group.Name;
                writeMap.cellGroups.Add(newGroup);
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
