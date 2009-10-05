using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PortalEdit
{
    public class Resources
    {
        protected static void DirWalk (DirectoryInfo info, ref List<string> contents, string filter, bool recursive, bool dirOnly )
        {
            if (!dirOnly)
            {
                FileInfo[] files;
                if (filter == string.Empty)
                    files = info.GetFiles();
                else
                    files = info.GetFiles(filter);

                foreach (FileInfo file in files)
                {
                    if (Path.GetFileNameWithoutExtension(file.FullName).ToCharArray()[0] != '.')
                        contents.Add(file.FullName);
                }
            }

            if (recursive || dirOnly)
            {
                foreach (DirectoryInfo dir in info.GetDirectories())
                {
                    if (Path.GetDirectoryName(dir.FullName).ToCharArray()[0] == '.')
                        continue;
                    if (dirOnly)
                        contents.Add(dir.FullName);
                    if (recursive)
                        DirWalk(dir, ref contents, filter, recursive, dirOnly);
                }
            }
        }

        public static List<string> Files(string subfolder, string filter, bool recursive)
        {
            List<string> files = new List<string>();
            foreach ( String dir in Settings.settings.ResourceDirs)
            {
                DirectoryInfo info = new DirectoryInfo(Path.Combine(dir, subfolder));
                if (info.Exists)
                {
                    List<string> dirFiles = new List<string>();
                    DirWalk(info, ref dirFiles, filter, recursive,false);

                    foreach (string file in dirFiles)
                    {
                        string name = file.Remove(0, dir.Length);
                        if (!files.Contains(name))
                            files.Add(name);
                    }
                }
            }

            return files;
        }

        public static List<string> Files(string subfolder, bool recursive)
        {
            return Files(subfolder, String.Empty, recursive);
        }

        public static List<string> Directories(string subfolder, bool recursive)
        {
            List<string> dirs = new List<string>();
            foreach (String dir in Settings.settings.ResourceDirs)
            {
                DirectoryInfo info = new DirectoryInfo(Path.Combine(dir, subfolder));
                if (info.Exists)
                {
                    List<string> dirDirs = new List<string>();
                    DirWalk(info, ref dirDirs, string.Empty, recursive, true);

                    foreach (string folder in dirDirs)
                    {
                        string name = folder.Remove(0, subfolder.Length);
                        if (!dirs.Contains(name))
                            dirs.Add(name);
                    }
                }
            }
            return dirs;
        }

        public static List<string> Directories(bool recursive)
        {
            return Directories(string.Empty, recursive);
        }

        public static FileInfo File(string filename)
        {
            foreach (String dir in Settings.settings.ResourceDirs)
            {
                FileInfo info = new FileInfo(Path.Combine(dir, filename));
                if (info.Exists)
                    return info;
            }
            return null;
        }
    }
}
