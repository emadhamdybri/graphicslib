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
    }

    public class Player
    {
        public RealmType Realm = RealmType.Unknown;

        public string Name = string.Empty;
        public int UID = -1;

        public PlayerColor Color = PlayerColor.Black;

        public int Gold = -1;

        public List<Castle> Castles = new List<Castle>();
        public List<Camp> Camps = new List<Camp>();

        public List<UnitDescriptor> UnitTypes = new List<UnitDescriptor>();

        public Dictionary<UnitType,List<UnitInstance>> DeployedUnits = new Dictionary<UnitType,List<UnitInstance>>();
    }
}
