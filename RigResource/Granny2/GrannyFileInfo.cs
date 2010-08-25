using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GrannyFileInfo
    {
        public IntPtr ArtToolInfo;
        public IntPtr ExporterInfo;
        public String FromFileName;
        public Int32 TextureCount;
        public IntPtr Textures;
        public Int32 MaterialCount;
        public IntPtr Materials;
        public Int32 SkeletonCount;
        public IntPtr Skeletons;
        public Int32 VertexDataCount;
        public IntPtr VertexDatas;
        public Int32 TriTopologyCount;
        public IntPtr TriTopologies;
        public Int32 MeshCount;
        public IntPtr Meshes;
        public Int32 ModelCount;
        public IntPtr Models;
        public Int32 TrackGroupCount;
        public IntPtr TrackGroups;
        public Int32 AnimationCount;
        public IntPtr Animations;
        public Variant Extra;

        public void Save(string filename, CompressionType c)
        {
            IntPtr pFileInfo = Marshal.AllocHGlobal(Marshal.SizeOf(this));
            Marshal.StructureToPtr(this,pFileInfo,false);
            IntPtr Builder = FileBuilder.BeginFile(1, 0x8000001C, Constants.GrannyFileMV_Old, IO.GetTemporaryDirectory(), "_gr2");
            IntPtr Writer = DataTreeWriter.BeginFileDataTreeWriting(Constants.GrannyFileInfoType, pFileInfo, 0, 0);
            FileBuilder.SetFileSectionFormat(Builder, 0, c, 4);
            if(!DataTreeWriter.WriteDataTreeToFileBuilder(Writer, Builder))
            {
                throw new Exception("Failed to write Granny2 data.");
            }
            if(!FileBuilder.EndFile(Builder, filename))
            {
                throw new Exception("Failed to write Granny2 file.");
            }
            Marshal.FreeHGlobal(pFileInfo);
        }
    }
}