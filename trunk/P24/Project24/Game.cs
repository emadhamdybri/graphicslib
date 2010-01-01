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
    class ConnectionInfo
    {
        public string Avatar = "Pilot0u";
        public string Username = "Developer";
        public string Token = string.Empty;
        public string Callsign = string.Empty;
        public string Hostname = string.Empty;
        public int Port = 0;
        public bool SelfServ = true;
    }

    class Game : GameWindow
    {
        Server server;
        public GameClient Client;

        Visual visual;

        public ChatLog Chat = new ChatLog();

        ConnectionInfo connectionInfo;

        public bool Joined
        {
            get { if (Client == null) return false; return Client.ThisPlayer != null; }
        }

        public bool Connected
        {
            get { if (Client == null) return false; return Client.ConnectedToHost; }
        }

        public Game( ConnectionInfo c) : base (1024,550,OpenTK.Graphics.GraphicsMode.Default,"Project24",GameWindowFlags.Default)
        {
            connectionInfo = c;

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
            Start();

            visual = new Visual(this);
            visual.Setup();
            visual.Resize(Width, Height);
            Chat.AddMessage(ChatLog.GeneralChatChannel, "Graphics Loaded");
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            Client.Kill();
            if (server != null)
                server.Kill();
        }

        public void Start ()
        {
            if (connectionInfo.SelfServ)
            {
                connectionInfo.Hostname = "localhost";
                connectionInfo.Port = new Random().Next(1000) + 2500;
                server = new Server(connectionInfo.Port);
                Chat.AddMessage(ChatLog.GeneralChatChannel, "Starting self-serv host on port " + connectionInfo.Port.ToString());
            }

            Client = new GameClient(connectionInfo.Hostname, connectionInfo.Port);
            Client.GetAuthentication = new AuthenticationCallback(GetAuth);

            Client.ServerVersionEvent += new ServerVersionHandler(ClientServerVersion);
            Client.sim.PlayerJoined += new PlayerJoinedHandler(PlayerJoined);
            Client.LocalPlayerJoinedEvent += new PlayerEventHandler(LocalPlayerJoined);

            Client.GetJoinInfo = new JoinInfoCallback(GetJoin);
        }

        protected void LocalPlayerJoined ( object sender, Player player )
        {
            // trigger something to say we can spawn maybe?
            connectionInfo.Callsign = player.Callsign;
        }

        protected void GetJoin(ref string callsign, ref string pilot)
        {
            callsign = connectionInfo.Username;
            pilot = connectionInfo.Avatar;
        }

        protected void GetAuth(ref string username, ref string token)
        {
            username = connectionInfo.Username;
            token = connectionInfo.Token;
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

            Client.Update(e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            visual.Render(e.Time);
            SwapBuffers();
        }
    }
}
