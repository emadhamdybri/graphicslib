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
            this.StaticModelToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // StaticModelToolStrip
            // 
            this.StaticModelToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.StaticModelToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.StaticModelToolStrip.Location = new System.Drawing.Point(0, 0);
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
            // StaticModelDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(493, 337);
            this.Controls.Add(this.StaticModelToolStrip);
            this.Name = "StaticModelDoc";
            this.Text = "Static Model";
            this.Controls.SetChildIndex(this.StaticModelToolStrip, 0);
            this.StaticModelToolStrip.ResumeLayout(false);
            this.StaticModelToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip StaticModelToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}
