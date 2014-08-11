using System;

namespace s3piwrappers.JazzGraph.Enums
{
    [Flags]
    public enum StateFlags : uint
    {
        None         = 0x0000,
        Public       = 0x0001,
        Entry        = 0x0002,
        Exit         = 0x0004,
        Loop         = 0x0008,
        OneShot      = 0x0010,
        OneShotHold  = 0x0020,
        Synchronized = 0x0040,
        Join         = 0x0080,
        Explicit     = 0x0100
    }
}
