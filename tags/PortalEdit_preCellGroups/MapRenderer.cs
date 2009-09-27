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
    public delegate void MouseStatusUpdateHandler(object sender, Point position);

    public enum MapEditMode
    {
        DrawMode,
        SelectMode,
    };

    public class MapRenderer 
    {
        List<Point> points = new List<Point>();

        Color cellColor = Color.FromArgb(128, Color.OliveDrab);
        Color outlineColor = Color.FromArgb(192, Color.Black);
        Color portalColor = Color.FromArgb(192, Color.DarkGoldenrod);
        Color selectedColor = Color.Red;

        Point hoverPoint = Point.Empty;
        Control control;

        float scale = 1;
        Point offset = new Point(10,10);
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
            points.Clear();
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

            int step = snapRadius;
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

            for (int x = 0; x < control.Width; x += step*10)
            {
                graphics.DrawLine(gridPenMain, x, -control.Height * 2, x, control.Height * 2);
                graphics.DrawLine(gridPenMain, -x, -control.Height * 2, -x, control.Height * 2);
            }
            for (int y = 0; y < control.Height; y += step * 10)
            {
                graphics.DrawLine(gridPenMain, -control.Width * 2, y, control.Width * 2, y);
                graphics.DrawLine(gridPenMain, -control.Width * 2, -y, control.Width * 2, -y);
            }

            Pen YPen = new Pen(Color.FromArgb(192,Color.Red),4);
            Pen XPen = new Pen(Color.FromArgb(192, Color.Blue), 4);
            XPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
            YPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            int axisSize = (int)(step * scale) * 10;

            graphics.DrawLine(XPen, 0, 0, axisSize,0);
            graphics.DrawLine(YPen, 0, 0, 0, axisSize);
            graphics.DrawLine(XPen, 0, 0, -axisSize, 0);
            graphics.DrawLine(YPen, 0, 0, 0, -axisSize);

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

        protected void Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.LightGray);
            SetupGraphicsContext(e.Graphics);

            Grid(e.Graphics);
            e.Graphics.Flush();
            Pen outlinePen = new Pen(Color.Blue, 2);
            outlinePen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

            Pen rubberBandPen = new Pen(Color.DarkBlue, 3);
            rubberBandPen.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;

            foreach (Cell cell in map.cells)
                DrawCell(cell,e.Graphics);
            DrawSelectedCell(Editor.instance.GetSelectedCell(), e.Graphics);

            if (EditMode == MapEditMode.DrawMode)
            {
                if (points.Count > 1)
                    e.Graphics.DrawLines(outlinePen, points.ToArray());

                if (points.Count > 0 && hoverPoint != Point.Empty)
                    e.Graphics.DrawLine(rubberBandPen, points[points.Count - 1], hoverPoint);
                else if (hoverPoint != Point.Empty)
                    e.Graphics.DrawEllipse(rubberBandPen, hoverPoint.X - 2, hoverPoint.Y - 2, 4, 4);

                foreach (Point p in points)
                    e.Graphics.FillRectangle(Brushes.CadetBlue, new Rectangle(p.X - 2, p.Y - 2, 5, 5));
            }
            else
                points.Clear();


            outlinePen.Dispose();
            rubberBandPen.Dispose();
        }

        protected Point[] GetCellPointList ( Cell cell )
        {
            Point[] a = new Point[cell.verts.Count];
            for ( int i = 0; i < cell.verts.Count; i++ )
                a[i] = new Point((int)(cell.verts[i].bottom.X * snapRadius), (int)(cell.verts[i].bottom.Y * snapRadius));

            return a;
        }

        protected Point VertToPoint ( Vector3 vert )
        {
            return new Point((int)(vert.X* snapRadius),(int)(vert.Y* snapRadius));
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

        protected void DrawCell ( Cell cell, Graphics graphics )
        {
            Pen polygonPen = new Pen(outlineColor, 3);

            Pen portalPen = new Pen(portalColor, 4);


            Brush brush = new SolidBrush(cellColor);

            Point[] pList = GetCellPointList(cell);

            graphics.FillPolygon(brush, pList);

            foreach (CellEdge edge in cell.edges)
            {
                Pen pen = polygonPen;
                if (edge.type == CellEdgeType.ePortal)
                    pen = portalPen;

                graphics.DrawLine(pen, VertToPoint(cell.verts[edge.start].bottom), VertToPoint(cell.verts[edge.end].bottom));
            }

            foreach (Point p in pList)
                graphics.DrawEllipse(polygonPen, p.X - 3, p.Y - 3, 6, 6);

            brush.Dispose();
            polygonPen.Dispose();
            portalPen.Dispose();
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
            float zoomPerScale = 1f / snapRadius;
            scale += zoomPerScale * (ticks);
            if (scale < 1)
                scale = 1;
            control.Invalidate(true);
        }

        public void MouseWheel (object sender, MouseEventArgs e)
        {
            Zoom(e.Delta / 120);
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
            Polygon poly = new Polygon(points);
            points = new List<Point>();

            if (NewPolygon != null)
                NewPolygon(this, poly);

            InvalidateAll();
        }

        // does not call the callback, cus we assume this is from the editor
        public void AddPolygon ( List<Point> points, object Tag )
        {
            Polygon poly = new Polygon(points);
            poly.tag = Tag;
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

            if (MouseStatusUpdate != null)
                MouseStatusUpdate(this, hoverPoint);
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

            foreach (Cell cell in map.cells)
            {
                // compute a color
                Color color = Color.FromArgb(255, rand.Next() % 255, rand.Next() % 255, rand.Next() % 255);
                while (color == Color.White || colorMap.ContainsKey(color))
                    color = Color.FromArgb(255, rand.Next(), rand.Next(), rand.Next());

                colorMap.Add(color, cell);

                DrawSelectionCell(cell, color, graphics);
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
        public object tag;

        public Point[] verts;
        public Color color = Color.FromArgb(128, Color.OliveDrab);
        public Color outlineColor = Color.FromArgb(192, Color.Black);

        public Polygon ( List<Point> points )
        {
            verts = points.ToArray();
        }

        public float GetNormalDepth ()
        {
            Vector3 v1 = new Vector3(verts[1].X - verts[0].X, verts[1].Y - verts[0].Y, 0);
            Vector3 v2 = new Vector3(verts[1].X - verts[2].X, verts[1].Y - verts[2].Y, 0);

            return Vector3.Cross(v1, v2).Z;
        }

        public void Reverse ()
        {
            Array.Reverse(verts);
        }
    }
}
