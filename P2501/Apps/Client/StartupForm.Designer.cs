namespace P2501Client
{
    partial class StartupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LoginGroup = new System.Windows.Forms.GroupBox();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.LoginButton = new System.Windows.Forms.Button();
            this.Password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Username = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NewsBrowser = new System.Windows.Forms.WebBrowser();
            this.CallsignsGroup = new System.Windows.Forms.GroupBox();
            this.NewCharacter = new System.Windows.Forms.Button();
            this.CallsignList = new System.Windows.Forms.ListBox();
            this.GamesGroup = new System.Windows.Forms.GroupBox();
            this.ServerInfo = new System.Windows.Forms.TextBox();
            this.Options = new System.Windows.Forms.Button();
            this.Play = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ServerList = new System.Windows.Forms.TreeView();
            this.LoginGroup.SuspendLayout();
            this.CallsignsGroup.SuspendLayout();
            this.GamesGroup.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoginGroup
            // 
            this.LoginGroup.Controls.Add(this.RegisterButton);
            this.LoginGroup.Controls.Add(this.LoginButton);
            this.LoginGroup.Controls.Add(this.Password);
            this.LoginGroup.Controls.Add(this.label2);
            this.LoginGroup.Controls.Add(this.Username);
            this.LoginGroup.Controls.Add(this.label1);
            this.LoginGroup.Location = new System.Drawing.Point(12, 12);
            this.LoginGroup.Name = "LoginGroup";
            this.LoginGroup.Size = new System.Drawing.Size(200, 140);
            this.LoginGroup.TabIndex = 0;
            this.LoginGroup.TabStop = false;
            this.LoginGroup.Text = "Login";
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(91, 106);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(75, 23);
            this.RegisterButton.TabIndex = 5;
            this.RegisterButton.Text = "Register";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(10, 106);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 23);
            this.LoginButton.TabIndex = 4;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(10, 80);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(184, 20);
            this.Password.TabIndex = 3;
            this.Password.TextChanged += new System.EventHandler(this.Password_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // Username
            // 
            this.Username.FormattingEnabled = true;
            this.Username.Location = new System.Drawing.Point(10, 36);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(184, 21);
            this.Username.TabIndex = 1;
            this.Username.SelectedIndexChanged += new System.EventHandler(this.Username_SelectedIndexChanged);
            this.Username.TextUpdate += new System.EventHandler(this.Username_TextUpdate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // NewsBrowser
            // 
            this.NewsBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.NewsBrowser.Location = new System.Drawing.Point(3, 3);
            this.NewsBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.NewsBrowser.Name = "NewsBrowser";
            this.NewsBrowser.Size = new System.Drawing.Size(304, 176);
            this.NewsBrowser.TabIndex = 1;
            // 
            // CallsignsGroup
            // 
            this.CallsignsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.CallsignsGroup.Controls.Add(this.NewCharacter);
            this.CallsignsGroup.Controls.Add(this.CallsignList);
            this.CallsignsGroup.Location = new System.Drawing.Point(12, 167);
            this.CallsignsGroup.Name = "CallsignsGroup";
            this.CallsignsGroup.Size = new System.Drawing.Size(200, 209);
            this.CallsignsGroup.TabIndex = 2;
            this.CallsignsGroup.TabStop = false;
            this.CallsignsGroup.Text = "Callsigns";
            // 
            // NewCharacter
            // 
            this.NewCharacter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewCharacter.Location = new System.Drawing.Point(10, 171);
            this.NewCharacter.Name = "NewCharacter";
            this.NewCharacter.Size = new System.Drawing.Size(75, 23);
            this.NewCharacter.TabIndex = 1;
            this.NewCharacter.Text = "New...";
            this.NewCharacter.UseVisualStyleBackColor = true;
            // 
            // CallsignList
            // 
            this.CallsignList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.CallsignList.FormattingEnabled = true;
            this.CallsignList.Location = new System.Drawing.Point(10, 18);
            this.CallsignList.Name = "CallsignList";
            this.CallsignList.Size = new System.Drawing.Size(175, 147);
            this.CallsignList.TabIndex = 0;
            // 
            // GamesGroup
            // 
            this.GamesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GamesGroup.Controls.Add(this.ServerInfo);
            this.GamesGroup.Controls.Add(this.ServerList);
            this.GamesGroup.Controls.Add(this.Options);
            this.GamesGroup.Controls.Add(this.Play);
            this.GamesGroup.Location = new System.Drawing.Point(3, 3);
            this.GamesGroup.Name = "GamesGroup";
            this.GamesGroup.Size = new System.Drawing.Size(304, 175);
            this.GamesGroup.TabIndex = 3;
            this.GamesGroup.TabStop = false;
            this.GamesGroup.Text = "Servers";
            // 
            // ServerInfo
            // 
            this.ServerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerInfo.Location = new System.Drawing.Point(188, 19);
            this.ServerInfo.Multiline = true;
            this.ServerInfo.Name = "ServerInfo";
            this.ServerInfo.ReadOnly = true;
            this.ServerInfo.Size = new System.Drawing.Size(110, 114);
            this.ServerInfo.TabIndex = 3;
            // 
            // Options
            // 
            this.Options.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Options.Location = new System.Drawing.Point(6, 146);
            this.Options.Name = "Options";
            this.Options.Size = new System.Drawing.Size(75, 23);
            this.Options.TabIndex = 1;
            this.Options.Text = "Options...";
            this.Options.UseVisualStyleBackColor = true;
            // 
            // Play
            // 
            this.Play.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Play.Location = new System.Drawing.Point(223, 146);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(75, 23);
            this.Play.TabIndex = 0;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(218, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.NewsBrowser);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GamesGroup);
            this.splitContainer1.Size = new System.Drawing.Size(310, 364);
            this.splitContainer1.SplitterDistance = 182;
            this.splitContainer1.TabIndex = 4;
            // 
            // ServerList
            // 
            this.ServerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ServerList.Location = new System.Drawing.Point(6, 19);
            this.ServerList.Name = "ServerList";
            this.ServerList.Size = new System.Drawing.Size(176, 114);
            this.ServerList.TabIndex = 2;
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 388);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.CallsignsGroup);
            this.Controls.Add(this.LoginGroup);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "StartupForm";
            this.Text = "Projekt 2501";
            this.Load += new System.EventHandler(this.StartupForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartupForm_FormClosing);
            this.LoginGroup.ResumeLayout(false);
            this.LoginGroup.PerformLayout();
            this.CallsignsGroup.ResumeLayout(false);
            this.GamesGroup.ResumeLayout(false);
            this.GamesGroup.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox LoginGroup;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Username;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RegisterButton;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.WebBrowser NewsBrowser;
        private System.Windows.Forms.GroupBox CallsignsGroup;
        private System.Windows.Forms.Button NewCharacter;
        private System.Windows.Forms.ListBox CallsignList;
        private System.Windows.Forms.GroupBox GamesGroup;
        private System.Windows.Forms.Button Options;
        private System.Windows.Forms.Button Play;
        private System.Windows.Forms.TextBox ServerInfo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView ServerList;
    }
}

