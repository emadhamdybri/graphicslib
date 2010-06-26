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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Drawables.DisplayLists;
using OpenTK;
using OpenTK.Graphics;

namespace ModelEditor
{
    public partial class ModelBaseDoc : Form
    {
        Vector3 offset = new Vector3(0, 0, 1);
        Vector2 rotation = new Vector2(0, 0);
        float pullback = 5f;

        protected DisplayListSystem DisplayLists = new DisplayListSystem();

        Point lastMouse = Point.Empty;
        ListableEvent GridList;

        protected bool loaded = false;

        protected bool ShowOriginIn3d = false;
        protected bool ShowOrientationOverlay = true;
        protected float GridSize = 25f;
        protected float GridSubDivisions = 0.25f;

        protected Color GridSubDivColor = Color.LightSlateGray;
        protected Color GridMainColor = Color.LightGray;
        protected Color GridAxisColor = Color.White;

        public ModelBaseDoc()
        {
            InitializeComponent();

            View.Paint += new PaintEventHandler(View_Paint);
            View.Resize += new EventHandler(View_Resize);
            View.MouseMove += new MouseEventHandler(View_MouseMove);
            View.MouseWheel += new MouseEventHandler(View_MouseWheel);
            View.MouseClick += new MouseEventHandler(View_MouseClick);
        }

        void View_MouseClick(object sender, MouseEventArgs e)
        {
        }

        void View_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomSpeed = 0.5f;
            if (Control.ModifierKeys == Keys.Shift)
                zoomSpeed *= 5f;

            pullback += ((float)e.Delta / 120f) * zoomSpeed;
            if (pullback < 0)
                pullback = 0;
            View_Paint(this, null);
        }

        void View_MouseMove(object sender, MouseEventArgs e)
        {
            float rotSpeed = 0.1f;
            float moveSpeed = 0.002f;

            if (e.Button != MouseButtons.None)
                Focus();

            if (e.Button == MouseButtons.Right)
            {
                rotation.X -= (e.X - lastMouse.X) * rotSpeed;
                rotation.Y += (e.Y - lastMouse.Y) * rotSpeed;
            }

            if (e.Button == MouseButtons.Middle)
            {
                float deg2Rad = (float)Math.PI / 180.0f;
                float x = 0;
                float y = 0;

                float XDelta = (e.X - lastMouse.X) * moveSpeed * pullback;
                float YDelta = (e.Y - lastMouse.Y) * moveSpeed * pullback;

                x = (float)Math.Cos(rotation.X * deg2Rad) * XDelta;
                y = -(float)Math.Sin(rotation.X * deg2Rad) * XDelta;

                offset.X -= x;
                offset.Y += y;


                x = -(float)Math.Cos((rotation.X + 90) * deg2Rad) * YDelta;
                y = (float)Math.Sin((rotation.X + 90) * deg2Rad) * YDelta;

                offset.X -= x;
                offset.Y += y;

            }

            View_Paint(this,null);

            lastMouse = new Point(e.X, e.Y);
        }

        void View_Resize(object sender, EventArgs e)
        {
            View.MakeCurrent();
            SetViewPort();
            View.Invalidate();
        }

        protected virtual void DrawView ()
        {
        }

        protected virtual void SetupView()
        {
        }

        public virtual string GetImportFilter()
        {
            return string.Empty;
        }

        public virtual string GetOpenFilter()
        {
            return string.Empty;
        }

        public virtual string GetSaveAsFilter()
        {
            return string.Empty;
        }

        public virtual bool Import ( FileInfo file )
        {
            return false;
        }

        protected void Redraw()
        {
            View.Invalidate();
        }

        void View_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;
            View.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Setup3dView();
            GL.PushMatrix();

            GL.Enable(EnableCap.Light0);
            Vector4 lightPos = new Vector4(10, 20, 20, 0);
            GL.Light(LightName.Light0, LightParameter.Position, lightPos);

            if (GridList != null)
                GridList.Call();
            DrawGridAxisMarker();

            DrawView();

            GL.PopMatrix();

            if(ShowOrientationOverlay)
                DrawOverlay();
            View.SwapBuffers();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            loaded = true;
            SetupGL();

