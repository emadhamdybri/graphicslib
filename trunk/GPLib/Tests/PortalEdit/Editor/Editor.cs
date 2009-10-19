using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Drawing;

using OpenTK;

using Drawables;
using Drawables.DisplayLists;
using Drawables.Textures;

using Math3D;

using World;

using coldet;

namespace PortalEdit
{
    public enum ViewEditMode
    {
        Select,
        Paint,
    }

    public delegate void MapLoadedHandler ( object sender, EventArgs args );

    public delegate void EditorSelectonChanged ( object sender, EventArgs args );

    class Editor
    {
        public PortalWorld map;
        public MapRenderer mapRenderer;
        public MapViewRenderer viewRenderer;
        public EditFrame frame;

        public static Editor instance;

        public static float EditZFloor = 0;
        public static float EditZRoof = 2;
        public static bool EditZInc = true;

        public ViewEditMode viewEditMode = ViewEditMode.Select;

        public event MapLoadedHandler MapLoaded;
        public event EditorSelectonChanged SelectionChanged;

        MapViewRenderer.CellClickedEventArgs lastSelectionArgs = null;

        public string FileName = string.Empty;

        bool Dirty = false;

        Color ambientLightColor;

        ColdetModel colModel = null;


        public static void SetDirty()
        {
            instance.Dirty = true;
        }

        public bool IsDirty ()
        {
            if (!map.Valid())
                return false;

            return Dirty;
        }

        public Editor(EditFrame _frame, Control mapctl, GLControl view)
        {
            instance = this;

            frame = _frame;
            map = new PortalWorld();

            mapRenderer = new MapRenderer(mapctl, map);
            viewRenderer = new MapViewRenderer(view, map);

            mapRenderer.NewPolygon += new NewPolygonHandler(mapRenderer_NewPolygon);
            mapRenderer.MouseStatusUpdate += new MouseStatusUpdateHandler(frame.mapRenderer_MouseStatusUpdate);
            mapRenderer.CellSelected += new CellSelectedHander(mapRenderer_CellSelected);

            viewRenderer.CellClicked += new MapViewRenderer.CellClickedEventHandler(viewRenderer_CellClicked);

            NewGroup(false);
        }

        void viewRenderer_CellClicked(object sender, MapViewRenderer.CellClickedEventArgs e)
        {
            if (viewEditMode == ViewEditMode.Select)
            {
                SelectObject(e.cell);
                lastSelectionArgs = e;

                if (SelectionChanged != null)
                    SelectionChanged(this, EventArgs.Empty);
            }
            else if (viewEditMode == ViewEditMode.Paint)
                PaintFace(e);
        }

        void PaintFace(MapViewRenderer.CellClickedEventArgs e)
        {
            if (e.cell == null ||frame.TextureList.SelectedNode == null || frame.TextureList.SelectedNode.Tag == null)
                return;

            string texture = String.Copy(frame.TextureList.SelectedNode.Tag.ToString());

            if (e.edge < 0)
            {
                if (e.floor)
                    e.cell.FloorMaterial.Material = texture;
                else if (e.roof)
                    e.cell.RoofMaterial.Material = texture;
            }
            else
                e.geo.Material.Material = texture;

            EditorCell cell = (EditorCell)e.cell;
            cell.GenerateDisplayGeometry();
            ResetViews();
        }

        TreeNode FindSelectedNode ( object tag, TreeNode node )
        {
            if (node.Tag == tag)
                return node;

            foreach (TreeNode child in node.Nodes)
            {
                TreeNode foundNode = FindSelectedNode(tag, child);
                if (foundNode != null)
                    return foundNode;
            }

            return null;
        }

        void mapRenderer_CellSelected(object sender, Cell cell)
        {
            SelectObject(cell);
        }

        public void SelectMapItem ( object item )
        {
            if (item == null || item.GetType() == typeof(CellGroup))
                lastSelectionArgs = new MapViewRenderer.CellClickedEventArgs(null, -1, null, false, false);
            else if (item.GetType() == typeof(EditorCell))
                lastSelectionArgs = new MapViewRenderer.CellClickedEventArgs((Cell)item, -1, null, false, false);
        }

