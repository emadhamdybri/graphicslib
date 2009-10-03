using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace PortalEdit
{
    public partial class SettingsDialog : Form
    {
        Settings settings;
        public SettingsDialog( Settings _settings )
        {
            settings = _settings;
            InitializeComponent();

            MapZoomPerClick.Value = settings.MapZoomTicksPerClick;
            PixelsPerUnit.Value = settings.PixelsPerUnit;
            GridSubUnits.Value = (decimal)settings.GridSubDivisions;
            SnapValue.Value = (decimal)settings.SnapValue;
            GridSize.Value = (decimal)settings.GridSize;
            MinimalSelection.Checked = settings.ShowLowestSelection;
            Show3DOrigin.Checked = settings.Show3dOrigin;
            
            UnderlayHasDepth.Checked = settings.ShowUnderlayWithDepth;
            UnderlayAlpha.Value = (decimal)settings.Underlay3DAlpha;
           
            UndoLevels.Value = (decimal)settings.UndoLevels;
       }

        private void OK_Click(object sender, EventArgs e)
        {
            settings.MapZoomTicksPerClick = (int)MapZoomPerClick.Value;
            settings.PixelsPerUnit = (int)PixelsPerUnit.Value;
            settings.GridSubDivisions = (float)GridSubUnits.Value;
            settings.SnapValue = (float)SnapValue.Value;
            settings.GridSize = (float)GridSize.Value;
            settings.ShowLowestSelection = MinimalSelection.Checked;
            settings.Show3dOrigin = Show3DOrigin.Checked;

            settings.ShowUnderlayWithDepth = UnderlayHasDepth.Checked;
            settings.Underlay3DAlpha = (float)UnderlayAlpha.Value;

            settings.UndoLevels = (int)UndoLevels.Value;
            Undo.System.CullUndos();

            settings.Write();
        }
    }

    public class Settings
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public static Settings settings = new Settings();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public FileInfo fileLoc;

        public bool DrawCellEdges = true;
        public bool DrawPortals = true;
        public int MapZoomTicksPerClick = 1;
        public float GridSubDivisions = 0.1f;
        public int PixelsPerUnit = 100;
        public float SnapValue = 0.1f;
        public float GridSize = 100f;
        public bool ShowLowestSelection = true;
        public bool Show3dOrigin = true;
        public int UndoLevels = 25;

        public bool ShowUnderlayWithDepth = true;
        public float Underlay3DAlpha = 1.0f;

        public Point NormalLoc = Point.Empty;
        public Size NormalSize = Size.Empty;
        public bool Maximized = false;

        public int RecentFilesLimit = 10;
        public List<String> RecentFiles = new List<string>();

        public static Settings Read(FileInfo file)
        {
            XmlSerializer XML = new XmlSerializer(typeof(Settings));
            FileStream stream = file.OpenRead();
            Settings s;
            try
            {
                s = (Settings)XML.Deserialize(stream);
                stream.Close();
            }
            catch (System.Exception ex)
            {
                stream.Close();
                file.Delete();
                s = settings;
            }
            s.fileLoc = file;
            return s;
        }

        public bool Write()
        {
            XmlSerializer XML = new XmlSerializer(typeof(Settings));

            fileLoc.Delete();
            FileStream stream = fileLoc.OpenWrite();
            XML.Serialize(stream, this);
            stream.Close();
            return fileLoc.Exists;
        }
    }
}
