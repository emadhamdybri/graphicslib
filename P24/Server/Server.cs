using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hosts;
using Simulation;
using Lidgren.Network;
using System.Threading;
using System.Reflection;

using Messages;

namespace Project23Server
{
    public class Client : System.Object
    {
        public NetConnection Connection;
        public Simulation.Player Player = null;

        public String Username = String.Empty;

        public Client ( NetConnection connection )
        {
            Connection = connection;
        }

        public void Hail ( Host host )
        {
            Messages.Hail hail = new Messages.Hail();
            host.SendMessage(Connection, hail.Pack(), hail.Channel());
        }
    }

    public delegate void MessageHandler ( Client client, MessageClass message );

    public partial class Server
    {
        Sim sim = new Sim();
        Host host;

        public bool SaveMessages = true;

        MessageMapper messageMapper = new MessageMapper();

        public Dictionary<NetConnection, Client> Clients = new Dictionary<NetConnection, Client>();

        public List<String> PendingHostMessages = new List<String>();

        public Server ( int port )
        {
            host = new Host(port);
            sim.Init();

            host.Connect += new MonitoringEvent(host_Message);
            host.Disconnect += new MonitoringEvent(host_Message);
            host.DebugMessage += new MonitoringEvent(host_Message);

            sim.PlayerJoined += new PlayerJoinedHandler(sim_PlayerJoined);
            sim.PlayerRemoved += new PlayerRemovedHandler(sim_PlayerRemoved);

            InitMessageHandlers();
        }

        public void Kill ( )
        {
            host.Kill();
        }

        protected void sim_PlayerJoined ( object sender, PlayerEventArgs args )
        {           
            PlayerInfo info = new PlayerInfo(args.player);

            host.Broadcast(info.Pack(), info.Channel());
        }

        protected void sim_PlayerRemoved(object sender, PlayerEventArgs args)
        {
            Client client = args.player.Tag as Client;
            if (client != null)
            {
                Messages.Disconnect disconnect = new Messages.Disconnect();
                disconnect.ID = client.Player.ID;
                host.Broadcast(disconnect.Pack(), disconnect.Channel());
            }
        }

        protected void DisconnectPlayer ( NetConnection player )
        {
            if (Clients.ContainsKey(player))
            {
                Client client = Clients[player];
                Clients.Remove(player);

                if (client.Player != null)
                    sim.RemovePlayer(client.Player);
            }
        }        

        public void Update ( double time )
        {
            NetConnection newConnect = host.GetPentConnection();
            while (newConnect != null)
            {
                Client client = new Client(newConnect);
                Clients.Add(newConnect,client );
                client.Hail(host);
                newConnect = host.GetPentConnection();
            }

            NetConnection newDisconnect = host.GetPentDisconnection();
            while (newDisconnect != null)
            {
                DisconnectPlayer(newDisconnect);
                newDisconnect = host.GetPentDisconnection();
            }

            Message msg = host.GetPentMessage();
            while (msg != null)
            {
                ProcessMessage(msg);
                msg = host.GetPentMessage();
            }

            sim.Update(time);
        }

        public String PopHostMessage ( )
        {
            String msg = string.Empty;
            lock(PendingHostMessages)
            {
                if (PendingHostMessages.Count > 0)
                {
                    msg = PendingHostMessages[0];
                    PendingHostMessages.Remove(msg);
                }
            }
            return msg;
        }

        void host_Message(object sender, MonitoringEventArgs args)
        {
            if (!SaveMessages)
                return;

            lock (PendingHostMessages)
                PendingHostMessages.Add(args.Message);
        }
    }
}
