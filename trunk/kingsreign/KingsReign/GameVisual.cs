using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace KingsReign
{
    public class GameVisual
    {
        protected GLControl Control;

        public GameVisual ( GLControl ctl )
        {
            Control = ctl;

            SetupGL();
            Control.Paint += new System.Windows.Forms.PaintEventHandler(Control_Paint);
            Control.Resize += new EventHandler(Control_Resize);
        }

        void Control_Resize(object sender, EventArgs e)
        {
            Control.MakeCurrent();
            SetViewPort();
            Control.Invalidate();
        }

        void Control_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Draw();
        }

        protected void Redraw()
        {
            Control.Invalidate();
        }

        protected virtual void SetupGL()
        {
            Control.MakeCurrent();
            GL.ClearColor(Color.SkyBlue);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.DepthTest);

            // setup light 0
            Vector4 lightInfo = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightInfo);

            lightInfo = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightInfo);
            GL.Light(LightName.Light0, LightParameter.Specular, lightInfo);

            SetViewPort();
        }

        protected virtual void SetViewPort()
        {
            Control.MakeCurrent();

            GL.Viewport(0, 0, Control.Width, Control.Height); // Use all of the glControl painting area  
            SetOrthographic();
        }

        protected void SetOrthographic()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Control.Width, 0, Control.Height, 0, 100);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected void Draw ()
        {
            Control.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.PushMatrix();

            GL.Enable(EnableCap.Light0);
            Vector4 lightPos = new Vector4(10, 20, 20, 0);
            GL.Light(LightName.Light0, LightParameter.Position, lightPos);

            GL.PopMatrix();

            Control.SwapBuffers();
        }
    }
}
