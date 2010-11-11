using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;

namespace s3piwrappers.Effects
{
    public class MetaparticleEffect : Effect, IEquatable<MetaparticleEffect>
    {

        public MetaparticleEffect(int apiVersion, EventHandler handler, MetaparticleEffect basis)
            : base(apiVersion, handler, basis)
        {
        }
        public MetaparticleEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mParticleParameters = new ParticleParams(0, handler);
            mFloatList01 = new DataList<FloatValue>(handler);
            mFloatList02 = new DataList<FloatValue>(handler);
            mFloatList03 = new DataList<FloatValue>(handler);
            mFloatList04 = new DataList<FloatValue>(handler);
            mFloatList05 = new DataList<FloatValue>(handler);
            mFloatList06 = new DataList<FloatValue>(handler);
            mFloatList07 = new DataList<FloatValue>(handler);
            mFloatList08 = new DataList<FloatValue>(handler);
            mColourList01 = new DataList<ColourValue>(handler);
            mVector3List01 = new DataList<Vector3ValueLE>(handler);
            mItemBList01 = new DataList<ItemB>(handler);
            mItemCList01 = new DataList<ItemC>(handler);
            mItemDList01 = new DataList<ItemD>(handler);
            mItemF01 = new ItemF(0, handler);
            mItemF02 = new ItemF(0, handler);
            mFloat08 = -1000000000.0f;
            mFloat09 = 0f;
            mFloat10 = -10000.0f;
            mFloat11 = 10000.0f;
        }
        public MetaparticleEffect(int apiVersion, EventHandler handler, ISection section, Stream s) : base(apiVersion, handler, section, s) { }

                private UInt32 mInt01;
        private UInt32 mInt02;
        private ParticleParams mParticleParameters;
        private DataList<FloatValue> mFloatList01;
        private float mFloat01;
        private UInt32 mFloat02;
        private DataList<FloatValue> mFloatList02;
        private float mFloat03;
        private DataList<FloatValue> mFloatList03;
        private DataList<FloatValue> mFloatList04;
        private DataList<FloatValue> mFloatList05;
        private float mFloat04;
        private float mFloat05;
        private float mFloat06;
        private float mFloat07;
        private float mFloat08;
        private float mFloat09;
        private DataList<ColourValue> mColourList01;
        private float mFloat10; //LE
        private float mFloat11; //LE
        private float mFloat12; //LE
        private DataList<FloatValue> mFloatList06;
        private float mFloat13;
        private String mBaseEffectName;
        private String mDeathEffectName;
        private Byte mByte01;
        private float mFloat14; //LE
        private float mFloat15; //LE
        private float mFloat16; //LE
        private float mFloat17; //LE
        private float mFloat18; //LE
        private float mFloat19; //LE

        private float mFloat20;
        private float mFloat21;
        private float mFloat22;

        private float mFloat23; //LE
        private float mFloat24; //LE
        private float mFloat25; //LE

        private float mFloat26;
        private float mFloat27;
        private DataList<ItemC> mItemCList01;
        private byte mByte02;
        private byte mByte03;
        private byte mByte04;
        private byte mByte05;
        private DataList<Vector3ValueLE> mVector3List01;
        private DataList<FloatValue> mFloatList07;
        private DataList<ItemB> mItemBList01; //25
        private float mFloat28;
        private float mFloat29;
        private float mFloat30;
        private float mFloat31;
        private float mFloat32;

        private float mFloat33;
        private float mFloat34;
        private float mFloat35; //LE
        private float mFloat36; //LE
        private UInt64 mLong01;
        private UInt64 mLong02;
        private UInt64 mLong03;

        private ItemF mItemF01;
        private ItemF mItemF02;

        private float mFloat51; //LE
        private float mFloat52; //LE
        private float mFloat53; //LE

        private float mFloat54;
        private float mFloat55;
        private float mFloat56;

        private float mFloat57; //LE
        private float mFloat58; //LE
        private float mFloat59; //LE

        private DataList<FloatValue> mFloatList08;
        private float mFloat60;
        private float mFloat61;
        private DataList<ItemD> mItemDList01;
        private float mFloat62;


        

        
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
        public ParticleParams ParticleParameters
        {
            get { return mParticleParameters; }
            set { mParticleParameters = value; OnElementChanged(); }
        }

