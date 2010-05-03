using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GameObjects
{
    public class Capital
    {
        public Point Location = Point.Empty;
        public RealmType Realm = RealmType.Unknown;
    }

    internal class MapConfig
    {
        public string Name = string.Empty;
        public string Base = string.Empty;
        public int Width = 0;
        public int Height = 0;
        public int Section = 0;

        public List<Capital> Capitals = new List<Capital>();
        public List<Point> Mines = new List<Point>();
        public int StaringGold = -1;
    }

    public class Map
    {
        public string Name = string.Empty;

        public Point Size = Point.Empty;

        public Image TerrainMap;
        public FileInfo[] ImageMaps;

        public Capital[] Capitals;
        public Point[] Mines;

        public int StaringGold = -1;

        public static Map Load(FileInfo file)
        {
            if (!file.Exists)
                return null;

            MapConfig cfg;

            XmlSerializer xml = new XmlSerializer(typeof(MapConfig));
            StreamReader sr = file.OpenText();
            try
            {
                cfg = (MapConfig)xml.Deserialize(sr);
            }
            catch (System.Exception ex)
            {
                return null;
            }

            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(file.FullName));

            Map map = new Map();

            map.Name = cfg.Name;
            map.
        }
    }
}
