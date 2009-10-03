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
using Drawables.DisplayLists;

namespace PortalEdit
{
    public partial class EditFrame : Form
    {
        Editor editor;
        MRU mru;

        public EditFrame()
        {
            InitializeComponent();

            MapRadioPanel.TagsAreValues = true;
            MapRadioPanel.SelectionChanged += new FormControls.ImageRadioPanel.SelectionChangedEvent(MapRadioPanel_SelectionChanged);

            ViewRadioPanel.TagsAreValues = true;
            ViewRadioPanel.SelectionChanged += new ImageRadioPanel.SelectionChangedEvent(ViewRadioPanel_SelectionChanged);

            Undo.System.UndoStateChanged += new UndoStateChangeEvent(System_UndoStateChanged);
            undoToolStripMenuItem.Enabled = Undo.System.UndoAvail();
            LoadSettings();
        }

        void ViewRadioPanel_SelectionChanged(object sender, ImageRadioPanel.SelectionChangedEventArgs e)
        {
            if (editor == null)
                return;
            editor.viewEditMode = (ViewEditMode)e.value;
            Invalidate(true);
        }

        void System_UndoStateChanged(object sender, bool available)
        {
            undoToolStripMenuItem.Enabled = available;
            undoToolStripMenuItem.Text = "Undo " + Undo.System.Description;
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
            Settings settings = Settings.settings;
            mru = new MRU(recentFilesToolStripMenuItem);
            mru.Clicked += new MRUItemClicked(mru_Clicked);

            if (settings.NormalLoc != Point.Empty)
            {
                StartPosition = FormStartPosition.Manual;
                Location = settings.NormalLoc;
                Size = settings.NormalSize;

                if (settings.Maximized)
                    WindowState = FormWindowState.Maximized;
            }

            MapRadioPanel.SelectedItem = DrawButton;
            ViewRadioPanel.SelectedItem = CellSelectButton;

            ViewCheckPanel.CheckButton(ShowCellEdges, settings.DrawCellEdges);
            ViewCheckPanel.CheckButton(ShowPortals, settings.DrawPortals);
            ViewCheckPanel.ItemCheckChanged += new ImageCheckPanel.ItemCheckChangedEvent(ViewCheckPanel_ItemCheckChanged);

            LoadEditorDepths();
       }

        void mru_Clicked(object sender, string file)
        {
            OpenFile(file);
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
            Invalidate(true);
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate(true);
            base.OnResize(e);
        }

        public void mapRenderer_MouseStatusUpdate(object sender, Vector2 position)
        {
            MousePositionStatus.Text = "Map:" + position.ToString() + "Offset: " + Editor.instance.mapRenderer.offset.ToString();
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Text = "New Map";
            CheckDirtySave();
            editor.New();
            Invalidate(true);
        }

