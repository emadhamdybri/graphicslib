namespace ModelEditor
{
    partial class StaticModelDoc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StaticModelDoc));
            this.StaticModelToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.StaticModelMenus = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.swapYZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.translateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StaticModelToolStrip.SuspendLayout();
            this.StaticModelMenus.SuspendLayout();
            this.SuspendLayout();
            // 
            // StaticModelToolStrip
            // 
            this.StaticModelToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.StaticModelToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.StaticModelToolStrip.Location = new System.Drawing.Point(0, 24);
            this.StaticModelToolStrip.Name = "StaticModelToolStrip";
            this.StaticModelToolStrip.Size = new System.Drawing.Size(493, 39);
            this.StaticModelToolStrip.TabIndex = 1;
            this.StaticModelToolStrip.Text = "Static Tools";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // StaticModelMenus
            // 
            this.StaticModelMenus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
            this.StaticModelMenus.Location = new System.Drawing.Point(0, 0);
            this.StaticModelMenus.Name = "StaticModelMenus";
            this.StaticModelMenus.Size = new System.Drawing.Size(493, 24);
            this.StaticModelMenus.TabIndex = 2;
            this.StaticModelMenus.Text = "StaticModelMenus";
            this.StaticModelMenus.Visible = false;
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.swapYZToolStripMenuItem,
            this.translateToolStripMenuItem,
            this.scaleToolStripMenuItem});
            this.toolsToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Replace;
            this.toolsToolStripMenuItem.MergeIndex = 0;
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // swapYZToolStripMenuItem
            // 
            this.swapYZToolStripMenuItem.Name = "swapYZToolStripMenuItem";
            this.swapYZToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.swapYZToolStripMenuItem.Text = "Swap YZ";
            this.swapYZToolStripMenuItem.Click += new System.EventHandler(this.swapYZToolStripMenuItem_Click);
            // 
            // translateToolStripMenuItem
            // 
            this.translateToolStripMenuItem.Name = "translateToolStripMenuItem";
            this.translateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.translateToolStripMenuItem.Text = "Translate...";
            this.translateToolStripMenuItem.Click += new System.EventHandler(this.translateToolStripMenuItem_Click);
            // 
            // scaleToolStripMenuItem
            // 
            this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
            this.scaleToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.scaleToolStripMenuItem.Text = "Scale...";
            this.scaleToolStripMenuItem.Click += new System.EventHandler(this.scaleToolStripMenuItem_Click);
            // 
            // StaticModelDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(493, 337);
            this.Controls.Add(this.StaticModelToolStrip);
            this.Controls.Add(this.StaticModelMenus);
            this.MainMenuStrip = this.StaticModelMenus;
            this.Name = "StaticModelDoc";
            this.Text = "Static Model";
            this.Controls.SetChildIndex(this.StaticModelMenus, 0);
            this.Controls.SetChildIndex(this.StaticModelToolStrip, 0);
            this.StaticModelToolStrip.ResumeLayout(false);
            this.StaticModelToolStrip.PerformLayout();
            this.StaticModelMenus.ResumeLayout(false);
            this.StaticModelMenus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip StaticModelToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.MenuStrip StaticModelMenus;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem swapYZToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem translateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scaleToolStripMenuItem;
    }
}
