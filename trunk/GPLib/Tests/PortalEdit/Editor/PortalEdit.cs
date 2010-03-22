/*
    Open Combat/Projekt 2501
    Copyright (C) 2010  Jeffery Allen Myers

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
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
using Drawables.Textures;
using Math3D;
using World;

namespace PortalEdit
{
    public partial class EditFrame : Form
    {
        Editor editor;
        MRU mru;

        bool loadingUI = false;

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
            ViewCheckPanel.CheckButton(ShowUnderlay, true);
            ViewCheckPanel.ItemCheckChanged += new ImageCheckPanel.ItemCheckChangedEvent(ViewCheckPanel_ItemCheckChanged);

            LoadTextures();

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
            if (e.button == ShowUnderlay)
                editor.viewRenderer.ShowUnderlay = e.value;

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
            editor.SelectionChanged += new EditorSelectonChanged(editor_SelectionChanged);
            editor.MapLoaded += new MapLoadedHandler(editor_MapLoaded);
            editor_MapLoaded(null, EventArgs.Empty,null);

            Objects.RegisterDefaults();
            SetupObjectNewMenu();

            populateCellList();

            base.OnLoad(e);
        }

        void SetupObjectNewMenu ()
        {
            ObjectListNewMenu.DropDownItems.Clear();
            foreach (KeyValuePair<string,MapObjectHandler> handler in Objects.Handlers)
            {
                ToolStripItem item = ObjectListNewMenu.DropDownItems.Add(handler.Key);
                item.Tag = handler.Value;
                item.Click += new EventHandler(ObjectListNewMenuItemClick);
            }
        }

        void ObjectListNewMenuItemClick(object sender, EventArgs e)
        {
            MapObjectHandler handler = (MapObjectHandler)((ToolStripItem)sender).Tag;

            editor.NewObject(handler.New());
        }

        FileInfo LocateTexture ( string file )
        {
            return Resources.File(file);
        }

        void editor_MapLoaded(object sender, EventArgs args, PortalWorld map)
        {
            TextureSystem.system.LocateFile = new TextureSystem.LocateFileHandler(LocateTexture);
            NamedDepthPresets.Items.Clear();
            NamedDepthPresets.Items.Add("None");

            PortalMapAttribute[] att = editor.map.MapAttributes.Find("Editor:NamedDepthSet");
            foreach ( PortalMapAttribute at in att )
            {
                string[] nugs = at.Value.Split(":".ToCharArray());
                if (nugs.Length < 3)
                    continue;

                NamedDepthPresets.Items.Add(nugs[0]);
            }

            NamedDepthPresets.Items.Add("New...");
            NamedDepthPresets.SelectedIndex = 0;

            HideGeo.Checked = EditorCell.HideGeo;
            HideBelowZ.Value = (decimal)EditorCell.HideGeoUnder;
            HideAboveZ.Value = (decimal)EditorCell.HideGeoOver;

            HideGeo_CheckedChanged(this, EventArgs.Empty);

            LoadObjectList();
            LoadLightList();
        }

        public static PortalMapAttribute FindDepthAttribute ( string name )
        {
            PortalMapAttribute[] att = Editor.instance.map.MapAttributes.Find("Editor:NamedDepthSet");
            foreach (PortalMapAttribute at in att)
            {
                string[] nugs = at.Value.Split(":".ToCharArray());
                if (nugs.Length < 3)
                    continue;

                if (nugs[0] == name)
                    return at;
            }

            return null;
        }

        void SaveDepthAttribute ( string name )
        {
            PortalMapAttribute att = FindDepthAttribute(name);
            if (att != null)
                editor.map.MapAttributes.Remove(att.Name, att.Value);

            string val = name + ":" + EditZMinus.Text + ":" + EditZPlus.Text + ":" + EditIncZ.Checked.ToString();
            editor.map.MapAttributes.Add("Editor:NamedDepthSet", val);
            Editor.SetDirty();
        }

        void SaveCurrentDepthAttribute ( )
        {
            if (NamedDepthPresets.SelectedItem == null)
                return;
            if (NamedDepthPresets.SelectedItem.ToString() != "None")
                SaveDepthAttribute(NamedDepthPresets.SelectedItem.ToString());
        }

        private void NamedDepthPresets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NamedDepthPresets.SelectedItem == null)
                return;
            if (NamedDepthPresets.SelectedItem.ToString() == "New...")
            {
                string name = "New Item" + NamedDepthPresets.Items.Count.ToString();

                RenameItem nameDlog = new RenameItem();
                nameDlog.Text = "Set Name";
                nameDlog.ItemName.Text = name;
                if (nameDlog.ShowDialog(this) == DialogResult.OK)
                    name = nameDlog.ItemName.Text;
                else
                {
                    NamedDepthPresets.SelectedIndex = 0;
                    return;
                }
                SaveDepthAttribute(name);

                NamedDepthPresets.Items.Insert(1, name);
                NamedDepthPresets.SelectedIndex = 1;
            }
            if (NamedDepthPresets.SelectedItem.ToString() == "None")
            {
            }
            else
            {
                PortalMapAttribute att = FindDepthAttribute(NamedDepthPresets.SelectedItem.ToString());
                if (att != null)
                {
                    string[] nugs = att.Value.Split(":".ToCharArray());
                    if (nugs.Length < 3)
                        return;

                    if (nugs[0] == NamedDepthPresets.SelectedItem.ToString())
                    {
                        EditZMinus.Text = nugs[1];
                        EditZPlus.Text = nugs[2];
                        EditIncZ.Checked = nugs[3] == "True";
                    }
                }
            }
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

        private void OpenFile ( string file )
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
            loadingUI = true;

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
                node.ContextMenuStrip = GroupRightMouseMenu;
                MapTree.Nodes.Add(node);
                if (group == selected)
                    selectedNode = node;
                foreach ( Cell cell in group.Cells )
                {
                    TreeNode child = new TreeNode(cell.ID.CellName, 1, 3);
                    child.Tag = cell;
                    child.ContextMenuStrip = CellRightMouseMenu;
                    node.Nodes.Add(child);
                    if (cell == selected)
                        selectedNode = child;
                }
            }

            MapTree.ExpandAll();
            MapTree.SelectedNode = selectedNode;

            PopulateCellInfoList();
            loadingUI = false;
        }

        public void PopulateVertInfoList (EditorCell cell)
        {
            if (cell == null)
                return;

            Plane floorPlane = cell.GetFloorPlane();
            Plane roofPlane = cell.GetRoofPlane();
           
            CellInfoZIsInc.Enabled = true;
            CellInfoZIsInc.Checked = cell.HeightIsIncremental;

            double testTolerance = 0.0001;

            for (int i = 0; i < cell.Verts.Count; i++)
            {
                CellVert vert = cell.Verts[i];
                List<string> items = new List<string>();
                items.Add(i.ToString());
                items.Add(vert.Bottom.Z.ToString());

                if (Math.Abs(Cell.GetZInPlane(floorPlane, vert.Bottom.X, vert.Bottom.Y) - vert.Bottom.Z) < testTolerance)
                    items.Add("True");
                else
                    items.Add("False");
     
                items.Add(vert.Top.ToString());

                if (Math.Abs(Cell.GetZInPlane(roofPlane, vert.Bottom.X, vert.Bottom.Y) - cell.RoofZ(i)) < testTolerance)
                    items.Add("True");
                else
                    items.Add("False");

                CellVertList.Rows.Add(items.ToArray());
            }
        }

        protected void PopulateEdgeInfoList ( EditorCell cell )
        {
            for (int i = 0; i < cell.Edges.Count; i++)
            {
               CellEdge edge = cell.Edges[i];

                List<string> items = new List<string>();
                items.Add(i.ToString());
                if (edge.EdgeType == CellEdgeType.Portal)
                    items.Add("Portal");
                else if (edge.EdgeType == CellEdgeType.Wall)
                    items.Add("Wall");
                else
                    items.Add("Unknown");

                items.Add(edge.Vizable.ToString());
                CellEdgeList.Rows.Add(items.ToArray());
            }
        }

        public void PopulateCellInfoList()
        {
            CellVertList.Rows.Clear();
            CellGroupDropdown.Items.Clear();
            CellEdgeList.Rows.Clear();
            CellInfoZIsInc.Enabled = false;

            EditorCell cell = editor.GetSelectedCell();
            if (cell == null)
                return;

            PopulateVertInfoList(cell);
            PopulateEdgeInfoList(cell);

            FloorViz.Checked = cell.FloorVizable;
            RoofVis.Checked = cell.RoofVizable;
         
            int thisGroup = -1;

            foreach (CellGroup group in editor.map.CellGroups)
            {
                int item = CellGroupDropdown.Items.Add(group.Name);
                if (cell.ID.GroupName == group.Name)
                    thisGroup = item;
            }
            CellGroupDropdown.SelectedIndex = thisGroup;
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

            loadingUI = true;
            PopulateCellInfoList();
            loadingUI = false;

            Invalidate(true);
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
            catch (System.Exception /*ex*/)
            {
                EditZMinus.Text = Editor.EditZFloor.ToString();
            }
            SaveCurrentDepthAttribute();
            Settings.settings.Write();
        }

        private void EditZPlus_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float.TryParse(EditZPlus.Text, out Editor.EditZRoof);
            }
            catch (System.Exception /*ex*/)
            {
                EditZPlus.Text = Editor.EditZRoof.ToString();
            }

            SaveCurrentDepthAttribute();
            Settings.settings.Write();
        }

        private void EditIncZ_CheckedChanged(object sender, EventArgs e)
        {
            Editor.EditZInc = EditIncZ.Checked;
            SaveCurrentDepthAttribute();
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
                LoadTextures();
                DisplayListSystem.system.Invalidate();
                Invalidate(true);
            }
        }

        private void setupImageUnderlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MapImageSetup.UP)
                return;
            new MapImageSetup().Show(this);
        }

        private void renameDepthGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NamedDepthPresets.SelectedItem != null && NamedDepthPresets.SelectedItem.ToString() != "New..." && NamedDepthPresets.SelectedItem.ToString() != "None")
            {
                RenameItem dlg = new RenameItem();
                dlg.ItemName.Text = NamedDepthPresets.SelectedItem.ToString();
                if (dlg.ShowDialog(this)== DialogResult.OK)
                {
                    PortalMapAttribute attribute = FindDepthAttribute(NamedDepthPresets.SelectedItem.ToString());
                    editor.map.MapAttributes.Remove(attribute.Name, attribute.Value);
                    SaveDepthAttribute(dlg.ItemName.Text);
                    NamedDepthPresets.SelectedItem = dlg.ItemName.Text;
                }

                NamedDepthPresets.Items.Remove(NamedDepthPresets.SelectedItem);
                NamedDepthPresets.SelectedIndex = 0;
            }
        }

        private void deleteDepthGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NamedDepthPresets.SelectedItem != null && NamedDepthPresets.SelectedItem.ToString() != "New..." && NamedDepthPresets.SelectedItem.ToString() != "None")
            {
                NamedDepthPresets.Items.Remove(NamedDepthPresets.SelectedItem);
                NamedDepthPresets.SelectedIndex = 0;
            }
        }

        private void CellGroupDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CellGroupDropdown.SelectedItem != null)
                editor.SetCellGroup(CellGroupDropdown.SelectedItem.ToString());

            Invalidate(true);
        }

        private void RenameGroup ( CellGroup group )
        {
            if (group == null)
                return;

            RenameItem dlg = new RenameItem();
            dlg.ItemName.Text = string.Copy(group.Name);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                if (editor.map.FindGroup(dlg.ItemName.Text) != null)
                {
                    MessageBox.Show("Group name \"" + dlg.ItemName.Text + "\" already exists");
                    return;
                }
                Undo.System.Add(new GroupRenameUndo(group.Name, dlg.ItemName.Text));
                editor.RenameGroup(group.Name, dlg.ItemName.Text);
                populateCellList();
            }
        }

        private void renameGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CellGroup group = editor.GetSelectedGroup();
            if (group == null)
            {
                EditorCell cell = editor.GetSelectedCell();
                if (cell != null)
                    group = cell.Group;
            }
            RenameGroup(group);
        }

        private void RenameCell ( EditorCell cell )
        {
            RenameItem dlg = new RenameItem();
            dlg.ItemName.Text = string.Copy(cell.ID.CellName);
            if (dlg.ShowDialog(this)== DialogResult.OK)
            {
                if (cell.Group.FindCell(dlg.ItemName.Text) != null)
                {
                    MessageBox.Show("Cell name \"" + dlg.ItemName.Text + "\" already exists in this group");
                    return;
                }
                Undo.System.Add(new CellRenameUndo(cell,dlg.ItemName.Text));
                editor.RenameCell(cell, dlg.ItemName.Text);
                populateCellList();
            }
        }

        private void renameCellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditorCell cell = editor.GetSelectedCell();
            if (cell != null)
                return;

           RenameCell(cell);
        }

        private void MapTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Tag.GetType() == typeof(CellGroup))
                    GroupRightMouseMenu.Tag = e.Node.Tag;
                else if (e.Node.Tag.GetType() == typeof(EditorCell))
                    CellRightMouseMenu.Tag = e.Node.Tag;
            }
        }

        private void RenameGroupRMM_Click(object sender, EventArgs e)
        {
            RenameGroup((CellGroup)GroupRightMouseMenu.Tag);
        }

        private void RenameCellRMM_Click(object sender, EventArgs e)
        {
            RenameCell((EditorCell)CellRightMouseMenu.Tag);
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            editor.NewGroup();
        }

        private void deselectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MapTree.SelectedNode = null;
            editor.SelectMapItem(null);
            Invalidate(true);
        }

        private void DeleteCell_Click(object sender, EventArgs e)
        {
            EditorCell cell = editor.GetSelectedCell();
            MapTree.SelectedNode = null;
            editor.DeleteCell(cell);
        }

        private void HideGeo_CheckedChanged(object sender, EventArgs e)
        {
            HideBelowZ.Enabled = HideGeo.Checked;
            HideAboveZ.Enabled = HideGeo.Checked;
            EditorCell.HideGeo = HideGeo.Checked;
            RebuildAll();
        }

        private void HideBelowZ_ValueChanged(object sender, EventArgs e)
        {
            EditorCell.HideGeoUnder = (float)HideBelowZ.Value;
            EditorCell.HideGeoOver = (float)HideAboveZ.Value;
            RebuildAll();
        }

        private void zToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.SetRoofVertToPlane();
        }

        private void zToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            editor.SetFloorVertToPlane();
        }

        private void MoveFloor_Click(object sender, EventArgs e)
        {
            EditorCell cell = editor.GetSelectedCell();
            if (cell == null)
                return;

            CellZSSet dlg = new CellZSSet();

            dlg.ZValue.Value = (decimal)cell.Verts[0].Bottom.Z;
            dlg.Roof = false;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                editor.SetCellFloor((float)dlg.ZValue.Value, !dlg.ForceDepth.Checked);
                Invalidate(true);
            }
        }

        private void MoveRoof_Click(object sender, EventArgs e)
        {
            EditorCell cell = editor.GetSelectedCell();
            if (cell == null)
                return;

            CellZSSet dlg = new CellZSSet();

            dlg.ZValue.Value = (decimal)cell.RoofZ(0);
            dlg.Roof = true;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                editor.SetCellRoof((float)dlg.ZValue.Value, !dlg.ForceDepth.Checked);
                Invalidate(true);
            }
        }

        private void SetCellToPreset_Click(object sender, EventArgs e)
        {
            EditorCell cell = editor.GetSelectedCell();
            if (cell == null)
                return;

            CellZSSet dlg = new CellZSSet();
            dlg.PresetOnly = true;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                PortalMapAttribute item = FindDepthAttribute(dlg.SelectedPreset);
                if (item != null)
                {
                    string[] nugs = item.Value.Split(":".ToCharArray());

                    float floor = float.Parse(nugs[1]);
                    float roof = float.Parse(nugs[2]);
                    bool incZ = nugs[3] == "True";

                    editor.SetCellToPreset(floor, roof, incZ, cell);
                    Invalidate(true);
                }
            }
        }

        private void CellEdgeList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (editor == null || loadingUI)
                return;

            EditorCell cell = editor.GetSelectedCell();
            if (cell == null || e.RowIndex < 0)
                return;

            int edge = int.Parse(CellEdgeList.Rows[e.RowIndex].Cells[0].Value.ToString());
            bool viz = CellEdgeList.Rows[e.RowIndex].Cells[2].Value.ToString() == "True";
            editor.SetCellEdgeViz(cell, edge, viz);
            Invalidate(true);
        }

        private void FloorViz_CheckedChanged(object sender, EventArgs e)
        {
            if (editor == null || loadingUI)
                return;

            EditorCell cell = editor.GetSelectedCell();
            if (cell == null)
                return;

            editor.SetCellFloorViz(cell, FloorViz.Checked);
            Invalidate(true);
        }

        private void RoofVis_CheckedChanged(object sender, EventArgs e)
        {
            if (editor == null || loadingUI)
                return;

            EditorCell cell = editor.GetSelectedCell();
            if (cell == null)
                return;

            editor.SetCellRoofViz(cell, RoofVis.Checked);
            Invalidate(true);
        }

        bool ValidImageExtension ( string path )
        {
            string ext = Path.GetExtension(path).ToLower();
            if (ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp" || ext == ".tiff")
                return true;

            return false;
        }

        protected TreeNode NodeForPath ( string path )
        {
            if (!ValidImageExtension(path))
                return null;

            String[] nugs = path.Split(Path.DirectorySeparatorChar);

            // find the root node
            TreeNode theNode = null;
            foreach ( TreeNode node in TextureList.Nodes )
            {
                if (node.Text == nugs[0])
                {
                    theNode = node;
                    break;
                }
            }

            if (theNode == null)
            {
                theNode = new TreeNode(nugs[0]);
                TextureList.Nodes.Add(theNode);
            }

            for (int i = 1; i < nugs.Length; i++)
            {
                // see if a node exists and we arn't at the end
                TreeNode newNode = null;
                foreach (TreeNode node in theNode.Nodes)
                {
                    if (node.Text == nugs[i])
                    {
                        newNode = node;
                        break;
                    }
                }

                if (newNode == null)
                {
                    newNode = new TreeNode(nugs[i]);
                    theNode.Nodes.Add(newNode);
                }
                theNode = newNode;
            }

            theNode.Tag = path;
            return theNode;
        }

        public void LoadTextures ()
        {
            // set the resource dirs into the texture manager
            TextureList.Nodes.Clear();
            List<String> images = Resources.Files("Textures", true);
            foreach (string image in images)
                NodeForPath(image);

            TextureList.ExpandAll();
        }

        private void TextureList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
            {
                TexturePreview.Image = DefaultImages.Images[1];
                PreviewInfo.Text = String.Empty;
                return;
            }

            FileInfo file = Resources.File(e.Node.Tag.ToString());

            TexturePreview.Image = Image.FromFile(Resources.File(e.Node.Tag.ToString()).FullName);
            PreviewInfo.Text = e.Node.Tag.ToString() + "\r\n" + TexturePreview.Image.Size.ToString();
        }

        private void UShift_ValueChanged(object sender, EventArgs e)
        {
            if (loadingUI)
                return;

            Editor.SetDirty();
            CellMaterialInfo info = editor.GetSelectedMaterialInfo();
            if (info == null)
                return;


            if (UScale.Value != 0)
                info.UVScale.X = 1f / (float)UScale.Value;
            else
                info.UVScale.X = 0;
    
            if (VScale.Value != 0)
                info.UVScale.Y = 1f / (float)VScale.Value;
            else
                info.UVScale.Y = 0;


            if (UShift.Value != 0)
                info.UVShift.X = 1f / (float)UShift.Value;
            else
                info.UVShift.X = 0;

            if (VShift.Value != 0)
                info.UVShift.Y = 1f / (float)VShift.Value;
            else
                info.UVShift.Y = 0;

            editor.GetSelectedCell().GenerateDisplayGeometry();
            RebuildAll();
        }

        void editor_SelectionChanged(object sender, EventArgs args)
        {
            loadingUI = true;
            UScale.Value = 1;
            VScale.Value = 1;

            UShift.Value = 0;
            VShift.Value = 0;

            FaceMatInfo.Text = String.Empty;

            CellMaterialInfo info = editor.GetSelectedMaterialInfo();
            if (info == null)
                return;

            if (info.UVScale.X != 0)
                UScale.Value = (decimal)(1f / info.UVScale.X);
            if (info.UVScale.X != 0)
                VScale.Value = (decimal)(1f / info.UVScale.Y);

            if (info.UVShift.X != 0)
                UShift.Value = (decimal) (1f / info.UVShift.X);

            if (info.UVShift.Y != 0)
                VShift.Value = (decimal)(1f / info.UVShift.Y);

            EditorCell cell = editor.GetSelectedCell();
            if (cell != null )
            {
                if (editor.GetFloorSelection())
                {

                }
                else if (editor.GetRoofSelection())
                {

                }
                else
                {
                    CellEdge edge = editor.GetSelectedEdge();
                    CellWallGeometry wall = editor.GetSelectedWallGeo();

                    if (wall != null && edge != null)
                    {
                        float dist = cell.EdgeDistance(edge);
                        float spZ = wall.UpperZ[0] - wall.LowerZ[0];
                        float epZ = wall.UpperZ[1] - wall.LowerZ[1];

                        FaceMatInfo.Text = "Face Width(U) = " + dist.ToString() + "\r\n";
                        FaceMatInfo.Text += "Face SP Height(V) = " + spZ.ToString() + "\r\n";
                        FaceMatInfo.Text += "Face EP Height(V) = " + epZ.ToString() + "\r\n";
                    }
                }
            }
            loadingUI = false;
        }

        public void LoadObjectList ()
        {
            loadingUI = true;
            ObjectList.Items.Clear();

            foreach(ObjectInstance obj in editor.map.MapObjects)
                ObjectList.Items.Add(obj);

            loadingUI = false;

        }

        private void ObjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadingUI = true;
            ObjectInfoBox.Text = string.Empty;

            ObjectPosX.Value = 0;
            ObjectPosY.Value = 0;
            ObjectPosZ.Value = 0;

            ObjectInstance inst = (ObjectInstance)ObjectList.SelectedItem;
            if (inst != null)
            {
                foreach (CellID ID in inst.cells)
                    ObjectInfoBox.Text += ID.GroupName + " " + ID.CellName;


                ObjectPosX.Value = (decimal)inst.Postion.X;
                ObjectPosY.Value = (decimal)inst.Postion.Y;
                ObjectPosZ.Value = (decimal)inst.Postion.Z;
            }
            loadingUI = false;
        }

        private void ObjectPosX_ValueChanged(object sender, EventArgs e)
        {
            if (loadingUI)
                return;

            ObjectInstance inst = (ObjectInstance)ObjectList.SelectedItem;
            if (inst != null)
            {
                editor.MoveObject(inst, new Vector3((float)ObjectPosX.Value,(float)ObjectPosY.Value,(float)ObjectPosZ.Value));
                ObjectList_SelectedIndexChanged(this,EventArgs.Empty);
                Invalidate(true);
            }
        }
        
        public void LoadLightList()
        {
            LightList.Items.Clear();
            if (editor == null)
                return;

            foreach (LightInstance light in editor.map.Lights)
                LightList.Items.Add(light.ToString()).Tag = light;

            loadingUI = true;
            AmbientLevel.Value = (decimal)editor.map.AmbientLight;
            loadingUI = false;
        }

        private void NewLight_Click(object sender, EventArgs e)
        {
            editor.NewLight();
        }

        private void RemoveLight_Click(object sender, EventArgs e)
        {

        }

        private void AmbientLevel_ValueChanged(object sender, EventArgs e)
        {
            if (loadingUI)
                return;

            Editor.SetDirty();
            editor.map.AmbientLight = (float)AmbientLevel.Value;
        }

        private void computeLightmapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.ComputeLightmaps();
        }

        private void LightList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(loadingUI)
                return;

            loadingUI = true;
            MinRad.Enabled = false;
            LightIntensity.Enabled = false;

            if (LightList.SelectedItems.Count > 0)
            {
                LightInstance light = (LightInstance)LightList.SelectedItems[0].Tag;
                if (light != null)
                {
                    MinRad.Enabled = true;
                    LightIntensity.Enabled = true;

                    MinRad.Value = (Decimal)light.MinRadius;
                    LightIntensity.Value = (Decimal)light.Inensity;
                }
            }

            loadingUI = false;
        }

        private void MinRad_ValueChanged(object sender, EventArgs e)
        {
            if (loadingUI)
                return;

            if (LightList.SelectedItems.Count > 0)
            {
                LightInstance light = (LightInstance)LightList.SelectedItems[0].Tag;
                if (light != null)
                {
                    Editor.SetDirty();
                    light.MinRadius = (float)MinRad.Value;
                }
            }
        }

        private void LightIntensity_ValueChanged(object sender, EventArgs e)
        {
            if (loadingUI)
                return;

            if (LightList.SelectedItems.Count > 0)
            {
                LightInstance light = (LightInstance)LightList.SelectedItems[0].Tag;
                if (light != null)
                {
                    Editor.SetDirty();
                    light.Inensity = (float)LightIntensity.Value;

                    Invalidate(true);
                }
            }
        }
    }
}
