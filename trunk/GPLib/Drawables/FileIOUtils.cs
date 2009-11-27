using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

namespace Drawables.FileIOUtils
{
    class BinUtils
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
    }
}
