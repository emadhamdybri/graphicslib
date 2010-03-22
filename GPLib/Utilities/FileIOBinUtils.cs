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
using System.Runtime.InteropServices;

namespace Utilities.FileIO
{
    public class BinUtils
    {
        public static int GetNextStreamCount(Stream fs)
        {
            byte[] b = new byte[Marshal.SizeOf(typeof(short))];
            fs.Read(b, 0, Marshal.SizeOf(typeof(short)));
            short i = (short)RawDeserializeEx(b, typeof(short));

            return i;
        }

        public static byte[] GetNextStruct(Stream fs, Type anytype)
        {
            byte[] b = new byte[Marshal.SizeOf(anytype)];
            fs.Read(b, 0, b.Length);

            return b;
        }

        public static object ReadObject(Stream fs, Type anytype)
        {
            return RawDeserializeEx(GetNextStruct(fs, anytype), anytype);
        }

        public static object RawDeserializeEx(byte[] rawdatas, Type anytype)
        {
            int rawsize = Marshal.SizeOf(anytype);
            if (rawsize > rawdatas.Length)
                return null;
            GCHandle handle = GCHandle.Alloc(rawdatas, GCHandleType.Pinned);
            IntPtr buffer = handle.AddrOfPinnedObject();
            object retobj = Marshal.PtrToStructure(buffer, anytype);
            handle.Free();
            return retobj;
        }

        public static String FixString ( String input )
        {
            string[] nugs = input.Split("\0".ToCharArray());
            if (nugs.Length > 0)
                return nugs[0];
            return input;
        }

        public static String FixString( char[] input)
        {
            return FixString(new string(input));
        }

        public static String FixString(byte[] input)
        {
            return FixString(System.Text.Encoding.UTF8.GetString(input));
        }

        public static String ReadString ( Stream fs, int count )
        {
            byte[] b = new byte[count];
            fs.Read(b, 0, b.Length); // name
            return FixString(System.Text.Encoding.UTF8.GetString(b));
        }
    }
}
