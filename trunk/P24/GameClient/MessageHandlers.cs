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
        }

        protected void HailHandler(MessageClass message)
        {
            Hail hail = message as Hail;
            if (hail == null)
                return;

            string username = string.Empty;
            string token = string.Empty;
            if (GetAuthentication != null)
                GetAuthentication(ref username, ref token);

            ThisPlayer.Callsign = username;

            Login login = new Login();
            login.username = username;
            login.token = token;
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

            sim.AddPlayer(player);
        }

        protected void PlayerListDoneHandler(MessageClass message)
        {
        }
    }
}
