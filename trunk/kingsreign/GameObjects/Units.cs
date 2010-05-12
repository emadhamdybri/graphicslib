using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameObjects
{
    public enum UnitType
    {
        Unknown,
        Ground,
        Ranged,
        Calvary,
        Airbone,
        Spy
    }

    public class CombatStats
    {
        public int Attack = 0;
        public int Defense = 0;

        public static CombatStats Empty = new CombatStats();
    }

    public class UnitDescriptor
    {
        public UnitType Type = UnitType.Unknown;
        public RealmType Realm = RealmType.Unknown;

        public CombatStats CombatPower = CombatStats.Empty;
        public CombatStats RangedPower = CombatStats.Empty;

        public int Health = 0;
        public float Speed = 0f;
        public int MovementCost = 0;
        public int Upkeep = 0;

        public int Cost = 0;

        public string GraphicType = string.Empty;
    }

    public class UnitInstance
    {
        public enum StationType
        {
            AtCastle,
            AtCamp,
            Deployed,
        }

        public UnitDescriptor Descriptor = new UnitDescriptor();

        public int PlayerID = -1;

        public float Compliment = 1.0f;
        public float Damage = 0.0f;
        public int Experience = 0;
        public int Level = 0;

        public StationType Station = StationType.AtCastle;

        public Point Position = Point.Empty;
        public Point Desination = Point.Empty;

        public static void Battle ( ref UnitInstance attacker, ref UnitInstance defender )
        {

        }
    }
}
