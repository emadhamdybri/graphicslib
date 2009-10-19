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
            this.LightingModeButton = new System.Windows.Forms.Button();
            this.EditVertButton = new System.Windows.Forms.Button();
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
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.computeLightmapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.RoofVis = new System.Windows.Forms.CheckBox();
            this.FloorViz = new System.Windows.Forms.CheckBox();
            this.SetCellToPreset = new System.Windows.Forms.Button();
            this.MoveRoof = new System.Windows.Forms.Button();
            this.MoveFloor = new System.Windows.Forms.Button();
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
            this.VertListRightMouseMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setToPlaneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.EdgeInfo = new System.Windows.Forms.TabPage();
            this.CellEdgeList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FaceInfo = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.VScale = new System.Windows.Forms.NumericUpDown();
            this.UScale = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ULabel = new System.Windows.Forms.Label();
            this.VShift = new System.Windows.Forms.NumericUpDown();
            this.UShift = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.FaceMatInfo = new System.Windows.Forms.TextBox();
            this.TextureInfo = new System.Windows.Forms.TabPage();
            this.PreviewInfo = new System.Windows.Forms.TextBox();
            this.TexturePreview = new System.Windows.Forms.PictureBox();
            this.TextureList = new System.Windows.Forms.TreeView();
            this.ObjectInfo = new System.Windows.Forms.TabPage();
            this.ObjectInfoBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ObjectPosZ = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.ObjectPosY = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.ObjectPosX = new System.Windows.Forms.NumericUpDown();
            this.ObjectList = new System.Windows.Forms.ListBox();
            this.ObjectListMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ObjectListNewMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LightInfo = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.AmbientLabel = new System.Windows.Forms.Label();
            this.MinRad = new System.Windows.Forms.NumericUpDown();
            this.AmbientLevel = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.LightIntensity = new System.Windows.Forms.NumericUpDown();
            this.LightList = new System.Windows.Forms.ListView();
            this.LightListRMM = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NewLight = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveLight = new System.Windows.Forms.ToolStripMenuItem();
            this.DefaultImages = new System.Windows.Forms.ImageList(this.components);
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
            this.VertListRightMouseMenu.SuspendLayout();
            this.EdgeInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CellEdgeList)).BeginInit();
            this.FaceInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UShift)).BeginInit();
            this.TextureInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TexturePreview)).BeginInit();
            this.ObjectInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectPosZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectPosX)).BeginInit();
            this.ObjectListMenuStrip.SuspendLayout();
            this.LightInfo.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinRad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmbientLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightIntensity)).BeginInit();
            this.LightListRMM.SuspendLayout();
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
            this.MainContainer.Size = new System.Drawing.Size(838, 513);
            this.MainContainer.SplitterDistance = 418;
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
            this.MapEditToolsPanel.Size = new System.Drawing.Size(403, 29);
            this.MapEditToolsPanel.TabIndex = 5;
            // 
            // MapRadioPanel
            // 
            this.MapRadioPanel.Controls.Add(this.LightingModeButton);
            this.MapRadioPanel.Controls.Add(this.EditVertButton);
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
            // LightingModeButton
            // 
            this.LightingModeButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.LightingModeButton.Image = ((System.Drawing.Image)(resources.GetObject("LightingModeButton.Image")));
            this.LightingModeButton.Location = new System.Drawing.Point(83, 1);
            this.LightingModeButton.Name = "LightingModeButton";
            this.LightingModeButton.Size = new System.Drawing.Size(25, 23);
            this.LightingModeButton.TabIndex = 4;
            this.LightingModeButton.Tag = PortalEdit.MapEditMode.EditLightMode;
            this.LightingModeButton.UseVisualStyleBackColor = true;
            // 
            // EditVertButton
            // 
            this.EditVertButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.EditVertButton.Image = ((System.Drawing.Image)(resources.GetObject("EditVertButton.Image")));
            this.EditVertButton.Location = new System.Drawing.Point(55, 1);
            this.EditVertButton.Name = "EditVertButton";
            this.EditVertButton.Size = new System.Drawing.Size(25, 23);
            this.EditVertButton.TabIndex = 3;
            this.EditVertButton.Tag = PortalEdit.MapEditMode.EditVertMode;
            this.EditVertButton.UseVisualStyleBackColor = true;
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
            this.MapZoomPanel.Location = new System.Drawing.Point(296, 0);
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
            this.MapView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapView.Location = new System.Drawing.Point(3, 109);
            this.MapView.Name = "MapView";
            this.MapView.Size = new System.Drawing.Size(407, 399);
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
            this.ViewCheckPanel.Size = new System.Drawing.Size(400, 37);
            this.ViewCheckPanel.TabIndex = 0;
            // 
            // ShowUnderlay
            // 
            this.ShowUnderlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowUnderlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowUnderlay.Image = ((System.Drawing.Image)(resources.GetObject("ShowUnderlay.Image")));
            this.ShowUnderlay.Location = new System.Drawing.Point(353, 1);
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
            this.ViewRadioPanel.Size = new System.Drawing.Size(354, 26);
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
            this.GLView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GLView.Location = new System.Drawing.Point(3, 77);
            this.GLView.Name = "GLView";
            this.GLView.Size = new System.Drawing.Size(408, 427);
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
            this.deleteCellToolStripMenuItem,
            this.toolStripSeparator9,
            this.computeLightmapsToolStripMenuItem});
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
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(206, 6);
            // 
            // computeLightmapsToolStripMenuItem
            // 
            this.computeLightmapsToolStripMenuItem.Name = "computeLightmapsToolStripMenuItem";
            this.computeLightmapsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.computeLightmapsToolStripMenuItem.Text = "Compute Lightmaps";
            this.computeLightmapsToolStripMenuItem.Click += new System.EventHandler(this.computeLightmapsToolStripMenuItem_Click);
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
            this.SidebarSplitter.Location = new System.Drawing.Point(844, 27);
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
            this.SidebarSplitter.Size = new System.Drawing.Size(262, 509);
            this.SidebarSplitter.SplitterDistance = 170;
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
            this.MapTree.Size = new System.Drawing.Size(254, 162);
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
            this.CellTabControl.Controls.Add(this.TextureInfo);
            this.CellTabControl.Controls.Add(this.ObjectInfo);
            this.CellTabControl.Controls.Add(this.LightInfo);
            this.CellTabControl.Location = new System.Drawing.Point(3, 3);
            this.CellTabControl.Multiline = true;
            this.CellTabControl.Name = "CellTabControl";
            this.CellTabControl.SelectedIndex = 0;
            this.CellTabControl.Size = new System.Drawing.Size(254, 331);
            this.CellTabControl.TabIndex = 11;
            // 
            // CellInfo
            // 
            this.CellInfo.Controls.Add(this.RoofVis);
            this.CellInfo.Controls.Add(this.FloorViz);
            this.CellInfo.Controls.Add(this.SetCellToPreset);
            this.CellInfo.Controls.Add(this.MoveRoof);
            this.CellInfo.Controls.Add(this.MoveFloor);
            this.CellInfo.Controls.Add(this.CellGroupDropdown);
            this.CellInfo.Controls.Add(this.CellGroupDropdownLabel);
            this.CellInfo.Location = new System.Drawing.Point(4, 49);
            this.CellInfo.Name = "CellInfo";
            this.CellInfo.Padding = new System.Windows.Forms.Padding(3);
            this.CellInfo.Size = new System.Drawing.Size(246, 278);
            this.CellInfo.TabIndex = 0;
            this.CellInfo.Text = "Cell";
            this.CellInfo.UseVisualStyleBackColor = true;
            // 
            // RoofVis
            // 
            this.RoofVis.AutoSize = true;
            this.RoofVis.Location = new System.Drawing.Point(6, 124);
            this.RoofVis.Name = "RoofVis";
            this.RoofVis.Size = new System.Drawing.Size(82, 17);
            this.RoofVis.TabIndex = 8;
            this.RoofVis.Text = "Roof Vizible";
            this.RoofVis.UseVisualStyleBackColor = true;
            this.RoofVis.CheckedChanged += new System.EventHandler(this.RoofVis_CheckedChanged);
            // 
            // FloorViz
            // 
            this.FloorViz.AutoSize = true;
            this.FloorViz.Location = new System.Drawing.Point(6, 101);
            this.FloorViz.Name = "FloorViz";
            this.FloorViz.Size = new System.Drawing.Size(82, 17);
            this.FloorViz.TabIndex = 7;
            this.FloorViz.Text = "Floor Vizible";
            this.FloorViz.UseVisualStyleBackColor = true;
            this.FloorViz.CheckedChanged += new System.EventHandler(this.FloorViz_CheckedChanged);
            // 
            // SetCellToPreset
            // 
            this.SetCellToPreset.Location = new System.Drawing.Point(6, 72);
            this.SetCellToPreset.Name = "SetCellToPreset";
            this.SetCellToPreset.Size = new System.Drawing.Size(75, 23);
            this.SetCellToPreset.TabIndex = 6;
            this.SetCellToPreset.Text = "Preset";
            this.SetCellToPreset.UseVisualStyleBackColor = true;
            this.SetCellToPreset.Click += new System.EventHandler(this.SetCellToPreset_Click);
            // 
            // MoveRoof
            // 
            this.MoveRoof.Location = new System.Drawing.Point(87, 43);
            this.MoveRoof.Name = "MoveRoof";
            this.MoveRoof.Size = new System.Drawing.Size(75, 23);
            this.MoveRoof.TabIndex = 5;
            this.MoveRoof.Text = "Move Roof";
            this.MoveRoof.UseVisualStyleBackColor = true;
            this.MoveRoof.Click += new System.EventHandler(this.MoveRoof_Click);
            // 
            // MoveFloor
            // 
            this.MoveFloor.Location = new System.Drawing.Point(6, 43);
            this.MoveFloor.Name = "MoveFloor";
            this.MoveFloor.Size = new System.Drawing.Size(75, 23);
            this.MoveFloor.TabIndex = 4;
            this.MoveFloor.Text = "Move Floor";
            this.MoveFloor.UseVisualStyleBackColor = true;
            this.MoveFloor.Click += new System.EventHandler(this.MoveFloor_Click);
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
            this.VertInfo.Location = new System.Drawing.Point(4, 49);
            this.VertInfo.Name = "VertInfo";
            this.VertInfo.Padding = new System.Windows.Forms.Padding(3);
            this.VertInfo.Size = new System.Drawing.Size(246, 278);
            this.VertInfo.TabIndex = 1;
            this.VertInfo.Text = "Verts";
            this.VertInfo.UseVisualStyleBackColor = true;
            // 
            // CellInfoZIsInc
            // 
            this.CellInfoZIsInc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CellInfoZIsInc.AutoSize = true;
            this.CellInfoZIsInc.Location = new System.Drawing.Point(3, 387);
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
            this.CellVertList.Size = new System.Drawing.Size(191, 314);
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
            // VertListRightMouseMenu
            // 
            this.VertListRightMouseMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setToPlaneToolStripMenuItem});
            this.VertListRightMouseMenu.Name = "VertListRightMouseMenu";
            this.VertListRightMouseMenu.Size = new System.Drawing.Size(140, 26);
            // 
            // setToPlaneToolStripMenuItem
            // 
            this.setToPlaneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zToolStripMenuItem,
            this.zToolStripMenuItem1});
            this.setToPlaneToolStripMenuItem.Name = "setToPlaneToolStripMenuItem";
            this.setToPlaneToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.setToPlaneToolStripMenuItem.Text = "Set To Plane";
            // 
            // zToolStripMenuItem
            // 
            this.zToolStripMenuItem.Name = "zToolStripMenuItem";
            this.zToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.zToolStripMenuItem.Text = "Z+ (Roof)";
            this.zToolStripMenuItem.Click += new System.EventHandler(this.zToolStripMenuItem_Click);
            // 
            // zToolStripMenuItem1
            // 
            this.zToolStripMenuItem1.Name = "zToolStripMenuItem1";
            this.zToolStripMenuItem1.Size = new System.Drawing.Size(125, 22);
            this.zToolStripMenuItem1.Text = "Z- (Floor)";
            this.zToolStripMenuItem1.Click += new System.EventHandler(this.zToolStripMenuItem1_Click);
            // 
            // EdgeInfo
            // 
            this.EdgeInfo.Controls.Add(this.CellEdgeList);
            this.EdgeInfo.Location = new System.Drawing.Point(4, 49);
            this.EdgeInfo.Name = "EdgeInfo";
            this.EdgeInfo.Size = new System.Drawing.Size(246, 278);
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
            this.CellEdgeList.Size = new System.Drawing.Size(191, 320);
            this.CellEdgeList.TabIndex = 1;
            this.CellEdgeList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.CellEdgeList_CellValueChanged);
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
            this.FaceInfo.Controls.Add(this.label6);
            this.FaceInfo.Controls.Add(this.label7);
            this.FaceInfo.Controls.Add(this.VScale);
            this.FaceInfo.Controls.Add(this.UScale);
            this.FaceInfo.Controls.Add(this.label8);
            this.FaceInfo.Controls.Add(this.label5);
            this.FaceInfo.Controls.Add(this.ULabel);
            this.FaceInfo.Controls.Add(this.VShift);
            this.FaceInfo.Controls.Add(this.UShift);
            this.FaceInfo.Controls.Add(this.label4);
            this.FaceInfo.Controls.Add(this.FaceMatInfo);
            this.FaceInfo.Location = new System.Drawing.Point(4, 49);
            this.FaceInfo.Name = "FaceInfo";
            this.FaceInfo.Size = new System.Drawing.Size(246, 278);
            this.FaceInfo.TabIndex = 3;
            this.FaceInfo.Text = "Face";
            this.FaceInfo.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(86, 270);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "V";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 270);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "U";
            // 
            // VScale
            // 
            this.VScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VScale.DecimalPlaces = 3;
            this.VScale.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.VScale.Location = new System.Drawing.Point(106, 268);
            this.VScale.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.VScale.Name = "VScale";
            this.VScale.Size = new System.Drawing.Size(56, 20);
            this.VScale.TabIndex = 8;
            this.VScale.ValueChanged += new System.EventHandler(this.UShift_ValueChanged);
            // 
            // UScale
            // 
            this.UScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UScale.DecimalPlaces = 3;
            this.UScale.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.UScale.Location = new System.Drawing.Point(24, 268);
            this.UScale.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.UScale.Name = "UScale";
            this.UScale.Size = new System.Drawing.Size(56, 20);
            this.UScale.TabIndex = 7;
            this.UScale.ValueChanged += new System.EventHandler(this.UShift_ValueChanged);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 252);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "UV Size";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(86, 219);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "V";
            // 
            // ULabel
            // 
            this.ULabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ULabel.AutoSize = true;
            this.ULabel.Location = new System.Drawing.Point(3, 219);
            this.ULabel.Name = "ULabel";
            this.ULabel.Size = new System.Drawing.Size(15, 13);
            this.ULabel.TabIndex = 4;
            this.ULabel.Text = "U";
            // 
            // VShift
            // 
            this.VShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VShift.DecimalPlaces = 3;
            this.VShift.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.VShift.Location = new System.Drawing.Point(106, 217);
            this.VShift.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.VShift.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.VShift.Name = "VShift";
            this.VShift.Size = new System.Drawing.Size(56, 20);
            this.VShift.TabIndex = 3;
            this.VShift.ValueChanged += new System.EventHandler(this.UShift_ValueChanged);
            // 
            // UShift
            // 
            this.UShift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.UShift.DecimalPlaces = 3;
            this.UShift.Increment = new decimal(new int[] {
            5,
            0,
            0,
            196608});
            this.UShift.Location = new System.Drawing.Point(24, 217);
            this.UShift.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UShift.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.UShift.Name = "UShift";
            this.UShift.Size = new System.Drawing.Size(56, 20);
            this.UShift.TabIndex = 2;
            this.UShift.ValueChanged += new System.EventHandler(this.UShift_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "UV Shift";
            // 
            // FaceMatInfo
            // 
            this.FaceMatInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FaceMatInfo.Location = new System.Drawing.Point(3, 3);
            this.FaceMatInfo.Multiline = true;
            this.FaceMatInfo.Name = "FaceMatInfo";
            this.FaceMatInfo.ReadOnly = true;
            this.FaceMatInfo.Size = new System.Drawing.Size(195, 194);
            this.FaceMatInfo.TabIndex = 0;
            // 
            // TextureInfo
            // 
            this.TextureInfo.Controls.Add(this.PreviewInfo);
            this.TextureInfo.Controls.Add(this.TexturePreview);
            this.TextureInfo.Controls.Add(this.TextureList);
            this.TextureInfo.Location = new System.Drawing.Point(4, 49);
            this.TextureInfo.Name = "TextureInfo";
            this.TextureInfo.Size = new System.Drawing.Size(246, 278);
            this.TextureInfo.TabIndex = 4;
            this.TextureInfo.Text = "Textures";
            this.TextureInfo.UseVisualStyleBackColor = true;
            // 
            // PreviewInfo
            // 
            this.PreviewInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PreviewInfo.Location = new System.Drawing.Point(121, 162);
            this.PreviewInfo.Multiline = true;
            this.PreviewInfo.Name = "PreviewInfo";
            this.PreviewInfo.ReadOnly = true;
            this.PreviewInfo.Size = new System.Drawing.Size(123, 113);
            this.PreviewInfo.TabIndex = 2;
            // 
            // TexturePreview
            // 
            this.TexturePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TexturePreview.Location = new System.Drawing.Point(3, 162);
            this.TexturePreview.Name = "TexturePreview";
            this.TexturePreview.Size = new System.Drawing.Size(112, 112);
            this.TexturePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.TexturePreview.TabIndex = 1;
            this.TexturePreview.TabStop = false;
            // 
            // TextureList
            // 
            this.TextureList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TextureList.Location = new System.Drawing.Point(3, 3);
            this.TextureList.Name = "TextureList";
            this.TextureList.Size = new System.Drawing.Size(241, 153);
            this.TextureList.TabIndex = 0;
            this.TextureList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TextureList_AfterSelect);
            // 
            // ObjectInfo
            // 
            this.ObjectInfo.Controls.Add(this.ObjectInfoBox);
            this.ObjectInfo.Controls.Add(this.label11);
            this.ObjectInfo.Controls.Add(this.ObjectPosZ);
            this.ObjectInfo.Controls.Add(this.label10);
            this.ObjectInfo.Controls.Add(this.ObjectPosY);
            this.ObjectInfo.Controls.Add(this.label9);
            this.ObjectInfo.Controls.Add(this.ObjectPosX);
            this.ObjectInfo.Controls.Add(this.ObjectList);
            this.ObjectInfo.Location = new System.Drawing.Point(4, 49);
            this.ObjectInfo.Name = "ObjectInfo";
            this.ObjectInfo.Size = new System.Drawing.Size(246, 278);
            this.ObjectInfo.TabIndex = 5;
            this.ObjectInfo.Text = "Objects";
            this.ObjectInfo.UseVisualStyleBackColor = true;
            // 
            // ObjectInfoBox
            // 
            this.ObjectInfoBox.Location = new System.Drawing.Point(3, 182);
            this.ObjectInfoBox.Multiline = true;
            this.ObjectInfoBox.Name = "ObjectInfoBox";
            this.ObjectInfoBox.ReadOnly = true;
            this.ObjectInfoBox.Size = new System.Drawing.Size(239, 67);
            this.ObjectInfoBox.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(167, 259);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Z";
            // 
            // ObjectPosZ
            // 
            this.ObjectPosZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ObjectPosZ.DecimalPlaces = 2;
            this.ObjectPosZ.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.ObjectPosZ.Location = new System.Drawing.Point(187, 255);
            this.ObjectPosZ.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ObjectPosZ.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.ObjectPosZ.Name = "ObjectPosZ";
            this.ObjectPosZ.Size = new System.Drawing.Size(56, 20);
            this.ObjectPosZ.TabIndex = 5;
            this.ObjectPosZ.ValueChanged += new System.EventHandler(this.ObjectPosX_ValueChanged);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(85, 259);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Y";
            // 
            // ObjectPosY
            // 
            this.ObjectPosY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ObjectPosY.DecimalPlaces = 2;
            this.ObjectPosY.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.ObjectPosY.Location = new System.Drawing.Point(105, 255);
            this.ObjectPosY.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ObjectPosY.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.ObjectPosY.Name = "ObjectPosY";
            this.ObjectPosY.Size = new System.Drawing.Size(56, 20);
            this.ObjectPosY.TabIndex = 3;
            this.ObjectPosY.ValueChanged += new System.EventHandler(this.ObjectPosX_ValueChanged);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 259);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "X";
            // 
            // ObjectPosX
            // 
            this.ObjectPosX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ObjectPosX.DecimalPlaces = 3;
            this.ObjectPosX.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.ObjectPosX.Location = new System.Drawing.Point(23, 255);
            this.ObjectPosX.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ObjectPosX.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.ObjectPosX.Name = "ObjectPosX";
            this.ObjectPosX.Size = new System.Drawing.Size(56, 20);
            this.ObjectPosX.TabIndex = 1;
            this.ObjectPosX.ValueChanged += new System.EventHandler(this.ObjectPosX_ValueChanged);
            // 
            // ObjectList
            // 
            this.ObjectList.ContextMenuStrip = this.ObjectListMenuStrip;
            this.ObjectList.FormattingEnabled = true;
            this.ObjectList.Location = new System.Drawing.Point(3, 3);
            this.ObjectList.Name = "ObjectList";
            this.ObjectList.Size = new System.Drawing.Size(239, 173);
            this.ObjectList.TabIndex = 0;
            this.ObjectList.SelectedIndexChanged += new System.EventHandler(this.ObjectList_SelectedIndexChanged);
            // 
            // ObjectListMenuStrip
            // 
            this.ObjectListMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ObjectListNewMenu,
            this.deleteToolStripMenuItem,
            this.duplicateToolStripMenuItem});
            this.ObjectListMenuStrip.Name = "ObjectListMenuStrip";
            this.ObjectListMenuStrip.Size = new System.Drawing.Size(125, 70);
            // 
            // ObjectListNewMenu
            // 
            this.ObjectListNewMenu.Name = "ObjectListNewMenu";
            this.ObjectListNewMenu.Size = new System.Drawing.Size(124, 22);
            this.ObjectListNewMenu.Text = "New";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // duplicateToolStripMenuItem
            // 
            this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
            this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.duplicateToolStripMenuItem.Text = "Duplicate";
            // 
            // LightInfo
            // 
            this.LightInfo.Controls.Add(this.panel2);
            this.LightInfo.Controls.Add(this.LightList);
            this.LightInfo.Location = new System.Drawing.Point(4, 49);
            this.LightInfo.Name = "LightInfo";
            this.LightInfo.Size = new System.Drawing.Size(246, 278);
            this.LightInfo.TabIndex = 6;
            this.LightInfo.Text = "Lights";
            this.LightInfo.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.AmbientLabel);
            this.panel2.Controls.Add(this.MinRad);
            this.panel2.Controls.Add(this.AmbientLevel);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.LightIntensity);
            this.panel2.Location = new System.Drawing.Point(3, 210);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(243, 65);
            this.panel2.TabIndex = 7;
            // 
            // AmbientLabel
            // 
            this.AmbientLabel.AutoSize = true;
            this.AmbientLabel.Location = new System.Drawing.Point(4, 36);
            this.AmbientLabel.Name = "AmbientLabel";
            this.AmbientLabel.Size = new System.Drawing.Size(45, 13);
            this.AmbientLabel.TabIndex = 0;
            this.AmbientLabel.Text = "Ambient";
            // 
            // MinRad
            // 
            this.MinRad.DecimalPlaces = 2;
            this.MinRad.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.MinRad.Location = new System.Drawing.Point(174, 8);
            this.MinRad.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.MinRad.Name = "MinRad";
            this.MinRad.Size = new System.Drawing.Size(65, 20);
            this.MinRad.TabIndex = 6;
            this.MinRad.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.MinRad.ValueChanged += new System.EventHandler(this.MinRad_ValueChanged);
            // 
            // AmbientLevel
            // 
            this.AmbientLevel.DecimalPlaces = 4;
            this.AmbientLevel.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.AmbientLevel.Location = new System.Drawing.Point(55, 34);
            this.AmbientLevel.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AmbientLevel.Name = "AmbientLevel";
            this.AmbientLevel.Size = new System.Drawing.Size(65, 20);
            this.AmbientLevel.TabIndex = 1;
            this.AmbientLevel.ValueChanged += new System.EventHandler(this.AmbientLevel_ValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(125, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "MinRad";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(46, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Intensity";
            // 
            // LightIntensity
            // 
            this.LightIntensity.DecimalPlaces = 2;
            this.LightIntensity.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.LightIntensity.Location = new System.Drawing.Point(54, 8);
            this.LightIntensity.Name = "LightIntensity";
            this.LightIntensity.Size = new System.Drawing.Size(65, 20);
            this.LightIntensity.TabIndex = 4;
            this.LightIntensity.ValueChanged += new System.EventHandler(this.LightIntensity_ValueChanged);
            // 
            // LightList
            // 
            this.LightList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LightList.AutoArrange = false;
            this.LightList.ContextMenuStrip = this.LightListRMM;
            this.LightList.HideSelection = false;
            this.LightList.LabelWrap = false;
            this.LightList.Location = new System.Drawing.Point(0, 0);
            this.LightList.MultiSelect = false;
            this.LightList.Name = "LightList";
            this.LightList.Size = new System.Drawing.Size(246, 204);
            this.LightList.TabIndex = 2;
            this.LightList.UseCompatibleStateImageBehavior = false;
            this.LightList.View = System.Windows.Forms.View.List;
            this.LightList.SelectedIndexChanged += new System.EventHandler(this.LightList_SelectedIndexChanged);
            // 
            // LightListRMM
            // 
            this.LightListRMM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewLight,
            this.RemoveLight});
            this.LightListRMM.Name = "LightListRMM";
            this.LightListRMM.Size = new System.Drawing.Size(118, 48);
            // 
            // NewLight
            // 
            this.NewLight.Name = "NewLight";
            this.NewLight.Size = new System.Drawing.Size(117, 22);
            this.NewLight.Text = "New";
            this.NewLight.Click += new System.EventHandler(this.NewLight_Click);
            // 
            // RemoveLight
            // 
            this.RemoveLight.Name = "RemoveLight";
            this.RemoveLight.Size = new System.Drawing.Size(117, 22);
            this.RemoveLight.Text = "Remove";
            this.RemoveLight.Click += new System.EventHandler(this.RemoveLight_Click);
            // 
            // DefaultImages
            // 
            this.DefaultImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DefaultImages.ImageStream")));
            this.DefaultImages.TransparentColor = System.Drawing.Color.Transparent;
            this.DefaultImages.Images.SetKeyName(0, "Grid.png");
            this.DefaultImages.Images.SetKeyName(1, "Folder - Pictures.png");
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
            this.VertListRightMouseMenu.ResumeLayout(false);
            this.EdgeInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CellEdgeList)).EndInit();
            this.FaceInfo.ResumeLayout(false);
            this.FaceInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UShift)).EndInit();
            this.TextureInfo.ResumeLayout(false);
            this.TextureInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TexturePreview)).EndInit();
            this.ObjectInfo.ResumeLayout(false);
            this.ObjectInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectPosZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectPosX)).EndInit();
            this.ObjectListMenuStrip.ResumeLayout(false);
            this.LightInfo.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinRad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AmbientLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LightIntensity)).EndInit();
            this.LightListRMM.ResumeLayout(false);
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
        private System.Windows.Forms.Button SetCellToPreset;
        private System.Windows.Forms.Button MoveRoof;
        private System.Windows.Forms.Button MoveFloor;
        private System.Windows.Forms.Button EditVertButton;
        private System.Windows.Forms.CheckBox RoofVis;
        private System.Windows.Forms.CheckBox FloorViz;
        private System.Windows.Forms.TabPage TextureInfo;
        private System.Windows.Forms.TextBox PreviewInfo;
        private System.Windows.Forms.PictureBox TexturePreview;
        public System.Windows.Forms.ImageList DefaultImages;
        public System.Windows.Forms.TreeView TextureList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label ULabel;
        private System.Windows.Forms.NumericUpDown VShift;
        private System.Windows.Forms.NumericUpDown UShift;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox FaceMatInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown VScale;
        private System.Windows.Forms.NumericUpDown UScale;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage ObjectInfo;
        private System.Windows.Forms.ListBox ObjectList;
        private System.Windows.Forms.ContextMenuStrip ObjectListMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ObjectListNewMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown ObjectPosZ;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown ObjectPosY;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown ObjectPosX;
        private System.Windows.Forms.TextBox ObjectInfoBox;
        private System.Windows.Forms.TabPage LightInfo;
        private System.Windows.Forms.Label AmbientLabel;
        private System.Windows.Forms.NumericUpDown AmbientLevel;
        private System.Windows.Forms.ListView LightList;
        private System.Windows.Forms.ContextMenuStrip LightListRMM;
        private System.Windows.Forms.ToolStripMenuItem NewLight;
        private System.Windows.Forms.ToolStripMenuItem RemoveLight;
        private System.Windows.Forms.Button LightingModeButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem computeLightmapsToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown MinRad;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown LightIntensity;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel2;
    }
}

