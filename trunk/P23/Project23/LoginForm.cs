using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Project23
{
    public partial class LoginForm : Form
    {
        public enum Gender
        {
           None,
           Male,
           Female,
        }

        public string UserName = string.Empty;
        public bool play = false;
        public bool selfServ = false;

        public int AvatarIndex = -1;
        public Gender AvatarGender = Gender.None;

        public LoginForm()
        {
            InitializeComponent();
            Username.Text = "Pilot";
        }

        protected void LoadSettings()
        {
            if (Settings.settings.fileLoc != null)
                return;

            DirectoryInfo AppSettingsDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Project23"));
            if (!AppSettingsDir.Exists)
                AppSettingsDir.Create();

            DirectoryInfo appDir = AppSettingsDir.CreateSubdirectory("Client");

            FileInfo prefsFile = new FileInfo(Path.Combine(appDir.FullName, "settings.xml"));
            if (prefsFile.Exists)
                Settings.settings = Settings.Read(prefsFile);
            else
            {
                Settings.settings.fileLoc = prefsFile;
                Settings.settings.Keys.SetDefaults();
            }
            Settings.settings.Write();
        }

        public void Setup ()
        {
            LoadSettings();
            SetupIndexList();

            AvatarIndex = Settings.settings.AvatarIndex;
            Username.Text = Settings.settings.UserName;
            AvatarGender = Settings.settings.gender;

            if (AvatarIndex > 0)
                AvatarList.SelectedIndex = AvatarIndex - 1;

            if (AvatarGender != Gender.None)
            {
                Male.Checked = AvatarGender == Gender.Male;
                Female.Checked = AvatarGender == Gender.Female;
            }

            SetAvatar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings.settings.AvatarIndex = AvatarIndex;
            Settings.settings.UserName = Username.Text;
            Settings.settings.gender = AvatarGender;
            Settings.settings.Write();

            UserName = Username.Text;
            selfServ = true;
            play = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void AvatarList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AvatarIndex = (int)AvatarList.SelectedItem;
            SetAvatar();
        }

        private void Male_CheckedChanged(object sender, EventArgs e)
        {
            AvatarGender = Gender.Male;
            SetAvatar();
        }

        private void Female_CheckedChanged(object sender, EventArgs e)
        {
            AvatarGender = Gender.Female;
            SetAvatar();
        }

        void SetupIndexList ()
        {
            int count = ResourceManager.FindFiles("pilots", "*f.png").Count;

            AvatarList.Items.Clear();

            for (int i = 1; i <= count; i++)
                AvatarList.Items.Add(i);
        }

        public string GetAvatarName ()
        {
            string AvatarName = "Pilot0u";

            if (AvatarGender != Gender.None && AvatarIndex > 0)
            {
                AvatarName = "Pilot" + AvatarIndex.ToString();
                if (AvatarGender == Gender.Female)
                    AvatarName += "f";
                else
                    AvatarName += "m";
            }

            return AvatarName;
        }

        protected void SetAvatar ( )
        {
            AvatarPixture.Image = new Bitmap(ResourceManager.FindFile("pilots/" + GetAvatarName() + ".png"));
        }
    }
}
