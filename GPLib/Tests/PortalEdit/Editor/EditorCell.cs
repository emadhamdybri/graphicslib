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
    public class WallGeometry : IDisposable
    {
        SingleListDrawableItem item;

        public Cell cell;
        public CellEdge edge;

        public CellWallGeometry geo;
        
        public WallGeometry ( Cell c, CellEdge e,  CellWallGeometry g)
        {
            cell = c;
            edge = e;
            geo = g;
            item = new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(Generate), DrawablesSystem.LastPass - EditorCell.WallPassOffet);
        }

        public void Generate ( object sender, DisplayList list)
        {
            GL.Color4(EditorCell.wallColor);

            GL.Begin(BeginMode.Quads);

            GL.Normal3(edge.Normal.X, edge.Normal.Y, 0);
            GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y,geo.LowerZ[1]);
            GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, geo.LowerZ[0]);
            GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, geo.UpperZ[0]);
            GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y, geo.UpperZ[1]);
            GL.End();

            GL.Color4(EditorCell.wallEdgeColor);

            GL.Disable(EnableCap.Lighting);
            GL.LineWidth(EditorCell.outlineLineWidth);

            if (Settings.settings.DrawCellEdges)
            {
                GL.Begin(BeginMode.LineLoop);

                GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y, geo.LowerZ[1]);
                GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, geo.LowerZ[0]);
                GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, geo.UpperZ[0]);
                GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y, geo.UpperZ[1]);

                GL.End();
            }

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        public void Dispose ()
        {
            item.Dispose();
        }
    }

    public class PortalGeometry : IDisposable
    {
        SingleListDrawableItem item;

        Cell cell;
        CellEdge edge;

        PortalDestination dest;

        public PortalGeometry(Cell c, CellEdge e, PortalDestination d)
        {
            cell = c;
            edge = e;
            dest = d;
            item = new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(Generate), DrawablesSystem.LastPass);
        }

        public void Generate(object sender, DisplayList list)
        {
            if (!Settings.settings.DrawPortals && edge.EdgeType == CellEdgeType.Portal)
                return;

            if (edge.EdgeType == CellEdgeType.Portal && dest.Group == cell.Group)
                return;

            CellVert destSP = dest.Cell.MatchingVert(cell.Verts[edge.Start]);
            CellVert destEP = dest.Cell.MatchingVert(cell.Verts[edge.End]);

            GL.Color4(EditorCell.portalColor);

            GL.DepthMask(false);

            GL.Begin(BeginMode.Quads);

            GL.Normal3(edge.Normal.X, edge.Normal.Y, 0);
            GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y, destEP.Bottom.Z);
            GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, destSP.Bottom.Z);
            GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, destSP.GetTopZ(dest.Cell.HeightIsIncremental));
            GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y,  destEP.GetTopZ(dest.Cell.HeightIsIncremental));

            GL.End();

            GL.DepthMask(true);

            GL.Color4(EditorCell.portalEdgeColor);

            GL.Disable(EnableCap.Lighting);
            GL.LineWidth(EditorCell.outlineLineWidth);

            GL.Begin(BeginMode.LineLoop);

            GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y, destEP.Bottom.Z);
            GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, destSP.Bottom.Z);
            GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, destSP.GetTopZ(dest.Cell.HeightIsIncremental));
            GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y,  destEP.GetTopZ(dest.Cell.HeightIsIncremental));

            GL.End();

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        public void Dispose()
        {
            item.Dispose();
        }
    }

    public class EditorCell : Cell
    {
        SingleListDrawableItem floorList;
        SingleListDrawableItem roofList;
        public List<WallGeometry> walls = new List<WallGeometry>();
        public List<PortalGeometry> portals = new List<PortalGeometry>();

        public static float PolygonScale = 0.1f;

        public static int WallPassOffet = 50;
        public static int FloorPassOffet = 100;

        public static Color cellColor = Color.White;
        public static Color cellEdgeColor = Color.Black;

        public static Color wallColor = Color.WhiteSmoke;
        public static Color wallEdgeColor = Color.Blue;//Color.FromArgb(128, Color.Blue);

        public static Color portalColor = Color.FromArgb(32, Color.Gold);
        public static Color portalEdgeColor = Color.FromArgb(128, Color.DarkGoldenrod);

        public static Color selectionColor = Color.Red;

        public static float outlineLineWidth = 2;
        public static float selectedLineWidht = 3;

        public static float selectedMarkSize = 0.5f;

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

        public WallGeometry FindWallGeo ( int edge, CellWallGeometry geo )
        {
            foreach (WallGeometry wall in walls)
            {
                if (wall.edge == Edges[edge] && wall.geo == geo)
                    return wall;
            }

            return null;
        }

        public WallGeometry FindWallGeo(CellEdge edge, CellWallGeometry geo)
        {
            foreach (WallGeometry wall in walls)
            {
                if (wall.edge == edge && wall.geo == geo)
                    return wall;
            }

            return null;
        }

        public bool buildFromPolygon ( Polygon poly, PortalMap map )
        {
            float v = poly.GetNormalDepth();

            List<Vector2> polyVerts = poly.Verts;

            if (v > 0)
                polyVerts = poly.Reverse();

            // build the polygon for 3d;
            Verts.Clear();
            Edges.Clear();

            HeightIsIncremental = Editor.EditZInc;

            foreach (Vector2 p in polyVerts)
            {
                CellVert vert = new CellVert();
                vert.Bottom = new Vector3(p.X, p.Y, Editor.EditZFloor);
                vert.Top = Editor.EditZRoof;
                Verts.Add(vert);
            }

            for (int i = 1; i < polyVerts.Count; i++)
            {
                CellEdge edge = new CellEdge();
                edge.Start = i - 1;
                edge.End = i;
                Edges.Add(edge);
            }

            CellEdge lastEdge = new CellEdge();
            lastEdge.Start = polyVerts.Count - 1;
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
                edge.Destinations.Clear();

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
                            CellVert thisSP = Verts[edge.Start];
                            CellVert thisEP = Verts[edge.End];

                            CellVert destSP = cell.MatchingVert(Verts[edge.Start]);
                            CellVert destEP = cell.MatchingVert(Verts[edge.End]);

                            if (thisSP.GetTopZ(HeightIsIncremental) < destSP.Bottom.Z && thisEP.GetTopZ(HeightIsIncremental) < destEP.Bottom.Z)
                                continue; // the destination is TOTALY above us, so we can't portal to it

                            if (thisSP.Bottom.Z > destSP.GetTopZ(cell.HeightIsIncremental) && thisEP.Bottom.Z > destEP.GetTopZ(cell.HeightIsIncremental))
                                continue; // the destination is TOTALY below us, so we can't portal to it

                            // we know that one edge of the dest cell is inside our height range so we can portal to it
                            edge.EdgeType = CellEdgeType.Portal;
                            PortalDestination dest = new PortalDestination();
                            dest.Cell = cell;
                            dest.Group = cell.Group;
                            dest.CellName = cell.Name;
                            dest.GroupName = cell.GroupName;
                            edge.Destinations.Add(dest);
                            hasPortal = true;
                        }
                    }
                }
            }

            generateWallDefs();
            generateGeometry();
            return hasPortal;
        }

        void generateWallDefs()
        {
            foreach (CellEdge edge in Edges)
            {
                edge.Geometry.Clear();
                CellWallGeometry geo;

                int pass = DrawablesSystem.LastPass + WallPassOffet;
                if (edge.EdgeType == CellEdgeType.Wall)
                {
                    geo = new CellWallGeometry();

                    geo.UpperZ[0] = Verts[edge.Start].GetTopZ(HeightIsIncremental);
                    geo.UpperZ[1] = Verts[edge.End].GetTopZ(HeightIsIncremental);

                    geo.LowerZ[0] = Verts[edge.Start].Bottom.Z;
                    geo.LowerZ[1] = Verts[edge.End].Bottom.Z;
                    edge.Geometry.Add(geo);
                }
                else
                {
                    CellVert thisSP = Verts[edge.Start];
                    CellVert thisEP = Verts[edge.End];

                    CellVert bestDestSP;
                    CellVert bestDestEP;
                    CellVert thisDestSP;
                    CellVert topDestSP;
                    CellVert topDestEP;
                    CellVert destSP;
                    CellVert lowestSP;

                    Cell bestDest = null;
                    Cell topDest = null;

                    // find the lowest top
                    Cell lowestTop = null;
                    foreach (PortalDestination dest in edge.Destinations)
                    {
                        destSP = dest.Cell.MatchingVert(thisSP);
                        if (destSP.GetTopZ(dest.Cell.HeightIsIncremental) > thisSP.Bottom.Z)
                        {
                            // the top is above us
                            if (lowestTop == null)
                                lowestTop = dest.Cell;
                            else
                            {
                                lowestSP = lowestTop.MatchingVert(thisSP);
                                if (destSP.GetTopZ(dest.Cell.HeightIsIncremental) < lowestSP.Bottom.Z)
                                    lowestTop = dest.Cell;
                            }
                        }
                    }

                    bool doBottomFace = true;
                    if (lowestTop != null)
                    {
                        lowestSP = lowestTop.MatchingVert(thisSP);
                        if (lowestSP.Bottom.Z < thisSP.Bottom.Z)
                        {
                            doBottomFace = false;
                            topDest = lowestTop;
                        }
                    }

                    if (doBottomFace)
                    {
                        // find the portal that has a bottom SP that is higher then our SP
                        // this will be the wall from our bottom to the next highest portal (bottom rung)
                        foreach (PortalDestination dest in edge.Destinations)
                        {
                            destSP = dest.Cell.MatchingVert(thisSP);

                            if (destSP.Bottom.Z <= thisSP.Bottom.Z)
                                continue; // it's below us, so we ignore it, any link walls will be to our top

                            if (bestDest == null)
                                bestDest = dest.Cell;
                            else
                            {
                                bestDestSP = bestDest.MatchingVert(thisSP);
                                if (bestDestSP.Bottom.Z > destSP.Bottom.Z) // his his bottom lower then the current best
                                    bestDest = dest.Cell;
                            }
                        }
                        topDest = bestDest;
                    }

                    if (topDest != null) // ok someone had a portal above us
                    {
                        if (doBottomFace)
                        {
                            // add geo from our bottom to it's bottom
                            geo = new CellWallGeometry();
                            bestDestSP = topDest.MatchingVert(thisSP);
                            bestDestEP = topDest.MatchingVert(thisSP);

                            geo.UpperZ[0] = bestDestSP.Bottom.Z;
                            geo.UpperZ[1] = bestDestEP.Bottom.Z;

                            geo.LowerZ[0] = thisSP.Bottom.Z;
                            geo.LowerZ[1] = thisSP.Bottom.Z;
                            edge.Geometry.Add(geo);
                        }

                        bestDestSP = topDest.MatchingVert(thisSP);
                        bestDestEP = topDest.MatchingVert(thisSP);
                        // check and see if his top is below our top
                        if (bestDestSP.GetTopZ(topDest.HeightIsIncremental) < thisSP.GetTopZ(HeightIsIncremental))
                        {
                            // it is, so we need to go and keep building ladders untll none are higher
                            bool done = false;

                            while (!done)
                            {
                                // find the portal that has a bottom above the current top and is still under our roof (next rung) 
                                bestDest = null;

                                topDestSP = topDest.MatchingVert(thisSP);

                                foreach (PortalDestination dest in edge.Destinations)
                                {
                                    if (dest.Cell == topDest)
                                        continue;

                                    thisDestSP = dest.Cell.MatchingVert(thisSP);

                                    if (thisDestSP.Bottom.Z > topDestSP.GetTopZ(topDest.HeightIsIncremental))
                                    {
                                        if (bestDest == null)
                                            bestDest = dest.Cell;
                                        else
                                        {
                                            bestDestSP = bestDest.MatchingVert(thisSP);

                                            if (bestDestSP.Bottom.Z > thisDestSP.Bottom.Z)
                                                bestDest = dest.Cell;
                                        }
                                    }
                                }

                                if (bestDest == null)
                                    done = true;
                                else
                                {
                                    // make a wall from the last top cell's top to the best dest cell's bottom because it's above us.
                                    geo = new CellWallGeometry();

                                    bestDestSP = bestDest.MatchingVert(thisSP);
                                    bestDestEP = bestDest.MatchingVert(thisEP);

                                    geo.UpperZ[0] = bestDestSP.Bottom.Z;
                                    geo.UpperZ[1] = bestDestEP.Bottom.Z;

                                    topDestSP = topDest.MatchingVert(thisSP);
                                    topDestEP = topDest.MatchingVert(thisEP);

                                    geo.LowerZ[0] = topDestSP.GetTopZ(topDest.HeightIsIncremental);
                                    geo.LowerZ[1] = topDestEP.GetTopZ(topDest.HeightIsIncremental);

                                    edge.Geometry.Add(geo);

                                    // set the next cell as the "top" and do it again
                                    topDest = bestDest;
                                    topDestSP = topDest.MatchingVert(thisSP);

                                    if (topDestSP.GetTopZ(topDest.HeightIsIncremental) > thisSP.GetTopZ(HeightIsIncremental))
                                        done = true; // if this guy goes outside of our cell then we are totaly done
                                }
                            }
                        }
                    }

                    if (topDest == null)
                    {
                        bestDest = null;
                        // no portals had a bottom below us, so find one that has the highest top z above us
                        foreach (PortalDestination dest in edge.Destinations)
                        {
                            destSP = dest.Cell.MatchingVert(thisSP);

                            if (destSP.GetTopZ(dest.Cell.HeightIsIncremental) < thisSP.GetTopZ(HeightIsIncremental))
                            {
                                if (bestDest == null)
                                    bestDest = dest.Cell;
                                else
                                {
                                    bestDestSP = bestDest.MatchingVert(thisSP);
                                    if (bestDestSP.GetTopZ(bestDest.HeightIsIncremental) > destSP.GetTopZ(dest.Cell.HeightIsIncremental)) // his his top higher then the current best
                                        bestDest = dest.Cell;

                                }
                            }
                        }
                        if (bestDest != null) // go from the best dest to our top
                        {
                            geo = new CellWallGeometry();
                            geo.UpperZ[0] = thisSP.GetTopZ(HeightIsIncremental);
                            geo.UpperZ[1] = thisEP.GetTopZ(HeightIsIncremental);

                            bestDestSP = bestDest.MatchingVert(thisSP);
                            bestDestEP = bestDest.MatchingVert(thisSP);

                            geo.LowerZ[0] = bestDestSP.GetTopZ(bestDest.HeightIsIncremental);
                            geo.LowerZ[1] = bestDestEP.GetTopZ(bestDest.HeightIsIncremental);
                            edge.Geometry.Add(geo);
                        }
                    }
                    else
                    {
                        topDestSP = topDest.MatchingVert(thisSP);

                        if (topDestSP.GetTopZ(topDest.HeightIsIncremental) < thisSP.GetTopZ(HeightIsIncremental))
                        {
                            // build a wall that goes from his top to our top
                            geo = new CellWallGeometry();
                            topDestSP = topDest.MatchingVert(thisSP);
                            topDestEP = topDest.MatchingVert(thisSP);

                            geo.LowerZ[0] = topDestSP.GetTopZ(topDest.HeightIsIncremental);
                            geo.LowerZ[1] = topDestEP.GetTopZ(topDest.HeightIsIncremental);

                            geo.UpperZ[0] = thisSP.GetTopZ(HeightIsIncremental);
                            geo.UpperZ[1] = thisSP.GetTopZ(HeightIsIncremental);
                            edge.Geometry.Add(geo);
                        }
                    }
                }
            }
        }

        void clearGeometry ( )
        {
            if (floorList != null)
                floorList.Dispose();
            if (roofList != null)
                roofList.Dispose();
            floorList = null;
            
            foreach (WallGeometry wall in walls)
                wall.Dispose();

            foreach (PortalGeometry portal in portals)
                portal.Dispose();

            walls.Clear();
            portals.Clear();
        }

        void generateGeometry ( )
        {
            clearGeometry();
        
            floorList = new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(floorList_Generate),DrawablesSystem.LastPass-FloorPassOffet);
            roofList = new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(roofList_Generate), DrawablesSystem.LastPass-FloorPassOffet);

            foreach (CellEdge edge in Edges)
            {
                foreach (CellWallGeometry geo in edge.Geometry)
                    walls.Add(new WallGeometry(this, edge, geo));

                if (edge.EdgeType == CellEdgeType.Portal)
                {
                    foreach (PortalDestination dest in edge.Destinations)
                        portals.Add(new PortalGeometry(this, edge, dest));
                }
            }
        }

        float GetRoofZ ( int index )
        {
            return Verts[index].GetTopZ(HeightIsIncremental);
        }

        public void floorList_Generate(object sender, DisplayList list)
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
                GL.LineWidth(outlineLineWidth);
                GL.Color4(cellEdgeColor);
                GL.Begin(BeginMode.LineLoop);
                foreach (CellEdge edge in Edges)
                    GL.Vertex3(Verts[edge.End].Bottom);

                GL.End();
            }
            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        public void roofList_Generate(object sender, DisplayList list)
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

        public void DrawFloorSelectionFrame ()
        {
            GL.DepthMask(false);
            GL.DepthFunc(DepthFunction.Always);
            GL.Disable(EnableCap.Lighting);
            GL.Color3(selectionColor);
            GL.LineWidth(selectedLineWidht);

            GL.Begin(BeginMode.LineLoop);
            foreach (CellVert vert in Verts)
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.Bottom.Z);
            GL.End();

            GL.Begin(BeginMode.Lines);
            foreach (CellVert vert in Verts)
            {
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.Bottom.Z - (selectedMarkSize * 0.25f));
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.Bottom.Z + selectedMarkSize);
            }

            GL.End();

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
        }

        public void DrawRoofSelectionFrame ()
        {
            GL.DepthMask(false);
            GL.DepthFunc(DepthFunction.Always);
            GL.Disable(EnableCap.Lighting);
            GL.Color3(selectionColor);
            GL.LineWidth(selectedLineWidht);

            GL.Begin(BeginMode.LineLoop);
            foreach (CellVert vert in Verts)
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.GetTopZ(HeightIsIncremental));
            GL.End();

            GL.Begin(BeginMode.Lines);
            foreach (CellVert vert in Verts)
            {
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.GetTopZ(HeightIsIncremental) + (selectedMarkSize*0.25f));
                GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.GetTopZ(HeightIsIncremental) - selectedMarkSize);
            }

            GL.End();

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
        }

        public void DrawSelectionFrame ()
        {
            DrawFloorSelectionFrame();
            DrawRoofSelectionFrame();
        }
    }
}