        [ElementPriority(4)]
        public DataList<FloatValue> FloatList01
        {
            get { return mFloatList01; }
            set { mFloatList01 = value; OnElementChanged(); }
        }

        [ElementPriority(5)]
        public float Float01
        {
            get { return mFloat01; }
            set { mFloat01 = value; OnElementChanged(); }
        }

        [ElementPriority(6)]
        public UInt32 Float02
        {
            get { return mFloat02; }
            set { mFloat02 = value; OnElementChanged(); }
        }

        [ElementPriority(7)]
        public DataList<FloatValue> FloatList02
        {
            get { return mFloatList02; }
            set { mFloatList02 = value; OnElementChanged(); }
        }

        [ElementPriority(8)]
        public float Float03
        {
            get { return mFloat03; }
            set { mFloat03 = value; OnElementChanged(); }
        }

        [ElementPriority(9)]
        public DataList<FloatValue> FloatList03
        {
            get { return mFloatList03; }
            set { mFloatList03 = value; OnElementChanged(); }
        }

        [ElementPriority(10)]
        public DataList<FloatValue> FloatList04
        {
            get { return mFloatList04; }
            set { mFloatList04 = value; OnElementChanged(); }
        }

        [ElementPriority(11)]
        public DataList<FloatValue> FloatList05
        {
            get { return mFloatList05; }
            set { mFloatList05 = value; OnElementChanged(); }
        }

        [ElementPriority(1)]
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
        public DataList<ColourValue> ColourList01
        {
            get { return mColourList01; }
            set { mColourList01 = value; OnElementChanged(); }
        }

        [ElementPriority(18)]
        public float Float10
        {
            get { return mFloat10; }
            set { mFloat10 = value; OnElementChanged(); }
        }

        [ElementPriority(19)]
        public float Float11
        {
            get { return mFloat11; }
            set { mFloat11 = value; OnElementChanged(); }
        }

        [ElementPriority(20)]
        public float Float12
        {
            get { return mFloat12; }
            set { mFloat12 = value; OnElementChanged(); }
        }

        [ElementPriority(21)]
        public DataList<FloatValue> FloatList06
        {
            get { return mFloatList06; }
            set { mFloatList06 = value; OnElementChanged(); }
        }

        [ElementPriority(22)]
        public float Float13
        {
            get { return mFloat13; }
            set { mFloat13 = value; OnElementChanged(); }
        }

        [ElementPriority(23)]
        public string BaseEffectName
        {
            get { return mBaseEffectName; }
            set { mBaseEffectName = value; OnElementChanged(); }
        }

        [ElementPriority(24)]
        public string DeathEffectName
        {
            get { return mDeathEffectName; }
            set { mDeathEffectName = value; OnElementChanged(); }
        }

        [ElementPriority(25)]
        public byte Byte01
        {
            get { return mByte01; }
            set { mByte01 = value; OnElementChanged(); }
        }

        [ElementPriority(26)]
        public float Float14
        {
            get { return mFloat14; }
            set { mFloat14 = value; OnElementChanged(); }
        }

        [ElementPriority(27)]
        public float Float15
        {
            get { return mFloat15; }
            set { mFloat15 = value; OnElementChanged(); }
        }

        [ElementPriority(28)]
        public float Float16
        {
            get { return mFloat16; }
            set { mFloat16 = value; OnElementChanged(); }
        }

        [ElementPriority(29)]
        public float Float17
        {
            get { return mFloat17; }
            set { mFloat17 = value; OnElementChanged(); }
        }

        [ElementPriority(30)]
        public float Float18
        {
            get { return mFloat18; }
            set { mFloat18 = value; OnElementChanged(); }
        }

        [ElementPriority(31)]
        public float Float19
        {
            get { return mFloat19; }
            set { mFloat19 = value; OnElementChanged(); }
        }

        [ElementPriority(32)]
        public float Float20
        {
            get { return mFloat20; }
            set { mFloat20 = value; OnElementChanged(); }
        }

        [ElementPriority(33)]
        public float Float21
        {
            get { return mFloat21; }
            set { mFloat21 = value; OnElementChanged(); }
        }

        [ElementPriority(34)]
        public float Float22
        {
            get { return mFloat22; }
            set { mFloat22 = value; OnElementChanged(); }
        }

        [ElementPriority(35)]
        public float Float23
        {
            get { return mFloat23; }
            set { mFloat23 = value; OnElementChanged(); }
        }

