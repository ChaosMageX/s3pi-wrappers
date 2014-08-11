using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class MetaparticleEffect : Effect, IEquatable<MetaparticleEffect>
    {
        #region Constructors
        public MetaparticleEffect(int apiVersion, EventHandler handler, MetaparticleEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public MetaparticleEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mParticleParameters = new ParticleParams(apiVersion, handler, true);
            mRateCurve = new DataList<FloatValue>(handler);
            mSizeCurve = new DataList<FloatValue>(handler);
            mPitchCurve = new DataList<FloatValue>(handler);
            mRollCurve = new DataList<FloatValue>(handler);
            mHeadingCurve = new DataList<FloatValue>(handler);
            mRollOffset = -1000000000.0f;
            mHeadingOffset = 0f;
            mColorCurve = new DataList<ColorValue>(handler);
            mColorVary = new ColorValue(apiVersion, handler);
            mAlphaCurve = new DataList<FloatValue>(handler);
            mDirectionalForcesSum = new Vector3ValueLE(apiVersion, handler);
            mGlobalForcesSum = new Vector3ValueLE(apiVersion, handler);
            mRadialForceLocation = new Vector3ValueLE(apiVersion, handler);
            mWiggles = new DataList<Wiggle>(handler);
            mLoopBoxColorCurve = new DataList<ColorValue>(handler);
            mLoopBoxAlphaCurve = new DataList<FloatValue>(handler);
            mSurfaces = new DataList<Surface>(handler);
            mAltitudeRange = new Vector2ValueLE(apiVersion, handler, -10000.0f, 10000.0f);
            mRandomWalk1 = new RandomWalk(apiVersion, handler);
            mRandomWalk2 = new RandomWalk(apiVersion, handler);
            mRandomWalkPreferredDirection = new Vector3ValueLE(apiVersion, handler);
            mAttractorOrigin = new Vector3ValueLE(apiVersion, handler);
            mAttractor = new Attractor(apiVersion, handler);
            mPathPoints = new DataList<PathPoint>(handler);
        }

        public MetaparticleEffect(int apiVersion, EventHandler handler, ISection section, Stream s) 
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private ulong mFlags;
        private ParticleParams mParticleParameters;
        private DataList<FloatValue> mRateCurve;
        private float mRateCurveTime;
        private float mRateCurveCycles;//originally uint
        private DataList<FloatValue> mSizeCurve;
        private float mSizeVary;
        private DataList<FloatValue> mPitchCurve;
        private DataList<FloatValue> mRollCurve;
        private DataList<FloatValue> mHeadingCurve;
        private float mPitchVary;
        private float mRollVary;
        private float mHeadingVary;
        private float mPitchOffset;
        private float mRollOffset;
        private float mHeadingOffset;
        private DataList<ColorValue> mColorCurve;
        private ColorValue mColorVary;
        private DataList<FloatValue> mAlphaCurve;
        private float mAlphaVary;
        private string mComponentName = string.Empty;
        private string mComponentType = string.Empty;
        private byte mAlignMode;
        private Vector3ValueLE mDirectionalForcesSum;
        private Vector3ValueLE mGlobalForcesSum;

        private float mWindStrength;
        private float mGravityStrength;
        private float mRadialForce;

        private Vector3ValueLE mRadialForceLocation;

        private float mDrag;
        private float mScrewRate;
        private DataList<Wiggle> mWiggles;
        private byte mScreenBloomAlphaRate;
        private byte mScreenBloomAlphaBase;
        private byte mScreenBloomSizeRate;
        private byte mScreenBloomSizeBase;
        private DataList<ColorValue> mLoopBoxColorCurve;//originally DataList<Vector3ValueLE>
        private DataList<FloatValue> mLoopBoxAlphaCurve;
        private DataList<Surface> mSurfaces; //25
        private float mMapBounce;
        private float mMapRepulseHeight;
        private float mMapRepulseStrength;
        private float mMapRepulseScoutDistance;
        private float mMapRepulseVertical;
        private float mMapRepulseKillHeight;
        private float mProbabilityDeath;
        private Vector2ValueLE mAltitudeRange;
        private ulong mForceMapId;
        private ulong mEmitRateMapId;
        private ulong mEmitColorMapId;

        private RandomWalk mRandomWalk1;
        private RandomWalk mRandomWalk2;

        private Vector3ValueLE mRandomWalkPreferredDirection;

        private float mAlignDamping;
        private float mBankAmount;
        private float mBankRestore;

        private Vector3ValueLE mAttractorOrigin;

        private Attractor mAttractor;
        private DataList<PathPoint> mPathPoints;
        private float mTractorResetSpeed;
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public ulong Flags
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
                mParticleParameters = new ParticleParams(requestedApiVersion, handler, true, value);
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
        public float RateCurveCycles
        {
            get { return mRateCurveCycles; }
            set
            {
                mRateCurveCycles = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public DataList<FloatValue> SizeCurve
        {
            get { return mSizeCurve; }
            set
            {
                mSizeCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public float SizeVary
        {
            get { return mSizeVary; }
            set
            {
                mSizeVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public DataList<FloatValue> PitchCurve
        {
            get { return mPitchCurve; }
            set
            {
                mPitchCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public DataList<FloatValue> RollCurve
        {
            get { return mRollCurve; }
            set
            {
                mRollCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public DataList<FloatValue> HeadingCurve
        {
            get { return mHeadingCurve; }
            set
            {
                mHeadingCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public float PitchVary
        {
            get { return mPitchVary; }
            set
            {
                mPitchVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public float RollVary
        {
            get { return mRollVary; }
            set
            {
                mRollVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(13)]
        public float HeadingVary
        {
            get { return mHeadingVary; }
            set
            {
                mHeadingVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(14)]
        public float PitchOffset
        {
            get { return mPitchOffset; }
            set
            {
                mPitchOffset = value;
                OnElementChanged();
            }
        }

        [ElementPriority(15)]
        public float RollOffset
        {
            get { return mRollOffset; }
            set
            {
                mRollOffset = value;
                OnElementChanged();
            }
        }

        [ElementPriority(16)]
        public float HeadingOffset
        {
            get { return mHeadingOffset; }
            set
            {
                mHeadingOffset = value;
                OnElementChanged();
            }
        }

        [ElementPriority(17)]
        public DataList<ColorValue> ColorCurve
        {
            get { return mColorCurve; }
            set
            {
                mColorCurve = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(18)]
        public ColorValue ColorVary
        {
            get { return mColorVary; }
            set
            {
                mColorVary = new ColorValue(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(19)]
        public DataList<FloatValue> AlphaCurve
        {
            get { return mAlphaCurve; }
            set
            {
                mAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(20)]
        public float AlphaVary
        {
            get { return mAlphaVary; }
            set
            {
                mAlphaVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(21)]
        public string ComponentName
        {
            get { return mComponentName; }
            set
            {
                mComponentName = value ?? string.Empty;
                OnElementChanged();
            }
        }

        [ElementPriority(22)]
        public string ComponentType
        {
            get { return mComponentType; }
            set
            {
                mComponentType = value ?? string.Empty;
                OnElementChanged();
            }
        }

        [ElementPriority(23)]
        public byte AlignMode
        {
            get { return mAlignMode; }
            set
            {
                mAlignMode = value;
                OnElementChanged();
            }
        }

        [ElementPriority(24)]
        public Vector3ValueLE DirectionalForcesSum
        {
            get { return mDirectionalForcesSum; }
            set
            {
                mDirectionalForcesSum = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(25)]
        public Vector3ValueLE GlobalForcesSum
        {
            get { return mGlobalForcesSum; }
            set
            {
                mGlobalForcesSum = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(26)]
        public float WindStrength
        {
            get { return mWindStrength; }
            set
            {
                mWindStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(27)]
        public float GravityStrength
        {
            get { return mGravityStrength; }
            set
            {
                mGravityStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(28)]
        public float RadialForce
        {
            get { return mRadialForce; }
            set
            {
                mRadialForce = value;
                OnElementChanged();
            }
        }

        [ElementPriority(29)]
        public Vector3ValueLE RadialForceDirection
        {
            get { return mRadialForceLocation; }
            set
            {
                mRadialForceLocation = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(30)]
        public float Drag
        {
            get { return mDrag; }
            set
            {
                mDrag = value;
                OnElementChanged();
            }
        }

        [ElementPriority(31)]
        public float ScrewRate
        {
            get { return mScrewRate; }
            set
            {
                mScrewRate = value;
                OnElementChanged();
            }
        }

        [ElementPriority(32)]
        public DataList<Wiggle> Wiggles
        {
            get { return mWiggles; }
            set
            {
                mWiggles = new DataList<Wiggle>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(33)]
        public byte ScreenBloomAlphaRate
        {
            get { return mScreenBloomAlphaRate; }
            set
            {
                mScreenBloomAlphaRate = value;
                OnElementChanged();
            }
        }

        [ElementPriority(34)]
        public byte ScreenBloomAlphaBase
        {
            get { return mScreenBloomAlphaBase; }
            set
            {
                mScreenBloomAlphaBase = value;
                OnElementChanged();
            }
        }

        [ElementPriority(35)]
        public byte ScreenBloomSizeRate
        {
            get { return mScreenBloomSizeRate; }
            set
            {
                mScreenBloomSizeRate = value;
                OnElementChanged();
            }
        }

        [ElementPriority(36)]
        public byte ScreenBloomSizeBase
        {
            get { return mScreenBloomSizeBase; }
            set
            {
                mScreenBloomSizeBase = value;
                OnElementChanged();
            }
        }

        [ElementPriority(37)]
        public DataList<ColorValue> LoopBoxColorCurve
        {
            get { return mLoopBoxColorCurve; }
            set
            {
                mLoopBoxColorCurve = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(38)]
        public DataList<FloatValue> LoopBoxAlphaCurve
        {
            get { return mLoopBoxAlphaCurve; }
            set
            {
                mLoopBoxAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(39)]
        public DataList<Surface> Surfaces
        {
            get { return mSurfaces; }
            set
            {
                mSurfaces = new DataList<Surface>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(40)]
        public float MapBounce
        {
            get { return mMapBounce; }
            set
            {
                mMapBounce = value;
                OnElementChanged();
            }
        }

        [ElementPriority(41)]
        public float MapRepulseHeight
        {
            get { return mMapRepulseHeight; }
            set
            {
                mMapRepulseHeight = value;
                OnElementChanged();
            }
        }

        [ElementPriority(42)]
        public float MapRepulseStrength
        {
            get { return mMapRepulseStrength; }
            set
            {
                mMapRepulseStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(43)]
        public float MapRepulseScoutDistance
        {
            get { return mMapRepulseScoutDistance; }
            set
            {
                mMapRepulseScoutDistance = value;
                OnElementChanged();
            }
        }

        [ElementPriority(44)]
        public float MapRepulseVertical
        {
            get { return mMapRepulseVertical; }
            set
            {
                mMapRepulseVertical = value;
                OnElementChanged();
            }
        }

        [ElementPriority(45)]
        public float MapRepulseKillHeight
        {
            get { return mMapRepulseKillHeight; }
            set
            {
                mMapRepulseKillHeight = value;
                OnElementChanged();
            }
        }

        [ElementPriority(46)]
        public float ProbabilityDeath
        {
            get { return mProbabilityDeath; }
            set
            {
                mProbabilityDeath = value;
                OnElementChanged();
            }
        }

        [ElementPriority(47)]
        public Vector2ValueLE AltitudeRange
        {
            get { return mAltitudeRange; }
            set
            {
                mAltitudeRange = new Vector2ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(48)]
        public ulong ForceMapId
        {
            get { return mForceMapId; }
            set
            {
                mForceMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(49)]
        public ulong EmitRateMap
        {
            get { return mEmitRateMapId; }
            set
            {
                mEmitRateMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(50)]
        public ulong EmitColorMap
        {
            get { return mEmitColorMapId; }
            set
            {
                mEmitColorMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(51)]
        public RandomWalk RandomWalk1
        {
            get { return mRandomWalk1; }
            set
            {
                mRandomWalk1 = new RandomWalk(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(52)]
        public RandomWalk RandomWalk2
        {
            get { return mRandomWalk2; }
            set
            {
                mRandomWalk2 = new RandomWalk(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(53)]
        public Vector3ValueLE RandomWalkPreferredDirection
        {
            get { return mRandomWalkPreferredDirection; }
            set
            {
                mRandomWalkPreferredDirection = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(54)]
        public float AlignDamping
        {
            get { return mAlignDamping; }
            set
            {
                mAlignDamping = value;
                OnElementChanged();
            }
        }

        [ElementPriority(55)]
        public float BankAmount
        {
            get { return mBankAmount; }
            set
            {
                mBankAmount = value;
                OnElementChanged();
            }
        }

        [ElementPriority(56)]
        public float BankRestore
        {
            get { return mBankRestore; }
            set
            {
                mBankRestore = value;
                OnElementChanged();
            }
        }

        [ElementPriority(57)]
        public Vector3ValueLE AttractorOrigin
        {
            get { return mAttractorOrigin; }
            set
            {
                mAttractorOrigin = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(58)]
        public Attractor Attractor
        {
            get { return mAttractor; }
            set
            {
                mAttractor = new Attractor(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(74)]
        public DataList<PathPoint> PathPoints
        {
            get { return mPathPoints; }
            set
            {
                mPathPoints = new DataList<PathPoint>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(75)]
        public float TractorResetSpeed
        {
            get { return mTractorResetSpeed; }
            set
            {
                mTractorResetSpeed = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            // no flag mask

            mParticleParameters = new ParticleParams(requestedApiVersion, handler, true, stream);
            mRateCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mRateCurveTime);
            s.Read(out mRateCurveCycles);
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
            s.Read(out mComponentName, StringType.ZeroDelimited);
            s.Read(out mComponentType, StringType.ZeroDelimited);
            s.Read(out mAlignMode);
            mDirectionalForcesSum = new Vector3ValueLE(requestedApiVersion, handler, stream);
            mGlobalForcesSum = new Vector3ValueLE(requestedApiVersion, handler, stream);

            s.Read(out mWindStrength);
            s.Read(out mGravityStrength);
            s.Read(out mRadialForce);

            mRadialForceLocation = new Vector3ValueLE(requestedApiVersion, handler, stream);

            s.Read(out mDrag);
            s.Read(out mScrewRate);
            mWiggles = new DataList<Wiggle>(handler, stream);
            s.Read(out mScreenBloomAlphaRate);
            s.Read(out mScreenBloomAlphaBase);
            s.Read(out mScreenBloomSizeRate);
            s.Read(out mScreenBloomSizeBase);
            mLoopBoxColorCurve = new DataList<ColorValue>(handler, stream);
            mLoopBoxAlphaCurve = new DataList<FloatValue>(handler, stream);
            mSurfaces = new DataList<Surface>(handler, stream);
            s.Read(out mMapBounce);
            s.Read(out mMapRepulseHeight);
            s.Read(out mMapRepulseStrength);
            s.Read(out mMapRepulseScoutDistance);
            s.Read(out mMapRepulseVertical);
            s.Read(out mMapRepulseKillHeight);
            s.Read(out mProbabilityDeath);

            mAltitudeRange = new Vector2ValueLE(requestedApiVersion, handler, stream);

            s.Read(out mForceMapId);
            s.Read(out mEmitRateMapId);
            s.Read(out mEmitColorMapId);

            mRandomWalk1 = new RandomWalk(0, handler, stream);
            mRandomWalk2 = new RandomWalk(0, handler, stream);

            mRandomWalkPreferredDirection = new Vector3ValueLE(requestedApiVersion, handler, stream);

            s.Read(out mAlignDamping);
            s.Read(out mBankAmount);
            s.Read(out mBankRestore);

            mAttractorOrigin = new Vector3ValueLE(requestedApiVersion, handler, stream);
            mAttractor = new Attractor(requestedApiVersion, handler, stream);

            mPathPoints = new DataList<PathPoint>(handler, stream);
            s.Read(out mTractorResetSpeed);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);

            s.Write(mFlags);
            mParticleParameters.UnParse(stream);
            mRateCurve.UnParse(stream);
            s.Write(mRateCurveTime);
            s.Write(mRateCurveCycles);
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
            s.Write(mComponentName, StringType.ZeroDelimited);
            s.Write(mComponentType, StringType.ZeroDelimited);
            s.Write(mAlignMode);
            mDirectionalForcesSum.UnParse(stream);
            mGlobalForcesSum.UnParse(stream);

            s.Write(mWindStrength);
            s.Write(mGravityStrength);
            s.Write(mRadialForce);

            mRadialForceLocation.UnParse(stream);

            s.Write(mDrag);
            s.Write(mScrewRate);
            mWiggles.UnParse(stream);
            s.Write(mScreenBloomAlphaRate);
            s.Write(mScreenBloomAlphaBase);
            s.Write(mScreenBloomSizeRate);
            s.Write(mScreenBloomSizeBase);
            mLoopBoxColorCurve.UnParse(stream);
            mLoopBoxAlphaCurve.UnParse(stream);
            mSurfaces.UnParse(stream);
            s.Write(mMapBounce);
            s.Write(mMapRepulseHeight);
            s.Write(mMapRepulseStrength);
            s.Write(mMapRepulseScoutDistance);
            s.Write(mMapRepulseVertical);
            s.Write(mMapRepulseKillHeight);
            s.Write(mProbabilityDeath);

            mAltitudeRange.UnParse(stream);

            s.Write(mForceMapId);
            s.Write(mEmitRateMapId);
            s.Write(mEmitColorMapId);

            mRandomWalk1.UnParse(stream);
            mRandomWalk2.UnParse(stream);

            mRandomWalkPreferredDirection.UnParse(stream);

            s.Write(mAlignDamping);
            s.Write(mBankAmount);
            s.Write(mBankRestore);

            mAttractorOrigin.UnParse(stream);
            mAttractor.UnParse(stream);

            mPathPoints.UnParse(stream);
            s.Write(mTractorResetSpeed);
        }
        #endregion

        public bool Equals(MetaparticleEffect other)
        {
            return base.Equals(other);
        }
    }
}
