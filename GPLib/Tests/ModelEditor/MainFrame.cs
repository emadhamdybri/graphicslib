using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModelEditor
{
    public partial class MainFrame : Form
    {
        public MainFrame()
        {
            InitializeComponent();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild.ty)
        }

        private void staticModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelBaseDoc doc = new StaticModelDoc();
            doc.MdiParent = this;
            doc.Show();
        }

        private void animatedModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModelBaseDoc doc = new AnimatedModelDoc();
            doc.MdiParent = this;
            doc.Show();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }
    }
}
