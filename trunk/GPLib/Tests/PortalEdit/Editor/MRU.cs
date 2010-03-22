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
using System.Windows.Forms;
using System.Text;
using System.IO;

namespace PortalEdit
{
    public delegate void MRUItemClicked ( object sender, string file );

    public class MRU
    {
        public event MRUItemClicked Clicked;

        protected ToolStripMenuItem MRUMenu;
 
        public MRU (ToolStripMenuItem menu)
        {
            MRUMenu = menu;
            BuildMenu();
        }

        public void AddFile ( string file )
        {
            if (Settings.settings.RecentFiles.Contains(file))
                Settings.settings.RecentFiles.Remove(file);

            Settings.settings.RecentFiles.Insert(0, file);
            Settings.settings.Write();
            BuildMenu();
        }

        protected void BuildMenu ()
        {
             MRUMenu.DropDownItems.Clear();

             if (Settings.settings.RecentFiles.Count > Settings.settings.RecentFilesLimit)
                 Settings.settings.RecentFiles.RemoveRange(Settings.settings.RecentFilesLimit, Settings.settings.RecentFiles.Count - Settings.settings.RecentFilesLimit);

             foreach (string file in Settings.settings.RecentFiles)
             {
                 ToolStripItem item = MRUMenu.DropDownItems.Add(Path.GetFileNameWithoutExtension(file));
                 item.Tag = file;
                 item.Click += new EventHandler(MRU_Click);
             }
        }

        void MRU_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;

            string file = (string)item.Tag;

            if (Clicked != null)
                Clicked(sender, (string)file);

            Settings.settings.RecentFiles.Remove(file);
            Settings.settings.RecentFiles.Insert(0, file);
            Settings.settings.Write();
            BuildMenu();
        }
    }
}
