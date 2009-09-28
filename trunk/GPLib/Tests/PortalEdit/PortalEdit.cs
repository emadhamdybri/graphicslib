using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using OpenTK;
using OpenTK.Graphics;
using FormControls;

namespace PortalEdit
{
    public partial class EditFrame : Form
    {
        Editor editor;

        public EditFrame()
        {
            InitializeComponent();

            MapRadioPanel.TagsAreValues = true;
            MapRadioPanel.SelectionChanged += new FormControls.ImageRadioPanel.SelectionChangedEvent(MapRadioPanel_SelectionChanged);
            LoadSettings();
        }

        void MapRadioPanel_SelectionChanged(object sender, FormControls.ImageRadioPanel.SelectionChangedEventArgs e)
        {
            if (editor == null)
                return;
            editor.mapRenderer.EditMode = (MapEditMode)e.value;
            Invalidate(true);
        }

        protected void LoadSettings()
        {
            DirectoryInfo AppSettingsDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"GPLib"));
            if (!AppSettingsDir.Exists)
                AppSettingsDir.Create();

            DirectoryInfo appDir = AppSettingsDir.CreateSubdirectory("PortalEdit");

            FileInfo prefsFile = new FileInfo(Path.Combine(appDir.FullName, "prefs.xml"));
            if (prefsFile.Exists)
                Settings.settings = Settings.Read(prefsFile);
            else
                Settings.settings.fileLoc = prefsFile;

            Settings.settings.Write();

            SetupSettings();
        }

        protected void SetupSettings ()
        {
            MapRadioPanel.SelectedItem = DrawButton;

            ViewCheckPanel.CheckButton(ShowCellEdges, Settings.settings.DrawCellEdges);
            ViewCheckPanel.CheckButton(ShowPortals, Settings.settings.DrawPortals);
            ViewCheckPanel.ItemCheckChanged += new ImageCheckPanel.ItemCheckChangedEvent(ViewCheckPanel_ItemCheckChanged);

            LoadEditorDepths();

       }

        protected void LoadEditorDepths ( )
        {
            EditIncZ.Checked = Editor.EditZInc;
            EditZMinus.Text = Editor.EditZFloor.ToString();
            EditZPlus.Text = Editor.EditZRoof.ToString();
        }

        protected void ViewCheckPanel_ItemCheckChanged(object sender, ImageCheckPanel.ItemCheckChangedEventArgs e)
        {
            if (e.button == ShowCellEdges)
                Settings.settings.DrawCellEdges = e.value;
            if (e.button == ShowPortals)
                Settings.settings.DrawPortals = e.value;
            RebuildAll();
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate(true);
            base.OnResize(e);
        }

        public void mapRenderer_MouseStatusUpdate(object sender, System.Drawing.Point position)
        {
            MousePositionStatus.Text = "Map:" + position.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Editor.instance == null)
                Editor.instance = new Editor(this, MapView, GLView);

            editor = Editor.instance;
            populateCellList();

            base.OnLoad(e);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            editor.mapRenderer.ClearEditPolygon();
            Invalidate(true);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open File";
            ofd.Filter = "Portal Map (*.PortalMap)|*.PortalMap|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!editor.Open(new FileInfo(ofd.FileName)))
                    MessageBox.Show("Error reading map file");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save File";
            sfd.Filter = "Portal Map (*.PortalMap)|*.PortalMap";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (!editor.Save(new FileInfo(sfd.FileName)))
                    MessageBox.Show("Error saving map file");
            }
        }

        public void populateCellList ()
        {
            object selected = null;
            if (MapTree.SelectedNode != null)
                selected = MapTree.SelectedNode.Tag;

            MapTree.Nodes.Clear();

            if (editor == null)
                return;

            TreeNode selectedNode = null;

            foreach(CellGroup group in editor.map.cellGroups)
            {
                TreeNode node = new TreeNode(group.Name, 0, 2);
                node.Tag = group;
                MapTree.Nodes.Add(node);
                if (group == selected)
                    selectedNode = node;
                foreach ( Cell cell in group.Cells )
                {
                    TreeNode child = new TreeNode(cell.Name, 1, 3);
                    child.Tag = cell;
                    node.Nodes.Add(child);
                    if (cell == selected)
                        selectedNode = child;
                }
            }

            MapTree.ExpandAll();
            MapTree.SelectedNode = selectedNode;
        }

        private void EditFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.settings.Write();
        }

        private void RebuildAll()
        {
            Drawables.DisplayLists.DisplayListSystem.system.Invalidate();
            Settings.settings.Write();
            Invalidate(true);
        }

        private void DeleteCell_Click(object sender, EventArgs e)
        {
            EditorCell cell = editor.GetSelectedCell();
            MapTree.SelectedNode = null;
            editor.DeleteCell(cell);
        }

        private void Deslect_Click(object sender, EventArgs e)
        {
            MapTree.SelectedNode = null;
            RebuildAll();
        }

        private void MapZoomIn_Click(object sender, EventArgs e)
        {
            editor.mapRenderer.Zoom(Settings.settings.MapZoomTicksPerClick);
        }

        private void MapZoomOut_Click(object sender, EventArgs e)
        {
            editor.mapRenderer.Zoom(-Settings.settings.MapZoomTicksPerClick);
        }

        private void MapTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RebuildAll();
        }

        private void NewGroup_Click(object sender, EventArgs e)
        {
            editor.NewGroup();
        }

        private void EditLoadZFromSelection_Click(object sender, EventArgs e)
        {
            if (editor == null)
                return;

            Cell cell = editor.GetSelectedCell();
            if (cell == null)
                return;

            Editor.EditZInc = cell.HeightIsIncremental;
            Editor.EditZFloor = cell.Verts[0].Bottom.Z;
            Editor.EditZRoof = cell.Verts[0].Top;

            LoadEditorDepths();
            RebuildAll();
        }

        private void EditZMinus_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float.TryParse(EditZMinus.Text,out Editor.EditZFloor);
            }
            catch (System.Exception ex)
            {
                EditZMinus.Text = Editor.EditZFloor.ToString();
            }

            RebuildAll();
        }

        private void EditZPlus_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float.TryParse(EditZPlus.Text, out Editor.EditZRoof);
            }
            catch (System.Exception ex)
            {
                EditZPlus.Text = Editor.EditZRoof.ToString();
            }

            RebuildAll();
        }

        private void EditIncZ_CheckedChanged(object sender, EventArgs e)
        {
            Editor.EditZInc = EditIncZ.Checked;
            RebuildAll();
        }
    }
}
