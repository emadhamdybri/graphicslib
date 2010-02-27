﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

using Hosts;
using Simulation;
using Messages;

using Simulation;

using Lidgren.Network;

namespace Project23Server
{
    public partial class Server
    {
        Dictionary<Type, MessageHandler> messageHandlers = new Dictionary<Type, MessageHandler>();

        protected void InitMessageHandlers()
        {
            messageHandlers.Add(typeof(Login), new MessageHandler(LoginHandler));
            messageHandlers.Add(typeof(PlayerJoin), new MessageHandler(PlayerJoinHandler));
            messageHandlers.Add(typeof(ChatMessage), new MessageHandler(ChatMessageHandler));
            messageHandlers.Add(typeof(RequestSpawn), new MessageHandler(RequestSpawnHandler));
            messageHandlers.Add(typeof(Ping), new MessageHandler(PingHandler));
            messageHandlers.Add(typeof(WhatTimeIsIt), new MessageHandler(WhatTimeIsItHandler));
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

        protected void PingHandler(Client client, MessageClass message)
        {
            Ping msg = message as Ping;
            if (msg == null)
                return;

            Pong pong = new Pong();
            pong.ID = msg.ID;
            host.SendMessage(client.Connection, pong.Pack(), pong.Channel());
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

            client.Player = sim.NewPlayer();
            client.Player.Tag = client;

            client.Player.ID = GUIDManager.NewGUID();
            client.Player.Pilot = join.Pilot;

            while (!sim.PlayerNameValid(join.Callsign))
                join.Callsign += "X";

            client.Player.Callsign = join.Callsign;

            sim.AddPlayer(client.Player);

            PlayerJoinAccept accept = new PlayerJoinAccept();
            accept.Callsign = client.Player.Callsign;
            accept.PlayerID = client.Player.ID;

            host.SendMessage(client.Connection, accept.Pack(), accept.Channel());

            AllowSpawn spawn = new AllowSpawn();
            host.SendMessage(client.Connection, spawn.Pack(), spawn.Channel());
        }

        protected void ChatMessageHandler(Client client, MessageClass message)
        {
            ChatMessage msg = message as ChatMessage;
            if (msg == null || client.Player == null)
                return;

            msg.From = client.Player.Callsign;
            host.Broadcast(msg.Pack(), msg.Channel());
        }
        
        protected void RequestSpawnHandler(Client client, MessageClass message)
        {
            RequestSpawn msg = message as RequestSpawn;
            if (msg == null || client.Player == null)
                return;

            sim.SpawnPlayer(client.Player, lastUpdateTime);
        }

        protected void WhatTimeIsItHandler(Client client, MessageClass message)
        {
            WhatTimeIsIt msg = message as WhatTimeIsIt;
            if (msg == null)
                return;

            TheTimeIsNow time = new TheTimeIsNow();
            time.ID = msg.ID;
            time.Time = Now();
            host.SendMessage(client.Connection, time.Pack(), time.Channel());
        }
    }
}