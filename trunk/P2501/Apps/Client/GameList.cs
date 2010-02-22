using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace P2501Client
{
    class GameList : IDisposable
    {
        UInt64 UID = 0;

        List<string> GameServers = new List<string>();
        bool dirty = false;

        Thread worker = null;

        public bool Dirty
        {
            get
            {
                lock (GameServers)
                {
                    return dirty; 
                }
            }
        }

        public void Dispose()
        {
            Kill();
        }

        public void Kill ()
        {
            if (worker == null)
                return;

            lock (GameServers)
            {
                worker.Abort();
                worker = null;
            }
        }

        public GameList ( UInt64 id )
        {
            UID = id;

            worker = new Thread(new ThreadStart(Poll));
            worker.Start();
        }

        void Poll ()
        {
            lock (GameServers)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.awesomelaser.com/p2501/List/list.php?action=viewlist&uid=" + UID.ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                GameServers.Clear();

                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);

                int count = int.Parse(reader.ReadLine());
                for (int i = 0; i < count; i++)
                    GameServers.Add(reader.ReadLine());

                dirty = true;
            }

            Thread.Sleep(120 * 1000);
        }

        public List<string> GetGameServers ()
        {
            List<string> newList = new List<string>();
            lock(GameServers)
            {
                foreach (string g in GameServers)
                    newList.Add(g);
               
                dirty = false;
            }

            return newList;
        }
    }
}
