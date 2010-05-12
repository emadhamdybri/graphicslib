using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GameObjects
{
    public class CapitalDefinition
    {
        public Point Location = Point.Empty;
        public RealmType Realm = RealmType.Unknown;
    }

    public class Mine
    {
        public Point Location = Point.Empty;
        public int TotalGold = -1;
    }

    public class MapConfig
    {
        public string Name = string.Empty;
        public string Base = string.Empty;
        public int Width = 0;
        public int Height = 0;
        public int SectionsW = 0;
        public int SectionsH = 0;

        public List<CapitalDefinition> Capitals = new List<CapitalDefinition>();
        public List<Mine> Mines = new List<Mine>();
        public int StaringGold = -1;

        public static MapConfig Read(FileInfo file)
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

            return cfg;
        }

        public static bool Write ( MapConfig config,  FileInfo file )
        {
            FileStream fs = file.OpenWrite();
            if (fs == null)
                return false;

            XmlSerializer xml = new XmlSerializer(typeof(MapConfig));
            xml.Serialize(fs, config);
            fs.Close();
            return true;
        }

        public static MapConfig BuildFromDir(DirectoryInfo dir)
        {
            MapConfig cfg = new MapConfig();
            cfg.Name = dir.Name;

            int imageCountW = 0;
            int imageCountH = 0;

            while (true)
            {
                FileInfo file = new FileInfo(Path.Combine(dir.FullName, imageCountW.ToString() + "_0.png"));
                if (file.Exists)
                    imageCountW++;
                else
                {
                    imageCountW--;
                    break;
                }
            }

            while (true)
            {
                FileInfo file = new FileInfo(Path.Combine(dir.FullName, "0_" + imageCountH.ToString() + ".png"));
                if (file.Exists)
                    imageCountH++;
                else
                {
                    imageCountH--;
                    break;
                }
            }

            for ( int w = 0; w <= imageCountW; w++ )
            {
                Image img = Bitmap.FromFile(Path.Combine(dir.FullName, w.ToString() + "_0.png"));
                cfg.Width += img.Width;
            }

            for (int h = 0; h <= imageCountH; h++)
            {
                Image img = Bitmap.FromFile(Path.Combine(dir.FullName, "0_" + h.ToString() + ".png"));
                cfg.Height += img.Height;
            }

            cfg.SectionsH = imageCountH + 1;
            cfg.SectionsW = imageCountW + 1;

            cfg.Capitals.Add(new CapitalDefinition());
            cfg.Capitals.Add(new CapitalDefinition());

            cfg.Mines.Add(new Mine());

            return cfg;
        }
    }

    public class Map
    {
        public string Name = string.Empty;

        public Point Size = Point.Empty;

        public Image TerrainMap;
        public FileInfo[] ImageMaps;

        public Castle[] Capitals;
        public Mine[] Mines;

        public int StaringGold = -1;

        protected double TerrainScaleX = -1;
        protected double TerrainScaleY = -1;

        public int XImages = -1;
        public int YImages = -1;

        public static Color SeaZone
        {
            get { return Color.Blue; }
        }

        public static Color PlainsZone
        {
            get { return Color.Green; }
        }

        public static Color DesertZone
        {
            get { return Color.Yellow; }
        }

        public static Color MountainZone
        {
            get { return Color.Black; }
        }

        public static Color Forrest
        {
            get { return Color.Red; }
        }

        public static Color IceZone
        {
            get { return Color.Black; }
        }

        public static Color RiverZone
        {
            get { return Color.Cyan; }
        }

        public enum TerrainType
        {
            eUnpassable,
            eRiver,
            eSea,
            eNormal,
            eSand,
            eForrest,
            eMountains,
            eIce,
        }

        public TerrainType GetTerrain ( Point pos )
        {
            if (TerrainMap as Bitmap == null)
                return TerrainType.eUnpassable;

            // scale the position from world space to the terrain map
            Point imagePos = new Point((int)(pos.X * TerrainScaleX), (int)(pos.Y * TerrainScaleY));

            Color pix = (TerrainMap as Bitmap).GetPixel(imagePos.X, imagePos.Y);
            
            if (pix == Map.SeaZone)
                return TerrainType.eSea;
            else if (pix ==Map.RiverZone)
                return TerrainType.eRiver;
            else if (pix == Map.Forrest)
                return TerrainType.eForrest;
            else if (pix == Map.PlainsZone)
                return TerrainType.eNormal;
            else if (pix == Map.DesertZone)
                return TerrainType.eSand;
            else if (pix == Map.IceZone)
                return TerrainType.eIce;
            else if (pix == Map.MountainZone)
                return TerrainType.eMountains;

            return TerrainType.eUnpassable;
        }

        protected void Init ( )
        {
            if (TerrainMap != null)
            {
                TerrainScaleX = (double)TerrainMap.Width / (double)Size.X;
                TerrainScaleY = (double)TerrainMap.Height / (double)Size.Y;
            }
        }

        public static bool SaveInitalConfig ( DirectoryInfo dir )
        {
            if (!dir.Exists)
                return false;

            FileInfo file = new FileInfo(Path.Combine(dir.FullName, "config.xml"));

            return MapConfig.Write(MapConfig.BuildFromDir(dir), file);
        }

        public static Map LoadFromConfig( FileInfo file )
        {
            MapConfig cfg = MapConfig.Read(file);
            if (cfg == null)
                return null;

            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(file.FullName));

            Map map = new Map();

            map.Name = cfg.Name;
            map.Mines = cfg.Mines.ToArray();

            List<Castle> c = new List<Castle>();
            foreach (CapitalDefinition capdef in cfg.Capitals)
                c.Add(new Castle(capdef));

            map.Capitals = c.ToArray();

            map.TerrainMap = Bitmap.FromFile(Path.Combine(dir.FullName, "map.png"));
            if (map.TerrainMap == null)
                return null;

            map.Size = new Point(cfg.Width, cfg.Height);

            List<FileInfo> fileList = new List<FileInfo>();

            for (int h = 0; h < cfg.SectionsH; h++)
            {
                for (int w = 0; w < cfg.SectionsW; w++)
                    fileList.Add(new FileInfo(Path.Combine(dir.FullName, w.ToString() + "_" + h.ToString() + ".png")));
            }

            map.ImageMaps = fileList.ToArray();

            map.XImages = cfg.SectionsW;
            map.YImages = cfg.SectionsH;

            map.Init();
            return map;
        }
    }
}
