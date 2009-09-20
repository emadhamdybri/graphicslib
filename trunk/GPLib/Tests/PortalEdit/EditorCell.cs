using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortalEdit
{
    public class EditorCell : Cell
    {
        Polygon mapPolygon;

        public EditorCell (Polygon poly) : base()
        {
            mapPolygon = poly;

            // build the polygon for 3d;
        }

        public void Draw ( )
        {

        }
    }
}
