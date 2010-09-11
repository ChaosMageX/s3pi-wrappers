using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;

namespace s3piwrappers.Effects
{
    public class ModelEffect : Effect, IEquatable<ModelEffect>
    {
        public ModelEffect(int apiVersion, EventHandler handler, ModelEffect basis)
            : base(apiVersion, handler, basis)
        {
        }
        public ModelEffect(int apiVersion, EventHandler handler, Section section)
            : base(apiVersion, handler, section)
        {
            mItems = new DataList<Item>(handler);
        }
        public ModelEffect(int apiVersion, EventHandler handler, Section section, Stream s) : base(apiVersion, handler, section, s) { }
        
        #region Nested Type: Item
        public class Item : ExportableDataElement, IEquatable<Item>
        {
            public Item(int apiVersion, EventHandler handler, Item basis)
                : base(apiVersion, handler)
            {
                MemoryStream ms = new MemoryStream();
                basis.UnParse(ms);
                ms.Position = 0L;
                Parse(ms);
            }
            public Item(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public Item(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }

            #region Fields
            private float mFloat01; //LE
            private float mFloat02; //LE
            private DataList<FloatValue> mFloatList01;
            private UInt32 mInt01;
            private UInt32 mInt02;
            private Byte mByte01;
            private Byte mByte02;
            #endregion

            #region Properties
            [ElementPriority(1)]
            public float Float01
            {
                get { return mFloat01; }
                set { mFloat01 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Float02
            {
                get { return mFloat02; }
                set { mFloat02 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public DataList<FloatValue> FloatList01
            {
                get { return mFloatList01; }
                set { mFloatList01 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public uint Int01
            {
                get { return mInt01; }
                set { mInt01 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public uint Int02
            {
                get { return mInt02; }
                set { mInt02 = value; OnElementChanged(); }
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
            #endregion

            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mFloat01, ByteOrder.LittleEndian);
                s.Read(out mFloat02, ByteOrder.LittleEndian);
                mFloatList01 = new DataList<FloatValue>(handler, stream);
                s.Read(out mInt01);
                s.Read(out mInt02);
                s.Read(out mByte01);
                s.Read(out mByte02);

            }
            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mFloat01, ByteOrder.LittleEndian);
                s.Write(mFloat02, ByteOrder.LittleEndian);
                mFloatList01.UnParse(stream);
                s.Write(mInt01);
                s.Write(mInt02);
                s.Write(mByte01);
                s.Write(mByte02);
            }

            public bool Equals(Item other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        #region Fields
        private UInt32 mInt01;
        private UInt64 mLong01;
        private float mFloat01;
        private float mFloat02; //LE
        private float mFloat03; //LE
        private float mFloat04; //LE
        private float mFloat05;
        private DataList<Item> mItems;
        private UInt64 mLong02;
        private byte mByte01;
        #endregion

        #region Properties
        [ElementPriority(1)]
        public uint Int01
        {
            get { return mInt01; }
            set { mInt01 = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public ulong Long01
        {
            get { return mLong01; }
            set { mLong01 = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public float Float01
        {
            get { return mFloat01; }
            set { mFloat01 = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public float Float02
        {
            get { return mFloat02; }
            set { mFloat02 = value; OnElementChanged(); }
        }
        [ElementPriority(5)]
        public float Float03
        {
            get { return mFloat03; }
            set { mFloat03 = value; OnElementChanged(); }
        }
        [ElementPriority(6)]
        public float Float04
        {
            get { return mFloat04; }
            set { mFloat04 = value; OnElementChanged(); }
        }
        [ElementPriority(7)]
        public float Float05
        {
            get { return mFloat05; }
            set { mFloat05 = value; OnElementChanged(); }
        }
        [ElementPriority(8)]
        public DataList<Item> Items
        {
            get { return mItems; }
            set { mItems = value; OnElementChanged(); }
        }
        [ElementPriority(9)]
        public ulong Long02
        {
            get { return mLong02; }
            set { mLong02 = value; OnElementChanged(); }
        }
        [ElementPriority(10)]
        public byte Byte01
        {
            get { return mByte01; }
            set { mByte01 = value; OnElementChanged(); }
        }
        #endregion

        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mInt01);
            s.Read(out mLong01);
            s.Read(out mFloat01);
            s.Read(out mFloat02, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat03, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat04, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat05);
            mItems = new DataList<Item>(handler, stream);
            s.Read(out mLong02);
            s.Read(out mByte01);
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mInt01);
            s.Write(mLong01);
            s.Write(mFloat01);
            s.Write(mFloat02, ByteOrder.LittleEndian); //LE
            s.Write(mFloat03, ByteOrder.LittleEndian); //LE
            s.Write(mFloat05, ByteOrder.LittleEndian); //LE
            s.Write(mFloat04);
            mItems.UnParse(stream);
            s.Write(mLong02);
            s.Write(mByte01);
        }


        public bool Equals(ModelEffect other)
        {
            return base.Equals(other);
        }
    }
}