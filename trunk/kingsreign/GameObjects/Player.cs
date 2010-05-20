using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameObjects
{
    public enum PlayerColor
    {
        Black,
        Blue,
        Brown,
        Green,
        Orange,
        Purple,
        Red,
        Teal,
        White,
        Unknown,
    }

    public class Player
    {
        public delegate void UnitEventHandler(Player player, UnitInstance unit);

        public static event UnitEventHandler UnitDeployed;
        public static event UnitEventHandler UnitDestroyed;
        public static event UnitEventHandler UnitMustered;
       
        public RealmType Realm = RealmType.Unknown;

        public string Name = string.Empty;
        public int UID = -1;

        public PlayerColor Color = PlayerColor.Black;

        public int Gold = -1;

        public List<Castle> Castles = new List<Castle>();
        public List<Camp> Camps = new List<Camp>();

        public List<UnitDescriptor> UnitTypes = new List<UnitDescriptor>();

        public Dictionary<UnitType,List<UnitInstance>> DeployedUnits = new Dictionary<UnitType,List<UnitInstance>>();

        public bool BuildDone = false;

        public bool CanBuildUnit ( UnitDescriptor unit )
        {
            return unit.Cost >= Gold;
        }

        public bool BuyUnit ( UnitDescriptor unit )
        {
            if (Castles.Count == 0)
                return false;

            return BuyUnit(unit, Castles[0]);
        }

        public bool BuyUnit ( UnitDescriptor unit, Castle castle )
        {
            if (!CanBuildUnit(unit))
                return false;

            if (!castle.StationedUnits.ContainsKey(unit.Type) || castle.StationedUnits[unit.Type] == null)
                castle.StationedUnits.Add(unit.Type,new List<UnitInstance>());

            UnitInstance u = UnitInstance.Muster(this, castle, unit);
            castle.StationedUnits[unit.Type].Add(u);
            if (UnitMustered != null)
                UnitMustered(this,u);
            return true;
        }

        public bool DeployUnit ( UnitInstance unit )
        {
             if (Castles.Count == 0)
                return false;

            return DeployUnit(unit, Castles[0]);
        }

        public bool DeployUnit ( UnitInstance unit, Castle castle )
        {
            if (!castle.StationedUnits.ContainsKey(unit.Descriptor.Type) || castle.StationedUnits[unit.Descriptor.Type].Count == null || !castle.StationedUnits[unit.Descriptor.Type].Contains(unit))
                return false;
           
            if (!DeployedUnits.ContainsKey(unit.Descriptor.Type) || DeployedUnits[unit.Descriptor.Type] == null)
                DeployedUnits.Add(unit.Descriptor.Type,new List<UnitInstance>());

            castle.StationedUnits[unit.Descriptor.Type].Remove(unit);
            DeployedUnits[unit.Descriptor.Type].Add(unit);

            unit.Station = UnitInstance.StationType.Deployed;

            unit.Position = castle.GetSpawnLoc();

            if (UnitDeployed != null)
                UnitDeployed(this,unit);
            return true;
        }

        public bool DestroyUnit ( UnitInstance unit )
        {
            if (UnitDestroyed != null)
                UnitDestroyed(this,unit);
            return false;
        }
    }
}
