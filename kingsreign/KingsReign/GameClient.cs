using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using GameObjects;

using Utilities.Paths;

namespace KingsReign
{
    public class GameClient
    {
        public delegate void MapLoadedEvent ( GameClient sender, Map map );
        public event MapLoadedEvent MapLoaded;

        public delegate void UpdateEvent(GameClient sender);
        public event UpdateEvent Updated;

        public delegate void UnitChangeEvent(GameClient sender, Player player, UnitInstance unit);
        public event UnitChangeEvent UnitAdded;
        public event UnitChangeEvent UnitChanged;

        public Map WorldMap;

        Dictionary<int, Player> Players = new Dictionary<int, Player>();

        public void LoadMap ( string file )
        {
            FileInfo f = new FileInfo(file);
            if (!f.Exists)
                return;

            WorldMap = Map.LoadFromConfig(f);

            if (MapLoaded != null)
                MapLoaded(this, WorldMap);
        }

        public void InitHosted ( string map )
        {
            LoadMap(ResourceManager.FindFile("maps/" + map +"/config.xml"));
        }

        public void InitClient ( string host, int port )
        {

        }

        public void Kill()
        {

        }

        public bool Update ()
        {
            if (Updated != null)
                Updated(this);
            return true;
        }
    }
}
