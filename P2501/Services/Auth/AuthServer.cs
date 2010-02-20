using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.Threading;

using Hosts;
using ServerConfigurator;
using MySql.Data.MySqlClient;
using Lidgren.Network;

namespace Auth
{
    class AuthServer
    {
        protected ServerConfig config;
        protected MySqlConnection database;

        protected CryptoHost host;

        Dictionary<NetConnection, UInt64> ConnectedUsers = new Dictionary<NetConnection, UInt64>();

        public AuthServer ( string configFile )
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
                    connection = host.GetPentConnection();
                }

                connection = host.GetPentDisconnection();
                while (connection != null)
                {
                    ConnectedUsers.Remove(connection);
                    connection = host.GetPentDisconnection();
                }

                Message msg = host.GetPentMessage();

                while (msg != null)
                {

                    msg = host.GetPentMessage();
                }

                Thread.Sleep(100);
            }
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

        protected UInt64 AuthUser ( string username, string password )
        {
            checkDatabase();
            List<UInt64> characterList = new List<UInt64>();

            String query = String.Format("SELECT ID, PassHash FROM users WHERE UserName is @name");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@name", username));

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
