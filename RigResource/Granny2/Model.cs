using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Model
    {
        public String Name;
        public IntPtr Skeleton;
        public Transform InitialPlacement;
        public Int32 MeshBindingCount;
        public IntPtr MeshBindings;

    }
}