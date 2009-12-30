using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project24Client;
using Project24Server;

using OpenTK;

namespace Project24
{
    class Game : GameWindow
    {
        Server server;
        GameClient client;

        Visual visual;

        public bool Connected
        {
            get { if (client == null) return false; return client.ConnectedToHost; }
        }

        public Game() : base (1024,550)
        {
            VSync = VSyncMode.Off;
        }

        protected override void OnLoad(EventArgs e)
        {
            visual = new Visual(this);
            visual.Setup();
            Start(string.Empty, 0, true);
            visual.Resize(Width, Height);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            client.Kill();
            if (server != null)
                server.Kill();
        }

        public void Start ( string host, int port, bool selfServe )
        {
            if (selfServe)
            {
                host = "localhost";
                port = new Random().Next(1000) + 2500;
                server = new Server(port);
            }

            client = new GameClient(host, port);
            client.GetAuthentication = new AuthenticationCallback(GetAuth);
        }

        protected void GetAuth(ref string username, ref string token)
        {
            username = "player1";
        }

        protected override void OnWindowInfoChanged(EventArgs e)
        {
            base.OnWindowInfoChanged(e);

            visual.Resize(Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (server != null)
                server.Update(e.Time);

            client.Update(e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            visual.Render(e.Time);
            SwapBuffers();
        }
    }
}
