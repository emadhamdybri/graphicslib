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

        public Castle ( CapitalDefinition def )
        {
            Location = def.Location;
            Realm = def.Realm;


        }
    }
}
