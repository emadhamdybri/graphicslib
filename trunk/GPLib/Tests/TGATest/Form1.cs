using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Drawables.TGA;

namespace TGATest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.pictureBox1.Image = TGAFile.Read("test32.tga");
          //  this.pictureBox1.Image = TGAFile.Read("test8.tga");
        }
    }
}
