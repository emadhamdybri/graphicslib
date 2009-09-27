using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;

using OpenTK;

using Drawables.DisplayLists;

namespace PortalEdit
{
    class Editor
    {
        public PortalMap map;
        public MapRenderer mapRenderer;
        public MapViewRenderer viewRenderer;
        public EditFrame frame;

        public static Editor instance;

        public Editor(EditFrame _frame, Control mapctl, GLControl view)
        {
            frame = _frame;
            map = new PortalMap();

            mapRenderer = new MapRenderer(mapctl, map);
            viewRenderer = new MapViewRenderer(view, map);

            mapRenderer.NewPolygon += new NewPolygonHandler(mapRenderer_NewPolygon);
            mapRenderer.MouseStatusUpdate += new MouseStatusUpdateHandler(frame.mapRenderer_MouseStatusUpdate);
            mapRenderer.CellSelected += new CellSelectedHander(mapRenderer_CellSelected);
        }

        void mapRenderer_CellSelected(object sender, Cell cell)
        {
            frame.CellList.SelectedItem= cell;
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
            return (EditorCell)frame.CellList.SelectedItem;
        }

        public bool Open ( FileInfo file )
        {
            PortalMap newMap = PortalMap.Read(file);
            if (newMap == null)
                return false;

            mapRenderer.ClearEditPolygon();

            map.cells.Clear();
            foreach(Cell cell in newMap.cells)
                map.cells.Add(new EditorCell(cell));

            map.RebindCells();
            ResetViews();
            return true;
        }

        public bool Save(FileInfo file)
        {
            return map.Write(file);
        }

        void RebuildMap ()
        {
            foreach(EditorCell cell in map.cells)
                cell.CheckEdges(map);
        }

        public bool DeleteCell(EditorCell cell)
        {
            if (cell == null)
                return false;

            cell.Dispose();
            map.RemoveCell(cell);
            RebuildMap();

            ResetViews();
            return true;
        }

        void mapRenderer_NewPolygon(object sender, Polygon polygon)
        {
            EditorCell cell = new EditorCell(polygon,map);
            cell.tag = polygon;

            map.AddCell(cell);

            foreach (Cell c in map.cells )
            {
                EditorCell eCell = (EditorCell)c;
                eCell.CheckEdges(map);
            }

            DisplayListSystem.system.Invalidate();
            ResetViews();
        }
    }
}
