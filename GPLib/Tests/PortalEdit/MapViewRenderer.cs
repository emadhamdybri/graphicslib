using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;

using Drawables.Cameras;

namespace PortalEdit
{
    public class MapViewRenderer
    {
        GLControl control;
        PortalMap map;

        Camera camera = new Camera();

        Point lastMouse = Point.Empty;

        public MapViewRenderer( GLControl ctl, PortalMap m )
        {
            map = m;
            control = ctl;
            SetupGL();
            camera.set(new Vector3(1, 1, 2), 0, 0);
            
            SetViewPort();
            Render3dView();

            ctl.Paint += new System.Windows.Forms.PaintEventHandler(ctl_Paint);
            ctl.Resize += new EventHandler(ctl_Resize);
            ctl.MouseMove += new MouseEventHandler(ctl_MouseMove);
        }

        void ctl_MouseMove(object sender, MouseEventArgs e)
        {
            float rotSpeed = 0.1f;
            float moveSpeed = 0.5f;

            if (e.Button == MouseButtons.Right)
            {
                camera.turn((e.Y - lastMouse.Y) * rotSpeed, (e.X - lastMouse.X) * rotSpeed);
                Render3dView();
            }

            if (e.Button == MouseButtons.Middle)
            {
                camera.move(0,(e.Y - lastMouse.Y) * moveSpeed, (e.X - lastMouse.X) * moveSpeed);
                Render3dView();
            }

            lastMouse = new Point(e.X,e.Y);
        }

        void ctl_Resize(object sender, EventArgs e)
        {
            SetViewPort();
            Render3dView();
        }

        void ctl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Render3dView();
        }

        protected virtual void SetupGL()
        {
            GL.ClearColor(Color.Wheat);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.LineSmooth);
          //  GL.LightModel(LightModelParameter.LightModelColorControl, 1);

            // setup light 0
//             Vector4 lightInfo = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
//             GL.Light(LightName.Light0, LightParameter.Ambient, lightInfo);
// 
//             lightInfo = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
//             GL.Light(LightName.Light0, LightParameter.Diffuse, lightInfo);
//             GL.Light(LightName.Light0, LightParameter.Specular, lightInfo);

        }

        protected virtual void SetViewPort()
        {
            GL.Viewport(0, 0, control.Width, control.Height); // Use all of the glControl painting area  
            if (camera != null)
                camera.Resize(control.Width, control.Height);
        }

        protected void DrawGrid ()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);

            GL.Begin(BeginMode.Lines);

            GL.Color3(Color.Red);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(10, 0, 0);

            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 10, 0);

            GL.Color3(Color.Blue);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 20);

            GL.End();
        }

        public void Render3dView()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            if (camera != null)
            {
               // do 3d camera
                camera.Execute();

                // do grid
                DrawGrid();

                // draw map
                if (map == null)
                    return;
                map.Draw();
            }
 
            control.SwapBuffers();
        }

    }
}
