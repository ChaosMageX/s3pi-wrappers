using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers
{
    //Intentionally non-abstract to prevent grid view from searching for [ConstructorInfo] to create instances
    public class Frame : AHandlerElement, IEquatable<Frame>
    {
        protected UInt16 mFrameIndex;
        protected Byte mFlags;
        protected Byte mExtraFlags;

        protected Frame(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
        }
        protected Frame(int apiVersion, EventHandler handler, Frame basis)
            : base(apiVersion, handler)
        {
            mFrameIndex = basis.mFrameIndex;
            mFlags = basis.mFlags;
            mExtraFlags = basis.mExtraFlags;
        }
        public Frame(int apiVersion, EventHandler handler, Stream s, CurveDataInfo info, IList<float> indexedFloats)
            : base(apiVersion, handler)
        {
            Parse(s,info,indexedFloats);
        }

        public static Frame CreateInstance(int apiVersion, EventHandler handler, CurveType t, Stream s, CurveDataInfo info, IList<float> floats)
        {
            switch (t)
            {
                case CurveType.Position:
                    return new PositionFrame(apiVersion, handler, s, info, floats);
                case CurveType.Orientation:
                    return new OrientationFrame(apiVersion, handler, s, info, floats);
                default: throw new NotSupportedException("Frame Type " + t.ToString() + " is not supported.");
            }
        }
        public static Frame CreateInstance(int apiVersion, EventHandler handler, CurveType t)
        {
            switch (t)
            {
                case CurveType.Position:
                    return new PositionFrame(apiVersion, handler);
                case CurveType.Orientation:
                    return new OrientationFrame(apiVersion, handler);
                default: throw new NotSupportedException("Frame Type " + t.ToString() + " is not supported.");
            }
        }
        [ElementPriority(1)]
        public ushort FrameIndex
        {
            get { return mFrameIndex; }
            set { mFrameIndex = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public byte Flags
        {
            get { return mFlags; }
            set { mFlags = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public byte ExtraFlags
        {
            get { return mExtraFlags; }
            set { mExtraFlags = value; OnElementChanged(); }
        }

        public virtual void Parse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            BinaryReader br = new BinaryReader(s);
            mFrameIndex = br.ReadUInt16();
            mFlags = br.ReadByte();
            mExtraFlags = br.ReadByte();
        }
        public virtual void UnParse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(mFrameIndex);
            bw.Write(mFlags);
        }

        public virtual IEnumerable<float> GetFloatValues() { yield break;}
        public override string ToString()
        {
            return String.Format("[{0:X2}]", mExtraFlags);
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return (AHandlerElement) Activator.CreateInstance(GetType(), new object[] {0, handler, this});
        }

        public override List<string> ContentFields
        {
            get { return GetContentFields(0, GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }

        public bool Equals(Frame other)
        {
            return base.Equals(other);
        }
    }
}