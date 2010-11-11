using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _Skeleton
    {
        public String Name;
        public Int32 BoneCount;
        public IntPtr Bones;
        public Int32 LODType;
        
    }
}