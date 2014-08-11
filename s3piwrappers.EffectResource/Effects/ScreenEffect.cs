using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class ScreenEffect : Effect, IEquatable<ScreenEffect>
    {
        public class Filter : DataElement, IEquatable<Filter>
        {
            #region Constructors
            public Filter(int apiVersion, EventHandler handler, Filter basis)
                : base(apiVersion, handler, basis)
            {
            }

            public Filter(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler, s)
            {
            }

            public Filter(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mParameters = new DataList<ByteValue>(handler);
            }
            #endregion

            #region Attributes
            private byte mType;
            private byte mDestination;
            private ulong mSource;
            private DataList<ByteValue> mParameters;
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public byte Type
            {
                get { return mType; }
                set
                {
                    mType = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public byte Destination
            {
                get { return mDestination; }
                set
                {
                    mDestination = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public ulong Source
            {
                get { return mSource; }
                set
                {
                    mSource = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(4)]
            public DataList<ByteValue> Parameters
            {
                get { return mParameters; }
                set
                {
                    mParameters = value;
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mType);
                s.Read(out mDestination);
                s.Read(out mSource);
                mParameters = new DataList<ByteValue>(handler, stream);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mType);
                s.Write(mDestination);
                s.Write(mSource);
                mParameters.UnParse(stream);
            }
            #endregion

            public bool Equals(Filter other)
            {
                return base.Equals(other);
            }
        }

        public class FilterTempBuffer : DataElement, IEquatable<FilterTempBuffer>
        {
            #region Constructors
            public FilterTempBuffer(int apiVersion, EventHandler handler, FilterTempBuffer basis)
                : base(apiVersion, handler, basis)
            {
            }

            public FilterTempBuffer(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler, s)
            {
            }

            public FilterTempBuffer(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }
            #endregion

            #region Attributes
            private int mScreenRatio;
            private int mSize;
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public int ScreenRatio
            {
                get { return mScreenRatio; }
                set
                {
                    mScreenRatio = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public int Size
            {
                get { return mSize; }
                set
                {
                    mSize = value;
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mScreenRatio);
                s.Read(out mSize);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mScreenRatio);
                s.Write(mSize);
            }
            #endregion

            public bool Equals(FilterTempBuffer other)
            {
                //return this.mScreenRatio == other.mScreenRatio && this.mSize == other.mSize;
                return base.Equals(other);
            }
        }

        public class ResourceId : DataElement, IEquatable<ResourceId>
        {
            #region Constructors
            public ResourceId(int apiVersion, EventHandler handler, Filter basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ResourceId(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler, s)
            {
            }

            public ResourceId(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }
            #endregion

            private ulong mData;

            [ElementPriority(1)]
            public ulong Data
            {
                get { return mData; }
                set
                {
                    mData = value;
                    OnElementChanged();
                }
            }

            #region Data I/O
            private bool isTheSims4 = false;

            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                if (isTheSims4)
                {
                    s.Read(out mData);
                }
                else
                {
                    uint data;
                    s.Read(out data);
                    mData = data;
                }
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                if (isTheSims4)
                {
                    s.Write(mData);
                }
                else
                {
                    uint data = (uint)(mData & 0xFFFFFFFF);
                    s.Write(data);
                }
            }
            #endregion

            public bool Equals(ResourceId other)
            {
                //return this.mData = other.mData;
                return base.Equals(other);
            }
        }

        #region Constructors
        public ScreenEffect(int apiVersion, EventHandler handler, ScreenEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public ScreenEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mColorCurve = new DataList<ColorValue>(handler);
            mStrengthCurve = new DataList<FloatValue>(handler);
            mDistanceCurve = new DataList<FloatValue>(handler);
            mFilters = new DataList<Filter>(handler);
            mTemporaryBuffers = new DataList<FilterTempBuffer>(handler);
            mFloatParameters = new DataList<FloatValue>(handler);
            mVector3Parameters = new DataList<Vector3ValueLE>(handler);
            mVector2Parameters = new DataList<Vector2ValueLE>(handler);
            mResourceIdParameters = new DataList<ResourceId>(handler);
        }

        public ScreenEffect(int apiVersion, EventHandler handler, ISection section, Stream s) 
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private byte mMode;
        private uint mFlags;
        private DataList<ColorValue> mColorCurve;
        private DataList<FloatValue> mStrengthCurve;
        private DataList<FloatValue> mDistanceCurve;
        private float mLifetime;
        private float mDelay;
        private float mFalloff;
        private float mDistanceBase;
        private ulong mTextureId;
        private DataList<Filter> mFilters;
        private DataList<FilterTempBuffer> mTemporaryBuffers;//originally DataList<Vector2Value>
        private DataList<FloatValue> mFloatParameters;//originally DataList<UInt32Value>
        private DataList<Vector3ValueLE> mVector3Parameters;
        private DataList<Vector2ValueLE> mVector2Parameters;
        private DataList<ResourceId> mResourceIdParameters;//originally DataList<UInt32Value>
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public byte Mode
        {
            get { return mMode; }
            set
            {
                mMode = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public uint Flags
        {
            get { return mFlags; }
            set
            {
                mFlags = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public DataList<ColorValue> ColorCurve
        {
            get { return mColorCurve; }
            set
            {
                mColorCurve = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public DataList<FloatValue> StrengthCurve
        {
            get { return mStrengthCurve; }
            set
            {
                mStrengthCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public DataList<FloatValue> DistanceCurve
        {
            get { return mDistanceCurve; }
            set
            {
                mDistanceCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public float Lifetime
        {
            get { return mLifetime; }
            set
            {
                mLifetime = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public float Delay
        {
            get { return mDelay; }
            set
            {
                mDelay = value;
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public float Falloff
        {
            get { return mFalloff; }
            set
            {
                mFalloff = value;
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public float DistanceBase
        {
            get { return mDistanceBase; }
            set
            {
                mDistanceBase = value;
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public ulong TextureId
        {
            get { return mTextureId; }
            set
            {
                mTextureId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public DataList<Filter> Items
        {
            get { return mFilters; }
            set
            {
                mFilters = new DataList<Filter>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public DataList<FilterTempBuffer> TemporaryBuffers
        {
            get { return mTemporaryBuffers; }
            set
            {
                mTemporaryBuffers = new DataList<FilterTempBuffer>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(13)]
        public DataList<FloatValue> FloatParameters
        {
            get { return mFloatParameters; }
            set
            {
                mFloatParameters = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(14)]
        public DataList<Vector3ValueLE> Vector3Parameters
        {
            get { return mVector3Parameters; }
            set
            {
                mVector3Parameters = new DataList<Vector3ValueLE>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(15)]
        public DataList<Vector2ValueLE> Vector2Parameters
        {
            get { return mVector2Parameters; }
            set
            {
                mVector2Parameters = new DataList<Vector2ValueLE>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(16)]
        public DataList<ResourceId> ResourceIdParameters
        {
            get { return mResourceIdParameters; }
            set
            {
                mResourceIdParameters = new DataList<ResourceId>(handler, value);
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mMode);
            s.Read(out mFlags);
            //mFlags &= 0x3;

            mColorCurve = new DataList<ColorValue>(handler, stream);
            mStrengthCurve = new DataList<FloatValue>(handler, stream);
            mDistanceCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mLifetime);
            s.Read(out mDelay);
            s.Read(out mFalloff);
            s.Read(out mDistanceBase);
            s.Read(out mTextureId);
            mFilters = new DataList<Filter>(handler, stream);
            mTemporaryBuffers = new DataList<FilterTempBuffer>(handler, stream);
            mFloatParameters = new DataList<FloatValue>(handler, stream);
            mVector3Parameters = new DataList<Vector3ValueLE>(handler, stream);
            mVector2Parameters = new DataList<Vector2ValueLE>(handler, stream);
            mResourceIdParameters = new DataList<ResourceId>(handler, stream);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mMode);
            s.Write(mFlags);
            mColorCurve.UnParse(stream);
            mStrengthCurve.UnParse(stream);
            mDistanceCurve.UnParse(stream);
            s.Write(mLifetime);
            s.Write(mDelay);
            s.Write(mFalloff);
            s.Write(mDistanceBase);
            s.Write(mTextureId);
            mFilters.UnParse(stream);
            mTemporaryBuffers.UnParse(stream);
            mFloatParameters.UnParse(stream);
            mVector3Parameters.UnParse(stream);
            mVector2Parameters.UnParse(stream);
            mResourceIdParameters.UnParse(stream);
        }
        #endregion

        public bool Equals(ScreenEffect other)
        {
            return base.Equals(other);
        }
    }
}
