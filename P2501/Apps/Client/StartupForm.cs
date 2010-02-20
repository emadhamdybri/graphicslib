using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class StartupForm : Form
    {
        bool logedIn = false;

        public StartupForm()
        {
            InitializeComponent();
        }

        private void StartupForm_Load(object sender, EventArgs e)
        {
            NewsBrowser.Navigate("http://www.awesomelaser.com/p2501/news.html");

            UpdateUIStates();
        }

        protected void UpdateUIStates ()
        {
            CallsignsGroup.Enabled = logedIn;
            GamesGroup.Enabled = logedIn;

            Password.Enabled = Username.Text != string.Empty;

            LoginButton.Enabled = Username.Text != string.Empty && Password.Text != string.Empty && Password.Enabled;
        }

        private void Username_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUIStates();
        }

        private void Username_TextUpdate(object sender, EventArgs e)
        {
            UpdateUIStates();
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            UpdateUIStates();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            RegistrationForm form = new RegistrationForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                Username.Text = form.AccountName;
            }
            UpdateUIStates();
        }
    }
}