        [ElementPriority(36)]
        public float Float24
        {
            get { return mFloat24; }
            set { mFloat24 = value; OnElementChanged(); }
        }

        [ElementPriority(37)]
        public float Float25
        {
            get { return mFloat25; }
            set { mFloat25 = value; OnElementChanged(); }
        }

        [ElementPriority(38)]
        public float Float26
        {
            get { return mFloat26; }
            set { mFloat26 = value; OnElementChanged(); }
        }

        [ElementPriority(39)]
        public float Float27
        {
            get { return mFloat27; }
            set { mFloat27 = value; OnElementChanged(); }
        }

        [ElementPriority(40)]
        public DataList<ItemC> ItemCList01
        {
            get { return mItemCList01; }
            set { mItemCList01 = value; OnElementChanged(); }
        }

        [ElementPriority(41)]
        public byte Byte02
        {
            get { return mByte02; }
            set { mByte02 = value; OnElementChanged(); }
        }

        [ElementPriority(42)]
        public byte Byte03
        {
            get { return mByte03; }
            set { mByte03 = value; OnElementChanged(); }
        }

        [ElementPriority(43)]
        public byte Byte04
        {
            get { return mByte04; }
            set { mByte04 = value; OnElementChanged(); }
        }

        [ElementPriority(44)]
        public byte Byte05
        {
            get { return mByte05; }
            set { mByte05 = value; OnElementChanged(); }
        }

        [ElementPriority(45)]
        public DataList<Vector3ValueLE> Vector3List01
        {
            get { return mVector3List01; }
            set { mVector3List01 = value; OnElementChanged(); }
        }

        [ElementPriority(46)]
        public DataList<FloatValue> FloatList07
        {
            get { return mFloatList07; }
            set { mFloatList07 = value; OnElementChanged(); }
        }

        [ElementPriority(47)]
        public DataList<ItemB> ItemBList01
        {
            get { return mItemBList01; }
            set { mItemBList01 = value; OnElementChanged(); }
        }

        [ElementPriority(48)]
        public float Float28
        {
            get { return mFloat28; }
            set { mFloat28 = value; OnElementChanged(); }
        }

        [ElementPriority(49)]
        public float Float29
        {
            get { return mFloat29; }
            set { mFloat29 = value; OnElementChanged(); }
        }

        [ElementPriority(50)]
        public float Float30
        {
            get { return mFloat30; }
            set { mFloat30 = value; OnElementChanged(); }
        }

        [ElementPriority(51)]
        public float Float31
        {
            get { return mFloat31; }
            set { mFloat31 = value; OnElementChanged(); }
        }

        [ElementPriority(52)]
        public float Float32
        {
            get { return mFloat32; }
            set { mFloat32 = value; OnElementChanged(); }
        }

        [ElementPriority(53)]
        public float Float33
        {
            get { return mFloat33; }
            set { mFloat33 = value; OnElementChanged(); }
        }

        [ElementPriority(54)]
        public float Float34
        {
            get { return mFloat34; }
            set { mFloat34 = value; OnElementChanged(); }
        }

        [ElementPriority(55)]
        public float Float35
        {
            get { return mFloat35; }
            set { mFloat35 = value; OnElementChanged(); }
        }

        [ElementPriority(56)]
        public float Float36
        {
            get { return mFloat36; }
            set { mFloat36 = value; OnElementChanged(); }
        }

        [ElementPriority(57)]
        public ulong Long01
        {
            get { return mLong01; }
            set { mLong01 = value; OnElementChanged(); }
        }

        [ElementPriority(58)]
        public ulong Long02
        {
            get { return mLong02; }
            set { mLong02 = value; OnElementChanged(); }
        }

        [ElementPriority(59)]
        public ulong Long03
        {
            get { return mLong03; }
            set { mLong03 = value; OnElementChanged(); }
        }

        [ElementPriority(60)]
        public ItemF ItemF01
        {
            get { return mItemF01; }
            set { mItemF01 = value; OnElementChanged(); }
        }
        [ElementPriority(61)]
        public ItemF ItemF02
        {
            get { return mItemF02; }
            set { mItemF02 = value; OnElementChanged(); }
        }
        [ElementPriority(62)]
        public float Float51
        {
            get { return mFloat51; }
            set { mFloat51 = value; OnElementChanged(); }
        }

