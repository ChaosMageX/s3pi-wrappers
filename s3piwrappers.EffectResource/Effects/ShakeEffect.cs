using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class ShakeEffect : Effect, IEquatable<ShakeEffect>
    {
        public ShakeEffect(int apiVersion, EventHandler handler, ShakeEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public ShakeEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mFloatList01 = new DataList<FloatValue>(handler);
            mFloatList02 = new DataList<FloatValue>(handler);
        }

        public ShakeEffect(int apiVersion, EventHandler handler, ISection section, Stream s) : base(apiVersion, handler, section, s)
        {
        }


        private float mFloat01;
        private float mFloat02;
        private DataList<FloatValue> mFloatList01;
        private DataList<FloatValue> mFloatList02;
        private float mFloat03;
        private byte mByte01;
        private float mFloat04;


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
        public DataList<FloatValue> FloatList01
        {
            get { return mFloatList01; }
            set
            {
                mFloatList01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public DataList<FloatValue> FloatList02
        {
            get { return mFloatList02; }
            set
            {
                mFloatList02 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public float Float03
        {
            get { return mFloat03; }
            set
            {
                mFloat03 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public byte Byte01
        {
            get { return mByte01; }
            set
            {
                mByte01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public float Float04
        {
            get { return mFloat04; }
            set
            {
                mFloat04 = value;
                OnElementChanged();
            }
        }


        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFloat01);
            s.Read(out mFloat02);
            mFloatList01 = new DataList<FloatValue>(handler, stream);
            mFloatList02 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat03);
            s.Read(out mByte01);
            s.Read(out mFloat04);
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFloat01);
            s.Write(mFloat02);
            mFloatList01.UnParse(stream);
            mFloatList02.UnParse(stream);
            s.Write(mFloat03);
            s.Write(mByte01);
            s.Write(mFloat04);
        }


        public bool Equals(ShakeEffect other)
        {
            return base.Equals(other);
        }
    }
}
