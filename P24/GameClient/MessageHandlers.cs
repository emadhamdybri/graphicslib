using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Messages;
using Simulation;

namespace Project24Client
{
    public partial class GameClient
    {
        Dictionary<Type, MessageHandler> messageHandlers = new Dictionary<Type, MessageHandler>();

        protected void InitMessageHandlers()
        {
            messageHandlers.Add(typeof(Hail), new MessageHandler(HailHandler));
            messageHandlers.Add(typeof(ServerVersInfo), new MessageHandler(ServerVersHandler));
            messageHandlers.Add(typeof(PlayerInfo), new MessageHandler(PlayerInfoHandler));
            messageHandlers.Add(typeof(PlayerListDone), new MessageHandler(PlayerListDoneHandler));
            messageHandlers.Add(typeof(MapInfo), new MessageHandler(MapInfoHandler));
            messageHandlers.Add(typeof(PlayerJoinAccept), new MessageHandler(PlayerJoinAcceptHandler));
        }

        protected void HailHandler(MessageClass message)
        {
            Hail hail = message as Hail;
            if (hail == null)
                return;

            Login login = new Login();
            login.username = string.Empty;
            login.token = string.Empty;
            if (GetAuthentication != null)
                GetAuthentication(ref login.username, ref login.token);

            client.SendMessage(login.Pack(), login.Channel());
        }

        protected void ServerVersHandler(MessageClass message)
        {
            ServerVersInfo info = message as ServerVersInfo;
            if (info == null)
                return;

            if (ServerVersionEvent != null)
                ServerVersionEvent(this, info.Vers);
        }

        protected void PlayerInfoHandler(MessageClass message)
        {
            PlayerInfo info = message as PlayerInfo;
            if (info == null)
                return;

            Player player = new Player();
            player.ID = info.PlayerID;
            player.Callsign = info.Callsign;
            player.Score = info.Score;
            player.Pilot = info.Pilot;

            sim.AddPlayer(player);

            if (ThisPlayer != null && ThisPlayer.ID == info.PlayerID)// we already got the join accept so fire it off
            {
                if (LocalPlayerJoinedEvent != null)
                    LocalPlayerJoinedEvent(this, ThisPlayer);
            }
        }

        protected void MapInfoHandler ( MessageClass message )
        {
            MapInfo info = message as MapInfo;
            if (info == null)
                return;

            sim.Map = info.Map;
        }

        protected void PlayerListDoneHandler ( MessageClass message )
        {
            PlayerJoin join = new PlayerJoin();
            join.Callsign = "Player";
            join.Pilot = "Pilot0u";

            if (GetJoinInfo != null)
                GetJoinInfo(ref join.Callsign, ref join.Pilot);

            client.SendMessage(join.Pack(), join.Channel());
        }

        protected void PlayerJoinAcceptHandler ( MessageClass message )
        {
            PlayerJoinAccept msg = message as PlayerJoinAccept;
            if (msg == null)
                return;

            bool playerExisted = true;
            Player player = sim.FindPlayer(msg.PlayerID);
            if (player == null)
            {
                playerExisted = false;
                player = new Player();
                player.ID = msg.PlayerID;
                sim.AddPlayer(player);
            }

            player.Callsign = msg.Callsign;
            ThisPlayer = player;

            if (playerExisted) // we have not gotten our info yet
            {
                if (LocalPlayerJoinedEvent != null)
                    LocalPlayerJoinedEvent(this, ThisPlayer);
            }
        }
    }
}