        void SelectObject ( object item )
        {
            TreeNode selectedNode = null;

            foreach (TreeNode child in frame.MapTree.Nodes)
            {
                selectedNode = FindSelectedNode(item, child);
                if (selectedNode != null)
                    break;
            }

            frame.MapTree.SelectedNode = selectedNode;
            frame.Invalidate(true);

            SelectMapItem(item);
        }

        public void NewObject ( ObjectInstance obj )
        {
            SetDirty();
            obj.cells = map.FindCells(obj.Postion);
            map.MapObjects.Add(obj);
            ResetViews();
            frame.LoadObjectList();
        }

        public void MoveObject ( ObjectInstance obj, Vector3 pos )
        {
            SetDirty();
            obj.Postion = pos;
            obj.cells = map.FindCells(obj.Postion);
        }

        public void NewGroup ()
        {
            NewGroup(true);
        }

        public void NewGroup ( bool undo )
        {
            CellGroup group = new CellGroup();
            group.Name = map.NewGroupName();
            map.CellGroups.Add(group);

            if (undo)
            Undo.System.Add(new GroupAddUndo(group));
            ResetViews();
            SelectObject(group);
        }

        protected void ResetViews ()
        {
            DisplayListSystem.system.Invalidate();
            frame.populateCellList();
            mapRenderer.Redraw();
            viewRenderer.Render3dView();
        }

        public EditorCell GetSelectedCell ( )
        {
            if (lastSelectionArgs == null)
                return null;

            return (EditorCell)lastSelectionArgs.cell;
        }

        public CellGroup GetSelectedGroup()
        {
            if (frame.MapTree.SelectedNode == null)
                return null;

            object tag = frame.MapTree.SelectedNode.Tag;
            if (tag.GetType() == typeof(CellGroup))
                return (CellGroup)tag;

            return null;
        }

        public CellVert GetSelectedVert()
        {
            Cell cell = GetSelectedCell();
            if (cell == null)
                return null;

            int index = GetSelectedVertIndex();
            if (index >= 0)
                return cell.Verts[index];
             return null;
        }

        public CellEdge GetSelectedEdge()
        {
            if (lastSelectionArgs == null || lastSelectionArgs.cell == null || lastSelectionArgs.edge < 0)
                return null;
            return lastSelectionArgs.cell.Edges[lastSelectionArgs.edge];
        }

        public CellWallGeometry GetSelectedWallGeo()
        {
            if (lastSelectionArgs == null || lastSelectionArgs.cell == null || lastSelectionArgs.edge < 0 || lastSelectionArgs.geo == null)
                return null;
            return lastSelectionArgs.geo;
        }

        public bool GetFloorSelection()
        {
            if (lastSelectionArgs == null || lastSelectionArgs.cell == null)
                return false;
            return lastSelectionArgs.floor;
        }

        public bool GetRoofSelection()
        {
            if (lastSelectionArgs == null || lastSelectionArgs.cell == null)
                return false;
            return lastSelectionArgs.roof;
        }

        public int GetSelectedVertIndex()
        {
            Cell cell = GetSelectedCell();
            if (cell == null)
                return -1;

            if (frame.CellVertList.SelectedRows.Count > 0)
            {
                int index = int.Parse(frame.CellVertList.SelectedRows[0].Cells[0].Value.ToString());
                return index;
            }
            return -1;
        }

        public CellMaterialInfo GetSelectedMaterialInfo ()
        {
            if (lastSelectionArgs == null || lastSelectionArgs.cell == null)
                return null;

            if (lastSelectionArgs.floor)
                return lastSelectionArgs.cell.FloorMaterial;
            if (lastSelectionArgs.roof)
                return lastSelectionArgs.cell.RoofMaterial;
            if (lastSelectionArgs.geo != null)
                return lastSelectionArgs.geo.Material;

            return null;
        }

        public void EditVert ()
        {
            CellVert vert = GetSelectedVert();
            if (vert == null)
                return;

            Undo.System.Add(new VertexDataEditUndo(GetSelectedCell(), GetSelectedVertIndex()));
            try
            {
                float.TryParse(frame.CellVertList.SelectedRows[0].Cells[1].Value.ToString(), out vert.Bottom.Z);
                float.TryParse(frame.CellVertList.SelectedRows[0].Cells[3].Value.ToString(), out vert.Top);

                DisplayListSystem.system.Invalidate();
                GetSelectedCell().Invaldate();
                RebuildMap();
                ResetViews();
            }
            catch (System.Exception ex)
            {
            }
        }

