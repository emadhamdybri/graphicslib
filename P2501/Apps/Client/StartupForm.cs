using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace P2501Client
{
    public partial class StartupForm : Form
    {
        UInt64 UID = 0;
        UInt64 Token = 0;

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
            CallsignsGroup.Enabled = Token != 0;
            GamesGroup.Enabled = Token != 0;

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

        protected class CharaterListItem
        {
            public string Name = string.Empty;
            public UInt64 UID = 0;

            public CharaterListItem ( string n, UInt64 id )
            {
                Name = n;
                UID = id;
            }

            public override string ToString()
            {
                return Name;
            }
        }
        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (UID != 0)
            {
                CallsignList.Items.Clear();
                UID = 0;
                Token = 0;
                LoginButton.Text = "Login";
                return;
            }

            Login login = new Login();
            UID = 0;
            Token = 0;
            if (login.Connect(Username.Text,Password.Text))
            {
                UID = login.UID;
                Token = login.Token;

                UpdateUIStates();
                LoginButton.Text = "Logout";
                
                // get characters
                Dictionary<UInt64, string> list = login.GetCharacterList();
                if (list != null)
                {
                    CallsignList.Items.Clear();

                    foreach (KeyValuePair<UInt64, string> character in list)
                        CallsignList.Items.Add(new CharaterListItem(character.Value, character.Key));
                }

                // get list
            }
            else
                MessageBox.Show("Login failure");
        }
    }
}
