using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public abstract class Frame : AHandlerElement,
                                  IEquatable<Frame>
    {
        protected float[] mData;
        protected UInt16 mFlags;
        protected UInt16 mFrameIndex;

        protected Frame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { mData = new float[GetFloatCount()]; }

        protected Frame(int apiVersion, EventHandler handler, Frame basis) : base(apiVersion, handler)
        {
            mData = basis.mData;
            mFrameIndex = basis.mFrameIndex;
            mFlags = basis.mFlags;
        }

        public Frame(int apiVersion, EventHandler handler, Stream s, CurveDataInfo info, IList<float> indexedFloats) : this(apiVersion, handler) { Parse(s, info, indexedFloats); }

        protected abstract Int32 GetFloatCount();
        protected abstract Int32 GetBitsPerFloat();
        protected abstract Int32 GetPackedCount();
        protected abstract UInt64 ReadPacked(Stream stream);
        protected abstract void WritePacked(Stream stream, UInt64 packed);


        [ElementPriority(1)]
        public ushort FrameIndex
        {
            get { return mFrameIndex; }
            set
            {
                if (mFrameIndex != value)
                {
                    mFrameIndex = value;
                    OnElementChanged();
                }
            }
        }

        [ElementPriority(2)]
        public UInt16 Flags
        {
            get { return mFlags; }
            set
            {
                if (mFlags != value)
                {
                    mFlags = value;
                    OnElementChanged();
                }
            }
        }

        [ElementPriority(3)]
        public float[] Data { get { return mData; } set { mData = value; } }

        public string Value { get { return ValueBuilder; } }

        public override List<string> ContentFields { get { return GetContentFields(0, GetType()); } }

        public override int RecommendedApiVersion { get { return 1; } }

        #region IEquatable<Frame> Members

        public bool Equals(Frame other)
        {
            var a = GetFloatValues();
            var b = other.GetFloatValues();
            if (a.Count() != b.Count()) return false;
            return !(from af in a from bf in b where af != bf select af).Any();
        }

        #endregion

        public static Frame CreateInstance(int apiVersion, EventHandler handler, CurveType t, Stream s, CurveDataInfo info, IList<float> floats)
        {
            switch (t)
            {
                case CurveType.Position:
                    return new Float3Frame(apiVersion, handler, s, info, floats);
                case CurveType.Orientation:
                    return new Float4Frame(apiVersion, handler, s, info, floats);
                default:
                    throw new NotSupportedException("Frame Type " + t + " is not supported.");
            }
        }

        public static Frame CreateInstance(int apiVersion, EventHandler handler, CurveType t)
        {
            switch (t)
            {
                case CurveType.Position:
                    return new Float3Frame(apiVersion, handler);
                case CurveType.Orientation:
                    return new Float4Frame(apiVersion, handler);
                default:
                    throw new NotSupportedException("Frame Type " + t + " is not supported.");
            }
        }

        public void Parse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            BinaryReader br = new BinaryReader(s);
            mFrameIndex = br.ReadUInt16();
            var flags = br.ReadUInt16();
            mFlags = (UInt16) (flags >> 4);
            try
            {
                int floatsRead = 0;
                switch (info.Flags.Format)
                {
                    case CurveDataFormat.Indexed:

                        while (floatsRead < GetFloatCount())
                        {
                            bool flipped = ((flags & 1 << floatsRead) != 0 ? true : false);
                            mData[floatsRead] = PackedFloat.Unpack(indexedFloats[br.ReadUInt16()], info.Offset, info.Scale, flipped);
                            floatsRead++;
                        }
                        break;
                    case CurveDataFormat.Packed:
                        while(floatsRead < GetFloatCount())
                        {
                            for (int packedRead = 0; packedRead < GetPackedCount(); packedRead++)
                            {
                                ulong packed = ReadPacked(s);
                                bool flipped = (flags & (1 << floatsRead)) != 0 ? true : false;
                                mData[floatsRead] = PackedFloat.Unpack(packed, packedRead, GetBitsPerFloat(), info.Offset, info.Scale, flipped);
                                floatsRead++;
                            }
                        }
                        break;
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public virtual void UnParse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(mFrameIndex);
            var flags = mFlags << 4;
            var floats = GetFloatValues().ToArray();
            for (int i = 0; i < floats.Length; i++)
                if (floats[i] < 0) flags |= (1 << i);
            bw.Write((UInt16) flags);


            switch (info.Flags.Format)
            {
                case CurveDataFormat.Indexed:

                    for (int i = 0; i < GetFloatCount(); i++)
                    {
                        bool flipped = ((flags & 1 << i) != 0 ? true : false);
                        float packedIndex = PackedFloat.Pack(mData[i], info.Offset, info.Scale, flipped);
                        if (!indexedFloats.Contains(packedIndex)) indexedFloats.Add(packedIndex);
                        bw.Write((UInt16) indexedFloats.IndexOf(packedIndex));
                    }
                    break;
                case CurveDataFormat.Packed:
                    //byte[] bytes = new byte[8];
                    //byte[] packedBytes = br.ReadBytes((int)Math.Ceiling((BitsPerFloat * FloatCount) / 8d));
                    //Array.Copy(bytes, 0, packedBytes, 0, packedBytes.Length);
                    //ulong packed = BitConverter.ToUInt64(bytes, 0);
                    //for (int i = 0; i < FloatCount; i++)
                    //{
                    //    bool flipped = ((flags & 1 << i) != 0 ? true : false);
                    //    mData[i] = PackedFloat.Unpack(packed, i, BitsPerFloat, info.Offset, info.Scale, flipped);
                    //}
                    break;
            }
        }

        public virtual IEnumerable<float> GetFloatValues() { return mData; }
        public override AHandlerElement Clone(EventHandler handler) { return (AHandlerElement) Activator.CreateInstance(GetType(), new object[] {0, handler, this}); }
    }
}