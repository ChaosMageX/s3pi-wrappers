using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class ModelEffect : Effect, IEquatable<ModelEffect>
    {
        #region Constructors
        public ModelEffect(int apiVersion, EventHandler handler, ModelEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public ModelEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mColor = new ColorValue(apiVersion, handler);
            mAnimationCurves = new DataList<AnimationCurve>(handler);
        }

        public ModelEffect(int apiVersion, EventHandler handler, ISection section, Stream s) : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        public class AnimationCurve : DataElement, IEquatable<AnimationCurve>
        {
            #region Constructors
            public AnimationCurve(int apiVersion, EventHandler handler, AnimationCurve basis)
                : base(apiVersion, handler, basis)
            {
            }

            public AnimationCurve(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mLengthRange = new Vector2ValueLE(apiVersion, handler);
                mCurve = new DataList<FloatValue>(handler);
            }

            public AnimationCurve(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler)
            {
                Parse(s);
            }
            #endregion

            #region Attributes
            private Vector2ValueLE mLengthRange;
            private DataList<FloatValue> mCurve;
            private float mCurveVary;//originally uint
            private float mSpeedScale;//originally uint
            private byte mChannelId;
            private byte mMode;
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public Vector2ValueLE LengthRange
            {
                get { return mLengthRange; }
                set
                {
                    mLengthRange = new Vector2ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public DataList<FloatValue> Curve
            {
                get { return mCurve; }
                set
                {
                    mCurve = new DataList<FloatValue>(handler, value);
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public float CurveVary
            {
                get { return mCurveVary; }
                set
                {
                    mCurveVary = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(4)]
            public float SpeedScale
            {
                get { return mSpeedScale; }
                set
                {
                    mSpeedScale = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(5)]
            public byte ChannelId
            {
                get { return mChannelId; }
                set
                {
                    mChannelId = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(6)]
            public byte Mode
            {
                get { return mMode; }
                set
                {
                    mMode = value;
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mLengthRange = new Vector2ValueLE(requestedApiVersion, handler, stream);
                mCurve = new DataList<FloatValue>(handler, stream);
                s.Read(out mCurveVary);
                s.Read(out mSpeedScale);
                s.Read(out mChannelId);
                s.Read(out mMode);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mLengthRange.UnParse(stream);
                mCurve.UnParse(stream);
                s.Write(mCurveVary);
                s.Write(mSpeedScale);
                s.Write(mChannelId);
                s.Write(mMode);
            }
            #endregion

            public bool Equals(AnimationCurve other)
            {
                return base.Equals(other);
            }
        }

        #region Attributes
        private uint mFlags;
        private ulong mResourceId;
        private float mSize;
        private ColorValue mColor;
        private float mAlpha;
        private DataList<AnimationCurve> mAnimationCurves;
        private ulong mMaterialId;
        private byte mOverrideSet;
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
        public ulong ResourceId
        {
            get { return mResourceId; }
            set
            {
                mResourceId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public float Size
        {
            get { return mSize; }
            set
            {
                mSize = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public ColorValue Color
        {
            get { return mColor; }
            set
            {
                mColor = new ColorValue(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public float Alpha
        {
            get { return mAlpha; }
            set
            {
                mAlpha = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public DataList<AnimationCurve> AnimationCurves
        {
            get { return mAnimationCurves; }
            set
            {
                mAnimationCurves = new DataList<AnimationCurve>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public ulong MaterialId
        {
            get { return mMaterialId; }
            set
            {
                mMaterialId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public byte OverrideSet
        {
            get { return mOverrideSet; }
            set
            {
                mOverrideSet = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            //mFlags &= 0x3;

            s.Read(out mResourceId);
            s.Read(out mSize);
            mColor = new ColorValue(requestedApiVersion, handler, stream);
            s.Read(out mAlpha);
            mAnimationCurves = new DataList<AnimationCurve>(handler, stream);
            s.Read(out mMaterialId);
            s.Read(out mOverrideSet);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);
            s.Write(mResourceId);
            s.Write(mSize);
            mColor.UnParse(stream);
            s.Write(mAlpha);
            mAnimationCurves.UnParse(stream);
            s.Write(mMaterialId);
            s.Write(mOverrideSet);
        }
        #endregion

        public bool Equals(ModelEffect other)
        {
            return base.Equals(other);
        }
    }
}
