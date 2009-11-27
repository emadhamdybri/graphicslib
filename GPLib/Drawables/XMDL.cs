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
