using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using OpenTK;
using OpenTK.Graphics;

using Drawables.Models;
using Drawables.Models.OBJ;

namespace ModelEditor
{
    public partial class StaticModelDoc : ModelEditor.ModelBaseDoc
    {
        protected Model model = null;

        public StaticModelDoc()
        {
            InitializeComponent();
        }

        public override string GetImportFilter()
        {
            return "Wavefront OBJ File (*.obj)|*.obj|All Files (*.*)|*.*"; ;
        }

        public override string GetOpenFilter()
        {
            return "Static Model File (*.XMDL)|*.XMDL|All Files (*.*)|*.*"; ;
        }

        public override string GetSaveAsFilter()
        {
            return "Static Model File (*.XMDL)|*.XMDL|All Files (*.*)|*.*"; ;
        }

        public override bool Import(FileInfo file)
        {
            model = OBJFile.Read(file);
            Redraw();
            return model != null;
        }

        protected override void DrawView()
        {
            GL.Enable(EnableCap.Texture2D);
            if (model != null)
                model.drawAll();
            base.DrawView();
        }

        protected override void SetupView()
        {
            GL.ClearColor(Color.LightSlateGray);
            GridSubDivColor = Color.DarkGray;
        }

        private void swapYZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (model == null)
                return;

            foreach (Mesh mesh in model.meshes)
                mesh.SwapYZ();

            model.Invalidate();

            Redraw();
        }

        private void translateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void scaleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
