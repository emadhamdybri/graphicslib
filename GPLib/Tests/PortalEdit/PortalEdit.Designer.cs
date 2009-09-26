namespace PortalEdit
{
    partial class EditFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditFrame));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.MapRadioPanel = new FormControls.ImageRadioPanel();
            this.SelectButton = new System.Windows.Forms.Button();
            this.DrawButton = new System.Windows.Forms.Button();
            this.MapView = new System.Windows.Forms.PictureBox();
            this.GLView = new OpenTK.GLControl();
            this.CellList = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.CancelEditPolygon = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.CellEdgeButton = new System.Windows.Forms.ToolStripSplitButton();
            this.ShowCellBorders = new System.Windows.Forms.ToolStripMenuItem();
            this.HideCellBorders = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.ShowPortals = new System.Windows.Forms.ToolStripMenuItem();
            this.HidePortals = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.MousePositionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.DeleteCell = new System.Windows.Forms.Button();
            this.Deslect = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.MapRadioPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(9, 66);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.MapRadioPanel);
            this.splitContainer1.Panel1.Controls.Add(this.MapView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.GLView);
            this.splitContainer1.Size = new System.Drawing.Size(676, 422);
            this.splitContainer1.SplitterDistance = 344;
            this.splitContainer1.TabIndex = 0;
            // 
            // MapRadioPanel
            // 
            this.MapRadioPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MapRadioPanel.Controls.Add(this.SelectButton);
            this.MapRadioPanel.Controls.Add(this.DrawButton);
            this.MapRadioPanel.HighlightColor = System.Drawing.Color.AliceBlue;
            this.MapRadioPanel.Location = new System.Drawing.Point(3, 0);
            this.MapRadioPanel.Name = "MapRadioPanel";
            this.MapRadioPanel.SelectedItem = null;
            this.MapRadioPanel.Size = new System.Drawing.Size(338, 26);
            this.MapRadioPanel.TabIndex = 3;
            this.MapRadioPanel.TagsAreValues = false;
            // 
            // SelectButton
            // 
            this.SelectButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectButton.Image")));
            this.SelectButton.Location = new System.Drawing.Point(31, 1);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(25, 23);
            this.SelectButton.TabIndex = 2;
            this.SelectButton.Tag = MapEditMode.SelectMode;
            this.SelectButton.UseVisualStyleBackColor = true;
            // 
            // DrawButton
            // 
            this.DrawButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrawButton.Image = global::PortalEdit.Properties.Resources.pencil;
            this.DrawButton.Location = new System.Drawing.Point(3, 1);
            this.DrawButton.Name = "DrawButton";
            this.DrawButton.Size = new System.Drawing.Size(25, 23);
            this.DrawButton.TabIndex = 1;
            this.DrawButton.Tag = MapEditMode.DrawMode;
            this.DrawButton.UseVisualStyleBackColor = true;
            // 
            // MapView
            // 
            this.MapView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MapView.Location = new System.Drawing.Point(3, 28);
            this.MapView.Name = "MapView";
            this.MapView.Size = new System.Drawing.Size(338, 391);
            this.MapView.TabIndex = 0;
            this.MapView.TabStop = false;
            // 
            // GLView
            // 
            this.GLView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GLView.BackColor = System.Drawing.Color.Black;
            this.GLView.Location = new System.Drawing.Point(3, 8);
            this.GLView.Name = "GLView";
            this.GLView.Size = new System.Drawing.Size(322, 411);
            this.GLView.TabIndex = 0;
            this.GLView.VSync = false;
            // 
            // CellList
            // 
            this.CellList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CellList.FormattingEnabled = true;
            this.CellList.Location = new System.Drawing.Point(691, 66);
            this.CellList.Name = "CellList";
            this.CellList.Size = new System.Drawing.Size(124, 147);
            this.CellList.TabIndex = 1;
            this.CellList.SelectedIndexChanged += new System.EventHandler(this.CellList_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(818, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CancelEditPolygon,
            this.toolStripSeparator1,
            this.CellEdgeButton,
            this.toolStripSplitButton1});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(818, 39);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // CancelEditPolygon
            // 
            this.CancelEditPolygon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CancelEditPolygon.Image = ((System.Drawing.Image)(resources.GetObject("CancelEditPolygon.Image")));
            this.CancelEditPolygon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CancelEditPolygon.Name = "CancelEditPolygon";
            this.CancelEditPolygon.Size = new System.Drawing.Size(36, 36);
            this.CancelEditPolygon.Text = "toolStripButton1";
            this.CancelEditPolygon.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // CellEdgeButton
            // 
            this.CellEdgeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CellEdgeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CellEdgeButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowCellBorders,
            this.HideCellBorders});
            this.CellEdgeButton.Image = ((System.Drawing.Image)(resources.GetObject("CellEdgeButton.Image")));
            this.CellEdgeButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.CellEdgeButton.Name = "CellEdgeButton";
            this.CellEdgeButton.Size = new System.Drawing.Size(48, 36);
            this.CellEdgeButton.Text = "Show/Hide Cell Borders";
            // 
            // ShowCellBorders
            // 
            this.ShowCellBorders.CheckOnClick = true;
            this.ShowCellBorders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ShowCellBorders.Name = "ShowCellBorders";
            this.ShowCellBorders.Size = new System.Drawing.Size(160, 22);
            this.ShowCellBorders.Text = "Show Cell Edges";
            this.ShowCellBorders.Click += new System.EventHandler(this.ShowCellBorders_Click);
            // 
            // HideCellBorders
            // 
            this.HideCellBorders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.HideCellBorders.Name = "HideCellBorders";
            this.HideCellBorders.Size = new System.Drawing.Size(160, 22);
            this.HideCellBorders.Text = "Hide Cell Edges";
            this.HideCellBorders.Click += new System.EventHandler(this.HideCellBorders_Click);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowPortals,
            this.HidePortals});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(48, 36);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // ShowPortals
            // 
            this.ShowPortals.Name = "ShowPortals";
            this.ShowPortals.Size = new System.Drawing.Size(142, 22);
            this.ShowPortals.Text = "Show Portals";
            this.ShowPortals.Click += new System.EventHandler(this.ShowPortals_Click);
            // 
            // HidePortals
            // 
            this.HidePortals.Name = "HidePortals";
            this.HidePortals.Size = new System.Drawing.Size(142, 22);
            this.HidePortals.Text = "Hide Portals";
            this.HidePortals.Click += new System.EventHandler(this.HidePortals_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MousePositionStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 491);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(818, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // MousePositionStatus
            // 
            this.MousePositionStatus.Name = "MousePositionStatus";
            this.MousePositionStatus.Size = new System.Drawing.Size(46, 17);
            this.MousePositionStatus.Text = "Mouse:";
            // 
            // DeleteCell
            // 
            this.DeleteCell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteCell.Location = new System.Drawing.Point(762, 219);
            this.DeleteCell.Name = "DeleteCell";
            this.DeleteCell.Size = new System.Drawing.Size(53, 23);
            this.DeleteCell.TabIndex = 5;
            this.DeleteCell.Text = "Delete";
            this.DeleteCell.UseVisualStyleBackColor = true;
            this.DeleteCell.Click += new System.EventHandler(this.DeleteCell_Click);
            // 
            // Deslect
            // 
            this.Deslect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Deslect.Location = new System.Drawing.Point(691, 219);
            this.Deslect.Name = "Deslect";
            this.Deslect.Size = new System.Drawing.Size(68, 23);
            this.Deslect.TabIndex = 6;
            this.Deslect.Text = "Deselect";
            this.Deslect.UseVisualStyleBackColor = true;
            this.Deslect.Click += new System.EventHandler(this.Deslect_Click);
            // 
            // EditFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 513);
            this.Controls.Add(this.Deslect);
            this.Controls.Add(this.DeleteCell);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.CellList);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EditFrame";
            this.Text = "Portal Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditFrame_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.MapRadioPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MapView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox MapView;
        private OpenTK.GLControl GLView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton CancelEditPolygon;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel MousePositionStatus;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        public System.Windows.Forms.ListBox CellList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSplitButton CellEdgeButton;
        private System.Windows.Forms.ToolStripMenuItem ShowCellBorders;
        private System.Windows.Forms.ToolStripMenuItem HideCellBorders;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem ShowPortals;
        private System.Windows.Forms.ToolStripMenuItem HidePortals;
        private System.Windows.Forms.Button DeleteCell;
        private System.Windows.Forms.Button Deslect;
        private System.Windows.Forms.Button DrawButton;
        private System.Windows.Forms.Button SelectButton;
        private FormControls.ImageRadioPanel MapRadioPanel;
    }
}

