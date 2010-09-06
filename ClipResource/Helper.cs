using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace s3piwrappers
{
    internal static class ClipIOExtensions
    {
        public static string ReadZString(this BinaryReader br)
        {
            return br.ReadZString(0);
        }
        public static string ReadZString(this BinaryReader br, int padLength)
        {
            String s = "";
            byte b = br.ReadByte();
            while (b != 0)
            {
                s += Encoding.ASCII.GetString(new byte[1] { b });
                b = br.ReadByte();
            }
            if (padLength != 0)
            {
                int count = padLength - 1 - s.Length;
                br.BaseStream.Seek(count, SeekOrigin.Current);
            }
            return s;
        }
        public static void WriteZString(this BinaryWriter bw, String s)
        {
            bw.WriteZString(s, 0x00, 0);
        }
        public static void WriteZString(this BinaryWriter bw, String s, byte paddingChar, int padLength)
        {
            if (!string.IsNullOrEmpty(s))
            {
                bw.Write(Encoding.ASCII.GetBytes(s));
            }

            bw.Write((byte)0x00);
            if (padLength > 0)
            {
                int count = padLength - 1;
                if (!string.IsNullOrEmpty(s)) count -= s.Length;
                for (int i = 0; i < count; i++)
                {
                    bw.Write(paddingChar);
                }
            }
        }
    }
}
