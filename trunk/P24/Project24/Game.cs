using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project24Client;
using Project24Server;

using Simulation;

using OpenTK;
using OpenTK.Input;

namespace Project24
{
    class Game : GameWindow
    {
        Server server;
        GameClient client;

        Visual visual;

        public ChatLog Chat = new ChatLog();

        public bool Connected
        {
            get { if (client == null) return false; return client.ConnectedToHost; }
        }

        public Game() : base (1024,550)
        {
            VSync = VSyncMode.Off;
            Chat.CurrentChannel = ChatLog.GeneralChatChannel;
            Chat.AddMessage(ChatLog.GeneralChatChannel, "Project24 Client Startup");
        }

        protected void ClientServerVersion ( object sender, int version )
        {
            Chat.AddMessage(ChatLog.GeneralChatChannel, "Server Version " + version.ToString());
        }

        protected void PlayerJoined ( object sender, PlayerEventArgs args )
        {
           // args.player.Tag = something graphical;
        }

        protected override void OnLoad(EventArgs e)
        {
            visual = new Visual(this);
            visual.Setup();
            Start(string.Empty, 0, true);
            visual.Resize(Width, Height);
            Chat.AddMessage(ChatLog.GeneralChatChannel, "Graphics Loaded");
            Chat.AddMessage("Custom", "Create Channel");
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
                Chat.AddMessage(ChatLog.GeneralChatChannel, "Starting selfserv host on port " + port.ToString());
            }

            client = new GameClient(host, port);
            client.GetAuthentication = new AuthenticationCallback(GetAuth);

            client.ServerVersionEvent += new ServerVersionHandler(ClientServerVersion);
            client.sim.PlayerJoined += new PlayerJoinedHandler(PlayerJoined);
        }

        protected void GetAuth(ref string username, ref string token)
        {
            username = "player1";
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (visual != null)
                visual.Resize(Width, Height);
        }

        protected override void OnWindowInfoChanged(EventArgs e)
        {
            base.OnWindowInfoChanged(e);

            visual.Resize(Width, Height);
        }

        protected void CheckInput()
        {
            if (Keyboard[Key.F1])
                Chat.CurrentChannel = ChatLog.GeneralChatChannel;
            if (Keyboard[Key.F2])
                Chat.CurrentChannel = "Custom";
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (server != null)
                server.Update(e.Time);

            CheckInput();

            client.Update(e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            visual.Render(e.Time);
            SwapBuffers();
        }
    }
}
