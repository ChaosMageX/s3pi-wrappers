using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace s3piwrappers.Granny2
{
    public static class IO
    {
        [DllImport("granny2.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GrannyReadEntireFileFromMemory")]
        private static extern IntPtr FromMemory(Int32 MemorySize, IntPtr Memory);
        [DllImport("granny2.dll", EntryPoint = "GrannyReadEntireFile", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr Open(string src);
        [DllImport("granny2.dll", EntryPoint = "GrannyGetFileInfo", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetFileInfo(IntPtr pFile);

        public static void SaveFile(GrannyFileInfo file, string filename, CompressionType c)
        {
            string tempDir = Path.GetTempPath();
            IntPtr pFileInfo = file.ToStruct().Ptr();
            IntPtr builder = FileBuilder.BeginFile(1, 0x8000001C, Constants.GrannyFileMV_Old, tempDir, "_gr2");
            IntPtr writer = DataTreeWriter.BeginFileDataTreeWriting(Constants.GrannyFileInfoType, pFileInfo, 0, 0);
            FileBuilder.SetFileSectionFormat(builder, 0, c, 4);
            if (!DataTreeWriter.WriteDataTreeToFileBuilder(writer, builder))
            {
                throw new Exception("Failed to write Granny2 data.");
            }
            if (!FileBuilder.EndFile(builder, filename))
            {
                throw new Exception("Failed to write Granny2 file.");
            }
            Marshal.FreeHGlobal(pFileInfo);
        }

        public static GrannyFileInfo LoadFile(int apiVersion,EventHandler handler,String path)
        {
            IntPtr pFile = IO.Open(path);
            IntPtr pFileInfo = IO.GetFileInfo(pFile);
            return new GrannyFileInfo(apiVersion, handler, pFileInfo.S<_GrannyFileInfo>());
        }

        public static GrannyFileInfo LoadFile(int apiVersion, EventHandler handler, Stream s)
        {

            byte[] buffer = new byte[s.Length];
            s.Read(buffer, 0, buffer.Length);
            IntPtr pBuffer = Marshal.AllocHGlobal(buffer.Length);
            Marshal.Copy(buffer, 0, pBuffer, buffer.Length);
            IntPtr pFile = IO.FromMemory(buffer.Length, pBuffer);
            IntPtr pFileInfo = IO.GetFileInfo(pFile);
            return new GrannyFileInfo(apiVersion, handler, pFileInfo.S<_GrannyFileInfo>());
        }
    }
}
