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
            if (Settings.settings.DrawCellEdges)
                ShowCellBorders.CheckState = CheckState.Checked;
            else
                HideCellBorders.CheckState = CheckState.Checked;

            if (Settings.settings.DrawPortals)
                ShowPortals.CheckState = CheckState.Checked;
            else
                HidePortals.CheckState = CheckState.Checked;

            MapRadioPanel.SelectedItem = DrawButton;
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
            object selected = CellList.SelectedItem;

            CellList.Items.Clear();

            foreach (Cell cell in editor.map.cells)
                CellList.Items.Add(cell);

            CellList.SelectedItem = selected;
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

        private void ShowCellBorders_Click(object sender, EventArgs e)
        {
            Settings.settings.DrawCellEdges = true;
            ShowCellBorders.CheckState = CheckState.Checked;
            HideCellBorders.CheckState = CheckState.Unchecked;
            RebuildAll();
        }

        private void HideCellBorders_Click(object sender, EventArgs e)
        {
            Settings.settings.DrawCellEdges = false;
            HideCellBorders.CheckState = CheckState.Checked;
            ShowCellBorders.CheckState = CheckState.Unchecked;
            RebuildAll();
        }

        private void ShowPortals_Click(object sender, EventArgs e)
        {
            Settings.settings.DrawPortals = true;
            ShowPortals.CheckState = CheckState.Checked;
            HidePortals.CheckState = CheckState.Unchecked;
            RebuildAll();
        }

        private void HidePortals_Click(object sender, EventArgs e)
        {
            Settings.settings.DrawPortals = false;
            HidePortals.CheckState = CheckState.Checked;
            ShowPortals.CheckState = CheckState.Unchecked;
            RebuildAll();
        }

        private void DeleteCell_Click(object sender, EventArgs e)
        {
            EditorCell cell = editor.GetSelectedCell();
            CellList.SelectedItem = null;
            editor.DeleteCell(cell);

        }

        private void CellList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RebuildAll();
        }

        private void Deslect_Click(object sender, EventArgs e)
        {
            CellList.SelectedItem = null;
            RebuildAll();
        }
    }
}
