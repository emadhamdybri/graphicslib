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
        Editor editor;

        public EditFrame()
        {
            InitializeComponent();
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate(true);
            base.OnResize(e);
        }

        public void mapRenderer_MouseStatusUpdate(object sender, System.Drawing.Point position)
        {
            MousePositionStatus.Text = "Map:" + position.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (editor == null)
                editor = new Editor(this,MapView, GLView);
            base.OnLoad(e);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
