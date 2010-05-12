using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameObjects
{
    public class Player
    {
        public RealmType Realm = RealmType.Unknown;

        public string Name = string.Empty;
        public int UID = -1;

        public int Gold = -1;

        public List<Castle> Castles = new List<Castle>();
        public List<Camp> Camps = new List<Camp>();

        public List<UnitDescriptor> UnitTypes = new List<UnitDescriptor>();

        public Dictionary<UnitType,UnitInstance> DeployedUnits = new Dictionary<UnitType,UnitInstance>();
    }
}