            GridList = new ListableEvent(DisplayLists);
            GridList.Generate += new ListableEvent.GenerateEventHandler(GridList_Generate);
        }

        protected virtual void SetupGL()
        {
            View.MakeCurrent();
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

            SetupView();
        }

        protected virtual void SetViewPort()
        {
            if (!loaded)
                return;
            View.MakeCurrent();

            GL.Viewport(0, 0, View.Width, View.Height); // Use all of the glControl painting area  
            SetPerspective();
        }

        protected void SetPerspective()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            float aspect = (float)View.Width / (float)View.Height;

            Glu.Perspective(45, aspect, 1f, 1000f);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected void SetOrthographic()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, View.Width, 0, View.Height, 0, 100);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        public void Setup3dView()
        {
            SetPerspective();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            SetCamera();
        }

        void GridList_Generate(object sender, DisplayList list)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.LineSmooth);
            GL.DepthMask(false);

            GL.PushMatrix();
            GL.Translate(0, 0, 0);

            GL.Color4(Color.FromArgb(128, GridSubDivColor));
            GL.LineWidth(1);
            GL.Begin(BeginMode.Lines);

            for (float i = 0; i < GridSize; i += GridSubDivisions)
            {
                if (i - (int)i < GridSubDivisions)
                    continue;

                GL.Vertex2(GridSize, i);
                GL.Vertex2(-GridSize, i);

                GL.Vertex2(GridSize, -i);
                GL.Vertex2(-GridSize, -i);

                GL.Vertex2(i, GridSize);
                GL.Vertex2(i, -GridSize);

                GL.Vertex2(-i, GridSize);
                GL.Vertex2(-i, -GridSize);
            }
            GL.End();
            GL.LineWidth(2);
            GL.Begin(BeginMode.Lines);
            GL.Color4(Color.FromArgb(128, GridMainColor));

            for (float i = 0; i < GridSize; i += 1f)
            {
                GL.Vertex2(GridSize, i);
                GL.Vertex2(-GridSize, i);

                GL.Vertex2(GridSize, -i);
                GL.Vertex2(-GridSize, -i);

                GL.Vertex2(i, GridSize);
                GL.Vertex2(i, -GridSize);

                GL.Vertex2(-i, GridSize);
                GL.Vertex2(-i, -GridSize);
            }
            GL.End();

            GL.LineWidth(3);
            GL.Begin(BeginMode.Lines);
            GL.Color3(GridAxisColor);
            GL.Vertex3(-GridSize, 0, 0);
            GL.Vertex3(GridSize, 0, 0);
            GL.Vertex3(0, -GridSize, 0);
            GL.Vertex3(0, GridSize, 0);
            GL.End();

            GL.Enable(EnableCap.LineSmooth);
            GL.PopMatrix();
            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);
        }

        void DrawOverlay()
        {
            SetOrthographic();
            GL.LoadIdentity();
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);

            float size = 15;

            GL.Begin(BeginMode.Polygon);
            GL.Color4(Color.FromArgb(128, Color.Gray));
            GL.Vertex3(0, 0, -50);
            GL.Vertex3(60, 0, -50);
            GL.Vertex3(60, 40, -50);
            GL.Vertex3(40, 60, -50);
            GL.Vertex3(0, 60, -50);
            GL.End();

            GL.Translate(25, 20, -25);
            GL.Rotate(rotation.Y, 1.0f, 0.0f, 0.0f);					// pops us to the tilt
            GL.Rotate(-rotation.X, 0.0f, 1.0f, 0.0f);					// gets us on our rot
            GL.Rotate(-90, 1.0f, 0.0f, 0.0f);							// gets us into XY

            GL.LineWidth(2);
            GL.Begin(BeginMode.Lines);

            GL.Color3(Color.Blue);
            GL.Vertex3(-size, 0, 0);
            GL.Vertex3(size * 2, 0, 0);

            GL.Color3(Color.Red);
            GL.Vertex3(0, -size, 0);
            GL.Vertex3(0, size * 2, 0);

            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, -size);
            GL.Vertex3(0, 0, size * 2.5);
            GL.End();
            GL.LineWidth(1);
            SetPerspective();
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Texture2D);
        }

        protected void DrawGridAxisMarker()
        {
            if (!ShowOriginIn3d)
                return;

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.LineSmooth);

            GL.LineWidth(2);
            GL.Begin(BeginMode.Lines);

            GL.Color3(Color.Blue);
            GL.Vertex3(-1, 0, 0);
            GL.Vertex3(2, 0, 0);

            GL.Color3(Color.Red);
            GL.Vertex3(0, -1, 0);
            GL.Vertex3(0, 2, 0);

            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, -1);
            GL.Vertex3(0, 0, 4);
            GL.End();

            GL.Enable(EnableCap.LineSmooth);
            GL.PopMatrix();
            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);
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
    }
}
