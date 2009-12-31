using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project24
{
    class ChatMessage
    {
        public String Message = string.Empty;
        public DateTime TimeStamp = DateTime.Now;

        public ChatMessage ( string msg )
        {
            Message = msg;
        }
    }
    class ChatChannel
    {
        public string Name = string.Empty;
        public int Limit = 10;
        public List<ChatMessage> ChatMessages = new List<ChatMessage>();

        public ChatChannel ( string name )
        {
            Name = name;
        }

        public void Add( ChatMessage message )
        {
            if (ChatMessages.Count == Limit - 1)
                ChatMessages.Remove(ChatMessages[0]);

            ChatMessages.Add(message);
        }

        public void Flush ( )
        {
            ChatMessages.Clear();
        }
    }

    class ChatLog
    {
        public static string GeneralChatChannel = "@General";
        public static string CombatChatChannel = "@Combat";
        public static string TeamChatChannel = "@Team";

        public string CurrentChannel = string.Empty;

        protected Dictionary<string, ChatChannel> Channels = new Dictionary<string, ChatChannel>();

        public void AddMessage ( string channel, string message )
        {
            if (!Channels.ContainsKey(channel))
                Channels.Add(channel, new ChatChannel(channel));

            Channels[channel].Add(new ChatMessage(message));
        }

        public void ClearChannel ( string channel )
        {
            if (!Channels.ContainsKey(channel))
                return;

            Channels.Remove(channel);
        }

        public ChatChannel GetChannel ( string channel )
        {
            if (!Channels.ContainsKey(channel))
                Channels.Add(channel, new ChatChannel(channel));

            return Channels[channel];
        }

        public string[] GetChannelNames()
        {
            return Channels.Keys.ToArray();
        }
    }
}
