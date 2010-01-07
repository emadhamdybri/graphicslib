using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Project23Client;
using Project23Server;

using Simulation;

using OpenTK;
using OpenTK.Input;

using Math3D;

namespace Project23
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

    class InputRedirector
    {
        public virtual bool Update ()
        {
            return true;
        }
    }

    class Game : GameWindow
    {
        Server server;
        public GameClient Client;

        Visual visual;

        public ChatLog Chat = new ChatLog();

        ConnectionInfo connectionInfo;

        KeyboardHandler keyHandler; // shitty class to manage keyboard since openTK punted on keyboard support;
        KeyManager keys;

        Vector2 mousePos = new Vector2(0, 0);

        public InputRedirector InputRedirect;

        public string OutgoingChatString = string.Empty;

        public bool Joined
        {
            get { if (Client == null) return false; return Client.ThisPlayer != null; }
        }

        public bool Connected
        {
            get { if (Client == null) return false; return Client.ConnectedToHost; }
        }

        public Game( ConnectionInfo c) : base (1024,550,OpenTK.Graphics.GraphicsMode.Default,"Project23",GameWindowFlags.Default)
        {
            this.Icon = Resource1.Project23;

            connectionInfo = c;
            keyHandler = new KeyboardHandler(this);
            keys = Settings.settings.Keys;
            keys.SetKeyboard(keyHandler);

            VSync = VSyncMode.Off;
            Chat.CurrentChannel = ChatLog.GeneralChatChannel;
            Chat.AddMessage(ChatLog.GeneralChatChannel,string.Empty, "Project23 Client Startup");
        }

        protected void ClientServerVersion ( object sender, int version )
        {
            Chat.AddMessage(ChatLog.GeneralChatChannel, string.Empty, "Server Version " + version.ToString());
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
            Chat.AddMessage(ChatLog.GeneralChatChannel, string.Empty, "Graphics Loaded");
            visual.Hud.SetStatusText("Connecting to host " + connectionInfo.Hostname + "(" + connectionInfo.Port.ToString() + ")...");
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
                server.Run();
                Chat.AddMessage(ChatLog.GeneralChatChannel, string.Empty, "Starting self-serv host on port " + connectionInfo.Port.ToString());
            }

            Client = new GameClient(connectionInfo.Hostname, connectionInfo.Port);
            Client.GetAuthentication = new AuthenticationCallback(GetAuth);
            Client.GetJoinInfo = new JoinInfoCallback(GetJoin);
            Client.HostConnectionEvent += new HostConnectionHandler(Client_HostConnectionEvent);

            Client.ServerVersionEvent += new ServerVersionHandler(ClientServerVersion);
            Client.sim.PlayerJoined += new PlayerJoinedHandler(PlayerJoined);
            Client.sim.PlayerStatusChanged += new PlayerStatusChangeHandler(sim_PlayerStatusChanged);

            Client.AllowSpawnEvent += new PlayerEventHandler(Client_AllowSpawnEvent);
            Client.ChatReceivedEvent += new ChatEventHandler(Client_ChatReceivedEvent);

            Mouse.WheelChanged += new EventHandler<MouseWheelEventArgs>(Mouse_WheelChanged);
            Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonDown);
            Mouse.Move += new EventHandler<MouseMoveEventArgs>(Mouse_Move);

        }

        void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            mousePos.X = e.X - Width / 2.0f;
            mousePos.Y = e.Y - Height / 2.0f;
        }

        void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Client.ThisPlayer != null && Client.ThisPlayer.Status == PlayerStatus.Despawned)
            {
                visual.Hud.SetStatusText(string.Empty);
                Client.RequestSpawn();
            }
        }

        void Client_AllowSpawnEvent(object sender, Player player)
        {
            player.Status = PlayerStatus.Despawned;
            visual.Hud.SetStatusText("Click to Spawn");
        }

        void sim_PlayerStatusChanged(object sender, PlayerEventArgs args)
        {
            if (args.player.Status == PlayerStatus.Alive)
            {
                // do a spawn event for the graphical dealy
            }
        }

        void Client_HostConnectionEvent(object sender, string error)
        {
            visual.Hud.SetStatusText("Connected to host...");
        }

        void Mouse_WheelChanged(object sender, MouseWheelEventArgs e)
        {
            visual.ZoomView(-e.DeltaPrecise);
        }

        void Client_ChatReceivedEvent(object sender, string channel, string from, string message)
        {
            Chat.AddMessage(channel, from, message);
        }

        protected void ClientPlayerJoined ( object sender, Player player )
        {
            if (player == Client.ThisPlayer)
            {
                connectionInfo.Callsign = player.Callsign;
                Chat.AddMessage(ChatLog.CombatChatChannel,"Simulation", "Sim Initialized");
                visual.Hud.SetStatusText("Joined game...");
            }
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
            if (keys.Keydown(KeyEvent.Quit))
                Exit();

            if (keys.Keydown(KeyEvent.Chat1))
                Chat.CurrentChannel = ChatLog.GeneralChatChannel;
            else if (keys.Keydown(KeyEvent.Chat2) && Chat.ChannelCount() > 1)
                Chat.CurrentChannel = Chat.GetChannelNames()[1];
            else if (keys.Keydown(KeyEvent.Chat3) && Chat.ChannelCount() > 2)
                Chat.CurrentChannel = Chat.GetChannelNames()[2];
            else if (keys.Keydown(KeyEvent.Chat4) && Chat.ChannelCount() > 3)
                Chat.CurrentChannel = Chat.GetChannelNames()[3];

            if (InputRedirect != null)
            {
                keyHandler.FlushKeys();
                if (InputRedirect.Update())
                    InputRedirect = null;
                return;
            }

            if (keys.Keydown(KeyEvent.StartChat))
                InputRedirect = new ChatInputHandler(this, keyHandler);

            if (keys.Keydown(KeyEvent.PlayerList))
                visual.Hud.TogglePlayerList();

            //mouse pos into angle
            Vector2 mouseVec = new Vector2(mousePos);
            mouseVec.Normalize();
            float ang = Math3D.Trig.RadianToDegree((float)Math.Atan2(mouseVec.Y, mouseVec.X));

            float delta = 


            keyHandler.FlushKeys();
        }

        class ChatInputHandler : InputRedirector
        {
            bool done = false;
            Game game;
            KeyboardHandler keyboard;

            public ChatInputHandler ( Game g, KeyboardHandler k )
            {
                keyboard = k;
                game = g;
                keyboard.FlushText();
                keyboard.TextMode = true;
                game.OutgoingChatString = keyboard.CurrentText;
                keyboard.TextModeNewline += new NewLineHandler(keyboard_TextModeNewline);
                game.visual.Hud.SetChatMode(true);
            }

            void keyboard_TextModeNewline(object sender, Key key)
            {
                if (done)
                    return;

                keyboard.TextModeNewline -= keyboard_TextModeNewline;

                game.Client.SendChat(ChatLog.GeneralChatChannel, string.Copy(keyboard.CurrentText));
                keyboard.FlushKeys();
                keyboard.FlushText();
                keyboard.TextMode = false;
                game.OutgoingChatString = string.Empty;
                done = true;
                game.visual.Hud.SetChatMode(false);
            }

            public override bool Update()
            {
                if (!done)
                    game.OutgoingChatString = keyboard.CurrentText;
                return done;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            CheckInput();

            Client.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            visual.Render(Client.Time, e.Time);
            SwapBuffers();
        }
    }
}
