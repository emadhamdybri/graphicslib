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
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;

namespace World
{
    public class WorldFileExtras
    {
        public string name;
        public string data;
    }

    public class OctreeWorldFile
    {
        public ObjectWorld world = null;
        public List<WorldFileExtras> extras = new List<WorldFileExtras>();

        public string findSection ( string name )
        {
            if (extras == null)
                return string.Empty;

            foreach( WorldFileExtras e in extras)
            {
                if (e.name == name)
                    return e.data;
            }

            return string.Empty;
        }

        public static bool read ( out OctreeWorldFile worldFile, FileInfo file)
        {
            return read(out worldFile,file,true);
        }

        public static bool read ( out OctreeWorldFile worldFile, FileInfo file, bool compress )
        {
            worldFile = new OctreeWorldFile();

            if (!file.Exists)
                return false;
           
            FileStream fs = file.OpenRead();
            if (fs == null)
                return false;

            GZipStream zip = new GZipStream(fs, CompressionMode.Decompress, false);
            StreamReader sr = new StreamReader(zip);
            if (!compress)
                sr = new StreamReader(fs);

            XmlSerializer xml = new XmlSerializer(typeof(OctreeWorldFile));
            worldFile = (OctreeWorldFile)xml.Deserialize(sr);
            sr.Close();
            zip.Close();
            fs.Close();

            return true;
        }

        public static bool write(OctreeWorldFile worldFile, FileInfo file)
        {
            return write(worldFile, file, true);
        }

        public static bool write(OctreeWorldFile worldFile, FileInfo file, bool compress)
        {
            FileStream fs = file.OpenWrite();
            if (fs == null)
                return false;
            GZipStream zip = new GZipStream(fs, CompressionMode.Compress, false);
            StreamWriter sw = new StreamWriter(zip);
            if (!compress)
                sw = new StreamWriter(fs);

            XmlSerializer xml = new XmlSerializer(typeof(OctreeWorldFile));
            xml.Serialize(sw, worldFile);
            sw.Close();
            zip.Close();
            fs.Close();

            return true;
        }
    }
}
