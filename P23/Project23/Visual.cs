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
    class Visual
    {
        Game game;
        Camera camera;

        Cloudscape clouds = null;

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

            camera = new Camera();
            camera.set(new Vector3(0, 0, 25), 90, 90);

            clouds = new Cloudscape(new Vector2(50, 50));

            Hud = new HudRenderer(game);

            game.Client.LocalPlayerJoinedEvent += new PlayerEventHandler(LocalPlayerJoined);
        }

        public void LocalPlayerJoined ( object sender, Player player )
        {
            Hud.SetPlayerData(player);
        }

        public void Resize(int Width, int Height)
        {
            GL.Viewport(0, 0, Width, Height);
            camera.Resize(Width, Height);
        }

        public void RenderWorld(double time)
        {
            camera.Execute();
            if (game.Connected)
            {
                clouds.Render(time, Vector3.UnitX);
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
            GL.End();
        }

        public void Render ( double time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.PushMatrix();
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);

            RenderWorld(time);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Disable(EnableCap.Lighting);

            camera.SetOrthographic();
            Hud.Render(time);
            camera.SetPersective();
            GL.PopMatrix();

        }
    }
}
