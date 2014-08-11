using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class ShakeEffect : Effect, IEquatable<ShakeEffect>
    {
        #region Constructor
        public ShakeEffect(int apiVersion, EventHandler handler, ShakeEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public ShakeEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mStrengthCurve = new DataList<FloatValue>(handler);
            mFrequencyCurve = new DataList<FloatValue>(handler);
        }

        public ShakeEffect(int apiVersion, EventHandler handler, ISection section, Stream s) 
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private float mLifetime;
        private float mFadeTime;
        private DataList<FloatValue> mStrengthCurve;
        private DataList<FloatValue> mFrequencyCurve;
        private float mAspectRatio;
        private byte mBaseTableType;
        private float mFalloff;
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public float Lifetime
        {
            get { return mLifetime; }
            set
            {
                mLifetime = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public float FadeTime
        {
            get { return mFadeTime; }
            set
            {
                mFadeTime = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public DataList<FloatValue> StrengthCurve
        {
            get { return mStrengthCurve; }
            set
            {
                mStrengthCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public DataList<FloatValue> FrequencyCurve
        {
            get { return mFrequencyCurve; }
            set
            {
                mFrequencyCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public float AspectRatio
        {
            get { return mAspectRatio; }
            set
            {
                mAspectRatio = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public byte BaseTableType
        {
            get { return mBaseTableType; }
            set
            {
                mBaseTableType = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public float Falloff
        {
            get { return mFalloff; }
            set
            {
                mFalloff = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mLifetime);
            s.Read(out mFadeTime);
            mStrengthCurve = new DataList<FloatValue>(handler, stream);
            mFrequencyCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mAspectRatio);
            s.Read(out mBaseTableType);
            s.Read(out mFalloff);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mLifetime);
            s.Write(mFadeTime);
            mStrengthCurve.UnParse(stream);
            mFrequencyCurve.UnParse(stream);
            s.Write(mAspectRatio);
            s.Write(mBaseTableType);
            s.Write(mFalloff);
        }
        #endregion

        public bool Equals(ShakeEffect other)
        {
            return base.Equals(other);
        }
    }
}
