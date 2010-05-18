using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Runtime;
using System.Text;

namespace ColorFileFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<FileInfo> FilesToKill = new List<FileInfo>();

            List<DirectoryInfo> ColorDirs = new List<DirectoryInfo>();

            ColorDirs.Add(MakeDir("red"));
            ColorDirs.Add(MakeDir("blue"));
            ColorDirs.Add(MakeDir("green"));
            ColorDirs.Add(MakeDir("purple"));
            ColorDirs.Add(MakeDir("orange"));
            ColorDirs.Add(MakeDir("black"));
            ColorDirs.Add(MakeDir("white"));
            ColorDirs.Add(MakeDir("brown"));
            ColorDirs.Add(MakeDir("teal"));

            DirectoryInfo rootDir = new DirectoryInfo("./");
            foreach (FileInfo file in rootDir.GetFiles("*.png"))
            {
                if (file.Name != "rc-magenta.png")
                {
                    string[] nugs = file.Name.Split("-".ToCharArray());
                    if ( nugs.Length >= 3)
                    {
                        string[] subnug = nugs[3].Split(".".ToCharArray());
                        try
                        {
                            int color = 0;
                            int.TryParse(subnug[0],out color);
                            if (color > 0)
                            {
                                Console.WriteLine("Moving file " + file.Name + " to color dir " + ColorDirs[color - 1].Name);
                                file.MoveTo(Path.Combine(ColorDirs[color - 1].FullName, nugs[0] + ".png"));
                            }
                        }
                        catch (System.Exception /*ex*/)
                        {
                            Console.WriteLine("Skipping file " + file.Name);
                        }
                    }
                }
            }
        }

        static DirectoryInfo MakeDir ( string path )
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            dir.Create();
            return dir;
        }
    }
}
