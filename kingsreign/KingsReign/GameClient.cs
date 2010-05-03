using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameObjects;

namespace KingsReign
{
    public class GameClient
    {
        public delegate void MapLoadedEvent ( GameClient sender, Map map );

        public event MapLoadedEvent MapLoaded;

        Map WorldMap;

        public void LoadMap ( string file )
        {
            FileInfo f = new FileInfo(file);
            if (!f.Exists)
                return;

            if (MapLoaded)
                MapLoaded(this, WorldMap);
        }
    }
}
