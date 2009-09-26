using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

using OpenTK;
using OpenTK.Graphics;

using Drawables;

namespace PortalEdit
{
    public class MapViewRenderer
    {
        GLControl control;
        PortalMap map;

        Vector3 offset = new Vector3(0, 0, 1);
        Vector2 rotation = new Vector2(0, 0);
        float pullback = 50f;

        Point lastMouse = Point.Empty;

        public MapViewRenderer(GLControl ctl, PortalMap m)
        {
            map = m;
            control = ctl;
            SetupGL();

            SetViewPort();
            Render3dView();

            ctl.Paint += new System.Windows.Forms.PaintEventHandler(ctl_Paint);
            ctl.Resize += new EventHandler(ctl_Resize);
            ctl.MouseMove += new MouseEventHandler(ctl_MouseMove);
            ctl.MouseWheel += new MouseEventHandler(ctl_MouseWheel);
        }

        void ctl_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomSpeed = 0.5f;
            pullback += ((float)e.Delta / 120f) * zoomSpeed;
            if (pullback < 0)
                pullback = 0;
            Render3dView();
        }

        void ctl_MouseMove(object sender, MouseEventArgs e)
        {
            float rotSpeed = 0.1f;
            float moveSpeed = 0.05f;

            if (e.Button == MouseButtons.Right)
            {
                rotation.X -= (e.X - lastMouse.X) * rotSpeed;
                rotation.Y += (e.Y - lastMouse.Y) * rotSpeed;
            }

            if (e.Button == MouseButtons.Middle)
            {
                offset.X -= (e.X - lastMouse.X) * moveSpeed;
                offset.Y += (e.Y - lastMouse.Y) * moveSpeed;
            }
            Render3dView();

            lastMouse = new Point(e.X, e.Y);
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
            GL.ClearColor(Color.LightSkyBlue);

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
        }

        protected virtual void SetViewPort()
        {
            GL.Viewport(0, 0, control.Width, control.Height); // Use all of the glControl painting area  

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            float aspect = (float)control.Width / (float)control.Height;
            Glu.Perspective(45 / aspect, aspect, 1f, 1000f);
        }

        protected void DrawGrid()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.LineSmooth);
            GL.PushMatrix();
            GL.Translate(0, 0, -0.1f);
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

            GL.Color4(Color.FromArgb(128, Color.Gray));

            for (float i = 0; i < 100f; i += 5f)
            {
                GL.Vertex2(100, i);
                GL.Vertex2(-100, i);

                GL.Vertex2(100, -i);
                GL.Vertex2(-100, -i);

                GL.Vertex2(i, 100);
                GL.Vertex2(i, -100);

                GL.Vertex2(-i, 100);
                GL.Vertex2(-i, -100);
            }

            GL.End();
            GL.Enable(EnableCap.LineSmooth);
            GL.PopMatrix();
            GL.Enable(EnableCap.Lighting);
        }

        protected void SetCamera()
        {
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(0, 0, -pullback);								// pull back on allong the zoom vector
            GL.Rotate(rotation.Y, 1.0f, 0.0f, 0.0f);					// pops us to the tilt
            GL.Rotate(-rotation.X, 0.0f, 1.0f, 0.0f);					// gets us on our rot
            GL.Translate(-offset.X, -offset.Z, offset.Y);	// take us to the pos
            GL.Rotate(-90, 1.0f, 0.0f, 0.0f);							// gets us into XY
        }

        void DrawMap()
        {
            if (map == null)
                return;

            DrawablesSystem.system.Execute();

            if (Editor.instance != null)
            {
                EditorCell selectedCell = Editor.instance.GetSelectedCell();
                if (selectedCell != null)
                    selectedCell.DrawSelectionFrame();
            }
        }

        public void Render3dView()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            SetCamera();

            GL.Enable(EnableCap.Light0);
            Vector4 lightPos = new Vector4(10, 20, 20, 0);
            GL.Light(LightName.Light0, LightParameter.Position, lightPos);

             DrawGrid();           
             DrawMap();
            control.SwapBuffers();
        }
    }
}
