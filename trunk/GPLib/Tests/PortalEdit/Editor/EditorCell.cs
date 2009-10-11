using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Drawables.Materials;
using Drawables.DisplayLists;
using Drawables;
using Math3D;

using World;

namespace PortalEdit
{
    public class Polygon
    {
        public List<Vector2> Verts = new List<Vector2>();
        public Color color = Color.FromArgb(128, Color.OliveDrab);
        public Color outlineColor = Color.FromArgb(192, Color.Black);
        
        public float GetNormalDepth()
        {
            if (Verts.Count < 2)
                return 0;

            Vector3 v1 = new Vector3(Verts[1].X - Verts[0].X, Verts[1].Y - Verts[0].Y, 0);

            v1.Normalize();

            int vert = 2;
            Vector3 v2 = new Vector3(Verts[vert].X - Verts[vert - 1].X, Verts[vert].Y - Verts[vert - 1].Y, 0);
            v2.Normalize();
            while (v1 == v2)
            {
                vert++;
                if (vert >= Verts.Count)
                    return 0;

                v2 = new Vector3(Verts[vert].X - Verts[vert - 1].X, Verts[vert].Y - Verts[vert - 1].Y, 0);
                v2.Normalize();
            }

            return Vector3.Cross(v1, -v2).Z;
        }

        public List<Vector2> Reverse()
        {
            List<Vector2> v = new List<Vector2>(Verts);
            v.Reverse();
            return v;
        }
    }

    public class WallGeometry : IDisposable
    {
        SingleListDrawableItem wallGeo;
        SingleListDrawableItem wallOutline;

        public Cell cell;
        public CellEdge edge;

        public CellWallGeometry geo;
        Material material;

        public WallGeometry ( Cell c, CellEdge e,  CellWallGeometry g)
        {
            cell = c;
            edge = e;
            geo = g;

            material = EditorCell.GetGeoMaterial(geo.Material);

            wallGeo = new SingleListDrawableItem(material, new ListableEvent.GenerateEventHandler(GenerateGeo), DrawablesSystem.LastPass - EditorCell.WallPassOffet);
            wallOutline = new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(GenerateOutline), DrawablesSystem.LastPass - EditorCell.WallPassOffet + 1);
            wallOutline.ShouldDrawItem += new SingleListDrawableItem.ShouldDrawItemHandler(wallOutline_ShouldDrawItem);
        }

        void wallOutline_ShouldDrawItem(object sender, ref bool draw)
        {
            draw = Settings.settings.DrawCellEdges;
        }

