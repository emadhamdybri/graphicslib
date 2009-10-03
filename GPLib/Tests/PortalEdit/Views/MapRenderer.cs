using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

using OpenTK;

namespace PortalEdit
{
    public delegate void NewPolygonHandler ( object sender, Polygon polygon );
    public delegate void CellSelectedHander(object sender, Cell cell);
    public delegate void MouseStatusUpdateHandler(object sender, Vector2 position);

    public enum MapEditMode
    {
        DrawMode,
        SelectMode,
    };

    public class MapRenderer 
    {
        public Polygon     incompletePoly = new Polygon();
        Vector2     hoverPos = Vector2.Zero;

        Color cellColor = Color.FromArgb(128, Color.Wheat);
        Color outlineColor = Color.FromArgb(192, Color.Black);
        Color internalPortalColor = Color.FromArgb(192, Color.Snow);
        Color externalPortalColor = Color.FromArgb(192, Color.DarkGoldenrod);
        Color selectedColor = Color.Red;
        Color vertSelectedColor = Color.Magenta;

        Control control;

        float scale = 1;
        public Point offset = new Point(10,10);
        Point lastMouse = Point.Empty;

        public event NewPolygonHandler NewPolygon;
        public event CellSelectedHander CellSelected;
        public event MouseStatusUpdateHandler MouseStatusUpdate;

        public PortalMap map;

        public MapEditMode EditMode { get { return editMode; } set { editMode = value; CheckCursor(); } }

        protected MapEditMode editMode = MapEditMode.DrawMode;

        Image underlay;
        Vector2 underlayCenter = Vector2.Zero;
        float underlayScale = 0.01f;

        public MapRenderer(Control ctl, PortalMap _map)
        {
            map = _map;
            control = ctl;
            ctl.Paint += new PaintEventHandler(Paint);
            ctl.MouseClick += new MouseEventHandler(MouseClick);
            ctl.MouseMove += new MouseEventHandler(MouseMove);
            ctl.MouseWheel += new MouseEventHandler(MouseWheel);
            ctl.Resize += new EventHandler(Resize);

            CheckCursor();

            offset = new Point(ctl.Width / 2, ctl.Height / 2);

            Editor.instance.MapLoaded += new MapLoadedHandler(MapLoaded);
        }

        void MapLoaded(object sender, EventArgs args)
        {
            CheckUnderlay();
        }

        public void CheckUnderlay ()
        {
            string imageFile = MapImageSetup.GetMapUnderlayImage(map);

            if (imageFile != string.Empty && File.Exists(imageFile))
            {
                underlay = Image.FromFile(imageFile);
                if (underlay != null)
                {
                    underlayScale = 1f / MapImageSetup.GetMapUnderlayPPU(map);
                    underlayCenter = MapImageSetup.GetMapUnderlayCenter(map);
                }
            }
            else
                underlay = null;
        }

        protected void CheckCursor ()
        {
            if (control != null)
            {
                switch(EditMode)
                {
                    case MapEditMode.DrawMode:
                        control.Cursor = Cursors.Cross;
                        break;
                     case MapEditMode.SelectMode:
                        control.Cursor = Cursors.Arrow;
                        break;
               }
            }
        }

        public void ClearEditPolygon ()
        {
            incompletePoly.Verts.Clear();
        }

        public void Redraw ()
        {
            InvalidateAll();
        }

        void Resize(object sender, EventArgs e)
        {
          //  offset = new Point(control.Width / 2, control.Height / 2);
        }

        protected void Grid ( Graphics graphics )
        {
            // figure out the sides of the grid box;

            Pen gridPen = new Pen(Color.Gray);
            Pen gridPenMain = new Pen(Color.DarkGreen,2);

            Settings settings = Settings.settings;

            int GridSize = (int)(settings.GridSize * settings.PixelsPerUnit);

            int step = (int)(settings.PixelsPerUnit*settings.GridSubDivisions);
            for (int x = 0; x < GridSize; x += step)
            {
                graphics.DrawLine(gridPen, x, -GridSize, x, GridSize);
                graphics.DrawLine(gridPen, -x, -GridSize, -x, GridSize);
            }
            for (int y = 0; y < GridSize; y += step)
            {
                graphics.DrawLine(gridPen, -GridSize, y, GridSize, y);
                graphics.DrawLine(gridPen, -GridSize, -y, GridSize, -y);
            }

            step = settings.PixelsPerUnit;

            for (int x = 0; x < GridSize; x += step)
            {
                graphics.DrawLine(gridPenMain, x, -GridSize, x, GridSize);
                graphics.DrawLine(gridPenMain, -x, -GridSize, -x, GridSize);
            }
            for (int y = 0; y < GridSize; y += step)
            {
                graphics.DrawLine(gridPenMain, -GridSize, y, GridSize, y);
                graphics.DrawLine(gridPenMain, -GridSize, -y, GridSize, -y);
            }

            Pen YPen = new Pen(Color.FromArgb(192,Color.Red),4);
            Pen XPen = new Pen(Color.FromArgb(192, Color.Blue), 4);
            XPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            YPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            int axisSize = (int)settings.PixelsPerUnit*2;

            graphics.DrawLine(XPen, 0, 0, axisSize,0);
            graphics.DrawLine(YPen, 0, 0, 0, axisSize);
            graphics.DrawLine(XPen, 0, 0, -axisSize/2, 0);
            graphics.DrawLine(YPen, 0, 0, 0, -axisSize/2);

            YPen.Dispose();
            XPen.Dispose();
            gridPen.Dispose();
        }

