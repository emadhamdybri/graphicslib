using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Drawables.DisplayLists;
using Drawables;

namespace PortalEdit
{
    public class EditorCell : Cell
    {
        SingleListDrawableItem floorList;
        SingleListDrawableItem roofList;
        List<SingleListDrawableItem> walls = new List<SingleListDrawableItem>();

        public static float PolygonScale = 0.1f;
        public static float PolygonFloor = 0f;
        public static float PolygonRoof = 10f;

        public static int WallPassOffet = 50;
        public static int FloorPassOffet = 100;

        static Color cellColor = Color.White;
        static Color cellEdgeColor = Color.Black;

        static Color wallColor = Color.WhiteSmoke;
        static Color wallEdgeColor = Color.Blue;//Color.FromArgb(128, Color.Blue);

        static Color portalColor = Color.FromArgb(32,Color.Gold);
        static Color portalEdgeColor = Color.FromArgb(128, Color.DarkGoldenrod);

        static Color selectionColor = Color.Red;

        public EditorCell(): base()
        {
        }

        public EditorCell (Polygon poly, PortalMap map) : base()
        {
            buildFromPolygon(poly, map);
        }

        public EditorCell(Cell cell)
            : base(cell)
        {
            generateGeometry();
        }

        public bool buildFromPolygon ( Polygon poly, PortalMap map )
        {
            float v = poly.GetNormalDepth();

            if (v > 0)
                poly.Reverse();

            // build the polygon for 3d;
            verts.Clear();
            edges.Clear();

            foreach (Point p in poly.verts)
            {
                CellVert vert = new CellVert();
                vert.bottom = new Vector3(p.X * PolygonScale, p.Y * PolygonScale, PolygonFloor);
                vert.top = PolygonRoof;
                verts.Add(vert);
            }

            for (int i = 1; i < poly.verts.Length; i++)
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

            return CheckEdges(map);
        }

        public void Dispose ()
        {
            clearGeometry();
        }

        public bool CheckEdges ( PortalMap map )
        {
            bool hasPortal = false;
            foreach (CellEdge edge in edges)
            {
                edge.type = CellEdgeType.eWall;
                edge.destination = null;
                edge.destinationName = string.Empty;

                edge.normal = new Vector2(verts[edge.start].bottom.Y - verts[edge.end].bottom.Y, -1f * (verts[edge.start].bottom.X - verts[edge.end].bottom.X));
                edge.normal.Normalize();

                Vector2 p1 = new Vector2(verts[edge.start].bottom.X, verts[edge.start].bottom.Y);
                Vector2 p2 = new Vector2(verts[edge.end].bottom.X, verts[edge.end].bottom.Y);
                List<Cell> cellsWithEdge = map.CellsThatContainEdge(p1, p2,this);

                if (cellsWithEdge.Count > 0)
                {
                    foreach (Cell cell in cellsWithEdge)
                    {
                        if (cell != this)
                        {
                            edge.type = CellEdgeType.ePortal;
                            edge.destination = cellsWithEdge[0];
                            edge.destinationName = cell.name;
                            hasPortal = true;
                            break;
                        }
                    }
                }
            }
            generateGeometry();
            return hasPortal;
        }

        void clearGeometry ( )
        {
            if (floorList != null)
                floorList.Dispose();
            if (roofList != null)
                roofList.Dispose();
            floorList = null;
            
            foreach (SingleListDrawableItem wall in walls)
                wall.Dispose();

            walls.Clear();
        }

        void generateGeometry ( )
        {
            clearGeometry();
        
            floorList = new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(floorList_Generate),DrawablesSystem.LastPass-FloorPassOffet);
            roofList = new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(roofList_Generate), DrawablesSystem.LastPass-FloorPassOffet);

