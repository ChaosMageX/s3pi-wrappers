using System;
using System.Runtime.InteropServices;
using System.IO;

namespace s3piwrappers.Granny2
{
    public enum CompressionType
    {
        NoCompression,
        Oodle0Compression,
        Oodle1Compression
    }
    public class IO
    {
        public static IntPtr FromMemory(Stream s)
        {

            byte[] buffer = new byte[s.Length];
            s.Read(buffer, 0, buffer.Length);
            IntPtr pBuffer = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, pBuffer, buffer.Length);
            return FromMemory(buffer.Length, pBuffer);
        }
        [DllImport("granny2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GrannyReadEntireFileFromMemory")]
        public static extern IntPtr FromMemory(Int32 MemorySize, IntPtr Memory);
        [DllImport("granny2.dll", EntryPoint = "GrannyReadEntireFile", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr Open(string src);
        [DllImport("granny2.dll", EntryPoint = "GrannyGetFileInfo", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetFileInfo(IntPtr pFile);
    }
}