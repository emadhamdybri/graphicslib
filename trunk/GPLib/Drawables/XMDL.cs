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
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;

using Drawables.StaticModels;
using Drawables.Materials;
using Utilities.Paths;

namespace Drawables.Models.XMDL
{
    public class XMDLFile
    {
        public StaticModel read(FileInfo file)
        {
            StaticModel model = new StaticModel();

            XmlSerializer xml = new XmlSerializer(typeof(StaticModel));
            FileStream fs = file.OpenRead();
            GZipStream zip = new GZipStream(fs, CompressionMode.Decompress, false);
            StreamReader sr = new StreamReader(zip);

            model = (StaticModel)xml.Deserialize(sr);
            sr.Close();
            zip.Close();
            fs.Close();

            // setup the new model to draw
            model.Invalidate();

            return model;
        }

        public bool write(FileInfo file, StaticModel model)
        {
            if (model == null || !model.valid())
                return false;

            XmlSerializer xml = new XmlSerializer(typeof(StaticModel));
            FileStream fs = file.OpenWrite();
            GZipStream zip = new GZipStream(fs, CompressionMode.Compress, true);
            StreamWriter sr = new StreamWriter(zip);
            xml.Serialize(sr, model);
            sr.Close();
            zip.Close();
            fs.Close();

            return true;
        }
    }
}