        public void GenerateOutline(object sender, DisplayList list)
        {
            float alpha = 1;

            if (!EditorCell.CellIsDrawn(cell))
                alpha = Settings.settings.HiddenItemAlpha;
            
            Color c = EditorCell.GetCellOutlineColor(cell);

            GL.Color4(c.R / 255f, c.G / 255f, c.B / 255f, alpha);

            GL.Color3(EditorCell.GetCellOutlineColor(cell));
            GL.Disable(EnableCap.Lighting);
            GL.LineWidth(EditorCell.outlineLineWidth);

            GL.Begin(BeginMode.LineLoop);

            GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y, geo.LowerZ[1]);
            GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, geo.LowerZ[0]);
            GL.Vertex3(cell.Verts[edge.Start].Bottom.X, cell.Verts[edge.Start].Bottom.Y, geo.UpperZ[0]);
            GL.Vertex3(cell.Verts[edge.End].Bottom.X, cell.Verts[edge.End].Bottom.Y, geo.UpperZ[1]);

            GL.End();

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        public void GenerateGeo(object sender, DisplayList list)
        {
            float alpha = 1;

            if (!EditorCell.CellIsDrawn(cell))
                alpha = Settings.settings.HiddenItemAlpha;
            material.baseColor.glColor(alpha);
    
            GL.Begin(BeginMode.Quads);

            CellVert sp = cell.Verts[edge.Start];
            CellVert ep = cell.Verts[edge.End];

            GL.Normal3(edge.Normal.X, edge.Normal.Y, 0);

            float edgeDistance = cell.EdgeDistance(edge);

            GL.TexCoord2(geo.Material.GetFinalUV(edgeDistance, geo.UpperZ[1]-geo.LowerZ[1]));
            GL.Vertex3(ep.Bottom.X, ep.Bottom.Y,geo.LowerZ[1]);

            GL.TexCoord2(geo.Material.GetFinalUV(0, geo.UpperZ[0] - geo.LowerZ[0]));
            GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, geo.LowerZ[0]);

            GL.TexCoord2(geo.Material.GetFinalUV(0, 0));
            GL.Vertex3(sp.Bottom.X, sp.Bottom.Y, geo.UpperZ[0]);

            GL.TexCoord2(geo.Material.GetFinalUV(edgeDistance, 0));
            GL.Vertex3(ep.Bottom.X, ep.Bottom.Y, geo.UpperZ[1]);
            GL.End();
        }

        public void Dispose ()
        {
            wallGeo.Dispose();
            wallOutline.Dispose();
        }
    }

    public class PortalGeometry : IDisposable
    {
        SingleListDrawableItem portalGeo;

        Cell cell;
        CellEdge edge;

        PortalDestination dest;

        public PortalGeometry(Cell c, CellEdge e, PortalDestination d)
        {
            cell = c;
            edge = e;
            dest = d;
            portalGeo = new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(GenerateGeo), DrawablesSystem.LastPass);
            portalGeo.ShouldDrawItem += new SingleListDrawableItem.ShouldDrawItemHandler(portalGeo_ShouldDrawItem);
        }

        void portalGeo_ShouldDrawItem(object sender, ref bool draw)
        {
            if (!Settings.settings.DrawPortals || dest.DestinationCell.GroupName == cell.ID.GroupName)
                draw = false;
        }

        public void GenerateGeo(object sender, DisplayList list)
        {
            CellVert destSP = dest.Cell.MatchingVert(cell.Verts[edge.Start]);
            CellVert destEP = dest.Cell.MatchingVert(cell.Verts[edge.End]);

            if (destSP == null || destEP == null)
                return;

            if (!EditorCell.CellIsDrawn(cell))
                return;

            GL.Color4(EditorCell.portalColor);

            GL.DepthMask(false);

            GL.Begin(BeginMode.Quads);

            GL.Normal3(edge.Normal.X, edge.Normal.Y, 0);
            GL.Vertex3(dest.EPBottom);
            GL.Vertex3(dest.SPBottom);
            GL.Vertex3(dest.SPTop);
            GL.Vertex3(dest.EPTop);

            GL.End();

            GL.DepthMask(true);

            GL.Color4(EditorCell.portalEdgeColor);

            GL.Disable(EnableCap.Lighting);
            GL.LineWidth(EditorCell.outlineLineWidth);

            GL.Begin(BeginMode.LineLoop);

            GL.Vertex3(dest.EPBottom);
            GL.Vertex3(dest.SPBottom);
            GL.Vertex3(dest.SPTop);
            GL.Vertex3(dest.EPTop);
            GL.End();

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        public void Dispose()
        {
            portalGeo.Dispose();
        }
    }

    public class CellGeometry : IDisposable
    {
        SingleListDrawableItem geo;
        SingleListDrawableItem outline;

        public Cell cell;
        public bool floor = true;

        Material material;

        public CellGeometry(bool f, Cell c)
        {
            cell = c;
            floor = f;

            if (f)
                material = EditorCell.GetGeoMaterial(cell.FloorMaterial);
            else
                material = EditorCell.GetGeoMaterial(cell.RoofMaterial);

            geo = new SingleListDrawableItem(material, new ListableEvent.GenerateEventHandler(GenerateGeo), DrawablesSystem.LastPass - EditorCell.FloorPassOffet);
            if (floor)
            {
                outline = new SingleListDrawableItem(new ListableEvent.GenerateEventHandler(GenerateOutline));
                outline.ShouldDrawItem += new SingleListDrawableItem.ShouldDrawItemHandler(outline_ShouldDrawItem);
            }
        }

        void outline_ShouldDrawItem(object sender, ref bool draw)
        {
            draw = Settings.settings.DrawCellEdges;
        }

        public void GenerateGeo(object sender, DisplayList list)
        {
            float alpha = 1;

            if (!EditorCell.CellIsDrawn(cell))
                alpha = Settings.settings.HiddenItemAlpha;

            material.baseColor.glColor(alpha);

            CellVert start = cell.Verts[cell.Edges[0].Start];
            if (floor)
            {
                // draw the bottom
              //  GL.Color4(EditorCell.cellFloorColor.R / 255f, EditorCell.cellFloorColor.G / 255f, EditorCell.cellFloorColor.B / 255f, alpha);
                GL.Begin(BeginMode.Polygon);

                GL.Normal3(cell.FloorNormal);

                foreach (CellEdge edge in cell.Edges)
                {
                    CellVert vert = cell.Verts[edge.End];
                    GL.TexCoord2(cell.FloorMaterial.GetFinalUV(vert.Bottom.X - start.Bottom.X, start.Bottom.Y-vert.Bottom.Y));
                    GL.Vertex3(vert.Bottom);
                }
                GL.End();
            }
            else
            {
                // draw the top
            //    GL.Color4(EditorCell.cellRoofColor.R / 255f, EditorCell.cellRoofColor.G / 255f, EditorCell.cellRoofColor.B / 255f, alpha);
                GL.Begin(BeginMode.Polygon);

                GL.Normal3(cell.RoofNormal);
                for (int i = cell.Edges.Count - 1; i >= 0; i--)
                {
                    CellVert vert = cell.Verts[cell.Edges[i].End];
                    GL.TexCoord2(cell.FloorMaterial.GetFinalUV(vert.Bottom.X - start.Bottom.X, start.Bottom.Y-vert.Bottom.Y));
                    GL.Vertex3(vert.Bottom.X, vert.Bottom.Y, vert.GetTopZ(cell.HeightIsIncremental));
                }
                GL.End();
            }
         }

        public void GenerateOutline(object sender, DisplayList list)
        {
            if (!EditorCell.CellIsDrawn(cell))
                return;

            GL.Disable(EnableCap.Lighting);

            GL.LineWidth(EditorCell.outlineLineWidth);
            if (EditorCell.CellIsDrawn(cell))
                GL.Color4(Color.White);
            else
                GL.Color4(1, 1, 1, Settings.settings.HiddenItemAlpha);
            GL.Color3(EditorCell.GetCellOutlineColor(cell));
            GL.Begin(BeginMode.LineLoop);
            foreach (CellEdge edge in cell.Edges)
                GL.Vertex3(cell.Verts[edge.End].Bottom);

            GL.End();

            GL.LineWidth(1);
            GL.Enable(EnableCap.Lighting);
        }

        public void Dispose()
        {
            geo.Dispose();
            if (outline != null)
                outline.Dispose();
        }
    }

    public class EditorCell : Cell
    {
        public CellGeometry floor;
        public CellGeometry roof;
        public List<WallGeometry> walls = new List<WallGeometry>();
        public List<PortalGeometry> portals = new List<PortalGeometry>();

        public static float PolygonScale = 0.1f;

        public static int WallPassOffet = 50;
        public static int FloorPassOffet = 100;

        public static Color cellColor = Color.White;
        public static Color cellFloorColor = Color.AntiqueWhite;
        public static Color cellRoofColor = Color.WhiteSmoke;
        public static Color cellEdgeColor = Color.Black;

        public static Color wallColor = Color.WhiteSmoke;
        public static Color wallEdgeColor = Color.Blue;//Color.FromArgb(128, Color.Blue);

        public static Color portalColor = Color.FromArgb(32, Color.Gold);
        public static Color portalEdgeColor = Color.FromArgb(128, Color.DarkGoldenrod);

        public static Color selectionColor = Color.Red;

        public static float outlineLineWidth = 2;
        public static float selectedLineWidht = 3;

        public static float selectedMarkSize = 0.5f;

        public static List<Color> CellGroupColors = new List<Color>();

        protected static Random ColorRand = new Random();
        public static Color GetCellOutlineColor( Cell cell )
        {
            if (CellGroupColors.Count == 0)
            {
                CellGroupColors.Add(Color.Red);
                CellGroupColors.Add(Color.White);
                CellGroupColors.Add(Color.Black);
            }

            if (cell.Group == null)
                return cellEdgeColor;

            Byte r = 255;
            Byte g = 255;
            Byte b = 255;

            PortalMapAttribute[] a = cell.Group.GroupAttributes.Find("Editor:GroupHighlightColor");

            if (a != null && a.Length > 0)
            {
                string[] nugs = a[0].Value.Split(" ".ToCharArray());
                if (nugs.Length > 2)
                {
                    r = Byte.Parse(nugs[0]);
                    g = Byte.Parse(nugs[1]);
                    b = Byte.Parse(nugs[2]);

                    return Color.FromArgb(255,r, g, b);
                }
                cell.Group.GroupAttributes.Remove("Editor:GroupHighlightColor");
                a = null;
            }

            Color color = Color.White;

            r = 255;
            g = 255;
            b = 255;

            int offset = 64;
            int mod = 64;

            while (CellGroupColors.Contains(color))
            {
                r = (Byte)((ColorRand.Next() % mod) + offset);
                g = (Byte)((ColorRand.Next() % mod) + offset);
                b = (Byte)((ColorRand.Next() % mod) + offset);

                color = Color.FromArgb(255,r, g, b);
            }

            CellGroupColors.Add(color);
            cell.Group.GroupAttributes.Add("Editor:GroupHighlightColor",r.ToString() + " " + g.ToString() + " " + b.ToString());
            return color;
        }

        static Material defaultMaterial = null;

        public static Material GetDefaultMaterial ()
        {
            if (defaultMaterial != null)
                return defaultMaterial;

            defaultMaterial = MaterialSystem.system.FromImage(Editor.instance.frame.DefaultImages.Images[0]);

            return defaultMaterial;
        }
       
        public static Material GetGeoMaterial ( CellMaterialInfo info )
        {
            Material mat = MaterialSystem.system.GetMaterial(info.Material);
            if (mat == null)
            {
                if (Resources.File(info.Material) != null)
                    mat = MaterialSystem.system.FromTextureFile(info.Material);
            }
            if (mat == null)
                mat = GetDefaultMaterial();

            return mat;
        }

        public static bool HideGeo = false;
        public static float HideGeoUnder = 0;
        public static float HideGeoOver = 10;

        public static bool CellIsDrawn(Cell cell)
        {
            if (!HideGeo)
                return true;

            foreach (CellVert vert in cell.Verts)
            {
                if (vert.Bottom.Z > HideGeoOver)
                    return false;

                if (vert.GetTopZ(cell.HeightIsIncremental) < HideGeoUnder)
                    return false;
            }

            return true;
        }

        public EditorCell(): base()
        {
        }

        public EditorCell(Polygon poly, PortalWorld map, CellGroup parentGroup)
            : base()
        {
            Group = parentGroup;
            ID.GroupName = parentGroup.Name;
            buildFromPolygon(poly, map);
            ID.CellName = parentGroup.NewCellName();
            FloorMaterial = new CellMaterialInfo();
            RoofMaterial = new CellMaterialInfo();
        }

        public EditorCell(Cell cell)
            : base(cell)
        {
            GenerateDisplayGeometry();
        }

        public EditorCell(EditorCell cell)
            : base(cell)
        {
            GenerateDisplayGeometry();
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

        public bool buildFromPolygon(Polygon poly, PortalWorld map)
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

        public bool CheckEdges(PortalWorld map)
        {
            bool hasPortal = false;
            foreach (CellEdge edge in Edges)
            {
                edge.Normal = new Vector2(Verts[edge.Start].Bottom.Y - Verts[edge.End].Bottom.Y, -1f * (Verts[edge.Start].Bottom.X - Verts[edge.End].Bottom.X));
                edge.Normal.Normalize();
                edge.Slope = new Vector2(Verts[edge.Start].Bottom.X - Verts[edge.End].Bottom.X, Verts[edge.Start].Bottom.Y - Verts[edge.End].Bottom.Y);
                edge.Slope.Normalize();

                Vector2 p1 = new Vector2(Verts[edge.Start].Bottom.X, Verts[edge.Start].Bottom.Y);
                Vector2 p2 = new Vector2(Verts[edge.End].Bottom.X, Verts[edge.End].Bottom.Y);
                List<Cell> cellsWithEdge = map.CellsThatContainEdge(p1, p2,this);

                edge.Destinations.Clear();

                bool edgeHasPortal = false;

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
                           
                            PortalDestination dest = new PortalDestination();
                            dest.Cell = cell;
                            dest.Group = cell.Group;
                            dest.DestinationCell = cell.ID;

                            dest.SPTop = dest.Cell.RoofPoint(destSP);
                            if (RoofZ(edge.Start) < dest.SPTop.Z)
                                dest.SPTop.Z = RoofZ(edge.Start);

                            dest.EPTop = dest.Cell.RoofPoint(destEP);
                            if (RoofZ(edge.End) < dest.EPTop.Z)
                                dest.EPTop.Z = RoofZ(edge.End);

                            dest.SPBottom = new Vector3(destSP.Bottom);
                            if (Verts[edge.Start].Bottom.Z > dest.SPBottom.Z)
                                dest.SPBottom.Z = Verts[edge.Start].Bottom.Z;

                            dest.EPBottom = new Vector3(destEP.Bottom);
                            if (Verts[edge.End].Bottom.Z > dest.EPBottom.Z)
                                dest.EPBottom.Z = Verts[edge.End].Bottom.Z;

                            edge.Destinations.Add(dest);
                            edgeHasPortal = true;
                            hasPortal = true;
                        }
                    }
                }

                if (edgeHasPortal)
                {
                    if (edge.EdgeType == CellEdgeType.Portal)
                        UpdatePortalGeo(edge);
                    else
                    {
                        // it's going from wall to portal so it will get new data
                        // save off it's first geo and use it for materials
                        CellWallGeometry matGeo = GetFirstGeo(edge);
                        edge.EdgeType = CellEdgeType.Portal;
                        generateWallDef(edge);
                        SetEdgeGeoMat(edge,matGeo);
                    }
                }
                else
                {
                    if (edge.EdgeType == CellEdgeType.Wall)
                        UpdateWallGeo(edge);
                    else
                    {
                        // it's going from a portal to a wall so go and generate new data
                         // save off it's first geo and use it for materials
                        CellWallGeometry matGeo = GetFirstGeo(edge);

                        edge.EdgeType = CellEdgeType.Wall;
                        edge.Destinations.Clear();
                        generateWallDef(edge);
                        SetEdgeGeoMat(edge,matGeo);
                    }
                }
            }

            setupCellGeoData();
            GenerateDisplayGeometry();
            return hasPortal;
        }

        CellWallGeometry GetFirstGeo ( CellEdge edge )
        {
            if (edge.Geometry.Count == 0)
                return null;

            return edge.Geometry[0];
        }

        void SetEdgeGeoMat ( CellEdge edge, CellWallGeometry matGeo )
        {
            if (matGeo == null)
                return;

            foreach(CellWallGeometry geo in edge.Geometry)
            {
                geo.Material.Material = matGeo.Material.Material;
                geo.Material.UVShift = matGeo.Material.UVShift;
                geo.Material.UVScale = matGeo.Material.UVScale;
            }
        }

        void setupCellGeoData ()
        {
            // make vectors for the first 2 edges
            Vector3 v1 = VectorHelper3.Subtract(Verts[1].Bottom, Verts[0].Bottom);
            v1.Normalize();

            int vert = 2;
            Vector3 v2 = VectorHelper3.Subtract(Verts[vert].Bottom, Verts[vert-1].Bottom);
            v2.Normalize();

            while ( v1 == v2)
            {
                vert++;
                if (vert >= Verts.Count)
                {
                    vert = Verts.Count - 1;
                    break;
                }

                v2 = VectorHelper3.Subtract(Verts[vert].Bottom, Verts[vert - 1].Bottom);
                v2.Normalize();
            }

            FloorNormal = Vector3.Cross(-v2, v1);
            FloorNormal.Normalize();

            v1.Z = Verts[1].GetTopZ(HeightIsIncremental) - Verts[0].GetTopZ(HeightIsIncremental);
            v2.Z = Verts[vert].GetTopZ(HeightIsIncremental) - Verts[vert-1].GetTopZ(HeightIsIncremental);

            RoofNormal = Vector3.Cross(v1, -v2);
            RoofNormal.Normalize();
        }

        void UpdateWallGeo ( CellEdge edge )
        {
            if (edge.Geometry.Count != 1)
                generateWallDef(edge);
            else
            {
                CellWallGeometry geo = edge.Geometry[0];

                float heightChange = FloorPoint(edge.Start).Z - geo.LowerZ[0];
                float heightChangeInScaledUV = heightChange / geo.Material.UVScale.Y;
                geo.Material.UVShift.Y += heightChangeInScaledUV;

                geo.LowerZ[0] = FloorPoint(edge.Start).Z;
                geo.LowerZ[1] = FloorPoint(edge.End).Z;
                geo.UpperZ[0] = RoofZ(edge.Start);
                geo.UpperZ[1] = RoofZ(edge.End);
            }
        }

        CellWallGeometry FindMatchingGeo (List<CellWallGeometry> list, CellWallGeometry geo )
        {
            foreach (CellWallGeometry g in list)
            {
                if (geo.Bottom.CellName == g.Bottom.CellName && geo.Bottom.GroupName == g.Bottom.GroupName && geo.Top.GroupName == g.Top.GroupName && geo.Top.CellName == g.Top.CellName)
                    return g;
            }

            return null;
        }

        void UpdatePortalGeo ( CellEdge edge )
        {
            List<CellWallGeometry> newGeo = generateWallDefs(edge);

            foreach (CellWallGeometry geo in newGeo)
            {
                CellWallGeometry oldGeo = FindMatchingGeo(edge.Geometry, geo);
                if (oldGeo != null)
                {
                    geo.Material.UVScale = oldGeo.Material.UVScale;
                    geo.Material.UVShift = oldGeo.Material.UVShift;

                    float heightChange = oldGeo.LowerZ[0] - geo.LowerZ[0];
                    float heightChangeInScaledUV = heightChange / oldGeo.Material.UVScale.Y;
                    geo.Material.UVShift.Y += heightChangeInScaledUV;

                    geo.Material.Material = oldGeo.Material.Material;
                }
                else if (edge.Geometry.Count > 0)
                {
                    geo.Material.Material = edge.Geometry[0].Material.Material;
                }
            }
            edge.Geometry.Clear();
            edge.Geometry = newGeo;
        }


        void generateWallDef( CellEdge edge )
        {
            edge.Geometry.Clear();
            edge.Geometry = generateWallDefs(edge);
        }

        List<CellWallGeometry> generateWallDefs ( CellEdge edge )
        {
            List<CellWallGeometry> Geometry = new List<CellWallGeometry>();

            CellWallGeometry geo;

            if (edge.EdgeType == CellEdgeType.Wall)
            {
                geo = new CellWallGeometry();

                geo.UpperZ[0] = Verts[edge.Start].GetTopZ(HeightIsIncremental);
                geo.UpperZ[1] = Verts[edge.End].GetTopZ(HeightIsIncremental);

                geo.LowerZ[0] = Verts[edge.Start].Bottom.Z;
                geo.LowerZ[1] = Verts[edge.End].Bottom.Z;

                geo.Bottom = ID;
                geo.Top = ID;

                Geometry.Add(geo);
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
                    if (lowestSP.Bottom.Z <= thisSP.Bottom.Z)
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
                        geo.LowerZ[1] = thisEP.Bottom.Z;

                        geo.Bottom = ID;
                        geo.Top = topDest.ID;

                        Geometry.Add(geo);
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

                                geo.Bottom = topDest.ID;
                                geo.Top = bestDest.ID;

                                Geometry.Add(geo);

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

                        geo.Bottom = bestDest.ID;
                        geo.Top = ID;

                        Geometry.Add(geo);
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
                        geo.UpperZ[1] = thisEP.GetTopZ(HeightIsIncremental);

                        geo.Bottom = topDest.ID;
                        geo.Top = ID;

                        Geometry.Add(geo);
                    }
                }
            }

            return Geometry;
        }

        void generateWallDefs()
        {
            foreach (CellEdge edge in Edges)
                generateWallDef(edge);
        }

        void clearGeometry ( )
        {
            if (floor != null)
                floor.Dispose();
            if (roof != null)
                roof.Dispose();
            floor = null;
            roof = null;
          
            foreach (WallGeometry wall in walls)
                wall.Dispose();

            foreach (PortalGeometry portal in portals)
                portal.Dispose();

            walls.Clear();
            portals.Clear();
        }

        public void GenerateDisplayGeometry ( )
        {
            clearGeometry();
        
            if (FloorVizable)
                floor = new CellGeometry(true,this);
            if (RoofVizable)
                roof = new CellGeometry(false, this);

            foreach (CellEdge edge in Edges)
            {
                if (!edge.Vizable)
                    continue;

                foreach (CellWallGeometry geo in edge.Geometry)
                {
                    if (geo.Vizable)
                        walls.Add(new WallGeometry(this, edge, geo));
                }

                if (edge.EdgeType == CellEdgeType.Portal)
                {
                    foreach (PortalDestination dest in edge.Destinations)
                        portals.Add(new PortalGeometry(this, edge, dest));
                }
            }
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
