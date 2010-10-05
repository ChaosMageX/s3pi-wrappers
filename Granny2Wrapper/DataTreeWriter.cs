using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    internal static class DataTreeWriter
    {
        [DllImport("granny2.dll", EntryPoint = "GrannyBeginFileDataTreeWriting", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr BeginFileDataTreeWriting
            (
            IntPtr rootObjectType,          
            IntPtr rootObject,              
            Int32 defaultTypeSectionIndex,  
            Int32 defaultObjectSectionIndex
            );
        [DllImport("granny2.dll", EntryPoint = "GrannyEndFileDataTreeWriting", CallingConvention = CallingConvention.StdCall)]
        public static extern void EndFileDataTreeWriting
            (
            IntPtr writer
            );
        [DllImport("granny2.dll", EntryPoint = "GrannyWriteDataTreeToFileBuilder", CallingConvention = CallingConvention.StdCall)]
        public static extern bool WriteDataTreeToFileBuilder
            (
            IntPtr writer, 
            IntPtr builder
            );

    }
}