using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class ParticleEffect : Effect, IEquatable<ParticleEffect>
    {
        private bool isTheSims4 = false;

        #region Constructors
        public ParticleEffect(int apiVersion, EventHandler handler, ParticleEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public ParticleEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mParticleParameters = new ParticleParams(apiVersion, handler, true);
            mRateCurve = new DataList<FloatValue>(handler);
            mSizeCurve = new DataList<FloatValue>(handler);
            mAspectCurve = new DataList<FloatValue>(handler);
            mRotationCurve = new DataList<FloatValue>(handler);
            mAlphaCurve = new DataList<FloatValue>(handler);
            mColorCurve = new DataList<ColorValue>(handler);
            mColorVary = new ColorValue(apiVersion, handler);
            mDrawInfo = new ResourceReference(apiVersion, handler, section);
            mDirectionalForcesSum = new Vector3ValueLE(apiVersion, handler);
            mRadialForceLocation = new Vector3ValueLE(apiVersion, handler);
            mWiggles = new DataList<Wiggle>(handler);
            mLoopBoxColorCurve = new DataList<ColorValue>(handler);
            mLoopBoxAlphaCurve = new DataList<FloatValue>(handler);
            mSurfaces = new DataList<Surface>(handler);
            mAltitudeRange = new Vector2ValueLE(apiVersion, handler, -10000.0f, 10000.0f);
            mRandomWalk = new RandomWalk(apiVersion, handler);
            mAttractorOrigin = new Vector3ValueLE(apiVersion, handler);
            mAttractor = new Attractor(apiVersion, handler);
            mPathPoints = new DataList<PathPoint>(handler);
            mVector01 = new Vector3ValueLE(apiVersion, handler);
            mVector3List01 = new DataList<Vector3ValueLE>(handler);
            mVector02 = new Vector3ValueLE(apiVersion, handler);
            mUnknown140 = new Vector2ValueLE(apiVersion, handler);
            mUnknown150 = new Vector2ValueLE(apiVersion, handler);
            mUnknown180 = new Vector3ValueLE(apiVersion, handler);
            mUnknown190 = new Vector2ValueLE(apiVersion, handler);
            mUnknown1A0 = new Vector2ValueLE(apiVersion, handler);
        }

        public ParticleEffect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private uint mFlags;
        private ParticleParams mParticleParameters;
        private DataList<FloatValue> mRateCurve;
        private float mRateCurveTime = 1.5f;
        private ushort mRateCurveCycles = 1;
        private float mRateSpeedScale;
        private DataList<FloatValue> mSizeCurve;
        private float mSizeVary;
        private DataList<FloatValue> mAspectCurve;
        private float mAspectVary;
        private float mRotationVary;
        private float mRotationOffset;
        private DataList<FloatValue> mRotationCurve;
        private DataList<FloatValue> mAlphaCurve;
        private float mAlphaVary;
        private DataList<ColorValue> mColorCurve;
        private ColorValue mColorVary;
        private ResourceReference mDrawInfo;

        private byte mPhysicsType;
        private byte mOverrideSet;
        private byte mTileCountU;
        private byte mTileCountV;
        private byte mAlignMode;
        private float mFrameSpeed;
        private byte mFrameStart;
        private byte mFrameCount;
        private byte mFrameRandom;

        private Vector3ValueLE mDirectionalForcesSum;

        private float mWindStrength;
        private float mGravityStrength;
        private float mRadialForce;

        private Vector3ValueLE mRadialForceLocation;

        private float mDrag;
        private float mVelocityStretch;
        private float mScrewRate;

        private DataList<Wiggle> mWiggles;
        private byte mScreenBloomAlphaRate;
        private byte mScreenBloomAlphaBase;
        private byte mScreenBloomSizeRate;
        private byte mScreenBloomSizeBase;
        private DataList<ColorValue> mLoopBoxColorCurve;//originally DataList<Vector3ValueLE>
        private DataList<FloatValue> mLoopBoxAlphaCurve;
        private DataList<Surface> mSurfaces;
        private float mMapBounce;
        private float mMapRepulseHeight;
        private float mMapRepulseStrength;
        private float mMapRepulseScoutDistance;
        private float mMapRepulseVertical;
        private float mMapRepulseKillHeight = -1000000000.0f;
        private float mProbabilityDeath;
        private Vector2ValueLE mAltitudeRange;

        private ulong mForceMapId;
        private ulong mEmitRateMapId;
        private ulong mEmitColorMapId;

        private RandomWalk mRandomWalk;

        private Vector3ValueLE mAttractorOrigin;

        private Attractor mAttractor;

        private DataList<PathPoint> mPathPoints;

        //Version 2+
        private Vector3ValueLE mVector01;
        private DataList<Vector3ValueLE> mVector3List01;

        //Version 3+
        private byte mByte01;

        //Version 4+
        private float mFloat01;//uint in Rick's wrapper

        //Version 5+
        private Vector3ValueLE mVector02;

        //The Sims 4 && Version 5+
        private byte mUnknown170;

        //The Sims 4 && Version 6+
        private Vector2ValueLE mUnknown140;

        //The Sims 4 && Version 7+
        private Vector2ValueLE mUnknown150;

        //The Sims 4
        private uint mUnknown160;
        private uint mUnknown164;
        private byte mUnknown1C1;
        private Vector3ValueLE mUnknown180;
        private byte mUnknown1C0;
        private Vector2ValueLE mUnknown190;

        //The Sims 4 && Version 7+
        private Vector2ValueLE mUnknown1A0;

        //The Sims 4
        private uint mUnknown1B0;
        private uint mUnknown1B4;

        //The Sims 4 && Version 6+
        private byte mUnknown208;
        private uint mUnknown20C;
        private byte mUnknown209;
        private byte mUnknown20A;
        private byte mUnknown20B;
        #endregion

        #region Content Fields
        [ElementPriority(80)]
        public byte Unknown20B
        {
            get { return mUnknown20B; }
            set
            {
                mUnknown20B = value;
                OnElementChanged();
            }
        }

        [ElementPriority(79)]
        public byte Unknown20A
        {
            get { return mUnknown20A; }
            set
            {
                mUnknown20A = value;
                OnElementChanged();
            }
        }

        [ElementPriority(78)]
        public byte Unknown209
        {
            get { return mUnknown209; }
            set
            {
                mUnknown209 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(77)]
        public uint Unknown20C
        {
            get { return mUnknown20C; }
            set
            {
                mUnknown20C = value;
                OnElementChanged();
            }
        }

        [ElementPriority(76)]
        public bool Unknown208
        {
            get { return mUnknown208 != 0; }
            set
            {
                mUnknown208 = (byte)(value ? 0xFF : 0x00);
                OnElementChanged();
            }
        }

        [ElementPriority(75)]
        public uint Unknown1B4
        {
            get { return mUnknown1B4; }
            set
            {
                mUnknown1B4 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(74)]
        public uint Unknown1B0
        {
            get { return mUnknown1B0; }
            set
            {
                mUnknown1B0 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(73)]
        public Vector2ValueLE Unknown1A0
        {
            get { return mUnknown1A0; }
            set
            {
                mUnknown1A0 = new Vector2ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(72)]
        public Vector2ValueLE Unknown190
        {
            get { return mUnknown190; }
            set
            {
                mUnknown190 = new Vector2ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(71)]
        public byte Unknown1C0
        {
            get { return mUnknown1C0; }
            set
            {
                mUnknown1C0 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(70)]
        public Vector3ValueLE Unknown180
        {
            get { return mUnknown180; }
            set
            {
                mUnknown180 = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(69)]
        public bool Unknown1C1
        {
            get { return mUnknown1C1 != 0; }
            set
            {
                mUnknown1C1 = (byte)(value ? 0xFF : 0x00);
                OnElementChanged();
            }
        }

        [ElementPriority(68)]
        public uint Unknown164
        {
            get { return mUnknown164; }
            set
            {
                mUnknown164 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(67)]
        public uint Unknown160
        {
            get { return mUnknown160; }
            set
            {
                mUnknown160 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(66)]
        public Vector2ValueLE Unknown150
        {
            get { return mUnknown150; }
            set
            {
                mUnknown150 = new Vector2ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(65)]
        public Vector2ValueLE Unknown140
        {
            get { return mUnknown140; }
            set
            {
                mUnknown140 = new Vector2ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(64)]
        public byte Byte02
        {
            get { return mUnknown170; }
            set
            {
                mUnknown170 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(63)]
        public Vector3ValueLE Vector02
        {
            get { return mVector02; }
            set
            {
                mVector02 = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(62)]
        public float Float01
        {
            get { return mFloat01; }
            set
            {
                mFloat01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(61)]
        public byte Byte01
        {
            get { return mByte01; }
            set
            {
                mByte01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(60)]
        public DataList<Vector3ValueLE> Vector3List01
        {
            get { return mVector3List01; }
            set
            {
                mVector3List01 = new DataList<Vector3ValueLE>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(59)]
        public Vector3ValueLE Vector01
        {
            get { return mVector01; }
            set
            {
                mVector01 = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(58)]
        public DataList<PathPoint> PathPoints
        {
            get { return mPathPoints; }
            set
            {
                mPathPoints = new DataList<PathPoint>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(57)]
        public Attractor Attractor
        {
            get { return mAttractor; }
            set
            {
                mAttractor = new Attractor(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(56)]
        public Vector3ValueLE AttractorOrigin
        {
            get { return mAttractorOrigin; }
            set
            {
                mAttractorOrigin = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(55)]
        public RandomWalk RandomWalk
        {
            get { return mRandomWalk; }
            set
            {
                mRandomWalk = new RandomWalk(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(54)]
        public ulong EmitColorMapId
        {
            get { return mEmitColorMapId; }
            set
            {
                mEmitColorMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(53)]
        public ulong EmitRateMapId
        {
            get { return mEmitRateMapId; }
            set
            {
                mEmitRateMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(52)]
        public ulong ForceMapId
        {
            get { return mForceMapId; }
            set
            {
                mForceMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(51)]
        public Vector2ValueLE AltitudeRange
        {
            get { return mAltitudeRange; }
            set
            {
                mAltitudeRange = new Vector2ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(50)]
        public float ProbabilityDeath
        {
            get { return mProbabilityDeath; }
            set
            {
                mProbabilityDeath = value;
                OnElementChanged();
            }
        }

        [ElementPriority(49)]
        public float MapRepulseKillHeight
        {
            get { return mMapRepulseKillHeight; }
            set
            {
                mMapRepulseKillHeight = value;
                OnElementChanged();
            }
        }

        [ElementPriority(48)]
        public float MapRepulseVertical
        {
            get { return mMapRepulseVertical; }
            set
            {
                mMapRepulseVertical = value;
                OnElementChanged();
            }
        }

        [ElementPriority(47)]
        public float MapRepulseScoutDistance
        {
            get { return mMapRepulseScoutDistance; }
            set
            {
                mMapRepulseScoutDistance = value;
                OnElementChanged();
            }
        }

        [ElementPriority(46)]
        public float MapRepulseStrength
        {
            get { return mMapRepulseStrength; }
            set
            {
                mMapRepulseStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(45)]
        public float MapRepulseHeight
        {
            get { return mMapRepulseHeight; }
            set
            {
                mMapRepulseHeight = value;
                OnElementChanged();
            }
        }

        [ElementPriority(44)]
        public float MapBounce
        {
            get { return mMapBounce; }
            set
            {
                mMapBounce = value;
                OnElementChanged();
            }
        }

        [ElementPriority(43)]
        public DataList<Surface> Surfaces
        {
            get { return mSurfaces; }
            set
            {
                mSurfaces = new DataList<Surface>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(42)]
        public DataList<FloatValue> LoopBoxAlphaCurve
        {
            get { return mLoopBoxAlphaCurve; }
            set
            {
                mLoopBoxAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(41)]
        public DataList<ColorValue> LoopBoxColorCurve
        {
            get { return mLoopBoxColorCurve; }
            set
            {
                mLoopBoxColorCurve = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(40)]
        public byte ScreenBloomSizeBase
        {
            get { return mScreenBloomSizeBase; }
            set
            {
                mScreenBloomSizeBase = value;
                OnElementChanged();
            }
        }

        [ElementPriority(39)]
        public byte ScreenBloomSizeRate
        {
            get { return mScreenBloomSizeRate; }
            set
            {
                mScreenBloomSizeRate = value;
                OnElementChanged();
            }
        }

        [ElementPriority(38)]
        public byte ScreenBloomAlphaBase
        {
            get { return mScreenBloomAlphaBase; }
            set
            {
                mScreenBloomAlphaBase = value;
                OnElementChanged();
            }
        }

        [ElementPriority(37)]
        public byte ScreenBloomAlphaRate
        {
            get { return mScreenBloomAlphaRate; }
            set
            {
                mScreenBloomAlphaRate = value;
                OnElementChanged();
            }
        }

        [ElementPriority(36)]
        public DataList<Wiggle> Wiggles
        {
            get { return mWiggles; }
            set
            {
                mWiggles = new DataList<Wiggle>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(35)]
        public float ScrewRate
        {
            get { return mScrewRate; }
            set
            {
                mScrewRate = value;
                OnElementChanged();
            }
        }

        [ElementPriority(34)]
        public float VelocityStretch
        {
            get { return mVelocityStretch; }
            set
            {
                mVelocityStretch = value;
                OnElementChanged();
            }
        }

        [ElementPriority(33)]
        public float Drag
        {
            get { return mDrag; }
            set
            {
                mDrag = value;
                OnElementChanged();
            }
        }

        [ElementPriority(32)]
        public Vector3ValueLE RadialForceLocation
        {
            get { return mRadialForceLocation; }
            set
            {
                mRadialForceLocation = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(31)]
        public float RadialForce
        {
            get { return mRadialForce; }
            set
            {
                mRadialForce = value;
                OnElementChanged();
            }
        }

        [ElementPriority(30)]
        public float GravityStrength
        {
            get { return mGravityStrength; }
            set
            {
                mGravityStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(29)]
        public float WindStrength
        {
            get { return mWindStrength; }
            set
            {
                mWindStrength = value;
                OnElementChanged();
            }
        }

        [ElementPriority(28)]
        public Vector3ValueLE DirectionalForcesSum
        {
            get { return mDirectionalForcesSum; }
            set
            {
                mDirectionalForcesSum = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(27)]
        public byte FrameRandom
        {
            get { return mFrameRandom; }
            set
            {
                mFrameRandom = value;
                OnElementChanged();
            }
        }

        [ElementPriority(26)]
        public byte FrameCount
        {
            get { return mFrameCount; }
            set
            {
                mFrameCount = value;
                OnElementChanged();
            }
        }

        [ElementPriority(25)]
        public byte FrameStart
        {
            get { return mFrameStart; }
            set
            {
                mFrameStart = value;
                OnElementChanged();
            }
        }

        [ElementPriority(24)]
        public float FrameSpeed
        {
            get { return mFrameSpeed; }
            set
            {
                mFrameSpeed = value;
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

        [ElementPriority(22)]
        public byte TileCountV
        {
            get { return mTileCountV; }
            set
            {
                mTileCountV = value;
                OnElementChanged();
            }
        }

        [ElementPriority(21)]
        public byte TileCountU
        {
            get { return mTileCountU; }
            set
            {
                mTileCountU = value;
                OnElementChanged();
            }
        }

        [ElementPriority(20)]
        public byte OverrideSet
        {
            get { return mOverrideSet; }
            set
            {
                mOverrideSet = value;
                OnElementChanged();
            }
        }

        [ElementPriority(19)]
        public byte PhysicsType
        {
            get { return mPhysicsType; }
            set
            {
                mPhysicsType = value;
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
        public ColorValue ColorVary
        {
            get { return mColorVary; }
            set
            {
                mColorVary = new ColorValue(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(16)]
        public DataList<ColorValue> ColorCurve
        {
            get { return mColorCurve; }
            set
            {
                mColorCurve = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(15)]
        public float AlphaVary
        {
            get { return mAlphaVary; }
            set
            {
                mAlphaVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(14)]
        public DataList<FloatValue> AlphaCurve
        {
            get { return mAlphaCurve; }
            set
            {
                mAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(13)]
        public DataList<FloatValue> RotationCurve
        {
            get { return mRotationCurve; }
            set
            {
                mRotationCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public float RotationOffset
        {
            get { return mRotationOffset; }
            set
            {
                mRotationOffset = value;
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public float RotationVary
        {
            get { return mRotationVary; }
            set
            {
                mRotationVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public float AspectVary
        {
            get { return mAspectVary; }
            set
            {
                mAspectVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public DataList<FloatValue> AspectCurve
        {
            get { return mAspectCurve; }
            set
            {
                mAspectCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public float SizeVary
        {
            get { return mSizeVary; }
            set
            {
                mSizeVary = value;
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

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            // no flag mask

            ParticleParameters = new ParticleParams(requestedApiVersion, handler, true, stream);
            mRateCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mRateCurveTime);
            s.Read(out mRateCurveCycles);
            s.Read(out mRateSpeedScale);
            mSizeCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mSizeVary);
            mAspectCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mAspectVary);
            s.Read(out mRotationVary);
            s.Read(out mRotationOffset);
            mRotationCurve = new DataList<FloatValue>(handler, stream);
            mAlphaCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mAlphaVary);
            mColorCurve = new DataList<ColorValue>(handler, stream);
            mColorVary = new ColorValue(requestedApiVersion, handler, stream);
            mDrawInfo = new ResourceReference(0, handler, mSection, stream);

            s.Read(out mPhysicsType);
            s.Read(out mOverrideSet);
            s.Read(out mTileCountU);
            s.Read(out mTileCountV);
            s.Read(out mAlignMode);
            s.Read(out mFrameSpeed);
            s.Read(out mFrameStart);
            s.Read(out mFrameCount);
            s.Read(out mFrameRandom);

            mDirectionalForcesSum = new Vector3ValueLE(requestedApiVersion, handler, stream);

            s.Read(out mWindStrength);
            s.Read(out mGravityStrength);
            s.Read(out mRadialForce);

            mRadialForceLocation = new Vector3ValueLE(requestedApiVersion, handler, stream);

            s.Read(out mDrag);
            s.Read(out mVelocityStretch);
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

            s.Read(out mMapRepulseKillHeight); //-1000000000.0f
            s.Read(out mProbabilityDeath); //0f
            mAltitudeRange = new Vector2ValueLE(requestedApiVersion, handler, stream); // (-10000.0f, 10000.0f)

            s.Read(out mForceMapId);
            s.Read(out mEmitRateMapId);
            s.Read(out mEmitColorMapId);
            mRandomWalk = new RandomWalk(requestedApiVersion, handler, stream);

            mAttractorOrigin = new Vector3ValueLE(requestedApiVersion, handler, stream);

            mAttractor = new Attractor(requestedApiVersion, handler, stream);

            mPathPoints = new DataList<PathPoint>(handler, stream);

            //Version 2+
            if (mSection.Version >= 0x0002 && stream.Position < stream.Length)
            {
                mVector01 = new Vector3ValueLE(requestedApiVersion, handler, stream);
                mVector3List01 = new DataList<Vector3ValueLE>(handler, stream);
            }

            //Version 3+
            if (mSection.Version >= 0x0003 && stream.Position < stream.Length) s.Read(out mByte01);

            //Version 4+
            if (mSection.Version >= 0x0004 && stream.Position < stream.Length) s.Read(out mFloat01);

            //Version 5+
            if (mSection.Version >= 0x0005 && stream.Position < stream.Length)
            {
                if (isTheSims4)
                {
                    mVector02 = new Vector3ValueLE(requestedApiVersion, handler, stream);
                    s.Read(out mUnknown170);
                }
                else
                {
                    float value;
                    s.Read(out value);
                    mVector02 = new Vector3ValueLE(requestedApiVersion, handler, value, value, value);
                }
            }

            if (isTheSims4)
            {
                //Version 6+
                if (mSection.Version >= 0x0006 && stream.Position < stream.Length)
                {
                    mUnknown140 = new Vector2ValueLE(requestedApiVersion, handler, stream);
                }
                else
                {
                    float value;
                    s.Read(out value);
                    mUnknown140 = new Vector2ValueLE(requestedApiVersion, handler, value, value);
                }

                //Version 7+
                if (mSection.Version >= 0x0007 && stream.Position < stream.Length)
                {
                    mUnknown150 = new Vector2ValueLE(requestedApiVersion, handler, stream);
                }

                s.Read(out mUnknown160);
                s.Read(out mUnknown164);

                s.Read(out mUnknown1C1);
                if (mUnknown1C1 != 0 && stream.Position < stream.Length)
                {
                    mUnknown180 = new Vector3ValueLE(requestedApiVersion, handler, stream);
                    s.Read(out mUnknown1C0);

                    if (mSection.Version >= 0x0006 && stream.Position < stream.Length)
                    {
                        mUnknown190 = new Vector2ValueLE(requestedApiVersion, handler, stream);
                    }
                    else
                    {
                        float value;
                        s.Read(out value);
                        mUnknown190 = new Vector2ValueLE(requestedApiVersion, handler, value, value);
                    }

                    if (mSection.Version >= 0x0007 && stream.Position < stream.Length)
                    {
                        mUnknown1A0 = new Vector2ValueLE(requestedApiVersion, handler, stream);
                    }

                    s.Read(out mUnknown1B0);
                    s.Read(out mUnknown1B4);
                }

                if (mSection.Version >= 0x0006 && stream.Position < stream.Length)
                {
                    s.Read(out mUnknown208);
                    if (mUnknown208 != 0)
                    {
                        s.Read(out mUnknown20C);
                        s.Read(out mUnknown209);
                        s.Read(out mUnknown20A);
                        s.Read(out mUnknown20B);
                    }
                }
            }
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(Flags);
            ParticleParameters.UnParse(stream);
            mRateCurve.UnParse(stream);
            s.Write(mRateCurveTime);
            s.Write(mRateCurveCycles);
            s.Write(mRateSpeedScale);
            mSizeCurve.UnParse(stream);
            s.Write(mSizeVary);
            mAspectCurve.UnParse(stream);
            s.Write(mAspectVary);
            s.Write(mRotationVary);
            s.Write(mRotationOffset);
            mRotationCurve.UnParse(stream);
            mAlphaCurve.UnParse(stream);
            s.Write(mAlphaVary);
            mColorCurve.UnParse(stream);
            mColorVary.UnParse(stream);
            mDrawInfo.UnParse(stream);

            s.Write(mPhysicsType);
            s.Write(mOverrideSet);
            s.Write(mTileCountU);
            s.Write(mTileCountV);
            s.Write(mAlignMode);
            s.Write(mFrameSpeed);
            s.Write(mFrameStart);
            s.Write(mFrameCount);
            s.Write(mFrameRandom);

            mDirectionalForcesSum.UnParse(stream);

            s.Write(mWindStrength);
            s.Write(mGravityStrength);
            s.Write(mRadialForce);

            mRadialForceLocation.UnParse(stream);

            s.Write(mDrag);
            s.Write(mVelocityStretch);
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

            s.Write(mMapRepulseKillHeight); //-1000000000.0f
            s.Write(mProbabilityDeath); //0f
            mAltitudeRange.UnParse(stream); // (-10000.0f, 10000.0f)

            s.Write(mForceMapId);
            s.Write(mEmitRateMapId);
            s.Write(mEmitColorMapId);
            mRandomWalk.UnParse(stream);
            mAttractorOrigin.UnParse(stream);

            mAttractor.UnParse(stream);

            mPathPoints.UnParse(stream);

            //Version 2+
            if (mSection.Version >= 0x0002)
            {
                mVector01.UnParse(stream);
                mVector3List01.UnParse(stream);
            }

            //Version 3+
            if (mSection.Version >= 0x0003) s.Write(mByte01);

            //Version 4+
            if (mSection.Version >= 0x0004) s.Write(mFloat01);

            //Version 5+
            if (mSection.Version >= 0x0005)
            {
                if (isTheSims4)
                {
                    mVector02.UnParse(stream);
                    s.Write(mUnknown170);
                }
                else
                {
                    s.Write(mVector02.X);
                }
            }

            if (isTheSims4)
            {
                //Version 6+
                if (mSection.Version >= 0x0006)
                {
                    mUnknown140.UnParse(stream);
                }
                else
                {
                    s.Write(mUnknown140.X);
                }

                //Version 7+
                if (mSection.Version >= 0x0007)
                {
                    mUnknown150.UnParse(stream);
                }

                s.Write(mUnknown160);
                s.Write(mUnknown164);

                s.Write(mUnknown1C1);
                if (mUnknown1C1 != 0)
                {
                    mUnknown180.UnParse(stream);
                    s.Write(mUnknown1C0);

                    if (mSection.Version >= 0x0006)
                    {
                        mUnknown190.UnParse(stream);
                    }
                    else
                    {
                        s.Write(mUnknown190.X);
                    }

                    if (mSection.Version >= 0x0007)
                    {
                        mUnknown1A0.UnParse(stream);
                    }

                    s.Write(mUnknown1B0);
                    s.Write(mUnknown1B4);
                }

                if (mSection.Version >= 0x0006)
                {
                    s.Write(mUnknown208);
                    if (mUnknown208 != 0)
                    {
                        s.Write(mUnknown20C);
                        s.Write(mUnknown209);
                        s.Write(mUnknown20A);
                        s.Write(mUnknown20B);
                    }
                }
            }
        }
        #endregion

        public override List<string> ContentFields
        {
            get
            {
                List<string> fields = base.ContentFields;
                if (!isTheSims4 || mSection.Version < 7)
                {
                    fields.Remove("Unknown150");
                    fields.Remove("Unknown1A0");
                }
                if (!isTheSims4 || mSection.Version < 6)
                {
                    fields.Remove("Unknown208");
                    fields.Remove("Unknown20C");
                    fields.Remove("Unknown209");
                    fields.Remove("Unknown20A");
                    fields.Remove("Unknown20B");
                }
                if (!isTheSims4)
                {
                    fields.Remove("Unknown140");
                    fields.Remove("Unknown160");
                    fields.Remove("Unknown164");
                    fields.Remove("Unknown1C1");
                    fields.Remove("Unknown180");
                    fields.Remove("Unknown1C0");
                    fields.Remove("Unknown190");
                    fields.Remove("Unknown1B0");
                    fields.Remove("Unknown1B4");
                }
                if (!isTheSims4 || mSection.Version < 5)
                {
                    fields.Remove("Byte02");
                }
                if (mSection.Version < 5)
                {
                    fields.Remove("Vector02");
                }
                if (mSection.Version < 4) fields.Remove("Float01");
                if (mSection.Version < 3) fields.Remove("Byte01");
                if (mSection.Version < 2)
                {
                    fields.Remove("Vector01");
                    fields.Remove("Vector3List01");
                }
                return fields;
            }
        }

        public bool Equals(ParticleEffect other)
        {
            return base.Equals(other);
        }
    }
}
