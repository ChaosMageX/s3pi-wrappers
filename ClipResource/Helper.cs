using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace s3piwrappers
{
    internal static class Helper
    {
        public static string ReadZString(this BinaryReader br)
        {
            String s = "";
            byte b = br.ReadByte();
            while (b != 0)
            {
                s += Encoding.ASCII.GetString(new byte[1] {b});
                b = br.ReadByte();
            }
            return s;
        }
        public static void WriteZString(this BinaryWriter bw, String s)
        {
            bw.Write(Encoding.ASCII.GetBytes(s));
            bw.Write((byte)0x00);
        }
    }
}
