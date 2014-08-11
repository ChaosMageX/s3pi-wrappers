using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class FastParticleEffect : Effect, IEquatable<FastParticleEffect>
    {
        #region Constructors
        public FastParticleEffect(int apiVersion, EventHandler handler, FastParticleEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public FastParticleEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mParticleParameters = new ParticleParams(apiVersion, handler, false);
            mRateCurve = new DataList<FloatValue>(handler);
            mSizeCurve = new DataList<FloatValue>(handler);
            mColorCurve = new DataList<ColorValue>(handler);
            mAlphaCurve = new DataList<FloatValue>(handler);
            mDrawInfo = new ResourceReference(apiVersion, handler, section);
            mDirectionalForcesSum = new Vector3ValueLE(apiVersion, handler);
            mRadialForceLocation = new Vector3ValueLE(apiVersion, handler);
        }

        public FastParticleEffect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private uint mFlags;
        private ParticleParams mParticleParameters;
        private DataList<FloatValue> mRateCurve;
        private float mRateCurveTime;
        private ushort mRateCurveCycles;
        private float mRateSpeedScale;
        private DataList<FloatValue> mSizeCurve;
        private DataList<ColorValue> mColorCurve;
        private DataList<FloatValue> mAlphaCurve;
        private ResourceReference mDrawInfo;
        private byte mAlignMode;
        private Vector3ValueLE mDirectionalForcesSum;
        private float mWindStrength;
        private float mGravityStrength;
        private float mRadialForce;
        private Vector3ValueLE mRadialForceLocation;
        private float mDrag;
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
        public ParticleParams ParticleParameters
        {
            get { return mParticleParameters; }
            set
            {
                mParticleParameters = new ParticleParams(requestedApiVersion, handler, false, value);
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public DataList<FloatValue> RateCurve
        {
            get { return mRateCurve; }
            set
            {
                mRateCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public float RateCurveTime
        {
            get { return mRateCurveTime; }
            set
            {
                mRateCurveTime = value;
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public ushort RateCurveCycles
        {
            get { return mRateCurveCycles; }
            set
            {
                mRateCurveCycles = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public float RateSpeedScale
        {
            get { return mRateSpeedScale; }
            set
            {
                mRateSpeedScale = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public DataList<FloatValue> SizeCurve
        {
            get { return mSizeCurve; }
            set
            {
                mSizeCurve = new DataList<FloatValue>(handler, value);
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

        [ElementPriority(10)]
        public ResourceReference DrawInfo
        {
            get { return mDrawInfo; }
            set
            {
                mDrawInfo = new ResourceReference(requestedApiVersion, handler, mSection, value);
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public byte AlignMode
        {
            get { return mAlignMode; }
            set
            {
                mAlignMode = value;
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public Vector3ValueLE DirectionalForcesSum
        {
            get { return mDirectionalForcesSum; }
            set
            {
                mDirectionalForcesSum = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(13)]
        public float WindStrength
        {
            get { return mWindStrength; }
            set
            {
                mWindStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(14)]
        public float GravityStrength
        {
            get { return mGravityStrength; }
            set
            {
                mGravityStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(15)]
        public float RadialForce
        {
            get { return mRadialForce; }
            set
            {
                mRadialForce = value;
                OnElementChanged();
            }
        }

        [ElementPriority(16)]
        public Vector3ValueLE RadialForceLocation
        {
            get { return mRadialForceLocation; }
            set
            {
                mRadialForceLocation = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(17)]
        public float Drag
        {
            get { return mDrag; }
            set
            {
                mDrag = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            //mFlags &= 0x1FFF;

            mParticleParameters = new ParticleParams(requestedApiVersion, handler, false, stream);
            mRateCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mRateCurveTime);
            s.Read(out mRateCurveCycles);
            s.Read(out mRateSpeedScale);
            mSizeCurve = new DataList<FloatValue>(handler, stream);
            mColorCurve = new DataList<ColorValue>(handler, stream);
            mAlphaCurve = new DataList<FloatValue>(handler, stream);
            mDrawInfo = new ResourceReference(requestedApiVersion, handler, mSection, stream);
            s.Read(out mAlignMode);
            mDirectionalForcesSum = new Vector3ValueLE(requestedApiVersion, handler, stream);
            s.Read(out mWindStrength);
            s.Read(out mGravityStrength);
            s.Read(out mRadialForce);
            mRadialForceLocation = new Vector3ValueLE(requestedApiVersion, handler, stream);
            s.Read(out mDrag);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);

            mParticleParameters.UnParse(stream);
            mRateCurve.UnParse(stream);
            s.Write(mRateCurveTime);
            s.Write(mRateCurveCycles);
            s.Write(mRateSpeedScale);
            mSizeCurve.UnParse(stream);
            mColorCurve.UnParse(stream);
            mAlphaCurve.UnParse(stream);
            mDrawInfo.UnParse(stream);
            s.Write(mAlignMode);
            mDirectionalForcesSum.UnParse(stream);
            s.Write(mWindStrength);
            s.Write(mGravityStrength);
            s.Write(mRadialForce);
            mRadialForceLocation.UnParse(stream);
            s.Write(mDrag);
        }
        #endregion

        public bool Equals(FastParticleEffect other)
        {
            return base.Equals(other);
        }
    }
}