        private void OpenFile ( String file )
        {
            CheckDirtySave();
            Text = "Portal Edit: " + Path.GetFileNameWithoutExtension(file);

            if (!editor.Open(new FileInfo(file)))
                MessageBox.Show("Error reading map file");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckDirtySave();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open File";
            ofd.Filter = "Portal Map (*.PortalMap)|*.PortalMap|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                OpenFile(ofd.FileName);
                mru.AddFile(ofd.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editor.FileName == string.Empty)
                saveAsToolStripMenuItem_Click(sender, e);
            else
            {
                if (!editor.Save(new FileInfo(editor.FileName)))
                    MessageBox.Show("Error saving map file");
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save File";
            sfd.Filter = "Portal Map (*.PortalMap)|*.PortalMap";
            if (editor.FileName != string.Empty)
            {
                sfd.FileName = Path.GetFileName(editor.FileName);
                sfd.InitialDirectory = Path.GetDirectoryName(editor.FileName);
            }
            else
                sfd.FileName = "New Map.PortalMap";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Text = "Portal Edit: " + Path.GetFileNameWithoutExtension(sfd.FileName);
                if (!editor.Save(new FileInfo(sfd.FileName)))
                    MessageBox.Show("Error saving map file");
                else
                    mru.AddFile(sfd.FileName);
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

            foreach(CellGroup group in editor.map.CellGroups)
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

            PopulateCellInfoList();
        }

        public void PopulateCellInfoList()
        {
            CellVertList.Rows.Clear();
            CellInfoZIsInc.Enabled = false;

            Cell cell = editor.GetSelectedCell();
            if (cell == null)
                return;

            CellInfoZIsInc.Enabled = true;
            CellInfoZIsInc.Checked = cell.HeightIsIncremental;

            for (int i = 0; i < cell.Verts.Count; i++)
            {
                CellVert vert = cell.Verts[i];
                List<String> items = new List<String>();
                items.Add(i.ToString());
                items.Add(vert.Bottom.Z.ToString());
                items.Add(vert.Top.ToString());

                CellVertList.Rows.Add(items.ToArray());
            }
        }

        private void CheckDirtySave ()
        {
            if (editor.IsDirty())
            {
                if (MessageBox.Show("This file has unsaved changed!\r\nDo you wish to save it now?", "Unsaved Changes", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    saveToolStripMenuItem_Click(this, EventArgs.Empty);
            }
        }

        private void EditFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckDirtySave();

            Settings settings = Settings.settings;

            if (this.WindowState == FormWindowState.Maximized)
            {
                settings.NormalLoc = RestoreBounds.Location;
                settings.NormalSize = RestoreBounds.Size;
                settings.Maximized = true;
            }
            else
            {
                settings.Maximized = false;
                settings.NormalSize = Size;
                settings.NormalLoc = Location;
            }

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
            editor.SelectMapItem(null);
            Invalidate(true);
        }

        private void MapZoomIn_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
                editor.mapRenderer.Zoom(Settings.settings.MapZoomTicksPerClick*5);
            else
                editor.mapRenderer.Zoom(Settings.settings.MapZoomTicksPerClick);
        }

        private void MapZoomOut_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
                editor.mapRenderer.Zoom(-Settings.settings.MapZoomTicksPerClick*5);
            else
                editor.mapRenderer.Zoom(-Settings.settings.MapZoomTicksPerClick);
        }

        private void ResetZoom_Click(object sender, EventArgs e)
        {
            if (editor != null)
                editor.mapRenderer.ResetZoom();
        }

        private void MapTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (MapTree.SelectedNode != null)
                editor.SelectMapItem(MapTree.SelectedNode.Tag);
            else
                editor.SelectMapItem(null);

            PopulateCellInfoList();
            Invalidate(true);
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
            Settings.settings.Write();
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

            Settings.settings.Write();
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

            Settings.settings.Write();
        }

        private void EditIncZ_CheckedChanged(object sender, EventArgs e)
        {
            Editor.EditZInc = EditIncZ.Checked;
            Settings.settings.Write();
        }

        private void CellVertList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (editor != null)
                editor.EditVert();
        }

        private void CellVertList_SelectionChanged(object sender, EventArgs e)
        {
            Invalidate(true);
        }

        private void CellInfoZIsInc_CheckedChanged(object sender, EventArgs e)
        {
            if (editor == null)
                return;

            editor.SetCellInZ(CellInfoZIsInc.Checked);
            RebuildAll();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo.System.Apply();
            populateCellList();
            RebuildAll();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new SettingsDialog(Settings.settings).ShowDialog(this) == DialogResult.OK)
            {
                Settings.settings.Write();
                DisplayListSystem.system.Invalidate();
                Invalidate(true);
            }
        }

        private void setupImageUnderlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MapImageSetup mis = new MapImageSetup();
            if (mis.ShowDialog(this) == DialogResult.OK)
            {
                editor.mapRenderer.CheckUnderlay();
                editor.viewRenderer.CheckUnderlay();
                Invalidate(true);
            }
        }
    }
}
