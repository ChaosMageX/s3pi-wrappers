using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;

namespace s3piwrappers.Effects
{
    public class DecalEffect : Effect, IEquatable<DecalEffect>
    {
        public DecalEffect(int apiVersion, EventHandler handler, DecalEffect basis)
            : base(apiVersion, handler, basis)
        {
        }
        public DecalEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mFloatList01 = new DataList<FloatValue>(handler);
            mFloatList02 = new DataList<FloatValue>(handler);
            mFloatList03 = new DataList<FloatValue>(handler);
            mColourList01 = new DataList<ColourValue>(handler);
            mFloatList04 = new DataList<FloatValue>(handler);
        }
        public DecalEffect(int apiVersion, EventHandler handler, ISection section, Stream s) : base(apiVersion, handler, section, s) { }


        #region Fields

        private UInt32 mUint01;
        private UInt64 mDdsResource;
        private Byte mByte01;
        private float mFloat01;
        private Byte mByte02;
        private float mFloat02;
        private DataList<FloatValue> mFloatList01;
        private DataList<FloatValue> mFloatList02;
        private DataList<FloatValue> mFloatList03;
        private DataList<ColourValue> mColourList01;
        private DataList<FloatValue> mFloatList04;
        private float mFloat04;
        private float mFloat05;
        private float mFloat06;
        private float mFloat07;
        private float mFloat08; //LE
        private float mFloat09; //LE
        private UInt64 mLong01;
        private byte mByte03; //version 2+

        #endregion

        #region Properties
        [ElementPriority(1)]
        public uint Uint02
        {
            get { return mUint01; }
            set { mUint01 = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public ulong DdsResource
        {
            get { return mDdsResource; }
            set { mDdsResource = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public byte Byte01
        {
            get { return mByte01; }
            set { mByte01 = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public float Float01
        {
            get { return mFloat01; }
            set { mFloat01 = value; OnElementChanged(); }
        }
        [ElementPriority(5)]
        public byte Byte02
        {
            get { return mByte02; }
            set { mByte02 = value; OnElementChanged(); }
        }
        [ElementPriority(6)]
        public float Float02
        {
            get { return mFloat02; }
            set { mFloat02 = value; OnElementChanged(); }
        }

        public DataList<FloatValue> FloatList01
        {
            get { return mFloatList01; }
            set { mFloatList01 = value; OnElementChanged(); }
        }
        [ElementPriority(7)]
        public DataList<FloatValue> FloatList02
        {
            get { return mFloatList02; }
            set { mFloatList02 = value; OnElementChanged(); }
        }
        [ElementPriority(8)]
        public DataList<FloatValue> FloatList03
        {
            get { return mFloatList03; }
            set { mFloatList03 = value; OnElementChanged(); }
        }
        [ElementPriority(9)]
        public DataList<ColourValue> ColourList01
        {
            get { return mColourList01; }
            set { mColourList01 = value; OnElementChanged(); }
        }
        [ElementPriority(10)]
        public DataList<FloatValue> FloatList04
        {
            get { return mFloatList04; }
            set { mFloatList04 = value; OnElementChanged(); }
        }
        [ElementPriority(11)]
        public float Float04
        {
            get { return mFloat04; }
            set { mFloat04 = value; OnElementChanged(); }
        }
        [ElementPriority(12)]
        public float Float05
        {
            get { return mFloat05; }
            set { mFloat05 = value; OnElementChanged(); }
        }
        [ElementPriority(13)]
        public float Float06
        {
            get { return mFloat06; }
            set { mFloat06 = value; OnElementChanged(); }
        }
        [ElementPriority(14)]
        public float Float07
        {
            get { return mFloat07; }
            set { mFloat07 = value; OnElementChanged(); }
        }
        [ElementPriority(15)]
        public float Float08
        {
            get { return mFloat08; }
            set { mFloat08 = value; OnElementChanged(); }
        }
        [ElementPriority(16)]
        public float Float09
        {
            get { return mFloat09; }
            set { mFloat09 = value; OnElementChanged(); }
        }
        [ElementPriority(17)]
        public ulong Long01
        {
            get { return mLong01; }
            set { mLong01 = value; OnElementChanged(); }
        }
        [ElementPriority(18)]
        public byte Byte03
        {
            get { return mByte03; }
            set { mByte03 = value; OnElementChanged(); }
        }
        #endregion

        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mUint01);
            s.Read(out mDdsResource);
            s.Read(out mByte01);
            s.Read(out mFloat01);
            s.Read(out mByte02);
            s.Read(out mFloat02);
            mFloatList01 = new DataList<FloatValue>(handler, stream);
            mFloatList02 = new DataList<FloatValue>(handler, stream);
            mFloatList03 = new DataList<FloatValue>(handler, stream);
            mColourList01 = new DataList<ColourValue>(handler, stream);
            mFloatList04 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat04);
            s.Read(out mFloat05);
            s.Read(out mFloat06);
            s.Read(out mFloat07);
            s.Read(out mFloat08, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat09, ByteOrder.LittleEndian); //LE
            s.Read(out mLong01);
            if (mSection.Version >= 2 && stream.Position < stream.Length) s.Read(out mByte03); //version 2+
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mUint01);
            s.Write(mDdsResource);
            s.Write(mByte01);
            s.Write(mFloat01);
            s.Write(mByte02);
            s.Write(mFloat02);
            mFloatList01.UnParse(stream);
            mFloatList02.UnParse(stream);
            mFloatList03.UnParse(stream);
            mColourList01.UnParse(stream);
            mFloatList04.UnParse(stream);
            s.Write(mFloat04);
            s.Write(mFloat05);
            s.Write(mFloat06);
            s.Write(mFloat07);
            s.Write(mFloat08, ByteOrder.LittleEndian); //LE
            s.Write(mFloat09, ByteOrder.LittleEndian); //LE
            s.Write(mLong01);
            if (mSection.Version >= 2) s.Write(mByte03); //version 2+
        }

        public override System.Collections.Generic.List<string> ContentFields
        {
            get
            {
                var fields = base.ContentFields;
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