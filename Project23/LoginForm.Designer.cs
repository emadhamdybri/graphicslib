namespace Project23
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.SelfServe = new System.Windows.Forms.Button();
            this.Username = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AvatarBox = new System.Windows.Forms.GroupBox();
            this.AvatarPixture = new System.Windows.Forms.PictureBox();
            this.AvatarList = new System.Windows.Forms.ComboBox();
            this.Female = new System.Windows.Forms.RadioButton();
            this.Male = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.Host = new System.Windows.Forms.TextBox();
            this.JoinHost = new System.Windows.Forms.Button();
            this.AvatarBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AvatarPixture)).BeginInit();
            this.SuspendLayout();
            // 
            // SelfServe
            // 
            this.SelfServe.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SelfServe.Location = new System.Drawing.Point(12, 89);
            this.SelfServe.Name = "SelfServe";
            this.SelfServe.Size = new System.Drawing.Size(75, 23);
            this.SelfServe.TabIndex = 0;
            this.SelfServe.Text = "SelfServe";
            this.SelfServe.UseVisualStyleBackColor = true;
            this.SelfServe.Click += new System.EventHandler(this.button1_Click);
            // 
            // Username
            // 
            this.Username.Location = new System.Drawing.Point(73, 9);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(199, 20);
            this.Username.TabIndex = 1;
            this.Username.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Username";
            // 
            // AvatarBox
            // 
            this.AvatarBox.Controls.Add(this.AvatarPixture);
            this.AvatarBox.Controls.Add(this.AvatarList);
            this.AvatarBox.Controls.Add(this.Female);
            this.AvatarBox.Controls.Add(this.Male);
            this.AvatarBox.Location = new System.Drawing.Point(278, 9);
            this.AvatarBox.Name = "AvatarBox";
            this.AvatarBox.Size = new System.Drawing.Size(142, 196);
            this.AvatarBox.TabIndex = 3;
            this.AvatarBox.TabStop = false;
            this.AvatarBox.Text = "Avatar";
            // 
            // AvatarPixture
            // 
            this.AvatarPixture.Location = new System.Drawing.Point(11, 18);
            this.AvatarPixture.Name = "AvatarPixture";
            this.AvatarPixture.Size = new System.Drawing.Size(121, 121);
            this.AvatarPixture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.AvatarPixture.TabIndex = 3;
            this.AvatarPixture.TabStop = false;
            // 
            // AvatarList
            // 
            this.AvatarList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AvatarList.FormattingEnabled = true;
            this.AvatarList.Location = new System.Drawing.Point(11, 145);
            this.AvatarList.Name = "AvatarList";
            this.AvatarList.Size = new System.Drawing.Size(121, 21);
            this.AvatarList.TabIndex = 2;
            this.AvatarList.SelectedIndexChanged += new System.EventHandler(this.AvatarList_SelectedIndexChanged);
            // 
            // Female
            // 
            this.Female.AutoSize = true;
            this.Female.Location = new System.Drawing.Point(73, 172);
            this.Female.Name = "Female";
            this.Female.Size = new System.Drawing.Size(59, 17);
            this.Female.TabIndex = 1;
            this.Female.TabStop = true;
            this.Female.Text = "Female";
            this.Female.UseVisualStyleBackColor = true;
            this.Female.CheckedChanged += new System.EventHandler(this.Female_CheckedChanged);
            // 
            // Male
            // 
            this.Male.AutoSize = true;
            this.Male.Location = new System.Drawing.Point(11, 172);
            this.Male.Name = "Male";
            this.Male.Size = new System.Drawing.Size(48, 17);
            this.Male.TabIndex = 0;
            this.Male.TabStop = true;
            this.Male.Text = "Male";
            this.Male.UseVisualStyleBackColor = true;
            this.Male.CheckedChanged += new System.EventHandler(this.Male_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Host";
            // 
            // Host
            // 
            this.Host.Location = new System.Drawing.Point(73, 41);
            this.Host.Name = "Host";
            this.Host.Size = new System.Drawing.Size(199, 20);
            this.Host.TabIndex = 5;
            // 
            // JoinHost
            // 
            this.JoinHost.Location = new System.Drawing.Point(93, 89);
            this.JoinHost.Name = "JoinHost";
            this.JoinHost.Size = new System.Drawing.Size(75, 23);
            this.JoinHost.TabIndex = 6;
            this.JoinHost.Text = "Join...";
            this.JoinHost.UseVisualStyleBackColor = true;
            this.JoinHost.Click += new System.EventHandler(this.JoinHost_Click);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.SelfServe;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 216);
            this.Controls.Add(this.JoinHost);
            this.Controls.Add(this.Host);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AvatarBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Username);
            this.Controls.Add(this.SelfServe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.Text = "Project23";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.AvatarBox.ResumeLayout(false);
            this.AvatarBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AvatarPixture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelfServe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox AvatarBox;
        private System.Windows.Forms.PictureBox AvatarPixture;
        private System.Windows.Forms.ComboBox AvatarList;
        private System.Windows.Forms.RadioButton Female;
        private System.Windows.Forms.RadioButton Male;
        protected System.Windows.Forms.TextBox Username;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Host;
        private System.Windows.Forms.Button JoinHost;
    }
}