        protected void SetupGraphicsContext ( Graphics graphics )
        {
            graphics.TranslateTransform(0, control.Height);
            graphics.ScaleTransform(scale, -scale);
            graphics.TranslateTransform(offset.X * scale, offset.Y * scale);
        }

        protected void DrawEditPolygon ( Graphics graphics )
        {
            Pen rubberBandPen = new Pen(Color.DarkBlue, 3);
            rubberBandPen.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;
            Pen outlinePen = new Pen(Color.Blue, 2);

            outlinePen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

             if (incompletePoly.Verts.Count > 1)
             {
                 for (int i = 1; i < incompletePoly.Verts.Count; i++)
                     graphics.DrawLine(outlinePen,VertToPoint(incompletePoly.Verts[i-1]),VertToPoint(incompletePoly.Verts[i]));
             }

            if (incompletePoly.Verts.Count > 0 && hoverPos != Vector2.Zero)
                graphics.DrawLine(rubberBandPen, VertToPoint(incompletePoly.Verts[incompletePoly.Verts.Count-1]), VertToPoint(hoverPos));
            else if (hoverPos != Vector2.Zero)
            {
                Point hoverLoc = VertToPoint(hoverPos);
                graphics.DrawEllipse(rubberBandPen, hoverLoc.X - 2, hoverLoc.Y - 2, 4, 4);
            }

            foreach (Vector2 p in incompletePoly.Verts)
            {
                Point loc = VertToPoint(p);
                graphics.FillRectangle(Brushes.CadetBlue, new Rectangle(loc.X - 2, loc.Y - 2, 5, 5));
            }

            outlinePen.Dispose();
            rubberBandPen.Dispose();
        }

        protected void DrawUnderlay ( Graphics graphics )
        {
            if (underlay == null)
                return;

            graphics.ScaleTransform(1, -1);

            //ok figure out how big the image will be in map space
            float imageMapX = underlay.Width * underlayScale;
            float imageMapY = underlay.Height * underlayScale;

            // now figure out how big it will be in pixel space
            int imagePixelX = (int)(imageMapX * Settings.settings.PixelsPerUnit);
            int imagePixelY = (int)(imageMapY * Settings.settings.PixelsPerUnit);

            // now figure out where the center is in pixel Units
            Point pos = VertToPoint(underlayCenter);
            pos.Offset(-imagePixelX/2,-imagePixelY/2);

            graphics.DrawImage(underlay,new Rectangle(pos,new Size(imagePixelX,imagePixelY)));

            graphics.ScaleTransform(1, -1);
        }

        protected void Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.LightGray);
            SetupGraphicsContext(e.Graphics);

            DrawUnderlay(e.Graphics);
            Grid(e.Graphics);

            foreach (CellGroup group in map.CellGroups)
            {
                foreach (Cell cell in group.Cells)
                    DrawCell(cell, group, e.Graphics);
            }

            if (Settings.settings.ShowLowestSelection)
            {
                Cell selectedCell = Editor.instance.GetSelectedCell();
                if (selectedCell != null)
                    DrawSelectedCell(selectedCell, e.Graphics);
                else
                {
                    CellGroup selectedGroup = Editor.instance.GetSelectedGroup();
                    if (selectedGroup != null)
                    {
                        foreach (Cell cell in selectedGroup.Cells)
                            DrawSelectedCell(cell, e.Graphics);
                    }
                }
            }
            else
            {
                CellGroup selectedGroup = Editor.instance.GetSelectedGroup();
                if (selectedGroup != null)
                {
                    foreach (Cell cell in selectedGroup.Cells)
                        DrawSelectedCell(cell, e.Graphics);
                }
                else
                    DrawSelectedCell(Editor.instance.GetSelectedCell(), e.Graphics);
            }

