using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortalEdit
{
    public class PortalMap
    {
        MapRenderer mapRenderer;
        public MapViewRenderer viewRenderer = null;

        List<EditorCell> cells = new List<EditorCell>();

        public PortalMap ( MapRenderer rend )
        {
            mapRenderer = rend;

            mapRenderer.NewPolygon += new NewPolygonHandler(mapRenderer_NewPolygon);
        }

        void UpdateView ()
        {
            if (viewRenderer != null)
                viewRenderer.Render3dView();
        }

        void mapRenderer_NewPolygon(object sender, Polygon polygon)
        {
            cells.Add(new EditorCell(polygon));
            UpdateView();
        }

        public void Draw ( )
        {
            foreach (EditorCell cell in cells)
                cell.Draw();
        }
    }
}
