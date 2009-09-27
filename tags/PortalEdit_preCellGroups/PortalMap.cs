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
        MapRenderer mapRenderer;
        public List<Cell> cells = new List<Cell>();

        public void AddCell ( Cell cell )
        {
            cells.Add(cell);
        }

        public List<Cell> CellsThatContainEdge ( Vector2 p1, Vector2 p2 )
        {
            return CellsThatContainEdge(p1, p2, null);
        }

        public List<Cell> CellsThatContainEdge ( Vector2 p1, Vector2 p2, Cell ignoreCell )
        {
            List<Cell> foundCells = new List<Cell>();
            foreach(Cell cell in cells)
            {
                if (cell != ignoreCell)
                {
                    if (cell.HasEdge(p1, p2))
                        foundCells.Add(cell);
                }
            }

            return foundCells;
        }

        public void RemoveCell(Cell cell)
        {
            cell.tag = null;
            cells.Remove(cell);
        }

        public Cell FindCell ( String name )
        {
            foreach( Cell cell in cells)
            {
                if (cell.name == name)
                    return cell;
            }

            return null;
        }

        public void RebindCells ( )
        {
            // link up all the portals
            foreach (Cell cell in cells)
            {
                foreach (CellEdge edge in cell.edges)
                {
                    if (edge.type == CellEdgeType.ePortal)
                        edge.destination = FindCell(edge.destinationName);
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
            foreach(Cell cell in cells)
                writeMap.cells.Add(new Cell(cell));

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
