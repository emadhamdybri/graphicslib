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

using GUI;
using GUI.UI;

using Drawables.Cameras;

using Utilities.Paths;

namespace GUITest
{
    class View : GameWindow
    {
        protected Screen screen = new Screen();

        protected Camera camera = new Camera();

        public View() : base(1024, 550)
        {
            VSync = VSyncMode.Off;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

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

            // setup light 0
            Vector4 lightInfo = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightInfo);

            lightInfo = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightInfo);
            GL.Light(LightName.Light0, LightParameter.Specular, lightInfo);

            camera.set(new Vector3(1, 1, 2), 0, 0);

            GL.Viewport(0, 0, Width, Height);
            screen.Resize(Width, Height);
            camera.Resize(Width, Height);

            SetupScreen();
        }

        void SetupScreen()
        {
            ResourceManager.AddPath("./resources");
            ResourceManager.AddPath("../resources");
            ResourceManager.AddPath("../../resources");
            ResourceManager.AddPath("../../../resources");

            Pannel pannel = new Pannel();

            pannel.FrameColor = Color.White;
            pannel.BackgroundColor = Color.DarkGray;
            pannel.Size = new Vector2(300, 300);
            pannel.Position = new Vector2(100, 100);
            screen.AddChild(pannel);

            Label label = new Label("Label");
            label.Position = new Vector2(10,50);
            label.FrameColor = Color.White;

            pannel.AddChild(label);

            GUI.UI.Image img = new GUI.UI.Image("exit.png");
            img.AnchorLeft = false;
            img.AnchorRight = true;
            img.AnchorTop = true;
            img.AnchorBottom = false;

            img.Position = new Vector2(300 - img.Size.X, 300 - img.Size.Y);
            pannel.AddChild(img);
        }

        protected override void OnWindowInfoChanged(EventArgs e)
        {
            base.OnWindowInfoChanged(e);

            GL.Viewport(0, 0, Width, Height);
            screen.Resize(Width, Height);
            camera.Resize(Width, Height);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            
            GL.Viewport(0, 0, Width, Height);
            screen.Resize(Width, Height);
            camera.Resize(Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            screen.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);

            camera.Execute();

            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.Disable(EnableCap.Lighting);
            camera.SetOrthographic();

            screen.Draw();

            camera.SetPersective();
            SwapBuffers();
        }
    }
}
