﻿using System;

namespace s3piwrappers
{
    public class CurveDataInfo
    {
        public CurveDataFlags Flags;
        public Int32 FrameCount;
        public UInt32 FrameDataOffset;
        public Single Offset;
        public Single Scale;
        public UInt32 TrackKey;
        public CurveType Type;
    }
}