using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class ScreenEffect : Effect, IEquatable<ScreenEffect>
    {
        public class Item : DataElement, IEquatable<Item>
        {
            public Item(int APIversion, EventHandler handler, Item basis)
                : base(APIversion, handler, basis)
            {
            }

            public Item(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }

            public Item(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mByteList01 = new DataList<ByteValue>(handler);
            }

            private Byte mByte01;
            private Byte mByte02;
            private UInt64 mLong01;
            private DataList<ByteValue> mByteList01;


            [ElementPriority(1)]
            public byte Byte01
            {
                get { return mByte01; }
                set
                {
                    mByte01 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public byte Byte02
            {
                get { return mByte02; }
                set
                {
                    mByte02 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public ulong Long01
            {
                get { return mLong01; }
                set
                {
                    mLong01 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(4)]
            public DataList<ByteValue> ByteList01
            {
                get { return mByteList01; }
                set
                {
                    mByteList01 = value;
                    OnElementChanged();
                }
            }


            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mByte01);
                s.Read(out mByte02);
                s.Read(out mLong01);
                mByteList01 = new DataList<ByteValue>(handler, stream);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mByte01);
                s.Write(mByte02);
                s.Write(mLong01);
                mByteList01.UnParse(stream);
            }

            public bool Equals(Item other)
            {
                return base.Equals(other);
            }
        }


        public ScreenEffect(int apiVersion, EventHandler handler, ScreenEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public ScreenEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mColourList01 = new DataList<ColourValue>(handler);
            mFloatList01 = new DataList<FloatValue>(handler);
            mFloatList02 = new DataList<FloatValue>(handler);
            mItems = new DataList<Item>(handler);
            mVector2List01 = new DataList<Vector2Value>(handler);
            mIntList01 = new DataList<UInt32Value>(handler);
            mVector3List01 = new DataList<Vector3ValueLE>(handler);
            mVector2List02 = new DataList<Vector2ValueLE>(handler);
            mIntList02 = new DataList<UInt32Value>(handler);
        }

        public ScreenEffect(int apiVersion, EventHandler handler, ISection section, Stream s) : base(apiVersion, handler, section, s)
        {
        }

        private Byte mByte01;
        private UInt32 mInt01;
        private DataList<ColourValue> mColourList01;
        private DataList<FloatValue> mFloatList01;
        private DataList<FloatValue> mFloatList02;
        private float mFloat01;
        private UInt32 mInt02;
        private UInt32 mInt03;
        private UInt32 mInt04;
        private UInt64 mLong01;
        private DataList<Item> mItems;
        private DataList<Vector2Value> mVector2List01;
        private DataList<UInt32Value> mIntList01;
        private DataList<Vector3ValueLE> mVector3List01;
        private DataList<Vector2ValueLE> mVector2List02;
        private DataList<UInt32Value> mIntList02;


        [ElementPriority(1)]
        public byte Byte01
        {
            get { return mByte01; }
            set
            {
                mByte01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public uint Int01
        {
            get { return mInt01; }
            set
            {
                mInt01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public DataList<ColourValue> ColourList01
        {
            get { return mColourList01; }
            set
            {
                mColourList01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public DataList<FloatValue> FloatList01
        {
            get { return mFloatList01; }
            set
            {
                mFloatList01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public DataList<FloatValue> FloatList02
        {
            get { return mFloatList01; }
            set
            {
                mFloatList01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public float Float01
        {
            get { return mFloat01; }
            set
            {
                mFloat01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public uint Int02
        {
            get { return mInt02; }
            set
            {
                mInt02 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public uint Int03
        {
            get { return mInt03; }
            set
            {
                mInt03 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public uint Int04
        {
            get { return mInt04; }
            set
            {
                mInt04 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public ulong Long01
        {
            get { return mLong01; }
            set
            {
                mLong01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public DataList<Item> Items
        {
            get { return mItems; }
            set
            {
                mItems = value;
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public DataList<Vector2Value> Vector2List01
        {
            get { return mVector2List01; }
            set
            {
                mVector2List01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(13)]
        public DataList<UInt32Value> IntList01
        {
            get { return mIntList01; }
            set
            {
                mIntList01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(14)]
        public DataList<Vector3ValueLE> Vector3List01
        {
            get { return mVector3List01; }
            set
            {
                mVector3List01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(15)]
        public DataList<Vector2ValueLE> Vector2List02
        {
            get { return mVector2List02; }
            set
            {
                mVector2List02 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(16)]
        public DataList<UInt32Value> IntList02
        {
            get { return mIntList02; }
            set
            {
                mIntList02 = value;
                OnElementChanged();
            }
        }


        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mByte01);
            s.Read(out mInt01);
            mColourList01 = new DataList<ColourValue>(handler, stream);
            mFloatList01 = new DataList<FloatValue>(handler, stream);
            mFloatList02 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat01);
            s.Read(out mInt02);
            s.Read(out mInt03);
            s.Read(out mInt04);
            s.Read(out mLong01);
            mItems = new DataList<Item>(handler, stream);
            mVector2List01 = new DataList<Vector2Value>(handler, stream);
            mIntList01 = new DataList<UInt32Value>(handler, stream);
            mVector3List01 = new DataList<Vector3ValueLE>(handler, stream);
            mVector2List02 = new DataList<Vector2ValueLE>(handler, stream);
            mIntList02 = new DataList<UInt32Value>(handler, stream);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mByte01);
            s.Write(mInt01);
            mColourList01.UnParse(stream);
            mFloatList01.UnParse(stream);
            mFloatList02.UnParse(stream);
            s.Write(mFloat01);
            s.Write(mInt02);
            s.Write(mInt03);
            s.Write(mInt04);
            s.Write(mLong01);
            mItems.UnParse(stream);
            mVector2List01.UnParse(stream);
            mIntList01.UnParse(stream);
            mVector3List01.UnParse(stream);
            mVector2List02.UnParse(stream);
            mIntList02.UnParse(stream);
        }

        public bool Equals(ScreenEffect other)
        {
            return base.Equals(other);
        }
    }
}
