using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Text;
using System.IO;

using OpenTK;
using OpenTK.Graphics;

using Drawables;
using Drawables.DisplayLists;
using Drawables.Textures;

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

        Texture underlay;
        Vector2 underlayCenter;
        float   underlayScale;

        public bool ShowUnderlay = true;

        public class CellClickedEventArgs : EventArgs
        {
            public CellClickedEventArgs(Cell c, int e, CellWallGeometry g, bool r, bool f)
            {
                cell = c;
                edge = e;
                geo = g;
                roof = r;
                floor = f;
            }
            public Cell cell = null;
            public int edge = -1;
            public CellWallGeometry geo = null;
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

            Editor.instance.MapLoaded += new MapLoadedHandler(MapLoaded);
        }

        public void UnloadMapGraphics ( )
        {
            DrawablesSystem.system.removeAll();
            DisplayListSystem.system.Flush();
            if(underlay != null)
            {
                underlay.Invalidate();
                underlay = null;
            }
        }

        void MapLoaded(object sender, EventArgs args)
        {
            CheckUnderlay();
        }

        public void CheckUnderlay ()
        {
            if (underlay != null)
            {
                underlay.Invalidate();
                underlay = null;
            }

            string imageFile = MapImageSetup.GetMapUnderlayImage(map);

            if (imageFile != string.Empty && File.Exists(imageFile))
            {
                underlay = TextureSystem.system.GetTexture(imageFile);
                if (underlay != null)
                {
                    underlayCenter = MapImageSetup.GetMapUnderlayCenter(map);
                    underlayScale = 1f/MapImageSetup.GetMapUnderlayPPU(map);
                }
            }
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

                foreach(CellGroup group in map.CellGroups)
                {
                    foreach (EditorCell cell in group.Cells)
                    {
                        // do the floor
                        GL.LoadName(name);
                        selectionArgs.Add(name,new CellClickedEventArgs(cell,-1,null,false,true));
                        cell.floor.GenerateGeo(null,null);
                        name++;

                        // do the roof
                        GL.LoadName(name);
                        selectionArgs.Add(name, new CellClickedEventArgs(cell, -1, null, true, false));
                        cell.roof.GenerateGeo(null, null);
                        name++;

                        for ( int i = 0; i < cell.Edges.Count; i++ )
                        {
                            CellEdge edge = cell.Edges[i];

                            foreach (CellWallGeometry geo in edge.Geometry)
                            {
                                WallGeometry wallGeo = cell.FindWallGeo(i, geo);
                                if (wallGeo != null)
                                {
                                    GL.LoadName(name);
                                    selectionArgs.Add(name, new CellClickedEventArgs(cell, i, geo, false, false));
                                    wallGeo.GenerateGeo(null, null);
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
                    CellClicked(this, new CellClickedEventArgs(null, -1, null, false, false));
                else
                    CellClicked(this, selectionArgs[selectedId]);
            }
        }

        void ctl_MouseWheel(object sender, MouseEventArgs e)
        {
            float zoomSpeed = 0.5f;
            if (Control.ModifierKeys == Keys.Shift)
                zoomSpeed *= 5f;

            pullback += ((float)e.Delta / 120f) * zoomSpeed;
            if (pullback < 0)
                pullback = 0;
            Render3dView();
        }

        void ctl_MouseMove(object sender, MouseEventArgs e)
        {
            float rotSpeed = 0.1f;
            float moveSpeed = 0.002f;

            if (e.Button == MouseButtons.Right)
            {
                rotation.X -= (e.X - lastMouse.X) * rotSpeed;
                rotation.Y += (e.Y - lastMouse.Y) * rotSpeed;
            }

            if (e.Button == MouseButtons.Middle)
            {
                float deg2Rad = (float)Math.PI/180.0f;
                float x = 0;
                float y = 0;

                float XDelta = (e.X - lastMouse.X) * moveSpeed * pullback;
                float YDelta = (e.Y - lastMouse.Y) * moveSpeed * pullback;

                x = (float)Math.Cos(rotation.X * deg2Rad) * XDelta;
                y = -(float)Math.Sin(rotation.X * deg2Rad) * XDelta;

                offset.X -= x;
                offset.Y += y;


                x = -(float)Math.Cos((rotation.X+90) * deg2Rad) * YDelta;
                y = (float)Math.Sin((rotation.X + 90) * deg2Rad) * YDelta;

                offset.X -= x;
                offset.Y += y;

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
            GL.Enable(EnableCap.LineSmooth);
            GL.DepthMask(false);

            float gridSize = Settings.settings.GridSize;
           
            GL.PushMatrix();
            GL.Translate(0, 0, 0);

            GL.Color4(Color.FromArgb(128, Color.LightSlateGray));
            GL.LineWidth(1);
            GL.Begin(BeginMode.Lines);

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
            GL.End();
            GL.LineWidth(2);
            GL.Begin(BeginMode.Lines);
            GL.Color4(Color.FromArgb(128, Color.LightGray));

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

            GL.LineWidth(3);
            GL.Begin(BeginMode.Lines);
            GL.Color3(Color.Snow);
            GL.Vertex3(-gridSize, 0, 0);
            GL.Vertex3(gridSize, 0, 0);
            GL.Vertex3(0, -gridSize, 0);
            GL.Vertex3(0, gridSize, 0);
            GL.End();

            GL.Enable(EnableCap.LineSmooth);
            GL.PopMatrix();
            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);
        }

        protected void DrawGridAxisMarker ()
        {
            if (!Settings.settings.Show3dOrigin)
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

        bool DrawGroupSelection ( CellGroup group )
        {
            if (group == null)
                return false;

            foreach (EditorCell cell in group.Cells)
                cell.DrawSelectionFrame();

            return true;
        }

        bool DrawCellSelection ( EditorCell cell )
        {
            if (cell == null)
                return false;
            cell.DrawSelectionFrame();
            return true;
        }

        bool DrawCellFlatSelections(EditorCell cell)
        {
            Editor editor = Editor.instance;

            if (cell == null)
                return false;

            if (editor.GetFloorSelection())
            {
                cell.DrawFloorSelectionFrame();
                return true;
            }

            if (editor.GetRoofSelection())
            {
                cell.DrawRoofSelectionFrame();
                return true;
            }

            return false;
        }

        bool DrawGeoSelection ( Cell cell, CellEdge edge, CellWallGeometry geo )
        {
            if (cell == null || edge == null || geo == null)
                return false;

            GL.DepthMask(false);
            GL.DepthFunc(DepthFunction.Always);

            GL.Color4(EditorCell.selectionColor);
            GL.LineWidth(EditorCell.selectedLineWidht);
            GL.Begin(BeginMode.LineLoop);

            GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, geo.LowerZ[0]);
            GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y, geo.LowerZ[1]);
            GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y, geo.UpperZ[1]);
            GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, geo.UpperZ[0]);

            GL.End();
            GL.LineWidth(1);

            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);

            return true;
        }

        void DrawSelections ()
        {
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.Texture2D);

            Editor editor = Editor.instance;

            if (editor == null)
                return;

            if (Settings.settings.ShowLowestSelection)
            {
                if (!DrawGeoSelection(editor.GetSelectedCell(), editor.GetSelectedEdge(), editor.GetSelectedWallGeo()))
                {
                    if (!DrawCellFlatSelections(Editor.instance.GetSelectedCell()))
                    {
                        if (!DrawCellSelection(Editor.instance.GetSelectedCell()))
                            DrawGroupSelection(editor.GetSelectedGroup());
                    }
                }
            }
            else // draw them all
            {
                if (!DrawGroupSelection(editor.GetSelectedGroup()))
                    DrawCellSelection(Editor.instance.GetSelectedCell());

                DrawGeoSelection(editor.GetSelectedCell(), editor.GetSelectedEdge(), editor.GetSelectedWallGeo());
            } 

            // we always draw the vert, as it's minor
            DrawSelectedVert();

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Texture2D);
        }

        void DrawMap()
        {
            if (map == null)
                return;

            DrawablesSystem.system.Execute();
            DrawSelections();
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

        public void DrawBasePlane ()
        {
            GL.Disable(EnableCap.Lighting);
            GL.DepthMask(false);
            GL.DepthFunc(DepthFunction.Always);

            float gridSize = Settings.settings.GridSize;

            GL.Color3(Color.FromArgb(255, 0x13, 0x48, 0x72));

            GL.Begin(BeginMode.Quads);
            GL.Vertex3(-gridSize, -gridSize, 0);
            GL.Vertex3(gridSize, -gridSize, 0);
            GL.Vertex3(gridSize, gridSize, 0);
            GL.Vertex3(-gridSize, gridSize, 0);
            GL.End();

            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
            GL.Disable(EnableCap.Lighting);
        }

        public void DrawUnderlay()
        {
            if (underlay != null && ShowUnderlay)
            {
                GL.Disable(EnableCap.Lighting);
                GL.Color4(Color.FromArgb((int)(255*Settings.settings.Underlay3DAlpha),Color.White));
                GL.Enable(EnableCap.Texture2D);

                if (!Settings.settings.ShowUnderlayWithDepth)
                {
                    GL.DepthMask(false);
                    GL.DepthFunc(DepthFunction.Always);

                }
                else
                {
                    GL.Enable(EnableCap.PolygonOffsetFill);
                    GL.PolygonOffset(10, -10);
                }

                underlay.Execute();

                GL.PushMatrix();
                GL.Translate(underlayCenter.X, underlayCenter.Y, 0);

                float x = underlay.Width * underlayScale * 0.5f;
                float y = underlay.Height * underlayScale * 0.5f;

                GL.Begin(BeginMode.Quads);

                GL.Normal3(0, 0, 1);

                GL.TexCoord2(0, 1);
                GL.Vertex3(-x, -y, 0);

                GL.TexCoord2(1, 1);
                GL.Vertex3(x, -y, 0);

                GL.TexCoord2(1, 0);
                GL.Vertex3(x, y, 0);

                GL.TexCoord2(0, 0);
                GL.Vertex3(-x, y, 0);

                GL.End();
                GL.PopMatrix();

                GL.Disable(EnableCap.Texture2D);

                GL.Disable(EnableCap.PolygonOffsetFill);
                GL.DepthMask(true);
                GL.DepthFunc(DepthFunction.Less);
                GL.Disable(EnableCap.Lighting);
            }
        }

        public void Render3dView()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Setup3dView();
            GL.PushMatrix();

            GL.Enable(EnableCap.Light0);
            Vector4 lightPos = new Vector4(10, 20, 20, 0);
            GL.Light(LightName.Light0, LightParameter.Position, lightPos);

            if (!Settings.settings.ShowUnderlayWithDepth || underlay == null)
            {
                DrawBasePlane();
                DrawUnderlay();
            }

            if (GridList != null)
                GridList.Call();
            DrawGridAxisMarker();

            DrawMap();

            if (Settings.settings.ShowUnderlayWithDepth)
                DrawUnderlay();

            GL.PopMatrix();

            DrawOverlay();
            control.SwapBuffers();
        }
    }
}
