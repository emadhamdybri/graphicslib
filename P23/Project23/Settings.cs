using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Project23
{
    public class Settings
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        public static Settings settings = new Settings();

        [System.Xml.Serialization.XmlIgnoreAttribute]
        public FileInfo fileLoc = null;

        public LoginForm.Gender gender = LoginForm.Gender.None;
        public int AvatarIndex = -1;
        public string UserName = "Pilot";

        public KeyManager Keys = new KeyManager();

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
            catch (System.Exception /*ex*/)
            {
                stream.Close();
                file.Delete();
                settings.Keys.SetDefaults();
                s = settings;
            }
            s.fileLoc = file;

            s.Keys.Deserialize();
            return s;
        }

        public bool Write()
        {
            XmlSerializer XML = new XmlSerializer(typeof(Settings));

            Keys.Serialize();
            fileLoc.Delete();
            FileStream stream = fileLoc.OpenWrite();
            XML.Serialize(stream, this);
            stream.Close();
            return fileLoc.Exists;
        }
    }
}
