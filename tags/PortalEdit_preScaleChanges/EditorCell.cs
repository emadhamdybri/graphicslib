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

        public EditorCell (Polygon poly, PortalMap map, CellGroup parentGroup) : base()
        {
            Group = parentGroup;
            GroupName = parentGroup.Name;
            buildFromPolygon(poly, map);
            Name = parentGroup.NewCellName();
        }

        public EditorCell(Cell cell)
            : base(cell)
        {
            generateGeometry();
        }

        public EditorCell(EditorCell cell)
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
            Verts.Clear();
            Edges.Clear();

            HeightIsIncremental = Editor.EditZInc;

            foreach (Point p in poly.verts)
            {
                CellVert vert = new CellVert();
                vert.Bottom = new Vector3(p.X * PolygonScale, p.Y * PolygonScale, Editor.EditZFloor);
                vert.Top = Editor.EditZRoof;
                Verts.Add(vert);
            }

            for (int i = 1; i < poly.verts.Length; i++)
            {
                CellEdge edge = new CellEdge();
                edge.Start = i - 1;
                edge.End = i;
                Edges.Add(edge);
            }

            CellEdge lastEdge = new CellEdge();
            lastEdge.Start = poly.verts.Length - 1;
            lastEdge.End = 0;
            Edges.Add(lastEdge);

            return CheckEdges(map);
        }

        public void Dispose ()
        {
            clearGeometry();
        }

        public bool CheckEdges ( PortalMap map )
        {
            bool hasPortal = false;
            foreach (CellEdge edge in Edges)
            {
                edge.EdgeType = CellEdgeType.Wall;
                edge.Destination.Cell = null;
                edge.Destination.CellName = string.Empty;
                edge.Destination.Group = null;
                edge.Destination.GroupName = string.Empty;

                edge.Normal = new Vector2(Verts[edge.Start].Bottom.Y - Verts[edge.End].Bottom.Y, -1f * (Verts[edge.Start].Bottom.X - Verts[edge.End].Bottom.X));
                edge.Normal.Normalize();

                Vector2 p1 = new Vector2(Verts[edge.Start].Bottom.X, Verts[edge.Start].Bottom.Y);
                Vector2 p2 = new Vector2(Verts[edge.End].Bottom.X, Verts[edge.End].Bottom.Y);
                List<Cell> cellsWithEdge = map.CellsThatContainEdge(p1, p2,this);

                if (cellsWithEdge.Count > 0)
                {
                    foreach (Cell cell in cellsWithEdge)
                    {
                        if (cell != this)
                        {
                            edge.EdgeType = CellEdgeType.Portal;
                            edge.Destination.Cell = cell;
                            edge.Destination.Group = cell.Group;
                            edge.Destination.CellName = cell.Name;
                            edge.Destination.GroupName = cell.GroupName;
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

            foreach (CellEdge edge in Edges)
            {
                int pass = DrawablesSystem.LastPass;
                if (edge.EdgeType == CellEdgeType.Wall)
                    pass -= WallPassOffet;

                walls.Add(new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(wall_Generate), edge, pass));
            }
        }

        float GetRoofZ ( int index )
        {
            return Verts[index].GetTopZ(HeightIsIncremental);
        }

        void generateWall(CellEdge edge )
        {
            GL.Color4(wallColor);

            GL.Begin(BeginMode.Quads);

                GL.Normal3(edge.Normal.X, edge.Normal.Y, 0);
                GL.Vertex3(Verts[edge.End].Bottom);
                GL.Vertex3(Verts[edge.Start].Bottom);
                GL.Vertex3(Verts[edge.Start].Bottom.X, Verts[edge.Start].Bottom.Y, GetRoofZ(edge.Start));
                GL.Vertex3(Verts[edge.End].Bottom.X, Verts[edge.End].Bottom.Y, GetRoofZ(edge.End));
            GL.End();

            GL.Color4(wallEdgeColor);

            GL.Disable(EnableCap.Lighting);
            GL.LineWidth(3);

            if (Settings.settings.DrawCellEdges)
            {
                GL.Begin(BeginMode.LineLoop);

                    GL.Vertex3(Verts[edge.End].Bottom);
                    GL.Vertex3(Verts[edge.Start].Bottom);
                    GL.Vertex3(Verts[edge.Start].Bottom.X, Verts[edge.Start].Bottom.Y, GetRoofZ(edge.Start));
                    GL.Vertex3(Verts[edge.End].Bottom.X, Verts[edge.End].Bottom.Y, GetRoofZ(edge.End));

                GL.End();
            }

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        void generatePortalBottomGap ( CellVert sp, CellVert ep, float topSP, float topEP, Vector2 normal )
        {
            GL.Color4(wallColor);

            GL.Begin(BeginMode.Quads);

            GL.Normal3(normal.X, normal.Y, 0);
            GL.Vertex3(ep.Bottom);
            GL.Vertex3(sp.Bottom);
            GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, topSP);
            GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, topEP);
            GL.End();

            GL.Color4(wallEdgeColor);

            GL.Disable(EnableCap.Lighting);
            GL.LineWidth(3);

            if (Settings.settings.DrawCellEdges)
            {
                GL.Begin(BeginMode.LineLoop);

                GL.Vertex3(ep.Bottom);
                GL.Vertex3(sp.Bottom);
                GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, topSP);
                GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, topEP);

                GL.End();
            }

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        void generatePortalTopGap(CellVert sp, CellVert ep, float bottomSP, float bottomEP, Vector2 normal)
        {
            GL.Color4(wallColor);

            GL.Begin(BeginMode.Quads);

            GL.Normal3(normal.X, normal.Y, 0);
            GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, bottomEP);
            GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, bottomSP);
            GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, sp.GetTopZ(HeightIsIncremental));
            GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, ep.GetTopZ(HeightIsIncremental));
            GL.End();

            GL.Color4(wallEdgeColor);

            GL.Disable(EnableCap.Lighting);
            GL.LineWidth(3);

            if (Settings.settings.DrawCellEdges)
            {
                GL.Begin(BeginMode.LineLoop);
                GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, bottomEP);
                GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, bottomSP);
                GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, sp.GetTopZ(HeightIsIncremental));
                GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, ep.GetTopZ(HeightIsIncremental));

                GL.End();
            }

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        void generatePortalEdges (CellEdge edge)
        {
            // check the lower bounds
            Cell dest = edge.Destination.Cell;

            CellVert sp = Verts[edge.Start];
            CellVert ep = Verts[edge.End];

            CellVert spMatch = dest.MatchingVert(sp.Bottom);
            CellVert epMatch = dest.MatchingVert(ep.Bottom);

            if (sp.Bottom.Z < spMatch.Bottom.Z || ep.Bottom.Z < epMatch.Bottom.Z)
                generatePortalBottomGap(sp, ep, spMatch.Bottom.Z, epMatch.Bottom.Z,edge.Normal);

            if (sp.GetTopZ(HeightIsIncremental) > spMatch.GetTopZ(dest.HeightIsIncremental) || ep.GetTopZ(HeightIsIncremental) > epMatch.GetTopZ(dest.HeightIsIncremental))
                generatePortalTopGap(sp, ep, spMatch.GetTopZ(dest.HeightIsIncremental), epMatch.GetTopZ(dest.HeightIsIncremental), edge.Normal);
        }

        void generatePortal(CellEdge edge)
        {
            generatePortalEdges(edge);

            if (!Settings.settings.DrawPortals && edge.EdgeType == CellEdgeType.Portal)
                return;

            if (edge.EdgeType == CellEdgeType.Portal && edge.Destination.Group == Group)
                return;

            GL.Color4(portalColor);

            GL.DepthMask(false);

            GL.Begin(BeginMode.Quads);

                GL.Normal3(edge.Normal.X, edge.Normal.Y, 0);
                GL.Vertex3(Verts[edge.End].Bottom);
                GL.Vertex3(Verts[edge.Start].Bottom);
                GL.Vertex3(Verts[edge.Start].Bottom.X, Verts[edge.Start].Bottom.Y, GetRoofZ(edge.Start));
                GL.Vertex3(Verts[edge.End].Bottom.X, Verts[edge.End].Bottom.Y, GetRoofZ(edge.End));

            GL.End();

            GL.DepthMask(true);

            GL.Color4(portalEdgeColor);

            GL.Disable(EnableCap.Lighting);
            GL.LineWidth(3);

            GL.Begin(BeginMode.LineLoop);

            GL.Vertex3(Verts[edge.End].Bottom);
            GL.Vertex3(Verts[edge.Start].Bottom);
            GL.Vertex3(Verts[edge.Start].Bottom.X, Verts[edge.Start].Bottom.Y, GetRoofZ(edge.Start));
            GL.Vertex3(Verts[edge.End].Bottom.X, Verts[edge.End].Bottom.Y, GetRoofZ(edge.End));

            GL.End();

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        void wall_Generate(object sender, DisplayList list)
        {
            ListableEvent wall = (ListableEvent)sender;
            CellEdge edge = (CellEdge)wall.tag;
          
            if (wall == null || edge == null)
                return;

            if (edge.EdgeType == CellEdgeType.Wall)
                generateWall(edge);
            else
                generatePortal(edge);           
        }

        void floorList_Generate(object sender, DisplayList list)
        {
            // draw the bottom
            GL.Color4(cellColor);
            GL.Begin(BeginMode.Polygon);

            GL.Normal3(0, 0, 1);
            foreach (CellEdge edge in Edges)
                GL.Vertex3(Verts[edge.End].Bottom);
            GL.End();

            GL.Disable(EnableCap.Lighting);

            if (Settings.settings.DrawCellEdges)
            {
                GL.LineWidth(3);
                GL.Color4(cellEdgeColor);
                GL.Begin(BeginMode.LineLoop);
                foreach (CellEdge edge in Edges)
                    GL.Vertex3(Verts[edge.End].Bottom);

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
            for (int i = Edges.Count - 1; i >= 0; i--)
            {
                float roof = Verts[Edges[i].End].Top;
                if (HeightIsIncremental)
                    roof += Verts[Edges[i].End].Bottom.Z;
                GL.Vertex3(Verts[Edges[i].End].Bottom.X, Verts[Edges[i].End].Bottom.Y, GetRoofZ(Edges[i].End));
            }
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
            foreach (CellVert vert in Verts)
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.GetTopZ(HeightIsIncremental));
            GL.End();

            GL.Begin(BeginMode.LineLoop);
            foreach (CellVert vert in Verts)
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.Bottom.Z);
            GL.End();

            GL.Begin(BeginMode.Lines);
            foreach (CellVert vert in Verts)
            {
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.Bottom.Z - 0.1f);
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.Bottom.Z +1f);
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.GetTopZ(HeightIsIncremental) + 0.1f);
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.GetTopZ(HeightIsIncremental) - 1f);
            }

            GL.End();

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
        }
    }
}
