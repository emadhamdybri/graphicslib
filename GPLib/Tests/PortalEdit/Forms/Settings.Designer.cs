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
            this.PixelsPerUnit = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MapZoomPerClick = new System.Windows.Forms.NumericUpDown();
            this.Show3DOrigin = new System.Windows.Forms.CheckBox();
            this.GridSize = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.SnapValue = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.GridSubUnits = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.MinimalSelection = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.GeneralPage = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.UndoLevels = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.GridPage = new System.Windows.Forms.TabPage();
            this.MapViewPage = new System.Windows.Forms.TabPage();
            this.UnderlayHasDepth = new System.Windows.Forms.CheckBox();
            this.UnderlayAlpha = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PixelsPerUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapZoomPerClick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SnapValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridSubUnits)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.GeneralPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UndoLevels)).BeginInit();
            this.GridPage.SuspendLayout();
            this.MapViewPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnderlayAlpha)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(99, 191);
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
            this.Cancel.Location = new System.Drawing.Point(181, 191);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // PixelsPerUnit
            // 
            this.PixelsPerUnit.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.PixelsPerUnit.Location = new System.Drawing.Point(92, 40);
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
            this.label2.Location = new System.Drawing.Point(7, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Pixels Per Unit";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zoom Per Click";
            // 
            // MapZoomPerClick
            // 
            this.MapZoomPerClick.Location = new System.Drawing.Point(92, 14);
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
            // Show3DOrigin
            // 
            this.Show3DOrigin.AutoSize = true;
            this.Show3DOrigin.Location = new System.Drawing.Point(12, 84);
            this.Show3DOrigin.Name = "Show3DOrigin";
            this.Show3DOrigin.Size = new System.Drawing.Size(100, 17);
            this.Show3DOrigin.TabIndex = 10;
            this.Show3DOrigin.Text = "Show 3D Origin";
            this.Show3DOrigin.UseVisualStyleBackColor = true;
            // 
            // GridSize
            // 
            this.GridSize.Location = new System.Drawing.Point(94, 6);
            this.GridSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.GridSize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.GridSize.Name = "GridSize";
            this.GridSize.Size = new System.Drawing.Size(46, 20);
            this.GridSize.TabIndex = 9;
            this.GridSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Grid Size";
            // 
            // SnapValue
            // 
            this.SnapValue.DecimalPlaces = 2;
            this.SnapValue.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.SnapValue.Location = new System.Drawing.Point(94, 58);
            this.SnapValue.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SnapValue.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.SnapValue.Name = "SnapValue";
            this.SnapValue.Size = new System.Drawing.Size(46, 20);
            this.SnapValue.TabIndex = 7;
            this.SnapValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Snap";
            // 
            // GridSubUnits
            // 
            this.GridSubUnits.DecimalPlaces = 2;
            this.GridSubUnits.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.GridSubUnits.Location = new System.Drawing.Point(94, 32);
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
            this.label3.Location = new System.Drawing.Point(9, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Grid Sub Units";
            // 
            // MinimalSelection
            // 
            this.MinimalSelection.AutoSize = true;
            this.MinimalSelection.Location = new System.Drawing.Point(7, 6);
            this.MinimalSelection.Name = "MinimalSelection";
            this.MinimalSelection.Size = new System.Drawing.Size(138, 17);
            this.MinimalSelection.TabIndex = 4;
            this.MinimalSelection.Text = "Show Minimal Selection";
            this.MinimalSelection.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.GeneralPage);
            this.tabControl1.Controls.Add(this.GridPage);
            this.tabControl1.Controls.Add(this.MapViewPage);
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(265, 182);
            this.tabControl1.TabIndex = 5;
            // 
            // GeneralPage
            // 
            this.GeneralPage.Controls.Add(this.label7);
            this.GeneralPage.Controls.Add(this.UndoLevels);
            this.GeneralPage.Controls.Add(this.label6);
            this.GeneralPage.Controls.Add(this.MinimalSelection);
            this.GeneralPage.Location = new System.Drawing.Point(4, 22);
            this.GeneralPage.Name = "GeneralPage";
            this.GeneralPage.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralPage.Size = new System.Drawing.Size(257, 156);
            this.GeneralPage.TabIndex = 0;
            this.GeneralPage.Text = "General";
            this.GeneralPage.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(132, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "0 is unlimited";
            // 
            // UndoLevels
            // 
            this.UndoLevels.Location = new System.Drawing.Point(77, 29);
            this.UndoLevels.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.UndoLevels.Name = "UndoLevels";
            this.UndoLevels.Size = new System.Drawing.Size(49, 20);
            this.UndoLevels.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Undo Levels";
            // 
            // GridPage
            // 
            this.GridPage.Controls.Add(this.Show3DOrigin);
            this.GridPage.Controls.Add(this.label5);
            this.GridPage.Controls.Add(this.GridSize);
            this.GridPage.Controls.Add(this.label3);
            this.GridPage.Controls.Add(this.GridSubUnits);
            this.GridPage.Controls.Add(this.SnapValue);
            this.GridPage.Controls.Add(this.label4);
            this.GridPage.Location = new System.Drawing.Point(4, 22);
            this.GridPage.Name = "GridPage";
            this.GridPage.Padding = new System.Windows.Forms.Padding(3);
            this.GridPage.Size = new System.Drawing.Size(257, 156);
            this.GridPage.TabIndex = 1;
            this.GridPage.Text = "Grid";
            this.GridPage.UseVisualStyleBackColor = true;
            // 
            // MapViewPage
            // 
            this.MapViewPage.Controls.Add(this.UnderlayAlpha);
            this.MapViewPage.Controls.Add(this.label8);
            this.MapViewPage.Controls.Add(this.UnderlayHasDepth);
            this.MapViewPage.Controls.Add(this.PixelsPerUnit);
            this.MapViewPage.Controls.Add(this.label1);
            this.MapViewPage.Controls.Add(this.label2);
            this.MapViewPage.Controls.Add(this.MapZoomPerClick);
            this.MapViewPage.Location = new System.Drawing.Point(4, 22);
            this.MapViewPage.Name = "MapViewPage";
            this.MapViewPage.Size = new System.Drawing.Size(257, 156);
            this.MapViewPage.TabIndex = 2;
            this.MapViewPage.Text = "Map View";
            this.MapViewPage.UseVisualStyleBackColor = true;
            // 
            // UnderlayHasDepth
            // 
            this.UnderlayHasDepth.AutoSize = true;
            this.UnderlayHasDepth.Location = new System.Drawing.Point(10, 77);
            this.UnderlayHasDepth.Name = "UnderlayHasDepth";
            this.UnderlayHasDepth.Size = new System.Drawing.Size(120, 17);
            this.UnderlayHasDepth.TabIndex = 4;
            this.UnderlayHasDepth.Text = "Underlay with depth";
            this.UnderlayHasDepth.UseVisualStyleBackColor = true;
            // 
            // UnderlayAlpha
            // 
            this.UnderlayAlpha.DecimalPlaces = 3;
            this.UnderlayAlpha.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.UnderlayAlpha.Location = new System.Drawing.Point(92, 106);
            this.UnderlayAlpha.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UnderlayAlpha.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.UnderlayAlpha.Name = "UnderlayAlpha";
            this.UnderlayAlpha.Size = new System.Drawing.Size(46, 20);
            this.UnderlayAlpha.TabIndex = 6;
            this.UnderlayAlpha.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 108);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Underlay Alpha";
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(268, 226);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.PixelsPerUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapZoomPerClick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SnapValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridSubUnits)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.GeneralPage.ResumeLayout(false);
            this.GeneralPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UndoLevels)).EndInit();
            this.GridPage.ResumeLayout(false);
            this.GridPage.PerformLayout();
            this.MapViewPage.ResumeLayout(false);
            this.MapViewPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnderlayAlpha)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown MapZoomPerClick;
        private System.Windows.Forms.NumericUpDown PixelsPerUnit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown GridSubUnits;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown SnapValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown GridSize;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox MinimalSelection;
        private System.Windows.Forms.CheckBox Show3DOrigin;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage GeneralPage;
        private System.Windows.Forms.TabPage GridPage;
        private System.Windows.Forms.TabPage MapViewPage;
        private System.Windows.Forms.NumericUpDown UndoLevels;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox UnderlayHasDepth;
        private System.Windows.Forms.NumericUpDown UnderlayAlpha;
        private System.Windows.Forms.Label label8;
    }
}