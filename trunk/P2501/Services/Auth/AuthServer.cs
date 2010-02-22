using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.Threading;
using System.Web;

using Hosts;
using ServerConfigurator;
using MySql.Data.MySqlClient;
using Lidgren.Network;

using Auth;

namespace AuthServer
{
    class AuthHost
    {
        protected ServerConfig config;
        protected MySqlConnection database;

        protected CryptoHost host;

        Dictionary<NetConnection, UInt64> ConnectedUsers = new Dictionary<NetConnection, UInt64>();

        public AuthHost(string configFile)
        {
            FileInfo file = new FileInfo(configFile);
            config = new ServerConfig(configFile);
            if (!file.Exists)
                ConfigDefaults();
        }

        protected void Init ()
        {
            string connStr = String.Format("server={0};user id={1}; password={2}; database=mysql; pooling=false",
            config.GetItem("AuthDatabaseHost"), config.GetItem("AuthDatabaseUser"), config.GetItem("AuthDatabasePassword"));

            database = new MySqlConnection(connStr);
            try
            {
                database.Open();
            }
            catch (System.Exception ex)
            {
                database.Close();
                database = null;
            }

            if (database != null && database.State != ConnectionState.Open)
            {
                database.Close();
                database = null;
            }
            if (database != null)
                database.ChangeDatabase(config.GetItem("AuthDatabase"));
            else
                Console.WriteLine("Connection to db failed");

            int port = 4111;
            if (config.ItemExists("AuthServePort"))
                port = int.Parse(config.GetItem("AuthServePort"));

            host = new CryptoHost(port);
        }

        public void Run()
        {
            Init();

            if (database == null)
                return;

            bool done = false;
            while (!done)
            {
                NetConnection connection = host.GetPentConnection();
                while (connection != null)
                {
                    ConnectedUsers.Add(connection, 0);

                    SendSingleCode(connection,AuthMessage.Hail);
                    connection = host.GetPentConnection();
                }

                connection = host.GetPentDisconnection();
                while (connection != null)
                {
                    ConnectedUsers.Remove(connection);
                    connection = host.GetPentDisconnection();
                }

                Message message = host.GetPentMessage();

                while (message != null)
                {
                    if (message.Name == AuthMessage.RequestAdd)
                        AddUser(message);
                    else if (message.Name == AuthMessage.RequestAuth)
                        AuthUser(message);
                    else if (message.Name == AuthMessage.RequestCharacterList)
                        CharacterList(message);
                    else if (message.Name == AuthMessage.RequestAddCharacter)
                        AddCharacter(message);

                    message = host.GetPentMessage();
                }

                Thread.Sleep(100);
            }
        }

        protected void SendSingleCode ( NetConnection user, int code)
        {
            NetBuffer buffer = new NetBuffer();
            buffer.Write(code);
            host.SendMessage(user, buffer, NetChannel.ReliableInOrder1);
        }

        protected void Send ( NetConnection user, AuthMessage message )
        {
            host.SendMessage(user, message.Pack(), message.Channel());
        }

        protected void AddCharacter ( Message msg )
        {
            RequestAddCharacter data = new RequestAddCharacter();
            data.Unpack(ref msg.Data);

            if (data.callsign == string.Empty || data.callsign.Length < 2 || CharacterExists(data.callsign))
            {
                SendSingleCode(msg.Sender, AuthMessage.CharacterAddBadName);
                return;
            }

            UInt64 id = GetID(msg.Sender);
            if (id == 0)
            {
                SendSingleCode(msg.Sender, AuthMessage.CharacterAddBadNoAuth);
                return;
            }

            checkDatabase();
            String query = String.Format("INSERT INTO characters (UID, Callsign) VALUES (@uid,@callsign)");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@uid", id));
            command.Parameters.Add(new MySqlParameter("@callsign", data.callsign));

            command.ExecuteNonQuery();

            SendSingleCode(msg.Sender, AuthMessage.CharacterAddOK);
        }

