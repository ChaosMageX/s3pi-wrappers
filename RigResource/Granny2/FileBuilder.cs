using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    public class FileBuilder
    {
        [DllImport("granny2.dll", EntryPoint = "GrannyBeginFile", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr BeginFile
            (
            Int32 sectionCount,
            UInt32 tag,
            IntPtr magic,
            string tempDir,
            string tempPrefix
            );
        [DllImport("granny2.dll", EntryPoint = "GrannyEndFile", CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndFile
            (
            IntPtr Builder,
            string fileName
            );

        [DllImport("granny2.dll", EntryPoint = "GrannyEndFileToWriter", CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndFile
            (
            IntPtr Builder,
            IntPtr Writer
            );
        [DllImport("granny2.dll", EntryPoint = "GrannySetFileSectionFormat", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetFileSectionFormat(IntPtr Builder, Int32 SectionIndex, CompressionType Compression, Int32 Alignment);
        [DllImport("granny2.dll", EntryPoint = "GrannyGetTemporaryDirectory", CallingConvention = CallingConvention.StdCall)]
        public static extern String GrannyGetTemporaryDirectory();


    }
}