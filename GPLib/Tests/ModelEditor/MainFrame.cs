/*
    Open Combat/Projekt 2501
    Copyright (C) 2010  Jeffery Allen Myers

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
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
