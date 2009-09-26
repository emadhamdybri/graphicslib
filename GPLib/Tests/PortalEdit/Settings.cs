using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PortalEdit
{
    public class Settings
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public static Settings settings = new Settings();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public FileInfo fileLoc;

        public bool DrawCellEdges = true;
        public bool DrawPortals = true;

        public static Settings Read ( FileInfo file )
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

        public bool Write ( )
        {
            XmlSerializer XML = new XmlSerializer(typeof(Settings));

            FileStream stream = fileLoc.OpenWrite();
            XML.Serialize(stream, this);
            stream.Close();
            return fileLoc.Exists;
        }
    }
}
