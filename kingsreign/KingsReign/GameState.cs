using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using GameObjects;

namespace KingsReign
{
    public class GameState
    {
        public Map WorldMap;

        public List<UnitDescriptor> UnitDefs = new List<UnitDescriptor>();
        public List<Player> Players = new List<Player>();

        public enum PlayState
        {
            Stopped,
            Waiting,
            Build,
            Play,
            Dead
        }

        public PlayState State = PlayState.Stopped;

        protected Stopwatch Timer = new Stopwatch();
        protected double BuildTime = 120.0;
        protected double ClockOffset = 0;

        protected double BuildStartTime = 0;

        protected bool RemoteHosted = false;

        public void Init ( bool remoteHosted )
        {
            RemoteHosted = remoteHosted;
            Timer.Start();
        }

        protected double GetTime()
        {
            if (RemoteHosted)
                return ClockOffset + Timer.ElapsedMilliseconds / 1000.0;
            return Timer.ElapsedMilliseconds / 1000.0;
        }

        public void Update ()
        {
            if (State == PlayState.Build)
            {

            }
        }

        public void StartBuild ()
        {
            BuildStartTime = GetTime();
            State == PlayState.Build;
        }

        protected void SetPlayerRealmData(Player player)
        {
            foreach (UnitDescriptor d in UnitDefs)
            {
                if (d.Realm == player.Realm || d.Realm == RealmType.All)
                    player.UnitTypes.Add(d);
            }
        }

        public RealmType FindEmptyCastle ()
        {
            foreach (Castle castle in WorldMap.Capitals)
            {
                if (castle.Player == null && castle.Realm != RealmType.All)
                    return castle.Realm;
            }
            return RealmType.Unknown;
        }

        protected bool ColorIsUsed ( PlayerColor color )
        {
            foreach(Player p in Players)
            {
                if (p.Color == color)
                    return true;
            }
            return false;
        }

        public PlayerColor FindFreeColor ( PlayerColor color )
        {
            if (!ColorIsUsed(color))
                return color;

            if (ColorIsUsed(PlayerColor.Green))
                return PlayerColor.Green;
            if (ColorIsUsed(PlayerColor.Red))
                return PlayerColor.Red;
            if (ColorIsUsed(PlayerColor.Blue))
                return PlayerColor.Blue;
            if (ColorIsUsed(PlayerColor.Purple))
                return PlayerColor.Purple;
            if (ColorIsUsed(PlayerColor.Orange))
                return PlayerColor.Orange;
            if (ColorIsUsed(PlayerColor.Teal))
                return PlayerColor.Teal;
            if (ColorIsUsed(PlayerColor.Orange))
                return PlayerColor.Orange;
            if (ColorIsUsed(PlayerColor.Black))
                return PlayerColor.Black;
            if (ColorIsUsed(PlayerColor.White))
                return PlayerColor.White;

            return PlayerColor.Unknown;
        }

        public Player NewPlayer (string name, PlayerColor color, RealmType realm )
        {
            if (realm == RealmType.Unknown)
                return null;

            Player p = new Player();
            p.Name = name;
            p.Gold = WorldMap.StaringGold;
            p.UID = Players.Count+1;

            // find them a capital
            foreach (Castle castle in WorldMap.Capitals)
            {
                if (castle.Player == null && castle.Realm == realm)
                {
                    p.Castles.Add(castle);
                    castle.Player = p;
                    break;
                }
            }

            if (p.Castles.Count == 0) // no empty castles of the realm
            {
                foreach (Castle castle in WorldMap.Capitals)
                {
                    if (castle.Player == null && castle.Realm == RealmType.All)
                    {
                        p.Castles.Add(castle);
                        castle.Player = p;
                        break;
                    }
                }
            }

            if (p.Castles.Count == 0)
                return NewPlayer(name, color, FindEmptyCastle());

            p.Realm = realm;
            p.Color = FindFreeColor(color);
            if (p.Color == PlayerColor.Unknown)
                return null;

            SetPlayerRealmData(p);
            Players.Add(p);
            return p;
        }
    }
}
