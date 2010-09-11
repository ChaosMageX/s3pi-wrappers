using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;

namespace s3piwrappers.Effects
{
    public class CameraEffect : Effect, IEquatable<CameraEffect>
    {
        public CameraEffect(int apiVersion, EventHandler handler, CameraEffect basis)
            : base(apiVersion, handler, basis)
        {
        }
        public CameraEffect(int apiVersion, EventHandler handler, Section section)
            : base(apiVersion, handler, section)
        {
            mFloatList01 = new DataList<FloatValue>(handler);
            mFloatList02 = new DataList<FloatValue>(handler);
            mFloatList03 = new DataList<FloatValue>(handler);
            mFloatList04 = new DataList<FloatValue>(handler);
            mFloatList05 = new DataList<FloatValue>(handler);
            mFloatList06 = new DataList<FloatValue>(handler);
            mFloatList07 = new DataList<FloatValue>(handler);
        }
        public CameraEffect(int apiVersion, EventHandler handler, Section section, Stream s) : base(apiVersion, handler, section, s) { }

        #region Fields
        private UInt32 mInt01;
        private UInt16 mShort01;
        private float mFloat01;
        private DataList<FloatValue> mFloatList01;
        private DataList<FloatValue> mFloatList02;
        private DataList<FloatValue> mFloatList03;
        private DataList<FloatValue> mFloatList04;
        private DataList<FloatValue> mFloatList05;
        private DataList<FloatValue> mFloatList06;
        private DataList<FloatValue> mFloatList07;
        private UInt64 mLong01;
        private UInt16 mShort02;
        #endregion

        #region Properties
        [ElementPriority(1)]
        public uint Int01
        {
            get { return mInt01; }
            set { mInt01 = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public ushort Short01
        {
            get { return mShort01; }
            set { mShort01 = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public float Float01
        {
            get { return mFloat01; }
            set { mFloat01 = value; OnElementChanged(); }
        }

        public DataList<FloatValue> FloatList01
        {
            get { return mFloatList01; }
            set { mFloatList01 = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public DataList<FloatValue> FloatList02
        {
            get { return mFloatList02; }
            set { mFloatList02 = value; OnElementChanged(); }
        }
        [ElementPriority(5)]
        public DataList<FloatValue> FloatList03
        {
            get { return mFloatList03; }
            set { mFloatList03 = value; OnElementChanged(); }
        }
        [ElementPriority(6)]
        public DataList<FloatValue> FloatList04
        {
            get { return mFloatList04; }
            set { mFloatList04 = value; OnElementChanged(); }
        }
        [ElementPriority(7)]
        public DataList<FloatValue> FloatList05
        {
            get { return mFloatList05; }
            set { mFloatList05 = value; OnElementChanged(); }
        }
        [ElementPriority(8)]
        public DataList<FloatValue> FloatList06
        {
            get { return mFloatList06; }
            set { mFloatList06 = value; OnElementChanged(); }
        }
        [ElementPriority(9)]
        public DataList<FloatValue> FloatList07
        {
            get { return mFloatList07; }
            set { mFloatList07 = value; OnElementChanged(); }
        }
        [ElementPriority(10)]
        public ulong Long01
        {
            get { return mLong01; }
            set { mLong01 = value; OnElementChanged(); }
        }
        [ElementPriority(11)]
        public ushort Short02
        {
            get { return mShort02; }
            set { mShort02 = value; OnElementChanged(); }
        }
        #endregion

        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mInt01);
            s.Read(out mShort01);
            s.Read(out mFloat01);
            mFloatList01 = new DataList<FloatValue>(handler, stream);
            mFloatList02 = new DataList<FloatValue>(handler, stream);
            mFloatList03 = new DataList<FloatValue>(handler, stream);
            mFloatList04 = new DataList<FloatValue>(handler, stream);
            mFloatList05 = new DataList<FloatValue>(handler, stream);
            mFloatList06 = new DataList<FloatValue>(handler, stream);
            mFloatList07 = new DataList<FloatValue>(handler, stream);
            s.Read(out mLong01);
            s.Read(out mShort02);
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mInt01);
            s.Write(mShort01);
            s.Write(mFloat01);
            mFloatList01.UnParse(stream);
            mFloatList02.UnParse(stream);
            mFloatList03.UnParse(stream);
            mFloatList04.UnParse(stream);
            mFloatList05.UnParse(stream);
            mFloatList06.UnParse(stream);
            mFloatList07.UnParse(stream);
            s.Write(mLong01);
            s.Write(mShort02);
        }

        public bool Equals(CameraEffect other)
        {
            return base.Equals(other);
        }
    }
}