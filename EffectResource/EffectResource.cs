using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Effects;
using s3piwrappers.Resources;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;
using System.Collections;

namespace s3piwrappers
{
    public class EffectResource : AResource
    {
        #region Effect Section

        #region Nested Type: EffectSectionList
        public class EffectSectionList : SectionList<EffectSection>
        {
            public EffectSectionList(EventHandler handler)
                : base(handler)
            {
            }

            public EffectSectionList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            protected override Type GetSectionType(ushort id)
            {
                switch (id)
                {
                    case 0x01: return typeof(ParticleEffectSection);
                    case 0x02: return typeof(MetaparticleEffectSection);
                    case 0x03: return typeof(DecalEffectSection);
                    case 0x04: return typeof(SequenceEffectSection);
                    case 0x05: return typeof(SoundEffectSection);
                    case 0x06: return typeof(ShakeEffectSection);
                    case 0x07: return typeof(CameraEffectSection);
                    case 0x08: return typeof(ModelEffectSection);
                    case 0x09: return typeof(ScreenEffectSection);
                    case 0x0b: return typeof(GameEffectSection);
                    case 0x0c: return typeof(FastParticleEffectSection);
                    case 0x0D: return typeof(DistributeEffectSection);
                    case 0x0E: return typeof(RibbonEffectSection);
                    default: throw new NotSupportedException("Effect Section type 0x" + id.ToString("X4") + " is not supported.");
                }
            }
        }
        #endregion

        #region Nested Type: EffectSection
        public abstract class EffectSection : Section, IEquatable<EffectSection>
        {
            protected EffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }

            protected EffectSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            protected EffectSection(int apiVersion, EventHandler handler, EffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            protected abstract override void Parse(Stream s);

            public abstract override void UnParse(Stream s);


            public bool Equals(EffectSection other)
            {
                return mType.Equals(other.mType);
            }
        }
        public abstract class EffectSection<T> : EffectSection
            where T : Effect, IEquatable<T>
        {
            protected EffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
                mItems = new SectionDataList<T>(handler, this);
            }

            protected EffectSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            protected EffectSection(int apiVersion, EventHandler handler, EffectSection<T> basis)
                : base(apiVersion, handler, basis)
            {
            }


            protected override void Parse(Stream s)
            {
                mItems = new SectionDataList<T>(handler, this, s);
            }

            public override void UnParse(Stream s)
            {
                mItems.UnParse(s);
            }
        }

        #endregion

