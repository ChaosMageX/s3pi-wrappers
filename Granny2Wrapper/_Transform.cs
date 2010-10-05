using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _Transform
    {
        public TransformFlags Flags;
        public _Triple Position;
        public _Quad Orientation;
        public _Triple ScaleShear_0;
        public _Triple ScaleShear_1;
        public _Triple ScaleShear_2;
    };
}