        [ElementPriority(63)]
        public float Float52
        {
            get { return mFloat52; }
            set { mFloat52 = value; OnElementChanged(); }
        }

        [ElementPriority(64)]
        public float Float53
        {
            get { return mFloat53; }
            set { mFloat53 = value; OnElementChanged(); }
        }

        [ElementPriority(65)]
        public float Float54
        {
            get { return mFloat54; }
            set { mFloat54 = value; OnElementChanged(); }
        }

        [ElementPriority(66)]
        public float Float55
        {
            get { return mFloat55; }
            set { mFloat55 = value; OnElementChanged(); }
        }

        [ElementPriority(67)]
        public float Float56
        {
            get { return mFloat56; }
            set { mFloat56 = value; OnElementChanged(); }
        }

        [ElementPriority(68)]
        public float Float57
        {
            get { return mFloat57; }
            set { mFloat57 = value; OnElementChanged(); }
        }

        [ElementPriority(69)]
        public float Float58
        {
            get { return mFloat58; }
            set { mFloat58 = value; OnElementChanged(); }
        }

        [ElementPriority(70)]
        public float Float59
        {
            get { return mFloat59; }
            set { mFloat59 = value; OnElementChanged(); }
        }

        [ElementPriority(71)]
        public DataList<FloatValue> FloatList08
        {
            get { return mFloatList08; }
            set { mFloatList08 = value; OnElementChanged(); }
        }

        [ElementPriority(72)]
        public float Float60
        {
            get { return mFloat60; }
            set { mFloat60 = value; OnElementChanged(); }
        }

        [ElementPriority(73)]
        public float Float61
        {
            get { return mFloat61; }
            set { mFloat61 = value; OnElementChanged(); }
        }

        [ElementPriority(74)]
        public DataList<ItemD> ItemDList01
        {
            get { return mItemDList01; }
            set { mItemDList01 = value; OnElementChanged(); }
        }

