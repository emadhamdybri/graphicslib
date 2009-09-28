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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.DepthEditPanel = new System.Windows.Forms.Panel();
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
            this.MapZoomOut = new System.Windows.Forms.Button();
            this.MapZoomIn = new System.Windows.Forms.Button();
            this.MapView = new System.Windows.Forms.PictureBox();
            this.ViewCheckPanel = new FormControls.ImageCheckPanel();
            this.ShowPortals = new System.Windows.Forms.Button();
            this.ShowCellEdges = new System.Windows.Forms.Button();
            this.ViewRadioPanel = new FormControls.ImageRadioPanel();
            this.CellFillMode = new System.Windows.Forms.Button();
            this.CellSelectButton = new System.Windows.Forms.Button();
            this.GLView = new OpenTK.GLControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.MousePositionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.DeleteCell = new System.Windows.Forms.Button();
            this.Deslect = new System.Windows.Forms.Button();
            this.MapTree = new System.Windows.Forms.TreeView();
            this.MapTreeIcons = new System.Windows.Forms.ImageList(this.components);
            this.NewGroup = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CellInfoZIsInc = new System.Windows.Forms.CheckBox();
            this.CellVertList = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CellListPanel = new System.Windows.Forms.Panel();
            this.RightSideOuterPanel = new System.Windows.Forms.Panel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.DepthEditPanel.SuspendLayout();
            this.MapEditToolsPanel.SuspendLayout();
            this.MapRadioPanel.SuspendLayout();
            this.MapZoomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MapView)).BeginInit();
            this.ViewCheckPanel.SuspendLayout();
            this.ViewRadioPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CellVertList)).BeginInit();
            this.CellListPanel.SuspendLayout();
            this.RightSideOuterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(9, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.DepthEditPanel);
            this.splitContainer1.Panel1.Controls.Add(this.MapEditToolsPanel);
            this.splitContainer1.Panel1.Controls.Add(this.MapView);
            this.splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ViewCheckPanel);
            this.splitContainer1.Panel2.Controls.Add(this.ViewRadioPanel);
            this.splitContainer1.Panel2.Controls.Add(this.GLView);
            this.splitContainer1.Panel2MinSize = 120;
            this.splitContainer1.Size = new System.Drawing.Size(788, 475);
            this.splitContainer1.SplitterDistance = 398;
            this.splitContainer1.TabIndex = 0;
            // 
            // DepthEditPanel
            // 
            this.DepthEditPanel.Controls.Add(this.EditLoadZFromSelection);
            this.DepthEditPanel.Controls.Add(this.EditIncZ);
            this.DepthEditPanel.Controls.Add(this.EditZPlus);
            this.DepthEditPanel.Controls.Add(this.label1);
            this.DepthEditPanel.Controls.Add(this.EditZMinus);
            this.DepthEditPanel.Controls.Add(this.ZMinusLabel);
            this.DepthEditPanel.Location = new System.Drawing.Point(6, 38);
            this.DepthEditPanel.Name = "DepthEditPanel";
            this.DepthEditPanel.Size = new System.Drawing.Size(216, 33);
            this.DepthEditPanel.TabIndex = 5;
            // 
            // EditLoadZFromSelection
            // 
            this.EditLoadZFromSelection.Image = global::PortalEdit.Properties.Resources.eyedropper_icon;
            this.EditLoadZFromSelection.Location = new System.Drawing.Point(181, 2);
            this.EditLoadZFromSelection.Name = "EditLoadZFromSelection";
            this.EditLoadZFromSelection.Size = new System.Drawing.Size(29, 28);
            this.EditLoadZFromSelection.TabIndex = 6;
            this.EditLoadZFromSelection.UseVisualStyleBackColor = true;
            this.EditLoadZFromSelection.Click += new System.EventHandler(this.EditLoadZFromSelection_Click);
            // 
            // EditIncZ
            // 
            this.EditIncZ.AutoSize = true;
            this.EditIncZ.Location = new System.Drawing.Point(121, 8);
            this.EditIncZ.Name = "EditIncZ";
            this.EditIncZ.Size = new System.Drawing.Size(57, 17);
            this.EditIncZ.TabIndex = 4;
            this.EditIncZ.Text = "Inc Z+";
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
            this.MapEditToolsPanel.Size = new System.Drawing.Size(390, 29);
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
            this.DrawButton.Image = global::PortalEdit.Properties.Resources.pencil;
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
            this.MapZoomPanel.Controls.Add(this.MapZoomOut);
            this.MapZoomPanel.Controls.Add(this.MapZoomIn);
            this.MapZoomPanel.Location = new System.Drawing.Point(331, 0);
            this.MapZoomPanel.Name = "MapZoomPanel";
            this.MapZoomPanel.Size = new System.Drawing.Size(59, 25);
            this.MapZoomPanel.TabIndex = 4;
            // 
            // MapZoomOut
            // 
            this.MapZoomOut.Image = global::PortalEdit.Properties.Resources.zoom_out;
            this.MapZoomOut.Location = new System.Drawing.Point(31, 0);
            this.MapZoomOut.Name = "MapZoomOut";
            this.MapZoomOut.Size = new System.Drawing.Size(25, 23);
            this.MapZoomOut.TabIndex = 1;
            this.MapZoomOut.UseVisualStyleBackColor = true;
            this.MapZoomOut.Click += new System.EventHandler(this.MapZoomOut_Click);
            // 
            // MapZoomIn
            // 
            this.MapZoomIn.Image = global::PortalEdit.Properties.Resources.zoom_in;
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
            this.MapView.Location = new System.Drawing.Point(3, 77);
            this.MapView.Name = "MapView";
            this.MapView.Size = new System.Drawing.Size(390, 393);
            this.MapView.TabIndex = 0;
            this.MapView.TabStop = false;
            // 
            // ViewCheckPanel
            // 
            this.ViewCheckPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ViewCheckPanel.Controls.Add(this.ShowPortals);
            this.ViewCheckPanel.Controls.Add(this.ShowCellEdges);
            this.ViewCheckPanel.HighlightBGColor = System.Drawing.Color.CadetBlue;
            this.ViewCheckPanel.HighlightColor = System.Drawing.Color.BlueViolet;
            this.ViewCheckPanel.Location = new System.Drawing.Point(3, 3);
            this.ViewCheckPanel.Name = "ViewCheckPanel";
            this.ViewCheckPanel.SelectedItem = null;
            this.ViewCheckPanel.Size = new System.Drawing.Size(378, 37);
            this.ViewCheckPanel.TabIndex = 0;
            // 
            // ShowPortals
            // 
            this.ShowPortals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowPortals.Image = global::PortalEdit.Properties.Resources.PortalShowHideButton;
            this.ShowPortals.Location = new System.Drawing.Point(37, 0);
            this.ShowPortals.Name = "ShowPortals";
            this.ShowPortals.Size = new System.Drawing.Size(36, 36);
            this.ShowPortals.TabIndex = 1;
            this.ShowPortals.UseVisualStyleBackColor = true;
            // 
            // ShowCellEdges
            // 
            this.ShowCellEdges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ShowCellEdges.Image = global::PortalEdit.Properties.Resources.EdgeShowHideButton;
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
            this.ViewRadioPanel.Size = new System.Drawing.Size(332, 26);
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
            this.CellFillMode.Tag = "Fill Mode";
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
            this.CellSelectButton.Tag = PortalEdit.MapEditMode.SelectMode;
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
            this.GLView.Size = new System.Drawing.Size(378, 393);
            this.GLView.TabIndex = 0;
            this.GLView.VSync = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1005, 24);
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
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MousePositionStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 505);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1005, 22);
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
            this.DeleteCell.Location = new System.Drawing.Point(1, 32);
            this.DeleteCell.Name = "DeleteCell";
            this.DeleteCell.Size = new System.Drawing.Size(64, 23);
            this.DeleteCell.TabIndex = 5;
            this.DeleteCell.Text = "Delete";
            this.DeleteCell.UseVisualStyleBackColor = true;
            this.DeleteCell.Click += new System.EventHandler(this.DeleteCell_Click);
            // 
            // Deslect
            // 
            this.Deslect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Deslect.Location = new System.Drawing.Point(1, 3);
            this.Deslect.Name = "Deslect";
            this.Deslect.Size = new System.Drawing.Size(64, 23);
            this.Deslect.TabIndex = 6;
            this.Deslect.Text = "Deselect";
            this.Deslect.UseVisualStyleBackColor = true;
            this.Deslect.Click += new System.EventHandler(this.Deslect_Click);
            // 
            // MapTree
            // 
            this.MapTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MapTree.FullRowSelect = true;
            this.MapTree.HideSelection = false;
            this.MapTree.ImageIndex = 0;
            this.MapTree.ImageList = this.MapTreeIcons;
            this.MapTree.Location = new System.Drawing.Point(68, 3);
            this.MapTree.Name = "MapTree";
            this.MapTree.SelectedImageIndex = 0;
            this.MapTree.Size = new System.Drawing.Size(122, 116);
            this.MapTree.TabIndex = 7;
            this.MapTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.MapTree_AfterSelect);
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
            // NewGroup
            // 
            this.NewGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NewGroup.Location = new System.Drawing.Point(1, 61);
            this.NewGroup.Name = "NewGroup";
            this.NewGroup.Size = new System.Drawing.Size(64, 23);
            this.NewGroup.TabIndex = 8;
            this.NewGroup.Text = "New Group";
            this.NewGroup.UseVisualStyleBackColor = true;
            this.NewGroup.Click += new System.EventHandler(this.NewGroup_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.CellInfoZIsInc);
            this.groupBox1.Controls.Add(this.CellVertList);
            this.groupBox1.Location = new System.Drawing.Point(5, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 320);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cell Info";
            // 
            // CellInfoZIsInc
            // 
            this.CellInfoZIsInc.AutoSize = true;
            this.CellInfoZIsInc.Location = new System.Drawing.Point(6, 172);
            this.CellInfoZIsInc.Name = "CellInfoZIsInc";
            this.CellInfoZIsInc.Size = new System.Drawing.Size(139, 17);
            this.CellInfoZIsInc.TabIndex = 1;
            this.CellInfoZIsInc.Text = "Heights Are Incremental";
            this.CellInfoZIsInc.UseVisualStyleBackColor = true;
            // 
            // CellVertList
            // 
            this.CellVertList.AllowUserToAddRows = false;
            this.CellVertList.AllowUserToDeleteRows = false;
            this.CellVertList.AllowUserToResizeRows = false;
            this.CellVertList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CellVertList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.CellVertList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CellVertList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.ZMin,
            this.ZMax});
            this.CellVertList.Location = new System.Drawing.Point(6, 19);
            this.CellVertList.MultiSelect = false;
            this.CellVertList.Name = "CellVertList";
            this.CellVertList.RowHeadersWidth = 4;
            this.CellVertList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CellVertList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CellVertList.Size = new System.Drawing.Size(182, 147);
            this.CellVertList.TabIndex = 0;
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
            // ZMax
            // 
            this.ZMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ZMax.HeaderText = "Z+";
            this.ZMax.Name = "ZMax";
            this.ZMax.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CellListPanel
            // 
            this.CellListPanel.Controls.Add(this.MapTree);
            this.CellListPanel.Controls.Add(this.Deslect);
            this.CellListPanel.Controls.Add(this.NewGroup);
            this.CellListPanel.Controls.Add(this.DeleteCell);
            this.CellListPanel.Location = new System.Drawing.Point(5, 3);
            this.CellListPanel.Name = "CellListPanel";
            this.CellListPanel.Size = new System.Drawing.Size(198, 123);
            this.CellListPanel.TabIndex = 10;
            // 
            // RightSideOuterPanel
            // 
            this.RightSideOuterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RightSideOuterPanel.Controls.Add(this.CellListPanel);
            this.RightSideOuterPanel.Controls.Add(this.groupBox1);
            this.RightSideOuterPanel.Location = new System.Drawing.Point(799, 27);
            this.RightSideOuterPanel.Name = "RightSideOuterPanel";
            this.RightSideOuterPanel.Size = new System.Drawing.Size(206, 456);
            this.RightSideOuterPanel.TabIndex = 11;
            // 
            // EditFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 527);
            this.Controls.Add(this.RightSideOuterPanel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(614, 450);
            this.Name = "EditFrame";
            this.Text = "Portal Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditFrame_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CellVertList)).EndInit();
            this.CellListPanel.ResumeLayout(false);
            this.RightSideOuterPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox MapView;
        private OpenTK.GLControl GLView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel MousePositionStatus;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button DeleteCell;
        private System.Windows.Forms.Button Deslect;
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
        private System.Windows.Forms.Button NewGroup;
        public System.Windows.Forms.TreeView MapTree;
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel CellListPanel;
        private System.Windows.Forms.Panel RightSideOuterPanel;
        private System.Windows.Forms.DataGridView CellVertList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZMax;
        private System.Windows.Forms.CheckBox CellInfoZIsInc;
    }
}

