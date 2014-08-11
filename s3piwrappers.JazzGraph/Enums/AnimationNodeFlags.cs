using System;

namespace s3piwrappers.JazzGraph.Enums
{
    [Flags]
    public enum AnimationNodeFlags : uint
    {
        TimingNormal = 0x00,
        Default = 0x01,
        AtEnd = 0x01,
        LoopAsNeeded = 0x02,
        OverridePriority = 0x04,
        Mirror = 0x08,
        OverrideMirror = 0x10,
        OverrideTiming0 = 0x20,
        OverrideTiming1 = 0x40,
        TimingMaster = 0x20,
        TimingSlave = 0x40,
        TimingIgnored = 0x60,
        TimingMask = 0x60,
        Interruptible = 0x80,
        ForceBlend = 0x100,
        UseTimingPriority = 0x200,
        UseTimingPriorityAsClockMaster = 0x400,
        BaseClipIsSocial = 0x800,
        AdditiveClipIsSocial = 0x1000,
        BaseClipIsObjectOnly = 0x2000,
        AdditiveClipIsObjectOnly = 0x4000,
        HoldPose = 0x8000,
        BlendMotionAccumulation = 0x10000
    }
}
