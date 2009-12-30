using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Messages;

namespace Project24Client
{
    public partial class GameClient
    {
        Dictionary<Type, MessageHandler> messageHandlers = new Dictionary<Type, MessageHandler>();

        protected void InitMessageHandlers()
        {
            messageHandlers.Add(typeof(Hail), new MessageHandler(HailHandler));
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

            Login login = new Login();
            login.username = username;
            login.token = token;
            client.SendMessage(login.Pack(), login.Channel());
        }
    }
}
