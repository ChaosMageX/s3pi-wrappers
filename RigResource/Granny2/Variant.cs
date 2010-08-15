using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Variant
    {
        public IntPtr Type;
        public IntPtr Data;
    }
}