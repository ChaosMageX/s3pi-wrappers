using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;

namespace s3piwrappers
{
    public class VisualEffect : SectionData, IEquatable<VisualEffect>
    {

        #region Index
        public class Index : SectionData, IEquatable<Index>
        {


            #region Fields
            private byte mBlockType;
            private uint mInt01;
            private ushort mShort01;
            private float mFloat01;
            private Matrix3x3Value mOrientation;
            private Vector3ValueLE mPosition;
            private byte mByte01;
            private byte mByte02;
            private DataList<Vector3Value> mVector3List01;
            private float mFloat02 = 1f;
            private float mFloat03 = 1f;
            private float mFloat04 = 1f;
            private float mFloat05 = 1f;
            private float mFloat06 = 1f;
            private float mFloat07 = 1f;
            private float mFloat08;
            private float mFloat09;
            private ushort mShort02;
            private ushort mShort03;
            private float mFloat10;
            private uint mBlockIndex;
            private byte mByte03; //version 2+
            private byte mByte04; //version 2+

            #endregion

            #region Properties

            public Index(int apiVersion, EventHandler handler, ISection section)
                : base(apiVersion, handler, section)
            {
                mOrientation = new Matrix3x3Value(0, handler);
                mPosition = new Vector3ValueLE(0, handler);
                mVector3List01 = new DataList<Vector3Value>(handler);
            }

            public Index(int apiVersion, EventHandler handler, ISection section, Stream s)
                : base(apiVersion, handler, section, s)
            {
            }

            public Index(int apiVersion, EventHandler handler, SectionData basis)
                : base(apiVersion, handler, basis)
            {
            }