        public void SetRoofVertToPlane ()
        {
            EditorCell cell = GetSelectedCell();
            CellVert vert = GetSelectedVert();
            if (vert == null || cell == null)
                return;

            Plane plane = cell.GetRoofPlane();
            float newZ = Cell.GetZInPlane(plane, vert.Bottom.X, vert.Bottom.Y);
            if (newZ < vert.Bottom.Z)
                return;

            Undo.System.Add(new VertexDataEditUndo(cell, GetSelectedVertIndex()));

            if (cell.HeightIsIncremental)
                vert.Top = newZ - vert.Bottom.Z;
            else
                vert.Top = newZ;

            DisplayListSystem.system.Invalidate();
            cell.Invaldate();
            RebuildMap();
            ResetViews();
        }

        public void SetFloorVertToPlane ()
        {
            EditorCell cell = GetSelectedCell();
            CellVert vert = GetSelectedVert();
            if (vert == null || cell == null)
                return;

            Plane plane = cell.GetFloorPlane();
            float newZ = Cell.GetZInPlane(plane, vert.Bottom.X, vert.Bottom.Y);

            Undo.System.Add(new VertexDataEditUndo(cell, GetSelectedVertIndex()));
            
            vert.Bottom.Z = newZ;

            DisplayListSystem.system.Invalidate();
            cell.Invaldate();
            RebuildMap();
            ResetViews();
        }

        public void SetCellInZ ( bool inc )
        {
            EditorCell cell = GetSelectedCell();
            if (cell == null)
                return;

            Undo.System.Add(new IncrementalHeightsUndo(cell));

            cell.HeightIsIncremental = inc;
            cell.Invaldate();
        }

        public void SetCellEdgeViz ( EditorCell cell, int edge, bool vis )
        {
            Dirty = true;

            Undo.System.Add(new EdgeVisUndo(cell, edge));

            CellEdge e = cell.Edges[edge];
            e.Vizable = vis;
            cell.GenerateDisplayGeometry();
            ResetViews();
        }

        public void SetCellFloorViz(EditorCell cell, bool vis)
        {
            Dirty = true;

            Undo.System.Add(new EdgeVisUndo(cell, -2));

            cell.FloorVizable = vis;
            cell.GenerateDisplayGeometry();
            ResetViews();
        }

        public void SetCellRoofViz(EditorCell cell, bool vis)
        {
            Dirty = true;

            Undo.System.Add(new EdgeVisUndo(cell, -1));

            cell.RoofVizable = vis;
            cell.GenerateDisplayGeometry();
            ResetViews();
        }

        public void RebuildMapGeo ()
        {
            foreach (CellGroup group in map.CellGroups)
            {
                foreach (EditorCell cell in group.Cells)
                    cell.GenerateDisplayGeometry();
            }
        }
        public void New ()
        {
            FileName = string.Empty;
            mapRenderer.ClearEditPolygon();

            viewRenderer.UnloadMapGraphics();

            map.MapObjects.Clear();
            map.CellGroups.Clear();
            map.MapAttributes.Clear();
            NewGroup();

            Dirty = false;

            if (MapLoaded != null)
                MapLoaded(this, EventArgs.Empty);


            ResetViews();
        }

        public bool Open ( FileInfo file )
        {
            FileName = file.FullName;
            PortalWorld newMap = PortalWorld.Read(file);
            if (newMap == null)
                return false;

            mapRenderer.ClearEditPolygon();

            viewRenderer.UnloadMapGraphics();

            map.MapAttributes.Clear();
            map.MapAttributes = newMap.MapAttributes;
            map.MapObjects = newMap.MapObjects;
            map.AmbientLight = newMap.AmbientLight;
            map.Lights = newMap.Lights;
            map.CellGroups.Clear();

            foreach (CellGroup group in newMap.CellGroups)
            {
                CellGroup newGroup = new CellGroup();
                newGroup.Name = group.Name;
                map.CellGroups.Add(newGroup);

                foreach (Cell cell in group.Cells)
                    newGroup.Cells.Add(new EditorCell(cell));
            }

            map.RebindCells();
            Dirty = false;

            if (MapLoaded != null)
                MapLoaded(this, EventArgs.Empty);

            ResetViews();
            return true;
        }

