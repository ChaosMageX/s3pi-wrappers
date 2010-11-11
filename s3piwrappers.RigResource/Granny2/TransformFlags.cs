using System;
namespace s3piwrappers.Granny2
{
    [Flags]
    public enum TransformFlags : uint
    {
        Position = 0x00000001,
        Orientation = 0x00000002,
        ScaleShear = 0x00000004
    }
}
