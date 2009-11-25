using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            ModelBaseDoc doc = ActiveMdiChild as ModelBaseDoc;
            if (doc == null)
                return;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = doc.GetImportFilter();

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                if (!doc.Import(new FileInfo(ofd.FileName)))
                    MessageBox.Show("Error importing file");
            }
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
