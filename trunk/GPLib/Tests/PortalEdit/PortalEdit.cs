using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            editor.mapRenderer.ClearEditPolygon();
            Invalidate(true);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open File";
            ofd.Filter = "Portal Map (*.PortalMap)|*.PortalMap|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!editor.Open(new FileInfo(ofd.FileName)))
                    MessageBox.Show("Error reading map file");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save File";
            sfd.Filter = "Portal Map (*.PortalMap)|*.PortalMap";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (!editor.Save(new FileInfo(sfd.FileName)))
                    MessageBox.Show("Error saving map file");
            }

        }
    }
}
