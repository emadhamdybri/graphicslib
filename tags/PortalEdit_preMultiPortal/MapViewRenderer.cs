using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;

using OpenTK;
using OpenTK.Graphics;

using Drawables;
using Drawables.DisplayLists;

namespace PortalEdit
{
    public class MapViewRenderer
    {
        GLControl control;
        PortalMap map;

        Vector3 offset = new Vector3(0, 0, 1);
        Vector2 rotation = new Vector2(0, 0);
        float pullback = 5f;

        Point lastMouse = Point.Empty;

        Color SelectedVertColor = Color.FromArgb(128,Color.Magenta);

        ListableEvent GridList;

        public class CellClickedEventArgs : EventArgs
        {
            public CellClickedEventArgs( Cell c, int e, bool t, bool r, bool f)
            {
                cell = c;
                edge = e;
                topFace = t;
                roof = r;
                floor = f;
            }
            public Cell cell = null;
            public int edge = -1;
            public bool topFace = false;
            public bool roof = false;
            public bool floor = false;
        }

        public delegate void CellClickedEventHandler(object sender, CellClickedEventArgs e);
        public event CellClickedEventHandler CellClicked;

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
            ctl.MouseClick += new MouseEventHandler(ctl_MouseClick);

            GridList = new ListableEvent();
            GridList.Generate += new ListableEvent.GenerateEventHandler(GridList_Generate);
        }

        void ctl_MouseClick(object sender, MouseEventArgs e)
        {
            int selBufferSize = 20000;

            if (e.Button == MouseButtons.Left && CellClicked != null)
            {
                int[] viewport = new int[4];
                GL.GetInteger(GetPName.Viewport,viewport);

                int[] selectBuffer = new int[selBufferSize];
                GL.SelectBuffer(selBufferSize, selectBuffer);

                GL.RenderMode(RenderingMode.Select);
                GL.InitNames();
                GL.PushName(0xffffffff);

                GL.MatrixMode(MatrixMode.Projection);

                GL.PushMatrix();
                GL.LoadIdentity();
                Glu.PickMatrix((double)e.X, (double)(viewport[3] - e.Y), 2.0, 2.0, viewport);

                float aspect = (float)control.Width / (float)control.Height;
                Glu.Perspective(45 / aspect, aspect, 1f, 1000f);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();
                GL.LoadIdentity();
                SetCamera();

                // draw all the stuff with names
                Dictionary<int, CellClickedEventArgs> selectionArgs = new Dictionary<int,CellClickedEventArgs>();
                int name = 1;

                foreach(CellGroup group in map.cellGroups)
                {
                    foreach (EditorCell cell in group.Cells)
                    {
                        // do the floor
                        GL.LoadName(name);
                        selectionArgs.Add(name,new CellClickedEventArgs(cell,-1,false,false,true));
                        cell.floorList_Generate(null,null);
                        name++;

                        // do the roof
                        GL.LoadName(name);
                        selectionArgs.Add(name,new CellClickedEventArgs(cell,-1,false,true,false));
                        cell.roofList_Generate(null,null);
                        name++;

                        for ( int i = 0; i < cell.Edges.Count; i++ )
                        {
                            CellEdge edge = cell.Edges[i];

                            if (edge.EdgeType == CellEdgeType.Wall)
                            {
                                GL.LoadName(name);
                                selectionArgs.Add(name, new CellClickedEventArgs(cell, i, false, false, false));
                                cell.generateWall(edge);
                                name++;
                            }
                            else
                            {
                                if (cell.PortalHasBottomFace(edge))
                                {
                                    GL.LoadName(name);
                                    selectionArgs.Add(name, new CellClickedEventArgs(cell,i, false, false, false));
                                    cell.generatePortalBottomGap(edge);
                                    name++;
                                }

                                if (cell.PortalHasTopFace(edge))
                                {
                                    GL.LoadName(name);
                                    selectionArgs.Add(name, new CellClickedEventArgs(cell, i, true, false, false));
                                    cell.generatePortalTopGap(edge);
                                    name++;
                                }
                            }
                        }
                    }
                }

                // all the names are in and we are rendered
                GL.PopMatrix(); // pop off the camera

                GL.MatrixMode(MatrixMode.Projection);
                GL.PopMatrix(); // pop off the select matrix

                GL.Flush();
                int hits = GL.RenderMode(RenderingMode.Render);
                
                int selectedId = -1;
                uint closest = uint.MaxValue;

                for (int i = 0; i < hits; i++)
                {
                    uint distance = (uint)selectBuffer[i * 4 + 1];

                    if (closest >= distance)
                    {
                        closest = distance;
                        selectedId = (int)selectBuffer[i * 4 + 3];
                    }
                }

                if (!selectionArgs.ContainsKey(selectedId))
                    CellClicked(this, new CellClickedEventArgs(null, -1, false, false, false));
                else
                    CellClicked(this, selectionArgs[selectedId]);
            }
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

            SetPerspective();
        }