            [ElementPriority(1)]
            public byte BlockType
            {
                get { return mBlockType; }
                set { mBlockType = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
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
            [ElementPriority(4)]
            public Matrix3x3Value Orientation
            {
                get { return mOrientation; }
                set { mOrientation = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public Vector3ValueLE Position
            {
                get { return mPosition; }
                set { mPosition = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public byte Byte01
            {
                get { return mByte01; }
                set { mByte01 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public byte Byte02
            {
                get { return mByte02; }
                set { mByte02 = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public DataList<Vector3Value> Vector3List01
            {
                get { return mVector3List01; }
                set { mVector3List01 = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public float Float02
            {
                get { return mFloat02; }
                set { mFloat02 = value; OnElementChanged(); }
            }
            [ElementPriority(10)]
            public float Float03
            {
                get { return mFloat03; }
                set { mFloat03 = value; OnElementChanged(); }
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
            public ushort Short02
            {
                get { return mShort02; }
                set { mShort02 = value; OnElementChanged(); }
            }
            [ElementPriority(18)]
            public ushort Short03
            {
                get { return mShort03; }
                set { mShort03 = value; OnElementChanged(); }
            }
            [ElementPriority(19)]
            public float Float10
            {
                get { return mFloat10; }
                set { mFloat10 = value; OnElementChanged(); }
            }
            [ElementPriority(20)]
            public uint BlockIndex
            {
                get { return mBlockIndex; }
                set { mBlockIndex = value; OnElementChanged(); }
            }
            [ElementPriority(21)]
            public byte Byte03
            {
                get { return mByte03; }
                set { mByte03 = value; OnElementChanged(); }
            }
            [ElementPriority(22)]
            public byte Byte04
            {
                get { return mByte04; }
                set { mByte04 = value; OnElementChanged(); }
            }
            #endregion

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mBlockType);
                s.Read(out mInt01);
                s.Read(out mShort01);
                s.Read(out mFloat01);
                mOrientation = new Matrix3x3Value(0, handler, stream);
                mPosition = new Vector3ValueLE(0, handler, stream);
                s.Read(out mByte01);
                s.Read(out mByte02);
                mVector3List01 = new DataList<Vector3Value>(handler, stream);
                s.Read(out mFloat02); //1.0
                s.Read(out mFloat03); //1.0
                s.Read(out mFloat04); //1.0
                s.Read(out mFloat05); //1.0
                s.Read(out mFloat06); //1.0
                s.Read(out mFloat07); //1.0
                s.Read(out mFloat08);
                s.Read(out mFloat09);
                s.Read(out mShort02);
                s.Read(out mShort03);
                s.Read(out mFloat10);
                s.Read(out mBlockIndex);
                if (stream.Position == stream.Length) return; //importing older sections
                if (mSection.Version >= 2)
                {
                    s.Read(out mByte03); //version 2+
                    s.Read(out mByte04); //version 2+
                }
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mBlockType);
                s.Write(mInt01);
                s.Write(mShort01);
                s.Write(mFloat01);
                mOrientation.UnParse(stream);
                mPosition.UnParse(stream);
                s.Write(mByte01);
                s.Write(mByte02);
                mVector3List01.UnParse(stream);
                s.Write(mFloat02); //1.0
                s.Write(mFloat03); //1.0
                s.Write(mFloat04); //1.0
                s.Write(mFloat05); //1.0
                s.Write(mFloat06); //1.0
                s.Write(mFloat07); //1.0
                s.Write(mFloat08);
                s.Write(mFloat09);
                s.Write(mShort02);
                s.Write(mShort03);
                s.Write(mFloat10);
                s.Write(mBlockIndex);
                if (mSection.Version >= 2)
                {
                    s.Write(mByte03); //version 2+
                    s.Write(mByte04); //version 2+
                }
            }


            public bool Equals(Index other)
            {
                return base.Equals(other);
            }
        }
        #endregion



        public VisualEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mFloatList01 = new DataList<FloatValue>(handler);
            mItems = new SectionDataList<Index>(handler, mSection);
        }

        public VisualEffect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }

        public VisualEffect(int apiVersion, EventHandler handler, VisualEffect basis)
            : base(apiVersion, handler, basis)
        {
        }
        #region Fields
        private UInt32 mInt01;
        private UInt32 mInt02;
        private UInt32 mInt03;
        private float mFloat01; //LE
        private float mFloat02; //LE
        private UInt32 mInt04;
        private byte mByte01;
        private DataList<FloatValue> mFloatList01;
        private float mFloat03; //LE
        private float mFloat04; //LE
        private float mFloat05; //LE
        private UInt32 mInt05;
        private SectionDataList<Index> mItems;
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
        public uint Int03
        {
            get { return mInt03; }
            set { mInt03 = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public float Float01
        {
            get { return mFloat01; }
            set { mFloat01 = value; OnElementChanged(); }
        }
        [ElementPriority(5)]
        public float Float02
        {
            get { return mFloat02; }
            set { mFloat02 = value; OnElementChanged(); }
        }
        [ElementPriority(6)]
        public uint Int04
        {
            get { return mInt04; }
            set { mInt04 = value; OnElementChanged(); }
        }
        [ElementPriority(7)]
        public byte Byte01
        {
            get { return mByte01; }
            set { mByte01 = value; OnElementChanged(); }
        }
        [ElementPriority(8)]
        public DataList<FloatValue> FloatList01
        {
            get { return mFloatList01; }
            set { mFloatList01 = value; OnElementChanged(); }
        }
        [ElementPriority(9)]
        public float Float03
        {
            get { return mFloat03; }
            set { mFloat03 = value; OnElementChanged(); }
        }
        [ElementPriority(10)]
        public float Float04
        {
            get { return mFloat04; }
            set { mFloat04 = value; OnElementChanged(); }
        }
        [ElementPriority(11)]
        public float Float05
        {
            get { return mFloat05; }
            set { mFloat05 = value; OnElementChanged(); }
        }
        [ElementPriority(12)]
        public uint Int05
        {
            get { return mInt05; }
            set { mInt05 = value; OnElementChanged(); }
        }
        [ElementPriority(13)]
        public SectionDataList<Index> Items
        {
            get { return mItems; }
            set { mItems = value; OnElementChanged(); }
        }


        #endregion

        public bool Equals(VisualEffect other)
        {
            return base.Equals(other);
        }
        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mInt01);
            s.Read(out mInt02);
            s.Read(out mInt03);
            s.Read(out mFloat01, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat02, ByteOrder.LittleEndian); //LE
            s.Read(out mInt04);
            s.Read(out mByte01);
            mFloatList01 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat03, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat04, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat05, ByteOrder.LittleEndian); //LE
            s.Read(out mInt05);
            mItems = new SectionDataList<Index>(handler, mSection, stream);
        }
        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mInt01);
            s.Write(mInt02);
            s.Write(mInt03);
            s.Write(mFloat01, ByteOrder.LittleEndian); //LE
            s.Write(mFloat02, ByteOrder.LittleEndian); //LE
            s.Write(mInt04);
            s.Write(mByte01);
            mFloatList01.UnParse(stream);
            s.Write(mFloat03, ByteOrder.LittleEndian); //LE
            s.Write(mFloat04, ByteOrder.LittleEndian); //LE
            s.Write(mFloat05, ByteOrder.LittleEndian); //LE
            s.Write(mInt05);
            mItems.UnParse(stream);
        }
    }
}