        public bool Save(FileInfo file)
        {
            Dirty = false;

            FileName = file.FullName;
            return map.Write(file);
        }

        public void RebuildMap ()
        {
            Dirty = true;

            foreach (CellGroup group in map.CellGroups)
                foreach (EditorCell cell in group.Cells)
                    cell.CheckEdges(map);
        }

        public bool DeleteCell(EditorCell cell)
        {
            if (cell == null)
                return false;

            Undo.System.Add(new CellDeleteUndo(cell));

            SelectMapItem(null);
            cell.Dispose();
            map.RemoveCell(cell);
            RebuildMap();

            ResetViews();
            return true;
        }

        public void RenameGroup ( string oldName, string newName )
        {
            foreach (CellGroup group in map.CellGroups)
            {
                if (group.Name == oldName)
                    group.Name = newName;

                foreach (EditorCell cell in group.Cells)
                {
                    if (cell.ID.GroupName == oldName)
                        cell.ID.GroupName = newName;

                    foreach( CellEdge edge in cell.Edges )
                    {
                        foreach (PortalDestination dest in edge.Destinations)
                        {
                            if (dest.DestinationCell.GroupName == oldName)
                                dest.DestinationCell.GroupName = newName;
                        }

                        foreach (CellWallGeometry geo in edge.Geometry)
                        {
                            if (geo.Bottom.GroupName == oldName)
                                geo.Bottom.GroupName = newName;

                            if (geo.Top.GroupName == oldName)
                                geo.Top.GroupName = newName;
                        }
                    }
                }
            }
            Editor.SetDirty();
            ResetViews();
        }

        public void RenameCell(EditorCell cell, string newName)
        {
            string oldName = String.Copy(cell.ID.CellName);
            cell.ID.CellName = newName;

            foreach (CellGroup group in map.CellGroups)
            {
                foreach (EditorCell tehCell in group.Cells)
                {
                    foreach (CellEdge edge in tehCell.Edges)
                    {
                        foreach (PortalDestination dest in edge.Destinations)
                        {
                            if (dest.Cell == cell)
                                dest.DestinationCell.CellName = newName;
                        }

                        foreach (CellWallGeometry geo in edge.Geometry)
                        {
                            if (geo.Bottom.GroupName == cell.ID.GroupName && geo.Bottom.CellName == oldName)
                                geo.Bottom.CellName = newName;
                            if (geo.Bottom.GroupName == string.Empty && tehCell == cell && geo.Bottom.CellName == oldName)
                                geo.Bottom.CellName = newName;

                            if (geo.Top.GroupName == cell.ID.GroupName && geo.Top.CellName == oldName)
                                geo.Top.CellName = newName;
                            if (geo.Top.GroupName == string.Empty && tehCell == cell && geo.Top.CellName == oldName)
                                geo.Top.CellName = newName;
                        }
                    }
                }
            }
            Editor.SetDirty();
            ResetViews();
        }

        public void SetCellGroup ( string name, EditorCell cell )
        {
            if (cell == null)
                return;

            CellGroup newGroup = map.FindGroup(name);
            if (newGroup == cell.Group)
                return;

            Undo.System.Add(new CellGroupChangeUndo(cell, name));

            cell.Group.Cells.Remove(cell);
            newGroup.Cells.Add(cell);
            cell.Group = newGroup;
            cell.ID.GroupName = newGroup.Name;

            RebuildMap();
            ResetViews();
        }

        public void SetCellGroup ( string name )
        {
            SetCellGroup(name,GetSelectedCell());
        }

        public void SetCellFloor ( float val, bool translate, EditorCell cell )
        {
            if (cell == null)
                return;

            foreach( CellVert vert in cell.Verts)
            {
                if (translate)
                    vert.Bottom.Z += val;
                else
                    vert.Bottom.Z = val;
            }

            RebuildMap();
            ResetViews();
        }

