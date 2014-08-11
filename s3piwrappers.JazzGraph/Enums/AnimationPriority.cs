using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.JazzGraph.Enums
{
    public enum AnimationPriority
    {
        kAPBroadcast = -1,
        kAPCarryLeft = 0x9c40,
        kAPCarryLeftPlus = 0xafc8,
        kAPCarryRight = 0x7530,
        kAPCarryRightPlus = 0x88b8,
        kAPDefault = -2,
        kAPFacialIdle = 0x445c,
        kAPHigh = 0x4e20,
        kAPHighPlus = 0x61a8,
        kAPLookAt = 0xea60,
        kAPLow = 0x1770,
        kAPLowPlus = 0x1f40,
        kAPNormal = 0x2710,
        kAPNormalPlus = 0x3a98,
        kAPUltra = 0xc350,
        kAPUltraPlus = 0xd6d8,
        kAPUnset = 0
    }
}
