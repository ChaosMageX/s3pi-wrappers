using System;
using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _Variant
    {
        public IntPtr Type;
        public IntPtr Data;
    }
}