using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameObjects
{
    public class Castle
    {
        public RealmType Realm = RealmType.Unknown;
        public Point Location = Point.Empty;

        public Dictionary<UnitType, UnitInstance> StationedUnits = new Dictionary<UnitType, UnitInstance>();

        public Castle ( CapitalDefinition def )
        {
            Location = def.Location;
            Realm = def.Realm;
        }
    }

    public class Camp
    {
        public RealmType Realm = RealmType.Unknown;
        public Point Location = Point.Empty;

        public int Fortitude = -1;
        public int Damage = -1;

        public Dictionary<UnitType, UnitInstance> StationedUnits = new Dictionary<UnitType, UnitInstance>();
    }
}
