using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _GrannyFileInfo
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
        public _Variant Extra;
    }
}