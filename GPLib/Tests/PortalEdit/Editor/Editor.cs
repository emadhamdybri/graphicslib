using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;

using OpenTK;

using Drawables.DisplayLists;
using Drawables;

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
                float.TryParse(frame.CellVertList.SelectedRows[0].Cells[2].Value.ToString(), out vert.Top);

                DisplayListSystem.system.Invalidate();
                RebuildMap();
                ResetViews();
            }
            catch (System.Exception ex)
            {
            }
        }

        public void SetCellInZ ( bool inc )
        {
            EditorCell cell = GetSelectedCell();
            if (cell == null)
                return;

            Undo.System.Add(new IncrementalHeightsUndo(cell));

            cell.HeightIsIncremental = inc;
        }

        public void New ()
        {
            FileName = string.Empty;
            mapRenderer.ClearEditPolygon();

            DisplayListSystem.system.Invalidate();
            DrawablesSystem.system.removeAll();
            map.CellGroups.Clear();
            map.MapAttributes.Clear();

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

            DisplayListSystem.system.Invalidate();
            DrawablesSystem.system.removeAll();

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
