using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;

namespace s3piwrappers.Effects
{
    public class DistributeEffect : Effect, IEquatable<DistributeEffect>
    {
        #region Nested Type: TransformElement
        public class TransformElement : ExportableDataElement, IEquatable<TransformElement>
        {

            public TransformElement(int apiVersion, EventHandler handler, TransformElement basis)
                : base(apiVersion, handler, basis) { }
            public TransformElement(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
            public TransformElement(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mOrientation = new Matrix3x3Value(0, handler);
                mPosition = new Vector3ValueLE(0, handler);
            }

            #region Fields
            private ushort mShort01;
            private float mFloat01;
            private Matrix3x3Value mOrientation;
            private Vector3ValueLE mPosition;
            #endregion

            #region Properties
            [ElementPriority(1)]
            public ushort Short01
            {
                get { return mShort01; }
                set { mShort01 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Float01
            {
                get { return mFloat01; }
                set { mFloat01 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Matrix3x3Value Orientation
            {
                get { return mOrientation; }
                set { mOrientation = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public Vector3ValueLE Position
            {
                get { return mPosition; }
                set { mPosition = value; OnElementChanged(); }
            }
            #endregion

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mShort01);
                s.Read(out mFloat01);
                mOrientation = new Matrix3x3Value(0, handler, stream);
                mPosition = new Vector3ValueLE(0, handler, stream);

            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mShort01);
                s.Write(mFloat01);
                mOrientation.UnParse(stream);
                mPosition.UnParse(stream);
            }
            public bool Equals(TransformElement other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        public DistributeEffect(int apiVersion, EventHandler handler, DistributeEffect basis)
            : base(apiVersion, handler, basis)
        {
        }
        public DistributeEffect(int apiVersion, EventHandler handler, Section section)
            : base(apiVersion, handler, section)
        {
            mTransform = new TransformElement(0, handler);
            mFloatList01 = new DataList<FloatValue>(handler);
            mFloatList02 = new DataList<FloatValue>(handler);
            mFloatList03 = new DataList<FloatValue>(handler);
            mFloatList04 = new DataList<FloatValue>(handler);
            mColourList01 = new DataList<ColourValue>(handler);
            mFloatList05 = new DataList<FloatValue>(handler);
            mItemBList01 = new DataList<ItemB>(handler);
            mResource = new Resource(0, handler);
        }
        public DistributeEffect(int apiVersion, EventHandler handler, Section section, Stream s) : base(apiVersion, handler, section, s) { }


        #region Fields
        private uint mInt01;
        private uint mInt02;
        private string mString01;
        private uint mInt03;
        private byte mByte01;
        private float mFloat01;
        private TransformElement mTransform;
        private DataList<FloatValue> mFloatList01;
        private uint mInt04;
        private DataList<FloatValue> mFloatList02;
        private DataList<FloatValue> mFloatList03;
        private DataList<FloatValue> mFloatList04;
        private uint mInt05;
        private uint mInt06;
        private uint mInt07;
        private uint mInt08;
        private uint mInt09;
        private uint mInt10;
        private DataList<ColourValue> mColourList01;
        private float mFloat02; //LE
        private float mFloat03; //LE
        private float mFloat04; //LE
        private DataList<FloatValue> mFloatList05;
        private uint mInt11;
        private DataList<ItemB> mItemBList01;
        private ulong mLong01;
        private ulong mLong02;
        private ulong mLong03;
        private float mFloat05; //LE
        private float mFloat06; //LE
        private Resource mResource;
        private byte mByte02;
        private uint mInt12;
        #endregion

        #region Properties
        [ElementPriority(1)]
        public uint Int01
        {
            get { return mInt01; }
            set { mInt01 = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public uint Int02
        {
            get { return mInt02; }
            set { mInt02 = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public string String01
        {
            get { return mString01; }
            set { mString01 = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public uint Int03
        {
            get { return mInt03; }
            set { mInt03 = value; OnElementChanged(); }
        }
        [ElementPriority(5)]
        public byte Byte01
        {
            get { return mByte01; }
            set { mByte01 = value; OnElementChanged(); }
        }
        [ElementPriority(6)]
        public float Float01
        {
            get { return mFloat01; }
            set { mFloat01 = value; OnElementChanged(); }
        }
        [ElementPriority(7)]
        public TransformElement Transform
        {
            get { return mTransform; }
            set { mTransform = value; OnElementChanged(); }
        }
        [ElementPriority(8)]
        public DataList<FloatValue> FloatList01
        {
            get { return mFloatList01; }
            set { mFloatList01 = value; OnElementChanged(); }
        }
        [ElementPriority(9)]
        public uint Int04
        {
            get { return mInt04; }
            set { mInt04 = value; OnElementChanged(); }
        }
        [ElementPriority(10)]
        public DataList<FloatValue> FloatList02
        {
            get { return mFloatList02; }
            set { mFloatList02 = value; OnElementChanged(); }
        }
        [ElementPriority(11)]
        public DataList<FloatValue> FloatList03
        {
            get { return mFloatList03; }
            set { mFloatList03 = value; OnElementChanged(); }
        }
        [ElementPriority(12)]
        public DataList<FloatValue> FloatList04
        {
            get { return mFloatList04; }
            set { mFloatList04 = value; OnElementChanged(); }
        }
        [ElementPriority(13)]
        public uint Int05
        {
            get { return mInt05; }
            set { mInt05 = value; OnElementChanged(); }
        }
        [ElementPriority(14)]
        public uint Int06
        {
            get { return mInt06; }
            set { mInt06 = value; OnElementChanged(); }
        }
        [ElementPriority(15)]
        public uint Int07
        {
            get { return mInt07; }
            set { mInt07 = value; OnElementChanged(); }
        }
        [ElementPriority(16)]
        public uint Int08
        {
            get { return mInt08; }
            set { mInt08 = value; OnElementChanged(); }
        }
        [ElementPriority(17)]
        public uint Int09
        {
            get { return mInt09; }
            set { mInt09 = value; OnElementChanged(); }
        }
        [ElementPriority(18)]
        public uint Int10
        {
            get { return mInt10; }
            set { mInt10 = value; OnElementChanged(); }
        }
        [ElementPriority(19)]
        public DataList<ColourValue> ColourList01
        {
            get { return mColourList01; }
            set { mColourList01 = value; OnElementChanged(); }
        }
        [ElementPriority(20)]
        public float Float02
        {
            get { return mFloat02; }
            set { mFloat02 = value; OnElementChanged(); }
        }
        [ElementPriority(21)]
        public float Float03
        {
            get { return mFloat03; }
            set { mFloat03 = value; OnElementChanged(); }
        }
        [ElementPriority(22)]
        public float Float04
        {
            get { return mFloat04; }
            set { mFloat04 = value; OnElementChanged(); }
        }
        [ElementPriority(23)]
        public DataList<FloatValue> FloatList05
        {
            get { return mFloatList05; }
            set { mFloatList05 = value; OnElementChanged(); }
        }
        [ElementPriority(24)]
        public uint Int11
        {
            get { return mInt11; }
            set { mInt11 = value; OnElementChanged(); }
        }
        [ElementPriority(25)]
        public DataList<ItemB> ItemBList01
        {
            get { return mItemBList01; }
            set { mItemBList01 = value; OnElementChanged(); }
        }
        [ElementPriority(26)]
        public ulong Long01
        {
            get { return mLong01; }
            set { mLong01 = value; OnElementChanged(); }
        }
        [ElementPriority(27)]
        public ulong Long02
        {
            get { return mLong02; }
            set { mLong02 = value; OnElementChanged(); }
        }
        [ElementPriority(28)]
        public ulong Long03
        {
            get { return mLong03; }
            set { mLong03 = value; OnElementChanged(); }
        }
        [ElementPriority(29)]
        public float Float05
        {
            get { return mFloat05; }
            set { mFloat05 = value; OnElementChanged(); }
        }
        [ElementPriority(30)]
        public float Float06
        {
            get { return mFloat06; }
            set { mFloat06 = value; OnElementChanged(); }
        }
        [ElementPriority(31)]
        public Resource ResourceKey
        {
            get { return mResource; }
            set { mResource = value; OnElementChanged(); }
        }
        [ElementPriority(32)]
        public byte Byte02
        {
            get { return mByte02; }
            set { mByte02 = value; OnElementChanged(); }
        }
        [ElementPriority(33)]
        public uint Int12
        {
            get { return mInt12; }
            set { mInt12 = value; OnElementChanged(); }
        }
        #endregion

        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mInt01);
            s.Read(out mInt02);
            s.Read(out mString01, StringType.ZeroDelimited);
            s.Read(out mInt03);
            s.Read(out mByte01);
            s.Read(out mFloat01);
            mTransform = new TransformElement(0, handler, stream);
            mFloatList01 = new DataList<FloatValue>(handler, stream);
            s.Read(out mInt04);
            mFloatList02 = new DataList<FloatValue>(handler, stream);
            mFloatList03 = new DataList<FloatValue>(handler, stream);
            mFloatList04 = new DataList<FloatValue>(handler, stream);
            s.Read(out mInt05);
            s.Read(out mInt06);
            s.Read(out mInt07);
            s.Read(out mInt08);
            s.Read(out mInt09);
            s.Read(out mInt10);
            mColourList01 = new DataList<ColourValue>(handler, stream);
            s.Read(out mFloat02, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat03, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat04, ByteOrder.LittleEndian); //LE
            mFloatList05 = new DataList<FloatValue>(handler, stream);
            s.Read(out mInt11);
            mItemBList01 = new DataList<ItemB>(handler, stream);
            s.Read(out mLong01);
            s.Read(out mLong02);
            s.Read(out mLong03);
            s.Read(out mFloat05, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat06, ByteOrder.LittleEndian); //LE
            mResource = new Resource(0, handler, stream);
            s.Read(out mByte02);
            s.Read(out mInt12);
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mInt01);
            s.Write(mInt02);
            s.Write(mString01, StringType.ZeroDelimited);
            s.Write(mInt03);
            s.Write(mByte01);
            s.Write(mFloat01);
            mTransform.UnParse(stream);
            mFloatList01.UnParse(stream);
            s.Write(mInt04);
            mFloatList02.UnParse(stream);
            mFloatList03.UnParse(stream);
            mFloatList04.UnParse(stream);
            s.Write(mInt05);
            s.Write(mInt06);
            s.Write(mInt07);
            s.Write(mInt08);
            s.Write(mInt09);
            s.Write(mInt10);
            mColourList01.UnParse(stream);
            s.Write(mFloat02, ByteOrder.LittleEndian); //LE
            s.Write(mFloat03, ByteOrder.LittleEndian); //LE
            s.Write(mFloat04, ByteOrder.LittleEndian); //LE
            mFloatList05.UnParse(stream);
            s.Write(mInt11);
            mItemBList01.UnParse(stream);
            s.Write(mLong01);
            s.Write(mLong02);
            s.Write(mLong03);
            s.Write(mFloat05, ByteOrder.LittleEndian); //LE
            s.Write(mFloat06, ByteOrder.LittleEndian); //LE
            mResource.UnParse(stream);
            s.Write(mByte02);
            s.Write(mInt12);
        }

        public bool Equals(DistributeEffect other)
        {
            return base.Equals(other);
        }
    }
}