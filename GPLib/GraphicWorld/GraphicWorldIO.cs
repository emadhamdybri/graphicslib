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
using System.Xml;
using System.Xml.Serialization;

using World;
using Drawables.Materials;
using Drawables.StaticModels;

namespace GraphicWorlds
{
    public class GraphicWorldIO
    {
        public static bool read(GraphicWorld world, FileInfo file)
        {
            if (!file.Exists)
                return false;

            OctreeWorldFile worldFile;
            if (!OctreeWorldFile.read(out worldFile, file, false))
                return false;
            return read(world,worldFile);
        }

        public static bool read(GraphicWorld world, OctreeWorldFile file)
        {
            world.world = file.world;

            string meshes = file.findSection("meshes");

            if (meshes != string.Empty)
            {
                FileInfo meshTemp = new FileInfo(Path.GetTempFileName());
                FileStream fs = meshTemp.OpenWrite();
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(meshes);
                sw.Close();
                fs.Close();

                fs = meshTemp.OpenRead();
                StreamReader sr = new StreamReader(fs);
                XmlSerializer xml = new XmlSerializer(typeof(List<StaticModel>));
                List<StaticModel> models = (List<StaticModel>)xml.Deserialize(sr);
                sr.Close();
                fs.Close();

                world.models = new Dictionary<string, StaticModel>();
                foreach (StaticModel m in models)
                    world.models[m.name] = m;
            }

            string materials = file.findSection("materials");

            if (materials != string.Empty)
            {
                FileInfo matTemp = new FileInfo(Path.GetTempFileName());
                FileStream fs = matTemp.OpenWrite();
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(materials);
                sw.Close();
                fs.Close();

                fs = matTemp.OpenRead();
                StreamReader sr = new StreamReader(fs);
                XmlSerializer xml = new XmlSerializer(typeof(List<Material>));
                List<Material> mats = (List<Material>)xml.Deserialize(sr);
                sr.Close();
                fs.Close();

                world.materials = new Dictionary<string, Material>();
                foreach (Material m in mats)
                    world.materials[m.name] = m;
            }

            return true;
        }

        public static bool write(GraphicWorld world, FileInfo file)
        {
            OctreeWorldFile worldFile = new OctreeWorldFile();
            if (!write(world, worldFile))
                return false;

            return OctreeWorldFile.write(worldFile, file, false);
        }

        public static bool write(GraphicWorld world, OctreeWorldFile file)
        {
            file.world = world.world;

            if (world.models != null && world.models.Count > 0)
            {
                FileInfo meshTemp = new FileInfo(Path.GetTempFileName());
                FileStream fs = meshTemp.OpenWrite();
                StreamWriter sw = new StreamWriter(fs);
                XmlSerializer xml = new XmlSerializer(typeof(List<StaticModel>));
                List<StaticModel> models = new List<StaticModel>();

                foreach (KeyValuePair<string, StaticModel> m in world.models)
                    models.Add(m.Value);

                xml.Serialize(sw,models);
                sw.Close();
                fs.Close();

                WorldFileExtras extra = new WorldFileExtras();
                extra.name = "meshes";

                fs = meshTemp.OpenRead();
                StreamReader sr = new StreamReader(fs);
                extra.data = sr.ReadToEnd();
                sr.Close();
                fs.Close();

                file.extras.Add(extra);
            }

            if (world.materials != null && world.materials.Count > 0)
            {
                FileInfo matTemp = new FileInfo(Path.GetTempFileName());
                FileStream fs = matTemp.OpenWrite();
                StreamWriter sw = new StreamWriter(fs);
                XmlSerializer xml = new XmlSerializer(typeof(List<Material>));
                List<Material> mats = new List<Material>();

                foreach (KeyValuePair<string, Material> m in world.materials)
                    mats.Add(m.Value);

                xml.Serialize(sw, mats);
                sw.Close();
                fs.Close();

                WorldFileExtras extra = new WorldFileExtras();
                extra.name = "materials";

                fs = matTemp.OpenRead();
                StreamReader sr = new StreamReader(fs);
                extra.data = sr.ReadToEnd();
                sr.Close();
                fs.Close();

                file.extras.Add(extra);
            }

            return true;
        }
    }
}
