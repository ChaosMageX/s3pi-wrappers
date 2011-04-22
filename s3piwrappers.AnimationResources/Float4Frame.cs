using System;
using System.Collections.Generic;
using System.IO;

namespace s3piwrappers
{
    public class Float4Frame : Frame,
                               IEquatable<Float4Frame>
    {
        public Float4Frame(int apiVersion, EventHandler handler, Float4Frame basis) : base(apiVersion, handler, basis) { }
        public Float4Frame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }

        public Float4Frame(int apiVersion, EventHandler handler, Stream s, CurveDataInfo info, IList<float> indexedFloats) : base(apiVersion, handler, s, info, indexedFloats) { }

        protected override int GetFloatCount() { return 4; }
        protected override int GetBitsPerFloat() { return 12; }
        protected override int GetPackedCount() { return 4; }
        protected override ulong ReadPacked(Stream stream) { return new BinaryReader(stream).ReadUInt16(); }
        protected override void WritePacked(Stream stream, ulong packed) { new BinaryWriter(stream).Write((UInt16) packed); }

        #region IEquatable<Float4Frame> Members

        public bool Equals(Float4Frame other) { return base.Equals(other); }

        #endregion
    }
}