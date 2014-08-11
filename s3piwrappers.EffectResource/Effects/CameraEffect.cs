using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class CameraEffect : Effect, IEquatable<CameraEffect>
    {
        #region Constructors
        public CameraEffect(int apiVersion, EventHandler handler, CameraEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public CameraEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mHeadingCurve = new DataList<FloatValue>(handler);
            mPitchCurve = new DataList<FloatValue>(handler);
            mRollCurve = new DataList<FloatValue>(handler);
            mDistanceCurve = new DataList<FloatValue>(handler);
            mFOVCurve = new DataList<FloatValue>(handler);
            mNearClipCurve = new DataList<FloatValue>(handler);
            mFarClipCurve = new DataList<FloatValue>(handler);
        }

        public CameraEffect(int apiVersion, EventHandler handler, ISection section, Stream s) 
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private uint mFlags;
        private ushort mViewFlags;
        private float mLifetime;
        private DataList<FloatValue> mHeadingCurve;
        private DataList<FloatValue> mPitchCurve;
        private DataList<FloatValue> mRollCurve;
        private DataList<FloatValue> mDistanceCurve;
        private DataList<FloatValue> mFOVCurve;
        private DataList<FloatValue> mNearClipCurve;
        private DataList<FloatValue> mFarClipCurve;
        private ulong mCameraId;
        private ushort mCubemapResource;
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
        public ushort ViewFlags
        {
            get { return mViewFlags; }
            set
            {
                mViewFlags = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public float Lifetime
        {
            get { return mLifetime; }
            set
            {
                mLifetime = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public DataList<FloatValue> HeadingCurve
        {
            get { return mHeadingCurve; }
            set
            {
                mHeadingCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public DataList<FloatValue> PitchCurve
        {
            get { return mPitchCurve; }
            set
            {
                mPitchCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public DataList<FloatValue> RollCurve
        {
            get { return mRollCurve; }
            set
            {
                mRollCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public DataList<FloatValue> DistanceCurve
        {
            get { return mDistanceCurve; }
            set
            {
                mDistanceCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public DataList<FloatValue> FOVCurve
        {
            get { return mFOVCurve; }
            set
            {
                mFOVCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public DataList<FloatValue> NearClipCurve
        {
            get { return mNearClipCurve; }
            set
            {
                mNearClipCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public DataList<FloatValue> FarClipCurve
        {
            get { return mFarClipCurve; }
            set
            {
                mFarClipCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public ulong CameraId
        {
            get { return mCameraId; }
            set
            {
                mCameraId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public ushort CubemapResource
        {
            get { return mCubemapResource; }
            set
            {
                mCubemapResource = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            //mFlags &= 0x3FF;
            
            s.Read(out mViewFlags);
            s.Read(out mLifetime);
            mHeadingCurve = new DataList<FloatValue>(handler, stream);
            mPitchCurve = new DataList<FloatValue>(handler, stream);
            mRollCurve = new DataList<FloatValue>(handler, stream);
            mDistanceCurve = new DataList<FloatValue>(handler, stream);
            mFOVCurve = new DataList<FloatValue>(handler, stream);
            mNearClipCurve = new DataList<FloatValue>(handler, stream);
            mFarClipCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mCameraId);
            s.Read(out mCubemapResource);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);
            s.Write(mViewFlags);
            s.Write(mLifetime);
            mHeadingCurve.UnParse(stream);
            mPitchCurve.UnParse(stream);
            mRollCurve.UnParse(stream);
            mDistanceCurve.UnParse(stream);
            mFOVCurve.UnParse(stream);
            mNearClipCurve.UnParse(stream);
            mFarClipCurve.UnParse(stream);
            s.Write(mCameraId);
            s.Write(mCubemapResource);
        }
        #endregion

        public bool Equals(CameraEffect other)
        {
            return base.Equals(other);
        }
    }
}
