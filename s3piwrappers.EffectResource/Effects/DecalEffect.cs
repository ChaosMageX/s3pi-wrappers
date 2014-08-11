using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class DecalEffect : Effect, IEquatable<DecalEffect>
    {
        private static readonly bool isTheSims4 = false;

        #region Constructors
        public DecalEffect(int apiVersion, EventHandler handler, DecalEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public DecalEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mRotationCurve = new DataList<FloatValue>(handler);
            mSizeCurve = new DataList<FloatValue>(handler);
            mAlphaCurve = new DataList<FloatValue>(handler);
            mColorCurve = new DataList<ColorValue>(handler);
            mAspectCurve = new DataList<FloatValue>(handler);
            mTextureOffset = new Vector2ValueLE(apiVersion, handler);
        }

        public DecalEffect(int apiVersion, EventHandler handler, ISection section, Stream s) 
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private uint mFlags;
        private ulong mDdsResource;
        private byte mByte01;
        private uint mInt01;//originally float
        private byte mByte02;
        private uint mInt02; //The Sims 4
        private float mLifetime;
        private DataList<FloatValue> mRotationCurve;
        private DataList<FloatValue> mSizeCurve;
        private DataList<FloatValue> mAlphaCurve;
        private DataList<ColorValue> mColorCurve;
        private DataList<FloatValue> mAspectCurve;
        private float mAlphaVary;
        private float mSizeVary;
        private float mRotationVary;
        private float mTextureRepeat;
        private Vector2ValueLE mTextureOffset;
        private ulong mEmitColorMapId;
        private byte mByte03; //version 2+
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
        public ulong DdsResource
        {
            get { return mDdsResource; }
            set
            {
                mDdsResource = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public byte Byte01
        {
            get { return mByte01; }
            set
            {
                mByte01 = value;
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
        public byte Byte02
        {
            get { return mByte02; }
            set
            {
                mByte02 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public uint Int02
        {
            get { return mInt02; }
            set
            {
                mInt02 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public float Lifetime
        {
            get { return mLifetime; }
            set
            {
                mLifetime = value;
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public DataList<FloatValue> RotationCurve
        {
            get { return mRotationCurve; }
            set
            {
                mRotationCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public DataList<FloatValue> SizeCurve
        {
            get { return mSizeCurve; }
            set
            {
                mSizeCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public DataList<FloatValue> AlphaCurve
        {
            get { return mAlphaCurve; }
            set
            {
                mAlphaCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public DataList<ColorValue> ColorCurve
        {
            get { return mColorCurve; }
            set
            {
                mColorCurve = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public DataList<FloatValue> AspectCurve
        {
            get { return mAspectCurve; }
            set
            {
                mAspectCurve = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(13)]
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
        public float SizeVary
        {
            get { return mSizeVary; }
            set
            {
                mSizeVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(15)]
        public float RotationVary
        {
            get { return mRotationVary; }
            set
            {
                mRotationVary = value;
                OnElementChanged();
            }
        }

        [ElementPriority(16)]
        public float TextureRepeat
        {
            get { return mTextureRepeat; }
            set
            {
                mTextureRepeat = value;
                OnElementChanged();
            }
        }

        [ElementPriority(17)]
        public Vector2ValueLE TextureOffset
        {
            get { return mTextureOffset; }
            set
            {
                mTextureOffset = new Vector2ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(18)]
        public ulong EmitColorMapId
        {
            get { return mEmitColorMapId; }
            set
            {
                mEmitColorMapId = value;
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
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            //mFlags &= 0x7F;

            s.Read(out mDdsResource);
            s.Read(out mByte01);
            s.Read(out mInt01);
            s.Read(out mByte02);
            if (isTheSims4) s.Read(out mInt02);
            s.Read(out mLifetime);
            mRotationCurve = new DataList<FloatValue>(handler, stream);
            mSizeCurve = new DataList<FloatValue>(handler, stream);
            mAlphaCurve = new DataList<FloatValue>(handler, stream);
            mColorCurve = new DataList<ColorValue>(handler, stream);
            mAspectCurve = new DataList<FloatValue>(handler, stream);
            s.Read(out mAlphaVary);
            s.Read(out mSizeVary);
            s.Read(out mRotationVary);
            s.Read(out mTextureRepeat);
            mTextureOffset = new Vector2ValueLE(requestedApiVersion, handler, stream);
            s.Read(out mEmitColorMapId);
            if (mSection.Version >= 2 && stream.Position < stream.Length) s.Read(out mByte03); //version 2+
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);
            s.Write(mDdsResource);
            s.Write(mByte01);
            s.Write(mInt01);
            s.Write(mByte02);
            if (isTheSims4) s.Write(mInt02);
            s.Write(mLifetime);
            mRotationCurve.UnParse(stream);
            mSizeCurve.UnParse(stream);
            mAlphaCurve.UnParse(stream);
            mColorCurve.UnParse(stream);
            mAspectCurve.UnParse(stream);
            s.Write(mAlphaVary);
            s.Write(mSizeVary);
            s.Write(mRotationVary);
            s.Write(mTextureRepeat);
            mTextureOffset.UnParse(stream);
            s.Write(mEmitColorMapId);
            if (mSection.Version >= 2) s.Write(mByte03); //version 2+
        }
        #endregion

        public override List<string> ContentFields
        {
            get
            {
                List<string> fields = base.ContentFields;
                if (!isTheSims4) fields.Remove("Int02");
                if (mSection.Version < 2) fields.Remove("Byte03");
                return fields;
            }
        }

        public bool Equals(DecalEffect other)
        {
            return base.Equals(other);
        }
    }
}
