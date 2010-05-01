namespace KingsReign
{
    partial class GameWindow
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
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Deployed", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Reserve Forces", System.Windows.Forms.HorizontalAlignment.Left);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Spliter = new System.Windows.Forms.SplitContainer();
            this.WorldViewCtl = new OpenTK.GLControl();
            this.ChatSpliter = new System.Windows.Forms.SplitContainer();
            this.ChatType = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ChatTabs = new System.Windows.Forms.TabControl();
            this.GeneralChatTab = new System.Windows.Forms.TabPage();
            this.CombatLogTab = new System.Windows.Forms.TabPage();
            this.EventLogTab = new System.Windows.Forms.TabPage();
            this.GeneralChat = new System.Windows.Forms.RichTextBox();
            this.CombatLog = new System.Windows.Forms.RichTextBox();
            this.EventLog = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label4 = new System.Windows.Forms.Label();
            this.StadingOrders = new System.Windows.Forms.ComboBox();
            this.TurnIndicator = new System.Windows.Forms.PictureBox();
            this.OpenTreasury = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CurrentGold = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CurrentLoyalty = new System.Windows.Forms.TextBox();
            this.CapitalHealth = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.RepairCapital = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.Spliter.Panel1.SuspendLayout();
            this.Spliter.Panel2.SuspendLayout();
            this.Spliter.SuspendLayout();
            this.ChatSpliter.Panel1.SuspendLayout();
            this.ChatSpliter.Panel2.SuspendLayout();
            this.ChatSpliter.SuspendLayout();
            this.ChatTabs.SuspendLayout();
            this.GeneralChatTab.SuspendLayout();
            this.CombatLogTab.SuspendLayout();
            this.EventLogTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TurnIndicator)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1091, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findGameToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // findGameToolStripMenuItem
            // 
            this.findGameToolStripMenuItem.Name = "findGameToolStripMenuItem";
            this.findGameToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.findGameToolStripMenuItem.Text = "Find Game";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(137, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // Spliter
            // 
            this.Spliter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Spliter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.Spliter.IsSplitterFixed = true;
            this.Spliter.Location = new System.Drawing.Point(0, 24);
            this.Spliter.Name = "Spliter";
            // 
            // Spliter.Panel1
            // 
            this.Spliter.Panel1.Controls.Add(this.ChatSpliter);
            // 
            // Spliter.Panel2
            // 
            this.Spliter.Panel2.Controls.Add(this.groupBox3);
            this.Spliter.Panel2.Controls.Add(this.groupBox2);
            this.Spliter.Panel2.Controls.Add(this.groupBox1);
            this.Spliter.Size = new System.Drawing.Size(1091, 523);
            this.Spliter.SplitterDistance = 852;
            this.Spliter.TabIndex = 1;
            // 
            // WorldViewCtl
            // 
            this.WorldViewCtl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.WorldViewCtl.BackColor = System.Drawing.Color.Black;
            this.WorldViewCtl.Location = new System.Drawing.Point(3, 3);
            this.WorldViewCtl.Name = "WorldViewCtl";
            this.WorldViewCtl.Size = new System.Drawing.Size(840, 364);
            this.WorldViewCtl.TabIndex = 0;
            this.WorldViewCtl.VSync = false;
            // 
            // ChatSpliter
            // 
            this.ChatSpliter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatSpliter.Location = new System.Drawing.Point(3, 3);
            this.ChatSpliter.Name = "ChatSpliter";
            this.ChatSpliter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ChatSpliter.Panel1
            // 
            this.ChatSpliter.Panel1.Controls.Add(this.WorldViewCtl);
            // 
            // ChatSpliter.Panel2
            // 
            this.ChatSpliter.Panel2.Controls.Add(this.ChatTabs);
            this.ChatSpliter.Panel2.Controls.Add(this.textBox1);
            this.ChatSpliter.Panel2.Controls.Add(this.ChatType);
            this.ChatSpliter.Size = new System.Drawing.Size(846, 520);
            this.ChatSpliter.SplitterDistance = 370;
            this.ChatSpliter.TabIndex = 1;
            // 
            // ChatType
            // 
            this.ChatType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ChatType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ChatType.FormattingEnabled = true;
            this.ChatType.Items.AddRange(new object[] {
            "General",
            "Pivate",
            "Team"});
            this.ChatType.Location = new System.Drawing.Point(3, 122);
            this.ChatType.Name = "ChatType";
            this.ChatType.Size = new System.Drawing.Size(121, 21);
            this.ChatType.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(130, 123);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(713, 20);
            this.textBox1.TabIndex = 1;
            // 
            // ChatTabs
            // 
            this.ChatTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatTabs.Controls.Add(this.GeneralChatTab);
            this.ChatTabs.Controls.Add(this.CombatLogTab);
            this.ChatTabs.Controls.Add(this.EventLogTab);
            this.ChatTabs.Location = new System.Drawing.Point(3, 0);
            this.ChatTabs.Name = "ChatTabs";
            this.ChatTabs.SelectedIndex = 0;
            this.ChatTabs.Size = new System.Drawing.Size(840, 117);
            this.ChatTabs.TabIndex = 2;
            // 
            // GeneralChatTab
            // 
            this.GeneralChatTab.Controls.Add(this.GeneralChat);
            this.GeneralChatTab.Location = new System.Drawing.Point(4, 22);
            this.GeneralChatTab.Name = "GeneralChatTab";
            this.GeneralChatTab.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralChatTab.Size = new System.Drawing.Size(832, 91);
            this.GeneralChatTab.TabIndex = 0;
            this.GeneralChatTab.Text = "General";
            this.GeneralChatTab.UseVisualStyleBackColor = true;
            // 
            // CombatLogTab
            // 
            this.CombatLogTab.Controls.Add(this.CombatLog);
            this.CombatLogTab.Location = new System.Drawing.Point(4, 22);
            this.CombatLogTab.Name = "CombatLogTab";
            this.CombatLogTab.Padding = new System.Windows.Forms.Padding(3);
            this.CombatLogTab.Size = new System.Drawing.Size(832, 91);
            this.CombatLogTab.TabIndex = 1;
            this.CombatLogTab.Text = "Combat";
            this.CombatLogTab.UseVisualStyleBackColor = true;
            // 
            // EventLogTab
            // 
            this.EventLogTab.Controls.Add(this.EventLog);
            this.EventLogTab.Location = new System.Drawing.Point(4, 22);
            this.EventLogTab.Name = "EventLogTab";
            this.EventLogTab.Size = new System.Drawing.Size(832, 91);
            this.EventLogTab.TabIndex = 2;
            this.EventLogTab.Text = "Events";
            this.EventLogTab.UseVisualStyleBackColor = true;
            // 
            // GeneralChat
            // 
            this.GeneralChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GeneralChat.BackColor = System.Drawing.Color.Black;
            this.GeneralChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GeneralChat.ForeColor = System.Drawing.Color.White;
            this.GeneralChat.Location = new System.Drawing.Point(0, 0);
            this.GeneralChat.Name = "GeneralChat";
            this.GeneralChat.ReadOnly = true;
            this.GeneralChat.Size = new System.Drawing.Size(832, 91);
            this.GeneralChat.TabIndex = 0;
            this.GeneralChat.Text = "";
            // 
            // CombatLog
            // 
            this.CombatLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CombatLog.Location = new System.Drawing.Point(3, 3);
            this.CombatLog.Name = "CombatLog";
            this.CombatLog.Size = new System.Drawing.Size(826, 88);
            this.CombatLog.TabIndex = 0;
            this.CombatLog.Text = "";
            // 
            // EventLog
            // 
            this.EventLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.EventLog.Location = new System.Drawing.Point(3, 3);
            this.EventLog.Name = "EventLog";
            this.EventLog.Size = new System.Drawing.Size(826, 85);
            this.EventLog.TabIndex = 0;
            this.EventLog.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.TurnIndicator);
            this.groupBox1.Location = new System.Drawing.Point(4, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 86);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Players";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.StadingOrders);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.listView1);
            this.groupBox2.Location = new System.Drawing.Point(4, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 259);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Forces";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            listViewGroup5.Header = "Deployed";
            listViewGroup5.Name = "DeployedForces";
            listViewGroup6.Header = "Reserve Forces";
            listViewGroup6.Name = "ReserveForces";
            this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup5,
            listViewGroup6});
            this.listView1.Location = new System.Drawing.Point(6, 19);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(216, 207);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 235);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Standing Orders";
            // 
            // StadingOrders
            // 
            this.StadingOrders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StadingOrders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StadingOrders.FormattingEnabled = true;
            this.StadingOrders.Items.AddRange(new object[] {
            "None",
            "Attack On Sight",
            "Counter Attack Only"});
            this.StadingOrders.Location = new System.Drawing.Point(92, 232);
            this.StadingOrders.Name = "StadingOrders";
            this.StadingOrders.Size = new System.Drawing.Size(124, 21);
            this.StadingOrders.TabIndex = 2;
            // 
            // TurnIndicator
            // 
            this.TurnIndicator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TurnIndicator.Location = new System.Drawing.Point(6, 19);
            this.TurnIndicator.Name = "TurnIndicator";
            this.TurnIndicator.Size = new System.Drawing.Size(210, 44);
            this.TurnIndicator.TabIndex = 0;
            this.TurnIndicator.TabStop = false;
            // 
            // OpenTreasury
            // 
            this.OpenTreasury.BackgroundImage = global::KingsReign.Properties.Resources.money;
            this.OpenTreasury.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.OpenTreasury.Location = new System.Drawing.Point(170, 14);
            this.OpenTreasury.Name = "OpenTreasury";
            this.OpenTreasury.Size = new System.Drawing.Size(52, 72);
            this.OpenTreasury.TabIndex = 0;
            this.OpenTreasury.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Gold";
            // 
            // CurrentGold
            // 
            this.CurrentGold.Location = new System.Drawing.Point(52, 16);
            this.CurrentGold.Name = "CurrentGold";
            this.CurrentGold.ReadOnly = true;
            this.CurrentGold.Size = new System.Drawing.Size(112, 20);
            this.CurrentGold.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Loyalty";
            // 
            // CurrentLoyalty
            // 
            this.CurrentLoyalty.Location = new System.Drawing.Point(52, 51);
            this.CurrentLoyalty.Name = "CurrentLoyalty";
            this.CurrentLoyalty.ReadOnly = true;
            this.CurrentLoyalty.Size = new System.Drawing.Size(112, 20);
            this.CurrentLoyalty.TabIndex = 4;
            // 
            // CapitalHealth
            // 
            this.CapitalHealth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CapitalHealth.Location = new System.Drawing.Point(51, 95);
            this.CapitalHealth.Name = "CapitalHealth";
            this.CapitalHealth.Size = new System.Drawing.Size(134, 23);
            this.CapitalHealth.Step = 1;
            this.CapitalHealth.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.CapitalHealth.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Capital";
            // 
            // RepairCapital
            // 
            this.RepairCapital.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RepairCapital.BackgroundImage = global::KingsReign.Properties.Resources.fix;
            this.RepairCapital.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.RepairCapital.Location = new System.Drawing.Point(191, 89);
            this.RepairCapital.Name = "RepairCapital";
            this.RepairCapital.Size = new System.Drawing.Size(31, 33);
            this.RepairCapital.TabIndex = 7;
            this.RepairCapital.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.RepairCapital);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.CapitalHealth);
            this.groupBox3.Controls.Add(this.CurrentLoyalty);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.CurrentGold);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.OpenTreasury);
            this.groupBox3.Location = new System.Drawing.Point(4, 363);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(228, 157);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Realm";
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 547);
            this.Controls.Add(this.Spliter);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GameWindow";
            this.Text = "Kings Reign";
            this.Load += new System.EventHandler(this.GameWindow_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.Spliter.Panel1.ResumeLayout(false);
            this.Spliter.Panel2.ResumeLayout(false);
            this.Spliter.ResumeLayout(false);
            this.ChatSpliter.Panel1.ResumeLayout(false);
            this.ChatSpliter.Panel2.ResumeLayout(false);
            this.ChatSpliter.Panel2.PerformLayout();
            this.ChatSpliter.ResumeLayout(false);
            this.ChatTabs.ResumeLayout(false);
            this.GeneralChatTab.ResumeLayout(false);
            this.CombatLogTab.ResumeLayout(false);
            this.EventLogTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TurnIndicator)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.SplitContainer Spliter;
        private OpenTK.GLControl WorldViewCtl;
        private System.Windows.Forms.ToolStripMenuItem findGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer ChatSpliter;
        private System.Windows.Forms.TabControl ChatTabs;
        private System.Windows.Forms.TabPage GeneralChatTab;
        private System.Windows.Forms.RichTextBox GeneralChat;
        private System.Windows.Forms.TabPage CombatLogTab;
        private System.Windows.Forms.RichTextBox CombatLog;
        private System.Windows.Forms.TabPage EventLogTab;
        private System.Windows.Forms.RichTextBox EventLog;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox ChatType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox TurnIndicator;
        private System.Windows.Forms.ComboBox StadingOrders;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button RepairCapital;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar CapitalHealth;
        private System.Windows.Forms.TextBox CurrentLoyalty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CurrentGold;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OpenTreasury;
    }
}

