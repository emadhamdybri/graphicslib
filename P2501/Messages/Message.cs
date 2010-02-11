using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

using Lidgren.Network;
using OpenTK;
using Simulation;

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
        public Int32 Name = -1;

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

        protected void PackClass (ref NetBuffer buffer, object obj)
        {
            BinaryFormatter formater = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formater.Serialize(stream,obj);

            byte[] b = stream.ToArray();
            stream.Close();
            buffer.Write(b.Length);
            buffer.Write(b);
        }

        protected object UnpackClass ( ref NetBuffer buffer )
        {
            Int32 size = buffer.ReadInt32();
            byte[] b = buffer.ReadBytes(size);

            MemoryStream stream = new MemoryStream(b);

            BinaryFormatter formater = new BinaryFormatter();
            object obj = formater.Deserialize(stream);
            stream.Close();
            return obj;
        }

        protected void PackCompressedClass(ref NetBuffer buffer, object obj)
        {
            BinaryFormatter formater = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            GZipStream gstream = new GZipStream(stream, CompressionMode.Compress);
            formater.Serialize(gstream, obj);

            byte[] b = stream.ToArray();
            gstream.Close();
            stream.Close();
            buffer.Write(b.Length);
            buffer.Write(b);
        }

        protected object UnpackCompressedClass(ref NetBuffer buffer)
        {
            Int32 size = buffer.ReadInt32();
            byte[] b = buffer.ReadBytes(size);

            MemoryStream stream = new MemoryStream(b);
            GZipStream gstream = new GZipStream(stream, CompressionMode.Decompress);

            BinaryFormatter formater = new BinaryFormatter();
            object obj = formater.Deserialize(gstream);
            gstream.Close();
            stream.Close();
            return obj;
        }

        protected void PackObjectState (ref NetBuffer buffer, ObjectState state )
        {
            buffer.Write(state.Position.X);
            buffer.Write(state.Position.Y);
            buffer.Write(state.Position.Z);

            buffer.Write(state.Movement.X);
            buffer.Write(state.Movement.Y);
            buffer.Write(state.Movement.Z);

            buffer.Write(state.Rotation);
            buffer.Write(state.Spin);
        }

        protected ObjectState UnpackObjectState(ref NetBuffer buffer)
        {
            ObjectState state = new ObjectState();
            state.Position = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
            state.Movement = new Vector3(buffer.ReadFloat(), buffer.ReadFloat(), buffer.ReadFloat());
            state.Rotation = buffer.ReadFloat();
            state.Spin = buffer.ReadFloat();

            return state;
        }

        public static int Ping = 1;
        public static int Pong = 2;

        public static int Hail = 10;
        public static int Disconnect = 11;

        public static int Login = 20;
        public static int ServerVersInfo = 30;

        public static int PlayerInfo = 40;
        public static int PlayerListDone = 41;
        public static int PlayerJoin = 42;
        public static int PlayerJoinAccept = 43;

        public static int RequestMapInfo = 50;
        public static int MapInfo = 51;

        public static int ChatMessage = 60;

        public static int AllowSpawn = 70;
        public static int RequestSpawn = 71;
        public static int PlayerSpawn = 72;

        public static int WhatTimeIsIt = 80;
        public static int TheTimeIsNow = 81;
    }

    public class Ping : MessageClass
    {
        public UInt64 ID = 0;
        public Ping()
        {
            Name = MessageClass.Ping;
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

        public override NetChannel Channel()
        {
            return NetChannel.Unreliable;
        }
    }

    public class Pong : MessageClass
    {
        public UInt64 ID = 0;
        public Pong()
        {
            Name = MessageClass.Pong;
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

        public override NetChannel Channel()
        {
            return NetChannel.Unreliable;
        }
    }

    public class Hail : MessageClass
    {
        public Hail()
        {
            Name = MessageClass.Hail;
        }
    }

    public class Disconnect : MessageClass
    {
        public UInt64 ID = 0;

        public Disconnect()
        {
            Name = MessageClass.Disconnect;
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
            Name = MessageClass.Login;
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
            Name = MessageClass.ServerVersInfo;
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
        public string Pilot = string.Empty;
        public Int32 Score = -1;
        public PlayerStatus Status = PlayerStatus.Despawned;

        public PlayerInfo()
        {
            Name = MessageClass.PlayerInfo;
        }

        public PlayerInfo( Player player)
        {
            Name = MessageClass.PlayerInfo;

            PlayerID = player.ID;
            Callsign = player.Callsign;
            Score = player.Score;
            Pilot = player.Pilot;
            Status = player.Status;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(PlayerID);
            buffer.Write(Callsign);
            buffer.Write(Score);
            buffer.Write(Pilot);
            buffer.Write((byte)Status);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            PlayerID = buffer.ReadUInt64();
            Callsign = buffer.ReadString();
            Score = buffer.ReadInt32();
            Pilot = buffer.ReadString();
            Status = (PlayerStatus)Enum.ToObject(typeof(PlayerStatus), buffer.ReadByte());
            return true;
        }

        public override NetChannel Channel()
        {
            return NetChannel.UnreliableInOrder2;
        }
    }

    public class PlayerListDone : MessageClass
    {
        public PlayerListDone()
        {
            Name = MessageClass.PlayerListDone;
        }

        public override NetChannel Channel()
        {
            return NetChannel.UnreliableInOrder2;
        }
    }

    public class PlayerJoin : MessageClass
    {
        public string Callsign = string.Empty;
        public string Pilot = string.Empty;

        public PlayerJoin()
        {
            Name = MessageClass.PlayerJoin;
        }

        public override NetChannel Channel()
        {
            return NetChannel.UnreliableInOrder3;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(Callsign);
            buffer.Write(Pilot);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            Callsign = buffer.ReadString();
            Pilot = buffer.ReadString();
            return true;
        }
    }

    public class PlayerJoinAccept : MessageClass
    {
        public UInt64 PlayerID = 0;
        public string Callsign = string.Empty;

        public PlayerJoinAccept()
        {
            Name = MessageClass.PlayerJoinAccept;
        }

        public override NetChannel Channel()
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

    public class RequestMapInfo : MessageClass
    {
        public RequestMapInfo()
        {
            Name = MessageClass.RequestMapInfo;
        }
    }

    public class MapInfo : MessageClass
    {
        public MapDef Map = null;

        public MapInfo()
        {
            Name = MessageClass.MapInfo;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            base.PackClass(ref buffer, Map);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            Map = (MapDef)base.UnpackClass(ref buffer);
            return true;
        }
    }

    public class ChatMessage : MessageClass
    {
        public string Channel = string.Empty;
        public string From = string.Empty;
        public string Message = string.Empty;

        public ChatMessage()
        {
            Name = MessageClass.ChatMessage;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(Channel);
            buffer.Write(From);
            buffer.Write(Message);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            Channel = buffer.ReadString();
            From = buffer.ReadString();
            Message = buffer.ReadString();
           return true;
        }
    }

    public class AllowSpawn : MessageClass
    {
        public AllowSpawn()
        {
            Name = MessageClass.AllowSpawn;
        }

        public override NetChannel Channel()
        {
            return NetChannel.UnreliableInOrder3;
        }
    }

    public class RequestSpawn : MessageClass
    {
        // todo, ship type goes here
        public RequestSpawn()
        {
            Name = MessageClass.RequestSpawn;
        }
    }

    public class PlayerSpawn : MessageClass
    {
        public UInt64 PlayerID = 0;
        public ObjectState PlayerState;
        public double Time = -1;

        public PlayerSpawn()
        {
            Name = MessageClass.PlayerSpawn;
        }

        public PlayerSpawn ( Player player )
        {
            Name = MessageClass.PlayerSpawn;
            PlayerID = player.ID;
            PlayerState = player.LastUpdateState;
            Time = player.LastUpdateTime;  
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(PlayerID);
            PackObjectState(ref buffer, PlayerState);
            buffer.Write(Time);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            PlayerID = buffer.ReadUInt64();
            PlayerState = UnpackObjectState(ref buffer);
            Time = buffer.ReadDouble();
            return true;
        }
    }

    public class WhatTimeIsIt : MessageClass
    {
        public UInt64 ID = 0;

        public WhatTimeIsIt()
        {
            Name = MessageClass.WhatTimeIsIt;
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

        public override NetChannel Channel()
        {
            return NetChannel.Unreliable;
        }
    }

    public class TheTimeIsNow : MessageClass
    {
        public double Time = -1;
        public UInt64 ID = 0;

        public TheTimeIsNow()
        {
            Name = MessageClass.TheTimeIsNow;
        }

        public override NetBuffer Pack()
        {
            NetBuffer buffer = base.Pack();
            buffer.Write(ID);
            buffer.Write(Time);
            return buffer;
        }

        public override bool Unpack(ref NetBuffer buffer)
        {
            if (!base.Unpack(ref buffer))
                return false;

            ID = buffer.ReadUInt64();
            Time = buffer.ReadDouble();
            return true;
        }

        public override NetChannel Channel()
        {
            return NetChannel.Unreliable;
        }
    }
}