        protected void AddUser ( Message msg )
        {
            RequestAdd data = new RequestAdd();
            data.Unpack(ref msg.Data);

            if (data.email == string.Empty || !data.email.Contains("@") || AccountExists(data.email))
            {
                SendSingleCode(msg.Sender, AuthMessage.AddBadEmail);
                return;
            }

            if (data.callsign == string.Empty || data.callsign.Length < 2 || CharacterExists(data.callsign))
            {
                SendSingleCode(msg.Sender, AuthMessage.AddBadCallsign);
                return;
            }

            if (data.password == string.Empty || data.callsign.Length < 2)
            {
                SendSingleCode(msg.Sender, AuthMessage.AddBadPass);
                return;
            }

            Random rand = new Random();
            UInt64 token = (UInt64)rand.Next();

            MD5 md5 = MD5.Create();
            byte[] inputHash = md5.ComputeHash(new ASCIIEncoding().GetBytes(data.password));

            string inputHashString = "";
            foreach (byte b in inputHash)
                inputHashString += b.ToString("x2");

            String query = String.Format("INSERT INTO users (EMail, PassHash, Verified, Token) VALUES (@email,@hash,0,@token)");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@email", data.email));
            command.Parameters.Add(new MySqlParameter("@hash", inputHashString));
            command.Parameters.Add(new MySqlParameter("@token", token));

            command.ExecuteNonQuery();

            query = String.Format("SELECT ID FROM users WHERE Token=@token");
            command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@token", token));

            MySqlDataReader reader = command.ExecuteReader();

            UInt64 id = 0;
            if (reader.Read())
                id = reader.GetUInt64(0);
            reader.Close();

            if (id == 0)
            {
                SendSingleCode(msg.Sender, AuthMessage.AddBadEmail);
                return;
            }

            query = String.Format("INSERT INTO characters (UID, Callsign) VALUES (@uid,@callsign)");
            command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@uid", id));
            command.Parameters.Add(new MySqlParameter("@callsign", data.callsign));

            command.ExecuteNonQuery();

            SendSingleCode(msg.Sender,AuthMessage.AddOK);
        }

        protected void AuthUser ( Message msg )
        {
            RequestAuth data = new RequestAuth();
            data.Unpack(ref msg.Data);

            UInt64 ID = LoginUser(data.email, data.password);
            if (ID == 0)
            {
                SendSingleCode(msg.Sender, AuthMessage.AuthBadCred);
                return;
            }

            ConnectedUsers[msg.Sender] = ID;

            Send(msg.Sender, new AuthOK(ID, GenerateToken(ID)));
        }

        protected UInt64 GetID ( NetConnection con )
        {
            if (!ConnectedUsers.ContainsKey(con))
                return 0;

            return ConnectedUsers[con];
        }

        protected void CharacterList( Message msg )
        {
            UInt64 id = GetID(msg.Sender);
            if (id == 0)
            {
                SendSingleCode(msg.Sender, AuthMessage.CharacterListBadNoAuth);
                return;
            }

            checkDatabase();
            List<UInt64> characterList = new List<UInt64>();

            String query = String.Format("SELECT ID, Callsign FROM characters WHERE UID=@id");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@id", id));

            MySqlDataReader reader = command.ExecuteReader();

            CharacterList list = new CharacterList();
            while (reader.Read())
                list.Characters.Add(new CharacterList.CharacterInfo(reader.GetString(1), reader.GetUInt64(0)));
            reader.Close();

            Send(msg.Sender, list);
        }

        protected void checkDatabase()
        {
            if (database.State != ConnectionState.Open)
            {
                database.Open();
                database.ChangeDatabase(config.GetItem("AuthDatabase"));
            }
        }

        protected void ConfigDefaults()
        {
            config.SetItem("AuthDatabaseHost", "localhost");
            config.SetItem("AuthDatabaseUser", "username");
            config.SetItem("AuthDatabasePassword", "password");
            config.SetItem("AuthDatabase", "database");

            config.Save();
        }

        protected UInt64 GenerateToken ( UInt64 UID )
        {
            Random rand = new Random();
            UInt64 token = (UInt64)rand.Next();

            String query = String.Format("UPDATE users SET Token=@Token WHERE ID=@UID");
            MySqlCommand  command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@UID", UID));
            command.Parameters.Add(new MySqlParameter("@Token", token));

            command.ExecuteNonQuery();

            return token;
        }

        protected bool AccountExists(string email)
        {
            checkDatabase();
            List<UInt64> characterList = new List<UInt64>();

            String query = String.Format("SELECT ID FROM users WHERE EMail=@email");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@email", email));

            MySqlDataReader reader = command.ExecuteReader();

            UInt64 id = 0;
            if (reader.Read())
                id = reader.GetUInt64(0);

            reader.Close();

            return id !=0;
        }

        protected bool CharacterExists(string character)
        {
            checkDatabase();
            List<UInt64> characterList = new List<UInt64>();

            String query = String.Format("SELECT ID FROM characters WHERE Callsign=@character");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@character", character));

            MySqlDataReader reader = command.ExecuteReader();

            UInt64 id = 0;
            if (reader.Read())
                id = reader.GetUInt64(0);

            reader.Close();

            return id != 0;
        }

        protected UInt64 LoginUser ( string email, string password )
        {
            checkDatabase();
            List<UInt64> characterList = new List<UInt64>();

            String query = String.Format("SELECT ID, PassHash FROM users WHERE EMail=@email");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@email", email));

            MySqlDataReader reader = command.ExecuteReader();

            UInt64 id = 0;
            if (reader.Read())
                id = reader.GetUInt64(0);

            if (id > 0)
            {
                MD5 md5 = MD5.Create();
                byte[] inputHash = md5.ComputeHash(new ASCIIEncoding().GetBytes(password));

                string inputHashString = "";
                foreach (byte b in inputHash)
                    inputHashString += b.ToString("x2");

                if (inputHashString != reader.GetString(1))
                    id = 0;
            } 
            
            reader.Close();

            return id;
        }
    }
}
