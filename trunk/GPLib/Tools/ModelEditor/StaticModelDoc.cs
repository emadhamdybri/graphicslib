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
