using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

using OpenTK;

namespace PortalEdit
{
    class Editor
    {
        PortalMap map;
        MapRenderer mapRenderer;
        MapViewRenderer viewRenderer;
        EditFrame   frame;

        public Editor(EditFrame _frame, Control mapctl, GLControl view)
        {
            frame = _frame;
            map = new PortalMap();

            mapRenderer = new MapRenderer(mapctl);
            viewRenderer = new MapViewRenderer(view, map);

            mapRenderer.NewPolygon += new NewPolygonHandler(mapRenderer_NewPolygon);
            mapRenderer.MouseStatusUpdate += new MouseStatusUpdateHandler(frame.mapRenderer_MouseStatusUpdate);
        }

        void mapRenderer_NewPolygon(object sender, Polygon polygon)
        {
            EditorCell cell = new EditorCell(polygon);
            cell.tag = polygon;
            map.AddCell(cell);

            viewRenderer.Render3dView();
        }
    }
}
