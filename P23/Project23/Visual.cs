using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Simulation;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Drawables.Cameras;

using Project23Client;

namespace Project23
{
    class VisualPlayer
    {
        public Player player;
        public PlayerModelInstance model;
    }

    class Visual
    {
        Game game;
        TopCam camera;

        Cloudscape clouds = null;

        Dictionary<Player, VisualPlayer> playerVisuals = new Dictionary<Player, VisualPlayer>();

        PlayerModel shipModel1;

        public static float ViewZoomIncrement = 0.5f;
        public static float ViewZoomMax = 200;
        public static float ViewZoomMin = 15;
        float ViewZoom = ViewZoomMax;

        public HudRenderer Hud;
        public Visual ( Game g )
        {
            game = g;
            Setup();
        }

        public void Setup ()
        {
            GL.ClearColor(System.Drawing.Color.SkyBlue);
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.LineSmooth);
            GL.LightModel(LightModelParameter.LightModelColorControl, 1);

            GL.Enable(EnableCap.Light0);
            // setup light 0
            Vector4 lightInfo = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightInfo);

            lightInfo = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightInfo);
            GL.Light(LightName.Light0, LightParameter.Specular, lightInfo);

            camera = new TopCam();
            camera.Zoom(ViewZoom);

            clouds = new Cloudscape(new Vector2(50, 50));

            Hud = new HudRenderer(game);

            game.Client.sim.PlayerJoined += new PlayerJoinedHandler(sim_PlayerJoined);
            game.Client.sim.PlayerRemoved += new PlayerRemovedHandler(sim_PlayerRemoved);
            game.Client.AllowSpawnEvent += new PlayerEventHandler(Client_AllowSpawnEvent);

            shipModel1 = new PlayerModel(ResourceManager.FindDirectory("ships/1/"));
        }

        void Client_AllowSpawnEvent(object sender, Player player)
        {
            Hud.SetPlayerData(player);
        }

        void sim_PlayerRemoved(object sender, PlayerEventArgs args)
        {
            playerVisuals.Remove(args.player);
        }

        void sim_PlayerJoined(object sender, PlayerEventArgs args)
        {
            if (playerVisuals.ContainsKey(args.player))
                return;
            VisualPlayer vis = new VisualPlayer();
            vis.player = args.player;
            vis.model = new PlayerModelInstance(shipModel1);
            playerVisuals.Add(args.player, vis);
        }

        public void ZoomView ( float dir )
        {
            ViewZoom += dir * ViewZoomIncrement;
            if (ViewZoom > ViewZoomMax)
                ViewZoom = ViewZoomMax;
            else if (ViewZoom < ViewZoomMin)
                ViewZoom = ViewZoomMin;
            else
                camera.Zoom(dir*ViewZoomIncrement);
        }

        public void Resize(int Width, int Height)
        {
            GL.Viewport(0, 0, Width, Height);
            camera.Resize((float)Width, (float)Height);

            Hud.Resize(Width, Height);
        }

        protected void RenderWorld(double time)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);

            if (game.Client.ThisPlayer != null && game.Client.ThisPlayer.Status == PlayerStatus.Alive)
            {
                clouds.Render(time, game.Client.ThisPlayer.CurrentState.Movement  * -0.5f);
            }
            else
            {
                clouds.Render(time, Vector3.Zero);
            }
            
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);

            GL.Begin(BeginMode.Lines);

            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1, 0, 0);

            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 1, 0);
           
            GL.Color3(Color.Blue);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 1);

            GL.Color4(0,0,0,0.25f);

            for (int x = -2000; x <= 2000; x += 25)
            {
                for (int y = -2000; y <= 2000; y += 25)
                {
                    GL.Vertex3(x - 0.5f, y, 0);
                    GL.Vertex3(x + 0.5f, y, 0);
                    GL.Vertex3(x, y - 0.5f, 0);
                    GL.Vertex3(x, y + 0.5f, 0);
                }
            }
            GL.End();

            // do the world crap
        }

        protected void RenderPlayers ( double time )
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);

            GL.Color4(Color.White);

            GL.Enable(EnableCap.Texture2D);

            foreach( KeyValuePair<Player,VisualPlayer> player in playerVisuals)
            {
                if (player.Key.Status == PlayerStatus.Alive)
                    player.Value.model.Draw(time, player.Key, game.Client.sim);
            }
        }

        public void Render ( double time, double delta )
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.PushMatrix();

            if (game.Client.ThisPlayer != null)
                camera.SetXY(game.Client.ThisPlayer.CurrentState.Position);

            camera.Execute();
            GL.Light(LightName.Light0, LightParameter.Position, new Vector4(10, 15, 10, 1.0f));

            RenderWorld(delta);
            RenderPlayers(delta);

            GL.PopMatrix();

            GL.PushMatrix();
            GL.Disable(EnableCap.Lighting);

            camera.Orthographic();
            Hud.Render(delta);
            camera.Perspective();
            GL.PopMatrix();

        }
    }
}
