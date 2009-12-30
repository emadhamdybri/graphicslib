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
        }
    }
}
