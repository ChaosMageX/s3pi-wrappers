using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    internal static class FileBuilder
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
            IntPtr builder,
            string fileName
            );

        [DllImport("granny2.dll", EntryPoint = "GrannyEndFileToWriter", CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndFile
            (
            IntPtr builder,
            IntPtr writer
            );
        [DllImport("granny2.dll", EntryPoint = "GrannySetFileSectionFormat", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetFileSectionFormat
            (
            IntPtr builder, 
            Int32 sectionIndex, 
            CompressionType compression, 
            Int32 alignment
            );



    }
}