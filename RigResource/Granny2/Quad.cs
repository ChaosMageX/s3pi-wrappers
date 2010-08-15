using System.Runtime.InteropServices;
using System;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quad
    {
        public float X, Y, Z, W;
    }

}