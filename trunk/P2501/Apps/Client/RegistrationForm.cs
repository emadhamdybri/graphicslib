using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Web;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class RegistrationForm : Form
    {
        public string AccountName = string.Empty;

        public RegistrationForm()
        {
            InitializeComponent();
            TermsBrowser.Visible = false;
            Check.Enabled = false;
            OK.Enabled = false;
        }

        private void Terms_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (TermsBrowser.Visible)
                return;

            TermsBrowser.Visible = true;
            TermsBrowser.Navigate("http://www.awesomelaser.com/p2501/terms.html");
            this.Height += 250;
        }

        private void CheckOK ()
        {
            OK.Enabled = false;

            if (Email.Text == string.Empty || !Email.Text.Contains("@"))
                return;

            if (Password.Text == string.Empty || PassVerify.Text == string.Empty || Password.Text != PassVerify.Text)
                return;

            if (Callsign.Text == string.Empty)
                return;

            if (!Agree.Checked)
                return;

            OK.Enabled = true;
        }

        private void CheckPasswords()
        {
            PassError.Visible = Password.Text != PassVerify.Text;
        }
        private void Email_TextChanged(object sender, EventArgs e)
        {
            CheckOK();
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            CheckOK();
            CheckPasswords();
        }

        private void PassVerify_TextChanged(object sender, EventArgs e)
        {
            CheckOK();
            CheckPasswords();
        }

        private void Callsign_TextChanged(object sender, EventArgs e)
        {
            CheckOK();
            Check.Enabled = Callsign.Text != string.Empty;
        }

        private void Agree_CheckedChanged(object sender, EventArgs e)
        {
            CheckOK();
        }

        private void Check_Click(object sender, EventArgs e)
        {
          if (CheckName())
              MessageBox.Show("The name " + Callsign.Text + " is currently available");
        }

        private bool CheckName ()
        {
            bool avail = true;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.awesomelaser.com/p2501/Auth/callsigncheck.php?name=" + HttpUtility.UrlEncode(Callsign.Text));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            string ret = reader.ReadToEnd();
            reader.Close();
            resStream.Close();

            avail = ret == "OK";

            if (!avail)
            {
                MessageBox.Show("The name " + Callsign.Text + " is not available");
                Callsign.Text = string.Empty;
                Callsign.Select();
            }

            return avail;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!CheckName())
            {
                DialogResult = DialogResult.None;
                return;
            }

            AccountName = Email.Text;
        }
    }
}
