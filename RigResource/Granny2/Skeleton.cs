using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    public enum SkeletonLod
    {
        GrannyNoSkeletonLOD = 0x0,
        GrannyEstimatedLOD = 0x1,
        GrannyMeasuredLOD = 0x2,
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Skeleton
    {
        public String Name;
        public Int32 BoneCount;
        public IntPtr Bones;
        public SkeletonLod LODType;
        
    }
}