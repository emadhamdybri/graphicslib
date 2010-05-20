using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameObjects
{
    public class Castle
    {
        public static int StationRadius = 100;

        public RealmType Realm = RealmType.Unknown;
        public Point Location = Point.Empty;

        public Dictionary<UnitType, List<UnitInstance>> StationedUnits = new Dictionary<UnitType, List<UnitInstance>>();
        public Player Player = null;

        public Castle ( CapitalDefinition def )
        {
            Location = def.Location;
            Realm = def.Realm;
        }

        public Point GetSpawnLoc ()
        {
            double angle = new Random().NextDouble();
            double x = Math.Cos(angle);
            double y = Math.Sin(angle);

            return new Point(Location.X + (int)(x * StationRadius * 0.75), Location.Y + (int)(y * StationRadius * 0.75));
        }
    }

    public class Camp
    {
        public static int StationRadius = 100;

        public RealmType Realm = RealmType.Unknown;
        public Point Location = Point.Empty;
        public Player Player = null;

        public int Fortitude = -1;
        public int Damage = -1;

        public Dictionary<UnitType, List<UnitInstance>> StationedUnits = new Dictionary<UnitType, List<UnitInstance>>();
    }
}