        [ConstructorParameters(new object[] { 0x0001, 0x0003 })]
        public class ParticleEffectSection : EffectSection<ParticleEffect>
        {
            public ParticleEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            public ParticleEffectSection(int apiVersion, EventHandler handler, ParticleEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ParticleEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0002, 0x0001 })]
        public class MetaparticleEffectSection : EffectSection<MetaparticleEffect>
        {
            public MetaparticleEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public MetaparticleEffectSection(int apiVersion, EventHandler handler, MetaparticleEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public MetaparticleEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0003, 0x0002 })]
        public class DecalEffectSection : EffectSection<DecalEffect>
        {
            public DecalEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public DecalEffectSection(int apiVersion, EventHandler handler, DecalEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public DecalEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0004, 0x0001 })]
        public class SequenceEffectSection : EffectSection<SequenceEffect>
        {
            public SequenceEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public SequenceEffectSection(int apiVersion, EventHandler handler, SequenceEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public SequenceEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0005, 0x0001 })]
        public class SoundEffectSection : EffectSection<SoundEffect>
        {
            public SoundEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public SoundEffectSection(int apiVersion, EventHandler handler, SoundEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public SoundEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0006, 0x0001 })]
        public class ShakeEffectSection : EffectSection<ShakeEffect>
        {
            public ShakeEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public ShakeEffectSection(int apiVersion, EventHandler handler, ShakeEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ShakeEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0007, 0x0001 })]
        public class CameraEffectSection : EffectSection<CameraEffect>
        {
            public CameraEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public CameraEffectSection(int apiVersion, EventHandler handler, CameraEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public CameraEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0008, 0x0001 })]
        public class ModelEffectSection : EffectSection<ModelEffect>
        {
            public ModelEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public ModelEffectSection(int apiVersion, EventHandler handler, ModelEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ModelEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0009, 0x0001 })]
        public class ScreenEffectSection : EffectSection<ScreenEffect>
        {
            public ScreenEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public ScreenEffectSection(int apiVersion, EventHandler handler, ScreenEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public ScreenEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x000B, 0x0001 })]
        public class GameEffectSection : EffectSection<DefaultEffect>
        {
            public GameEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public GameEffectSection(int apiVersion, EventHandler handler, GameEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public GameEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x000C, 0x0001 })]
        public class FastParticleEffectSection : EffectSection<DefaultEffect>
        {
            public FastParticleEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public FastParticleEffectSection(int apiVersion, EventHandler handler, FastParticleEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public FastParticleEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x000D, 0x0001 })]
        public class DistributeEffectSection : EffectSection<DistributeEffect>
        {
            public DistributeEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public DistributeEffectSection(int apiVersion, EventHandler handler, DistributeEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public DistributeEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x000E, 0x0001 })]
        public class RibbonEffectSection : EffectSection<DefaultEffect>
        {
            public RibbonEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }

            public RibbonEffectSection(int apiVersion, EventHandler handler, RibbonEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }

            public RibbonEffectSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }
        }
        #endregion

        #region Resource Section
        public abstract class ResourceSection : Section, IEquatable<ResourceSection>
        {
            protected ResourceSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }

            protected ResourceSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            protected ResourceSection(int apiVersion, EventHandler handler, ResourceSection basis)
                : base(apiVersion, handler, basis)
            {
            }
            protected abstract override void Parse(Stream s);

            public abstract override void UnParse(Stream s);


            public bool Equals(ResourceSection other)
            {
                return mType.Equals(other.mType);
            }
        }
        public abstract class ResourceSection<T> : ResourceSection
            where T : Resource, IEquatable<T>
        {
            protected ResourceSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
                mItems = new SectionDataList<T>(handler,this);
            }

            protected ResourceSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            protected ResourceSection(int apiVersion, EventHandler handler, ResourceSection<T> basis)
                : base(apiVersion, handler, basis)
            {
            }

            protected override void Parse(Stream s)
            {
                mItems = new SectionDataList<T>(handler, this, s);
            }

            public override void UnParse(Stream s)
            {
                mItems.UnParse(s);
            }

        }
        [ConstructorParameters(new object[] { 0x0000, 0x0000 })]
        public class MapSection : ResourceSection<Map>
        {
            public MapSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }

            public MapSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            public MapSection(int apiVersion, EventHandler handler, MapSection basis)
                : base(apiVersion, handler, basis)
            {
            }
        }
        [ConstructorParameters(new object[] { 0x0001, 0x0000 })]
        public class MaterialSection : ResourceSection<Material>
        {
            public MaterialSection(int apiVersion, EventHandler handler, ushort type, ushort version)
                : base(apiVersion, handler, type, version)
            {
            }

            public MaterialSection(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
            }

            public MaterialSection(int apiVersion, EventHandler handler, MaterialSection basis)
                : base(apiVersion, handler, basis)
            {
            }
        }
        public class ResourceSectionList : SectionList<ResourceSection>
        {
            public ResourceSectionList(EventHandler handler)
                : base(handler)
            {
            }

            public ResourceSectionList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            protected override Type GetSectionType(ushort id)
            {
                switch (id)
                {
                    case 0x000:
                        return typeof(MapSection);
                    case 0x0001:
                        return typeof(MaterialSection);
                    default: throw new NotSupportedException("Resource Section type 0x" + id.ToString("X4") + " is not supported.");
                }
            }
        }
        #endregion

        #region VisualEffect Section
        public class VisualEffectSection : Section<VisualEffect>
        {
            public VisualEffectSection(int apiVersion, EventHandler handler, UInt16 version)
                : base(apiVersion, handler, 0, version)
            {
            }

            public VisualEffectSection(int apiVersion, EventHandler handler, UInt16 version, Stream s)
                : base(apiVersion, handler, 0, version, s)
            {
            }

            public VisualEffectSection(int apiVersion, EventHandler handler, VisualEffectSection basis)
                : base(apiVersion, handler, basis)
            {
            }
        }

        public class VisualEffect : SectionData, IEquatable<VisualEffect>
        {

            #region Nested Type: Index
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

                public Index(int apiVersion, EventHandler handler, Section section) : base(apiVersion, handler, section)
                {
                    mOrientation = new Matrix3x3Value(0, handler);
                    mPosition = new Vector3ValueLE(0, handler);
                    mVector3List01 = new DataList<Vector3Value>(handler);
                }

                public Index(int apiVersion, EventHandler handler, Section section, Stream s) : base(apiVersion, handler, section, s)
                {
                }

                public Index(int apiVersion, EventHandler handler, SectionData basis) : base(apiVersion, handler, basis)
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



            public VisualEffect(int apiVersion, EventHandler handler, Section section)
                : base(apiVersion, handler, section)
            {
                mFloatList01 = new DataList<FloatValue>(handler);
                mItems = new SectionDataList<Index>(handler,mSection);
            }

            public VisualEffect(int apiVersion, EventHandler handler, Section section, Stream s)
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
            private float mFloat01;
            private float mFloat02;
            private UInt32 mInt04;
            private byte mByte01;
            private DataList<FloatValue> mFloatList01;
            private float mFloat03;
            private float mFloat04;
            private float mFloat05;
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
                s.Read(out mFloat01);
                s.Read(out mFloat02);
                s.Read(out mInt04);
                s.Read(out mByte01);
                mFloatList01 = new DataList<FloatValue>(handler, stream);
                s.Read(out mFloat03);
                s.Read(out mFloat04);
                s.Read(out mFloat05);
                s.Read(out mInt05);
                mItems = new SectionDataList<Index>(handler,mSection, stream);
            }
            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mInt01);
                s.Write(mInt02);
                s.Write(mInt03);
                s.Write(mFloat01);
                s.Write(mFloat02);
                s.Write(mInt04);
                s.Write(mByte01);
                mFloatList01.UnParse(stream);
                s.Write(mFloat03);
                s.Write(mFloat04);
                s.Write(mFloat05);
                s.Write(mInt05);
                mItems.UnParse(stream);
            }
        }
        #endregion

        #region Nested Type: VisualEffectHandle
        public class VisualEffectHandle : DataElement, IEquatable<VisualEffectHandle>
        {
            public VisualEffectHandle(int apiVersion, EventHandler handler, VisualEffectHandle basis)
                : base(apiVersion, handler)
            {
                mEffectName = basis.EffectName;
                mIndex = basis.Index;
            }
            public VisualEffectHandle(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public VisualEffectHandle(int apiVersion, EventHandler handler, Stream s, uint index) : base(apiVersion, handler, s) { mIndex = index; }


            #region Fields
            private UInt32 mIndex;
            private string mEffectName = "<Insert Effect Name>";

            #endregion

            #region Properties
            [ElementPriority(2)]
            public String EffectName
            {
                get { return mEffectName; }
                set { mEffectName = value; OnElementChanged(); }
            }
            [ElementPriority(1)]
            public uint Index
            {
                get { return mIndex; }
                set { mIndex = value; OnElementChanged(); }
            }

            #endregion


            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mEffectName = s.ReadString(StringType.ZeroDelimited);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mIndex);
                s.Write(mEffectName, StringType.ZeroDelimited);
            }


            #region IComparable<VisualEffectHandle> Members

            public int CompareTo(VisualEffectHandle other)
            {
                return mEffectName.CompareTo(other.mEffectName);
            }

            #endregion

            #region IComparable Members

            public int CompareTo(object obj)
            {
                return mEffectName.CompareTo(((VisualEffectHandle)obj).mEffectName);
            }

            #endregion

            public override string ToString()
            {
                return EffectName;
            }

            public bool Equals(VisualEffectHandle other)
            {
                return mEffectName.Equals(other);
            }
        }
        #endregion

        #region Nested Type: VisualEffectHandleList
        public class VisualEffectHandleList : AHandlerList<VisualEffectHandle>, IGenericAdd
        {

            public VisualEffectHandleList(EventHandler handler) : base(handler) { }
            public VisualEffectHandleList(EventHandler handler, Stream s)
                : base(handler)
            {
                Parse(s);
            }
            protected void Parse(Stream s)
            {
                BinaryStreamWrapper bw = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                uint index = bw.ReadUInt32();
                while (index != 0xFFFFFFFF)
                {
                    VisualEffectHandle item = new VisualEffectHandle(0, base.handler, s, index);
                    index = bw.ReadUInt32();
                    base.Add(item);
                }
            }

            public void UnParse(Stream s)
            {
                BinaryStreamWrapper w = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                foreach (var item in this)
                {
                    VisualEffectHandle handle = (VisualEffectHandle)item;
                    ((VisualEffectHandle)item).UnParse(s);
                }
                w.Write((uint)0xFFFFFFFF);
            }

            public bool Add(params object[] fields)
            {
                if (fields == null)
                {
                    return false;
                }
                var args = new object[2 + fields.Length];
                args[0] = 0;
                args[1] = handler;
                Array.Copy(fields,0,args,2,fields.Length);
                base.Add((VisualEffectHandle)Activator.CreateInstance(typeof(VisualEffectHandle),args));
                return true;
            }

            public void Add()
            {
                Add(new object[]{});
            }
        }
        #endregion

        #region Fields
        private UInt16 mVersion = 0x00000002;
        private EffectSectionList mEffects;
        private ResourceSectionList mResources;
        private VisualEffectSection mVisualEffects;
        private byte[] mReserved;
        private VisualEffectHandleList mVisualEffectHandles;
        #endregion

        #region Properties
        [ElementPriority(1)]
        public UInt16 Version
        {
            get { return mVersion; }
            set { mVersion = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(2)]
        public EffectSectionList Effects
        {
            get { return mEffects; }
            set { mEffects = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public ResourceSectionList Resources
        {
            get { return mResources; }
            set { mResources = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        public VisualEffectSection VisualEffects
        {
            get { return mVisualEffects; }
            set { mVisualEffects = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(5)]
        public byte[] Reserved
        {
            get { return mReserved; }
            set { mReserved = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(6)]
        public VisualEffectHandleList VisualEffectHandles
        {
            get { return mVisualEffectHandles; }
            set { mVisualEffectHandles = value; OnResourceChanged(this, new EventArgs()); }
        }
        #endregion

        #region Constructors
        public EffectResource(int apiVersion, Stream s)
            : base(apiVersion, s)
        {
            if (base.stream == null)
            {
                base.stream = this.UnParse();
                this.OnResourceChanged(this, new EventArgs());
            }
            base.stream.Position = 0L;
            Parse(base.stream);
        }
        #endregion

        public void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            mVersion = s.ReadUInt16();
            mEffects = new EffectSectionList(this.OnResourceChanged, stream);
            mResources = new ResourceSectionList(this.OnResourceChanged, stream);
            mVisualEffects = new VisualEffectSection(0, this.OnResourceChanged, s.ReadUInt16(), stream);
            mReserved = s.ReadBytes(4);
            mVisualEffectHandles = new VisualEffectHandleList(this.OnResourceChanged, stream);
        }

        protected override Stream UnParse()
        {
            MemoryStream stream = new MemoryStream();
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mVersion);
            if (mEffects == null) mEffects = new EffectSectionList(this.OnResourceChanged);
            mEffects.UnParse(stream);
            if (this.mResources == null) this.mResources = new ResourceSectionList(this.OnResourceChanged);
            mResources.UnParse(stream);
            if (mVisualEffects == null) mVisualEffects = new VisualEffectSection(0, this.OnResourceChanged, 2);
            s.Write(mVisualEffects.Version);
            mVisualEffects.UnParse(stream);
            if (mReserved == null) mReserved = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
            s.Write(mReserved);
            if (mVisualEffectHandles == null) mVisualEffectHandles = new VisualEffectHandleList(this.OnResourceChanged);
            mVisualEffectHandles.UnParse(stream);
            return stream;
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedAPIVersion; }
        }
        const int kRecommendedAPIVersion = 1;
    }
}
