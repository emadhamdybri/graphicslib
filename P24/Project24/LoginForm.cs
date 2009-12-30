using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Project24
{
    public partial class LoginForm : Form
    {
        public bool play = false;

        public LoginForm()
        {
            InitializeComponent();
        }

        public void Setup ()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            play = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
