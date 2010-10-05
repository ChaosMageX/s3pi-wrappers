using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _Matrix4x4
    {
        public _Quad m0, m1, m2, m3;
    }
}