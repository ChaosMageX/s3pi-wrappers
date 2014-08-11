using System;

namespace s3piwrappers.JazzGraph.Enums
{
    [Flags]
    public enum StateMachineFlags : uint
    {
        Default = 0x01,
        UnilateralActor = 0x01,
        PinAllResources = 0x02,
        BlendMotionAccumulation = 0x04,
        HoldAllPoses = 0x08
    }
}