            foreach (CellEdge edge in edges)
            {
                int pass = DrawablesSystem.LastPass;
                if (edge.type == CellEdgeType.eWall)
                    pass -= WallPassOffet;

                walls.Add(new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(wall_Generate), edge, pass));
            }
        }

        void wall_Generate(object sender, DisplayList list)
        {
            ListableEvent wall = (ListableEvent)sender;
            CellEdge edge = (CellEdge)wall.tag;
          
            if (wall == null || edge == null)
                return;

            if (!Settings.settings.DrawPortals && edge.type == CellEdgeType.ePortal)
                return;

            if (edge.type == CellEdgeType.eWall)
                GL.Color4(wallColor);
            else
                GL.Color4(portalColor);

            if (edge.type == CellEdgeType.ePortal)
                GL.DepthMask(false);

            GL.Begin(BeginMode.Quads);

            GL.Normal3(edge.normal.X, edge.normal.Y, 0);

            GL.Vertex3(verts[edge.end].bottom);
            GL.Vertex3(verts[edge.start].bottom);
            GL.Vertex3(verts[edge.start].bottom.X, verts[edge.start].bottom.Y, verts[edge.start].top);
            GL.Vertex3(verts[edge.end].bottom.X, verts[edge.end].bottom.Y, verts[edge.end].top);

            GL.End();

            GL.DepthMask(true);

            if (edge.type == CellEdgeType.eWall)
                GL.Color4(wallEdgeColor);
            else
                GL.Color4(portalEdgeColor);

            GL.Disable(EnableCap.Lighting);
            GL.LineWidth(3);

            if (Settings.settings.DrawCellEdges || edge.type == CellEdgeType.ePortal)
            {
                GL.Begin(BeginMode.LineLoop);
               
                GL.Vertex3(verts[edge.end].bottom);
                GL.Vertex3(verts[edge.start].bottom);
                GL.Vertex3(verts[edge.start].bottom.X, verts[edge.start].bottom.Y, verts[edge.start].top);
                GL.Vertex3(verts[edge.end].bottom.X, verts[edge.end].bottom.Y, verts[edge.end].top);

                GL.End();
            }

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        void floorList_Generate(object sender, DisplayList list)
        {
            // draw the bottom
            GL.Color4(cellColor);
            GL.Begin(BeginMode.Polygon);

            GL.Normal3(0, 0, 1);
            foreach (CellEdge edge in edges)
                GL.Vertex3(verts[edge.end].bottom);
            GL.End();

            GL.Disable(EnableCap.Lighting);

            if (Settings.settings.DrawCellEdges)
            {
                GL.LineWidth(3);
                GL.Color4(cellEdgeColor);
                GL.Begin(BeginMode.LineLoop);
                foreach (CellEdge edge in edges)
                    GL.Vertex3(verts[edge.end].bottom);

                GL.End();
            }
            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        void roofList_Generate(object sender, DisplayList list)
        {
            // draw the bottom
            GL.Color4(cellColor);
            GL.Begin(BeginMode.Polygon);

            GL.Normal3(0, 0, -1);
            for (int i = edges.Count - 1; i >= 0; i--)
                GL.Vertex3(verts[edges[i].end].bottom.X, verts[edges[i].end].bottom.Y, verts[edges[i].end].top);
            GL.End();
        }

        public void DrawSelectionFrame ()
        {
            GL.DepthMask(false);
            GL.DepthFunc(DepthFunction.Always);
            GL.Disable(EnableCap.Lighting);
            GL.Color3(selectionColor);
            GL.LineWidth(5);

            GL.Begin(BeginMode.LineLoop);
            foreach (CellVert vert in verts)
                GL.Vertex3(vert.bottom.X, vert.bottom.Y, vert.top);
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            foreach (CellVert vert in verts)
                GL.Vertex3(vert.bottom.X, vert.bottom.Y, vert.bottom.Z);
            GL.End();

            GL.Begin(BeginMode.Lines);
            foreach (CellVert vert in verts)
            {
                GL.Vertex3(vert.bottom.X, vert.bottom.Y, vert.bottom.Z - 0.1f);
                GL.Vertex3(vert.bottom.X, vert.bottom.Y, vert.bottom.Z +1f);
                GL.Vertex3(vert.bottom.X, vert.bottom.Y, vert.top + 0.1f);
                GL.Vertex3(vert.bottom.X, vert.bottom.Y, vert.top - 1f);
            }

            GL.End();

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
        }
    }
}
