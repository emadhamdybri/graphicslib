using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

using Hosts;
using Simulation;
using Messages;

using Lidgren.Network;

namespace Project24Server
{
    public partial class Server
    {
        Dictionary<Type, MessageHandler> messageHandlers = new Dictionary<Type, MessageHandler>();

        protected void InitMessageHandlers()
        {
            messageHandlers.Add(typeof(Login), new MessageHandler(LoginHandler));
            messageHandlers.Add(typeof(PlayerJoin), new MessageHandler(PlayerJoinHandler));
        }

        protected void ProcessMessage(Message msg)
        {
            if (!Clients.ContainsKey(msg.Sender))
                return;

            Client client = Clients[msg.Sender];

            MessageClass message = messageMapper.MessageFromID(msg.Name);
            message.Unpack(ref msg.Data);

            if (messageHandlers.ContainsKey(message.GetType()))
                messageHandlers[message.GetType()](client, message);
        }

        protected void LoginHandler(Client client, MessageClass message)
        {
            Login login = message as Login;
            if (login == null)
                return;

            client.Username = login.username;

            ServerVersInfo vers = new ServerVersInfo();
            host.SendMessage(client.Connection, vers.Pack(), vers.Channel());

            MapInfo map = new MapInfo();
            map.Map = sim.Map;
            host.SendMessage(client.Connection, map.Pack(), map.Channel());

            foreach( Player player in sim.Players)
            {
                PlayerInfo info = new PlayerInfo(player);
                host.SendMessage(client.Connection, info.Pack(), info.Channel());
            }

            PlayerListDone done = new PlayerListDone();
            host.SendMessage(client.Connection, done.Pack(), done.Channel());
        }

        protected void PlayerJoinHandler(Client client, MessageClass message)
        {
            PlayerJoin join = message as PlayerJoin;
            if (join == null)
                return;

            Player player = new Player();
            player.Tag = client;
            player.ID = GUIDManager.NewGUID();
            player.Pilot = join.Pilot;

            while (!sim.PlayerNameValid(join.Callsign))
                join.Callsign += "X";

            player.Callsign = join.Callsign;

            sim.AddPlayer(player);

            PlayerJoinAccept accept = new PlayerJoinAccept();
            accept.Callsign = player.Callsign;
            accept.PlayerID = player.ID;

            host.SendMessage(client.Connection, accept.Pack(), accept.Channel());
        }
    }
}
