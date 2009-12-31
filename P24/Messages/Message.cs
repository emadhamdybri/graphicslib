using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Lidgren.Network;

namespace Messages
{
    public class MessageMapper
    {
        protected Dictionary<int, Type> MessageTypes = new Dictionary<int, Type>();

        public MessageMapper()
        {
            Assembly myModule = Assembly.GetExecutingAssembly();
            foreach (Type t in myModule.GetTypes())
            {
                if (t.BaseType == typeof(MessageClass))
                {
                    MessageClass m = (MessageClass)Activator.CreateInstance(t);

                    MessageTypes.Add(m.Name, t);
                }
            }
        }

        public MessageClass MessageFromID ( int id )
        {
            if (MessageTypes.ContainsKey(id))
                return (MessageClass)Activator.CreateInstance(MessageTypes[id]);
            return null;
        }
    }

    public class MessageClass
    {
        public int Name = -1;

        static int GetName ( ref NetBuffer  buffer )
        {
            return buffer.ReadInt32();
        }

        public virtual NetBuffer Pack ()
        {
            NetBuffer buffer = new NetBuffer();
            buffer.Write(Name);
            return buffer;
        }

        public virtual bool Unpack(ref NetBuffer buffer)
        {
            return true;
        }

        public virtual NetChannel Channel ()
        {
            return NetChannel.ReliableInOrder1;
        }
    }

    public class Hail : MessageClass
    {
        public Hail()
        {
            Name = 10;
        }
    }

    public class Disconnect : MessageClass
    {
        public UInt64 ID = 0;

        public Disconnect()
        {
            Name = 11;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(ID);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            ID = buffer.ReadUInt64();
            return true;
        }
    }

    public class Login : MessageClass
    {
        public String username = string.Empty;
        public String token = string.Empty;

        public Login()
        {
            Name = 20;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(username);
            buffer.Write(token);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            username = buffer.ReadString();
            token = buffer.ReadString();
            return true;
        }
    }

    public class ServerVersInfo : MessageClass
    {
        public int Vers = 1;

        public ServerVersInfo()
        {
            Name = 30;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(Vers);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            Vers = buffer.ReadInt32();
            return true;
        }
    }

    public class PlayerInfo : MessageClass
    {
        public UInt64 PlayerID = 0;
        public string Callsign = string.Empty;
        public int Score = -1;

        public PlayerInfo()
        {
            Name = 40;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(PlayerID);
            buffer.Write(Callsign);
            buffer.Write(Score);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            PlayerID = buffer.ReadUInt64();
            Callsign = buffer.ReadString();
            Score = buffer.ReadInt32();
            return true;
        }

        public virtual NetChannel Channel()
        {
            return NetChannel.UnreliableInOrder2;
        }
    }

    public class PlayerListDone : MessageClass
    {
        public PlayerListDone()
        {
            Name = 41;
        }

        public virtual NetChannel Channel()
        {
            return NetChannel.UnreliableInOrder2;
        }
    }

    public class PlayerJoin : MessageClass
    {
        public string Callsign = string.Empty;

        public PlayerJoin()
        {
            Name = 42;
        }

        public virtual NetChannel Channel()
        {
            return NetChannel.UnreliableInOrder3;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(Callsign);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            Callsign = buffer.ReadString();
            return true;
        }
    }

    public class PlayerJoinAccept : MessageClass
    {
        public UInt64 PlayerID = 0;
        public string Callsign = string.Empty;

        public PlayerJoinAccept()
        {
            Name = 43;
        }

        public virtual NetChannel Channel()
        {
            return NetChannel.UnreliableInOrder3;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(PlayerID);
            buffer.Write(Callsign);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            PlayerID = buffer.ReadUInt64();
            Callsign = buffer.ReadString();
            return true;
        }
    }
}
