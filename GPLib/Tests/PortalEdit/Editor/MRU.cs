using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.IO;

namespace PortalEdit
{
    public delegate void MRUItemClicked ( object sender, String file );

    public class MRU
    {
        public event MRUItemClicked Clicked;

        protected ToolStripMenuItem MRUMenu;
 
        public MRU (ToolStripMenuItem menu)
        {
            MRUMenu = menu;
            BuildMenu();
        }

        public void AddFile ( String file )
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

             foreach (String file in Settings.settings.RecentFiles)
             {
                 ToolStripItem item = MRUMenu.DropDownItems.Add(Path.GetFileNameWithoutExtension(file));
                 item.Tag = file;
                 item.Click += new EventHandler(MRU_Click);
             }
        }

        void MRU_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;

            String file = (string)item.Tag;

            if (Clicked != null)
                Clicked(sender, (string)file);

            Settings.settings.RecentFiles.Remove(file);
            Settings.settings.RecentFiles.Insert(0, file);
            Settings.settings.Write();
            BuildMenu();
        }
    }
}