        public void SetCellFloor ( float val, bool translate )
        {
            SetCellFloor(val,translate,GetSelectedCell());
        }

        public void SetCellRoof(float val, bool translate, EditorCell cell)
        {
            if (cell == null)
                return;

            foreach (CellVert vert in cell.Verts)
            {
                if (translate)
                    vert.Top += val;
                else
                {
                    if (cell.HeightIsIncremental)
                        vert.Top = val - vert.Bottom.Z;
                    else
                        vert.Top = val;
                }
            }
            cell.Invaldate();
            RebuildMap();
            ResetViews();
        }

        public void SetCellRoof(float val, bool translate)
        {
            SetCellRoof(val, translate, GetSelectedCell());
        }

        public void SetCellToPreset ( float floor, float roof, bool incZ, EditorCell cell)
        {
            if (cell == null)
                return;

            cell.HeightIsIncremental = incZ;
            foreach (CellVert vert in cell.Verts)
            {
                vert.Bottom.Z = floor;
                vert.Top = roof;

            }
            cell.Invaldate();
            RebuildMap();
            ResetViews();
        }

        public void SetCellVertXY ( Vector2 pos, int vertIndex, EditorCell cell)
        {
            if (cell == null)
                return;

            cell.Verts[vertIndex].Bottom.X = pos.X;
            cell.Verts[vertIndex].Bottom.Y = pos.Y;
            cell.Invaldate();
            RebuildMap();
            ResetViews();
        }

        void mapRenderer_NewPolygon(object sender, Polygon polygon)
        {
            if (map.CellGroups.Count == 0)
                return;

            CellGroup group = GetSelectedGroup();
            if (group == null)
            {
                Cell selCel = GetSelectedCell();
                if (selCel != null)
                    group = selCel.Group;

                if (group == null && selCel != null)
                    group = map.FindGroup(selCel.ID);

                if (group == null)
                    group = map.CellGroups[map.CellGroups.Count - 1];
            }

            EditorCell cell = new EditorCell(polygon, map, group);
            Undo.System.Add(new CellAddUndo(cell, polygon));

            group.Cells.Add(cell);

            foreach(CellGroup g in map.CellGroups)
            {
                foreach (Cell c in g.Cells )
                {
                    EditorCell eCell = (EditorCell)c;
                    eCell.CheckEdges(map);
                }
            }

            DisplayListSystem.system.Invalidate();
            ResetViews();
        }

        public void NewLight()
        {
            SetDirty();
            map.Lights.Add(new LightInstance());
            ResetViews();
            frame.LoadLightList();
        }

        public void MoveLight ( LightInstance light, Vector3 pos )
        {
            SetDirty();
            light.Position = pos;
            ResetViews();
            frame.LoadLightList();
        }

        ColdetModel buildCollisionModel()
        {
            ColdetModel model = Coldet.NewCollisionModel();

            foreach (CellGroup group in map.CellGroups)
            {
                foreach (EditorCell cell in group.Cells)
                {
                    for (int i = 2; i < cell.Verts.Count; i++)
                    {
                        model.AddTriangle(cell.Verts[0].Bottom, cell.Verts[i - 1].Bottom, cell.Verts[i].Bottom);
                        model.AddTriangle(cell.RoofPoint(0), cell.RoofPoint(i), cell.RoofPoint(i - 1));
                    }

                    foreach (CellEdge edge in cell.Edges)
                    {
                        Vector3 sp = cell.Verts[edge.Start].Bottom;
                        Vector3 ep = cell.Verts[edge.End].Bottom;

                        foreach (CellWallGeometry geo in edge.Geometry)
                        {
                            model.AddTriangle(ep.X,ep.Y,geo.LowerZ[1], sp.X,sp.Y,geo.LowerZ[0], sp.X,sp.Y,geo.UpperZ[0]);
                            model.AddTriangle(ep.X, ep.Y, geo.LowerZ[1], sp.X, sp.Y, geo.UpperZ[0], ep.X, ep.Y, geo.UpperZ[1]);
                        }
                    }
                }
            }
            model.FinalizeMesh();

            return model;
        }
       
        protected Color GetColorForLightmapPos ( Vector3 pos, Vector3 normal, Color initalColor )
        {
            return GetColorForLightmapPos(pos, normal, initalColor, false);
        }

