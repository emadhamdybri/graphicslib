using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Project23Server;

namespace CLIServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 2501;
            if (args.Length > 0)
                port = int.Parse(args[0]);

            Server server = new Server(port);

            while (true)
            {
                server.Update();
                Thread.Sleep(10);
            }
        }
    }
}
