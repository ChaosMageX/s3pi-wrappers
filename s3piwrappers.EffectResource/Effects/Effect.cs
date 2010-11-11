using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;

namespace s3piwrappers.Effects
{
    public class Effect : SectionData, IEquatable<Effect>
    {

                public class ResourceReference : DataElement
        {
            public ResourceReference(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public ResourceReference(int apiVersion, EventHandler handler, ResourceReference basis)
                : base(apiVersion, handler)
            {
                mInstanceId = basis.mInstanceId;
                mByte01 = basis.mByte01;
                mByte02 = basis.mByte02;
                mInt01 = basis.mInt01;
                mByte03 = basis.mByte03;
                mByte04 = basis.mByte04;
                mShort01 = basis.mShort01;
                mInt02 = basis.mInt02;
                mLong01 = basis.mLong01;
            }
            public ResourceReference(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }

                        private UInt64 mInstanceId;
            private byte mByte01;
            private byte mByte02;
            private UInt32 mInt01; //if Byte02 & 0x80
            private byte mByte03;
            private byte mByte04;
            private UInt16 mShort01;
            private UInt32 mInt02;
            private UInt64 mLong01;
            

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mInstanceId);
                s.Read(out mByte01);
                s.Read(out mByte02);
                if ((Byte02 & 0x80) == 0x80) s.Read(out mInt01);
                s.Read(out mByte03);
                s.Read(out mByte04);
                s.Read(out mShort01);
                s.Read(out mInt02);
                s.Read(out mLong01);
            }
            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(Instance);
                s.Write(Byte01);
                s.Write(Byte02);
                if ((Byte02 & 0x80) == 0x80) s.Write(Int01);
                s.Write(Byte03);
                s.Write(Byte04);
                s.Write(Short01);
                s.Write(Int02);
                s.Write(Long01);
            }

