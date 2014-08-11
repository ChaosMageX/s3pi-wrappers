using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class SpriteEffect : Effect, IEquatable<SpriteEffect>
    {
        #region Constructors
        public SpriteEffect(int apiVersion, EventHandler handler, SpriteEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public SpriteEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mResource = new ResourceReference(0, handler, section);
            mFloatList01 = new DataList<FloatValue>(handler);
            mColorList01 = new DataList<ColorValue>(handler);
            mFloatList02 = new DataList<FloatValue>(handler);
            mVector01 = new Vector3ValueLE(apiVersion, handler);
            mVector02 = new Vector3ValueLE(apiVersion, handler);
        }

        public SpriteEffect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private uint mFlags;
        private uint mInt02;
        private uint mInt03;
        private float mFloat01;
        private ushort mShort01;
        private DataList<FloatValue> mFloatList01;
        private DataList<ColorValue> mColorList01;
        private DataList<FloatValue> mFloatList02;
        private float mFloat02;
        private float mFloat03;
        private float mFloat04;

        private Vector3ValueLE mVector01;

        private Vector3ValueLE mVector02;

        private float mFloat11;
        private float mFloat12;
        private float mFloat13;
        private float mFloat14;
        private float mFloat15;
        private float mFloat16;
        private float mFloat17;
        private float mFloat18;
        private float mFloat19;
        private float mFloat20;
        private float mFloat21;
        private float mFloat22;
        private float mFloat23;
        private float mFloat24;
        private float mFloat25;
        private float mFloat26;
        private ResourceReference mResource;
        private byte mByte01;
        #endregion

        #region Content Fields
        [ElementPriority(31)]
        public byte Byte01
        {
            get { return mByte01; }
            set
            {
                mByte01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(30)]
        public ResourceReference Resource
        {
            get { return mResource; }
            set
            {
                mResource = new ResourceReference(requestedApiVersion, handler, mSection, value);
                OnElementChanged();
            }
        }

        [ElementPriority(29)]
        public float Float26
        {
            get { return mFloat26; }
            set
            {
                mFloat26 = value;
                OnElementChanged();
            }
        }

/**/

        [ElementPriority(28)]
        public float Float25
        {
            get { return mFloat25; }
            set
            {
                mFloat25 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(27)]
        public float Float24
        {
            get { return mFloat24; }
            set
            {
                mFloat24 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(26)]
        public float Float23
        {
            get { return mFloat23; }
            set
            {
                mFloat23 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(25)]
        public float Float22
        {
            get { return mFloat22; }
            set
            {
                mFloat22 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(24)]
        public float Float21
        {
            get { return mFloat21; }
            set
            {
                mFloat21 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(23)]
        public float Float20
        {
            get { return mFloat20; }
            set
            {
                mFloat20 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(22)]
        public float Float19
        {
            get { return mFloat19; }
            set
            {
                mFloat19 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(21)]
        public float Float18
        {
            get { return mFloat18; }
            set
            {
                mFloat18 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(20)]
        public float Float17
        {
            get { return mFloat17; }
            set
            {
                mFloat17 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(19)]
        public float Float16
        {
            get { return mFloat16; }
            set
            {
                mFloat16 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(18)]
        public float Float15
        {
            get { return mFloat15; }
            set
            {
                mFloat15 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(17)]
        public float Float14
        {
            get { return mFloat14; }
            set
            {
                mFloat14 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(16)]
        public float Float13
        {
            get { return mFloat13; }
            set
            {
                mFloat13 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(15)]
        public float Float12
        {
            get { return mFloat12; }
            set
            {
                mFloat12 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(14)]
        public float Float11
        {
            get { return mFloat11; }
            set
            {
                mFloat11 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(13)]
        public Vector3ValueLE Vector02
        {
            get { return mVector02; }
            set
            {
                mVector02 = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public Vector3ValueLE Vector01
        {
            get { return mVector01; }
            set
            {
                mVector01 = new Vector3ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public float Float04
        {
            get { return mFloat04; }
            set
            {
                mFloat04 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public float Float03
        {
            get { return mFloat03; }
            set
            {
                mFloat03 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public float Float02
        {
            get { return mFloat02; }
            set
            {
                mFloat02 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public DataList<FloatValue> FloatList02
        {
            get { return mFloatList02; }
            set
            {
                mFloatList02 = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public DataList<ColorValue> ColourList01
        {
            get { return mColorList01; }
            set
            {
                mColorList01 = new DataList<ColorValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public DataList<FloatValue> FloatList01
        {
            get { return mFloatList01; }
            set
            {
                mFloatList01 = new DataList<FloatValue>(handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public ushort Short01
        {
            get { return mShort01; }
            set
            {
                mShort01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public float Float01
        {
            get { return mFloat01; }
            set
            {
                mFloat01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public uint Int03
        {
            get { return mInt03; }
            set
            {
                mInt03 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public uint Int02
        {
            get { return mInt02; }
            set
            {
                mInt02 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(1)]
        public uint Int01
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
            s.Read(out mInt02);
            s.Read(out mInt03);
            s.Read(out mFloat01);
            s.Read(out mShort01);
            mFloatList01 = new DataList<FloatValue>(handler, stream);
            mColorList01 = new DataList<ColorValue>(handler, stream);
            mFloatList02 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat02);
            s.Read(out mFloat03);
            s.Read(out mFloat04);

            mVector01 = new Vector3ValueLE(requestedApiVersion, handler, stream);

            mVector02 = new Vector3ValueLE(requestedApiVersion, handler, stream);

            s.Read(out mFloat11);
            s.Read(out mFloat12);
            s.Read(out mFloat13);
            s.Read(out mFloat14);
            s.Read(out mFloat15);
            s.Read(out mFloat16);
            s.Read(out mFloat17);
            s.Read(out mFloat18);
            s.Read(out mFloat19);
            s.Read(out mFloat20);
            s.Read(out mFloat21);
            s.Read(out mFloat22);
            s.Read(out mFloat23);
            s.Read(out mFloat24);
            s.Read(out mFloat25);
            s.Read(out mFloat26);
            mResource = new ResourceReference(0, handler, mSection, stream);
            s.Read(out mByte01);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);
            s.Write(mInt02);
            s.Write(mInt03);
            s.Write(mFloat01);
            s.Write(mShort01);
            mFloatList01.UnParse(stream);
            mColorList01.UnParse(stream);
            mFloatList02.UnParse(stream);
            s.Write(mFloat02);
            s.Write(mFloat03);
            s.Write(mFloat04);

            mVector01.UnParse(stream);

            mVector02.UnParse(stream);

            s.Write(mFloat11);
            s.Write(mFloat12);
            s.Write(mFloat13);
            s.Write(mFloat14);
            s.Write(mFloat15);
            s.Write(mFloat16);
            s.Write(mFloat17);
            s.Write(mFloat18);
            s.Write(mFloat19);
            s.Write(mFloat20);
            s.Write(mFloat21);
            s.Write(mFloat22);
            s.Write(mFloat23);
            s.Write(mFloat24);
            s.Write(mFloat25);
            s.Write(mFloat26);
            mResource.UnParse(stream);
            s.Write(mByte01);
        }
        #endregion

        public bool Equals(SpriteEffect other)
        {
            return base.Equals(other);
        }
    }
}
