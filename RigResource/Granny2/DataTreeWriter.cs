using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    public struct DataTreeWriter
    {
        [DllImport("granny2.dll", EntryPoint = "GrannyBeginFileDataTreeWriting", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr BeginFileDataTreeWriting
            (
            IntPtr rootObjectType,          
            IntPtr rootObject,              
            Int32 defaultTypeSectionIndex,  
            Int32 defaultObjectSectionIndex
            );
        [DllImport("granny2.dll", EntryPoint = "GrannyWriteDataTreeToFile", CallingConvention = CallingConvention.StdCall)]
        public static extern bool WriteDataTreeToFile
            (
            IntPtr writer,
            UInt32 fileTypeTag,
            IntPtr platformMagicValue,
            string fileName,
            Int32 fileSectionCount
            );

        [DllImport("granny2.dll", EntryPoint = "GrannyEndFileDataTreeWriting", CallingConvention = CallingConvention.StdCall)]
        public static extern void EndFileDataTreeWriting(IntPtr Writer);

        [DllImport("granny2.dll", EntryPoint = "GrannyWriteDataTreeToFileBuilder", CallingConvention = CallingConvention.StdCall)]
        public static extern bool WriteDataTreeToFileBuilder(IntPtr Writer, IntPtr Builder);
        [DllImport("granny2.dll", EntryPoint = "GrannySetFileSectionForObject", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetFileSectionForObject(IntPtr Writer, IntPtr Obj, Int32 SectionIndex);

    }
}