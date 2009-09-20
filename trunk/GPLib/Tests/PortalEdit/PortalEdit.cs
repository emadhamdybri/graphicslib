using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;

namespace PortalEdit
{
    public partial class EditFrame : Form
    {
        MapRenderer renderer;
        bool GLLoaded = false;
        PortalMap map;

        public EditFrame()
        {
            InitializeComponent();
            renderer = new MapRenderer(MapView);
            map = new PortalMap(renderer);

            renderer.MouseStatusUpdate += new MouseStatusUpdateHandler(renderer_MouseStatusUpdate);
        }

        void renderer_MouseStatusUpdate(object sender, Point position)
        {
            MousePosition.Text = "Map:" + position.ToString();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GLView_Paint(this, e);
            base.OnPaint(e);
        }

        private void GLView_Paint(object sender, PaintEventArgs e)
        {
            Render3dView();
        }

        private void Render3dView (  )
        {
            if (!GLLoaded || map == null)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // do 3d camera

            // do grid

            // draw map
            map.Draw();

            GLView.SwapBuffers();
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate(true);
            base.OnResize(e);
            Render3dView();
        }

        private void GLView_Resize(object sender, EventArgs e)
        {
            SetViewPort();
            Invalidate(true);
            Render3dView();
        }

        protected virtual void SetupGL()
        {
            SetViewPort();
            GL.ClearColor(Color.SkyBlue);
        }

        protected virtual void SetViewPort()
        {
            int w = GLView.Width;
            int h = GLView.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area  
        }

        private void EditFrame_Load(object sender, EventArgs e)
        {
             if (GLLoaded)
                 return;
 
             GLLoaded = true;
             SetupGL();
        }
    }
}
