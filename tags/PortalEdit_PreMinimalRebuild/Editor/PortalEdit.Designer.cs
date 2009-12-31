﻿namespace PortalEdit
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditFrame));
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.HideGeo = new System.Windows.Forms.CheckBox();
            this.HideAboveZ = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HideBelowZ = new System.Windows.Forms.NumericUpDown();
            this.DepthEditPanel = new System.Windows.Forms.Panel();
            this.NamedDepthPresets = new System.Windows.Forms.ComboBox();
            this.EditLoadZFromSelection = new System.Windows.Forms.Button();
            this.EditIncZ = new System.Windows.Forms.CheckBox();
            this.EditZPlus = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.EditZMinus = new System.Windows.Forms.TextBox();
            this.ZMinusLabel = new System.Windows.Forms.Label();
            this.MapEditToolsPanel = new System.Windows.Forms.Panel();
            this.MapRadioPanel = new FormControls.ImageRadioPanel();
            this.SelectButton = new System.Windows.Forms.Button();
            this.DrawButton = new System.Windows.Forms.Button();
            this.MapZoomPanel = new System.Windows.Forms.Panel();
            this.ResetZoom = new System.Windows.Forms.Button();
            this.MapZoomOut = new System.Windows.Forms.Button();
            this.MapZoomIn = new System.Windows.Forms.Button();
            this.MapView = new System.Windows.Forms.PictureBox();
            this.ViewCheckPanel = new FormControls.ImageCheckPanel();
            this.ShowUnderlay = new System.Windows.Forms.Button();
            this.ShowPortals = new System.Windows.Forms.Button();
            this.ShowCellEdges = new System.Windows.Forms.Button();
            this.ViewRadioPanel = new FormControls.ImageRadioPanel();
            this.CellFillMode = new System.Windows.Forms.Button();
            this.CellSelectButton = new System.Windows.Forms.Button();
            this.GLView = new OpenTK.GLControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.setupImageUnderlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.renameDepthGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDepthGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.newGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.renameCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.MousePositionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.MapTreeIcons = new System.Windows.Forms.ImageList(this.components);
            this.GroupRightMouseMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RenameGroupRMM = new System.Windows.Forms.ToolStripMenuItem();
            this.CellRightMouseMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RenameCellRMM = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteCellRMM = new System.Windows.Forms.ToolStripMenuItem();
            this.SidebarSplitter = new System.Windows.Forms.SplitContainer();
            this.MapTree = new System.Windows.Forms.TreeView();
            this.MapTreeRightMouseMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deselectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CellTabControl = new System.Windows.Forms.TabControl();
            this.CellInfo = new System.Windows.Forms.TabPage();
            this.CellGroupDropdown = new System.Windows.Forms.ComboBox();
            this.CellGroupDropdownLabel = new System.Windows.Forms.Label();
            this.VertInfo = new System.Windows.Forms.TabPage();
            this.CellInfoZIsInc = new System.Windows.Forms.CheckBox();
            this.CellVertList = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanarFloor = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ZMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlanarRoof = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.EdgeInfo = new System.Windows.Forms.TabPage();
            this.CellEdgeList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FaceInfo = new System.Windows.Forms.TabPage();
            this.VertListRightMouseMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setToPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HideAboveZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HideBelowZ)).BeginInit();
            this.DepthEditPanel.SuspendLayout();
            this.MapEditToolsPanel.SuspendLayout();
            this.MapRadioPanel.SuspendLayout();
            this.MapZoomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapView)).BeginInit();
            this.ViewCheckPanel.SuspendLayout();
            this.ViewRadioPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.GroupRightMouseMenu.SuspendLayout();
            this.CellRightMouseMenu.SuspendLayout();
            this.SidebarSplitter.Panel1.SuspendLayout();
            this.SidebarSplitter.Panel2.SuspendLayout();
            this.SidebarSplitter.SuspendLayout();
            this.MapTreeRightMouseMenu.SuspendLayout();
            this.CellTabControl.SuspendLayout();
            this.CellInfo.SuspendLayout();
            this.VertInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CellVertList)).BeginInit();
            this.EdgeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CellEdgeList)).BeginInit();
            this.VertListRightMouseMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainContainer
            // 
            this.MainContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MainContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainContainer.Location = new System.Drawing.Point(0, 27);
            this.MainContainer.Name = "MainContainer";
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.Controls.Add(this.panel1);
            this.MainContainer.Panel1.Controls.Add(this.DepthEditPanel);
            this.MainContainer.Panel1.Controls.Add(this.MapEditToolsPanel);
            this.MainContainer.Panel1.Controls.Add(this.MapView);
            this.MainContainer.Panel1MinSize = 250;
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.Controls.Add(this.ViewCheckPanel);
            this.MainContainer.Panel2.Controls.Add(this.ViewRadioPanel);
            this.MainContainer.Panel2.Controls.Add(this.GLView);
            this.MainContainer.Panel2MinSize = 120;
            this.MainContainer.Size = new System.Drawing.Size(889, 513);
            this.MainContainer.SplitterDistance = 449;
            this.MainContainer.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.HideGeo);
            this.panel1.Controls.Add(this.HideAboveZ);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.HideBelowZ);
            this.panel1.Location = new System.Drawing.Point(6, 74);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 29);
            this.panel1.TabIndex = 11;
            // 
            // HideGeo
            // 
            this.HideGeo.AutoSize = true;
            this.HideGeo.Location = new System.Drawing.Point(9, 6);
            this.HideGeo.Name = "HideGeo";
            this.HideGeo.Size = new System.Drawing.Size(48, 17);
            this.HideGeo.TabIndex = 10;
            this.HideGeo.Text = "Hide";
            this.HideGeo.UseVisualStyleBackColor = true;
            this.HideGeo.CheckedChanged += new System.EventHandler(this.HideGeo_CheckedChanged);
            // 
            // HideAboveZ
            // 
            this.HideAboveZ.DecimalPlaces = 2;
            this.HideAboveZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.HideAboveZ.Location = new System.Drawing.Point(197, 5);
            this.HideAboveZ.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.HideAboveZ.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.HideAboveZ.Name = "HideAboveZ";
            this.HideAboveZ.Size = new System.Drawing.Size(54, 20);
            this.HideAboveZ.TabIndex = 8;
            this.HideAboveZ.ValueChanged += new System.EventHandler(this.HideBelowZ_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(159, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Above";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Below";
            // 
            // HideBelowZ
            // 
            this.HideBelowZ.DecimalPlaces = 2;
            this.HideBelowZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.HideBelowZ.Location = new System.Drawing.Point(96, 5);
            this.HideBelowZ.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.HideBelowZ.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.HideBelowZ.Name = "HideBelowZ";
            this.HideBelowZ.Size = new System.Drawing.Size(53, 20);
            this.HideBelowZ.TabIndex = 6;
            this.HideBelowZ.ValueChanged += new System.EventHandler(this.HideBelowZ_ValueChanged);
            // 
            // DepthEditPanel
            // 
            this.DepthEditPanel.Controls.Add(this.NamedDepthPresets);
            this.DepthEditPanel.Controls.Add(this.EditLoadZFromSelection);
            this.DepthEditPanel.Controls.Add(this.EditIncZ);
            this.DepthEditPanel.Controls.Add(this.EditZPlus);
            this.DepthEditPanel.Controls.Add(this.label1);
            this.DepthEditPanel.Controls.Add(this.EditZMinus);
            this.DepthEditPanel.Controls.Add(this.ZMinusLabel);
            this.DepthEditPanel.Location = new System.Drawing.Point(6, 38);
            this.DepthEditPanel.Name = "DepthEditPanel";
            this.DepthEditPanel.Size = new System.Drawing.Size(290, 33);
            this.DepthEditPanel.TabIndex = 5;
            // 
            // NamedDepthPresets
            // 
            this.NamedDepthPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NamedDepthPresets.FormattingEnabled = true;
            this.NamedDepthPresets.Location = new System.Drawing.Point(200, 5);
            this.NamedDepthPresets.MaxDropDownItems = 25;
            this.NamedDepthPresets.Name = "NamedDepthPresets";
            this.NamedDepthPresets.Size = new System.Drawing.Size(78, 21);
            this.NamedDepthPresets.TabIndex = 6;
            this.NamedDepthPresets.SelectedIndexChanged += new System.EventHandler(this.NamedDepthPresets_SelectedIndexChanged);
            // 
            // EditLoadZFromSelection
            // 
            this.EditLoadZFromSelection.Image = ((System.Drawing.Image)(resources.GetObject("EditLoadZFromSelection.Image")));
            this.EditLoadZFromSelection.Location = new System.Drawing.Point(165, 2);
            this.EditLoadZFromSelection.Name = "EditLoadZFromSelection";
            this.EditLoadZFromSelection.Size = new System.Drawing.Size(29, 28);
            this.EditLoadZFromSelection.TabIndex = 6;
            this.EditLoadZFromSelection.UseVisualStyleBackColor = true;
            this.EditLoadZFromSelection.Click += new System.EventHandler(this.EditLoadZFromSelection_Click);
            // 
            // EditIncZ
            // 
            this.EditIncZ.AutoSize = true;
            this.EditIncZ.Location = new System.Drawing.Point(118, 7);
            this.EditIncZ.Name = "EditIncZ";
            this.EditIncZ.Size = new System.Drawing.Size(41, 17);
            this.EditIncZ.TabIndex = 4;
            this.EditIncZ.Text = "Inc";
            this.EditIncZ.UseVisualStyleBackColor = true;
            this.EditIncZ.CheckedChanged += new System.EventHandler(this.EditIncZ_CheckedChanged);
            // 
            // EditZPlus
            // 
            this.EditZPlus.Location = new System.Drawing.Point(83, 5);
            this.EditZPlus.Name = "EditZPlus";
            this.EditZPlus.Size = new System.Drawing.Size(31, 20);
            this.EditZPlus.TabIndex = 3;
            this.EditZPlus.TextChanged += new System.EventHandler(this.EditZPlus_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Z+";
            // 
            // EditZMinus
            // 
            this.EditZMinus.Location = new System.Drawing.Point(23, 5);
            this.EditZMinus.Name = "EditZMinus";
            this.EditZMinus.Size = new System.Drawing.Size(31, 20);
            this.EditZMinus.TabIndex = 1;
            this.EditZMinus.TextChanged += new System.EventHandler(this.EditZMinus_TextChanged);
            // 
            // ZMinusLabel
            // 
            this.ZMinusLabel.AutoSize = true;
            this.ZMinusLabel.Location = new System.Drawing.Point(5, 8);
            this.ZMinusLabel.Name = "ZMinusLabel";
            this.ZMinusLabel.Size = new System.Drawing.Size(17, 13);
            this.ZMinusLabel.TabIndex = 0;
            this.ZMinusLabel.Text = "Z-";
            // 
            // MapEditToolsPanel
            // 
            this.MapEditToolsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MapEditToolsPanel.Controls.Add(this.MapRadioPanel);
            this.MapEditToolsPanel.Controls.Add(this.MapZoomPanel);
            this.MapEditToolsPanel.Location = new System.Drawing.Point(3, 3);
            this.MapEditToolsPanel.Name = "MapEditToolsPanel";
            this.MapEditToolsPanel.Size = new System.Drawing.Size(441, 29);
            this.MapEditToolsPanel.TabIndex = 5;
            // 
            // MapRadioPanel
            // 
            this.MapRadioPanel.Controls.Add(this.SelectButton);
            this.MapRadioPanel.Controls.Add(this.DrawButton);
            this.MapRadioPanel.HighlightBGColor = System.Drawing.Color.DarkGoldenrod;
            this.MapRadioPanel.HighlightColor = System.Drawing.Color.AliceBlue;
            this.MapRadioPanel.Location = new System.Drawing.Point(3, 0);
            this.MapRadioPanel.Name = "MapRadioPanel";
            this.MapRadioPanel.SelectedItem = null;
            this.MapRadioPanel.Size = new System.Drawing.Size(114, 26);
            this.MapRadioPanel.TabIndex = 3;
            this.MapRadioPanel.TagsAreValues = false;
            // 
            // SelectButton
            // 
            this.SelectButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SelectButton.Image = ((System.Drawing.Image)(resources.GetObject("SelectButton.Image")));
            this.SelectButton.Location = new System.Drawing.Point(29, 1);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(25, 23);
            this.SelectButton.TabIndex = 2;
            this.SelectButton.Tag = PortalEdit.MapEditMode.SelectMode;
            this.SelectButton.UseVisualStyleBackColor = true;
            // 
            // DrawButton
            // 
            this.DrawButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DrawButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DrawButton.Image = ((System.Drawing.Image)(resources.GetObject("DrawButton.Image")));
            this.DrawButton.Location = new System.Drawing.Point(3, 1);
            this.DrawButton.Name = "DrawButton";
            this.DrawButton.Size = new System.Drawing.Size(25, 23);
            this.DrawButton.TabIndex = 1;
            this.DrawButton.Tag = PortalEdit.MapEditMode.DrawMode;
            this.DrawButton.UseVisualStyleBackColor = true;
            // 
            // MapZoomPanel
            // 
            this.MapZoomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MapZoomPanel.Controls.Add(this.ResetZoom);
            this.MapZoomPanel.Controls.Add(this.MapZoomOut);
            this.MapZoomPanel.Controls.Add(this.MapZoomIn);
            this.MapZoomPanel.Location = new System.Drawing.Point(341, 0);
            this.MapZoomPanel.Name = "MapZoomPanel";
            this.MapZoomPanel.Size = new System.Drawing.Size(100, 25);
            this.MapZoomPanel.TabIndex = 4;
            // 
            // ResetZoom
            // 
            this.ResetZoom.Location = new System.Drawing.Point(60, 0);
            this.ResetZoom.Name = "ResetZoom";
            this.ResetZoom.Size = new System.Drawing.Size(37, 23);
            this.ResetZoom.TabIndex = 2;
            this.ResetZoom.Text = "1:1";
            this.ResetZoom.UseVisualStyleBackColor = true;
            this.ResetZoom.Click += new System.EventHandler(this.ResetZoom_Click);
            // 
            // MapZoomOut
            // 
            this.MapZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("MapZoomOut.Image")));
            this.MapZoomOut.Location = new System.Drawing.Point(31, 0);
            this.MapZoomOut.Name = "MapZoomOut";
            this.MapZoomOut.Size = new System.Drawing.Size(25, 23);
            this.MapZoomOut.TabIndex = 1;
            this.MapZoomOut.UseVisualStyleBackColor = true;
            this.MapZoomOut.Click += new System.EventHandler(this.MapZoomOut_Click);
            // 
            // MapZoomIn
            // 
            this.MapZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("MapZoomIn.Image")));
            this.MapZoomIn.Location = new System.Drawing.Point(3, 0);
            this.MapZoomIn.Name = "MapZoomIn";
            this.MapZoomIn.Size = new System.Drawing.Size(25, 23);
            this.MapZoomIn.TabIndex = 0;
            this.MapZoomIn.UseVisualStyleBackColor = true;
            this.MapZoomIn.Click += new System.EventHandler(this.MapZoomIn_Click);
            // 
            // MapView
            // 
            this.MapView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MapView.Location = new System.Drawing.Point(3, 109);
            this.MapView.Name = "MapView";
            this.MapView.Size = new System.Drawing.Size(441, 399);
            this.MapView.TabIndex = 0;
            this.MapView.TabStop = false;
            // 
            // ViewCheckPanel
            // 
            this.ViewCheckPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ViewCheckPanel.Controls.Add(this.ShowUnderlay);
            this.ViewCheckPanel.Controls.Add(this.ShowPortals);
            this.ViewCheckPanel.Controls.Add(this.ShowCellEdges);
            this.ViewCheckPanel.HighlightBGColor = System.Drawing.Color.CadetBlue;
            this.ViewCheckPanel.HighlightColor = System.Drawing.Color.BlueViolet;
            this.ViewCheckPanel.Location = new System.Drawing.Point(3, 3);
            this.ViewCheckPanel.Name = "ViewCheckPanel";
            this.ViewCheckPanel.SelectedItem = null;
            this.ViewCheckPanel.Size = new System.Drawing.Size(428, 37);
            this.ViewCheckPanel.TabIndex = 0;
            // 
            // ShowUnderlay
            // 
            this.ShowUnderlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowUnderlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowUnderlay.Image = ((System.Drawing.Image)(resources.GetObject("ShowUnderlay.Image")));
            this.ShowUnderlay.Location = new System.Drawing.Point(389, 1);
            this.ShowUnderlay.Name = "ShowUnderlay";
            this.ShowUnderlay.Size = new System.Drawing.Size(36, 36);
            this.ShowUnderlay.TabIndex = 2;
            this.ShowUnderlay.UseVisualStyleBackColor = true;
            // 
            // ShowPortals
            // 
            this.ShowPortals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowPortals.Image = ((System.Drawing.Image)(resources.GetObject("ShowPortals.Image")));
            this.ShowPortals.Location = new System.Drawing.Point(37, 0);
            this.ShowPortals.Name = "ShowPortals";
            this.ShowPortals.Size = new System.Drawing.Size(36, 36);
            this.ShowPortals.TabIndex = 1;
            this.ShowPortals.UseVisualStyleBackColor = true;
            // 
            // ShowCellEdges
            // 
            this.ShowCellEdges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowCellEdges.Image = ((System.Drawing.Image)(resources.GetObject("ShowCellEdges.Image")));
            this.ShowCellEdges.Location = new System.Drawing.Point(0, 0);
            this.ShowCellEdges.Name = "ShowCellEdges";
            this.ShowCellEdges.Size = new System.Drawing.Size(36, 36);
            this.ShowCellEdges.TabIndex = 0;
            this.ShowCellEdges.UseVisualStyleBackColor = true;
            // 
            // ViewRadioPanel
            // 
            this.ViewRadioPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ViewRadioPanel.Controls.Add(this.CellFillMode);
            this.ViewRadioPanel.Controls.Add(this.CellSelectButton);
            this.ViewRadioPanel.HighlightBGColor = System.Drawing.Color.DarkGoldenrod;
            this.ViewRadioPanel.HighlightColor = System.Drawing.Color.AliceBlue;
            this.ViewRadioPanel.Location = new System.Drawing.Point(3, 45);
            this.ViewRadioPanel.Name = "ViewRadioPanel";
            this.ViewRadioPanel.SelectedItem = null;
            this.ViewRadioPanel.Size = new System.Drawing.Size(382, 26);
            this.ViewRadioPanel.TabIndex = 4;
            this.ViewRadioPanel.TagsAreValues = false;
            // 
            // CellFillMode
            // 
            this.CellFillMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CellFillMode.Image = ((System.Drawing.Image)(resources.GetObject("CellFillMode.Image")));
            this.CellFillMode.Location = new System.Drawing.Point(34, 1);
            this.CellFillMode.Name = "CellFillMode";
            this.CellFillMode.Size = new System.Drawing.Size(25, 23);
            this.CellFillMode.TabIndex = 3;
            this.CellFillMode.Tag = PortalEdit.ViewEditMode.Paint;
            this.CellFillMode.UseVisualStyleBackColor = true;
            // 
            // CellSelectButton
            // 
            this.CellSelectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CellSelectButton.Image = ((System.Drawing.Image)(resources.GetObject("CellSelectButton.Image")));
            this.CellSelectButton.Location = new System.Drawing.Point(3, 1);
            this.CellSelectButton.Name = "CellSelectButton";
            this.CellSelectButton.Size = new System.Drawing.Size(25, 23);
            this.CellSelectButton.TabIndex = 2;
            this.CellSelectButton.Tag = PortalEdit.ViewEditMode.Select;
            this.CellSelectButton.UseVisualStyleBackColor = true;
            // 
            // GLView
            // 
            this.GLView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GLView.BackColor = System.Drawing.Color.Black;
            this.GLView.Location = new System.Drawing.Point(3, 77);
            this.GLView.Name = "GLView";
            this.GLView.Size = new System.Drawing.Size(428, 431);
            this.GLView.TabIndex = 0;
            this.GLView.VSync = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1106, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.recentFilesToolStripMenuItem,
            this.toolStripSeparator4,
            this.closeToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // recentFilesToolStripMenuItem
            // 
            this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
            this.recentFilesToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.recentFilesToolStripMenuItem.Text = "Recent Files";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(192, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(192, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator5,
            this.setupImageUnderlayToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(187, 6);
            // 
            // setupImageUnderlayToolStripMenuItem
            // 
            this.setupImageUnderlayToolStripMenuItem.Name = "setupImageUnderlayToolStripMenuItem";
            this.setupImageUnderlayToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.setupImageUnderlayToolStripMenuItem.Text = "Setup Image Underlay";
            this.setupImageUnderlayToolStripMenuItem.Click += new System.EventHandler(this.setupImageUnderlayToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator6,
            this.renameDepthGroupToolStripMenuItem,
            this.deleteDepthGroupToolStripMenuItem,
            this.toolStripSeparator7,
            this.newGroupToolStripMenuItem,
            this.renameGroupToolStripMenuItem,
            this.toolStripSeparator8,
            this.renameCellToolStripMenuItem,
            this.deleteCellToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(206, 6);
            // 
            // renameDepthGroupToolStripMenuItem
            // 
            this.renameDepthGroupToolStripMenuItem.Name = "renameDepthGroupToolStripMenuItem";
            this.renameDepthGroupToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.renameDepthGroupToolStripMenuItem.Text = "Rename Depth Group...";
            this.renameDepthGroupToolStripMenuItem.Click += new System.EventHandler(this.renameDepthGroupToolStripMenuItem_Click);
            // 
            // deleteDepthGroupToolStripMenuItem
            // 
            this.deleteDepthGroupToolStripMenuItem.Name = "deleteDepthGroupToolStripMenuItem";
            this.deleteDepthGroupToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.deleteDepthGroupToolStripMenuItem.Text = "Delete Depth Group";
            this.deleteDepthGroupToolStripMenuItem.Click += new System.EventHandler(this.deleteDepthGroupToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(206, 6);
            // 
            // newGroupToolStripMenuItem
            // 
            this.newGroupToolStripMenuItem.Name = "newGroupToolStripMenuItem";
            this.newGroupToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.N)));
            this.newGroupToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.newGroupToolStripMenuItem.Text = "New Group";
            this.newGroupToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
            // 
            // renameGroupToolStripMenuItem
            // 
            this.renameGroupToolStripMenuItem.Name = "renameGroupToolStripMenuItem";
            this.renameGroupToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.renameGroupToolStripMenuItem.Text = "Rename Group...";
            this.renameGroupToolStripMenuItem.Click += new System.EventHandler(this.renameGroupToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(206, 6);
            // 
            // renameCellToolStripMenuItem
            // 
            this.renameCellToolStripMenuItem.Name = "renameCellToolStripMenuItem";
            this.renameCellToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.renameCellToolStripMenuItem.Text = "Rename Cell...";
            this.renameCellToolStripMenuItem.Click += new System.EventHandler(this.renameCellToolStripMenuItem_Click);
            // 
            // deleteCellToolStripMenuItem
            // 
            this.deleteCellToolStripMenuItem.Name = "deleteCellToolStripMenuItem";
            this.deleteCellToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteCellToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.deleteCellToolStripMenuItem.Text = "Delete Cell";
            this.deleteCellToolStripMenuItem.Click += new System.EventHandler(this.DeleteCell_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MousePositionStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 543);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1106, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // MousePositionStatus
            // 
            this.MousePositionStatus.Name = "MousePositionStatus";
            this.MousePositionStatus.Size = new System.Drawing.Size(46, 17);
            this.MousePositionStatus.Text = "Mouse:";
            // 
            // MapTreeIcons
            // 
            this.MapTreeIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MapTreeIcons.ImageStream")));
            this.MapTreeIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.MapTreeIcons.Images.SetKeyName(0, "cell_group_tree_icon.png");
            this.MapTreeIcons.Images.SetKeyName(1, "cell_tree_icon.png");
            this.MapTreeIcons.Images.SetKeyName(2, "cell_group_tree_icon_selected.png");
            this.MapTreeIcons.Images.SetKeyName(3, "cell_tree_icon_selected.png");
            // 
            // GroupRightMouseMenu
            // 
            this.GroupRightMouseMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RenameGroupRMM});
            this.GroupRightMouseMenu.Name = "GroupRightMouseMenu";
            this.GroupRightMouseMenu.Size = new System.Drawing.Size(118, 26);
            // 
            // RenameGroupRMM
            // 
            this.RenameGroupRMM.Name = "RenameGroupRMM";
            this.RenameGroupRMM.Size = new System.Drawing.Size(117, 22);
            this.RenameGroupRMM.Text = "Rename";
            this.RenameGroupRMM.Click += new System.EventHandler(this.RenameGroupRMM_Click);
            // 
            // CellRightMouseMenu
            // 
            this.CellRightMouseMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RenameCellRMM,
            this.DeleteCellRMM});
            this.CellRightMouseMenu.Name = "GroupRightMouseMenu";
            this.CellRightMouseMenu.Size = new System.Drawing.Size(118, 48);
            // 
            // RenameCellRMM
            // 
            this.RenameCellRMM.Name = "RenameCellRMM";
            this.RenameCellRMM.Size = new System.Drawing.Size(117, 22);
            this.RenameCellRMM.Text = "Rename";
            this.RenameCellRMM.Click += new System.EventHandler(this.RenameCellRMM_Click);
            // 
            // DeleteCellRMM
            // 
            this.DeleteCellRMM.Name = "DeleteCellRMM";
            this.DeleteCellRMM.Size = new System.Drawing.Size(117, 22);
            this.DeleteCellRMM.Text = "Delete";
            this.DeleteCellRMM.Click += new System.EventHandler(this.DeleteCell_Click);
            // 
            // SidebarSplitter
            // 
            this.SidebarSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SidebarSplitter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SidebarSplitter.Location = new System.Drawing.Point(895, 27);
            this.SidebarSplitter.Name = "SidebarSplitter";
            this.SidebarSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SidebarSplitter.Panel1
            // 
            this.SidebarSplitter.Panel1.Controls.Add(this.MapTree);
            // 
            // SidebarSplitter.Panel2
            // 
            this.SidebarSplitter.Panel2.Controls.Add(this.CellTabControl);
            this.SidebarSplitter.Size = new System.Drawing.Size(211, 509);
            this.SidebarSplitter.SplitterDistance = 217;
            this.SidebarSplitter.TabIndex = 12;
            // 
            // MapTree
            // 
            this.MapTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MapTree.ContextMenuStrip = this.MapTreeRightMouseMenu;
            this.MapTree.FullRowSelect = true;
            this.MapTree.HideSelection = false;
            this.MapTree.ImageIndex = 0;
            this.MapTree.ImageList = this.MapTreeIcons;
            this.MapTree.Location = new System.Drawing.Point(3, 3);
            this.MapTree.Name = "MapTree";
            this.MapTree.SelectedImageIndex = 0;
            this.MapTree.Size = new System.Drawing.Size(203, 209);
            this.MapTree.TabIndex = 7;
            this.MapTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MapTree_AfterSelect);
            this.MapTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.MapTree_NodeMouseClick);
            // 
            // MapTreeRightMouseMenu
            // 
            this.MapTreeRightMouseMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem1,
            this.deselectToolStripMenuItem});
            this.MapTreeRightMouseMenu.Name = "MapTreeRightMouseMenu";
            this.MapTreeRightMouseMenu.Size = new System.Drawing.Size(119, 48);
            // 
            // newToolStripMenuItem1
            // 
            this.newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            this.newToolStripMenuItem1.Size = new System.Drawing.Size(118, 22);
            this.newToolStripMenuItem1.Text = "New";
            this.newToolStripMenuItem1.Click += new System.EventHandler(this.newToolStripMenuItem1_Click);
            // 
            // deselectToolStripMenuItem
            // 
            this.deselectToolStripMenuItem.Name = "deselectToolStripMenuItem";
            this.deselectToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.deselectToolStripMenuItem.Text = "Deselect";
            this.deselectToolStripMenuItem.Click += new System.EventHandler(this.deselectToolStripMenuItem_Click);
            // 
            // CellTabControl
            // 
            this.CellTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CellTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.CellTabControl.Controls.Add(this.CellInfo);
            this.CellTabControl.Controls.Add(this.VertInfo);
            this.CellTabControl.Controls.Add(this.EdgeInfo);
            this.CellTabControl.Controls.Add(this.FaceInfo);
            this.CellTabControl.Location = new System.Drawing.Point(-3, 3);
            this.CellTabControl.Multiline = true;
            this.CellTabControl.Name = "CellTabControl";
            this.CellTabControl.SelectedIndex = 0;
            this.CellTabControl.Size = new System.Drawing.Size(209, 279);
            this.CellTabControl.TabIndex = 11;
            // 
            // CellInfo
            // 
            this.CellInfo.Controls.Add(this.CellGroupDropdown);
            this.CellInfo.Controls.Add(this.CellGroupDropdownLabel);
            this.CellInfo.Location = new System.Drawing.Point(4, 25);
            this.CellInfo.Name = "CellInfo";
            this.CellInfo.Padding = new System.Windows.Forms.Padding(3);
            this.CellInfo.Size = new System.Drawing.Size(201, 250);
            this.CellInfo.TabIndex = 0;
            this.CellInfo.Text = "Cell";
            this.CellInfo.UseVisualStyleBackColor = true;
            // 
            // CellGroupDropdown
            // 
            this.CellGroupDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CellGroupDropdown.FormattingEnabled = true;
            this.CellGroupDropdown.Location = new System.Drawing.Point(48, 6);
            this.CellGroupDropdown.Name = "CellGroupDropdown";
            this.CellGroupDropdown.Size = new System.Drawing.Size(139, 21);
            this.CellGroupDropdown.TabIndex = 3;
            this.CellGroupDropdown.SelectedIndexChanged += new System.EventHandler(this.CellGroupDropdown_SelectedIndexChanged);
            // 
            // CellGroupDropdownLabel
            // 
            this.CellGroupDropdownLabel.AutoSize = true;
            this.CellGroupDropdownLabel.Location = new System.Drawing.Point(6, 9);
            this.CellGroupDropdownLabel.Name = "CellGroupDropdownLabel";
            this.CellGroupDropdownLabel.Size = new System.Drawing.Size(36, 13);
            this.CellGroupDropdownLabel.TabIndex = 2;
            this.CellGroupDropdownLabel.Text = "Group";
            // 
            // VertInfo
            // 
            this.VertInfo.Controls.Add(this.CellInfoZIsInc);
            this.VertInfo.Controls.Add(this.CellVertList);
            this.VertInfo.Location = new System.Drawing.Point(4, 25);
            this.VertInfo.Name = "VertInfo";
            this.VertInfo.Padding = new System.Windows.Forms.Padding(3);
            this.VertInfo.Size = new System.Drawing.Size(201, 250);
            this.VertInfo.TabIndex = 1;
            this.VertInfo.Text = "Verts";
            this.VertInfo.UseVisualStyleBackColor = true;
            // 
            // CellInfoZIsInc
            // 
            this.CellInfoZIsInc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CellInfoZIsInc.AutoSize = true;
            this.CellInfoZIsInc.Location = new System.Drawing.Point(3, 227);
            this.CellInfoZIsInc.Name = "CellInfoZIsInc";
            this.CellInfoZIsInc.Size = new System.Drawing.Size(139, 17);
            this.CellInfoZIsInc.TabIndex = 2;
            this.CellInfoZIsInc.Text = "Heights Are Incremental";
            this.CellInfoZIsInc.UseVisualStyleBackColor = true;
            // 
            // CellVertList
            // 
            this.CellVertList.AllowUserToAddRows = false;
            this.CellVertList.AllowUserToDeleteRows = false;
            this.CellVertList.AllowUserToResizeRows = false;
            this.CellVertList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CellVertList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CellVertList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.CellVertList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CellVertList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.ZMin,
            this.PlanarFloor,
            this.ZMax,
            this.PlanarRoof});
            this.CellVertList.ContextMenuStrip = this.VertListRightMouseMenu;
            this.CellVertList.Location = new System.Drawing.Point(4, 6);
            this.CellVertList.MultiSelect = false;
            this.CellVertList.Name = "CellVertList";
            this.CellVertList.RowHeadersWidth = 4;
            this.CellVertList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CellVertList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CellVertList.Size = new System.Drawing.Size(191, 215);
            this.CellVertList.TabIndex = 0;
            this.CellVertList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellVertList_CellValueChanged);
            this.CellVertList.SelectionChanged += new System.EventHandler(this.CellVertList_SelectionChanged);
            // 
            // Index
            // 
            this.Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Index.HeaderText = "#";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Index.Width = 20;
            // 
            // ZMin
            // 
            this.ZMin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ZMin.HeaderText = "Z-";
            this.ZMin.Name = "ZMin";
            this.ZMin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PlanarFloor
            // 
            this.PlanarFloor.HeaderText = "Pln";
            this.PlanarFloor.Name = "PlanarFloor";
            this.PlanarFloor.ReadOnly = true;
            // 
            // ZMax
            // 
            this.ZMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ZMax.HeaderText = "Z+";
            this.ZMax.Name = "ZMax";
            this.ZMax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PlanarRoof
            // 
            this.PlanarRoof.HeaderText = "Pln";
            this.PlanarRoof.Name = "PlanarRoof";
            this.PlanarRoof.ReadOnly = true;
            // 
            // EdgeInfo
            // 
            this.EdgeInfo.Controls.Add(this.CellEdgeList);
            this.EdgeInfo.Location = new System.Drawing.Point(4, 25);
            this.EdgeInfo.Name = "EdgeInfo";
            this.EdgeInfo.Size = new System.Drawing.Size(201, 250);
            this.EdgeInfo.TabIndex = 2;
            this.EdgeInfo.Text = "Edges";
            this.EdgeInfo.UseVisualStyleBackColor = true;
            // 
            // CellEdgeList
            // 
            this.CellEdgeList.AllowUserToAddRows = false;
            this.CellEdgeList.AllowUserToDeleteRows = false;
            this.CellEdgeList.AllowUserToResizeRows = false;
            this.CellEdgeList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CellEdgeList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CellEdgeList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.CellEdgeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CellEdgeList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.CellEdgeList.Location = new System.Drawing.Point(3, 3);
            this.CellEdgeList.MultiSelect = false;
            this.CellEdgeList.Name = "CellEdgeList";
            this.CellEdgeList.RowHeadersWidth = 4;
            this.CellEdgeList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CellEdgeList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CellEdgeList.Size = new System.Drawing.Size(191, 168);
            this.CellEdgeList.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "#";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 20;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Type";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Visible";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // FaceInfo
            // 
            this.FaceInfo.Location = new System.Drawing.Point(4, 25);
            this.FaceInfo.Name = "FaceInfo";
            this.FaceInfo.Size = new System.Drawing.Size(201, 250);
            this.FaceInfo.TabIndex = 3;
            this.FaceInfo.Text = "Faces";
            this.FaceInfo.UseVisualStyleBackColor = true;
            // 
            // VertListRightMouseMenu
            // 
            this.VertListRightMouseMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setToPlaneToolStripMenuItem});
            this.VertListRightMouseMenu.Name = "VertListRightMouseMenu";
            this.VertListRightMouseMenu.Size = new System.Drawing.Size(153, 48);
            // 
            // setToPlaneToolStripMenuItem
            // 
            this.setToPlaneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zToolStripMenuItem,
            this.zToolStripMenuItem1});
            this.setToPlaneToolStripMenuItem.Name = "setToPlaneToolStripMenuItem";
            this.setToPlaneToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.setToPlaneToolStripMenuItem.Text = "Set To Plane";
            // 
            // zToolStripMenuItem
            // 
            this.zToolStripMenuItem.Name = "zToolStripMenuItem";
            this.zToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.zToolStripMenuItem.Text = "Z+ (Roof)";
            this.zToolStripMenuItem.Click += new System.EventHandler(this.zToolStripMenuItem_Click);
            // 
            // zToolStripMenuItem1
            // 
            this.zToolStripMenuItem1.Name = "zToolStripMenuItem1";
            this.zToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.zToolStripMenuItem1.Text = "Z- (Floor)";
            this.zToolStripMenuItem1.Click += new System.EventHandler(this.zToolStripMenuItem1_Click);
            // 
            // EditFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 565);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MainContainer);
            this.Controls.Add(this.SidebarSplitter);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(614, 450);
            this.Name = "EditFrame";
            this.Text = "Portal Edit: New Map";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditFrame_FormClosing);
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel2.ResumeLayout(false);
            this.MainContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HideAboveZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HideBelowZ)).EndInit();
            this.DepthEditPanel.ResumeLayout(false);
            this.DepthEditPanel.PerformLayout();
            this.MapEditToolsPanel.ResumeLayout(false);
            this.MapRadioPanel.ResumeLayout(false);
            this.MapZoomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MapView)).EndInit();
            this.ViewCheckPanel.ResumeLayout(false);
            this.ViewRadioPanel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.GroupRightMouseMenu.ResumeLayout(false);
            this.CellRightMouseMenu.ResumeLayout(false);
            this.SidebarSplitter.Panel1.ResumeLayout(false);
            this.SidebarSplitter.Panel2.ResumeLayout(false);
            this.SidebarSplitter.ResumeLayout(false);
            this.MapTreeRightMouseMenu.ResumeLayout(false);
            this.CellTabControl.ResumeLayout(false);
            this.CellInfo.ResumeLayout(false);
            this.CellInfo.PerformLayout();
            this.VertInfo.ResumeLayout(false);
            this.VertInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CellVertList)).EndInit();
            this.EdgeInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CellEdgeList)).EndInit();
            this.VertListRightMouseMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer MainContainer;
        private System.Windows.Forms.PictureBox MapView;
        private OpenTK.GLControl GLView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel MousePositionStatus;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button DrawButton;
        private System.Windows.Forms.Button SelectButton;
        private FormControls.ImageRadioPanel MapRadioPanel;
        private System.Windows.Forms.Panel MapZoomPanel;
        private System.Windows.Forms.Button MapZoomIn;
        private System.Windows.Forms.Button MapZoomOut;
        private FormControls.ImageRadioPanel ViewRadioPanel;
        private System.Windows.Forms.Button CellFillMode;
        private System.Windows.Forms.Button CellSelectButton;
        private System.Windows.Forms.ImageList MapTreeIcons;
        private FormControls.ImageCheckPanel ViewCheckPanel;
        private System.Windows.Forms.Button ShowCellEdges;
        private System.Windows.Forms.Button ShowPortals;
        private System.Windows.Forms.Panel MapEditToolsPanel;
        private System.Windows.Forms.Panel DepthEditPanel;
        private System.Windows.Forms.TextBox EditZPlus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox EditZMinus;
        private System.Windows.Forms.Label ZMinusLabel;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.Button EditLoadZFromSelection;
        private System.Windows.Forms.CheckBox EditIncZ;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Button ResetZoom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem recentFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem setupImageUnderlayToolStripMenuItem;
        private System.Windows.Forms.ComboBox NamedDepthPresets;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem renameDepthGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteDepthGroupToolStripMenuItem;
        private System.Windows.Forms.Button ShowUnderlay;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem renameGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameCellToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip GroupRightMouseMenu;
        private System.Windows.Forms.ToolStripMenuItem RenameGroupRMM;
        private System.Windows.Forms.ContextMenuStrip CellRightMouseMenu;
        private System.Windows.Forms.ToolStripMenuItem RenameCellRMM;
        private System.Windows.Forms.ToolStripMenuItem DeleteCellRMM;
        private System.Windows.Forms.SplitContainer SidebarSplitter;
        public System.Windows.Forms.TreeView MapTree;
        private System.Windows.Forms.TabControl CellTabControl;
        private System.Windows.Forms.TabPage CellInfo;
        private System.Windows.Forms.ComboBox CellGroupDropdown;
        private System.Windows.Forms.Label CellGroupDropdownLabel;
        private System.Windows.Forms.TabPage VertInfo;
        private System.Windows.Forms.CheckBox CellInfoZIsInc;
        public System.Windows.Forms.DataGridView CellVertList;
        private System.Windows.Forms.TabPage EdgeInfo;
        public System.Windows.Forms.DataGridView CellEdgeList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.TabPage FaceInfo;
        private System.Windows.Forms.ContextMenuStrip MapTreeRightMouseMenu;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deselectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCellToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.NumericUpDown HideAboveZ;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown HideBelowZ;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox HideGeo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZMin;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PlanarFloor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZMax;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PlanarRoof;
        private System.Windows.Forms.ContextMenuStrip VertListRightMouseMenu;
        private System.Windows.Forms.ToolStripMenuItem setToPlaneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zToolStripMenuItem1;
    }
}
