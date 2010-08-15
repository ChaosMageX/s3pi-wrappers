using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Transform
    {
        public UInt32 Flags;
        public Triple Position;
        public Quad Orientation;
        public Triple ScaleShear_0;
        public Triple ScaleShear_1;
        public Triple ScaleShear_2;
    };
}