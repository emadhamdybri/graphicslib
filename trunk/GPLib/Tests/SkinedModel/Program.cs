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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SetupGL();
            BuidMilkshapeModel("../../jill.ms3d");
            anim = new AnimationHandler(model);
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
            camera.set(new Vector3(0, -2, 0.0f), 0, 90);

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

            Title = RenderFrequency.ToString();
            

            SwapBuffers();
        }

        private void DrawModel()
        {
            if (model == null)
                return;

            GL.PushMatrix();

            GL.Rotate(90, 1, 0, 0);

            GL.Rotate(viewRot, 0, 1, 0);
            model.Draw(anim);

           // GL.Disable(EnableCap.Lighting);

          //  model.DrawSkeliton(anim);
            GL.PopMatrix();
          }
    }
}
