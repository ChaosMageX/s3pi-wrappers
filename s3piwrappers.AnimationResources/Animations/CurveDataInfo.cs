using System;

namespace s3piwrappers
{
    public class CurveDataInfo
    {
        public UInt32 FrameDataOffset;
        public UInt32 TrackKey;
        public Single Base;
        public Single Scale;
        public Int32 FrameCount;
        public CurveDataFlags Flags;
        public CurveType Type;
    }
}