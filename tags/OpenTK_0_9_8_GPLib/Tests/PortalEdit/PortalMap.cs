using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortalEdit
{
    public class PortalMap
    {
        MapRenderer mapRenderer;
        public List<Cell> cells = new List<Cell>();

        public void AddCell ( Cell cell )
        {
            cells.Add(cell);
        }
    }
}
