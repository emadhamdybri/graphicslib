using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;

using OpenTK;

using Drawables.DisplayLists;
using Drawables;

using Math3D;

namespace PortalEdit
{
    public enum ViewEditMode
    {
        Select,
        Paint,
    }

    public delegate void MapLoadedHandler ( object sender, EventArgs args );

    class Editor
    {
        public PortalMap map;
        public MapRenderer mapRenderer;
        public MapViewRenderer viewRenderer;
        public EditFrame frame;

        public static Editor instance;

        public static float EditZFloor = 0;
        public static float EditZRoof = 2;
        public static bool EditZInc = true;

        public ViewEditMode viewEditMode = ViewEditMode.Select;

        public event MapLoadedHandler MapLoaded;

        MapViewRenderer.CellClickedEventArgs lastSelectionArgs = null;

        public String FileName = string.Empty;

        bool Dirty = false;

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
            map = new PortalMap();

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
                SelectObject(e.cell);
        
            lastSelectionArgs = e;
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
            PortalMap newMap = PortalMap.Read(file);
            if (newMap == null)
                return false;

            mapRenderer.ClearEditPolygon();

            viewRenderer.UnloadMapGraphics();

            map.MapAttributes.Clear();
            map.MapAttributes = newMap.MapAttributes;
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

        public void RenameGroup ( String oldName, String newName )
        {
            foreach (CellGroup group in map.CellGroups)
            {
                if (group.Name == oldName)
                    group.Name = newName;

                foreach (EditorCell cell in group.Cells)
                {
                    if (cell.GroupName == oldName)
                        cell.GroupName = newName;

                    foreach( CellEdge edge in cell.Edges )
                    {
                        foreach (PortalDestination dest in edge.Destinations)
                        {
                            if (dest.GroupName == oldName)
                                dest.GroupName = newName;
                        }
                    }
                }
            }
            Editor.SetDirty();
            ResetViews();
        }

        public void RenameCell(EditorCell cell, String newName)
        {
            cell.Name = newName;

            foreach (CellGroup group in map.CellGroups)
            {
                foreach (EditorCell tehCell in group.Cells)
                {
                    foreach (CellEdge edge in tehCell.Edges)
                    {
                        foreach (PortalDestination dest in edge.Destinations)
                        {
                            if (dest.Cell == cell)
                                dest.CellName = newName;
                        }
                    }
                }
            }
            Editor.SetDirty();
            ResetViews();
        }

        public void SetCellGroup ( String name, EditorCell cell )
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
            cell.GroupName = newGroup.Name;

            RebuildMap();
            ResetViews();
        }

        public void SetCellGroup ( String name )
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

            RebuildMap();
            ResetViews();
        }

        public void SetCellVertXY ( Vector2 pos, int vertIndex, EditorCell cell)
        {
            if (cell == null)
                return;

            cell.Verts[vertIndex].Bottom.X = pos.X;
            cell.Verts[vertIndex].Bottom.Y = pos.Y;
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
                    group = map.FindGroup(selCel.GroupName);

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
    }
}
