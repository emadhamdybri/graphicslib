﻿/*
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
using System.Drawing;
using OpenTK;

using Drawables.Models;
using Drawables.Materials;

namespace Drawables.StaticModels.OBJ
{
    public class OBJFile
    {
        static string[] splitOnDelim(string data, string delim, int count)
        {
            return data.Split(delim.ToCharArray(), count);
        }

        static string[] splitOnDelim(string data, string delim)
        {
            return data.Split(delim.ToCharArray());
        }

        static Vector3 readV3D(string data)
        {
            Vector3 v = new Vector3();
            string[] n = splitOnDelim(data, " ", 3);
            if (n.Length > 2)
            {
                v.X = float.Parse(n[0]);
                v.Y = float.Parse(n[1]);
                v.Z = float.Parse(n[2]);
            }
            return v;
        }

        static GLColor readColor(string data)
        {
            string[] n = splitOnDelim(data, " ", 5);
            if (n.Length > 2)
            {
                float R = float.Parse(n[0]);
                float G = float.Parse(n[1]);
                float B = float.Parse(n[2]);
                float A = 1.0f;
                if (n.Length > 3)
                    A = float.Parse(n[3]);

                return new GLColor(R, G, B, A);
            }
            return GLColor.White;
        }

        static Vector2 readV2D(string data)
        {
            Vector2 v = new Vector2();
            string[] n = splitOnDelim(data, " ", 2);
            if (n.Length > 1)
            {
                v.X = float.Parse(n[0]);
                v.Y = 1.0f - float.Parse(n[1]);
            }
            return v;
        }

        static List<FaceVert> readFaces(string data)
        {
            List<FaceVert> faces = new List<FaceVert>();
            string[] nubs = splitOnDelim(data, " ");
            foreach (string n in nubs)
            {
                if (n.Contains("#"))
                    break;

                FaceVert f = new FaceVert();
                string[] i = splitOnDelim(n, "/", 3);
                if (i.Length > 0 && i[0] != string.Empty)
                    f.vert = int.Parse(i[0]);
                if (i.Length > 1 && i[1] != string.Empty)
                    f.uv = int.Parse(i[1]);
                if (i.Length > 2 && i[2] != string.Empty)
                    f.normal = int.Parse(i[2]);

                faces.Add(f);
            }

            return faces;
        }

        static bool readMaterialFile(FileInfo file, StaticModel model)
        {
            if (!file.Exists)
                return false;

            FileStream fs = file.OpenRead();
            if (fs == null)
                return false;

            StreamReader sr = new StreamReader(fs);

            Material currentMat = null;
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] nubs = splitOnDelim(line, " ",2);
                if (nubs.Length >1)
                {
                    string code = nubs[0].TrimStart();
                    if (code == "newmtl")
                    {
                        if (currentMat != null)
                            model.addMaterial(currentMat);
                        currentMat = new Material();
                        currentMat.name = nubs[1];
                    }
                    else if (currentMat != null)
                    {
                        if (code == "map_Kd")
                            currentMat.textureName = Path.Combine(Path.GetDirectoryName(file.FullName),nubs[1]);
                        else if (code == "Kd")
                            currentMat.baseColor = readColor(nubs[1]);
                        else if (code == "Ka")
                            currentMat.ambinent = readColor(nubs[1]);
                        else if (code == "Ks")
                            currentMat.specular = readColor(nubs[1]);
                        else if (code == "Ke")
                            currentMat.emmision = readColor(nubs[1]);
                        else if (code == "Ns")
                            currentMat.shine = float.Parse(nubs[1]);
                    }
                }
            }
            if (currentMat != null)
                model.addMaterial(currentMat);

            return true;
        }

        static Vector3 getIndex(int index, List<Vector3> list)
        {
            if (index <= 0 && index + list.Count >= 0)
                return list[list.Count - 1 + index];

            if (index-1 > list.Count)
                return list[list.Count - 1];

            return list[index-1];
        }

        static Vector2 getIndex(int index, List<Vector2> list)
        {
            if (index <= 0 && index + list.Count >= 0)
                return list[list.Count - 1 + index];

            if (index - 1 > list.Count)
                return list[list.Count - 1];

            return list[index - 1];
        }

        public static StaticModel Read(string file)
        {
            return Read(new FileInfo(file));
        }

        public static StaticModel Read(FileInfo file)
        {
            StaticModel model = new StaticModel();

            model.name = Path.GetFileNameWithoutExtension(file.FullName);

            FileStream fs = file.OpenRead();
            StreamReader sr = new StreamReader(fs);

            // save off the verts, as they may span groups
            // we'll add em to each submesh as the faces get added
            // this way each submesh only has the verts for it's faces
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> norms = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();

            Mesh currentMesh = null;

            string currentObjectName = string.Empty;
            string currentGroupName = string.Empty;
            string currentMapName = string.Empty;

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] nubs = splitOnDelim(line, " ", 2);
                if (nubs.Length > 0)
                {
                    if (nubs.Length > 1)
                    {
                        string code = nubs[0];
                        if (code == "o")
                            currentObjectName = nubs[1];
                        else if (code == "mtllib")
                            readMaterialFile(new FileInfo(Path.Combine(Path.GetDirectoryName(file.FullName), nubs[1])), model);
                        else if (code == "usemtl")
                            currentMesh = model.getMeshForMaterial(nubs[1]);
                        else if (code == "usemap")
                            currentMapName = nubs[1];
                        else if (code == "g")
                            currentGroupName = nubs[1];
                        else if (code == "v")
                            verts.Add(readV3D(nubs[1]));
                        else if (code == "vn")
                            norms.Add(readV3D(nubs[1]));
                        else if (code == "vt")
                            uvs.Add(readV2D(nubs[1]));
                        else if (code == "f")
                        {
                            if (currentMesh == null) // this means that the geometry has no material, so make it white
                                currentMesh = model.getMesh(new Material());

                            Face face = new Face();

                            face.verts = readFaces(nubs[1]);
                            foreach (FaceVert f in face.verts)
                            {
                                f.vert = currentMesh.addVert(getIndex(f.vert, verts));
                                f.uv = currentMesh.addUV(getIndex(f.uv, uvs));
                                f.normal = currentMesh.addNormal(getIndex(f.normal, norms));
                            }
                            currentMesh.addFace(currentGroupName, face);
                        }
                    }
                }
            }

            sr.Close();
            fs.Close();
            return model;
        }
    }
}
