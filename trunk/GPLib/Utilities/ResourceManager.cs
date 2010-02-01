using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utilities.Paths
{
    public class ResourceManager
    {
        public static List<string> SearchPaths = new List<string>();

        public static void AddPath ( string path )
        {
            SearchPaths.Add(path);
        }

        public static string FindFile(string name)
        {
            foreach (string searchpath in SearchPaths)
            {
                FileInfo file = new FileInfo(Path.Combine(searchpath, name));
                if (file.Exists)
                    return file.FullName;
            }

            return string.Empty;
        }

        public static List<string> FindFiles(string path)
        {
            return FindFiles(path, string.Empty);
        }

        public static List<string> FindFiles(string path, string search)
        {
            List<string> files = new List<string>();

            foreach (string searchpath in SearchPaths)
            {
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(searchpath, path));
                if (dir.Exists)
                {
                    FileInfo[] dirFiles;
                    if (search == string.Empty)
                        dirFiles = dir.GetFiles();
                    else
                        dirFiles = dir.GetFiles(search);

                    foreach (FileInfo file in dirFiles)
                        files.Add(file.FullName);
                }
            }

            return files;
        }

        public static string FindDirectory(string name)
        {
            foreach (string searchpath in SearchPaths)
            {
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(searchpath, name));
                if (dir.Exists)
                    return dir.FullName;
            }

            return string.Empty;
        }
    }
}
