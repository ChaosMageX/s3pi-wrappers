using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class Effect : SectionData, IEquatable<Effect>
    {
        private static readonly bool isTheSims4 = false;

        public class ResourceReference : DataElement
        {
            private ISection mSection;

            #region Constructors
            public ResourceReference(int apiVersion, EventHandler handler, ISection section)
                : base(apiVersion, handler)
            {
                mSection = section;
            }

            public ResourceReference(int apiVersion, EventHandler handler, ISection section, ResourceReference basis)
                : base(apiVersion, handler)
            {
                mSection = section;
                mResourceId = basis.mResourceId;
                mFormat = basis.mFormat;
                mDrawMode = basis.mDrawMode;
                mInt01 = basis.mInt01;
                mByte01 = basis.mByte01;
                mDrawFlags = basis.mDrawFlags;
                mBuffer = basis.mBuffer;
                mLayer = basis.mLayer;
                mSortOffset = basis.mSortOffset;
                mResourceId2 = basis.mResourceId2;
            }

            public ResourceReference(int apiVersion, EventHandler handler, ISection section, Stream s)
                : base(apiVersion, handler, s)
            {
                mSection = section;
            }
            #endregion

            #region Attributes
            private ulong mResourceId;
            private byte mFormat;
            private byte mDrawMode;
            private uint mInt01;  //if DrawMode & 0x80 != 0
            private byte mByte01; //if DrawMode & 0x40 != 0 && The Sims 3
            private ushort mDrawFlags;//byte; version 6+ = word
            private byte mBuffer;
            private short mLayer;//originally ushort
            private float mSortOffset;//originally uint
            private ulong mResourceId2;
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mResourceId);
                s.Read(out mFormat);
                s.Read(out mDrawMode);

                if ((mDrawMode & 0x80) == 0x80) s.Read(out mInt01);
                if ((mDrawMode & 0x40) == 0x40 && !isTheSims4) s.Read(out mByte01);

                if (mSection.Version >= 0x0006)
                {
                    s.Read(out mDrawFlags);
                }
                else
                {
                    byte flags;
                    s.Read(out flags);
                    mDrawFlags = flags;
                }

                s.Read(out mBuffer);
                s.Read(out mLayer);
                s.Read(out mSortOffset);
                s.Read(out mResourceId2);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mResourceId);
                s.Write(mFormat);
                s.Write(mDrawMode);

                if ((mDrawMode & 0x80) == 0x80) s.Write(mInt01);
                if ((mDrawMode & 0x40) == 0x40 && !isTheSims4) s.Write(mByte01);

                if (mSection.Version >= 0x0006)
                {
                    s.Write(mDrawFlags);
                }
                else
                {
                    byte flags = (byte)(mDrawFlags & 0xFF);
                    s.Write(flags);
                }

                s.Write(mBuffer);
                s.Write(mLayer);
                s.Write(mSortOffset);
                s.Write(mResourceId2);
            }
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public ulong Instance
            {
                get { return mResourceId; }
                set
                {
                    mResourceId = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public byte Format
            {
                get { return mFormat; }
                set
                {
                    mFormat = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public byte DrawMode
            {
                get { return mDrawMode; }
                set
                {
                    mDrawMode = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(4)]
            public uint Int01
            {
                get { return mInt01; }
                set
                {
                    mInt01 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(5)]
            public byte Byte01
            {
                get { return mByte01; }
                set
                {
                    mByte01 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(6)]
            public ushort DrawFlags
            {
                get { return mDrawFlags; }
                set
                {
                    mDrawFlags = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(7)]
            public byte Buffer
            {
                get { return mBuffer; }
                set
                {
                    mBuffer = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(8)]
            public short Layer
            {
                get { return mLayer; }
                set
                {
                    mLayer = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(9)]
            public float SortOffset
            {
                get { return mSortOffset; }
                set
                {
                    mSortOffset = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(10)]
            public ulong ResourceId2
            {
                get { return mResourceId2; }
                set
                {
                    mResourceId2 = value;
                    OnElementChanged();
                }
            }
            #endregion
        }

        public class ParticleParams : DataElement
        {
            private bool bHasTorus;

            #region Constructors
            public ParticleParams(int apiVersion, EventHandler handler, bool hasTorus, ParticleParams basis)
                : base(apiVersion, handler)
            {
                bHasTorus = hasTorus;
                mParticleLifetime = new Vector2ValueLE(apiVersion, handler, basis.mParticleLifetime);
                mPrerollTime = basis.mPrerollTime;
                mEmitDelay = new Vector2ValueLE(apiVersion, handler, basis.mEmitDelay);
                mEmitRetrigger = new Vector2ValueLE(apiVersion, handler, basis.mEmitRetrigger);
                mEmitDirection = new BoundingBoxValue(apiVersion, handler, basis.mEmitDirection);
                mEmitSpeed = new Vector2ValueLE(apiVersion, handler, basis.mEmitSpeed);
                mEmitVolume = new BoundingBoxValue(apiVersion, handler, basis.mEmitVolume);
                mEmitTorusWidth = basis.mEmitTorusWidth;
            }

            public ParticleParams(int apiVersion, EventHandler handler, bool hasTorus)
                : base(apiVersion, handler)
            {
                bHasTorus = hasTorus;
                mParticleLifetime = new Vector2ValueLE(apiVersion, handler, 10f, 10f);
                //mPrerollTime
                mEmitDelay = new Vector2ValueLE(apiVersion, handler, -1f, -1f);
                mEmitRetrigger = new Vector2ValueLE(apiVersion, handler, -1f, -1f);
                mEmitDirection = new BoundingBoxValue(apiVersion, handler);
                mEmitSpeed = new Vector2ValueLE(apiVersion, handler);
                mEmitVolume = new BoundingBoxValue(apiVersion, handler);
                mEmitTorusWidth = -1f;
            }

            public ParticleParams(int apiVersion, EventHandler handler, bool hasTorus, Stream s)
                : base(apiVersion, handler)
            {
                bHasTorus = hasTorus;
                Parse(s);
            }
            #endregion

            #region Attributes
            private Vector2ValueLE mParticleLifetime;
            private float mPrerollTime;
            private Vector2ValueLE mEmitDelay;
            private Vector2ValueLE mEmitRetrigger;
            private BoundingBoxValue mEmitDirection;
            private Vector2ValueLE mEmitSpeed;
            private BoundingBoxValue mEmitVolume;
            private float mEmitTorusWidth;
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public Vector2ValueLE ParticleLifetime
            {
                get { return mParticleLifetime; }
                set
                {
                    mParticleLifetime = new Vector2ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public float PrerollTime
            {
                get { return mPrerollTime; }
                set
                {
                    mPrerollTime = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public Vector2ValueLE EmitDelay
            {
                get { return mEmitDelay; }
                set
                {
                    mEmitDelay = new Vector2ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(4)]
            public Vector2ValueLE EmitRetrigger
            {
                get { return mEmitRetrigger; }
                set
                {
                    mEmitRetrigger = new Vector2ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(5)]
            public BoundingBoxValue EmitDirection
            {
                get { return mEmitDirection; }
                set
                {
                    mEmitDirection = new BoundingBoxValue(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(6)]
            public Vector2ValueLE EmitSpeed
            {
                get { return mEmitSpeed; }
                set
                {
                    mEmitSpeed = new Vector2ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(7)]
            public BoundingBoxValue EmitVolume
            {
                get { return mEmitVolume; }
                set
                {
                    mEmitVolume = new BoundingBoxValue(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(8)]
            public float EmitTorusWidth
            {
                get { return mEmitTorusWidth; }
                set
                {
                    mEmitTorusWidth = value;
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mParticleLifetime = new Vector2ValueLE(requestedApiVersion, handler, stream);
                s.Read(out mPrerollTime, ByteOrder.LittleEndian);
                mEmitDelay = new Vector2ValueLE(requestedApiVersion, handler, stream);
                mEmitRetrigger = new Vector2ValueLE(requestedApiVersion, handler, stream);
                mEmitDirection = new BoundingBoxValue(requestedApiVersion, handler, stream);
                mEmitSpeed = new Vector2ValueLE(requestedApiVersion, handler, stream);
                mEmitVolume = new BoundingBoxValue(requestedApiVersion, handler, stream);
                if (bHasTorus) s.Read(out mEmitTorusWidth);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mParticleLifetime.UnParse(stream);
                s.Write(mPrerollTime, ByteOrder.LittleEndian);
                mEmitDelay.UnParse(stream);
                mEmitRetrigger.UnParse(stream);
                mEmitDirection.UnParse(stream);
                mEmitSpeed.UnParse(stream);
                mEmitVolume.UnParse(stream);
                if (bHasTorus) s.Write(mEmitTorusWidth);
            }
            #endregion

            public override List<string> ContentFields
            {
                get
                {
                    List<string> results = base.ContentFields;
                    if (!bHasTorus) results.Remove("EmitTorusWidth");
                    return results;
                }
            }
        }


        public class Wiggle : DataElement, IEquatable<Wiggle>
        {
            #region Attributes
            private uint mTimeRate;//originally float
            private Vector3ValueLE mRateDir;
            private Vector3ValueLE mWiggleDir;
            #endregion

            #region Constructors
            public Wiggle(int apiVersion, EventHandler handler, Wiggle basis) 
                : base(apiVersion, handler)
            {
                mTimeRate = basis.mTimeRate;
                mRateDir = new Vector3ValueLE(apiVersion, handler, basis.mRateDir);
                mWiggleDir = new Vector3ValueLE(apiVersion, handler, basis.mWiggleDir);
            }

            public Wiggle(int apiVersion, EventHandler handler, Stream s) 
                : base(apiVersion, handler, s)
            {
            }

            public Wiggle(int apiVersion, EventHandler handler) 
                : base(apiVersion, handler)
            {
                mRateDir = new Vector3ValueLE(apiVersion, handler);
                mWiggleDir = new Vector3ValueLE(apiVersion, handler);
            }
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public uint TimeRate
            {
                get { return mTimeRate; }
                set
                {
                    mTimeRate = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public Vector3ValueLE RateDir
            {
                get { return mRateDir; }
                set
                {
                    mRateDir = new Vector3ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public Vector3ValueLE WiggleDir
            {
                get { return mWiggleDir; }
                set
                {
                    mWiggleDir = new Vector3ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mTimeRate);
                mRateDir = new Vector3ValueLE(requestedApiVersion, handler, stream);
                mWiggleDir = new Vector3ValueLE(requestedApiVersion, handler, stream);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mTimeRate);
                mRateDir.UnParse(stream);
                mWiggleDir.UnParse(stream);
            }
            #endregion

            public bool Equals(Wiggle other)
            {
                return base.Equals(other);
            }
        }

        public class Surface : DataElement, IEquatable<Surface>
        {
            #region Attributes
            private uint mFlags;
            private ulong mId;
            private uint mBounce;//originally float
            private uint mSlide;//originally float
            private uint mCollisionRadius;//originally float
            private uint mDeathProbability;//originally float
            private uint mPinOffset;
            private string mString01 = string.Empty;
            private string mString02 = string.Empty;
            private DataList<Vector3ValueLE> mData;
            #endregion

            #region Constructors
            public Surface(int apiVersion, EventHandler handler, Surface basis)
                : base(apiVersion, handler, basis)
            {
                /*mFlags = basis.mFlags;
                mId = basis.mId;
                mBounce = basis.mBounce;
                mSlide = basis.mSlide;
                mCollisionRadius = basis.mCollisionRadius;
                mDeathProbability = basis.mDeathProbability;
                mPinOffset = basis.mPinOffset;
                mString01 = basis.mString01;
                mString02 = basis.mString02;
                mData = new DataList<Vector3ValueLE>(handler, basis.mData);/* */
            }

            public Surface(int apiVersion, EventHandler handler, Stream s) 
                : base(apiVersion, handler, s)
            {
            }

            public Surface(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mData = new DataList<Vector3ValueLE>(handler);
            }
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
            public ulong Id
            {
                get { return mId; }
                set
                {
                    mId = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public uint Bounce
            {
                get { return mBounce; }
                set
                {
                    mBounce = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(4)]
            public uint Slide
            {
                get { return mSlide; }
                set
                {
                    mSlide = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(5)]
            public uint CollisionRadius
            {
                get { return mCollisionRadius; }
                set
                {
                    mCollisionRadius = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(6)]
            public uint DeathProbability
            {
                get { return mDeathProbability; }
                set
                {
                    mDeathProbability = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(7)]
            public uint PinOffset
            {
                get { return mPinOffset; }
                set
                {
                    mPinOffset = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(8)]
            public string String01
            {
                get { return mString01; }
                set
                {
                    mString01 = value ?? string.Empty;
                    OnElementChanged();
                }
            }

            [ElementPriority(9)]
            public string String02
            {
                get { return mString02; }
                set
                {
                    mString02 = value ?? string.Empty;
                    OnElementChanged();
                }
            }

            [ElementPriority(10)]
            public DataList<Vector3ValueLE> Data
            {
                get { return mData; }
                set
                {
                    mData = new DataList<Vector3ValueLE>(handler, value);
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

                s.Read(out mId);
                s.Read(out mBounce);
                s.Read(out mSlide);
                s.Read(out mCollisionRadius);
                s.Read(out mDeathProbability);
                s.Read(out mPinOffset);
                s.Read(out mString01, StringType.ZeroDelimited);
                s.Read(out mString02, StringType.ZeroDelimited);
                mData = new DataList<Vector3ValueLE>(handler, stream);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mFlags);
                s.Write(mId);
                s.Write(mBounce);
                s.Write(mSlide);
                s.Write(mCollisionRadius);
                s.Write(mDeathProbability);
                s.Write(mPinOffset);
                s.Write(mString01, StringType.ZeroDelimited);
                s.Write(mString02, StringType.ZeroDelimited);
                mData.UnParse(stream);
            }
            #endregion

            public bool Equals(Surface other)
            {
                return base.Equals(other);
            }
        }

        //public class ItemC : DataElement, IEquatable<ItemC>
        //{
        //    public ItemC(int apiVersion, EventHandler handler, ItemC basis)
        //        : base(apiVersion, handler, basis)
        //    {
        //    }

        //    public ItemC(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s)
        //    {
        //    }

        //    public ItemC(int apiVersion, EventHandler handler)
        //        : base(apiVersion, handler)
        //    {
        //        mItems = new DataList<ItemE>(handler);
        //    }

        //    private DataList<ItemE> mItems;

        //    [ElementPriority(1)]
        //    public DataList<ItemE> Items
        //    {
        //        get { return mItems; }
        //        set
        //        {
        //            mItems = value;
        //            OnElementChanged();
        //        }
        //    }

        //    protected override void Parse(Stream stream)
        //    {
        //        mItems = new DataList<ItemE>(handler, stream);
        //    }

        //    public override void UnParse(Stream stream)
        //    {
        //        mItems.UnParse(stream);
        //    }

        //    public bool Equals(ItemC other)
        //    {
        //        return base.Equals(other);
        //    }
        //}

        public class PathPoint : DataElement, IEquatable<PathPoint>
        {
            #region Constructors
            public PathPoint(int apiVersion, EventHandler handler, PathPoint basis)
                : base(apiVersion, handler)
            {
                mPosition = new Vector3ValueLE(apiVersion, handler, basis.mPosition);
                mVelocity = new Vector3ValueLE(apiVersion, handler, basis.mVelocity);
                mTime = basis.mTime;
            }

            public PathPoint(int apiVersion, EventHandler handler, Stream s) 
                : base(apiVersion, handler, s)
            {
            }

            public PathPoint(int apiVersion, EventHandler handler) 
                : base(apiVersion, handler)
            {
                mPosition = new Vector3ValueLE(apiVersion, handler);
                mVelocity = new Vector3ValueLE(apiVersion, handler);
            }
            #endregion

            #region Attributes
            private Vector3ValueLE mPosition;
            private Vector3ValueLE mVelocity;
            private float mTime;
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public Vector3ValueLE Position
            {
                get { return mPosition; }
                set
                {
                    mPosition = new Vector3ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public Vector3ValueLE Velocity
            {
                get { return mVelocity; }
                set
                {
                    mVelocity = new Vector3ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public float Time
            {
                get { return mTime; }
                set
                {
                    mTime = value;
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mPosition = new Vector3ValueLE(requestedApiVersion, handler, stream);
                mVelocity = new Vector3ValueLE(requestedApiVersion, handler, stream);
                s.Read(out mTime);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mPosition.UnParse(stream);
                mVelocity.UnParse(stream);
                s.Write(mTime);
            }
            #endregion

            public bool Equals(PathPoint other)
            {
                return base.Equals(other);
            }
        }

        // ItemE is just a repeat of Wiggle
        /*public class ItemE : DataElement, IEquatable<ItemE>
        {
            #region Attributes
            private float mFloat01;
            private float mFloat02;
            private float mFloat03;
            private float mFloat04;
            private float mFloat05;
            private float mFloat06;
            private float mFloat07;
            #endregion

            #region Constructors
            public ItemE(int apiVersion, EventHandler handler, ItemE basis)
                : base(apiVersion, handler)
            {
                mFloat01 = basis.mFloat01;
                mFloat02 = basis.mFloat02;
                mFloat03 = basis.mFloat03;
                mFloat04 = basis.mFloat04;
                mFloat05 = basis.mFloat05;
                mFloat06 = basis.mFloat06;
                mFloat07 = basis.mFloat07;
            }

            public ItemE(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s)
            {
            }

            public ItemE(int apiVersion, EventHandler handler) : base(apiVersion, handler)
            {
            }
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public float Float01
            {
                get { return mFloat01; }
                set
                {
                    mFloat01 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public float Float02
            {
                get { return mFloat02; }
                set
                {
                    mFloat02 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public float Float03
            {
                get { return mFloat03; }
                set
                {
                    mFloat03 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(4)]
            public float Float04
            {
                get { return mFloat04; }
                set
                {
                    mFloat04 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(5)]
            public float Float05
            {
                get { return mFloat05; }
                set
                {
                    mFloat05 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(6)]
            public float Float06
            {
                get { return mFloat06; }
                set
                {
                    mFloat06 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(7)]
            public float Float07
            {
                get { return mFloat07; }
                set
                {
                    mFloat07 = value;
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mFloat01);
                s.Read(out mFloat02, ByteOrder.LittleEndian);
                s.Read(out mFloat03, ByteOrder.LittleEndian);
                s.Read(out mFloat04, ByteOrder.LittleEndian);
                s.Read(out mFloat05, ByteOrder.LittleEndian);
                s.Read(out mFloat06, ByteOrder.LittleEndian);
                s.Read(out mFloat07, ByteOrder.LittleEndian);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mFloat01);
                s.Write(mFloat02, ByteOrder.LittleEndian);
                s.Write(mFloat03, ByteOrder.LittleEndian);
                s.Write(mFloat04, ByteOrder.LittleEndian);
                s.Write(mFloat05, ByteOrder.LittleEndian);
                s.Write(mFloat06, ByteOrder.LittleEndian);
                s.Write(mFloat07, ByteOrder.LittleEndian);
            }
            #endregion

            public bool Equals(ItemE other)
            {
                return base.Equals(other);
            }
        }/* */

        public class RandomWalk : DataElement
        {
            #region Constructors
            public RandomWalk(int apiVersion, EventHandler handler, RandomWalk basis)
                : base(apiVersion, handler, basis)
            {
                /*mTime = new Vector2ValueLE(apiVersion, handler, basis.mTime);
                mStrength = new Vector2ValueLE(apiVersion, handler, basis.mStrength);
                mTurnRange = basis.mTurnRange;
                mTurnOffset = basis.mTurnOffset;
                mMix = basis.mMix;
                mTurnOffsets = new DataList<FloatValue>(handler, basis.mTurnOffsets);
                mWalkLoopType = basis.mWalkLoopType;/* */
            }

            public RandomWalk(int apiVersion, EventHandler handler, Stream s) 
                : base(apiVersion, handler, s)
            {
            }

            public RandomWalk(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mTime = new Vector2ValueLE(apiVersion, handler);
                mStrength = new Vector2ValueLE(apiVersion, handler);
                mTurnOffsets = new DataList<FloatValue>(handler);
            }
            #endregion

            #region Attributes
            private Vector2ValueLE mTime;
            private Vector2ValueLE mStrength;
            private float mTurnRange;
            private float mTurnOffset;
            private float mMix;
            private DataList<FloatValue> mTurnOffsets;
            private byte mWalkLoopType;
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public Vector2ValueLE Time
            {
                get { return mTime; }
                set
                {
                    mTime = new Vector2ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public Vector2ValueLE Strength
            {
                get { return mStrength; }
                set
                {
                    mStrength = new Vector2ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public float TurnRange
            {
                get { return mTurnRange; }
                set
                {
                    mTurnRange = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(4)]
            public float TurnOffset
            {
                get { return mTurnOffset; }
                set
                {
                    mTurnOffset = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(5)]
            public float Mix
            {
                get { return mMix; }
                set
                {
                    mMix = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(6)]
            public DataList<FloatValue> TurnOffsets
            {
                get { return mTurnOffsets; }
                set
                {
                    mTurnOffsets = new DataList<FloatValue>(handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(9)]
            public byte WalkLoopType
            {
                get { return mWalkLoopType; }
                set
                {
                    mWalkLoopType = value;
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mTime = new Vector2ValueLE(requestedApiVersion, handler, stream);
                mStrength = new Vector2ValueLE(requestedApiVersion, handler, stream);
                s.Read(out mTurnRange);
                s.Read(out mTurnOffset);
                s.Read(out mMix);
                mTurnOffsets = new DataList<FloatValue>(handler, stream);
                s.Read(out mWalkLoopType);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mTime.UnParse(stream);
                mStrength.UnParse(stream);
                s.Write(mTurnRange);
                s.Write(mTurnOffset);
                s.Write(mMix);
                mTurnOffsets.UnParse(stream);
                s.Write(mWalkLoopType);
            }
            #endregion
        }

        public class Attractor : DataElement, IEquatable<Attractor>
        {
            #region Attributes
            private DataList<FloatValue> mStrengthCurve;
            private float mRange;
            private float mKillRange;
            #endregion

            #region Constructors
            public Attractor(int apiVersion, EventHandler handler, Attractor basis)
                : base(apiVersion, handler, basis)
            {
                /*mStrengthCurve = new DataList<FloatValue>(handler, basis.mStrengthCurve);
                mRange = basis.mRange;
                mKillRange = basis.mKillRange;/* */
            }

            public Attractor(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler, s)
            {
            }

            public Attractor(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mStrengthCurve = new DataList<FloatValue>(handler);
            }
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public DataList<FloatValue> StrengthCurve
            {
                get { return mStrengthCurve; }
                set
                {
                    mStrengthCurve = new DataList<FloatValue>(handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public float Range
            {
                get { return mRange; }
                set
                {
                    mRange = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public float KillRange
            {
                get { return mKillRange; }
                set
                {
                    mKillRange = value;
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mStrengthCurve = new DataList<FloatValue>(handler, stream);
                s.Read(out mRange);
                s.Read(out mKillRange);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mStrengthCurve.UnParse(stream);
                s.Write(mRange);
                s.Write(mKillRange);
            }
            #endregion

            public bool Equals(Attractor other)
            {
                return base.Equals(other);
            }
        }

        public Effect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
        }

        public Effect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }

        public Effect(int apiVersion, EventHandler handler, Effect basis)
            : base(apiVersion, handler, basis)
        {
        }

        protected override void Parse(Stream stream)
        {
        }

        public override void UnParse(Stream stream)
        {
        }

        public bool Equals(Effect other)
        {
            return base.Equals(other);
        }
    }
}
