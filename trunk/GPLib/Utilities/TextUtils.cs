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
using System.IO;
using System.Text;

namespace Utilities.Paths
{
    public class PathUtils
    {
        public static string MakePathRelitive(string rootpath, string outpath)
        {
            if (rootpath == string.Empty)
                return outpath;

            string[] rootChunks = rootpath.Split(Path.DirectorySeparatorChar.ToString().ToCharArray());

            string[] outchunks = outpath.Split(Path.DirectorySeparatorChar.ToString().ToCharArray());

            string relPath = string.Empty;

            int i = 0;
            for (i = 0; i < rootChunks.Length; i++)
            {
                if (rootChunks[i] != outchunks[i])
                    break;
            }

            for (int j = i; j < rootChunks.Length; j++ )
                relPath += ".." + Path.DirectorySeparatorChar.ToString();

            for (; i < outchunks.Length; i++)
            {
                relPath += outchunks[i];
                if (i != outchunks.Length - 1)
                    relPath += Path.DirectorySeparatorChar.ToString();
            }
            return relPath;
        }
    }
}