        [ElementPriority(75)]
        public float Float62
        {
            get { return mFloat62; }
            set { mFloat62 = value; OnElementChanged(); }
        }

        

        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mInt01);
            s.Read(out mInt02);
            mParticleParameters = new ParticleParams(0, handler, stream);
            mFloatList01 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat01);
            s.Read(out mFloat02);
            mFloatList02 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat03);
            mFloatList03 = new DataList<FloatValue>(handler, stream);
            mFloatList04 = new DataList<FloatValue>(handler, stream);
            mFloatList05 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat04);
            s.Read(out mFloat05);
            s.Read(out mFloat06);
            s.Read(out mFloat07);
            s.Read(out mFloat08);
            s.Read(out mFloat09);
            mColourList01 = new DataList<ColourValue>(handler, stream);
            s.Read(out mFloat10, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat11, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat12, ByteOrder.LittleEndian); //LE
            mFloatList06 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat13);
            s.Read(out mBaseEffectName, StringType.ZeroDelimited);
            s.Read(out mDeathEffectName, StringType.ZeroDelimited);
            s.Read(out mByte01);
            s.Read(out mFloat14, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat15, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat16, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat17, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat18, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat19, ByteOrder.LittleEndian); //LE

            s.Read(out mFloat20);
            s.Read(out mFloat21);
            s.Read(out mFloat22);

            s.Read(out mFloat23, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat24, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat25, ByteOrder.LittleEndian); //LE

            s.Read(out mFloat26);
            s.Read(out mFloat27);
            mItemCList01 = new DataList<ItemC>(handler, stream);
            s.Read(out mByte02);
            s.Read(out mByte03);
            s.Read(out mByte04);
            s.Read(out mByte05);
            mVector3List01 = new DataList<Vector3ValueLE>(handler, stream);
            FloatList07 = new DataList<FloatValue>(handler, stream);
            mItemBList01 = new DataList<ItemB>(handler, stream);
            s.Read(out mFloat28);
            s.Read(out mFloat29);
            s.Read(out mFloat30);
            s.Read(out mFloat31);
            s.Read(out mFloat32);
            s.Read(out mFloat33);
            s.Read(out mFloat34);

            s.Read(out mFloat35, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat36, ByteOrder.LittleEndian); //LE

            s.Read(out mLong01);
            s.Read(out mLong02);
            s.Read(out mLong03);

            mItemF01 = new ItemF(0, handler, stream);
            mItemF02 = new ItemF(0, handler, stream);

            s.Read(out mFloat51, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat52, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat53, ByteOrder.LittleEndian); //LE

            s.Read(out mFloat54);
            s.Read(out mFloat55);
            s.Read(out mFloat56);

            s.Read(out mFloat57, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat58, ByteOrder.LittleEndian); //LE
            s.Read(out mFloat59, ByteOrder.LittleEndian); //LE

            mFloatList08 = new DataList<FloatValue>(handler, stream);
            s.Read(out mFloat60);
            s.Read(out mFloat61);
            mItemDList01 = new DataList<ItemD>(handler, stream);
            s.Read(out mFloat62);
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);

            s.Write(mInt01);
            s.Write(mInt02);
            mParticleParameters.UnParse(stream);
            mFloatList01.UnParse(stream);
            s.Write(mFloat01);
            s.Write(mFloat02);
            mFloatList02.UnParse(stream);
            s.Write(mFloat03);
            mFloatList03.UnParse(stream);
            mFloatList04.UnParse(stream);
            mFloatList05.UnParse(stream);
            s.Write(mFloat04);
            s.Write(mFloat05);
            s.Write(mFloat06);
            s.Write(mFloat07);
            s.Write(mFloat08);
            s.Write(mFloat09);
            mColourList01.UnParse(stream);
            s.Write(mFloat10, ByteOrder.LittleEndian); //LE
            s.Write(mFloat11, ByteOrder.LittleEndian); //LE
            s.Write(mFloat12, ByteOrder.LittleEndian); //LE
            mFloatList06.UnParse(stream);
            s.Write(mFloat13);
            s.Write(mBaseEffectName, StringType.ZeroDelimited);
            s.Write(mDeathEffectName, StringType.ZeroDelimited);
            s.Write(mByte01);
            s.Write(mFloat14, ByteOrder.LittleEndian); //LE
            s.Write(mFloat15, ByteOrder.LittleEndian); //LE
            s.Write(mFloat16, ByteOrder.LittleEndian); //LE
            s.Write(mFloat17, ByteOrder.LittleEndian); //LE
            s.Write(mFloat18, ByteOrder.LittleEndian); //LE
            s.Write(mFloat19, ByteOrder.LittleEndian); //LE

            s.Write(mFloat20);
            s.Write(mFloat21);
            s.Write(mFloat22);

            s.Write(mFloat23, ByteOrder.LittleEndian); //LE
            s.Write(mFloat24, ByteOrder.LittleEndian); //LE
            s.Write(mFloat25, ByteOrder.LittleEndian); //LE

            s.Write(mFloat26);
            s.Write(mFloat27);
            mItemCList01.UnParse(stream);
            s.Write(mByte02);
            s.Write(mByte03);
            s.Write(mByte04);
            s.Write(mByte05);
            mVector3List01.UnParse(stream);
            mFloatList07.UnParse(stream);
            mItemBList01.UnParse(stream);
            s.Write(mFloat28);
            s.Write(mFloat29);
            s.Write(mFloat30);
            s.Write(mFloat31);
            s.Write(mFloat32);
            s.Write(mFloat33);
            s.Write(mFloat34);

            s.Write(mFloat35, ByteOrder.LittleEndian); //LE
            s.Write(mFloat36, ByteOrder.LittleEndian); //LE

            s.Write(mLong01);
            s.Write(mLong02);
            s.Write(mLong03);

            mItemF01.UnParse(stream);
            mItemF02.UnParse(stream);

            s.Write(mFloat51, ByteOrder.LittleEndian); //LE
            s.Write(mFloat52, ByteOrder.LittleEndian); //LE
            s.Write(mFloat53, ByteOrder.LittleEndian); //LE

            s.Write(mFloat54);
            s.Write(mFloat55);
            s.Write(mFloat56);

            s.Write(mFloat57, ByteOrder.LittleEndian); //LE
            s.Write(mFloat58, ByteOrder.LittleEndian); //LE
            s.Write(mFloat59, ByteOrder.LittleEndian); //LE

            mFloatList08.UnParse(stream);
            s.Write(mFloat60);
            s.Write(mFloat61);
            mItemDList01.UnParse(stream);
            s.Write(mFloat62);
        }


        public bool Equals(MetaparticleEffect other)
        {
            return base.Equals(other);
        }
    }
}