﻿/*
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
using System.Windows.Forms;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using Math3D;

using Drawables.Cameras;
using Drawables.AnimateModels;

namespace SkinedModel
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new Visual().Run();
        }
    }
    class Visual : GameWindow
    {
        bool loaded = false;

        AnimatedModel model;

        AnimationHandler anim;
        Timer timer = new Timer();

        Camera camera;

        float bendAngle = 0;
        float bendDir = 1;

        float viewRot = 0;

        double viewTime = 0;

       
        void BuidMilkshapeModel ( string filename )
        {
            model = MS3DReader.Read(filename);
            model.Meshes[3].Show = false;
        }

        void BuildCal3dModel ( string folder )
        {
            DirectoryInfo dir = new DirectoryInfo(folder);
            if (!dir.Exists)
                return;
            List<string> meshFiles = new List<string>();
            foreach (FileInfo file in dir.GetFiles("*.cmf"))
                meshFiles.Add(file.FullName);

            string skelly = string.Empty;
            foreach (FileInfo file in dir.GetFiles("*.csf"))
                skelly = file.FullName;
         
            string anim = string.Empty;
            foreach (FileInfo file in dir.GetFiles("*.caf"))
                anim = file.FullName;

            model = Call3dReader.Read(meshFiles, skelly, anim);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SetupGL();
            BuidMilkshapeModel("../../jill.ms3d");
           // BuildCal3dModel("../../jill/");//"../../enkif/enkif.CMF", "../../enkif/enkif.CSF", "../../enkif/enkif_idle.CAF");
            anim = new AnimationHandler(model);

            anim.SetTime(0);
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
            GL.LightModel(LightModelParameter.LightModelColorControl, 1);

            GL.Enable(EnableCap.Light0);
            // setup light 0
            Vector4 lightInfo = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightInfo);

            lightInfo = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightInfo);
            GL.Light(LightName.Light0, LightParameter.Specular, lightInfo);

            loaded = true;

            camera = new Camera();
            camera.set(new Vector3(0, -3, 1.0f), 0, 90);

            OnResize(EventArgs.Empty);
        }

        protected virtual void SetViewPort()
        {
        }
       
        bool DoInput(FrameEventArgs e)
        {
            if (Keyboard[Key.Escape])
                return true;

            return false;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (DoInput(e))
                Exit();

            bendAngle += bendDir * 30 * (float)e.Time * 2;
            if (Math.Abs(bendAngle) > 30)
            {
                bendAngle = bendDir * 30;
                bendDir *= -1;
            }

            viewRot += (float)e.Time * 15;

            viewTime += e.Time;
            anim.SetTime(viewTime);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!loaded)
                return;

            GL.Viewport(0, 0, Width, Height); // Use all of the glControl painting area  
            camera.Resize(Width, Height);
        }

        protected void Grid()
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

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            camera.Execute();

            Grid();

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new Vector4(10, -15, 10, 1.0f));

            DrawModel();

            this.Title = "FPS: " + string.Format("{0:F}", 1.0 / e.Time);
            
            SwapBuffers();
        }

        private void DrawModel()
        {
            if (model == null)
                return;

            GL.PushMatrix();

            if (false)
            {
                GL.Rotate(90, 1, 0, 0);
                GL.Rotate(viewRot, 0, 1, 0);
            }
            else
            {
                GL.Rotate(viewRot, 0, 0, 1);
            }
            model.Draw(anim);

            GL.Disable(EnableCap.Lighting);
            model.DrawSkeliton(anim);
            GL.PopMatrix();
          }
    }
}
