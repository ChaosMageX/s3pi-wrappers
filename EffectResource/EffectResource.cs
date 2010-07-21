using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using System.ComponentModel;
using System.Globalization;
using s3piwrappers.IO;

namespace s3piwrappers
{
    public class EffectResource : AResource
    {
        #region Nested Type: FloatList
        public class FloatList : AResource.DependentList<float>
        {
            public FloatList(EventHandler handler) : base(handler) { }
            public FloatList(EventHandler handler, Stream s) : base(handler, s) { }

            protected override uint ReadCount(Stream s)
            {
                return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
            }

            protected override void WriteCount(Stream s, uint count)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
            }

            public override void Add()
            {
                base.Add(0f);
            }

            protected override float CreateElement(Stream s)
            {
                return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadFloat();
            }

            protected override void WriteElement(Stream s, float element)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write(element);
            }
        }
        #endregion

        #region Nested Type: FloatListLE
        public class FloatListLE : AResource.DependentList<float>
        {

            public FloatListLE(EventHandler handler) : base(handler) { }
            public FloatListLE(EventHandler handler, Stream s) : base(handler, s) { }

            protected override uint ReadCount(Stream s)
            {
                return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
            }
            protected override void WriteCount(Stream s, uint count)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
            }

            public override void Add()
            {
                base.Add(0f);
            }

            protected override float CreateElement(Stream s)
            {
                return new BinaryStreamWrapper(s, ByteOrder.LittleEndian).ReadFloat();
            }

            protected override void WriteElement(Stream s, float element)
            {
                new BinaryStreamWrapper(s, ByteOrder.LittleEndian).Write(element);
            }
        }
        #endregion

        #region Nested Type: Vector3
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class Vector3 : AHandlerElement, IEquatable<Vector3>
        {
            private float mX, mY, mZ;
            [ElementPriority(1)]
            public float X
            {
                get { return mX; }
                set { mX = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Y
            {
                get { return mY; }
                set { mY = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public float Z
            {
                get { return mZ; }
                set { mZ = value; OnElementChanged(); }
            }

            public Vector3(int apiVersion, EventHandler handler, Vector3 basis) : this(apiVersion, handler, basis.X, basis.Y, basis.Z) { }
            public Vector3(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public Vector3(int apiVersion, EventHandler handler, float X, float Y, float Z)
                : base(apiVersion, handler)
            {
                this.mX = X;
                this.mY = Y;
                this.mZ = Z;
            }

            public override bool Equals(object obj)
            {
                if ((obj == null) || (obj.GetType() != base.GetType()))
                {
                    return false;
                }
                Vector3 vector = (Vector3)obj;
                return (((X == vector.X) && (Y == vector.Y)) && (Z == vector.Z));
            }

            public override int GetHashCode()
            {
                return ((X.GetHashCode() ^ Y.GetHashCode()) ^ Z.GetHashCode());
            }

            public override string ToString()
            {
                return String.Format("({0:0.000},{1:0.000},{2:0.000})", X, Y, Z);
            }
            public static bool TryParse(string values, out Vector3 vector3)
            {
                vector3 = null;
                if (values == null)
                {
                    return false;
                }
                NumberFormatInfo format = CultureInfo.CurrentCulture.NumberFormat;
                float x, y, z;
                string[] strArray = values.Replace(" ", "").Replace("f", "").Split(new char[] { ',' });
                if (strArray.Length != 3)
                {
                    return false;
                }
                if (!float.TryParse(strArray[0], NumberStyles.Float, format, out x))
                {
                    return false;
                }
                if (!float.TryParse(strArray[1], NumberStyles.Float, format, out y))
                {
                    return false;
                }
                if (!float.TryParse(strArray[2], NumberStyles.Float, format, out z))
                {
                    return false;
                }
                vector3 = new Vector3(0, null, x, y, z);
                return true;
            }


            public bool Equals(Vector3 other)
            {
                return mX.Equals(other.mX) && mY.Equals(other.mY) && mZ.Equals(other.mZ);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override System.Collections.Generic.List<string> ContentFields
            {
                get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, typeof(Vector3)); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedAPIVersion; }
            }
        }
        #endregion

        #region Nested Type: Vector3List
        public class Vector3List : AResource.DependentList<Vector3>
        {
            public Vector3List(EventHandler handler) : base(handler) { }
            public Vector3List(EventHandler handler, Stream s) : base(handler, s) { }
            protected override uint ReadCount(Stream s)
            {
                return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
            }
            protected override void WriteCount(Stream s, uint count)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
            }

            public override void Add()
            {
                base.Add(new object[0] { });
            }

            protected override Vector3 CreateElement(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                float x = s.ReadFloat();
                float y = s.ReadFloat();
                float z = s.ReadFloat();
                return new Vector3(0, elementHandler, x, y, z);
            }

            protected override void WriteElement(Stream stream, Vector3 element)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(element.X);
                s.Write(element.Y);
                s.Write(element.Z);
            }
        }
        #endregion

        #region Nested Type: Vector3ListLE
        public class Vector3ListLE : AResource.DependentList<Vector3>
        {
            public Vector3ListLE(EventHandler handler) : base(handler) { }
            public Vector3ListLE(EventHandler handler, Stream s) : base(handler, s) { }
            protected override uint ReadCount(Stream s)
            {
                return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
            }
            protected override void WriteCount(Stream s, uint count)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
            }

            public override void Add()
            {
                base.Add(new object[0] { });
            }

            protected override Vector3 CreateElement(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.LittleEndian);
                float x = s.ReadFloat();
                float y = s.ReadFloat();
                float z = s.ReadFloat();
                return new Vector3(0, elementHandler, x, y, z);
            }

            protected override void WriteElement(Stream stream, Vector3 element)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.LittleEndian);
                s.Write(element.X);
                s.Write(element.Y);
                s.Write(element.Z);
            }
        }
        #endregion

        #region Nested Type: ColorRgb
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class Color : AHandlerElement, IEquatable<Color>
        {
            public Color(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public Color(int apiVersion, EventHandler handler, Color basis) : this(apiVersion, handler, basis.Red, basis.Green, basis.Blue) { }
            public Color(int apiVersion, EventHandler handler, float r, float g, float b)
                : base(apiVersion, handler)
            {
                mRed = r;
                mGreen = g;
                mBlue = b;
            }
            private float mRed, mGreen, mBlue;
            [ElementPriority(1)]
            public float Red
            {
                get { return mRed; }
                set { mRed = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Green
            {
                get { return mGreen; }
                set { mGreen = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public float Blue
            {
                get { return mBlue; }
                set { mBlue = value; OnElementChanged(); }
            }

            public bool Equals(Color other)
            {
                return mRed == other.mRed && mGreen == other.mGreen && mBlue == other.mBlue;
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override System.Collections.Generic.List<string> ContentFields
            {
                get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, typeof(Color)); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedAPIVersion; }
            }
        }
        #endregion

        #region Nested Type: ColorList
        public class ColorList : AResource.DependentList<Color>
        {
            public ColorList(EventHandler handler) : base(handler) { }
            public ColorList(EventHandler handler, Stream s) : base(handler, s) { }

            protected override uint ReadCount(Stream s)
            {
                return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
            }
            protected override void WriteCount(Stream s, uint count)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
            }


            public override void Add()
            {
                base.Add(new object[0] { });
            }

            protected override Color CreateElement(Stream s)
            {
                var bw = new BinaryStreamWrapper(s, ByteOrder.LittleEndian);
                return new Color(0, elementHandler, bw.ReadFloat(), bw.ReadFloat(), bw.ReadFloat());
            }

            protected override void WriteElement(Stream s, Color element)
            {
                var bw = new BinaryStreamWrapper(s, ByteOrder.LittleEndian);
                bw.Write(element.Red);
                bw.Write(element.Green);
                bw.Write(element.Blue);
            }
        }
        #endregion

        #region Effect Sections
        #region Nested Type: EffectSection

        public abstract class AbstractEffectSection : AHandlerElement, IEquatable<AbstractEffectSection>
        {
            public static AbstractEffectSection CreateInstance(int apiVersion, EventHandler handler, ushort type, ushort version)
            {
                return CreateInstance(apiVersion, handler, type, version, null);
            }
            public static AbstractEffectSection CreateInstance(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
            {
                switch (type)
                {
                    case 0x0001:
                        return new ParticleEffectSection(apiVersion, handler, type, version, s);
                    case 0x0002:
                        return new MetaparticleEffectSection(apiVersion, handler, type, version, s);
                    case 0x0003:
                        return new DecalEffectSection(apiVersion, handler, type, version, s);
                    case 0x0004:
                        return new SequenceEffectSection(apiVersion, handler, type, version, s);
                    case 0x0005:
                        return new SoundEffectSection(apiVersion, handler, type, version, s);
                    case 0x0006:
                        return new ShakeEffectSection(apiVersion, handler, type, version, s);
                    case 0x0007:
                        return new CameraEffectSection(apiVersion, handler, type, version, s);
                    case 0x0008:
                        return new ModelEffectSection(apiVersion, handler, type, version, s);
                    case 0x0009:
                        return new ScreenEffectSection(apiVersion, handler, type, version, s);
                    case 0x000B:
                        return new GameEffectSection(apiVersion, handler, type, version, s);
                    case 0x000C:
                        return new FastParticleEffectSection(apiVersion, handler, type, version, s);
                    case 0x000D:
                        return new DistributeEffectSection(apiVersion, handler, type, version, s);
                    case 0x000E:
                        return new RibbonEffectSection(apiVersion, handler, type, version, s);
                    default:
                        throw new Exception("Invalid block section type 0x" + type.ToString("X4"));
                }
            }

            protected AbstractEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version)
                : base(apiVersion, handler)
            {
                mType = type;
                mVersion = version;
            }
            protected AbstractEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler)
            {
                mType = type;
                mVersion = version;
            }

            private UInt16 mType;
            private UInt16 mVersion;
            [ElementPriority(1)]
            public UInt16 BlockTypeId
            {
                get { return mType; }
            }
            [ElementPriority(2)]
            public UInt16 Version
            {
                get { return mVersion; }
                set { mVersion = value; OnElementChanged(); }
            }
            public bool Equals(AbstractEffectSection other)
            {
                return mType.Equals(other.mType);
            }
            public abstract void UnParse(Stream s);
        }
        public abstract class AbstractEffectSection<TEffect> : AbstractEffectSection
            where TEffect : AbstractEffect, IEquatable<TEffect>
        {
            private ushort mVersion;
            protected AbstractEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version)
                : base(apiVersion, handler, type, version)
            {
                mVersion = version;
                mList = new EffectList<TEffect>(handler);
            }
            protected AbstractEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s)
            {
                mVersion = version;
                if (s == null)
                {
                    mList = new EffectList<TEffect>(handler);
                }
                else { Parse(s); }
            }
            private EffectList<TEffect> mList;
            [ElementPriority(3)]
            public EffectList<TEffect> Items
            {
                get { return mList; }
                set { mList = value; OnElementChanged(); }
            }
            private void Parse(Stream s)
            {
                mList = new EffectList<TEffect>(handler, s, mVersion);
            }
            public override void UnParse(Stream s)
            {
                mList.UnParse(s);
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override List<string> ContentFields
            {
                get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedAPIVersion; }
            }

        }
        #endregion

        [ConstructorParameters(new object[] { 0x0001, 0x0003 })]
        public class ParticleEffectSection : AbstractEffectSection<ParticleEffect>
        {
            internal ParticleEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x0002, 0x0001 })]
        public class MetaparticleEffectSection : AbstractEffectSection<MetaparticleEffect>
        {
            internal MetaparticleEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x0003, 0x0002 })]
        public class DecalEffectSection : AbstractEffectSection<DecalEffect>
        {
            internal DecalEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x0004, 0x0001 })]
        public class SequenceEffectSection : AbstractEffectSection<SequenceEffect>
        {
            internal SequenceEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x0005, 0x0001 })]
        public class SoundEffectSection : AbstractEffectSection<SoundEffect>
        {
            internal SoundEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x0006, 0x0001 })]
        public class ShakeEffectSection : AbstractEffectSection<ShakeEffect>
        {
            internal ShakeEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x0007, 0x0001 })]
        public class CameraEffectSection : AbstractEffectSection<CameraEffect>
        {
            internal CameraEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x0008, 0x0001 })]
        public class ModelEffectSection : AbstractEffectSection<ModelEffect>
        {
            internal ModelEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x0009, 0x0001 })]
        public class ScreenEffectSection : AbstractEffectSection<ScreenEffect>
        {
            internal ScreenEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x000B, 0x0001 })]
        public class GameEffectSection : AbstractEffectSection<DefaultEffect>
        {
            internal GameEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x000C, 0x0001 })]
        public class FastParticleEffectSection : AbstractEffectSection<DefaultEffect>
        {
            internal FastParticleEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x000D, 0x0001 })]
        public class DistributeEffectSection : AbstractEffectSection<DistributeEffect>
        {
            internal DistributeEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }
        [ConstructorParameters(new object[] { 0x000E, 0x0001 })]
        public class RibbonEffectSection : AbstractEffectSection<DefaultEffect>
        {
            internal RibbonEffectSection(int apiVersion, EventHandler handler, UInt16 type, UInt16 version, Stream s)
                : base(apiVersion, handler, type, version, s) { }
        }

        #region Nested Type: EffectSectionList
        public class EffectSectionList : AHandlerList<AbstractEffectSection>, IGenericAdd
        {
            public EffectSectionList(EventHandler handler) : base(handler) { }
            public EffectSectionList(EventHandler handler, Stream s) : base(handler) { Parse(s); }
            protected void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                UInt16 blockType;
                blockType = s.ReadUInt16();
                while (blockType != 0xFFFF)
                {
                    UInt16 blockVersion = s.ReadUInt16();
                    AbstractEffectSection blocklist = AbstractEffectSection.CreateInstance(0, handler, blockType, blockVersion, stream);
                    base.Add(blocklist);
                    blockType = s.ReadUInt16();
                }
            }

            public void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                foreach (var list in this)
                {
                    s.Write(list.BlockTypeId);
                    s.Write(list.Version);
                    list.UnParse(stream);
                }
                s.Write((ushort)0xFFFF);
            }

            public bool Add(params object[] fields)
            {
                if (fields.Length == 0) return false;
                if (fields.Length == 1 && typeof(AbstractEffectSection).IsAssignableFrom(fields[0].GetType()))
                {
                    base.Add((AbstractEffectSection)fields[0]);
                    return true;
                }
                Add(AbstractEffectSection.CreateInstance(0, this.handler, (ushort)(int)fields[0], (ushort)(int)fields[1]));
                return true;
            }

            public void Add()
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #endregion

        #region Effects
        #region Nested Type: EffectList

        public class EffectList<TEffect> : AResource.DependentList<TEffect>
            where TEffect : AbstractEffect, IEquatable<TEffect>
        {
            private ushort mVersion;
            public EffectList(EventHandler handler)
                : this(handler, null, 1) { }
            public EffectList(EventHandler handler, Stream s, ushort version)
                : base(handler)
            {
                mVersion = version;
                if (s != null) Parse(s);
            }

            protected override uint ReadCount(Stream s)
            {
                return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
            }
            protected override void WriteCount(Stream s, uint count)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
            }
            protected override Type GetElementType(params object[] fields)
            {
                return typeof(TEffect);
            }
            protected override TEffect CreateElement(Stream s)
            {
                var e = (TEffect)Activator.CreateInstance(typeof(TEffect), new object[] { 0, elementHandler, s, mVersion });
                return e;
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override void WriteElement(Stream s, TEffect element)
            {
                element.UnParse(s);
            }
        }
        #endregion

        #region Nested Type: AbstractEffect
        public abstract class AbstractEffect : AHandlerElement, IEquatable<AbstractEffect>
        {

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class ParticleResourceKey : AHandlerElement
            {
                public ParticleResourceKey(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
                public ParticleResourceKey(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
                private UInt64 mLong01;
                private byte mByte01;
                private byte mByte02;
                private UInt32 mInt01; //if Byte02 & 0x80
                private byte mByte03;
                private byte mByte04;
                private UInt16 mShort01;
                private UInt32 mInt02;
                private UInt64 mLong02;
                protected void Parse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    s.Read(out mLong01);
                    s.Read(out mByte01);
                    s.Read(out mByte02);
                    if ((Byte02 & 0x80) == 0x80) s.Read(out mInt01);
                    s.Read(out mByte03);
                    s.Read(out mByte04);
                    s.Read(out mShort01);
                    s.Read(out mInt02);
                    s.Read(out mLong02);
                }
                public void UnParse(Stream stream)
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
                    s.Write(Long02);
                }

                public override AHandlerElement Clone(EventHandler handler)
                {
                    throw new NotImplementedException();
                }

                public override List<string> ContentFields
                {
                    get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
                }

                public override int RecommendedApiVersion
                {
                    get { return kRecommendedAPIVersion; }
                }
                [ElementPriority(1)]
                public ulong Instance
                {
                    get { return mLong01; }
                    set { mLong01 = value; OnElementChanged(); }
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
                public ulong Long02
                {
                    get { return mLong02; }
                    set { mLong02 = value; OnElementChanged(); }
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class ParticleParams : AHandlerElement
            {
                public ParticleParams(int apiVersion, EventHandler handler)
                    : base(apiVersion, handler)
                {
                    mVector3_1 = new Vector3(0, handler);
                    mVector3_2 = new Vector3(0, handler);
                    mVector3_3 = new Vector3(0, handler);
                    mVector3_4 = new Vector3(0, handler);
                }
                public ParticleParams(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }

                private float mFloat01 = 10f;
                private float mFloat02 = 10f;

                private float mFloat03;

                private float mFloat04 = -1f;
                private float mFloat05 = -1f;

                private float mFloat06 = -1f;
                private float mFloat07 = -1f;

                Vector3 mVector3_1;
                Vector3 mVector3_2;

                private float mFloat14;
                private float mFloat15;

                Vector3 mVector3_3;
                Vector3 mVector3_4;

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
                public Vector3 Vector3_01
                {
                    get { return mVector3_1; }
                    set { mVector3_1 = value; OnElementChanged(); }
                }
                [ElementPriority(9)]
                public Vector3 Vector3_02
                {
                    get { return mVector3_2; }
                    set { mVector3_2 = value; OnElementChanged(); }
                }
                [ElementPriority(10)]
                public float Float8
                {
                    get { return mFloat14; }
                    set { mFloat14 = value; OnElementChanged(); }
                }
                [ElementPriority(11)]
                public float Float9
                {
                    get { return mFloat15; }
                    set { mFloat15 = value; OnElementChanged(); }
                }
                [ElementPriority(12)]
                public Vector3 Vector3_03
                {
                    get { return mVector3_3; }
                    set { mVector3_3 = value; OnElementChanged(); }
                }
                [ElementPriority(13)]
                public Vector3 Vector3_04
                {
                    get { return mVector3_4; }
                    set { mVector3_4 = value; OnElementChanged(); }
                }
                [ElementPriority(14)]
                public float Float10
                {
                    get { return mFloat22; }
                    set { mFloat22 = value; OnElementChanged(); }
                }

                private void Parse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    s.Read(out mFloat01, ByteOrder.LittleEndian);
                    s.Read(out mFloat02, ByteOrder.LittleEndian);
                    s.Read(out mFloat03, ByteOrder.LittleEndian);
                    s.Read(out mFloat04, ByteOrder.LittleEndian);
                    s.Read(out mFloat05, ByteOrder.LittleEndian);
                    s.Read(out mFloat06, ByteOrder.LittleEndian);
                    s.Read(out mFloat07, ByteOrder.LittleEndian);
                    float x, y, z;
                    s.Read(out x, ByteOrder.LittleEndian);
                    s.Read(out y, ByteOrder.LittleEndian);
                    s.Read(out z, ByteOrder.LittleEndian);
                    mVector3_1 = new Vector3(0, handler, x, y, z);
                    s.Read(out x, ByteOrder.LittleEndian);
                    s.Read(out y, ByteOrder.LittleEndian);
                    s.Read(out z, ByteOrder.LittleEndian);
                    mVector3_2 = new Vector3(0, handler, x, y, z);
                    s.Read(out mFloat14, ByteOrder.LittleEndian);
                    s.Read(out mFloat15, ByteOrder.LittleEndian);
                    s.Read(out x, ByteOrder.LittleEndian);
                    s.Read(out y, ByteOrder.LittleEndian);
                    s.Read(out z, ByteOrder.LittleEndian);
                    mVector3_3 = new Vector3(0, handler, x, y, z);
                    s.Read(out x, ByteOrder.LittleEndian);
                    s.Read(out y, ByteOrder.LittleEndian);
                    s.Read(out z, ByteOrder.LittleEndian);
                    mVector3_4 = new Vector3(0, handler, x, y, z);
                    s.Read(out mFloat22);
                }

                public void UnParse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    s.Write(mFloat01, ByteOrder.LittleEndian);
                    s.Write(mFloat02, ByteOrder.LittleEndian);
                    s.Write(mFloat03, ByteOrder.LittleEndian);
                    s.Write(mFloat04, ByteOrder.LittleEndian);
                    s.Write(mFloat05, ByteOrder.LittleEndian);
                    s.Write(mFloat06, ByteOrder.LittleEndian);
                    s.Write(mFloat07, ByteOrder.LittleEndian);
                    s.Write(mVector3_1.X, ByteOrder.LittleEndian);
                    s.Write(mVector3_1.Y, ByteOrder.LittleEndian);
                    s.Write(mVector3_1.Z, ByteOrder.LittleEndian);
                    s.Write(mVector3_2.X, ByteOrder.LittleEndian);
                    s.Write(mVector3_2.Y, ByteOrder.LittleEndian);
                    s.Write(mVector3_2.Z, ByteOrder.LittleEndian);
                    s.Write(mFloat14, ByteOrder.LittleEndian);
                    s.Write(mFloat15, ByteOrder.LittleEndian);
                    s.Write(mVector3_3.X, ByteOrder.LittleEndian);
                    s.Write(mVector3_3.Y, ByteOrder.LittleEndian);
                    s.Write(mVector3_3.Z, ByteOrder.LittleEndian);
                    s.Write(mVector3_4.X, ByteOrder.LittleEndian);
                    s.Write(mVector3_4.Y, ByteOrder.LittleEndian);
                    s.Write(mVector3_4.Z, ByteOrder.LittleEndian);
                    s.Write(mFloat22);
                }

                public override AHandlerElement Clone(EventHandler handler)
                {
                    throw new NotImplementedException();
                }

                public override System.Collections.Generic.List<string> ContentFields
                {
                    get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
                }

                public override int RecommendedApiVersion
                {
                    get { return kRecommendedAPIVersion; }
                }
            }

            public class ItemA : AHandlerElement, IEquatable<ItemA>
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
                    MemoryStream ms = new MemoryStream();
                    basis.UnParse(ms);
                    ms.Position = 0L;
                    Parse(ms);
                }
                public ItemA(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
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


                protected void Parse(Stream stream)
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

                public void UnParse(Stream stream)
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
                public override AHandlerElement Clone(EventHandler handler)
                {
                    throw new NotImplementedException();
                }

                public override System.Collections.Generic.List<string> ContentFields
                {
                    get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
                }

                public override int RecommendedApiVersion
                {
                    get { return kRecommendedAPIVersion; }
                }

                public bool Equals(ItemA other)
                {
                    return base.Equals(other);
                }
            }
            public class ItemAList : AResource.DependentList<ItemA>
            {
                public ItemAList(EventHandler handler) : base(handler) { }
                public ItemAList(EventHandler handler, Stream s) : base(handler, s) { }
                public override void Add()
                {
                    base.Add(new object[0] { });
                }

                protected override ItemA CreateElement(Stream s)
                {
                    return new ItemA(0, this.elementHandler, s);
                }
                protected override void WriteElement(Stream s, ItemA element)
                {
                    element.UnParse(s);
                }
                protected override uint ReadCount(Stream s)
                {
                    return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
                }
                protected override void WriteCount(Stream s, uint count)
                {
                    new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
                }
            }
            public class ItemB : AHandlerElement, IEquatable<ItemB>
            {
                private byte[] mByteArray01 = new byte[4];
                private float mFloat01;
                private float mFloat02;
                private float mFloat03;
                private float mFloat04;
                private UInt32 mInt01;
                private UInt64 mLong01;
                private String mString01 = string.Empty;
                private String mString02 = string.Empty;
                private Vector3ListLE mVector3List01;

                public ItemB(int apiVersion, EventHandler handler, ItemB basis)
                    : base(apiVersion, handler)
                {
                    MemoryStream ms = new MemoryStream();
                    basis.UnParse(ms);
                    ms.Position = 0L;
                    Parse(ms);
                }
                public ItemB(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
                public ItemB(int apiVersion, EventHandler handler)
                    : base(apiVersion, handler)
                {
                    mVector3List01 = new Vector3ListLE(handler);
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
                public byte[] ByteArray01
                {
                    get { return mByteArray01; }
                    set { mByteArray01 = value; OnElementChanged(); }
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
                public Vector3ListLE Vector3List01
                {
                    get { return mVector3List01; }
                    set { mVector3List01 = value; OnElementChanged(); }
                }

                protected void Parse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    s.Read(out mInt01);
                    s.Read(out mLong01);
                    s.Read(out mFloat01);
                    s.Read(out mFloat02);
                    s.Read(out mFloat03);
                    s.Read(out mFloat04);
                    s.Read(out mByteArray01, 4);
                    s.Read(out mString01, StringType.ZeroDelimited);
                    s.Read(out mString02, StringType.ZeroDelimited);
                    mVector3List01 = new Vector3ListLE(handler, stream);
                }

                public void UnParse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    s.Write(mInt01);
                    s.Write(mLong01);
                    s.Write(mFloat01);
                    s.Write(mFloat02);
                    s.Write(mFloat03);
                    s.Write(mFloat04);
                    s.Write(mByteArray01);
                    s.Write(mString01, StringType.ZeroDelimited);
                    s.Write(mString02, StringType.ZeroDelimited);
                    mVector3List01.UnParse(stream);
                }

                public override AHandlerElement Clone(EventHandler handler)
                {
                    throw new NotImplementedException();
                }

                public override System.Collections.Generic.List<string> ContentFields
                {
                    get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
                }

                public override int RecommendedApiVersion
                {
                    get { return kRecommendedAPIVersion; }
                }

                public bool Equals(ItemB other)
                {
                    return base.Equals(other);
                }
            }

            public class ItemBList : AResource.DependentList<ItemB>
            {
                public ItemBList(EventHandler handler) : base(handler) { }
                public ItemBList(EventHandler handler, Stream s) : base(handler, s) { }
                public override void Add()
                {
                    base.Add(new object[0] { });
                }

                protected override ItemB CreateElement(Stream s)
                {
                    return new ItemB(0, this.elementHandler, s);
                }
                protected override void WriteElement(Stream s, ItemB element)
                {
                    element.UnParse(s);
                }
                protected override uint ReadCount(Stream s)
                {
                    return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
                }
                protected override void WriteCount(Stream s, uint count)
                {
                    new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
                }
            }

            public class ItemC : AHandlerElement, IEquatable<ItemC>
            {
                public ItemC(int apiVersion, EventHandler handler, ItemC basis)
                    : base(apiVersion, handler)
                {
                    MemoryStream ms = new MemoryStream();
                    basis.UnParse(ms);
                    ms.Position = 0L;
                    Parse(ms);
                }
                public ItemC(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
                public ItemC(int apiVersion, EventHandler handler)
                    : base(apiVersion, handler)
                {
                    mItems = new ItemEList(handler);
                }
                private ItemEList mItems;
                [ElementPriority(1)]
                public ItemEList Items
                {
                    get { return mItems; }
                    set { mItems = value; OnElementChanged(); }
                }

                protected void Parse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);

                    mItems = new ItemEList(handler, stream);
                }


                public void UnParse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    mItems.UnParse(stream);
                }

                public override AHandlerElement Clone(EventHandler handler)
                {
                    throw new NotImplementedException();
                }

                public override System.Collections.Generic.List<string> ContentFields
                {
                    get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
                }

                public override int RecommendedApiVersion
                {
                    get { return kRecommendedAPIVersion; }
                }

                public bool Equals(ItemC other)
                {
                    return base.Equals(other);
                }
            }
            public class ItemCList : AResource.DependentList<ItemC>
            {

                public ItemCList(EventHandler handler) : base(handler) { }
                public ItemCList(EventHandler handler, Stream s) : base(handler, s) { }
                public override void Add()
                {
                    base.Add(new object[0] { });
                }

                protected override ItemC CreateElement(Stream s)
                {
                    return new ItemC(0, this.elementHandler, s);
                }
                protected override void WriteElement(Stream s, ItemC element)
                {
                    element.UnParse(s);
                }
                protected override uint ReadCount(Stream s)
                {
                    return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
                }
                protected override void WriteCount(Stream s, uint count)
                {
                    new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
                }
            }

            public class ItemD : AHandlerElement, IEquatable<ItemD>
            {
                public ItemD(int apiVersion, EventHandler handler, ItemD basis)
                    : base(apiVersion, handler)
                {
                    MemoryStream ms = new MemoryStream();
                    basis.UnParse(ms);
                    ms.Position = 0L;
                    Parse(ms);
                }
                public ItemD(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
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

                protected void Parse(Stream stream)
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

                public void UnParse(Stream stream)
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
                public override System.Collections.Generic.List<string> ContentFields
                {
                    get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
                }

                public override int RecommendedApiVersion
                {
                    get { return kRecommendedAPIVersion; }
                }

                public bool Equals(ItemD other)
                {
                    return base.Equals(other);
                }

                public override AHandlerElement Clone(EventHandler handler)
                {
                    throw new NotImplementedException();
                }
            }
            public class ItemDList : AResource.DependentList<ItemD>
            {

                public ItemDList(EventHandler handler) : base(handler) { }
                public ItemDList(EventHandler handler, Stream s) : base(handler, s) { }
                public override void Add()
                {
                    base.Add(new object[0] { });
                }

                protected override ItemD CreateElement(Stream s)
                {
                    return new ItemD(0, this.elementHandler, s);
                }
                protected override void WriteElement(Stream s, ItemD element)
                {
                    element.UnParse(s);
                }
                protected override uint ReadCount(Stream s)
                {
                    return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
                }
                protected override void WriteCount(Stream s, uint count)
                {
                    new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
                }
            }


            public class ItemE : AHandlerElement, IEquatable<ItemE>
            {
                public ItemE(int apiVersion, EventHandler handler, ItemE basis)
                    : base(apiVersion, handler)
                {
                    MemoryStream ms = new MemoryStream();
                    basis.UnParse(ms);
                    ms.Position = 0L;
                    Parse(ms);
                }
                public ItemE(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
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

                protected void Parse(Stream stream)
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

                public void UnParse(Stream stream)
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
                public override System.Collections.Generic.List<string> ContentFields
                {
                    get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
                }

                public override int RecommendedApiVersion
                {
                    get { return kRecommendedAPIVersion; }
                }

                public bool Equals(ItemE other)
                {
                    return base.Equals(other);
                }

                public override AHandlerElement Clone(EventHandler handler)
                {
                    throw new NotImplementedException();
                }
            }
            public class ItemEList : AResource.DependentList<ItemE>
            {

                public ItemEList(EventHandler handler) : base(handler) { }
                public ItemEList(EventHandler handler, Stream s) : base(handler, s) { }
                public override void Add()
                {
                    base.Add(new object[0] { });
                }

                protected override ItemE CreateElement(Stream s)
                {
                    return new ItemE(0, this.elementHandler, s);
                }
                protected override void WriteElement(Stream s, ItemE element)
                {
                    element.UnParse(s);
                }
                protected override uint ReadCount(Stream s)
                {
                    return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
                }
                protected override void WriteCount(Stream s, uint count)
                {
                    new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
                }
            }
            protected ushort mVersion;
            protected AbstractEffect(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            protected AbstractEffect(int apiVersion, EventHandler handler, Stream s, ushort version)
                : base(apiVersion, handler)
            {
                mVersion = version;
                if (s != null) Parse(s);
            }
            protected AbstractEffect(int apiVersion, EventHandler handler, AbstractEffect basis)
                : base(apiVersion, handler)
            {
                MemoryStream ms = new MemoryStream();
                basis.UnParse(ms);
                ms.Position = 0L;
                Parse(ms);
            }
            protected abstract void Parse(Stream stream);
            public abstract void UnParse(Stream stream);

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override System.Collections.Generic.List<string> ContentFields
            {
                get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedAPIVersion; }
            }

            public bool Equals(AbstractEffect other)
            {
                return base.Equals(other);
            }
            [ElementPriority(0)]
            public virtual BinaryReader Data
            {
                get
                {
                    MemoryStream s = new MemoryStream();
                    UnParse(s);
                    s.Position = 0L;
                    return new BinaryReader(s);
                }
                set
                {
                    if (value.BaseStream.CanSeek)
                    {
                        value.BaseStream.Position = 0L;
                        this.Parse(value.BaseStream);
                    }
                    else
                    {
                        MemoryStream s = new MemoryStream();
                        byte[] buffer = new byte[0x100000];
                        for (int i = value.BaseStream.Read(buffer, 0, buffer.Length); i > 0; i = value.BaseStream.Read(buffer, 0, buffer.Length))
                        {
                            s.Write(buffer, 0, i);
                        }
                        this.Parse(s);
                    }
                    OnElementChanged();
                }
            }


        }
        #endregion

        #region Nested Type: DefaultEffect
        public class DefaultEffect : AbstractEffect, IEquatable<DefaultEffect>
        {
            public DefaultEffect(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public DefaultEffect(int apiVersion, EventHandler handler, Stream s, ushort version) : base(apiVersion, handler, s, version) { }
            protected override void Parse(Stream stream)
            {
                throw new NotImplementedException();
            }

            public override void UnParse(Stream stream)
            {
                throw new NotImplementedException();
            }

            public bool Equals(DefaultEffect other)
            {
                return base.Equals(other);
            }
        }
        #endregion


        #region Nested Type: ParticleEffect
        public class ParticleEffect : AbstractEffect, IEquatable<ParticleEffect>
        {
            public ParticleEffect(int apiVersion, EventHandler handler, ParticleEffect basis)
                : base(apiVersion, handler, basis)
            {
            }
            public ParticleEffect(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                ParticleParameters = new ParticleParams(RecommendedApiVersion, handler);
                mItemAList01 = new ItemAList(handler);
                mItemBList01 = new ItemBList(handler);
                mItemDList01 = new ItemDList(handler);
                mFloatList01 = new FloatList(handler);
                mFloatList02 = new FloatList(handler);
                mFloatList03 = new FloatList(handler);
                mFloatList04 = new FloatList(handler);
                mFloatList05 = new FloatList(handler);
                mFloatList06 = new FloatList(handler);
                mFloatList07 = new FloatList(handler);
                mFloatList08 = new FloatList(handler);
                mColorList01 = new ColorList(handler);
                mVector3List01 = new Vector3ListLE(handler);
                mVector3List02 = new Vector3ListLE(handler);


            }
            public ParticleEffect(int apiVersion, EventHandler handler, Stream s, ushort version) 
                : base(apiVersion, handler, s, version) { }

            #region Fields
            private UInt32 mInt01;
            private ParticleParams mParticleParameters;
            private FloatList mFloatList01;
            private float mFloat01 = 1.5f;
            private UInt16 mShort01 = 1;
            private float mFloat02;
            private FloatList mFloatList02;
            private float mFloat03;
            private FloatList mFloatList03;
            private float mFloat04;
            private float mFloat05;
            private float mFloat06;
            private FloatList mFloatList04;
            private FloatList mFloatList05;
            private float mFloat07;
            private ColorList mColorList01;
            private float mFloat08; //LE
            private float mFloat09; //LE
            private float mFloat10; //LE
            private ParticleResourceKey mResourceKey;

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

            private ItemAList mItemAList01;
            private Byte mByte09;
            private Byte mByte10;
            private Byte mByte11;
            private Byte mByte12;
            private Vector3ListLE mVector3List01;
            private FloatList mFloatList06;
            private ItemBList mItemBList01;
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


            private float mFloat33; //LE
            private float mFloat34; //LE
            private float mFloat35; //LE
            private float mFloat36; //LE

            private float mFloat37;
            private float mFloat38;
            private float mFloat39;

            private FloatList mFloatList07;
            private Byte mByte13;

            private float mFloat40; //LE
            private float mFloat41; //LE
            private float mFloat42; //LE

            private FloatList mFloatList08;
            private float mFloat43;
            private float mFloat44;

            private ItemDList mItemDList01;

            //Version 2+
            private float mFloat45; //LE
            private float mFloat46; //LE
            private float mFloat47; //LE
            private Vector3ListLE mVector3List02;

            //Version 3+
            private Byte mByte14;

            //Version 4+
            private float mFloat48;

            #endregion
            #region Properties

            [ElementPriority(83)]
            public float Float48
            {
                get { return mFloat48; }
                set { mFloat48 = value; OnElementChanged(); }
            }

            [ElementPriority(82)]
            public byte Byte14
            {
                get { return mByte14; }
                set { mByte14 = value; OnElementChanged(); }
            }

            [ElementPriority(81)]
            public Vector3ListLE Vector3List02
            {
                get { return mVector3List02; }
                set { mVector3List02 = value; OnElementChanged(); }
            }

            [ElementPriority(80)]
            public float Float47
            {
                get { return mFloat47; }
                set { mFloat47 = value; OnElementChanged(); }
            }

            [ElementPriority(79)]
            public float Float46
            {
                get { return mFloat46; }
                set { mFloat46 = value; OnElementChanged(); }
            }

            [ElementPriority(78)]
            public float Float45
            {
                get { return mFloat45; }
                set { mFloat45 = value; OnElementChanged(); }
            }

            [ElementPriority(77)]
            public ItemDList ItemDList01
            {
                get { return mItemDList01; }
                set { mItemDList01 = value; OnElementChanged(); }
            }

            [ElementPriority(76)]
            public float Float44
            {
                get { return mFloat44; }
                set { mFloat44 = value; OnElementChanged(); }
            }

            [ElementPriority(75)]
            public float Float43
            {
                get { return mFloat43; }
                set { mFloat43 = value; OnElementChanged(); }
            }

            [ElementPriority(74)]
            public FloatList FloatList08
            {
                get { return mFloatList08; }
                set { mFloatList08 = value; OnElementChanged(); }
            }

            [ElementPriority(73)]
            public float Float42
            {
                get { return mFloat42; }
                set { mFloat42 = value; OnElementChanged(); }
            }

            [ElementPriority(72)]
            public float Float41
            {
                get { return mFloat41; }
                set { mFloat41 = value; OnElementChanged(); }
            }

            [ElementPriority(71)]
            public float Float40
            {
                get { return mFloat40; }
                set { mFloat40 = value; OnElementChanged(); }
            }

            [ElementPriority(70)]
            public byte Byte13
            {
                get { return mByte13; }
                set { mByte13 = value; OnElementChanged(); }
            }

            [ElementPriority(69)]
            public FloatList FloatList07
            {
                get { return mFloatList07; }
                set { mFloatList07 = value; OnElementChanged(); }
            }

            [ElementPriority(68)]
            public float Float39
            {
                get { return mFloat39; }
                set { mFloat39 = value; OnElementChanged(); }
            }

            [ElementPriority(67)]
            public float Float38
            {
                get { return mFloat38; }
                set { mFloat38 = value; OnElementChanged(); }
            }

            [ElementPriority(66)]
            public float Float37
            {
                get { return mFloat37; }
                set { mFloat37 = value; OnElementChanged(); }
            }

            [ElementPriority(65)]
            public float Float36
            {
                get { return mFloat36; }
                set { mFloat36 = value; OnElementChanged(); }
            }

            [ElementPriority(64)]
            public float Float35
            {
                get { return mFloat35; }
                set { mFloat35 = value; OnElementChanged(); }
            }

            [ElementPriority(63)]
            public float Float34
            {
                get { return mFloat34; }
                set { mFloat34 = value; OnElementChanged(); }
            }

            [ElementPriority(62)]
            public float Float33
            {
                get { return mFloat33; }
                set { mFloat33 = value; OnElementChanged(); }
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
            public ItemBList ItemBList01
            {
                get { return mItemBList01; }
                set { mItemBList01 = value; OnElementChanged(); }
            }

            [ElementPriority(48)]
            public FloatList FloatList06
            {
                get { return mFloatList06; }
                set { mFloatList06 = value; OnElementChanged(); }
            }

            [ElementPriority(47)]
            public Vector3ListLE Vector3List01
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
            public ItemAList ItemAList01
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
            public ParticleResourceKey ResourceKey
            {
                get { return mResourceKey; }
                set { mResourceKey = value; OnElementChanged(); }
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
            public ColorList ColorList01
            {
                get { return mColorList01; }
                set { mColorList01 = value; OnElementChanged(); }
            }

            [ElementPriority(15)]
            public float Float07
            {
                get { return mFloat07; }
                set { mFloat07 = value; OnElementChanged(); }
            }

            [ElementPriority(14)]
            public FloatList FloatList05
            {
                get { return mFloatList05; }
                set { mFloatList05 = value; OnElementChanged(); }
            }

            [ElementPriority(13)]
            public FloatList FloatList04
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
            public FloatList FloatList03
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
            public FloatList FloatList02
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
            public FloatList FloatList01
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
                mFloatList01 = new FloatList(handler, stream);
                s.Read(out mFloat01);
                s.Read(out mShort01);
                s.Read(out mFloat02);
                mFloatList02 = new FloatList(handler, stream);
                s.Read(out mFloat03);
                mFloatList03 = new FloatList(handler, stream);
                s.Read(out mFloat04);
                s.Read(out mFloat05);
                s.Read(out mFloat06);
                mFloatList04 = new FloatList(handler, stream);
                mFloatList05 = new FloatList(handler, stream);
                s.Read(out mFloat07);
                mColorList01 = new ColorList(handler, stream);
                s.Read(out mFloat08, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat09, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat10, ByteOrder.LittleEndian); //LE
                mResourceKey = new ParticleResourceKey(0, handler, stream);

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

                mItemAList01 = new ItemAList(handler, stream);
                s.Read(out mByte09);
                s.Read(out mByte10);
                s.Read(out mByte11);
                s.Read(out mByte12);
                mVector3List01 = new Vector3ListLE(handler, stream);
                mFloatList06 = new FloatList(handler, stream);
                mItemBList01 = new ItemBList(handler, stream);
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


                s.Read(out mFloat33, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat34, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat35, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat36, ByteOrder.LittleEndian); //LE

                s.Read(out mFloat37);
                s.Read(out mFloat38);
                s.Read(out mFloat39);

                mFloatList07 = new FloatList(handler, stream);
                s.Read(out mByte13);

                s.Read(out mFloat40, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat41, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat42, ByteOrder.LittleEndian); //LE

                mFloatList08 = new FloatList(handler, stream);
                s.Read(out mFloat43);
                s.Read(out mFloat44);

                mItemDList01 = new ItemDList(handler, stream);

                //Version 2+
                if (mVersion >= 0x0002)
                {
                    s.Read(out mFloat45, ByteOrder.LittleEndian); //LE
                    s.Read(out mFloat46, ByteOrder.LittleEndian); //LE
                    s.Read(out mFloat47, ByteOrder.LittleEndian); //LE
                    mVector3List02 = new Vector3ListLE(handler, stream);
                }

                //Version 3+
                if(mVersion>= 0x0003)s.Read(out mByte14);

                //Version 4+
                if(mVersion>= 0x0004)s.Read(out mFloat48);

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
                mColorList01.UnParse(stream);
                s.Write(mFloat08, ByteOrder.LittleEndian); //LE
                s.Write(mFloat09, ByteOrder.LittleEndian); //LE
                s.Write(mFloat10, ByteOrder.LittleEndian); //LE
                mResourceKey.UnParse(stream);

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


                s.Write(mFloat33, ByteOrder.LittleEndian); //LE
                s.Write(mFloat34, ByteOrder.LittleEndian); //LE
                s.Write(mFloat35, ByteOrder.LittleEndian); //LE
                s.Write(mFloat36, ByteOrder.LittleEndian); //LE

                s.Write(mFloat37);
                s.Write(mFloat38);
                s.Write(mFloat39);

                mFloatList07.UnParse(stream);
                s.Write(mByte13);

                s.Write(mFloat40, ByteOrder.LittleEndian); //LE
                s.Write(mFloat41, ByteOrder.LittleEndian); //LE
                s.Write(mFloat42, ByteOrder.LittleEndian); //LE

                mFloatList08.UnParse(stream);
                s.Write(mFloat43);
                s.Write(mFloat44);

                mItemDList01.UnParse(stream);

                //Version 2+
                if (mVersion >= 0x0002)
                {
                    s.Write(mFloat45, ByteOrder.LittleEndian); //LE
                    s.Write(mFloat46, ByteOrder.LittleEndian); //LE
                    s.Write(mFloat47, ByteOrder.LittleEndian); //LE
                    mVector3List02.UnParse(stream);
                }

                //Version 3+
                if (mVersion >= 0x0003) s.Write(mByte14);

                //Version 4+
                if (mVersion >= 0x0004) s.Write(mFloat48);

            }

            public bool Equals(ParticleEffect other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        #region Nested Type: MetaparticleEffect
        public class MetaparticleEffect : AbstractEffect, IEquatable<MetaparticleEffect>
        {

            public MetaparticleEffect(int apiVersion, EventHandler handler, MetaparticleEffect basis)
                : base(apiVersion, handler, basis)
            {
            }
            public MetaparticleEffect(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                this.ParticleParameters = new ParticleParams(0, handler);
                this.ItemCList01 = new ItemCList(handler);
                FloatList01 = new FloatList(handler);
                FloatList02 = new FloatList(handler);
                FloatList03 = new FloatList(handler);
                FloatList04 = new FloatList(handler);
                FloatList05 = new FloatList(handler);
                FloatList06 = new FloatList(handler);
                FloatList07 = new FloatList(handler);
                FloatList08 = new FloatList(handler);
                ColorList01 = new ColorList(handler);

                this.ItemBList01 = new ItemBList(handler);

                Float08 = -1000000000.0f;
                Float09 = 0f;
                Float10 = -10000.0f;
                Float11 = 10000.0f;
            }
            public MetaparticleEffect(int apiVersion, EventHandler handler, Stream s, ushort version) : base(apiVersion, handler, s, version) { }

            #region Fields
            private UInt32 mInt01;
            private UInt32 mInt02;
            private ParticleParams mParticleParameters;
            private FloatList mFloatList01;
            private float mFloat01;
            private float mFloat02;
            private FloatList mFloatList02;
            private float mFloat03;
            private FloatList mFloatList03;
            private FloatList mFloatList04;
            private FloatList mFloatList05;
            private float mFloat04;
            private float mFloat05;
            private float mFloat06;
            private float mFloat07;
            private float mFloat08;
            private float mFloat09;
            private ColorList mColorList01;
            private float mFloat10; //LE
            private float mFloat11; //LE
            private float mFloat12; //LE
            private FloatList mFloatList06;
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
            private ItemCList mItemCList01;
            private byte mByte02;
            private byte mByte03;
            private byte mByte04;
            private byte mByte05;
            private Vector3ListLE mVector3List01;
            private FloatList mFloatList07;
            private ItemBList mItemBList01; //25
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

            private float mFloat37; //LE
            private float mFloat38; //LE
            private float mFloat39; //LE
            private float mFloat40; //LE

            private float mFloat41;
            private float mFloat42;
            private float mFloat43;
            private FloatList mFloatList08;
            private byte mByte06;

            private float mFloat44; //LE
            private float mFloat45; //LE
            private float mFloat46; //LE
            private float mFloat47; //LE

            private float mFloat48;
            private float mFloat49;
            private float mFloat50;
            private FloatList mFloatList09;
            private byte mByte07;

            private float mFloat51; //LE
            private float mFloat52; //LE
            private float mFloat53; //LE

            private float mFloat54;
            private float mFloat55;
            private float mFloat56;

            private float mFloat57; //LE
            private float mFloat58; //LE
            private float mFloat59; //LE

            private FloatList mFloatList10;
            private float mFloat60;
            private float mFloat61;
            private ItemDList mItemDList01;
            private float mFloat62;


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
            public ParticleParams ParticleParameters
            {
                get { return mParticleParameters; }
                set { mParticleParameters = value; OnElementChanged(); }
            }

            [ElementPriority(4)]
            public FloatList FloatList01
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
            public float Float02
            {
                get { return mFloat02; }
                set { mFloat02 = value; OnElementChanged(); }
            }

            [ElementPriority(7)]
            public FloatList FloatList02
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
            public FloatList FloatList03
            {
                get { return mFloatList03; }
                set { mFloatList03 = value; OnElementChanged(); }
            }

            [ElementPriority(10)]
            public FloatList FloatList04
            {
                get { return mFloatList04; }
                set { mFloatList04 = value; OnElementChanged(); }
            }

            [ElementPriority(11)]
            public FloatList FloatList05
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
            public ColorList ColorList01
            {
                get { return mColorList01; }
                set { mColorList01 = value; OnElementChanged(); }
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
            public FloatList FloatList06
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
            public ItemCList ItemCList01
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
            public Vector3ListLE Vector3List01
            {
                get { return mVector3List01; }
                set { mVector3List01 = value; OnElementChanged(); }
            }

            [ElementPriority(46)]
            public FloatList FloatList07
            {
                get { return mFloatList07; }
                set { mFloatList07 = value; OnElementChanged(); }
            }

            [ElementPriority(47)]
            public ItemBList ItemBList01
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
            public float Float37
            {
                get { return mFloat37; }
                set { mFloat37 = value; OnElementChanged(); }
            }

            [ElementPriority(61)]
            public float Float38
            {
                get { return mFloat38; }
                set { mFloat38 = value; OnElementChanged(); }
            }

            [ElementPriority(62)]
            public float Float39
            {
                get { return mFloat39; }
                set { mFloat39 = value; OnElementChanged(); }
            }

            [ElementPriority(63)]
            public float Float40
            {
                get { return mFloat40; }
                set { mFloat40 = value; OnElementChanged(); }
            }

            [ElementPriority(64)]
            public float Float41
            {
                get { return mFloat41; }
                set { mFloat41 = value; OnElementChanged(); }
            }

            [ElementPriority(65)]
            public float Float42
            {
                get { return mFloat42; }
                set { mFloat42 = value; OnElementChanged(); }
            }

            [ElementPriority(66)]
            public float Float43
            {
                get { return mFloat43; }
                set { mFloat43 = value; OnElementChanged(); }
            }

            [ElementPriority(67)]
            public FloatList FloatList08
            {
                get { return mFloatList08; }
                set { mFloatList08 = value; OnElementChanged(); }
            }

            [ElementPriority(68)]
            public byte Byte06
            {
                get { return mByte06; }
                set { mByte06 = value; OnElementChanged(); }
            }

            [ElementPriority(69)]
            public float Float44
            {
                get { return mFloat44; }
                set { mFloat44 = value; OnElementChanged(); }
            }

            [ElementPriority(70)]
            public float Float45
            {
                get { return mFloat45; }
                set { mFloat45 = value; OnElementChanged(); }
            }

            [ElementPriority(71)]
            public float Float46
            {
                get { return mFloat46; }
                set { mFloat46 = value; OnElementChanged(); }
            }

            [ElementPriority(72)]
            public float Float47
            {
                get { return mFloat47; }
                set { mFloat47 = value; OnElementChanged(); }
            }

            [ElementPriority(73)]
            public float Float48
            {
                get { return mFloat48; }
                set { mFloat48 = value; OnElementChanged(); }
            }

            [ElementPriority(74)]
            public float Float49
            {
                get { return mFloat49; }
                set { mFloat49 = value; OnElementChanged(); }
            }

            [ElementPriority(75)]
            public float Float50
            {
                get { return mFloat50; }
                set { mFloat50 = value; OnElementChanged(); }
            }

            [ElementPriority(76)]
            public FloatList FloatList09
            {
                get { return mFloatList09; }
                set { mFloatList09 = value; OnElementChanged(); }
            }

            [ElementPriority(77)]
            public byte Byte07
            {
                get { return mByte07; }
                set { mByte07 = value; OnElementChanged(); }
            }

            [ElementPriority(78)]
            public float Float51
            {
                get { return mFloat51; }
                set { mFloat51 = value; OnElementChanged(); }
            }

            [ElementPriority(79)]
            public float Float52
            {
                get { return mFloat52; }
                set { mFloat52 = value; OnElementChanged(); }
            }

            [ElementPriority(80)]
            public float Float53
            {
                get { return mFloat53; }
                set { mFloat53 = value; OnElementChanged(); }
            }

            [ElementPriority(81)]
            public float Float54
            {
                get { return mFloat54; }
                set { mFloat54 = value; OnElementChanged(); }
            }

            [ElementPriority(82)]
            public float Float55
            {
                get { return mFloat55; }
                set { mFloat55 = value; OnElementChanged(); }
            }

            [ElementPriority(83)]
            public float Float56
            {
                get { return mFloat56; }
                set { mFloat56 = value; OnElementChanged(); }
            }

            [ElementPriority(84)]
            public float Float57
            {
                get { return mFloat57; }
                set { mFloat57 = value; OnElementChanged(); }
            }

            [ElementPriority(85)]
            public float Float58
            {
                get { return mFloat58; }
                set { mFloat58 = value; OnElementChanged(); }
            }

            [ElementPriority(86)]
            public float Float59
            {
                get { return mFloat59; }
                set { mFloat59 = value; OnElementChanged(); }
            }

            [ElementPriority(87)]
            public FloatList FloatList10
            {
                get { return mFloatList10; }
                set { mFloatList10 = value; OnElementChanged(); }
            }

            [ElementPriority(88)]
            public float Float60
            {
                get { return mFloat60; }
                set { mFloat60 = value; OnElementChanged(); }
            }

            [ElementPriority(89)]
            public float Float61
            {
                get { return mFloat61; }
                set { mFloat61 = value; OnElementChanged(); }
            }

            [ElementPriority(90)]
            public ItemDList ItemDList01
            {
                get { return mItemDList01; }
                set { mItemDList01 = value; OnElementChanged(); }
            }

            [ElementPriority(91)]
            public float Float62
            {
                get { return mFloat62; }
                set { mFloat62 = value; OnElementChanged(); }
            }

            #endregion

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out  mInt01);
                s.Read(out  mInt02);
                mParticleParameters = new ParticleParams(0, handler, stream);
                mFloatList01 = new FloatList(handler, stream);
                s.Read(out mFloat01);
                s.Read(out mFloat02);
                mFloatList02 = new FloatList(handler, stream);
                s.Read(out mFloat03);
                mFloatList03 = new FloatList(handler, stream);
                mFloatList04 = new FloatList(handler, stream);
                mFloatList05 = new FloatList(handler, stream);
                s.Read(out mFloat04);
                s.Read(out mFloat05);
                s.Read(out mFloat06);
                s.Read(out mFloat07);
                s.Read(out mFloat08);
                s.Read(out mFloat09);
                mColorList01 = new ColorList(handler, stream);
                s.Read(out mFloat10, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat11, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat12, ByteOrder.LittleEndian); //LE
                mFloatList06 = new FloatList(handler, stream);
                s.Read(out mFloat13);
                s.Read(out  mBaseEffectName, StringType.ZeroDelimited);
                s.Read(out  mDeathEffectName, StringType.ZeroDelimited);
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
                mItemCList01 = new ItemCList(handler, stream);
                s.Read(out mByte02);
                s.Read(out mByte03);
                s.Read(out mByte04);
                s.Read(out mByte05);
                mVector3List01 = new Vector3ListLE(handler, stream);
                FloatList07 = new FloatList(handler, stream);
                mItemBList01 = new ItemBList(handler, stream);
                s.Read(out mFloat28);
                s.Read(out mFloat29);
                s.Read(out mFloat30);
                s.Read(out mFloat31);
                s.Read(out mFloat32);
                s.Read(out mFloat33);
                s.Read(out mFloat34);

                s.Read(out mFloat35, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat36, ByteOrder.LittleEndian); //LE

                s.Read(out  mLong01);
                s.Read(out  mLong02);
                s.Read(out  mLong03);

                s.Read(out mFloat37, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat38, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat39, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat40, ByteOrder.LittleEndian); //LE

                s.Read(out mFloat41);
                s.Read(out mFloat42);
                s.Read(out mFloat43);
                mFloatList08 = new FloatList(handler, stream);
                s.Read(out mByte06);

                s.Read(out mFloat44, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat45, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat46, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat47, ByteOrder.LittleEndian); //LE

                s.Read(out mFloat48);
                s.Read(out mFloat49);
                s.Read(out mFloat50);
                mFloatList09 = new FloatList(handler, stream);
                s.Read(out mByte07);

                s.Read(out mFloat51, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat52, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat53, ByteOrder.LittleEndian); //LE

                s.Read(out mFloat54);
                s.Read(out mFloat55);
                s.Read(out mFloat56);

                s.Read(out mFloat57, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat58, ByteOrder.LittleEndian); //LE
                s.Read(out mFloat59, ByteOrder.LittleEndian); //LE

                mFloatList10 = new FloatList(handler, stream);
                s.Read(out mFloat60);
                s.Read(out mFloat61);
                mItemDList01 = new ItemDList(handler, stream);
                s.Read(out mFloat62);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                
            s.Write( mInt01);
                s.Write( mInt02);
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
                mColorList01.UnParse(stream);
                s.Write(mFloat10, ByteOrder.LittleEndian); //LE
                s.Write(mFloat11, ByteOrder.LittleEndian); //LE
                s.Write(mFloat12, ByteOrder.LittleEndian); //LE
                mFloatList06.UnParse(stream);
                s.Write(mFloat13);
                s.Write( mBaseEffectName, StringType.ZeroDelimited);
                s.Write( mDeathEffectName, StringType.ZeroDelimited);
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

                s.Write( mLong01);
                s.Write( mLong02);
                s.Write( mLong03);

                s.Write(mFloat37, ByteOrder.LittleEndian); //LE
                s.Write(mFloat38, ByteOrder.LittleEndian); //LE
                s.Write(mFloat39, ByteOrder.LittleEndian); //LE
                s.Write(mFloat40, ByteOrder.LittleEndian); //LE

                s.Write(mFloat41);
                s.Write(mFloat42);
                s.Write(mFloat43);
                mFloatList08.UnParse(stream);
                s.Write(mByte06);

                s.Write(mFloat44, ByteOrder.LittleEndian); //LE
                s.Write(mFloat45, ByteOrder.LittleEndian); //LE
                s.Write(mFloat46, ByteOrder.LittleEndian); //LE
                s.Write(mFloat47, ByteOrder.LittleEndian); //LE

                s.Write(mFloat48);
                s.Write(mFloat49);
                s.Write(mFloat50);
                mFloatList09.UnParse(stream);
                s.Write(mByte07);

                s.Write(mFloat51, ByteOrder.LittleEndian); //LE
                s.Write(mFloat52, ByteOrder.LittleEndian); //LE
                s.Write(mFloat53, ByteOrder.LittleEndian); //LE

                s.Write(mFloat54);
                s.Write(mFloat55);
                s.Write(mFloat56);

                s.Write(mFloat57, ByteOrder.LittleEndian); //LE
                s.Write(mFloat58, ByteOrder.LittleEndian); //LE
                s.Write(mFloat59, ByteOrder.LittleEndian); //LE

                mFloatList10.UnParse(stream);
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
        #endregion

        #region Nested Type: DecalEffect
        public class DecalEffect : AbstractEffect, IEquatable<DecalEffect>
        {
            public DecalEffect(int apiVersion, EventHandler handler, DecalEffect basis)
                : base(apiVersion, handler, basis)
            {
            }
            public DecalEffect(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mFloatList01 = new FloatList(handler);
                mFloatList02 = new FloatList(handler);
                mFloatList03 = new FloatList(handler);
                mVector3List01 = new Vector3ListLE(handler);
                mVector3List02 = new Vector3List(handler);
                mByteArray01 = new byte[6];
                mByteArray02 = new byte[4];
                mByteArray03 = new byte[8];
                mByteArray04 = new byte[8] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            }
            public DecalEffect(int apiVersion, EventHandler handler, Stream s, ushort version) : base(apiVersion, handler, s, version) { }


            #region Fields

            private UInt32 mUint01;
            private UInt64 mDdsResource;
            private UInt16 mInt01;
            private UInt32 mInt02;
            private byte[] mByteArray01;
            private float mFloat01;
            private FloatList mFloatList01;
            private FloatList mFloatList02;
            private FloatList mFloatList03;
            private Vector3ListLE mVector3List01;
            private Vector3List mVector3List02;
            private byte[] mByteArray02;
            private float mFloat02;
            private byte[] mByteArray03;
            private byte[] mByteArray04;
            private byte mByte03;

            #endregion

            #region Properties
            [ElementPriority(1)]
            public UInt32 Uint01
            {
                get { return mUint01; }
                set { mUint01 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public ulong Decal
            {
                get { return mDdsResource; }
                set { mDdsResource = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public UInt16 Int01
            {
                get { return mInt01; }
                set { mInt01 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public UInt32 Int02
            {
                get { return mInt02; }
                set { mInt02 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public float Float01
            {
                get { return mFloat01; }
                set { mFloat01 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public FloatList FloatList01
            {
                get { return mFloatList01; }
                set { mFloatList01 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public FloatList FloatList02
            {
                get { return mFloatList02; }
                set { mFloatList02 = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public FloatList FloatList03
            {
                get { return mFloatList03; }
                set { mFloatList03 = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public Vector3ListLE MapEmitColor
            {
                get { return mVector3List01; }
                set { mVector3List01 = value; OnElementChanged(); }
            }
            [ElementPriority(10)]
            public Vector3List Vector3List02
            {
                get { return mVector3List02; }
                set { mVector3List02 = value; OnElementChanged(); }
            }
            [ElementPriority(11)]
            public byte[] ByteArray02
            {
                get { return mByteArray02; }
                set { mByteArray02 = value; OnElementChanged(); }
            }
            [ElementPriority(12)]
            public float Float02
            {
                get { return mFloat02; }
                set { mFloat02 = value; OnElementChanged(); }
            }
            [ElementPriority(13)]
            public byte[] ByteArray03
            {
                get { return mByteArray03; }
                set { mByteArray03 = value; OnElementChanged(); }
            }
            [ElementPriority(14)]
            public byte[] ByteArray04
            {
                get { return mByteArray04; }
                set { mByteArray04 = value; OnElementChanged(); }
            }
            [ElementPriority(15)]
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
                s.Read(out mInt01);
                s.Read(out mInt02);
                s.Read(out mFloat01);

                mFloatList01 = new FloatList(handler, stream);

                mFloatList02 = new FloatList(handler, stream);

                mFloatList03 = new FloatList(handler, stream);

                mVector3List01 = new Vector3ListLE(handler, stream);

                mVector3List02 = new Vector3List(handler, stream);

                s.Read(out mByteArray02, 4);
                s.Read(out mFloat02);
                s.Read(out mByteArray03, 8);
                s.Read(out mByteArray04, 8);
                s.Read(out mByte03);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mUint01);
                s.Write(mDdsResource);
                s.Write(mInt01);
                s.Write(mInt02);
                s.Write(mFloat01);

                mFloatList01.UnParse(stream);

                mFloatList02.UnParse(stream);

                mFloatList03.UnParse(stream);

                mVector3List01.UnParse(stream);

                mVector3List02.UnParse(stream);

                s.Write(mByteArray02);
                s.Write(mFloat02);
                s.Write(mByteArray03);
                s.Write(mByteArray04);
                s.Write(mByte03);
            }


            public bool Equals(DecalEffect other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        #region Nested Type: SequenceEffect
        public class SequenceEffect : AbstractEffect, IEquatable<SequenceEffect>
        {
            public SequenceEffect(int apiVersion, EventHandler handler, SequenceEffect basis)
                : base(apiVersion, handler, basis)
            {
            }
            public SequenceEffect(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mElements = new ElementList(handler);
            }
            public SequenceEffect(int apiVersion, EventHandler handler, Stream s, ushort version) : base(apiVersion, handler, s, version) { }
            public class ElementList : AResource.DependentList<SequenceElement>
            {
                public ElementList(EventHandler handler) : base(handler) { }
                public ElementList(EventHandler handler, Stream s) : base(handler, s) { }

                protected override uint ReadCount(Stream s)
                {
                    return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
                }
                protected override void WriteCount(Stream s, uint count)
                {
                    new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
                }
                public override void Add()
                {
                    base.Add(new object[0] { });
                }

                protected override SequenceElement CreateElement(Stream s)
                {
                    return new SequenceElement(0, elementHandler, s);
                }

                protected override void WriteElement(Stream s, SequenceElement element)
                {
                    element.UnParse(s);
                }
            }

            #region Nested Type: Sequence Element

            public class SequenceElement : AHandlerElement, IEquatable<SequenceElement>
            {
                public SequenceElement(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
                public SequenceElement(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
                private float mFloat01 = 1.0f;
                private float mFloat02 = 1.0f;
                private string mString01 = string.Empty;
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
                public string EffectName
                {
                    get { return mString01; }
                    set { mString01 = value; OnElementChanged(); }
                }

                protected void Parse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    s.Read(out mFloat01, ByteOrder.LittleEndian);
                    s.Read(out mFloat02, ByteOrder.LittleEndian);
                    s.Read(out mString01, StringType.ZeroDelimited);
                }


                public void UnParse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    s.Write(mFloat01, ByteOrder.LittleEndian);
                    s.Write(mFloat02, ByteOrder.LittleEndian);
                    s.Write(mString01, StringType.ZeroDelimited);
                }

                public bool Equals(SequenceElement other)
                {
                    return base.Equals(other);
                }

                public override AHandlerElement Clone(EventHandler handler)
                {
                    throw new NotImplementedException();
                }

                public override System.Collections.Generic.List<string> ContentFields
                {
                    get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
                }

                public override int RecommendedApiVersion
                {
                    get { return kRecommendedAPIVersion; }
                }
            }

            #endregion

            #region Fields

            private ElementList mElements;
            private UInt32 mInt01;

            #endregion

            #region Properties
            [ElementPriority(1)]
            public ElementList Elements
            {
                get { return mElements; }
                set { mElements = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public uint Int01
            {
                get { return mInt01; }
                set { mInt01 = value; OnElementChanged(); }
            }

            #endregion

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mElements = new ElementList(handler, stream);
                s.Read(out mInt01);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mElements.UnParse(stream);
                s.Write(mInt01);
            }


            public bool Equals(SequenceEffect other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        #region Nested Type: SoundEffect
        public class SoundEffect : AbstractEffect, IEquatable<SoundEffect>
        {
            public SoundEffect(int apiVersion, EventHandler handler, SoundEffect basis)
                : base(apiVersion, handler, basis)
            {
            }
            public SoundEffect(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public SoundEffect(int apiVersion, EventHandler handler, Stream s, ushort version) : base(apiVersion, handler, s, version) { }


            private UInt32 mUint01;
            private UInt64 mLong01;
            private float mFloat01 = 0.25f;
            private float mFloat02;
            private float mFloat03;
            [ElementPriority(1)]
            public uint Uint01
            {
                get { return mUint01; }
                set { mUint01 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public ulong AudioResourceInstance
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

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mUint01);
                s.Read(out mLong01);
                s.Read(out mFloat01);
                s.Read(out mFloat02);
                s.Read(out mFloat03);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mUint01);
                s.Write(mLong01);
                s.Write(mFloat01);
                s.Write(mFloat02);
                s.Write(mFloat03);
            }


            public bool Equals(SoundEffect other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        #region Nested Type: ShakeEffect
        public class ShakeEffect : AbstractEffect, IEquatable<ShakeEffect>
        {
            public ShakeEffect(int apiVersion, EventHandler handler, ShakeEffect basis)
                : base(apiVersion, handler, basis)
            {
            }
            public ShakeEffect(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mFloatList01 = new FloatList(handler);
                mFloatList02 = new FloatList(handler);
            }
            public ShakeEffect(int apiVersion, EventHandler handler, Stream s, ushort version) : base(apiVersion, handler, s, version) { }


            private float mFloat01;
            private float mFloat02;
            private FloatList mFloatList01;
            private FloatList mFloatList02;
            private float mFloat03;
            private byte mByte01;
            private float mFloat04;
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
            public FloatList FloatList01
            {
                get { return mFloatList01; }
                set { mFloatList01 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public FloatList FloatList02
            {
                get { return mFloatList02; }
                set { mFloatList02 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public float Float03
            {
                get { return mFloat03; }
                set { mFloat03 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public byte Byte01
            {
                get { return mByte01; }
                set { mByte01 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public float Float04
            {
                get { return mFloat04; }
                set { mFloat04 = value; OnElementChanged(); }
            }

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mFloat01);
                s.Read(out mFloat02);

                mFloatList01 = new FloatList(handler, stream);

                mFloatList02 = new FloatList(handler, stream);


                s.Read(out mFloat03);
                s.Read(out mByte01);
                s.Read(out mFloat04);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mFloat01);
                s.Write(mFloat02);

                mFloatList01.UnParse(stream);

                mFloatList02.UnParse(stream);


                s.Write(mFloat03);
                s.Write(mByte01);
                s.Write(mFloat04);
            }


            public bool Equals(ShakeEffect other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        #region Nested Type: Camera Effect
        public class CameraEffect : AbstractEffect, IEquatable<CameraEffect>
        {
            public CameraEffect(int apiVersion, EventHandler handler, CameraEffect basis)
                : base(apiVersion, handler, basis)
            {
            }
            public CameraEffect(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mFloatList01 = new FloatList(handler);
                mFloatList02 = new FloatList(handler);
                mFloatList03 = new FloatList(handler);
                mFloatList04 = new FloatList(handler);
                mByteArray01 = new Byte[17];
            }
            public CameraEffect(int apiVersion, EventHandler handler, Stream s, ushort version) : base(apiVersion, handler, s, version) { }

            private UInt32 mInt01;
            private UInt16 mShort01;
            private float mFloat01;
            private FloatList mFloatList01;
            private FloatList mFloatList02;
            private FloatList mFloatList03;
            private FloatList mFloatList04;
            private byte[] mByteArray01;
            private UInt32 mInt02;
            private byte mByte01;
            [ElementPriority(1)]
            public uint Int01
            {
                get { return mInt01; }
                set { mInt01 = value; }
            }
            [ElementPriority(2)]
            public ushort Short01
            {
                get { return mShort01; }
                set { mShort01 = value; }
            }
            [ElementPriority(3)]
            public float Float01
            {
                get { return mFloat01; }
                set { mFloat01 = value; }
            }
            [ElementPriority(4)]
            public FloatList FloatList01
            {
                get { return mFloatList01; }
                set { mFloatList01 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public FloatList FloatList02
            {
                get { return mFloatList02; }
                set { mFloatList02 = value; }
            }
            [ElementPriority(6)]
            public FloatList FloatList03
            {
                get { return mFloatList03; }
                set { mFloatList03 = value; }
            }
            [ElementPriority(7)]
            public FloatList FloatList04
            {
                get { return mFloatList04; }
                set { mFloatList04 = value; }
            }
            [ElementPriority(8)]
            public byte[] ByteArray01
            {
                get { return mByteArray01; }
                set { mByteArray01 = value; }
            }
            [ElementPriority(9)]
            public uint Int02
            {
                get { return mInt02; }
                set { mInt02 = value; }
            }
            [ElementPriority(10)]
            public byte Byte01
            {
                get { return mByte01; }
                set { mByte01 = value; }
            }

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mInt01);
                s.Read(out mShort01);
                s.Read(out mFloat01);

                mFloatList01 = new FloatList(handler, stream);

                mFloatList02 = new FloatList(handler, stream);

                mFloatList03 = new FloatList(handler, stream);

                mFloatList04 = new FloatList(handler, stream);

                s.Read(out mByteArray01, 17);
                s.Read(out mInt02);
                s.Read(out mByte01);
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

                s.Write(mByteArray01);
                s.Write(mInt02);
                s.Write(mByte01);
            }

            public bool Equals(CameraEffect other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        #region Nested Type: ModelEffect
        public class ModelEffect : AbstractEffect, IEquatable<ModelEffect>
        {
            public ModelEffect(int apiVersion, EventHandler handler, ModelEffect basis)
                : base(apiVersion, handler, basis)
            {
            }
            public ModelEffect(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mByteArray01 = new byte[12];
                mByteArray02 = new byte[4];
                mByteArray03 = new byte[8];
            }
            public ModelEffect(int apiVersion, EventHandler handler, Stream s, ushort version) : base(apiVersion, handler, s, version) { }


            private byte[] mByteArray01;
            private float mFloat01;
            private float mFloat02;
            private float mFloat03;
            private float mFloat04;
            private float mFloat05;
            private byte[] mByteArray02;
            private byte[] mByteArray03;
            private byte mByte01;
            [ElementPriority(1)]
            public byte[] ByteArray01
            {
                get { return mByteArray01; }
                set { mByteArray01 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Float01
            {
                get { return mFloat01; }
                set { mFloat01 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public float Float02
            {
                get { return mFloat02; }
                set { mFloat02 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public float Float03
            {
                get { return mFloat03; }
                set { mFloat03 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public float Float04
            {
                get { return mFloat04; }
                set { mFloat04 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public float Float05
            {
                get { return mFloat05; }
                set { mFloat05 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public byte[] ByteArray02
            {
                get { return mByteArray02; }
                set { mByteArray02 = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public byte[] ByteArray03
            {
                get { return mByteArray03; }
                set { mByteArray03 = value; OnElementChanged(); }
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
                s.Read(out mByteArray01, 12);
                s.Read(out mFloat01);
                s.Read(out mFloat02, ByteOrder.LittleEndian);
                s.Read(out mFloat03, ByteOrder.LittleEndian);
                s.Read(out mFloat04, ByteOrder.LittleEndian);
                s.Read(out mFloat05);
                s.Read(out mByteArray02, 4);
                s.Read(out mByteArray03, 8);
                s.Read(out mByte01);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mByteArray01);
                s.Write(mFloat01);
                s.Write(mFloat02, ByteOrder.LittleEndian);
                s.Write(mFloat03, ByteOrder.LittleEndian);
                s.Write(mFloat04, ByteOrder.LittleEndian);
                s.Write(mFloat05);
                s.Write(mByteArray02);
                s.Write(mByteArray03);
                s.Write(mByte01);
            }


            public bool Equals(ModelEffect other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        #region Nested Type: ScreenEffect
        public class ScreenEffect : AbstractEffect, IEquatable<ScreenEffect>
        {
            public ScreenEffect(int apiVersion, EventHandler handler, ScreenEffect basis)
                : base(apiVersion, handler, basis)
            {
            }
            public ScreenEffect(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mVector3List01 = new Vector3ListLE(handler);
                mVector3List02 = new Vector3List(handler);
            }
            public ScreenEffect(int apiVersion, EventHandler handler, Stream s, ushort version) : base(apiVersion, handler, s, version) { }


            private byte mByte01;
            private UInt32 mInt01;
            private Vector3ListLE mVector3List01;
            private Vector3List mVector3List02;
            private byte[] mByteArray01 = new byte[4];
            private UInt64 mLong01 = 0xFFFFFFFFFFFFFFFFUL;
            private byte[] mByteArray02 = new byte[21];
            private byte[] mByteArray03 = new byte[3];
            [ElementPriority(1)]
            public byte Byte01
            {
                get { return mByte01; }
                set { mByte01 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public uint Int01
            {
                get { return mInt01; }
                set { mInt01 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Vector3ListLE Vector3List01
            {
                get { return mVector3List01; }
                set { mVector3List01 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public Vector3List Vector3List02
            {
                get { return mVector3List02; }
                set { mVector3List02 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public byte[] ByteArray01
            {
                get { return mByteArray01; }
                set { mByteArray01 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public UInt64 Long01
            {
                get { return mLong01; }
                set { mLong01 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public byte[] ByteArray02
            {
                get { return mByteArray02; }
                set { mByteArray02 = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public byte[] ByteArray03
            {
                get { return mByteArray03; }
                set { mByteArray03 = value; OnElementChanged(); }
            }

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                int count;
                s.Read(out mByte01);
                s.Read(out mInt01);

                mVector3List01 = new Vector3ListLE(handler, stream);

                mVector3List02 = new Vector3List(handler, stream);

                count = Byte01 == 0 ? 4 : 12;
                s.Read(out mByteArray01, count);
                s.Read(out mLong01);
                s.Read(out mByteArray02, 21);
                s.Read(out mByteArray03, 3);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mByte01);
                s.Write(mInt01);

                mVector3List01.UnParse(stream);

                mVector3List02.UnParse(stream);

                s.Write(mByteArray01);
                s.Write(mLong01);
                s.Write(mByteArray02);
                s.Write(mByteArray03);
            }

            public bool Equals(ScreenEffect other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        #region Nested Type: Distribute Effect
        public class DistributeEffect : AbstractEffect, IEquatable<DistributeEffect>
        {
            public DistributeEffect(int apiVersion, EventHandler handler, DistributeEffect basis)
                : base(apiVersion, handler, basis)
            {
            }
            public DistributeEffect(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mByteArray01 = new byte[5];
                mByteArray02 = new byte[2];
                mByteArray03 = new byte[84];
                mByteArray04 = new byte[24];
                mByteArray05 = new byte[8];
                mByteArray06 = new byte[14];
                mByteArray07 = new byte[8];
                mByteArray08 = new byte[5];
            }
            public DistributeEffect(int apiVersion, EventHandler handler, Stream s, ushort version) : base(apiVersion, handler, s, version) { }


            private UInt32 mInt01;
            private UInt32 mInt02;
            private String mString01;
            private byte[] mByteArray01;
            private float mFloat01;
            private byte[] mByteArray02;
            private float mFloat02;
            private float mFloat03;
            private float mFloat04;
            private float mFloat05;
            private float mFloat06;
            private float mFloat07;
            private float mFloat08;
            private float mFloat09;
            private float mFloat10;
            private byte[] mByteArray03;
            private byte[] mByteArray04;
            private float mFloat11;
            private float mFloat12;
            private float mFloat13;
            private byte[] mByteArray05;
            private byte[] mByteArray06;
            private byte[] mByteArray07;
            private byte[] mByteArray08;
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
            public byte[] ByteArray01
            {
                get { return mByteArray01; }
                set { mByteArray01 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public float Float01
            {
                get { return mFloat01; }
                set { mFloat01 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public byte[] ByteArray02
            {
                get { return mByteArray02; }
                set { mByteArray02 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public float Float02
            {
                get { return mFloat02; }
                set { mFloat02 = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public float Float03
            {
                get { return mFloat03; }
                set { mFloat03 = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public float Float04
            {
                get { return mFloat04; }
                set { mFloat04 = value; OnElementChanged(); }
            }
            [ElementPriority(10)]
            public float Float05
            {
                get { return mFloat05; }
                set { mFloat05 = value; OnElementChanged(); }
            }
            [ElementPriority(11)]
            public float Float06
            {
                get { return mFloat06; }
                set { mFloat06 = value; OnElementChanged(); }
            }
            [ElementPriority(12)]
            public float Float07
            {
                get { return mFloat07; }
                set { mFloat07 = value; OnElementChanged(); }
            }
            [ElementPriority(13)]
            public float Float08
            {
                get { return mFloat08; }
                set { mFloat08 = value; OnElementChanged(); }
            }
            [ElementPriority(14)]
            public float Float09
            {
                get { return mFloat09; }
                set { mFloat09 = value; OnElementChanged(); }
            }
            [ElementPriority(15)]
            public float Float10
            {
                get { return mFloat10; }
                set { mFloat10 = value; OnElementChanged(); }
            }
            [ElementPriority(16)]
            public float Float11
            {
                get { return mFloat11; }
                set { mFloat11 = value; OnElementChanged(); }
            }
            [ElementPriority(17)]
            public byte[] ByteArray03
            {
                get { return mByteArray03; }
                set { mByteArray03 = value; OnElementChanged(); }
            }
            [ElementPriority(18)]
            public byte[] ByteArray04
            {
                get { return mByteArray04; }
                set { mByteArray04 = value; OnElementChanged(); }
            }
            [ElementPriority(19)]
            public float Float12
            {
                get { return mFloat12; }
                set { mFloat12 = value; OnElementChanged(); }
            }
            [ElementPriority(20)]
            public float Float13
            {
                get { return mFloat13; }
                set { mFloat13 = value; OnElementChanged(); }
            }
            [ElementPriority(21)]
            public byte[] ByteArray05
            {
                get { return mByteArray05; }
                set { mByteArray05 = value; OnElementChanged(); }
            }
            [ElementPriority(22)]
            public byte[] ByteArray06
            {
                get { return mByteArray06; }
                set { mByteArray06 = value; OnElementChanged(); }
            }
            [ElementPriority(23)]
            public byte[] ByteArray07
            {
                get { return mByteArray07; }
                set { mByteArray07 = value; OnElementChanged(); }
            }
            [ElementPriority(24)]
            public byte[] ByteArray08
            {
                get { return mByteArray08; }
                set { mByteArray08 = value; OnElementChanged(); }
            }

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mInt01);
                s.Read(out mInt02);
                s.Read(out mString01, StringType.ZeroDelimited);
                s.Read(out mByteArray01, 5);
                s.Read(out mFloat01);
                s.Read(out mByteArray02, 2);
                s.Read(out mFloat02);
                s.Read(out mFloat03, ByteOrder.LittleEndian);
                s.Read(out mFloat04, ByteOrder.LittleEndian);
                s.Read(out mFloat05, ByteOrder.LittleEndian);
                s.Read(out mFloat06, ByteOrder.LittleEndian);
                s.Read(out mFloat07, ByteOrder.LittleEndian);
                s.Read(out mFloat08, ByteOrder.LittleEndian);
                s.Read(out mFloat09, ByteOrder.LittleEndian);
                s.Read(out mFloat10, ByteOrder.LittleEndian);
                s.Read(out mFloat11, ByteOrder.LittleEndian);
                s.Read(out mByteArray03, 84);
                s.Read(out mByteArray04, 24);
                s.Read(out mFloat12, ByteOrder.LittleEndian);
                s.Read(out mFloat13, ByteOrder.LittleEndian);
                s.Read(out mByteArray05, 8);
                s.Read(out mByteArray06, 14);
                s.Read(out mByteArray07, 8);
                s.Read(out mByteArray08, 5);
            }

            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mInt01);
                s.Write(mInt02);
                s.Write(mString01, StringType.ZeroDelimited);
                s.Write(mByteArray01);
                s.Write(mFloat01);
                s.Write(mByteArray02);
                s.Write(mFloat02);
                s.Write(mFloat03, ByteOrder.LittleEndian);
                s.Write(mFloat04, ByteOrder.LittleEndian);
                s.Write(mFloat05, ByteOrder.LittleEndian);
                s.Write(mFloat06, ByteOrder.LittleEndian);
                s.Write(mFloat07, ByteOrder.LittleEndian);
                s.Write(mFloat08, ByteOrder.LittleEndian);
                s.Write(mFloat09, ByteOrder.LittleEndian);
                s.Write(mFloat10, ByteOrder.LittleEndian);
                s.Write(mFloat11, ByteOrder.LittleEndian);
                s.Write(mByteArray03);
                s.Write(mByteArray04);
                s.Write(mFloat12, ByteOrder.LittleEndian);
                s.Write(mFloat13, ByteOrder.LittleEndian);
                s.Write(mByteArray05);
                s.Write(mByteArray06);
                s.Write(mByteArray07);
                s.Write(mByteArray08);
            }

            public bool Equals(DistributeEffect other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        #endregion

        #region Nested Type: Material
        public class Material : AHandlerElement, IEquatable<Material>
        {
            public enum ShaderType : ulong
            {
                FluidEffect = 0x0000000008FAB3764UL,
                SimCensor = 0x000000004968A478UL
            }

            public enum PropertyKey : ulong
            {
                DiffuseMap = 0x000000006CC0FD85UL,
                NormalMap = 0x000000006E56548AUL,
                EdgeDarkening = 0x000000008C27D8C9UL,
                RefractionDistortionScale = 0x00000000C3C472A1UL,
                ClipAlphaOpacity = 0x000000006A203374UL,
                AlphaCutoff = 0x00000000556010DCUL,
                SpecularScale = 0x00000000F2FCAD8CUL,
                MultiplyValue = 0x00000000F43D2BDCUL,
                AdditiveValue = 0x000000003965ECE0UL
            }

            public enum ValueType : byte
            {
                Float = 0x00,
                ResourceKey = 0x06
            }

            #region Nested Type: PropertyList

            public class PropertyList : AResource.DependentList<Material.Property>
            {
                public PropertyList(EventHandler handler) : base(handler) { }
                public PropertyList(EventHandler handler, Stream s) : base(handler, s) { }
                protected override uint ReadCount(Stream s)
                {
                    uint count = new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
                    return count;
                }
                protected override void WriteCount(Stream s, uint count)
                {
                    new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
                }
                public override void Add()
                {
                    base.Add(new object[0] { });
                }

                protected override Material.Property CreateElement(System.IO.Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    PropertyKey key = (PropertyKey)s.ReadUInt64();
                    ValueType val = (ValueType)s.BaseStream.ReadByte();
                    return Property.CreateInstance(0, elementHandler, val, key, stream);
                }

                protected override void WriteElement(System.IO.Stream stream, Material.Property element)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    s.Write((UInt64)element.Key);
                    s.Write((byte)element.ValueType);
                    element.UnParse(stream);
                }
                protected override Type GetElementType(params object[] fields)
                {
                    if (fields.Length == 0) return null;
                    Property p = fields[0] as Property;
                    if (p != null) return p.GetType();

                    ValueType val = (ValueType)fields[0];
                    switch (val)
                    {
                        case ValueType.Float:
                            return typeof(FloatProperty);
                        case ValueType.ResourceKey:
                            return typeof(ResourceKeyProperty);
                        default:
                            throw new Exception("Unknown Property type!");
                    }
                }
            }
            #endregion

            #region Nested Type: Property

            public abstract class Property : AHandlerElement, IEquatable<Property>, IComparable<Property>
            {
                protected Property(int apiVersion, EventHandler handler, Property basis)
                    : base(apiVersion, handler)
                {
                    mKey = basis.Key;
                    mValType = basis.ValueType;
                    MemoryStream ms = new MemoryStream();
                    basis.UnParse(ms);
                    ms.Position = 0L;
                    Parse(ms);
                }
                protected Property(int apiVersion, EventHandler handler, ValueType type, PropertyKey key)
                    : base(apiVersion, handler)
                {
                    mKey = key;
                    mValType = type;
                }
                protected Property(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s)
                    : base(apiVersion, handler)
                {
                    mKey = key;
                    mValType = type;
                    if (s != null) Parse(s);
                }

                private PropertyKey mKey;
                private ValueType mValType;
                private object mValue = 0;
                [ElementPriority(0)]
                public PropertyKey Key
                {
                    get { return mKey; }
                    set { mKey = value; OnElementChanged(); }
                }
                [ElementPriority(1)]
                public ValueType ValueType
                {
                    get { return mValType; }
                }
                public static Property CreateInstance(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s)
                {
                    switch (type)
                    {
                        case ValueType.Float:
                            return new FloatProperty(apiVersion, handler, type, key, s);
                        case ValueType.ResourceKey:
                            return new ResourceKeyProperty(apiVersion, handler, type, key, s);
                        default:
                            throw new Exception("Unknown Property type!");
                    }

                }

                protected abstract void Parse(Stream s);
                public abstract void UnParse(Stream s);

            #endregion

                public override string ToString()
                {
                    return mKey.ToString();
                }

                public bool Equals(Property other)
                {
                    return mKey.Equals(other.mKey);
                }

                public override AHandlerElement Clone(EventHandler handler)
                {
                    return (AHandlerElement)MemberwiseClone();
                }

                public override System.Collections.Generic.List<string> ContentFields
                {
                    get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
                }

                public override int RecommendedApiVersion
                {
                    get { return kRecommendedAPIVersion; }
                }

                public int CompareTo(Property other)
                {
                    return mKey.CompareTo(other.mKey);
                }
            }
            [ConstructorParameters(new object[] { ValueType.Float, PropertyKey.AdditiveValue })]
            public class FloatProperty : Property
            {
                public FloatProperty(int apiVersion, EventHandler handler, FloatProperty basis) : base(apiVersion, handler, basis) { }
                public FloatProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key) : base(apiVersion, handler, type, key) { }
                public FloatProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s) : base(apiVersion, handler, type, key, s) { }
                private float mValue;
                [ElementPriority(2)]
                public float Data
                {
                    get { return mValue; }
                    set { mValue = value; OnElementChanged(); }
                }
                protected override void Parse(Stream s)
                {
                    mValue = new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadFloat();
                }

                public override void UnParse(Stream s)
                {
                    new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write(mValue);
                }
            }
            [ConstructorParameters(new object[] { ValueType.ResourceKey, PropertyKey.DiffuseMap })]
            public class ResourceKeyProperty : Property
            {
                public ResourceKeyProperty(int apiVersion, EventHandler handler, ResourceKeyProperty basis) : base(apiVersion, handler, basis) { }
                public ResourceKeyProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key) : base(apiVersion, handler, type, key) { }
                public ResourceKeyProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s) : base(apiVersion, handler, type, key, s) { }
                private UInt64 mValue;
                [ElementPriority(2)]
                public UInt64 Data
                {
                    get { return mValue; }
                    set { mValue = value; OnElementChanged(); }
                }
                protected override void Parse(Stream s)
                {
                    mValue = new BinaryStreamWrapper(s, ByteOrder.LittleEndian).ReadUInt64();
                }

                public override void UnParse(Stream s)
                {
                    new BinaryStreamWrapper(s, ByteOrder.LittleEndian).Write(mValue);
                }
            }

            private ulong mHashedName;
            private ShaderType mType = ShaderType.FluidEffect;
            private PropertyList mProperties;

            public override string ToString()
            {
                return String.Format("Shader(0x{0:X16})", HashedName);
            }
            public Material(int apiVersion, EventHandler handler, Material basis)
                : base(apiVersion, handler)
            {
                MemoryStream ms = new MemoryStream();
                basis.UnParse(ms);
                ms.Position = 0L;
                Parse(ms);
            }
            public Material(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mProperties = new PropertyList(handler);
            }
            public Material(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }

            [ElementPriority(0)]
            public virtual BinaryReader Data
            {
                get
                {
                    MemoryStream s = new MemoryStream();
                    UnParse(s);
                    s.Position = 0L;
                    return new BinaryReader(s);
                }
                set
                {
                    if (value.BaseStream.CanSeek)
                    {
                        value.BaseStream.Position = 0L;
                        this.Parse(value.BaseStream);
                    }
                    else
                    {
                        MemoryStream s = new MemoryStream();
                        byte[] buffer = new byte[0x100000];
                        for (int i = value.BaseStream.Read(buffer, 0, buffer.Length); i > 0; i = value.BaseStream.Read(buffer, 0, buffer.Length))
                        {
                            s.Write(buffer, 0, i);
                        }
                        this.Parse(s);
                    }
                    OnElementChanged();
                }
            }
            [ElementPriority(1)]
            public ulong HashedName
            {
                get { return mHashedName; }
                set { mHashedName = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public ShaderType Type
            {
                get { return mType; }
                set { mType = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public PropertyList Properties
            {
                get { return mProperties; }
                set { mProperties = value; OnElementChanged(); }
            }


            protected void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                HashedName = s.ReadUInt64();
                Type = (ShaderType)s.ReadUInt64();

                mProperties = new PropertyList(handler, stream);
            }

            public void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(HashedName);
                s.Write((UInt64)Type);
                mProperties.UnParse(stream);
            }


            public override AHandlerElement Clone(EventHandler handler)
            {
                return (AHandlerElement)MemberwiseClone();
            }

            public override System.Collections.Generic.List<string> ContentFields
            {
                get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedAPIVersion; }
            }

            public bool Equals(Material other)
            {
                return mHashedName.Equals(other.mHashedName);
            }

        }
        #endregion

        #region Nested Type: MaterialList
        public class MaterialList : AResource.DependentList<Material>
        {
            public MaterialList(EventHandler handler) : base(handler) { }
            public MaterialList(EventHandler handler, Stream s) : base(handler, s) { }
            protected override uint ReadCount(Stream s)
            {
                uint count = new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
                return count;
            }
            protected override void WriteCount(Stream s, uint count)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
            }

            public override void Add()
            {
                base.Add(new object[0] { });
            }

            protected override Material CreateElement(Stream s)
            {
                return new Material(0, elementHandler, s);
            }

            protected override void WriteElement(Stream s, Material element)
            {
                element.UnParse(s);
            }
        }
        #endregion

        #region Nested Type: Compilation
        public class Compilation : AHandlerElement, IEquatable<Compilation>
        {
            public Compilation(int apiVersion, EventHandler handler, Compilation basis)
                : base(apiVersion, handler)
            {
                MemoryStream ms = new MemoryStream();
                basis.UnParse(ms);
                ms.Position = 0L;
                Parse(ms);
            }
            public Compilation(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
                mFloatList01 = new FloatList(handler);
                mItems = new IndexList(handler);
                mByteArray01 = new byte[8];
                mByteArray02 = new byte[5];
                mByteArray03 = new byte[16];
            }
            public Compilation(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
            private UInt32 mUint01;
            private byte[] mByteArray01;
            private float mFloat01;
            private float mFloat02;
            private byte[] mByteArray02;
            private FloatList mFloatList01;
            private byte[] mByteArray03;
            private IndexList mItems;

            [ElementPriority(0)]
            public virtual BinaryReader Data
            {
                get
                {
                    MemoryStream s = new MemoryStream();
                    UnParse(s);
                    s.Position = 0L;
                    return new BinaryReader(s);
                }
                set
                {
                    if (value.BaseStream.CanSeek)
                    {
                        value.BaseStream.Position = 0L;
                        this.Parse(value.BaseStream);
                    }
                    else
                    {
                        MemoryStream s = new MemoryStream();
                        byte[] buffer = new byte[0x100000];
                        for (int i = value.BaseStream.Read(buffer, 0, buffer.Length); i > 0; i = value.BaseStream.Read(buffer, 0, buffer.Length))
                        {
                            s.Write(buffer, 0, i);
                        }
                        this.Parse(s);
                    }
                    OnElementChanged();
                }
            }

            [ElementPriority(1)]
            public uint Uint01
            {
                get { return mUint01; }
                set { mUint01 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public byte[] ByteArray01
            {
                get { return mByteArray01; }
                set { mByteArray01 = value; OnElementChanged(); }
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
            public byte[] ByteArray02
            {
                get { return mByteArray02; }
                set { mByteArray02 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public FloatList FloatList01
            {
                get { return mFloatList01; }
                set { mFloatList01 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public byte[] ByteArray03
            {
                get { return mByteArray03; }
                set { mByteArray03 = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public IndexList Items
            {
                get { return mItems; }
                set { mItems = value; OnElementChanged(); }
            }

            #region Nested Type: Index
            public class Index : AHandlerElement, IEquatable<Index>
            {
                public Index(int apiVersion, EventHandler handler, Index basis)
                    : base(apiVersion, handler)
                {
                    MemoryStream ms = new MemoryStream();
                    basis.UnParse(ms);
                    ms.Position = 0L;
                    Parse(ms);
                }
                public Index(int apiVersion, EventHandler handler)
                    : base(apiVersion, handler)
                {
                    mXAxis = new Vector3(0, handler);
                    mYAxis = new Vector3(0, handler);
                    mZAxis = new Vector3(0, handler);
                    mOffset = new Vector3(0, handler);
                    mVector3List01 = new Vector3List(handler);
                    mByteArray01 = new byte[12];
                }
                public Index(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler) { Parse(s); }
                #region Fields

                private byte mByte01;
                private UInt32 mUint01;
                private UInt16 mUshort01;
                private float mFloat01;
                private Vector3 mXAxis, mYAxis, mZAxis, mOffset;
                private byte mByte02;
                private byte mByte03;
                private Vector3List mVector3List01;
                private float mFloat14 = 1f;
                private float mFloat15 = 1f;
                private float mFloat16 = 1f;
                private float mFloat17 = 1f;
                private float mFloat18 = 1f;
                private float mFloat19 = 1f;
                private byte[] mByteArray01;
                private float mFloat20 = 1f;
                private UInt32 mUint02;
                private byte mByte04;
                private byte mByte05;

                #endregion



                #region Properties
                [ElementPriority(1)]
                public byte BlockTypeId
                {
                    get { return mByte01; }
                    set { mByte01 = value; OnElementChanged(); }
                }
                [ElementPriority(2)]
                public uint Uint01
                {
                    get { return mUint01; }
                    set { mUint01 = value; OnElementChanged(); }
                }
                [ElementPriority(3)]
                public ushort Ushort01
                {
                    get { return mUshort01; }
                    set { mUshort01 = value; OnElementChanged(); }
                }
                [ElementPriority(4)]
                public float Float01
                {
                    get { return mFloat01; }
                    set { mFloat01 = value; OnElementChanged(); }
                }
                [ElementPriority(5)]
                public Vector3 X_Axis
                {
                    get { return mXAxis; }
                    set { mXAxis = value; OnElementChanged(); }
                }
                [ElementPriority(6)]
                public Vector3 Y_Axis
                {
                    get { return mYAxis; }
                    set { mYAxis = value; OnElementChanged(); }
                }
                [ElementPriority(7)]
                public Vector3 Z_Axis
                {
                    get { return mZAxis; }
                    set { mZAxis = value; OnElementChanged(); }
                }
                [ElementPriority(8)]
                public Vector3 Offset
                {
                    get { return mOffset; }
                    set { mOffset = value; OnElementChanged(); }
                }
                [ElementPriority(9)]
                public byte Byte02
                {
                    get { return mByte02; }
                    set { mByte02 = value; OnElementChanged(); }
                }
                [ElementPriority(10)]
                public byte Byte03
                {
                    get { return mByte03; }
                    set { mByte03 = value; OnElementChanged(); }
                }
                [ElementPriority(11)]
                public Vector3List Vector3List01
                {
                    get { return mVector3List01; }
                    set { mVector3List01 = value; OnElementChanged(); }
                }
                [ElementPriority(12)]
                public float Float14
                {
                    get { return mFloat14; }
                    set { mFloat14 = value; OnElementChanged(); }
                }
                [ElementPriority(13)]
                public float Float15
                {
                    get { return mFloat15; }
                    set { mFloat15 = value; OnElementChanged(); }
                }
                [ElementPriority(14)]
                public float Float16
                {
                    get { return mFloat16; }
                    set { mFloat16 = value; OnElementChanged(); }
                }
                [ElementPriority(15)]
                public float Float17
                {
                    get { return mFloat17; }
                    set { mFloat17 = value; OnElementChanged(); }
                }
                [ElementPriority(16)]
                public float Float18
                {
                    get { return mFloat18; }
                    set { mFloat18 = value; OnElementChanged(); }
                }
                [ElementPriority(17)]
                public float Float19
                {
                    get { return mFloat19; }
                    set { mFloat19 = value; OnElementChanged(); }
                }
                [ElementPriority(18)]
                public byte[] ByteArray01
                {
                    get { return mByteArray01; }
                    set { mByteArray01 = value; OnElementChanged(); }
                }
                [ElementPriority(19)]
                public float Float20
                {
                    get { return mFloat20; }
                    set { mFloat20 = value; OnElementChanged(); }
                }
                [ElementPriority(20)]
                public uint BlockIndex
                {
                    get { return mUint02; }
                    set { mUint02 = value; OnElementChanged(); }
                }
                [ElementPriority(21)]
                public byte Byte04
                {
                    get { return mByte04; }
                    set { mByte04 = value; OnElementChanged(); }
                }
                [ElementPriority(22)]
                public byte Byte05
                {
                    get { return mByte05; }
                    set { mByte05 = value; OnElementChanged(); }
                }

                #endregion

                protected void Parse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    s.Read(out mByte01);
                    s.Read(out mUint01);
                    s.Read(out mUshort01);
                    s.Read(out mFloat01);
                    float x, y, z;
                    s.Read(out x, ByteOrder.LittleEndian);
                    s.Read(out y, ByteOrder.LittleEndian);
                    s.Read(out z, ByteOrder.LittleEndian);
                    mXAxis = new Vector3(0, handler, x, y, z);
                    s.Read(out x, ByteOrder.LittleEndian);
                    s.Read(out y, ByteOrder.LittleEndian);
                    s.Read(out z, ByteOrder.LittleEndian);
                    mYAxis = new Vector3(0, handler, x, y, z);
                    s.Read(out x, ByteOrder.LittleEndian);
                    s.Read(out y, ByteOrder.LittleEndian);
                    s.Read(out z, ByteOrder.LittleEndian);
                    mZAxis = new Vector3(0, handler, x, y, z);
                    s.Read(out x, ByteOrder.LittleEndian);
                    s.Read(out y, ByteOrder.LittleEndian);
                    s.Read(out z, ByteOrder.LittleEndian);
                    mOffset = new Vector3(0, handler, x, y, z);
                    s.Read(out mByte02);
                    s.Read(out mByte03);
                    mVector3List01 = new Vector3List(this.handler, stream);
                    s.Read(out mFloat14);
                    s.Read(out mFloat15);
                    s.Read(out mFloat16);
                    s.Read(out mFloat17);
                    s.Read(out mFloat18);
                    s.Read(out mFloat19);
                    s.Read(out mByteArray01, 12);
                    s.Read(out mFloat20);
                    s.Read(out mUint02);
                    s.Read(out mByte04);
                    s.Read(out mByte05);
                }

                public void UnParse(Stream stream)
                {
                    BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                    s.Write(mByte01);
                    s.Write(mUint01);
                    s.Write(mUshort01);
                    s.Write(mFloat01);
                    s.Write(mXAxis.X, ByteOrder.LittleEndian);
                    s.Write(mXAxis.Y, ByteOrder.LittleEndian);
                    s.Write(mXAxis.Z, ByteOrder.LittleEndian);
                    s.Write(mYAxis.X, ByteOrder.LittleEndian);
                    s.Write(mYAxis.Y, ByteOrder.LittleEndian);
                    s.Write(mYAxis.Z, ByteOrder.LittleEndian);
                    s.Write(mZAxis.X, ByteOrder.LittleEndian);
                    s.Write(mZAxis.Y, ByteOrder.LittleEndian);
                    s.Write(mZAxis.Z, ByteOrder.LittleEndian);
                    s.Write(mOffset.X, ByteOrder.LittleEndian);
                    s.Write(mOffset.Y, ByteOrder.LittleEndian);
                    s.Write(mOffset.Z, ByteOrder.LittleEndian);
                    s.Write(mByte02);
                    s.Write(mByte03);
                    mVector3List01.UnParse(stream);
                    s.Write(mFloat14);
                    s.Write(mFloat15);
                    s.Write(mFloat16);
                    s.Write(mFloat17);
                    s.Write(mFloat18);
                    s.Write(mFloat19);
                    s.Write(mByteArray01);
                    s.Write(mFloat20);
                    s.Write(mUint02);
                    s.Write(mByte04);
                    s.Write(mByte05);
                }


                public bool Equals(Index other)
                {
                    return base.Equals(other);
                }

                public override AHandlerElement Clone(EventHandler handler)
                {
                    throw new NotImplementedException();
                }

                public override List<string> ContentFields
                {
                    get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
                }

                public override int RecommendedApiVersion
                {
                    get { return kRecommendedAPIVersion; }
                }
            }
            #endregion

            public class IndexList : AResource.DependentList<Index>
            {
                public IndexList(EventHandler handler) : base(handler) { }
                public IndexList(EventHandler handler, Stream s) : base(handler, s) { }



                protected override uint ReadCount(Stream s)
                {
                    return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
                }
                protected override void WriteCount(Stream s, uint count)
                {
                    new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
                }

                public override void Add()
                {
                    base.Add(new object[0] { });
                }

                protected override Index CreateElement(Stream s)
                {
                    return new Index(0, elementHandler, s);
                }

                protected override void WriteElement(Stream s, Index element)
                {
                    element.UnParse(s);
                }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override List<string> ContentFields
            {
                get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedAPIVersion; }
            }

            public bool Equals(Compilation other)
            {
                return base.Equals(other);
            }
            private void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mUint01);
                s.Read(out mByteArray01, 8);
                s.Read(out mFloat01);
                s.Read(out mFloat02);
                s.Read(out mByteArray02, 5);
                mFloatList01 = new FloatList(handler, stream);
                s.Read(out mByteArray03, 16);
                mItems = new IndexList(handler, stream);
            }
            public void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mUint01);
                s.Write(mByteArray01);
                s.Write(mFloat01);
                s.Write(mFloat02);
                s.Write(mByteArray02);
                mFloatList01.UnParse(stream);
                s.Write(mByteArray03);
                mItems.UnParse(stream);

            }
        }
        #endregion

        #region Nested Type: CompilationList
        public class CompilationList : AResource.DependentList<Compilation>
        {

            public CompilationList(EventHandler handler) : base(handler) { }
            public CompilationList(EventHandler handler, Stream s) : base(handler, s) { }
            public override void Add()
            {
                base.Add(new object[0] { });
            }

            protected override Compilation CreateElement(Stream s)
            {
                return new Compilation(0, this.elementHandler, s);
            }

            protected override void WriteElement(Stream s, Compilation element)
            {
                element.UnParse(s);
            }
            protected override uint ReadCount(Stream s)
            {
                return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadUInt32();
            }
            protected override void WriteCount(Stream s, uint count)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((UInt32)count);
            }
        }
        #endregion

        #region Nested Type: EffectHandle
        public class EffectHandle : AHandlerElement, IEquatable<EffectHandle>
        {

            #region Fields
            private UInt32 mIndex;
            private string mEffectName = "<Insert Effect Name>";

            #endregion
            public EffectHandle(int apiVersion, EventHandler handler, EffectHandle basis)
                : base(apiVersion, handler)
            {
                mEffectName = basis.EffectName;
                mIndex = basis.Index;
            }
            public EffectHandle(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public EffectHandle(int apiVersion, EventHandler handler, Stream s, uint index) : base(apiVersion, handler) { mIndex = index; Parse(s); }

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


            protected void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mEffectName = s.ReadString(StringType.ZeroDelimited);
            }

            public void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mIndex);
                s.Write(mEffectName, StringType.ZeroDelimited);
            }


            #region IComparable<EffectHandle> Members

            public int CompareTo(EffectHandle other)
            {
                return mEffectName.CompareTo(other.mEffectName);
            }

            #endregion


            #region IComparable Members

            public int CompareTo(object obj)
            {
                return mEffectName.CompareTo(((EffectHandle)obj).mEffectName);
            }

            #endregion

            public override string ToString()
            {
                return EffectName;
            }

            public bool Equals(EffectHandle other)
            {
                return mEffectName.Equals(other);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override System.Collections.Generic.List<string> ContentFields
            {
                get { return AApiVersionedFields.GetContentFields(RecommendedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedAPIVersion; }
            }
        }
        #endregion

        #region Nested Type: EffectHandleList
        public class EffectHandleList : AHandlerList<EffectHandle>, IGenericAdd
        {

            public EffectHandleList(EventHandler handler) : base(handler) { }
            public EffectHandleList(EventHandler handler, Stream s)
                : base(handler)
            {
                Parse(s);
            }
            protected void Parse(System.IO.Stream s)
            {
                BinaryStreamWrapper bw = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                uint index = bw.ReadUInt32();
                while (index != 0xFFFFFFFF)
                {
                    EffectHandle item = new EffectHandle(0, base.handler, s, index);
                    index = bw.ReadUInt32();
                    base.Add(item);
                }
            }

            public void UnParse(Stream s)
            {
                BinaryStreamWrapper w = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                foreach (var item in this)
                {
                    EffectHandle handle = (EffectHandle)item;
                    ((EffectHandle)item).UnParse(s);
                }
                w.Write((uint)0xFFFFFFFF);
            }




            public bool Add(params object[] fields)
            {
                try
                {
                    Add(Activator.CreateInstance(typeof(EffectHandle), fields));
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public void Add()
            {
                base.Add(new EffectHandle(0, null));
            }
        }
        #endregion

        #region Fields
        private UInt16 mVersion = 0x00000002;
        private EffectSectionList mEffects;
        private byte[] mByteArray01 = new byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        private UInt32 mUint01 = 0x00000001;
        private UInt16 mShort01 = 0x0000;
        private byte[] mByteArray02 = new byte[4] { 0xFF, 0xFF, 0x00, 0x02 };
        private byte[] mByteArray03 = new byte[4] { 0xFF, 0xFF, 0xFF, 0xFF };
        private MaterialList mMaterials;
        private CompilationList mCompilations;
        private EffectHandleList mHandles;
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

        [ElementPriority(6)]
        public byte[] ByteArray01
        {
            get { return mByteArray01; }
            set { mByteArray01 = value; OnResourceChanged(this, new EventArgs()); }
        }

        [ElementPriority(7)]
        public uint Uint01
        {
            get { return mUint01; }
            set { mUint01 = value; OnResourceChanged(this, new EventArgs()); }
        }

        [ElementPriority(8)]
        public ushort Short01
        {
            get { return mShort01; }
            set { mShort01 = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public MaterialList Materials
        {
            get { return mMaterials; }
            set { mMaterials = value; OnResourceChanged(this, new EventArgs()); }
        }

        [ElementPriority(9)]
        public byte[] ByteArray02
        {
            get { return mByteArray02; }
            set { mByteArray02 = value; OnResourceChanged(this, new EventArgs()); }
        }

        [ElementPriority(10)]
        public byte[] ByteArray03
        {
            get { return mByteArray03; }
            set { mByteArray03 = value; ; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        public CompilationList Compilations
        {
            get { return mCompilations; }
            set { mCompilations = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(5)]
        public EffectHandleList Handles
        {
            get { return mHandles; }
            set { mHandles = value; OnResourceChanged(this, new EventArgs()); }
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
            mByteArray01 = s.ReadBytes(6);
            s.Read(out mUint01);
            s.Read(out mShort01);
            mMaterials = new MaterialList(this.OnResourceChanged, stream);
            mByteArray02 = s.ReadBytes(4);
            mCompilations = new CompilationList(this.OnResourceChanged, stream);
            mByteArray03 = s.ReadBytes(4);
            mHandles = new EffectHandleList(this.OnResourceChanged, stream);
        }


        protected override Stream UnParse()
        {
            MemoryStream stream = new MemoryStream();
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mVersion);
            if (mEffects == null) mEffects = new EffectSectionList(this.OnResourceChanged);
            mEffects.UnParse(stream);
            s.Write(mByteArray01);
            s.Write(mUint01);
            s.Write(mShort01);
            if (this.mMaterials == null) this.mMaterials = new MaterialList(this.OnResourceChanged);
            mMaterials.UnParse(stream);
            s.Write(mByteArray02);
            if (mCompilations == null) mCompilations = new CompilationList(this.OnResourceChanged);
            mCompilations.UnParse(stream);
            s.Write(mByteArray03);
            if (mHandles == null) mHandles = new EffectHandleList(this.OnResourceChanged);
            mHandles.UnParse(stream);
            return stream;
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedAPIVersion; }
        }
        const int kRecommendedAPIVersion = 1;
    }
}