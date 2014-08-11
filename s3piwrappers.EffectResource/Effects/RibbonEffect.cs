using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class RibbonEffect : Effect, IEquatable<RibbonEffect>
    {
        #region Constructors
        public RibbonEffect(int apiVersion, EventHandler handler, RibbonEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public RibbonEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mRibbonLifetime = new Vector2ValueLE(apiVersion, handler);
            mOffsetCurve = new DataList<FloatValue>(handler);
            mWidthCurve = new DataList<FloatValue>(handler);
            mColorCurve = new DataList<ColorValue>(handler);
            mAlphaCurve = new DataList<FloatValue>(handler);
            mLengthColorCurve = new DataList<ColorValue>(handler);
            mLengthAlphaCurve = new DataList<FloatValue>(handler);
            mEdgeColorCurve = new DataList<ColorValue>(handler);
            mEdgeAlphaCurve = new DataList<FloatValue>(handler);
            mStartEdgeAlphaCurve = new DataList<FloatValue>(handler);
            mEndEdgeAlphaCurve = new DataList<FloatValue>(handler);
            mDrawInfo = new ResourceReference(apiVersion, handler, section);
            mDirectionalForcesSum = new Vector3ValueLE(apiVersion, handler);
        }

        public RibbonEffect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private uint mFlags;
        private Vector2ValueLE mRibbonLifetime;//originally uint,uint
        private DataList<FloatValue> mOffsetCurve;
        private DataList<FloatValue> mWidthCurve;
        private float mTaper;
        private float mFade;
        private float mAlphaDecay;
        private DataList<ColorValue> mColorCurve;
        private DataList<FloatValue> mAlphaCurve;
        private DataList<ColorValue> mLengthColorCurve;//originally DataList<Vector3ValueLE>
        private DataList<FloatValue> mLengthAlphaCurve;
        private DataList<ColorValue> mEdgeColorCurve;//originally DataList<Vector3ValueLE>
        private DataList<FloatValue> mEdgeAlphaCurve;
        private DataList<FloatValue> mStartEdgeAlphaCurve;
        private DataList<FloatValue> mEndEdgeAlphaCurve;
        private int mSegmentCount;//originally uint
        private float mSegmentLength;
        private ResourceReference mDrawInfo;
        private int mTileUV; //0xFFFFFFFF; originally uint
        private float mSlipCurveSpeed;
        private float mSlipUVSpeed;

        private float mUVRepeat; //Version 2+

        private Vector3ValueLE mDirectionalForcesSum;

        private float mWindStrength;
        private float mGravityStrength;
        private ulong mEmitColorMapId; //0xFFFFFFFFFFFFFFFF
        private ulong mForceMapId; //0xFFFFFFFFFFFFFFFF
        private float mMapRepulseStrength; //0.0f; originally uint
        #endregion

        #region Content Fields
        [ElementPriority(28)]
        public float MapRepulseStrength
        {
            get { return mMapRepulseStrength; }
            set
            {
                mMapRepulseStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(27)]
        public ulong ForceMapId
        {
            get { return mForceMapId; }
            set
            {
                mForceMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(26)]
        public ulong EmitColorMapId
        {
            get { return mEmitColorMapId; }
            set
            {
                mEmitColorMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(25)]
        public float GravityStrength
        {
            get { return mGravityStrength; }
            set
            {
                mGravityStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(24)]
        public float WindStrength
        {
            get { return mWindStrength; }
            set
            {
                mWindStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(23)]
        public Vector3ValueLE DirectionalForcesSum
        {
            get { return mDirectionalForcesSum; }
            set
            {
                mDirectionalForcesSum = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(22)]
        public float UVRepeat
        {
            get { return mUVRepeat; }
            set
            {
                mUVRepeat = value;
                OnElementChanged();
            }
        }

        [ElementPriority(21)]
        public float SlipUVSpeed
        {
            get { return mSlipUVSpeed; }
            set
            {
                mSlipUVSpeed = value;
                OnElementChanged();
            }
        }

        [ElementPriority(20)]
        public float SlipCurveSpeed
        {
            get { return mSlipCurveSpeed; }
            set
            {
                mSlipCurveSpeed = value;
                OnElementChanged();
            }
        }

        [ElementPriority(19)]
        public int TileUV
        {
            get { return mTileUV; }
            set
            {
                mTileUV = value;
                OnElementChanged();
            }
        }

        [ElementPriority(18)]
        public ResourceReference DrawInfo
        {
            get { return mDrawInfo; }
            set
            {
                mDrawInfo = new ResourceReference(requestedApiVersion, handler, mSection, value);
                OnElementChanged();
            }
        }

        [ElementPriority(17)]
        public float SegmentLength
        {
            get { return mSegmentLength; }
            set
            {
                mSegmentLength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(16)]
        public int SegmentCount
        {
            get { return mSegmentCount; }
            set
            {
                mSegmentCount = value;
                OnElementChanged();
            }
        }

        [ElementPriority(15)]
        public DataList<FloatValue> EndEdgeAlphaCurve
        {
            get { return mEndEdgeAlphaCurve; }
            set
            {
                mEndEdgeAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(14)]
        public DataList<FloatValue> StartEdgeAlphaCurve
        {
            get { return mStartEdgeAlphaCurve; }
            set
            {
                mStartEdgeAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(13)]
        public DataList<FloatValue> EdgeAlphaCurve
        {
            get { return mEdgeAlphaCurve; }
            set
            {
                mEdgeAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public DataList<ColorValue> EdgeColorCurve
        {
            get { return mEdgeColorCurve; }
            set
            {
                mEdgeColorCurve = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public DataList<FloatValue> LengthAlphaCurve
        {
            get { return mLengthAlphaCurve; }
            set
            {
                mLengthAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public DataList<ColorValue> LengthColorCurve
        {
            get { return mLengthColorCurve; }
            set
            {
                mLengthColorCurve = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public DataList<FloatValue> AlphaCurve
        {
            get { return mAlphaCurve; }
            set
            {
                mAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public DataList<ColorValue> ColorCurve
        {
            get { return mColorCurve; }
            set
            {
                mColorCurve = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public float AlphaDecay
        {
            get { return mAlphaDecay; }
            set
            {
                mAlphaDecay = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public float Fade
        {
            get { return mFade; }
            set
            {
                mFade = value;
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public float Taper
        {
            get { return mTaper; }
            set
            {
                mTaper = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public DataList<FloatValue> WidthCurve
        {
            get { return mWidthCurve; }
            set
            {
                mWidthCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public DataList<FloatValue> OffsetCurve
        {
            get { return mOffsetCurve; }
            set
            {
                mOffsetCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public Vector2ValueLE RibbonLifetime
        {
            get { return mRibbonLifetime; }
            set
            {
                mRibbonLifetime = new Vector2ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

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
        #endregion

        public override List<string> ContentFields
        {
            get
            {
                List<string> fields = base.ContentFields;
                if (mSection.Version < 0x0002) fields.Remove("UVRepeat");
                return fields;
            }
        }

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            //mFlags &= 0x3FFF;

            mRibbonLifetime = new Vector2ValueLE(requestedApiVersion, handler, stream);
            mOffsetCurve = new DataList<FloatValue>(handler, stream);
            mWidthCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mTaper);
            s.Read(out mFade);
            s.Read(out mAlphaDecay);
            mColorCurve = new DataList<ColorValue>(handler, stream);
            mAlphaCurve = new DataList<FloatValue>(handler, stream);
            mLengthColorCurve = new DataList<ColorValue>(handler, stream);
            mLengthAlphaCurve = new DataList<FloatValue>(handler, stream);
            mEdgeColorCurve = new DataList<ColorValue>(handler, stream);
            mEdgeAlphaCurve = new DataList<FloatValue>(handler, stream);
            mStartEdgeAlphaCurve = new DataList<FloatValue>(handler, stream);
            mEndEdgeAlphaCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mSegmentCount);
            s.Read(out mSegmentLength);
            mDrawInfo = new ResourceReference(requestedApiVersion, handler, mSection, stream);
            s.Read(out mTileUV);
            s.Read(out mSlipCurveSpeed);
            s.Read(out mSlipUVSpeed);

            if (mSection.Version >= 0x0002)
            {
                s.Read(out mUVRepeat);
            }
            else
            {
                mUVRepeat = 1.0f;
            }

            mDirectionalForcesSum = new Vector3ValueLE(requestedApiVersion, handler, stream);

            s.Read(out mWindStrength);
            s.Read(out mGravityStrength);
            s.Read(out mEmitColorMapId);
            s.Read(out mForceMapId);
            s.Read(out mMapRepulseStrength);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);
            mRibbonLifetime.UnParse(stream);
            mOffsetCurve.UnParse(stream);
            mWidthCurve.UnParse(stream);
            s.Write(mTaper);
            s.Write(mFade);
            s.Write(mAlphaDecay);
            mColorCurve.UnParse(stream);
            mAlphaCurve.UnParse(stream);
            mLengthColorCurve.UnParse(stream);
            mLengthAlphaCurve.UnParse(stream);
            mEdgeColorCurve.UnParse(stream);
            mEdgeAlphaCurve.UnParse(stream);
            mStartEdgeAlphaCurve.UnParse(stream);
            mEndEdgeAlphaCurve.UnParse(stream);
            s.Write(mSegmentCount);
            s.Write(mSegmentLength);
            mDrawInfo.UnParse(stream);
            s.Write(mTileUV);
            s.Write(mSlipCurveSpeed);
            s.Write(mSlipUVSpeed);

            if (mSection.Version >= 0x0002)
            {
                s.Write(mUVRepeat);
            }

            mDirectionalForcesSum.UnParse(stream);

            s.Write(mWindStrength);
            s.Write(mGravityStrength);
            s.Write(mEmitColorMapId);
            s.Write(mForceMapId);
            s.Write(mMapRepulseStrength);
        }
        #endregion

        public bool Equals(RibbonEffect other)
        {
            return base.Equals(other);
        }
    }
}
