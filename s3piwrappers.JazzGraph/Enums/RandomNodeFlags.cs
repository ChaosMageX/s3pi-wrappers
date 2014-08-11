using System;

namespace s3piwrappers.JazzGraph.Enums
{
    [Flags]
    public enum RandomNodeFlags
    {
        None = 0x00,
        AvoidRepeats = 0x01
    }
}
