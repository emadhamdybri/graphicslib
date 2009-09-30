namespace PortalEdit
{
    partial class SettingsDialog
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
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PixelsPerUnit = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MapZoomPerClick = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SnapPixels = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.GridSubUnits = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PixelsPerUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapZoomPerClick)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SnapPixels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridSubUnits)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(184, 162);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 0;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(266, 162);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PixelsPerUnit);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.MapZoomPerClick);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(147, 90);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map View";
            // 
            // PixelsPerUnit
            // 
            this.PixelsPerUnit.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.PixelsPerUnit.Location = new System.Drawing.Point(91, 56);
            this.PixelsPerUnit.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.PixelsPerUnit.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.PixelsPerUnit.Name = "PixelsPerUnit";
            this.PixelsPerUnit.Size = new System.Drawing.Size(46, 20);
            this.PixelsPerUnit.TabIndex = 3;
            this.PixelsPerUnit.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pixels Per Unit";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zoom Per Click";
            // 
            // MapZoomPerClick
            // 
            this.MapZoomPerClick.Location = new System.Drawing.Point(91, 24);
            this.MapZoomPerClick.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MapZoomPerClick.Name = "MapZoomPerClick";
            this.MapZoomPerClick.Size = new System.Drawing.Size(46, 20);
            this.MapZoomPerClick.TabIndex = 0;
            this.MapZoomPerClick.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.SnapPixels);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.GridSubUnits);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(147, 77);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Grid";
            // 
            // SnapPixels
            // 
            this.SnapPixels.Location = new System.Drawing.Point(91, 43);
            this.SnapPixels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SnapPixels.Name = "SnapPixels";
            this.SnapPixels.Size = new System.Drawing.Size(46, 20);
            this.SnapPixels.TabIndex = 7;
            this.SnapPixels.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Snap Pixels";
            // 
            // GridSubUnits
            // 
            this.GridSubUnits.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.GridSubUnits.Location = new System.Drawing.Point(91, 14);
            this.GridSubUnits.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.GridSubUnits.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.GridSubUnits.Name = "GridSubUnits";
            this.GridSubUnits.Size = new System.Drawing.Size(46, 20);
            this.GridSubUnits.TabIndex = 5;
            this.GridSubUnits.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Grid Sub Units";
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(353, 197);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PixelsPerUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapZoomPerClick)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SnapPixels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridSubUnits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown MapZoomPerClick;
        private System.Windows.Forms.NumericUpDown PixelsPerUnit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown GridSubUnits;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown SnapPixels;
        private System.Windows.Forms.Label label4;
    }
}