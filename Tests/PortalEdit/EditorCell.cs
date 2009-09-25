using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Math;


namespace PortalEdit
{
    public class EditorCell : Cell
    {
        Polygon mapPolygon;

        public static float PolygonScale = 0.1f;
        public static float PolygonFloor = 0f;
        public static float PolygonRoof = 1f;

        public EditorCell (Polygon poly) : base()
        {
            float v = poly.GetNormalDepth();

            if (v > 0)
                poly.Reverse();

            mapPolygon = poly;

            // build the polygon for 3d;

            foreach(Point p in poly.verts)
            {
                CellVert vert = new CellVert();
                vert.bottom = new Vector3(p.X * PolygonScale, p.Y * PolygonScale, PolygonFloor);
                vert.top = PolygonRoof;
                verts.Add(vert);
            }

            for (int i = 1; i < poly.verts.Length; i++ )
            {
                CellEdge edge = new CellEdge();
                edge.start = i - 1;
                edge.end = i;
                edges.Add(edge);
            }

            CellEdge lastEdge = new CellEdge();
            lastEdge.start = poly.verts.Length - 1;
            lastEdge.end = 0;
            edges.Add(lastEdge);
        }

        public void Draw ( )
        {
            // draw the bottom
            GL.Color3(Color.White);
            GL.Begin(BeginMode.Polygon);

            GL.Normal3(0, 0, 1);
            foreach (CellEdge edge in edges)
                GL.Vertex3(verts[edge.end].bottom);
            GL.End();

            GL.LineWidth(3);
            GL.Color3(Color.Black);
            GL.Begin(BeginMode.LineLoop);
            foreach (CellEdge edge in edges)
                GL.Vertex3(verts[edge.end].bottom);

            GL.End();
            GL.LineWidth(1);
        }
    }
}
