using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;
using System.Collections.Generic;

namespace s3piwrappers.Effects
{
    public class ParticleEffect : Effect, IEquatable<ParticleEffect>
    {

        public ParticleEffect(int apiVersion, EventHandler handler, ParticleEffect basis)
            : base(apiVersion, handler, basis)
        {
        }
        public ParticleEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mParticleParameters = new ParticleParams(0, handler);
            mResource = new ResourceReference(0, handler);
            mItemAList01 = new DataList<ItemA>(handler);
            mItemBList01 = new DataList<ItemB>(handler);
            mItemDList01 = new DataList<ItemD>(handler);
            mFloatList01 = new DataList<FloatValue>(handler);
            mFloatList02 = new DataList<FloatValue>(handler);
            mFloatList03 = new DataList<FloatValue>(handler);
            mFloatList04 = new DataList<FloatValue>(handler);
            mFloatList05 = new DataList<FloatValue>(handler);
            mFloatList06 = new DataList<FloatValue>(handler);
            mItemF01 = new ItemF(0, handler);
            mFloatList08 = new DataList<FloatValue>(handler);
            mColourList01 = new DataList<ColourValue>(handler);
            mVector3List01 = new DataList<Vector3ValueLE>(handler);
            mVector3List02 = new DataList<Vector3ValueLE>(handler);


        }
        public ParticleEffect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s) { }

        #region Fields
        private UInt32 mInt01;
        private ParticleParams mParticleParameters;
        private DataList<FloatValue> mFloatList01;
        private float mFloat01 = 1.5f;
        private UInt16 mShort01 = 1;
        private float mFloat02;
        private DataList<FloatValue> mFloatList02;
        private float mFloat03;
        private DataList<FloatValue> mFloatList03;
        private float mFloat04;
        private float mFloat05;
        private float mFloat06;
        private DataList<FloatValue> mFloatList04;
        private DataList<FloatValue> mFloatList05;
        private float mFloat07;
        private DataList<ColourValue> mColourList01;
        private float mFloat08; //LE
        private float mFloat09; //LE
        private float mFloat10; //LE
        private ResourceReference mResource;

        private Byte mByte01;
        private Byte mByte02;
        private Byte mByte03;
        private Byte mByte04;
        private Byte mByte05;
        private float mFloat11;
        private Byte mByte06;
        private Byte mByte07;
        private Byte mByte08;

        private float mFloat12; //LE
        private float mFloat13; //LE
        private float mFloat14; //LE

        private float mFloat15;
        private float mFloat16;
        private float mFloat17;

        private float mFloat18; //LE
        private float mFloat19; //LE
        private float mFloat20; //LE

        private float mFloat21;
        private float mFloat22;
        private float mFloat23;

        private DataList<ItemA> mItemAList01;
        private Byte mByte09;
        private Byte mByte10;
        private Byte mByte11;
        private Byte mByte12;
        private DataList<Vector3ValueLE> mVector3List01;
        private DataList<FloatValue> mFloatList06;
        private DataList<ItemB> mItemBList01;
        private float mFloat24;
        private float mFloat25;
        private float mFloat26;
        private float mFloat27;
        private float mFloat28;

        private float mFloat29 = -1000000000.0f;
        private float mFloat30;
        private float mFloat31 = -10000.0f; //LE
        private float mFloat32 = 10000.0f; //LE

        private UInt64 mLong01;
        private UInt64 mLong02;
        private UInt64 mLong03;


        private ItemF mItemF01;

        private float mFloat40; //LE
        private float mFloat41; //LE
        private float mFloat42; //LE

        private DataList<FloatValue> mFloatList08;
        private float mFloat43;
        private float mFloat44;

        private DataList<ItemD> mItemDList01;

        //Version 2+
        private float mFloat45; //LE
        private float mFloat46; //LE
        private float mFloat47; //LE
        private DataList<Vector3ValueLE> mVector3List02;

        //Version 3+
        private Byte mByte14;

        //Version 4+
        private float mFloat48;

        #endregion

        #region Properties

        [ElementPriority(75)]
        public float Float48
        {
            get { return mFloat48; }
            set { mFloat48 = value; OnElementChanged(); }
        }

        [ElementPriority(74)]
        public byte Byte14
        {
            get { return mByte14; }
            set { mByte14 = value; OnElementChanged(); }
        }

        [ElementPriority(73)]
        public DataList<Vector3ValueLE> Vector3List02
        {
            get { return mVector3List02; }
            set { mVector3List02 = value; OnElementChanged(); }
        }

        [ElementPriority(72)]
        public float Float47
        {
            get { return mFloat47; }
            set { mFloat47 = value; OnElementChanged(); }
        }

        [ElementPriority(71)]
        public float Float46
        {
            get { return mFloat46; }
            set { mFloat46 = value; OnElementChanged(); }
        }

        [ElementPriority(70)]
        public float Float45
        {
            get { return mFloat45; }
            set { mFloat45 = value; OnElementChanged(); }
        }

        [ElementPriority(69)]
        public DataList<ItemD> ItemDList01
        {
            get { return mItemDList01; }
            set { mItemDList01 = value; OnElementChanged(); }
        }

        [ElementPriority(68)]
        public float Float44
        {
            get { return mFloat44; }
            set { mFloat44 = value; OnElementChanged(); }
        }

        [ElementPriority(67)]
        public float Float43
        {
            get { return mFloat43; }
            set { mFloat43 = value; OnElementChanged(); }
        }

        [ElementPriority(66)]
        public DataList<FloatValue> FloatList08
        {
            get { return mFloatList08; }
            set { mFloatList08 = value; OnElementChanged(); }
        }

        [ElementPriority(65)]
        public float Float42
        {
            get { return mFloat42; }
            set { mFloat42 = value; OnElementChanged(); }
        }

        [ElementPriority(64)]
        public float Float41
        {
            get { return mFloat41; }
            set { mFloat41 = value; OnElementChanged(); }
        }

        [ElementPriority(63)]
        public float Float40
        {
            get { return mFloat40; }
            set { mFloat40 = value; OnElementChanged(); }
        }


        [ElementPriority(62)]
        public ItemF ItemF01
        {
            get { return mItemF01; }
            set { mItemF01 = value; OnElementChanged(); }
        }

        [ElementPriority(61)]
        public ulong Long03
        {
            get { return mLong03; }
            set { mLong03 = value; OnElementChanged(); }
        }

        [ElementPriority(60)]
        public ulong Long02
        {
            get { return mLong02; }
            set { mLong02 = value; OnElementChanged(); }
        }

        [ElementPriority(59)]
        public ulong Long01
        {
            get { return mLong01; }
            set { mLong01 = value; OnElementChanged(); }
        }

        [ElementPriority(58)]
        public float Float32
        {
            get { return mFloat32; }
            set { mFloat32 = value; OnElementChanged(); }
        }

        [ElementPriority(57)]
        public float Float31
        {
            get { return mFloat31; }
            set { mFloat31 = value; OnElementChanged(); }
        }

        [ElementPriority(56)]
        public float Float30
        {
            get { return mFloat30; }
            set { mFloat30 = value; OnElementChanged(); }
        }

        [ElementPriority(55)]
        public float Float29
        {
            get { return mFloat29; }
            set { mFloat29 = value; OnElementChanged(); }
        }

        [ElementPriority(54)]
        public float Float28
        {
            get { return mFloat28; }
            set { mFloat28 = value; OnElementChanged(); }
        }

        [ElementPriority(53)]
        public float Float27
        {
            get { return mFloat27; }
            set { mFloat27 = value; OnElementChanged(); }
        }

        [ElementPriority(52)]
        public float Float26
        {
            get { return mFloat26; }
            set { mFloat26 = value; OnElementChanged(); }
        }

        [ElementPriority(51)]
        public float Float25
        {
            get { return mFloat25; }
            set { mFloat25 = value; OnElementChanged(); }
        }

        [ElementPriority(50)]
        public float Float24
        {
            get { return mFloat24; }
            set { mFloat24 = value; OnElementChanged(); }
        }

        [ElementPriority(49)]
        public DataList<ItemB> ItemBList01
        {
            get { return mItemBList01; }
            set { mItemBList01 = value; OnElementChanged(); }
        }

        [ElementPriority(48)]
        public DataList<FloatValue> FloatList06
        {
            get { return mFloatList06; }
            set { mFloatList06 = value; OnElementChanged(); }
        }

        [ElementPriority(47)]
        public DataList<Vector3ValueLE> Vector3List01
        {
            get { return mVector3List01; }
            set { mVector3List01 = value; OnElementChanged(); }
        }

        [ElementPriority(46)]
        public byte Byte12
        {
            get { return mByte12; }
            set { mByte12 = value; OnElementChanged(); }
        }

        [ElementPriority(45)]
        public byte Byte11
        {
            get { return mByte11; }
            set { mByte11 = value; OnElementChanged(); }
        }

        [ElementPriority(44)]
        public byte Byte10
        {
            get { return mByte10; }
            set { mByte10 = value; OnElementChanged(); }
        }

        [ElementPriority(43)]
        public byte Byte09
        {
            get { return mByte09; }
            set { mByte09 = value; OnElementChanged(); }
        }

        [ElementPriority(42)]
        public DataList<ItemA> ItemAList01
        {
            get { return mItemAList01; }
            set { mItemAList01 = value; OnElementChanged(); }
        }

        [ElementPriority(41)]
        public float Float23
        {
            get { return mFloat23; }
            set { mFloat23 = value; OnElementChanged(); }
        }

        [ElementPriority(40)]
        public float Float22
        {
            get { return mFloat22; }
            set { mFloat22 = value; OnElementChanged(); }
        }

        [ElementPriority(39)]
        public float Float21
        {
            get { return mFloat21; }
            set { mFloat21 = value; OnElementChanged(); }
        }

        [ElementPriority(38)]
        public float Float20
        {
            get { return mFloat20; }
            set { mFloat20 = value; OnElementChanged(); }
        }

        [ElementPriority(37)]
        public float Float19
        {
            get { return mFloat19; }
            set { mFloat19 = value; OnElementChanged(); }
        }

        [ElementPriority(36)]
        public float Float18
        {
            get { return mFloat18; }
            set { mFloat18 = value; OnElementChanged(); }
        }

        [ElementPriority(35)]
        public float Float17
        {
            get { return mFloat17; }
            set { mFloat17 = value; OnElementChanged(); }
        }

        [ElementPriority(34)]
        public float Float16
        {
            get { return mFloat16; }
            set { mFloat16 = value; OnElementChanged(); }
        }

        [ElementPriority(33)]
        public float Float15
        {
            get { return mFloat15; }
            set { mFloat15 = value; OnElementChanged(); }
        }

        [ElementPriority(32)]
        public float Float14
        {
            get { return mFloat14; }
            set { mFloat14 = value; OnElementChanged(); }
        }

        [ElementPriority(31)]
        public float Float13
        {
            get { return mFloat13; }
            set { mFloat13 = value; OnElementChanged(); }
        }

        [ElementPriority(30)]
        public float Float12
        {
            get { return mFloat12; }
            set { mFloat12 = value; OnElementChanged(); }
        }

        [ElementPriority(29)]
        public byte Byte08
        {
            get { return mByte08; }
            set { mByte08 = value; OnElementChanged(); }
        }

        [ElementPriority(28)]
        public byte Byte07
        {
            get { return mByte07; }
            set { mByte07 = value; OnElementChanged(); }
        }

        [ElementPriority(27)]
        public byte Byte06
        {
            get { return mByte06; }
            set { mByte06 = value; OnElementChanged(); }
        }

        [ElementPriority(26)]
        public float Float11
        {
            get { return mFloat11; }
            set { mFloat11 = value; OnElementChanged(); }
        }

        [ElementPriority(25)]
        public byte Byte05
        {
            get { return mByte05; }
            set { mByte05 = value; OnElementChanged(); }
        }

        [ElementPriority(24)]
        public byte Byte04
        {
            get { return mByte04; }
            set { mByte04 = value; OnElementChanged(); }
        }

        [ElementPriority(23)]
        public byte Byte03
        {
            get { return mByte03; }
            set { mByte03 = value; OnElementChanged(); }
        }

        [ElementPriority(22)]
        public byte Byte02
        {
            get { return mByte02; }
            set { mByte02 = value; OnElementChanged(); }
        }

        [ElementPriority(21)]
        public byte Byte01
        {
            get { return mByte01; }
            set { mByte01 = value; OnElementChanged(); }
        }

        [ElementPriority(20)]
        public ResourceReference ResourceKey
        {
            get { return mResource; }
            set { mResource = value; OnElementChanged(); }
        }

        [ElementPriority(19)]
        public float Float10
        {
            get { return mFloat10; }
            set { mFloat10 = value; OnElementChanged(); }
        }

        [ElementPriority(18)]
        public float Float09
        {
            get { return mFloat09; }
            set { mFloat09 = value; OnElementChanged(); }
        }

        [ElementPriority(17)]
        public float Float08
        {
            get { return mFloat08; }
            set { mFloat08 = value; OnElementChanged(); }
        }

        [ElementPriority(16)]
        public DataList<ColourValue> ColourDeltas
        {
            get { return mColourList01; }
            set { mColourList01 = value; OnElementChanged(); }
        }

        [ElementPriority(15)]
        public float Float07
        {
            get { return mFloat07; }
            set { mFloat07 = value; OnElementChanged(); }
        }

        [ElementPriority(14)]
        public DataList<FloatValue> FloatList05
        {
            get { return mFloatList05; }
            set { mFloatList05 = value; OnElementChanged(); }
        }

        [ElementPriority(13)]
        public DataList<FloatValue> FloatList04
        {
            get { return mFloatList04; }
            set { mFloatList04 = value; OnElementChanged(); }
        }

        [ElementPriority(12)]
        public float Float06
        {
            get { return mFloat06; }
            set { mFloat06 = value; OnElementChanged(); }
        }

        [ElementPriority(11)]
        public float Float05
        {
            get { return mFloat05; }
            set { mFloat05 = value; OnElementChanged(); }
        }

        [ElementPriority(10)]
        public float Float04
        {
            get { return mFloat04; }
            set { mFloat04 = value; OnElementChanged(); }
        }

        [ElementPriority(9)]
        public DataList<FloatValue> FloatList03
        {
            get { return mFloatList03; }
            set { mFloatList03 = value; OnElementChanged(); }
        }

        [ElementPriority(8)]
        public float Float03
        {
            get { return mFloat03; }
            set { mFloat03 = value; OnElementChanged(); }
        }

        [ElementPriority(7)]
        public DataList<FloatValue> ScaleDeltas
        {
            get { return mFloatList02; }
            set { mFloatList02 = value; OnElementChanged(); }
        }

        [ElementPriority(6)]
        public float Float02
        {
            get { return mFloat02; }
            set { mFloat02 = value; OnElementChanged(); }
        }

        [ElementPriority(5)]
        public ushort Short02
        {
            get { return mShort01; }
            set { mShort01 = value; OnElementChanged(); }
        }

        [ElementPriority(4)]
        public float Float01
        {
            get { return mFloat01; }
            set { mFloat01 = value; OnElementChanged(); }
        }

        [ElementPriority(3)]
        public DataList<FloatValue> FloatList01
        {
            get { return mFloatList01; }
            set { mFloatList01 = value; OnElementChanged(); }
        }

        [ElementPriority(2)]
        public ParticleParams ParticleParameters
        {
            get { return mParticleParameters; }
            set { mParticleParameters = value; OnElementChanged(); }
        }
        [ElementPriority(1)]
        public uint Int01
        {
            get { return mInt01; }
            set { mInt01 = value; OnElementChanged(); }
        }


        #endregion

        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mInt01);
            ParticleParameters = new ParticleParams(0, handler, stream);
            mFloatList01 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat01);
            s.Read(out mShort01);
            s.Read(out mFloat02);
            mFloatList02 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat03);
            mFloatList03 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat04);
            s.Read(out mFloat05);
            s.Read(out mFloat06);
            mFloatList04 = new DataList<FloatValue>(handler, stream);
            mFloatList05 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat07);
            mColourList01 = new DataList<ColourValue>(handler, stream);
            s.Read(out mFloat08, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat09, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat10, ByteOrder.LittleEndian); //LE
            mResource = new ResourceReference(0, handler, stream);

            s.Read(out mByte01);
            s.Read(out mByte02);
            s.Read(out mByte03);
            s.Read(out mByte04);
            s.Read(out mByte05);
            s.Read(out mFloat11);
            s.Read(out mByte06);
            s.Read(out mByte07);
            s.Read(out mByte08);

            s.Read(out mFloat12, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat13, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat14, ByteOrder.LittleEndian); //LE

            s.Read(out mFloat15);
            s.Read(out mFloat16);
            s.Read(out mFloat17);

            s.Read(out mFloat18, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat19, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat20, ByteOrder.LittleEndian); //LE

            s.Read(out mFloat21);
            s.Read(out mFloat22);
            s.Read(out mFloat23);

            mItemAList01 = new DataList<ItemA>(handler, stream);
            s.Read(out mByte09);
            s.Read(out mByte10);
            s.Read(out mByte11);
            s.Read(out mByte12);
            mVector3List01 = new DataList<Vector3ValueLE>(handler, stream);
            mFloatList06 = new DataList<FloatValue>(handler, stream);
            mItemBList01 = new DataList<ItemB>(handler, stream);
            s.Read(out mFloat24);
            s.Read(out mFloat25);
            s.Read(out mFloat26);
            s.Read(out mFloat27);
            s.Read(out mFloat28);

            s.Read(out mFloat29); //-1000000000.0f
            s.Read(out mFloat30); //0f
            s.Read(out mFloat31, ByteOrder.LittleEndian); //LE  -10000.0f
            s.Read(out mFloat32, ByteOrder.LittleEndian); //LE   10000.0f

            s.Read(out mLong01);
            s.Read(out mLong02);
            s.Read(out mLong03);
            mItemF01 = new ItemF(0, handler, stream);



            s.Read(out mFloat40, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat41, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat42, ByteOrder.LittleEndian); //LE

            mFloatList08 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat43);
            s.Read(out mFloat44);

            mItemDList01 = new DataList<ItemD>(handler, stream);
            
            //Version 2+
            if (mSection.Version >= 0x0002 && stream.Position < stream.Length)
            {
                s.Read(out mFloat45, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat46, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat47, ByteOrder.LittleEndian); //LE
                mVector3List02 = new DataList<Vector3ValueLE>(handler, stream);
            }

            //Version 3+
            if (mSection.Version >= 0x0003 && stream.Position < stream.Length) s.Read(out mByte14);

            //Version 4+
            if (mSection.Version >= 0x0004 && stream.Position < stream.Length) s.Read(out mFloat48);

        }
        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(Int01);
            ParticleParameters.UnParse(stream);
            mFloatList01.UnParse(stream);
            s.Write(mFloat01);
            s.Write(mShort01);
            s.Write(mFloat02);
            mFloatList02.UnParse(stream);
            s.Write(mFloat03);
            mFloatList03.UnParse(stream);
            s.Write(mFloat04);
            s.Write(mFloat05);
            s.Write(mFloat06);
            mFloatList04.UnParse(stream);
            mFloatList05.UnParse(stream);
            s.Write(mFloat07);
            mColourList01.UnParse(stream);
            s.Write(mFloat08, ByteOrder.LittleEndian); //LE
            s.Write(mFloat09, ByteOrder.LittleEndian); //LE
            s.Write(mFloat10, ByteOrder.LittleEndian); //LE
            mResource.UnParse(stream);

            s.Write(mByte01);
            s.Write(mByte02);
            s.Write(mByte03);
            s.Write(mByte04);
            s.Write(mByte05);
            s.Write(mFloat11);
            s.Write(mByte06);
            s.Write(mByte07);
            s.Write(mByte08);

            s.Write(mFloat12, ByteOrder.LittleEndian); //LE
            s.Write(mFloat13, ByteOrder.LittleEndian); //LE
            s.Write(mFloat14, ByteOrder.LittleEndian); //LE

            s.Write(mFloat15);
            s.Write(mFloat16);
            s.Write(mFloat17);

            s.Write(mFloat18, ByteOrder.LittleEndian); //LE
            s.Write(mFloat19, ByteOrder.LittleEndian); //LE
            s.Write(mFloat20, ByteOrder.LittleEndian); //LE

            s.Write(mFloat21);
            s.Write(mFloat22);
            s.Write(mFloat23);

            mItemAList01.UnParse(stream);
            s.Write(mByte09);
            s.Write(mByte10);
            s.Write(mByte11);
            s.Write(mByte12);
            mVector3List01.UnParse(stream);
            mFloatList06.UnParse(stream);
            mItemBList01.UnParse(stream);
            s.Write(mFloat24);
            s.Write(mFloat25);
            s.Write(mFloat26);
            s.Write(mFloat27);
            s.Write(mFloat28);

            s.Write(mFloat29); //-1000000000.0f
            s.Write(mFloat30); //0f
            s.Write(mFloat31, ByteOrder.LittleEndian); //LE  -10000.0f
            s.Write(mFloat32, ByteOrder.LittleEndian); //LE   10000.0f

            s.Write(mLong01);
            s.Write(mLong02);
            s.Write(mLong03);
            mItemF01.UnParse(stream);
            s.Write(mFloat40, ByteOrder.LittleEndian); //LE
            s.Write(mFloat41, ByteOrder.LittleEndian); //LE
            s.Write(mFloat42, ByteOrder.LittleEndian); //LE

            mFloatList08.UnParse(stream);
            s.Write(mFloat43);
            s.Write(mFloat44);

            mItemDList01.UnParse(stream);

            //Version 2+
            if (mSection.Version >= 0x0002)
            {
                s.Write(mFloat45, ByteOrder.LittleEndian); //LE
                s.Write(mFloat46, ByteOrder.LittleEndian); //LE
                s.Write(mFloat47, ByteOrder.LittleEndian); //LE
                mVector3List02.UnParse(stream);
            }

            //Version 3+
            if (mSection.Version >= 0x0003) s.Write(mByte14);

            //Version 4+
            if (mSection.Version >= 0x0004) s.Write(mFloat48);

        }
        public override List<string> ContentFields
        {
            get
            {
                var fields = base.ContentFields;
                if (mSection.Version < 4) fields.Remove("Float48");
                if (mSection.Version < 3) fields.Remove("Byte14");
                if (mSection.Version < 2)
                {
                    fields.Remove("Float45");
                    fields.Remove("Float46");
                    fields.Remove("Float47");
                }
                return fields;
            }
        }
        public bool Equals(ParticleEffect other)
        {
            return base.Equals(other);
        }
    }
}