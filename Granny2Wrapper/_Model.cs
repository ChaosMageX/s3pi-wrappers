using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _Model
    {
        public String Name;
        public IntPtr Skeleton;
        public _Transform InitialPlacement;
        public Int32 MeshBindingCount;
        public IntPtr MeshBindings;

    }
}