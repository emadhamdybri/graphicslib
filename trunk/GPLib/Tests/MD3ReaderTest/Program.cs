using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using MD3;

namespace MD3ReaderTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo dir = new DirectoryInfo(args[0]);
            Reader.Read(dir);
        }
    }
}
