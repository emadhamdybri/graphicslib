using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics;

namespace ModelEditor
{
    public partial class StaticModelDoc : ModelEditor.ModelBaseDoc
    {
        public StaticModelDoc()
        {
            InitializeComponent();
        }

        protected override void SetupView()
        {
            GL.ClearColor(Color.LightSlateGray);
            GridSubDivColor = Color.DarkGray;
        }
    }
}
