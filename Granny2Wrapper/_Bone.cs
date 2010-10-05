using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _Bone
    {
        public String Name;
        public Int32 ParentIndex;
        public _Transform LocalTransform;
        public _Matrix4x4 InverseWorld4x4;
        public Single LODError;
        public _Variant Extra;
        
    }
}