            [ElementPriority(1)]
            public ulong Instance
            {
                get { return mInstanceId; }
                set { mInstanceId = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public byte Byte01
            {
                get { return mByte01; }
                set { mByte01 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public byte Byte02
            {
                get { return mByte02; }
                set { mByte02 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public uint Int01
            {
                get { return mInt01; }
                set { mInt01 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public byte Byte03
            {
                get { return mByte03; }
                set { mByte03 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public byte Byte04
            {
                get { return mByte04; }
                set { mByte04 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public ushort Short01
            {
                get { return mShort01; }
                set { mShort01 = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public uint Int02
            {
                get { return mInt02; }
                set { mInt02 = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public ulong Long01
            {
                get { return mLong01; }
                set { mLong01 = value; OnElementChanged(); }
            }
        }
        

                public class ParticleParams : DataElement
        {
            public ParticleParams(int apiVersion, EventHandler handler, ParticleParams basis)
                : base(apiVersion, handler, basis)
            {
            }
            public ParticleParams(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }
            public ParticleParams(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }

                        private float mFloat01 = 10f;
            private float mFloat02 = 10f;

            private float mFloat03;

            private float mFloat04 = -1f;
            private float mFloat05 = -1f;

            private float mFloat06 = -1f;
            private float mFloat07 = -1f;

            private float mFloat08;
            private float mFloat09;
            private float mFloat10;

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

            private float mFloat22 = -1f;
            

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
            public float Float03
            {
                get { return mFloat03; }
                set { mFloat03 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public float Float04
            {
                get { return mFloat04; }
                set { mFloat04 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public float Float05
            {
                get { return mFloat05; }
                set { mFloat05 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public float Float06
            {
                get { return mFloat06; }
                set { mFloat06 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public float Float07
            {
                get { return mFloat07; }
                set { mFloat07 = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public float Float08
            {
                get { return mFloat08; }
                set { mFloat08 = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public float Float09
            {
                get { return mFloat09; }
                set { mFloat09 = value; OnElementChanged(); }
            }
            [ElementPriority(10)]
            public float Float10
            {
                get { return mFloat10; }
                set { mFloat10 = value; OnElementChanged(); }
            }
            [ElementPriority(11)]
            public float Float11
            {
                get { return mFloat11; }
                set { mFloat11 = value; OnElementChanged(); }
            }
            [ElementPriority(12)]
            public float Float12
            {
                get { return mFloat12; }
                set { mFloat12 = value; OnElementChanged(); }
            }
            [ElementPriority(13)]
            public float Float13
            {
                get { return mFloat13; }
                set { mFloat13 = value; OnElementChanged(); }
            }
            [ElementPriority(14)]
            public float Float14
            {
                get { return mFloat14; }
                set { mFloat14 = value; OnElementChanged(); }
            }
            [ElementPriority(15)]
            public float Float15
            {
                get { return mFloat15; }
                set { mFloat15 = value; OnElementChanged(); }
            }
            [ElementPriority(16)]
            public float Float16
            {
                get { return mFloat16; }
                set { mFloat16 = value; OnElementChanged(); }
            }
            [ElementPriority(17)]
            public float Float17
            {
                get { return mFloat17; }
                set { mFloat17 = value; OnElementChanged(); }
            }
            [ElementPriority(18)]
            public float Float18
            {
                get { return mFloat18; }
                set { mFloat18 = value; OnElementChanged(); }
            }
            [ElementPriority(19)]
            public float Float19
            {
                get { return mFloat19; }
                set { mFloat19 = value; OnElementChanged(); }
            }
            [ElementPriority(20)]
            public float Float20
            {
                get { return mFloat20; }
                set { mFloat20 = value; OnElementChanged(); }
            }
            [ElementPriority(21)]
            public float Float21
            {
                get { return mFloat21; }
                set { mFloat21 = value; OnElementChanged(); }
            }
            [ElementPriority(22)]
            public float Float22
            {
                get { return mFloat22; }
                set { mFloat22 = value; OnElementChanged(); }
            }

            

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mFloat01, ByteOrder.LittleEndian);
                s.Read(out mFloat02, ByteOrder.LittleEndian);
                s.Read(out mFloat03, ByteOrder.LittleEndian);
                s.Read(out mFloat04, ByteOrder.LittleEndian);
                s.Read(out mFloat05, ByteOrder.LittleEndian);
                s.Read(out mFloat06, ByteOrder.LittleEndian);
                s.Read(out mFloat07, ByteOrder.LittleEndian);
                s.Read(out mFloat08, ByteOrder.LittleEndian);
                s.Read(out mFloat09, ByteOrder.LittleEndian);
                s.Read(out mFloat10, ByteOrder.LittleEndian);
                s.Read(out mFloat11, ByteOrder.LittleEndian);
                s.Read(out mFloat12, ByteOrder.LittleEndian);
                s.Read(out mFloat13, ByteOrder.LittleEndian);
                s.Read(out mFloat14, ByteOrder.LittleEndian);
                s.Read(out mFloat15, ByteOrder.LittleEndian);
                s.Read(out mFloat16, ByteOrder.LittleEndian);
                s.Read(out mFloat17, ByteOrder.LittleEndian);
                s.Read(out mFloat18, ByteOrder.LittleEndian);
                s.Read(out mFloat19, ByteOrder.LittleEndian);
                s.Read(out mFloat20, ByteOrder.LittleEndian);
                s.Read(out mFloat21, ByteOrder.LittleEndian);
                s.Read(out mFloat22);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mFloat01, ByteOrder.LittleEndian);
                s.Write(mFloat02, ByteOrder.LittleEndian);
                s.Write(mFloat03, ByteOrder.LittleEndian);
                s.Write(mFloat04, ByteOrder.LittleEndian);
                s.Write(mFloat05, ByteOrder.LittleEndian);
                s.Write(mFloat06, ByteOrder.LittleEndian);
                s.Write(mFloat07, ByteOrder.LittleEndian);
                s.Write(mFloat08, ByteOrder.LittleEndian);
                s.Write(mFloat09, ByteOrder.LittleEndian);
                s.Write(mFloat10, ByteOrder.LittleEndian);
                s.Write(mFloat11, ByteOrder.LittleEndian);
                s.Write(mFloat12, ByteOrder.LittleEndian);
                s.Write(mFloat13, ByteOrder.LittleEndian);
                s.Write(mFloat14, ByteOrder.LittleEndian);
                s.Write(mFloat15, ByteOrder.LittleEndian);
                s.Write(mFloat16, ByteOrder.LittleEndian);
                s.Write(mFloat17, ByteOrder.LittleEndian);
                s.Write(mFloat18, ByteOrder.LittleEndian);
                s.Write(mFloat19, ByteOrder.LittleEndian);
                s.Write(mFloat20, ByteOrder.LittleEndian);
                s.Write(mFloat21, ByteOrder.LittleEndian);
                s.Write(mFloat22);
            }
        }
        

                public class ItemA : DataElement, IEquatable<ItemA>
        {
            private float mFloat01;
            private float mFloat02;
            private float mFloat03;
            private float mFloat04;
            private float mFloat05;
            private float mFloat06;
            private float mFloat07;
            public ItemA(int apiVersion, EventHandler handler, ItemA basis)
                : base(apiVersion, handler)
            {
                mFloat01 = basis.mFloat01;
                mFloat02 = basis.mFloat02;
                mFloat03 = basis.mFloat03;
                mFloat04 = basis.mFloat04;
                mFloat05 = basis.mFloat05;
                mFloat06 = basis.mFloat06;
                mFloat07 = basis.mFloat07;
            }
            public ItemA(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
            public ItemA(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
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
                set
                {
                    mFloat02 = value; OnElementChanged();
                }
            }
            [ElementPriority(3)]
            public float Float03
            {
                get { return mFloat03; }
                set
                {
                    mFloat03 = value; OnElementChanged();
                }
            }
            [ElementPriority(4)]
            public float Float04
            {
                get { return mFloat04; }
                set
                {
                    mFloat04 = value; OnElementChanged();
                }
            }
            [ElementPriority(5)]
            public float Float05
            {
                get { return mFloat05; }
                set
                {
                    mFloat05 = value; OnElementChanged();
                }
            }
            [ElementPriority(6)]
            public float Float06
            {
                get { return mFloat06; }
                set
                {
                    mFloat06 = value; OnElementChanged();
                }
            }
            [ElementPriority(7)]
            public float Float07
            {
                get { return mFloat07; }
                set
                {
                    mFloat07 = value; OnElementChanged();
                }
            }


            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mFloat01);
                s.Read(out mFloat02, ByteOrder.LittleEndian);
                s.Read(out mFloat03, ByteOrder.LittleEndian);
                s.Read(out mFloat04, ByteOrder.LittleEndian);
                s.Read(out mFloat05, ByteOrder.LittleEndian);
                s.Read(out mFloat06, ByteOrder.LittleEndian);
                s.Read(out mFloat07, ByteOrder.LittleEndian);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mFloat01);
                s.Write(mFloat02, ByteOrder.LittleEndian);
                s.Write(mFloat03, ByteOrder.LittleEndian);
                s.Write(mFloat04, ByteOrder.LittleEndian);
                s.Write(mFloat05, ByteOrder.LittleEndian);
                s.Write(mFloat06, ByteOrder.LittleEndian);
                s.Write(mFloat07, ByteOrder.LittleEndian);
            }

            public bool Equals(ItemA other)
            {
                return base.Equals(other);
            }
        }
        

                public class ItemB : DataElement, IEquatable<ItemB>
        {
            private UInt32 mInt02;
            private float mFloat01;
            private float mFloat02;
            private float mFloat03;
            private float mFloat04;
            private UInt32 mInt01;
            private UInt64 mLong01;
            private String mString01 = string.Empty;
            private String mString02 = string.Empty;
            private DataList<Vector3ValueLE> mVector3List01;

            public ItemB(int apiVersion, EventHandler handler, ItemB basis)
                : base(apiVersion, handler, basis) { }
            public ItemB(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
            public ItemB(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mVector3List01 = new DataList<Vector3ValueLE>(handler);
            }
            [ElementPriority(1)]
            public UInt32 Int01
            {
                get { return mInt01; }
                set { mInt01 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public UInt64 Long01
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
            public UInt32 Int02
            {
                get { return mInt02; }
                set { mInt02 = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public String String01
            {
                get { return mString01; }
                set { mString01 = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public String String02
            {
                get { return mString02; }
                set { mString02 = value; OnElementChanged(); }
            }
            [ElementPriority(10)]
            public DataList<Vector3ValueLE> Vector3List01
            {
                get { return mVector3List01; }
                set { mVector3List01 = value; OnElementChanged(); }
            }

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mInt01);
                s.Read(out mLong01);
                s.Read(out mFloat01);
                s.Read(out mFloat02);
                s.Read(out mFloat03);
                s.Read(out mFloat04);
                s.Read(out mInt02);
                s.Read(out mString01, StringType.ZeroDelimited);
                s.Read(out mString02, StringType.ZeroDelimited);
                mVector3List01 = new DataList<Vector3ValueLE>(handler, stream);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mInt01);
                s.Write(mLong01);
                s.Write(mFloat01);
                s.Write(mFloat02);
                s.Write(mFloat03);
                s.Write(mFloat04);
                s.Write(mInt02);
                s.Write(mString01, StringType.ZeroDelimited);
                s.Write(mString02, StringType.ZeroDelimited);
                mVector3List01.UnParse(stream);
            }
            public bool Equals(ItemB other)
            {
                return base.Equals(other);
            }
        }
        

                public class ItemC : DataElement, IEquatable<ItemC>
        {
            public ItemC(int apiVersion, EventHandler handler, ItemC basis)
                : base(apiVersion, handler, basis) { }
            public ItemC(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
            public ItemC(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mItems = new DataList<ItemE>(handler);
            }
            private DataList<ItemE> mItems;
            [ElementPriority(1)]
            public DataList<ItemE> Items
            {
                get { return mItems; }
                set { mItems = value; OnElementChanged(); }
            }
            protected override void Parse(Stream stream)
            {
                mItems = new DataList<ItemE>(handler, stream);
            }
            public override void UnParse(Stream stream)
            {
                mItems.UnParse(stream);
            }
            public bool Equals(ItemC other)
            {
                return base.Equals(other);
            }
        }
        

                public class ItemD : DataElement, IEquatable<ItemD>
        {
            public ItemD(int apiVersion, EventHandler handler, ItemD basis)
                : base(apiVersion, handler)
            {
                mFloat01 = basis.mFloat01;
                mFloat02 = basis.mFloat02;
                mFloat03 = basis.mFloat03;
                mFloat04 = basis.mFloat04;
                mFloat05 = basis.mFloat05;
                mFloat06 = basis.mFloat06;
                mFloat07 = basis.mFloat07;
            }
            public ItemD(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
            public ItemD(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            private float mFloat01;
            private float mFloat02;
            private float mFloat03;
            private float mFloat04;
            private float mFloat05;
            private float mFloat06;
            private float mFloat07;
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
            public float Float03
            {
                get { return mFloat03; }
                set { mFloat03 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public float Float04
            {
                get { return mFloat04; }
                set { mFloat04 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public float Float05
            {
                get { return mFloat05; }
                set { mFloat05 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public float Float06
            {
                get { return mFloat06; }
                set { mFloat06 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public float Timecode
            {
                get { return mFloat07; }
                set { mFloat07 = value; OnElementChanged(); }
            }

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mFloat01, ByteOrder.LittleEndian);
                s.Read(out mFloat02, ByteOrder.LittleEndian);
                s.Read(out mFloat03, ByteOrder.LittleEndian);
                s.Read(out mFloat04, ByteOrder.LittleEndian);
                s.Read(out mFloat05, ByteOrder.LittleEndian);
                s.Read(out mFloat06, ByteOrder.LittleEndian);
                s.Read(out mFloat07);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mFloat01, ByteOrder.LittleEndian);
                s.Write(mFloat02, ByteOrder.LittleEndian);
                s.Write(mFloat03, ByteOrder.LittleEndian);
                s.Write(mFloat04, ByteOrder.LittleEndian);
                s.Write(mFloat05, ByteOrder.LittleEndian);
                s.Write(mFloat06, ByteOrder.LittleEndian);
                s.Write(mFloat07);
            }
            public bool Equals(ItemD other)
            {
                return base.Equals(other);
            }
        }
        

                public class ItemE : DataElement, IEquatable<ItemE>
        {
            public ItemE(int apiVersion, EventHandler handler, ItemE basis)
                : base(apiVersion, handler)
            {
                mFloat01 = basis.mFloat01;
                mFloat02 = basis.mFloat02;
                mFloat03 = basis.mFloat03;
                mFloat04 = basis.mFloat04;
                mFloat05 = basis.mFloat05;
                mFloat06 = basis.mFloat06;
                mFloat07 = basis.mFloat07;
            }
            public ItemE(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
            public ItemE(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            private float mFloat01;
            private float mFloat02;
            private float mFloat03;
            private float mFloat04;
            private float mFloat05;
            private float mFloat06;
            private float mFloat07;
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
            public float Float03
            {
                get { return mFloat03; }
                set { mFloat03 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public float Float04
            {
                get { return mFloat04; }
                set { mFloat04 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public float Float05
            {
                get { return mFloat05; }
                set { mFloat05 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public float Float06
            {
                get { return mFloat06; }
                set { mFloat06 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public float Float07
            {
                get { return mFloat07; }
                set { mFloat07 = value; OnElementChanged(); }
            }

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mFloat01);
                s.Read(out mFloat02, ByteOrder.LittleEndian);
                s.Read(out mFloat03, ByteOrder.LittleEndian);
                s.Read(out mFloat04, ByteOrder.LittleEndian);
                s.Read(out mFloat05, ByteOrder.LittleEndian);
                s.Read(out mFloat06, ByteOrder.LittleEndian);
                s.Read(out mFloat07, ByteOrder.LittleEndian);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mFloat01);
                s.Write(mFloat02, ByteOrder.LittleEndian);
                s.Write(mFloat03, ByteOrder.LittleEndian);
                s.Write(mFloat04, ByteOrder.LittleEndian);
                s.Write(mFloat05, ByteOrder.LittleEndian);
                s.Write(mFloat06, ByteOrder.LittleEndian);
                s.Write(mFloat07, ByteOrder.LittleEndian);
            }

            public bool Equals(ItemE other)
            {
                return base.Equals(other);
            }

        }
        

                public class ItemF : DataElement
        {
            public ItemF(int apiVersion, EventHandler handler, ItemF basis)
                : base(apiVersion, handler, basis) { }
            public ItemF(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
            public ItemF(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mFloatList01 = new DataList<FloatValue>(handler);
            }

                        private float mFloat01; //LE
            private float mFloat02; //LE
            private float mFloat03; //LE
            private float mFloat04; //LE
            private float mFloat05;
            private float mFloat06;
            private float mFloat07;
            private DataList<FloatValue> mFloatList01;
            private byte mByte01;
            

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
            public float Float03
            {
                get { return mFloat03; }
                set { mFloat03 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public float Float04
            {
                get { return mFloat04; }
                set { mFloat04 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public float Float05
            {
                get { return mFloat05; }
                set { mFloat05 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public float Float06
            {
                get { return mFloat06; }
                set { mFloat06 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public float Float07
            {
                get { return mFloat07; }
                set { mFloat07 = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public DataList<FloatValue> FloatList01
            {
                get { return mFloatList01; }
                set { mFloatList01 = value; OnElementChanged(); }
            }

            [ElementPriority(9)]
            public byte Byte01
            {
                get { return mByte01; }
                set { mByte01 = value; OnElementChanged(); }
            }
            

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mFloat01, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat02, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat03, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat04, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat05);
                s.Read(out mFloat06);
                s.Read(out mFloat07);
                mFloatList01 = new DataList<FloatValue>(handler, stream);
                s.Read(out mByte01);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mFloat01, ByteOrder.LittleEndian); //LE
                s.Write(mFloat02, ByteOrder.LittleEndian); //LE
                s.Write(mFloat03, ByteOrder.LittleEndian); //LE
                s.Write(mFloat04, ByteOrder.LittleEndian); //LE
                s.Write(mFloat05);
                s.Write(mFloat06);
                s.Write(mFloat07);
                mFloatList01.UnParse(stream);
                s.Write(mByte01);
            }
        }
        

        public Effect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
        }

        public Effect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }

        public Effect(int apiVersion, EventHandler handler, Effect basis)
            : base(apiVersion, handler, basis)
        {
        }

        protected override void Parse(Stream stream) { }
        public override void UnParse(Stream stream) { }

        public bool Equals(Effect other)
        {
            return base.Equals(other);
        }
    }
}