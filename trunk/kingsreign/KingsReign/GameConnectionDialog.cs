using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KingsReign
{
    public partial class GameConnectionDialog : Form
    {
        public bool TestGame = false;

        public GameConnectionDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestGame = true;
        }
    }
}
