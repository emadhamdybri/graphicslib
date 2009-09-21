using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;

namespace PortalEdit
{
    public partial class EditFrame : Form
    {
        MapRenderer renderer;
        MapViewRenderer mapViewRenderer;
        PortalMap map;

        public EditFrame()
        {
            InitializeComponent();
            renderer = new MapRenderer(MapView);
            map = new PortalMap(renderer);

            renderer.MouseStatusUpdate += new MouseStatusUpdateHandler(renderer_MouseStatusUpdate);
        }

        void renderer_MouseStatusUpdate(object sender, Point position)
        {
            MousePositionStatus.Text = "Map:" + position.ToString();
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate(true);
            base.OnResize(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (mapViewRenderer == null)
                mapViewRenderer = new MapViewRenderer(GLView, map);

            base.OnLoad(e);
        }
    }
}
