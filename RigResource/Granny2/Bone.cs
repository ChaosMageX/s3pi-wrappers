using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Bone
    {
        public String Name;
        public Int32 ParentIndex;
        public Transform LocalTransform;
        public Matrix4x4 InverseWorld4x4;
        public Single LODError;
        public Variant ExtendedData;
        
    }
}