        protected float GetLightAttenuationFactor ( LightInstance light, float dotToSurface, float distance )
        {
            float attenuation = 1;

            if (distance > light.MinRadius)
            {
                float scaler = distance - light.MinRadius;
                attenuation = (1f / (distance * distance)) * light.Inensity;
            }

            float intensity = light.Inensity * attenuation;
            if (intensity > 1f)
                intensity = 1f;

            return intensity * (float)Math.Abs(dotToSurface);
        }

        protected Color GetColorForLightmapPos ( Vector3 pos, Vector3 normal, Color initalColor, bool showDebugRay )
        {
            Color returnColor = Color.FromArgb(initalColor.A,initalColor.R,initalColor.G,initalColor.B);
            foreach (LightInstance light in map.Lights)
            {
                Vector3 vecToLight = VectorHelper3.Subtract(pos,light.Position);

                float mag = vecToLight.Length;
                vecToLight.Normalize();

                float dot = Vector3.Dot(vecToLight, normal);
                if (dot < 0)
                {
                    float lightValue = GetLightAttenuationFactor(light, dot, mag);
                    
                    if (lightValue > 0.01f && !colModel.RayCollision(light.Position, vecToLight, false, 0.1f, mag - 0.1f))
                    {
                        if (showDebugRay)
                        {
                            RayTestDebugInfo info = new RayTestDebugInfo();
                            info.Origin = light.Position;
                            info.vector = vecToLight;
                            info.mag = mag;
                            viewRenderer.debugRays.Add(info);
                        }

                        Byte c = (Byte)(255 * lightValue);
                        Byte R = returnColor.R;
                        Byte G = returnColor.G;
                        Byte B = returnColor.B;

                        if ((int)returnColor.R + (int)c > 255)
                            R = 255;
                        else
                            R += c;

                        if ((int)returnColor.G + (int)c > 255)
                            G = 255;
                        else
                            G += c;

                        if ((int)returnColor.B + (int)c > 255)
                            B = 255;
                        else
                            B += c;

                        returnColor = Color.FromArgb(R, G, B);
                    }
                    else
                    {
                        if (showDebugRay)
                        {
                            RayTestDebugInfo info = new RayTestDebugInfo();
                            info.Origin = light.Position;
                            info.vector = vecToLight;
                            info.mag = mag;
                            info.HitDist = (Vector3)colModel.GetCollisionPoint();
                            viewRenderer.debugRays.Add(info);
                        }
                    }
                }
            }

            return returnColor;
        }

        protected Color GetDarkestColor ( List<Color> colors )
        {
            float luminance = (colors[0].R + colors[0].B + colors[0].G) / 3f;

            Color retColor = colors[0];

            for ( int i = 1; i < colors.Count; i++)
            {
                float l = (colors[i].R + colors[i].B + colors[i].G) / 3f;
                if (l < luminance)
                {
                    luminance = l;
                    retColor = colors[i];
                }
            }

            return retColor;
        }

