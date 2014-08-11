using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class DistributeEffect : Effect, IEquatable<DistributeEffect>
    {
        #region Constructors
        public DistributeEffect(int apiVersion, EventHandler handler, DistributeEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public DistributeEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mPreTransform = new TransformElement(apiVersion, handler);
            mSizeCurve = new DataList<FloatValue>(handler);
            mPitchCurve = new DataList<FloatValue>(handler);
            mRollCurve = new DataList<FloatValue>(handler);
            mHeadingCurve = new DataList<FloatValue>(handler);
            mColorCurve = new DataList<ColorValue>(handler);
            mColorVary = new ColorValue(apiVersion, handler);
            mAlphaCurve = new DataList<FloatValue>(handler);
            mSurfaces = new DataList<Surface>(handler);
            mAltitudeRange = new Vector2ValueLE(apiVersion, handler);
            mDrawInfo = new ResourceReference(apiVersion, handler, section);
        }

        public DistributeEffect(int apiVersion, EventHandler handler, ISection section, Stream s) 
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private uint mFlags;
        private int mDensity;//originally uint
        private string mComponentName = string.Empty;
        private int mStart;//originally uint
        private byte mSourceType;
        private float mSourceSize;
        private TransformElement mPreTransform;
        private DataList<FloatValue> mSizeCurve;
        private float mSizeVary;
        private DataList<FloatValue> mPitchCurve;
        private DataList<FloatValue> mRollCurve;
        private DataList<FloatValue> mHeadingCurve;
        private float mPitchVary;//originally uint
        private float mRollVary;//originally uint
        private float mHeadingVary;//originally uint
        private float mPitchOffset;//originally uint
        private float mRollOffset;//originally uint
        private float mHeadingOffset;//originally uint
        private DataList<ColorValue> mColorCurve;
        private ColorValue mColorVary;
        private DataList<FloatValue> mAlphaCurve;
        private float mAlphaVary;//originally uint
        private DataList<Surface> mSurfaces;
        private ulong mEmitMapId;
        private ulong mColorMapId;
        private ulong mPinMapId;
        private Vector2ValueLE mAltitudeRange;
        private ResourceReference mDrawInfo;
        private byte mOverrideSet;
        private uint mMessageId;
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public uint Flags
        {
            get { return mFlags; }
            set
            {
                mFlags = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public int Density
        {
            get { return mDensity; }
            set
            {
                mDensity = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public string ComponentName
        {
            get { return mComponentName; }
            set
            {
                mComponentName = value ?? string.Empty;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public int Start
        {
            get { return mStart; }
            set
            {
                mStart = value;
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public byte SourceType
        {
            get { return mSourceType; }
            set
            {
                mSourceType = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public float SourceSize
        {
            get { return mSourceSize; }
            set
            {
                mSourceSize = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public TransformElement Transform
        {
            get { return mPreTransform; }
            set
            {
                mPreTransform = new TransformElement(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public DataList<FloatValue> SizeCurve
        {
            get { return mSizeCurve; }
            set
            {
                mSizeCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public float SizeVary
        {
            get { return mSizeVary; }
            set
            {
                mSizeVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public DataList<FloatValue> PitchCurve
        {
            get { return mPitchCurve; }
            set
            {
                mPitchCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public DataList<FloatValue> RollCurve
        {
            get { return mRollCurve; }
            set
            {
                mRollCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public DataList<FloatValue> HeadingCurve
        {
            get { return mHeadingCurve; }
            set
            {
                mHeadingCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(13)]
        public float PitchVary
        {
            get { return mPitchVary; }
            set
            {
                mPitchVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(14)]
        public float RollVary
        {
            get { return mRollVary; }
            set
            {
                mRollVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(15)]
        public float HeadingVary
        {
            get { return mHeadingVary; }
            set
            {
                mHeadingVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(16)]
        public float PitchOffset
        {
            get { return mPitchOffset; }
            set
            {
                mPitchOffset = value;
                OnElementChanged();
            }
        }

        [ElementPriority(17)]
        public float RollOffset
        {
            get { return mRollOffset; }
            set
            {
                mRollOffset = value;
                OnElementChanged();
            }
        }

        [ElementPriority(18)]
        public float HeadingOffset
        {
            get { return mHeadingOffset; }
            set
            {
                mHeadingOffset = value;
                OnElementChanged();
            }
        }

        [ElementPriority(19)]
        public DataList<ColorValue> ColorCurve
        {
            get { return mColorCurve; }
            set
            {
                mColorCurve = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(20)]
        public ColorValue ColorVary
        {
            get { return mColorVary; }
            set
            {
                mColorVary = new ColorValue(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(21)]
        public DataList<FloatValue> AlphaCurve
        {
            get { return mAlphaCurve; }
            set
            {
                mAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(22)]
        public float AlphaVary
        {
            get { return mAlphaVary; }
            set
            {
                mAlphaVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(23)]
        public DataList<Surface> Surfaces
        {
            get { return mSurfaces; }
            set
            {
                mSurfaces = new DataList<Surface>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(24)]
        public ulong EmitMapId
        {
            get { return mEmitMapId; }
            set
            {
                mEmitMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(25)]
        public ulong ColorMapId
        {
            get { return mColorMapId; }
            set
            {
                mColorMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(26)]
        public ulong PinMapId
        {
            get { return mPinMapId; }
            set
            {
                mPinMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(27)]
        public Vector2ValueLE AltitudeRange
        {
            get { return mAltitudeRange; }
            set
            {
                mAltitudeRange = new Vector2ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(28)]
        public ResourceReference DrawInfo
        {
            get { return mDrawInfo; }
            set
            {
                mDrawInfo = new ResourceReference(requestedApiVersion, handler, mSection, value);
                OnElementChanged();
            }
        }

        [ElementPriority(29)]
        public byte OverrideSet
        {
            get { return mOverrideSet; }
            set
            {
                mOverrideSet = value;
                OnElementChanged();
            }
        }

        [ElementPriority(30)]
        public uint MessageId
        {
            get { return mMessageId; }
            set
            {
                mMessageId = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            //mFlags &= 0x3FFF;

            s.Read(out mDensity);
            s.Read(out mComponentName, StringType.ZeroDelimited);
            s.Read(out mStart);
            s.Read(out mSourceType);
            s.Read(out mSourceSize);
            mPreTransform = new TransformElement(0, handler, stream);
            mSizeCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mSizeVary);
            mPitchCurve = new DataList<FloatValue>(handler, stream);
            mRollCurve = new DataList<FloatValue>(handler, stream);
            mHeadingCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mPitchVary);
            s.Read(out mRollVary);
            s.Read(out mHeadingVary);
            s.Read(out mPitchOffset);
            s.Read(out mRollOffset);
            s.Read(out mHeadingOffset);
            mColorCurve = new DataList<ColorValue>(handler, stream);
            mColorVary = new ColorValue(requestedApiVersion, handler, stream);
            mAlphaCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mAlphaVary);
            mSurfaces = new DataList<Surface>(handler, stream);
            s.Read(out mEmitMapId);
            s.Read(out mColorMapId);
            s.Read(out mPinMapId);
            mAltitudeRange = new Vector2ValueLE(requestedApiVersion, handler, stream);
            mDrawInfo = new ResourceReference(requestedApiVersion, handler, mSection, stream);
            s.Read(out mOverrideSet);
            s.Read(out mMessageId);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);
            s.Write(mDensity);
            s.Write(mComponentName, StringType.ZeroDelimited);
            s.Write(mStart);
            s.Write(mSourceType);
            s.Write(mSourceSize);
            mPreTransform.UnParse(stream);
            mSizeCurve.UnParse(stream);
            s.Write(mSizeVary);
            mPitchCurve.UnParse(stream);
            mRollCurve.UnParse(stream);
            mHeadingCurve.UnParse(stream);
            s.Write(mPitchVary);
            s.Write(mRollVary);
            s.Write(mHeadingVary);
            s.Write(mPitchOffset);
            s.Write(mRollOffset);
            s.Write(mHeadingOffset);
            mColorCurve.UnParse(stream);
            mColorVary.UnParse(stream);
            mAlphaCurve.UnParse(stream);
            s.Write(mAlphaVary);
            mSurfaces.UnParse(stream);
            s.Write(mEmitMapId);
            s.Write(mColorMapId);
            s.Write(mPinMapId);
            mAltitudeRange.UnParse(stream);
            mDrawInfo.UnParse(stream);
            s.Write(mOverrideSet);
            s.Write(mMessageId);
        }
        #endregion

        public bool Equals(DistributeEffect other)
        {
            return base.Equals(other);
        }
    }
}
