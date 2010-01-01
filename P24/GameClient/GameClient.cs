﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Simulation;
using Clients;
using Messages;
using Lidgren.Network;

namespace Project23Client
{
    public delegate void MessageHandler(MessageClass message);

    public delegate void AuthenticationCallback(ref string username, ref string token);
    public delegate void JoinInfoCallback(ref string callsign, ref string pilot);

    public delegate void ServerVersionHandler ( object sender, int version );
    public delegate void PlayerEventHandler(object sender, Player player);

    public partial class GameClient
    {
        public Sim sim = new Sim();
        public Player ThisPlayer = null;

        public event ServerVersionHandler ServerVersionEvent;
        public event PlayerEventHandler LocalPlayerJoinedEvent;

        Client client;
        bool connected = false;

        public bool ConnectedToHost
        {
            get { return connected; }
        }

        public AuthenticationCallback GetAuthentication;
        public JoinInfoCallback GetJoinInfo;

        MessageMapper messageMapper = new MessageMapper();

        public GameClient ( string address, int port )
        {
            InitMessageHandlers();
            client = new Client(address, port);
        }

        public void Kill ()
        {
            client.Kill();
        }

        public bool Update (double time)
        {
            if (connected)
            {
                if (!client.IsConnected)
                    return false;

                NetBuffer buffer = client.GetPentMessage();
                while (buffer != null)
                {
                    if (buffer.LengthBytes >= sizeof(Int32))
                    {
                        MessageClass msg = messageMapper.MessageFromID(buffer.ReadInt32());
                        msg.Unpack(ref buffer);

                        if (messageHandlers.ContainsKey(msg.GetType()))
                            messageHandlers[msg.GetType()](msg);
                    }
                    buffer = client.GetPentMessage();
                }

                sim.Update(time);
            }
            else
                connected = client.IsConnected;
            return true;
        }
    }
}