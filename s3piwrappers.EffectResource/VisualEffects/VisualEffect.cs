using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers
{
    public class VisualEffect : SectionData, IEquatable<VisualEffect>
    {
        public class Description : SectionData, IEquatable<Description>
        {
            #region Attributes
            private byte mComponentType;
            private uint mFlags;
            private TransformElement mLocalXForm;
            private byte mLODBegin;
            private byte mLODEnd;
            private DataList<LODScale> mLODScales;//originally DataList<Vector3Value>
            private float mEmitScaleBegin = 1f;
            private float mEmitScaleEnd = 1f;
            private float mSizeScaleBegin = 1f;
            private float mSizeScaleEnd = 1f;
            private float mAlphaScaleBegin = 1f;
            private float mAlphaScaleEnd = 1f;
            private uint mAppFlags;//originally float
            private uint mAppFlagsMask;//originally float
            private ushort mSelectionGroup;
            private ushort mSelectionChance;
            private float mTimeScale;
            private int mComponentIndex;
            private byte mByte03; //version 2+
            private byte mByte04; //version 2+
            private DataList<FloatValue> mFloatList01;//version 3+
            #endregion

            #region Constructors
            public Description(int apiVersion, EventHandler handler, ISection section)
                : base(apiVersion, handler, section)
            {
                mLocalXForm = new TransformElement(apiVersion, handler);
                mLODScales = new DataList<LODScale>(handler);
                mFloatList01 = new DataList<FloatValue>(handler);
            }

            public Description(int apiVersion, EventHandler handler, ISection section, Stream s)
                : base(apiVersion, handler, section, s)
            {
            }

            public Description(int apiVersion, EventHandler handler, SectionData basis)
                : base(apiVersion, handler, basis)
            {
            }
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public byte ComponentType
            {
                get { return mComponentType; }
                set
                {
                    mComponentType = value;
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
            public TransformElement LocalXForm
            {
                get { return mLocalXForm; }
                set
                {
                    mLocalXForm = new TransformElement(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(4)]
            public byte LODBegin
            {
                get { return mLODBegin; }
                set
                {
                    mLODBegin = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(5)]
            public byte LODEnd
            {
                get { return mLODEnd; }
                set
                {
                    mLODEnd = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(6)]
            public DataList<LODScale> LODScales
            {
                get { return mLODScales; }
                set
                {
                    mLODScales = new DataList<LODScale>(handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(7)]
            public float EmitScaleBegin
            {
                get { return mEmitScaleBegin; }
                set
                {
                    mEmitScaleBegin = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(8)]
            public float EmitScaleEnd
            {
                get { return mEmitScaleEnd; }
                set
                {
                    mEmitScaleEnd = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(9)]
            public float SizeScaleBegin
            {
                get { return mSizeScaleBegin; }
                set
                {
                    mSizeScaleBegin = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(10)]
            public float SizeScaleEnd
            {
                get { return mSizeScaleEnd; }
                set
                {
                    mSizeScaleEnd = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(11)]
            public float AlphaScaleBegin
            {
                get { return mAlphaScaleBegin; }
                set
                {
                    mAlphaScaleBegin = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(12)]
            public float AlphaScaleEnd
            {
                get { return mAlphaScaleEnd; }
                set
                {
                    mAlphaScaleEnd = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(13)]
            public uint AppFlags
            {
                get { return mAppFlags; }
                set
                {
                    mAppFlags = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(14)]
            public uint AppFlagsMask
            {
                get { return mAppFlagsMask; }
                set
                {
                    mAppFlagsMask = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(15)]
            public ushort SelectionGroup
            {
                get { return mSelectionGroup; }
                set
                {
                    mSelectionGroup = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(16)]
            public ushort SelectionChance
            {
                get { return mSelectionChance; }
                set
                {
                    mSelectionChance = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(17)]
            public float TimeScale
            {
                get { return mTimeScale; }
                set
                {
                    mTimeScale = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(18)]
            public int ComponentIndex
            {
                get { return mComponentIndex; }
                set
                {
                    mComponentIndex = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(19)]
            public byte Byte03
            {
                get { return mByte03; }
                set
                {
                    mByte03 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(20)]
            public byte Byte04
            {
                get { return mByte04; }
                set
                {
                    mByte04 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(21)]
            public DataList<FloatValue> FloatList01
            {
                get { return mFloatList01; }
                set
                {
                    mFloatList01 = new DataList<FloatValue>(handler, value);
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mComponentType);
                s.Read(out mFlags);
                mLocalXForm = new TransformElement(requestedApiVersion, handler, stream);
                s.Read(out mLODBegin);
                s.Read(out mLODEnd);
                mLODScales = new DataList<LODScale>(handler, stream);
                s.Read(out mEmitScaleBegin); //1.0
                s.Read(out mEmitScaleEnd); //1.0
                s.Read(out mSizeScaleBegin); //1.0
                s.Read(out mSizeScaleEnd); //1.0
                s.Read(out mAlphaScaleBegin); //1.0
                s.Read(out mAlphaScaleEnd); //1.0
                s.Read(out mAppFlags);
                s.Read(out mAppFlagsMask);
                s.Read(out mSelectionGroup);
                s.Read(out mSelectionChance);
                s.Read(out mTimeScale);
                s.Read(out mComponentIndex);
                if (mSection.Version >= 2 && stream.Position < stream.Length)
                {
                    s.Read(out mByte03); //version 2+
                    s.Read(out mByte04); //version 2+
                }
                if (mSection.Version >= 3 && stream.Position < stream.Length)
                {
                    mFloatList01 = new DataList<FloatValue>(handler, stream);//version 3+
                }
                else
                {
                    mFloatList01 = new DataList<FloatValue>(handler);
                }
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mComponentType);
                s.Write(mFlags);
                mLocalXForm.UnParse(stream);
                s.Write(mLODBegin);
                s.Write(mLODEnd);
                mLODScales.UnParse(stream);
                s.Write(mEmitScaleBegin); //1.0
                s.Write(mEmitScaleEnd); //1.0
                s.Write(mSizeScaleBegin); //1.0
                s.Write(mSizeScaleEnd); //1.0
                s.Write(mAlphaScaleBegin); //1.0
                s.Write(mAlphaScaleEnd); //1.0
                s.Write(mAppFlags);
                s.Write(mAppFlagsMask);
                s.Write(mSelectionGroup);
                s.Write(mSelectionChance);
                s.Write(mTimeScale);
                s.Write(mComponentIndex);
                if (mSection.Version >= 2)
                {
                    s.Write(mByte03); //version 2+
                    s.Write(mByte04); //version 2+
                }
                if (mSection.Version >= 3)
                {
                    mFloatList01.UnParse(stream);
                }
            }
            #endregion

            public override List<string> ContentFields
            {
                get
                {
                    List<string> fields = base.ContentFields;
                    if (mSection.Version < 3)
                    {
                        fields.Remove("FloatList01");
                    }
                    if (mSection.Version < 2)
                    {
                        fields.Remove("Byte03");
                        fields.Remove("Byte04");
                    }
                    return fields;
                }
            }

            public bool Equals(Description other)
            {
                return base.Equals(other);
            }
        }

        public class LODScale : DataElement, IEquatable<LODScale>
        {
            #region Constructors
            public LODScale(int apiVersion, EventHandler handler, LODScale basis)
                : base(apiVersion, handler, basis)
            {
            }

            public LODScale(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler, s)
            {
            }

            public LODScale(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }
            #endregion

            #region Attributes
            private float mEmitScale;
            private float mSizeScale;
            private float mAlphaScale;
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public float EmitScale
            {
                get { return mEmitScale; }
                set
                {
                    mEmitScale = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public float SizeScale
            {
                get { return mSizeScale; }
                set
                {
                    mSizeScale = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public float AlphaScale
            {
                get { return mAlphaScale; }
                set
                {
                    mAlphaScale = value;
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mEmitScale);
                s.Read(out mSizeScale);
                s.Read(out mAlphaScale);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mEmitScale);
                s.Write(mSizeScale);
                s.Write(mAlphaScale);
            }
            #endregion

            public bool Equals(LODScale other)
            {
                /*return this.mEmitScale == other.mEmitScale 
                    && this.mSizeScale == other.mSizeScale 
                    && this.mAlphaScale == other.mAlphaScale;/* */
                return base.Equals(other);
            }
        }

        #region Constructors
        public VisualEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mScreenSizeRange = new Vector2ValueLE(apiVersion, handler);
            mLODDistances = new DataList<FloatValue>(handler);
            mExtendedLODWeights = new Vector3ValueLE(apiVersion, handler);
            mDescriptions = new SectionDataList<Description>(handler, mSection);
        }

        public VisualEffect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }

        public VisualEffect(int apiVersion, EventHandler handler, VisualEffect basis)
            : base(apiVersion, handler, basis)
        {
        }
        #endregion

        #region Attributes
        private uint mFlags;
        private uint mComponentAppFlagsMask;
        private uint mNotifyMessageId;
        private Vector2ValueLE mScreenSizeRange;
        private float mCursorActiveDistance;//originally uint
        private byte mCursorButton;
        private DataList<FloatValue> mLODDistances;
        private Vector3ValueLE mExtendedLODWeights;
        private uint mSeed;
        private SectionDataList<Description> mDescriptions;
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
        public uint ComponentAppFlagsMask
        {
            get { return mComponentAppFlagsMask; }
            set
            {
                mComponentAppFlagsMask = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public uint NotifyMessageId
        {
            get { return mNotifyMessageId; }
            set
            {
                mNotifyMessageId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public Vector2ValueLE ScreenSizeRange
        {
            get { return mScreenSizeRange; }
            set
            {
                mScreenSizeRange = new Vector2ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public float CursorActiveDistance
        {
            get { return mCursorActiveDistance; }
            set
            {
                mCursorActiveDistance = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public byte CursorButton
        {
            get { return mCursorButton; }
            set
            {
                mCursorButton = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public DataList<FloatValue> LODDistances
        {
            get { return mLODDistances; }
            set
            {
                mLODDistances = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public Vector3ValueLE ExtendedLODWeights
        {
            get { return mExtendedLODWeights; }
            set
            {
                mExtendedLODWeights = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public uint Seed
        {
            get { return mSeed; }
            set
            {
                mSeed = value;
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public SectionDataList<Description> Descriptions
        {
            get { return mDescriptions; }
            set
            {
                mDescriptions = new SectionDataList<Description>(handler, mSection, value);
                OnElementChanged();
            }
        }
        #endregion

        public bool Equals(VisualEffect other)
        {
            return base.Equals(other);
        }

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            s.Read(out mComponentAppFlagsMask);
            s.Read(out mNotifyMessageId);
            mScreenSizeRange = new Vector2ValueLE(requestedApiVersion, handler, stream);
            s.Read(out mCursorActiveDistance);
            s.Read(out mCursorButton);
            mLODDistances = new DataList<FloatValue>(handler, stream);
            mExtendedLODWeights = new Vector3ValueLE(requestedApiVersion, handler, stream);
            s.Read(out mSeed);
            mDescriptions = new SectionDataList<Description>(handler, mSection, stream);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);
            s.Write(mComponentAppFlagsMask);
            s.Write(mNotifyMessageId);
            mScreenSizeRange.UnParse(stream);
            s.Write(mCursorActiveDistance);
            s.Write(mCursorButton);
            mLODDistances.UnParse(stream);
            mExtendedLODWeights.UnParse(stream);
            s.Write(mSeed);
            mDescriptions.UnParse(stream);
        }
        #endregion
    }
}
