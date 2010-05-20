using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;

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

        protected UnitDescriptor Archers;

        public GameState State = new GameState();

        public void LoadMap ( string file )
        {
            FileInfo f = new FileInfo(file);
            if (!f.Exists)
                return;

            State.WorldMap = Map.LoadFromConfig(f);

            if (MapLoaded != null)
                MapLoaded(this, State.WorldMap);
        }

        public void LoadDefaultUnits ()
        {
            // Archers
            UnitDescriptor d = new UnitDescriptor();
            d.Name = "Archers";
            d.CombatPower = new CombatStats(10, 10);
            d.RangedPower = new CombatStats(25, 20);
            d.Speed = 25;
            d.MovementCost = 1;
            d.Type = UnitType.Ranged;
            d.Realm = RealmType.All;
            d.Health = 100;
            d.GraphicType = "archer";
            d.Upkeep = 0;
            State.UnitDefs.Add(d);

            // Infantry
            d = new UnitDescriptor();
            d.Name = "Infantry";
            d.CombatPower = new CombatStats(25, 30);
            d.RangedPower = new CombatStats(0, 15);
            d.Speed = 20;
            d.MovementCost = 2;
            d.Type = UnitType.Ground;
            d.Realm = RealmType.All;
            d.Health = 200;
            d.GraphicType = "legion";
            d.Upkeep = 1;
            State.UnitDefs.Add(d);
            Archers = d;

            // Calvary
            d = new UnitDescriptor();
            d.Name = "Calvary";
            d.CombatPower = new CombatStats(30, 15);
            d.RangedPower = new CombatStats(0, 10);
            d.Speed = 40;
            d.MovementCost = 4;
            d.Type = UnitType.Calvary;
            d.Realm = RealmType.All;
            d.Health = 150;
            d.GraphicType = "calvary";
            d.Upkeep = 2;
            State.UnitDefs.Add(d);

            // Flyers
            d = new UnitDescriptor();
            d.Name = "Gryphon Riders";
            d.CombatPower = new CombatStats(10, 25);
            d.RangedPower = new CombatStats(20, 15);
            d.Speed = 30;
            d.MovementCost = 3;
            d.Type = UnitType.Airbone;
            d.Realm = RealmType.All;
            d.Health = 100;
            d.GraphicType = "flyers";
            d.Upkeep = 1;
            State.UnitDefs.Add(d);

            // mages
            d = new UnitDescriptor();
            d.Name = "Mages";
            d.CombatPower = new CombatStats(10, 10);
            d.RangedPower = new CombatStats(35, 35);
            d.Speed = 15;
            d.MovementCost = 1;
            d.Type = UnitType.Ranged;
            d.Realm = RealmType.All;
            d.Health = 80;
            d.GraphicType = "mages";
            d.Upkeep = 4;
            State.UnitDefs.Add(d);

            // spy
            d = new UnitDescriptor();
            d.Name = "Spys";
            d.CombatPower = new CombatStats(35, 10);
            d.RangedPower = new CombatStats(0, 10);
            d.Speed = 35;
            d.MovementCost = 0;
            d.Type = UnitType.Ranged;
            d.Realm = RealmType.All;
            d.Health = 65;
            d.GraphicType = "spy";
            d.Upkeep = 5;
            State.UnitDefs.Add(d);
        }

        public void SetupTestPlayer ()
        {
            Player p = State.NewPlayer("TestPlayer", PlayerColor.Teal, RealmType.Arlan);

            UnitInstance unit = new UnitInstance();
            unit.Descriptor = Archers;
            unit.Player = p;
            unit.Position = new Point(p.Castles[0].Location.X + 100, p.Castles[0].Location.Y);
            unit.Station = UnitInstance.StationType.Deployed;
            AddUnit(unit, p);
        }

        public void InitHosted ( string map )
        {
            LoadMap(ResourceManager.FindFile("maps/" + map +"/config.xml"));
            LoadDefaultUnits();
        }

        public void InitClient ( string host, int port )
        {

        }

        public void Kill()
        {

        }

        public void AddUnit ( UnitInstance unit, Player player )
        {
            if (!player.DeployedUnits.ContainsKey(unit.Descriptor.Type))
                player.DeployedUnits.Add(unit.Descriptor.Type,new List<UnitInstance>());
            
            player.DeployedUnits[unit.Descriptor.Type].Add(unit);
            if (UnitAdded != null)
                UnitAdded(this, player, unit);
        }

        public bool Update ()
        {
            if (Updated != null)
                Updated(this);
            return true;
        }
    }
}
