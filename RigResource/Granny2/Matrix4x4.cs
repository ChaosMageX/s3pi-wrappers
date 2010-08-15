using System.Runtime.InteropServices;

namespace s3piwrappers.Granny2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Matrix4x4
    {
        public Quad m0, m1, m2, m3;
    }
}