using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Project24
{
    class ResourceManager
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
    }
}
