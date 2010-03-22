/*
    Open Combat/Projekt 2501
    Copyright (C) 2010  Jeffery Allen Myers

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Drawables.Cameras;
using Drawables.Textures;

namespace LightmapTest
{
    class Visual : GameWindow
    {
        public Camera camera;

        Point lastMousePos = Point.Empty;
        Point thisMousePos = new Point(0, 0);

        bool loaded = false;
        bool rightDown = false;

        Texture lightmap,surface;

        public override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            camera = new Camera();

            SetupGL();
        }

        void SetupGL()
        {
            GL.ClearColor(System.Drawing.Color.LightBlue);
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

            camera.FOV = 30f;
            camera.set(new Vector3(-5, 0, 0), 0, 0);

            loaded = true;

            Mouse.Move += new EventHandler<MouseMoveEventArgs>(Mouse_Move);
            Mouse.ButtonDown += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonDown);
            Mouse.ButtonUp += new EventHandler<MouseButtonEventArgs>(Mouse_ButtonUp);
            OnResize(EventArgs.Empty);

            surface = TextureSystem.system.FromImage(Resource1.picture);
            lightmap = TextureSystem.system.FromImage(Resource1.Lightmap);
           // lightmap.Luminance = true;
        }

        protected virtual void SetViewPort()
        {
        }

        void Mouse_ButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
                rightDown = false;
        }

        void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
                rightDown = true;
        }

        public void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            if (lastMousePos == Point.Empty)
                lastMousePos = new Point(e.X, e.Y);

            thisMousePos.X = +e.XDelta;
            thisMousePos.Y = +e.YDelta;
        }

        bool DoInput (FrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
                return true;

            float turnSpeed = 40.0f;
            turnSpeed *= (float)e.Time;

            float sensitivity = 0.1f;

            if (rightDown)
                camera.turn(-turnSpeed * sensitivity * thisMousePos.Y, -turnSpeed * sensitivity * thisMousePos.X);
            thisMousePos.X = 0;
            thisMousePos.Y = 0;

            Vector3 movement = new Vector3();

            float speed = 5.0f;
            speed *= (float)e.Time;

            if (Keyboard[Key.A])
                movement.X = speed;
            if (Keyboard[Key.D])
                movement.X = -speed;
            if (Keyboard[Key.W])
                movement.Y = speed;
            if (Keyboard[Key.S])
                movement.Y = -speed;

            if (Keyboard[Key.PageUp])
                movement.Z = speed;
            if (Keyboard[Key.PageDown])
                movement.Z = -speed;
            
            if (Keyboard[Key.A] || Keyboard[Key.S] || Keyboard[Key.D] || Keyboard[Key.W] || Keyboard[Key.PageUp] || Keyboard[Key.PageDown])
                camera.MoveRelitive(movement);

            return false;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (DoInput(e))
                Exit();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!loaded)
                return;
           
            GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area  
            camera.Resize(Width, Height);
        }

        protected void Grid ()
        {
            GL.Color4(Color.DarkBlue);
            GL.Begin(BeginMode.Lines);

            for (int i = -10; i <= 10; i++)
            {
                GL.Vertex3(i, 10, 0);
                GL.Vertex3(i, -10, 0);
                GL.Vertex3(10, i, 0);
                GL.Vertex3(-10, i, 0);
            }

            GL.End();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            if (!loaded)
                return;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            camera.Execute();
            GL.MatrixMode(MatrixMode.Texture);
            GL.LoadIdentity();

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Color4(Color.Red);
            GL.Begin(BeginMode.Lines);

            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 1);
            GL.End();
            Grid();

            bool ortho = false;
           
            if (ortho)
            {
                camera.SetOrthographic();
                GL.Translate(Width / 2, Height / 2, -10);
            }

            GL.Disable(EnableCap.Lighting);

            GL.Color4(Color.White);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Enable(EnableCap.Texture2D);
            surface.Execute();

            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Modulate);
               
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.Enable(EnableCap.Texture2D);
            lightmap.Execute();
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Combine);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.CombineRgb, (int)TextureEnvModeCombine.AddSigned);

            float size = 150;
            if (!ortho)
            {
                size = 2;
                GL.Rotate(-90, 0, 1, 0);
                GL.Rotate(-90, 0, 0, 1);
            }
            GL.Begin(BeginMode.Quads);
           // if (ortho)
            {
                GL.MultiTexCoord2(TextureUnit.Texture0, 0, 1);
                GL.MultiTexCoord2(TextureUnit.Texture1, 0, 1);
                GL.Vertex3(-size, -size, 0);

                GL.MultiTexCoord2(TextureUnit.Texture0, 1, 1);
                GL.MultiTexCoord2(TextureUnit.Texture1, 1, 1);
                GL.Vertex3(size, -size, 0);

                GL.MultiTexCoord2(TextureUnit.Texture0, 1, 0);
                GL.MultiTexCoord2(TextureUnit.Texture1, 1, 0);
                GL.Vertex3(size, size, 0);

                GL.MultiTexCoord2(TextureUnit.Texture0, 0, 0);
                GL.MultiTexCoord2(TextureUnit.Texture1, 0, 0);
                GL.Vertex3(-size, size, 0);
            }

            GL.End();

            SwapBuffers();
        }
    }
}