        void SetPerspective ( )
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            float aspect = (float)control.Width / (float)control.Height;
            Glu.Perspective(45 / aspect, aspect, 1f, 1000f);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        void SetOrthographic()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, control.Width, 0, control.Height,0,100);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        void GridList_Generate(object sender, DisplayList list)
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.LineSmooth);
            GL.PushMatrix();
            GL.Translate(0, 0, 0);
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

            GL.Color4(Color.FromArgb(128, Color.DarkGray));
            GL.LineWidth(1);
            GL.DepthMask(false);
            GL.Begin(BeginMode.Lines);

            float gridSize = Settings.settings.GridSize;

            for (float i = 0; i < gridSize; i += Settings.settings.GridSubDivisions)
            {
                if (i -(int)i < Settings.settings.GridSubDivisions)
                    continue;

                GL.Vertex2(gridSize, i);
                GL.Vertex2(-gridSize, i);

                GL.Vertex2(gridSize, -i);
                GL.Vertex2(-gridSize, -i);

                GL.Vertex2(i, gridSize);
                GL.Vertex2(i, -gridSize);

                GL.Vertex2(-i, gridSize);
                GL.Vertex2(-i, -gridSize);
            }

            GL.Color4(Color.FromArgb(128, Color.Gray));

            for (float i = 0; i < gridSize; i += 1f)
            {
                GL.Vertex2(gridSize, i);
                GL.Vertex2(-gridSize, i);

                GL.Vertex2(gridSize, -i);
                GL.Vertex2(-gridSize, -i);

                GL.Vertex2(i, gridSize);
                GL.Vertex2(i, -gridSize);

                GL.Vertex2(-i, gridSize);
                GL.Vertex2(-i, -gridSize);
            }

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

        protected void DrawSelectedVert ( )
        {
            CellVert vert = Editor.instance.GetSelectedVert();
            Cell cell = Editor.instance.GetSelectedCell();
            if (vert == null || cell == null)
                return;

            GL.Color4(SelectedVertColor);
            IntPtr quadric = Glu.NewQuadric();

            float markSize = 0.125f;

            GL.PushMatrix();
                GL.Translate(vert.Bottom);
                Glu.Sphere(quadric, markSize, 4, 2);
            GL.PopMatrix();

            GL.PushMatrix();
                GL.Translate(vert.Bottom.X, vert.Bottom.Y, vert.GetTopZ(cell.HeightIsIncremental));
                Glu.Sphere(quadric, markSize, 4, 2);
            GL.PopMatrix();

            Glu.DeleteQuadric(quadric);
        }

        void DrawMap()
        {
            if (map == null)
                return;

            DrawablesSystem.system.Execute();

            if (Editor.instance != null)
            {
                CellGroup group = Editor.instance.GetSelectedGroup();
                if ( group != null)
                {
                    foreach (EditorCell cell in group.Cells)
                        cell.DrawSelectionFrame();
                }
                else
                {
                    EditorCell selectedCell = Editor.instance.GetSelectedCell();
                    if (selectedCell != null)
                        selectedCell.DrawSelectionFrame();
                }

                DrawSelectedVert();
            }
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
                GL.Vertex3(size*2, 0, 0);

                GL.Color3(Color.Red);
                GL.Vertex3(0, -size, 0);
                GL.Vertex3(0, size*2, 0);

                GL.Color3(Color.Green);
                GL.Vertex3(0, 0, -size);
                GL.Vertex3(0, 0, size*2.5);
            GL.End();
            GL.LineWidth(1);
            SetPerspective();
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Texture2D);
        }

        public void Setup3dView ()
        {
            SetPerspective();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            SetCamera();
        }

        public void Render3dView()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Setup3dView();
            GL.PushMatrix();

            GL.Enable(EnableCap.Light0);
            Vector4 lightPos = new Vector4(10, 20, 20, 0);
            GL.Light(LightName.Light0, LightParameter.Position, lightPos);

            if (GridList != null)
                GridList.Call();           
            DrawMap();

            GL.PopMatrix();

            DrawOverlay();
            control.SwapBuffers();
        }
    }
}