        protected void BuildLightmapForCellWall ( EditorCell cell, CellWallGeometry geo, CellEdge edge )
        {
            cell.GenerateLightmapForWall(geo, edge);
            Application.DoEvents();
            
            Graphics graphics = Graphics.FromImage(geo.Lightmap.Map);
            graphics.Clear(ambientLightColor);
            graphics.Dispose();
            Bitmap bitmap = geo.Lightmap.Map as Bitmap;

            float pixelInUnits = 1.0f / geo.Lightmap.UnitSize;
            Vector3 XStep = VectorHelper3.Subtract(cell.FloorPoint(edge.End), cell.FloorPoint(edge.Start));
            Vector3 Start = new Vector3(cell.FloorPoint(edge.Start));
            XStep.Z = 0;
            XStep.Normalize();

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    // compute the color at all 4 corners of the pixel and take the darkest one
                    List<Color> colors = new List<Color>();
                    Vector3 pos = Start + (XStep * (x * pixelInUnits));
                    pos.Z += y * pixelInUnits;
                    
                    Vector3 realNormal = new Vector3(edge.Normal);

                    bool debug = GetSelectedWallGeo() == geo;

                    colors.Add(GetColorForLightmapPos(pos, realNormal, bitmap.GetPixel(x, y)));

                  /*  pos = Start + (XStep * ((x + 1) * pixelInUnits));
                    pos.Z += y * pixelInUnits;
                    colors.Add(GetColorForLightmapPos(pos, realNormal, bitmap.GetPixel(x, y)));

                    pos = Start + (XStep * ((x + 1) * pixelInUnits));
                    pos.Z += (y+1) * pixelInUnits;
                    colors.Add(GetColorForLightmapPos(pos, realNormal, bitmap.GetPixel(x, y)));

                    pos = Start + (XStep * ((x) * pixelInUnits));
                    pos.Z += (y + 1) * pixelInUnits;
                    colors.Add(GetColorForLightmapPos(pos, realNormal, bitmap.GetPixel(x, y))); */

                    bitmap.SetPixel(x, y, GetDarkestColor(colors));
                }
            }
        }

        protected void BuildLightmapForCellFloor(EditorCell cell )
        {
            cell.GenerateLightmapForFloor();
            Application.DoEvents();

            Graphics graphics = Graphics.FromImage(cell.FloorLightmap.Map);
            graphics.Clear(ambientLightColor);
            graphics.Dispose();
            Bitmap bitmap = cell.FloorLightmap.Map as Bitmap;

            float pixelInUnits = 1.0f / cell.FloorLightmap.UnitSize;

            Vector2 startPos = cell.FindMinXY();

            Plane plane = cell.GetFloorPlane();

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Vector3 pos = new Vector3();
                    pos.X = startPos.X + (x * pixelInUnits);
                    pos.Y = startPos.Y + (y * pixelInUnits);
                    pos.Z = Cell.GetZInPlane(plane, pos.X, pos.Y);

                    bitmap.SetPixel(x, y, GetColorForLightmapPos(pos, cell.FloorNormal, bitmap.GetPixel(x, y)));
                }
            }
        }

        protected void BuildLightmapForCellRoof(EditorCell cell)
        {
            cell.GenerateLightmapForRoof();

            Application.DoEvents();

            Graphics graphics = Graphics.FromImage(cell.RoofLightmap.Map);
            graphics.Clear(ambientLightColor);
            graphics.Dispose();
            Bitmap bitmap = cell.RoofLightmap.Map as Bitmap;

            float pixelInUnits = 1.0f / cell.RoofLightmap.UnitSize;

            Vector2 startPos = cell.FindMinXY();

            Plane plane = cell.GetRoofPlane();

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Vector3 pos = new Vector3();
                    pos.X = startPos.X + (x * pixelInUnits);
                    pos.Y = startPos.Y + (y * pixelInUnits);
                    pos.Z = Cell.GetZInPlane(plane, pos.X, pos.Y);

                    bitmap.SetPixel(x, y, GetColorForLightmapPos(pos, cell.RoofNormal, bitmap.GetPixel(x, y)));
                }
            }
        }

        public void ComputeLightmaps ()
        {
            viewRenderer.debugRays.Clear();

            LightmapProgress progress = new LightmapProgress();

            progress.Progress.Step = 1;
            progress.Progress.Maximum = map.CountFaces();
            progress.Progress.Minimum = 0;

            progress.ProgressText.Text = "Starting Lightmaping";

            progress.Show(frame);
            progress.Update();

            SetDirty();
            Byte v = (Byte)(255*map.AmbientLight);
            ambientLightColor = Color.FromArgb(255, v, v, v);

            colModel = buildCollisionModel();

            int cellCount = 0;
            foreach(CellGroup group in map.CellGroups)
            {
                foreach (EditorCell cell in group.Cells)
                {
                    cellCount++;

                    progress.Progress.PerformStep();
                    progress.ProgressText.Text = "Floor for cell " + cellCount.ToString();
                    progress.Update();
                    BuildLightmapForCellFloor(cell);

                    progress.Progress.PerformStep();
                    progress.ProgressText.Text = "Roof for cell " + cellCount.ToString();
                    progress.Update();
                    BuildLightmapForCellRoof(cell);

                    int edgeCount = 0;
                    foreach (CellEdge edge in cell.Edges)
                    {
                        edgeCount++;
                        progress.ProgressText.Text = "Edge " +edgeCount.ToString() + " for cell " + cellCount.ToString();
                        progress.Update();

                        foreach (CellWallGeometry geo in edge.Geometry)
                        {
                            progress.Progress.PerformStep();
                            progress.Update();
                            BuildLightmapForCellWall(cell, geo, edge);
                        }
                    }
                }
            }
            progress.ProgressText.Text = "Done";
            progress.Update();
            progress.Close();

            colModel = null;
            TextureSystem.system.FlushAllImageTextures();
            RebuildMapGeo();
            ResetViews();
        }
    
        float RayHitsPlane ( Vector3 origin, Vector3 vector, Plane plane )
        {
            float top = -plane.D - (plane.Normal.X * origin.X) - (plane.Normal.Y * origin.Y) - (plane.Normal.Z * origin.Z);
            float bottom = (plane.Normal.X * vector.X) + (plane.Normal.Y * vector.Y) + (plane.Normal.Z * vector.Z);

            if (Math.Abs(bottom) < 0.00001f)
                return -1;
            return top / bottom;
        }

        public bool PointInPolygon ( Vector3 point, Vector3[] polygon, Vector3 polygonNormal )
        {
            for (int i = 1; i < polygon.Length; i++ )
            {
                Vector3 edge = VectorHelper3.Subtract(polygon[i], polygon[i - 1]);
                Vector3 normal = Vector3.Cross(edge, polygonNormal);

                Vector3 vecToPoint = VectorHelper3.Subtract(point, polygon[i]);
                if (Vector3.Dot(vecToPoint, normal) < 0)
                    return false;
            }

            return true;
        }
        
        public bool RayHitsFace(Vector3 origin, Vector3 vector ,float maxDist )
        {
            foreach (CellGroup group in map.CellGroups )
            {
                foreach (EditorCell cell in group.Cells)
                {
                    // check the floors and roofs
                    List<Vector3> cellVerts = new List<Vector3>();

                    foreach (CellVert vert in cell.Verts)
                        cellVerts.Add(vert.Bottom);

                    Plane plane = cell.GetFloorPlane();

                    float t = RayHitsPlane(origin, vector, plane);
                    if (t > 0 && t < maxDist)
                    {
                        Vector3 pointInPlane = vector * t;

                        if (PointInPolygon(pointInPlane, cellVerts.ToArray(), plane.Normal))
                            return true;
                    }

                    plane = cell.GetRoofPlane();

                    t = RayHitsPlane(origin, vector, plane);
                    if (t > 0 && t < maxDist)
                    {
                        Vector3 pointInPlane = vector * t;


                        cellVerts.Clear();

                        foreach (CellVert vert in cell.Verts)
                            cellVerts.Add(cell.RoofPoint(vert));

                        cellVerts.Reverse();

                        if (PointInPolygon(pointInPlane, cellVerts.ToArray(), plane.Normal))
                            return true;
                    }
                
                    // check the walls
                    foreach (CellEdge edge in cell.Edges)
                    {
                        // see if the ray hits the edge plane at all

                        plane = cell.GetEdgePlane(edge);

                        t = RayHitsPlane(origin, vector, plane);
                        if (t > 0 && t < maxDist)
                        {
                            Vector3 pointInPlane = vector * t;

                            foreach (CellWallGeometry geo in edge.Geometry)
                            {
                                // see if the point is inside the edges of the geometry

                                List<Vector3> poly = new List<Vector3>();
                                Vector3 SPBottom = cell.FloorPoint(edge.Start);
                                SPBottom.Z = geo.LowerZ[0];

                                Vector3 EPBottom = cell.FloorPoint(edge.End);
                                EPBottom.Z = geo.LowerZ[1];

                                Vector3 SPTop = cell.RoofPoint(edge.Start);
                                SPTop.Z = geo.UpperZ[0];
                                
                                Vector3 EPTop = cell.RoofPoint(edge.End);
                                EPTop.Z = geo.UpperZ[1];

                                poly.Add(EPBottom);
                                poly.Add(SPBottom);
                                poly.Add(SPTop);
                                poly.Add(EPTop);

                                if (PointInPolygon(pointInPlane,poly.ToArray(),new Vector3(edge.Normal)))
                                    return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
