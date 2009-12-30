using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Drawables.Cameras;

namespace Project24
{
    class Visual
    {
        Game game;
        Camera camera;

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
            camera.set(new Vector3(0, 0, 10), -90, 0);
        }

        public void Resize(int Width, int Height)
        {
            GL.Viewport(0, 0, Width, Height);
            camera.Resize(Width, Height);
        }

        public void RenderWorld(double time)
        {
            if (game.Connected)
            {

            }
        }

        public void RenderHud(double time)
        {

        }

        public void Render ( double time)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.PushMatrix();
            camera.Execute();
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);

            RenderWorld(time);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);

            camera.SetOrthographic();
            RenderHud(time);
            GL.PopMatrix();

        }
    }
}
