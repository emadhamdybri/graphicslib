using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using OpenTK;
using OpenTK.Graphics;

namespace GUIGameWindow
{
    public partial class GUIGameWindowBase : Form
    {
        bool Loaded = false;

        protected bool FixedFrameRate = false;
        protected int LogicUpdateTime = 16;
       
        Stopwatch stopwatch = new Stopwatch();
        double lastUpdateTime = 0;
        double lastFrameTime = 0;

        public GUIGameWindowBase()
        {
            InitializeComponent();
            InitEvents();
        }

        public GUIGameWindowBase(int width, int height)
        {
            InitializeComponent();
            this.Size = new Size(width, height);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            InitEvents();
        }

        protected void InitEvents ()
        {
            Application.Idle += new EventHandler(OnApplicationIdle);
        }

        public void AddControll ( GUIFormBase form )
        {
            Control child = form.GetRootPannel(this);
            child.Parent = glControl1;
            child.Show();
        }

        private void OnApplicationIdle(object sender, EventArgs e)
        {
            if (!FixedFrameRate)
            {
                while (Loaded && glControl1.IsIdle)
                    Draw();
            }
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            glControl1.Location = new Point(0, 0);
            glControl1.Size = this.Size;
            SetViewPort();
            base.OnResizeEnd(e);
            if (!FixedFrameRate)
                Draw();
        }

        protected override void OnResize(EventArgs e)
        {
            glControl1.Location = new Point(0, 0);
            glControl1.Size = this.Size;
            SetViewPort();
            base.OnResize(e);
            if (!FixedFrameRate)
                Draw();
        }
        protected override void OnResizeBegin(EventArgs e)
        {
            glControl1.Location = new Point(0, 0);
            glControl1.Size = this.Size;
            SetViewPort();
            base.OnResizeBegin(e);
            if (!FixedFrameRate)
                Draw();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!FixedFrameRate)
                Draw();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            glControl1_Load(this, e);
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            if (Loaded)
                return;

            timer1.Interval = LogicUpdateTime;
            timer1.Start();

            Loaded = true;
            SetupGL();
            InitGame();
            stopwatch.Start();
            lastUpdateTime = lastFrameTime = stopwatch.ElapsedMilliseconds * 0.001f;
            OnResizeEnd(EventArgs.Empty);
        }

        private void Draw()
        {
            foreach (Control child in glControl1.Controls)
                child.Invalidate(true);

            if (!Loaded)
                return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            double now = stopwatch.ElapsedMilliseconds * 0.001f;

            DrawMainView(now, now - lastFrameTime);

            lastFrameTime = now;

            glControl1.SwapBuffers();
        }

        protected virtual void SetViewPort()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area  
        }

        protected virtual void SetupGL()
        {
            SetViewPort();
            GL.ClearColor(Color.SkyBlue);
        }

        protected virtual void InitGame()
        {
        }

        protected virtual void UpdateGame(double time, double delta)
        {
        }

        protected virtual void DrawMainView(double time, double delta)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Loaded)
                return;

            double now = stopwatch.ElapsedMilliseconds * 0.001f;

            UpdateGame(now, now - lastUpdateTime);
            lastUpdateTime = now;
            if (FixedFrameRate && !Focused)
                Draw();
        }
    }
}
