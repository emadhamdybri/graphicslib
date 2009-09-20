using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PortalEdit
{
    public delegate void NewPolygonHandler ( object sender, Polygon polygon );
    public delegate void MouseStatusUpdateHandler (object sender, Point position);

    public class MapRenderer 
    {
        List<Point> points = new List<Point>();
        List<Polygon> polygons = new List<Polygon>();

        Point hoverPoint = Point.Empty;
        Control control;

        float scale = 1;
        Point offset = new Point(10,10);
        Point lastMouse = Point.Empty;

        public int snapRadius = 10;

        public event NewPolygonHandler NewPolygon;
        public event MouseStatusUpdateHandler MouseStatusUpdate;

        public MapRenderer(Control ctl)
        {
            control = ctl;
            ctl.Paint += new PaintEventHandler(Paint);
            ctl.MouseClick += new MouseEventHandler(MouseClick);
            ctl.MouseMove += new MouseEventHandler(MouseMove);
            ctl.MouseWheel += new MouseEventHandler(MouseWheel);
            ctl.Resize += new EventHandler(Resize);

            offset = new Point(ctl.Width / 2, ctl.Height / 2);
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

        protected void Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.LightGray);
            e.Graphics.TranslateTransform(0, control.Height);
            e.Graphics.ScaleTransform(scale, -scale);
            e.Graphics.TranslateTransform(offset.X * scale, offset.Y * scale);

            Grid(e.Graphics);
            e.Graphics.Flush();
            Pen outlinePen = new Pen(Color.Red, 2);
            outlinePen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

            Pen rubberBandPen = new Pen(Color.DarkRed, 3);
            rubberBandPen.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;

            foreach (Polygon poly in polygons)
                poly.Paint(e.Graphics);

            if (points.Count > 1)
                e.Graphics.DrawLines(outlinePen,points.ToArray());

            if (points.Count > 0 && hoverPoint != Point.Empty)
                e.Graphics.DrawLine(rubberBandPen, points[points.Count - 1], hoverPoint);
            else if (hoverPoint != Point.Empty)
                e.Graphics.DrawEllipse(rubberBandPen, hoverPoint.X-2,hoverPoint.Y-2,4,4);
          
            foreach (Point p in points)
                e.Graphics.FillRectangle(Brushes.OrangeRed, new Rectangle(p.X - 2, p.Y - 2, 5, 5));

            outlinePen.Dispose();
            rubberBandPen.Dispose();
        }

        public void MouseWheel (object sender, MouseEventArgs e)
        {
            float zoomPerScale = 1f/snapRadius;
            scale += zoomPerScale * (e.Delta/120);
            if (scale < 1)
                scale = 1;
            control.Invalidate(true);
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

        private void MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                points.Add(GetSnapPoint(GetScalePoint(e.X, e.Y)));
            else
            {
                if (points.Count > 2)
                {
                    Polygon poly = new Polygon(points);
                    polygons.Add(poly);
                    points = new List<Point>();

                    if (NewPolygon != null)
                        NewPolygon(this,poly);
                }
            }
            control.Invalidate(true);
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
    }

    public class Polygon
    {
        Point[] verts;
        Color color = Color.FromArgb(128,Color.OliveDrab);
        Color outlineColor = Color.FromArgb(192, Color.Black);

        public Polygon ( List<Point> points )
        {
            verts = points.ToArray();
        }

        public void Paint ( Graphics graphics )
        {
            Pen polygonPen = new Pen(outlineColor, 3);
            polygonPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
            polygonPen.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;

            Brush brush = new SolidBrush(color);

            graphics.FillPolygon(brush, verts);
            graphics.DrawPolygon(polygonPen,verts);

            foreach (Point p in verts)
                graphics.DrawEllipse(polygonPen, p.X - 3, p.Y - 3, 6, 6);

            brush.Dispose();
            polygonPen.Dispose();
        }
    }
}