            DrawSelectedVert(Editor.instance.GetSelectedVert(), e.Graphics);

            if (EditMode == MapEditMode.DrawMode)
                DrawEditPolygon(e.Graphics);
            else
                incompletePoly.Verts.Clear();
        }

        protected Point[] GetCellPointList ( Cell cell )
        {
            Point[] a = new Point[cell.Verts.Count];
            for ( int i = 0; i < cell.Verts.Count; i++ )
                a[i] = new Point((int)(cell.Verts[i].Bottom.X * Settings.settings.PixelsPerUnit), (int)(cell.Verts[i].Bottom.Y * Settings.settings.PixelsPerUnit));

            return a;
        }

        protected Point VertToPoint ( Vector3 vert )
        {
            return new Point((int)(vert.X*Settings.settings.PixelsPerUnit),(int)(vert.Y*Settings.settings.PixelsPerUnit));
        }

        protected Point VertToPoint ( Vector2 vert )
        {
            return new Point((int)(vert.X*Settings.settings.PixelsPerUnit),(int)(vert.Y*Settings.settings.PixelsPerUnit));
        }

        protected void DrawSelectedCell ( Cell cell, Graphics graphics )
        {
            if (cell == null)
                return;

            Pen polygonPen = new Pen(selectedColor, 3);
            polygonPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
            polygonPen.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;

            Point[] pList = GetCellPointList(cell);
            graphics.DrawPolygon(polygonPen, pList);

            foreach (Point p in pList)
                graphics.DrawEllipse(polygonPen, p.X - 6, p.Y - 6, 12, 12);

            polygonPen.Dispose();
        }

        protected void DrawCell ( Cell cell, CellGroup group, Graphics graphics )
        {
            Pen polygonPen = new Pen(outlineColor, 3);
            Pen internalPortalPen = new Pen(internalPortalColor,4);
            Pen externalPortalPen = new Pen(externalPortalColor, 4);
            Brush brush = new SolidBrush(cellColor);

            Point[] pList = GetCellPointList(cell);

            graphics.FillPolygon(brush, pList);

            foreach (CellEdge edge in cell.Edges)
            {
                Pen pen = polygonPen;
                if (edge.EdgeType == CellEdgeType.Portal)
                {
                    bool externalPortal = false;

                    foreach (PortalDestination dest in edge.Destinations)
                        if (dest.Group == group)
                            externalPortal = true;
                    if (externalPortal)
                        pen = externalPortalPen;
                    else
                        pen = internalPortalPen;
                }

                graphics.DrawLine(pen, VertToPoint(cell.Verts[edge.Start].Bottom), VertToPoint(cell.Verts[edge.End].Bottom));
            }

            foreach (Point p in pList)
                graphics.DrawEllipse(polygonPen, p.X - 3, p.Y - 3, 6, 6);

            brush.Dispose();
            polygonPen.Dispose();
            internalPortalPen.Dispose();
            externalPortalPen.Dispose();
        }

        protected void DrawSelectedVert(CellVert vert, Graphics graphics)
        {
            if (vert == null)
                return;

            Pen pen = new Pen(vertSelectedColor, 3);

            Point p = VertToPoint(vert.Bottom);
            graphics.DrawEllipse(pen, p.X - 10, p.Y - 10, 20, 20);

            pen.Dispose();
        }

        protected void DrawSelectionCell(Cell cell, Color color, Graphics graphics)
        {
            Brush brush = new SolidBrush(color);
            graphics.FillPolygon(brush, GetCellPointList(cell));
            brush.Dispose();
        }

        protected void InvalidateAll ( )
        {
            if (control.Parent != null)
                control.Parent.Invalidate(true);
        }

        public void ResetZoom ( )
        {
            scale = 1;
            control.Invalidate(true);
        }

        public void Zoom ( int ticks )
        {
            float zoomPerScale = 1f / Settings.settings.PixelsPerUnit;

            scale += zoomPerScale * (ticks);
            if (scale < zoomPerScale*25)
                scale = zoomPerScale*25;
            control.Invalidate(true);
        }

        public void MouseWheel (object sender, MouseEventArgs e)
        {
            Zoom(e.Delta / 120);
        }

        private Vector2 ScreenToMap ( int x, int y)
        {
            float unscaledPixelX = (x / scale) - offset.X*scale;
            float mapX = unscaledPixelX / Settings.settings.PixelsPerUnit;
       
            float unscaledPixelY = (y / scale) - offset.Y * scale;
            float mapY = unscaledPixelY / Settings.settings.PixelsPerUnit;

            return new Vector2(mapX, mapY);
        }

        private Vector2 ClampToAxis(Vector2 vec)
        {
            if (Control.ModifierKeys == Keys.Shift && EditMode == MapEditMode.DrawMode && incompletePoly.Verts.Count > 0)
            {
                Vector2 lastVert = incompletePoly.Verts[incompletePoly.Verts.Count - 1];
                float dx = (float)Math.Abs(hoverPos.X - lastVert.X);
                float dy = (float)Math.Abs(hoverPos.Y - lastVert.Y);

                Vector2 v = new Vector2(vec);
                if (dx > dy)
                    v.Y = lastVert.Y;
                else if (dy > dx)
                    v.X = lastVert.X;

                return v;
            }
            else
                return vec;
        }

        private Vector2 SnapPoint ( Vector2 pos )
        {
            float snapInMapSpace = Settings.settings.SnapValue;

            float xMod = 1;
            if (pos.X != 0)
                xMod = pos.X / Math.Abs(pos.X);
            float yMod = 1;
            if (pos.Y != 0)
                yMod = pos.Y / Math.Abs(pos.Y);
            int xSnap = (int)((pos.X + (xMod*snapInMapSpace / 2f)) / snapInMapSpace);
            int ySnap = (int)((pos.Y + (yMod*snapInMapSpace / 2f)) / snapInMapSpace);

            return ClampToAxis(new Vector2(xSnap * snapInMapSpace, ySnap * snapInMapSpace));
        }

        private void EditAddPolygon ( )
        {
            if (NewPolygon != null)
                NewPolygon(this, incompletePoly);

            incompletePoly = new Polygon();

            InvalidateAll();
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            if (EditMode == MapEditMode.DrawMode)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Undo.System.Add(new MapVertAddUndo(incompletePoly));
                    incompletePoly.Verts.Add(SnapPoint(ScreenToMap(e.X, control.Height - e.Y)));
                }
                else
                {
                    if (incompletePoly.Verts.Count > 2)
                        EditAddPolygon();
                }
                control.Invalidate(true);
            }
            else if (EditMode == MapEditMode.SelectMode)
                PolygonSelection(e.Location);
        }


        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                offset.X += (int)((e.X - lastMouse.X)/scale);
                offset.Y -= (int)((e.Y - lastMouse.Y) / scale);
            }

            hoverPos = SnapPoint(ScreenToMap(e.X, control.Height - e.Y));

            if (MouseStatusUpdate != null)
                MouseStatusUpdate(this, hoverPos);
            lastMouse = new Point(e.X,e.Y);
            control.Invalidate(true);
        }

        private void PolygonSelection (Point p)
        {
            if (CellSelected == null)
                return;

            Bitmap bitmap = new Bitmap(control.Width, control.Height);
            Graphics graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.White);
            SetupGraphicsContext(graphics);

            Dictionary<Color,Cell> colorMap = new Dictionary<Color,Cell>();

            Random rand = new Random();

            foreach (CellGroup group in map.CellGroups)
            {
                foreach (Cell cell in group.Cells)
                {
                    // compute a color
                    Color color = Color.FromArgb(255, rand.Next() % 255, rand.Next() % 255, rand.Next() % 255);
                    while (color == Color.White || colorMap.ContainsKey(color))
                        color = Color.FromArgb(255, rand.Next(), rand.Next(), rand.Next());

                    colorMap.Add(color, cell);

                    DrawSelectionCell(cell, color, graphics);
                }
            }
            graphics.Flush();
            graphics.Dispose();

            Color selectedColor = bitmap.GetPixel(p.X,p.Y);

            if (colorMap.ContainsKey(selectedColor))
                CellSelected(this,colorMap[selectedColor]);
        }
    }

    public class Polygon
    {
        public List<Vector2> Verts = new List<Vector2>();
        public Color color = Color.FromArgb(128, Color.OliveDrab);
        public Color outlineColor = Color.FromArgb(192, Color.Black);

        public float GetNormalDepth ()
        {
            if (Verts.Count < 2)
                return 0;

            Vector3 v1 = new Vector3(Verts[1].X - Verts[0].X, Verts[1].Y - Verts[0].Y, 0);
            Vector3 v2 = new Vector3(Verts[1].X - Verts[2].X, Verts[1].Y - Verts[2].Y, 0);

            return Vector3.Cross(v1, v2).Z;
        }

        public List<Vector2> Reverse ()
        {
            List<Vector2> v = new List<Vector2>(Verts);
            v.Reverse();
            return v;
        }
    }
}
