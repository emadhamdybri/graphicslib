using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

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
        List<Point> points = new List<Point>();
        Polygon     incompletePoly = new Polygon();
        Vector2     hoverPos = Vector2.Zero;

        Color cellColor = Color.FromArgb(128, Color.Wheat);
        Color outlineColor = Color.FromArgb(192, Color.Black);
        Color internalPortalColor = Color.FromArgb(192, Color.Snow);
        Color externalPortalColor = Color.FromArgb(192, Color.DarkGoldenrod);
        Color selectedColor = Color.Red;
        Color vertSelectedColor = Color.Magenta;

        Point hoverPoint = Point.Empty;
        Control control;

        float scale = 1;
        public Point offset = new Point(10,10);
        Point lastMouse = Point.Empty;

        public int snapRadius = 10;

        public event NewPolygonHandler NewPolygon;
        public event CellSelectedHander CellSelected;
        public event MouseStatusUpdateHandler MouseStatusUpdate;

        public PortalMap map;

        public MapEditMode EditMode { get { return editMode; } set { editMode = value; CheckCursor(); } }

        protected MapEditMode editMode = MapEditMode.DrawMode;

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

            int step = (int)(settings.PixelsPerUnit*settings.GridSubDivisions);
            for (int x = 0; x < control.Width; x += step)
            {
                graphics.DrawLine(gridPen, x, -control.Height * 2, x, control.Height * 2);
                graphics.DrawLine(gridPen, -x, -control.Height * 2, -x, control.Height * 2);
            }
            for (int y = 0; y < control.Height; y += step)
            {
                graphics.DrawLine(gridPen, -control.Width * 2, y, control.Width * 2, y);
                graphics.DrawLine(gridPen, -control.Width * 2, -y, control.Width * 2, -y);
            }

            step = settings.PixelsPerUnit;

            for (int x = 0; x < control.Width; x += step)
            {
                graphics.DrawLine(gridPenMain, x, -control.Height * 2, x, control.Height * 2);
                graphics.DrawLine(gridPenMain, -x, -control.Height * 2, -x, control.Height * 2);
            }
            for (int y = 0; y < control.Height; y += step)
            {
                graphics.DrawLine(gridPenMain, -control.Width * 2, y, control.Width * 2, y);
                graphics.DrawLine(gridPenMain, -control.Width * 2, -y, control.Width * 2, -y);
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
            else if (hoverPoint != Point.Empty)
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

        protected void Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.LightGray);
            SetupGraphicsContext(e.Graphics);

            Grid(e.Graphics);
            e.Graphics.Flush();

            foreach (CellGroup group in map.cellGroups)
            {
                foreach (Cell cell in group.Cells)
                    DrawCell(cell, group, e.Graphics);
            }
            CellGroup selectedGroup = Editor.instance.GetSelectedGroup();
            if (selectedGroup != null)
            {
                foreach (Cell cell in selectedGroup.Cells)
                    DrawSelectedCell(cell, e.Graphics);
            }
            else
                DrawSelectedCell(Editor.instance.GetSelectedCell(), e.Graphics);

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
                a[i] = new Point((int)(cell.Verts[i].Bottom.X * snapRadius), (int)(cell.Verts[i].Bottom.Y * snapRadius));

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
                    if (edge.Destination.Group == group)
                        pen = internalPortalPen;
                    else
                        pen = externalPortalPen;
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

        public void Zoom ( int ticks )
        {
            float zoomPerScale = 1f / Settings.settings.PixelsPerUnit;

            scale += zoomPerScale * (ticks);
            if (scale < 1)
                scale = 1;
            control.Invalidate(true);
        }

        public void MouseWheel (object sender, MouseEventArgs e)
        {
            Zoom(e.Delta / 120);
        }

        private Vector2 ScreenToMap ( int x, int y)
        {
            float unscaledPixelX = (x / scale) - offset.X;
            float mapX = unscaledPixelX / Settings.settings.PixelsPerUnit;

            return new Vector2(mapX, 0);
        }

        private Point GetScalePoint(int x, int y)
        {
            // take it to the original pixel
            int px = (int)(x / scale);
            int py = (int)((control.Height-y) / scale);

            int scaleX = (int)(offset.X*scale);
            int scaleY = (int)(offset.Y * scale);

            return new Point(px - scaleX, py - scaleY);
        }

        private Point GetSnapPoint(Point p)
        {
            int snapX = (p.X / snapRadius) * snapRadius;
            if (p.X - snapX > snapRadius / 2)
                snapX += snapRadius;

            int snapY = (p.Y / snapRadius) * snapRadius;
            if (p.Y - snapY > snapRadius / 2)
                snapY += snapRadius;

            return new Point(snapX, snapY);
        }

        private void EditAddPolygon ( )
        {
            if (NewPolygon != null)
                NewPolygon(this, incompletePoly);

            incompletePoly.Verts.Clear();

            InvalidateAll();
        }

        private void MouseClick(object sender, MouseEventArgs e)
        {
            if (EditMode == MapEditMode.DrawMode)
            {
                if (e.Button == MouseButtons.Left)
                    points.Add(GetSnapPoint(GetScalePoint(e.X, e.Y)));
                else
                {
                    if (points.Count > 2)
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
            hoverPoint = GetSnapPoint(GetScalePoint(e.X, e.Y));
            hoverPos = ScreenToMap(e.X, e.Y);

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

            foreach (CellGroup group in map.cellGroups)
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

        public void Reverse ()
        {
            Verts.Reverse();
        }
    }
}
