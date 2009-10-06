﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

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

      
        public KeyboardDevice   Keyboard;
        public MouseDevice      Mouse;

        public class UpdateFrameArgs : EventArgs
        {
            private double timeDelta;
            private double time;

            /// <summary>
            /// Gets the Time elapsed between frame updates, in seconds.
            /// </summary>
            public double Time
            {
                get { return time; }
                internal set { time = value; }
            }

            public double TimeDelta
            {
                get { return timeDelta; }
                internal set { timeDelta = value; }
            }

        }

        public class RenderFrameArgs : EventArgs
        {
            private double time;
            private double timeDelta;
            private double scale_factor;

            /// <summary>
            /// Gets the Time elapsed between frame updates, in seconds.
            /// </summary>
            public double Time
            {
                get { return time; }
                internal set { time = value; }
            }

            public double TimeDelta
            {
                get { return timeDelta; }
                internal set { timeDelta = value; }
            }

            public double ScaleFactor
            {
                get
                {
                    return scale_factor;
                }
                internal set
                {
                    if (value != 0.0 && !Double.IsNaN(value))
                        scale_factor = value;
                    else
                        scale_factor = 1.0;
                }
            }
        }

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

        public void Exit ()
        {
            Application.Exit();
        }

        protected void InitEvents ()
        {
            Keyboard = new KeyboardDevice(glControl1);
            Mouse = new MouseDevice(glControl1);
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
            Resized(e);
            if (!FixedFrameRate)
                Draw();
        }

        protected override void OnResize(EventArgs e)
        {
            glControl1.Location = new Point(0, 0);
            glControl1.Size = this.Size;
            SetViewPort();
            base.OnResize(e);
            Resized(e);
            if (!FixedFrameRate)
                Draw();
        }
        protected override void OnResizeBegin(EventArgs e)
        {
            glControl1.Location = new Point(0, 0);
            glControl1.Size = this.Size;
            SetViewPort();
            base.OnResizeBegin(e);
            Resized(e);
            if (!FixedFrameRate)
                Draw();
        }

        protected virtual void Resized (EventArgs e)
        {

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

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            double now = stopwatch.ElapsedMilliseconds * 0.001f;

            RenderFrameArgs args = new RenderFrameArgs();
            args.Time = now;
            args.TimeDelta = now - lastFrameTime;
            OnRenderFrame(args);

            lastFrameTime = now;

        }

        public void SwapBuffers ()
        {
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

        public virtual void OnUpdateFrame(UpdateFrameArgs e)
        {
        }

        public virtual void OnRenderFrame(RenderFrameArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            SwapBuffers();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Loaded)
                return;

            double now = stopwatch.ElapsedMilliseconds * 0.001f;

            UpdateFrameArgs args = new UpdateFrameArgs();
            args.Time = now;
            args.TimeDelta = now - lastUpdateTime;
            OnUpdateFrame(args);

            lastUpdateTime = now;
            if (FixedFrameRate && !Focused)
                Draw();
        }
    }
}