using System;
using System.Collections.Generic;
using System.IO;

namespace s3piwrappers
{
    public class Float3Frame : Frame,
                               IEquatable<Float3Frame>
    {
        public Float3Frame(int apiVersion, EventHandler handler, Float3Frame basis) : base(apiVersion, handler, basis) { }
        public Float3Frame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }

        public Float3Frame(int apiVersion, EventHandler handler, Stream s, CurveDataInfo info, IList<float> indexedFloats) : base(apiVersion, handler, s, info, indexedFloats) { }
        protected override int GetFloatCount() { return 3; }
        protected override int GetBitsPerFloat() { return 10; }
        protected override int GetPackedCount() { return 1; }
        protected override ulong ReadPacked(Stream stream) { return new BinaryReader(stream).ReadUInt32(); }
        protected override void WritePacked(Stream stream, ulong packed) { new BinaryWriter(stream).Write((UInt32) packed); }

        #region IEquatable<Float3Frame> Members

        public bool Equals(Float3Frame other) { return base.Equals(other); }

        #endregion
    }
}