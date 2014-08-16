using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Resources
{
    public class Material : Resource, IEquatable<Material>
    {
        #region Subtypes

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
            Int = 0x01,
            Bool = 0x02,
            FloatList = 0x03,
            IntList = 0x04,
            BoolList = 0x05,
            ResourceKey = 0x06
        }

        public class PropertyList : DependentList<Property>
        {
            public PropertyList(EventHandler handler) : base(handler)
            {
            }

            public PropertyList(EventHandler handler, Stream s) : base(handler, s)
            {
            }

            protected override int ReadCount(Stream s)
            {
                return new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadInt32();
            }

            protected override void WriteCount(Stream s, int count)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write((uint) count);
            }

            public override void Add()
            {
                throw new NotSupportedException();
            }

            protected override Property CreateElement(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                var key = (PropertyKey) s.ReadUInt64();
                var val = (ValueType) s.BaseStream.ReadByte();
                return Property.CreateInstance(0, elementHandler, val, key, stream);
            }

            protected override void WriteElement(Stream stream, Property element)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write((ulong) element.Key);
                s.Write((byte) element.ValueType);
                element.UnParse(stream);
            }
        }


        public abstract class Property : DataElement, IEquatable<Property>, IComparable<Property>
        {
            #region Constructors
            protected Property(int apiVersion, EventHandler handler, Property basis)
                : base(apiVersion, handler, basis)
            {
                mKey = basis.Key;
            }

            protected Property(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }

            protected Property(int apiVersion, EventHandler handler, PropertyKey key)
                : this(apiVersion, handler)
            {
                mKey = key;
            }

            protected Property(int apiVersion, EventHandler handler, PropertyKey key, Stream s)
                : base(apiVersion, handler, s)
            {
                mKey = key;
            }
            #endregion

            #region Attributes
            private PropertyKey mKey;
            private object mValue = 0;
            #endregion

            #region Content Fields
            [ElementPriority(0)]
            public PropertyKey Key
            {
                get { return mKey; }
                set
                {
                    mKey = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(1)]
            public abstract ValueType ValueType { get; }
            #endregion

            #region Data I/O
            public static Property CreateInstance(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s)
            {
                switch (type)
                {
                case ValueType.Float:
                    return new FloatProperty(apiVersion, handler, type, key, s);
                case ValueType.Int:
                    return new IntProperty(apiVersion, handler, type, key, s);
                case ValueType.Bool:
                    return new BoolProperty(apiVersion, handler, type, key, s);
                case ValueType.FloatList:
                    return new FloatListProperty(apiVersion, handler, type, key, s);
                case ValueType.IntList:
                    return new IntListProperty(apiVersion, handler, type, key, s);
                case ValueType.BoolList:
                    return new BoolListProperty(apiVersion, handler, type, key, s);
                case ValueType.ResourceKey:
                    return new ResourceKeyProperty(apiVersion, handler, type, key, s);
                default:
                    throw new Exception("Unknown Property type!");
                }
            }

            protected abstract override void Parse(Stream s);
            public abstract override void UnParse(Stream s);
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
                return (AHandlerElement) MemberwiseClone();
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            public int CompareTo(Property other)
            {
                return mKey.CompareTo(other.mKey);
            }
        }


        public class FloatProperty : Property
        {
            #region Constructors
            public FloatProperty(int apiVersion, EventHandler handler, FloatProperty basis) 
                : base(apiVersion, handler, basis)
            {
            }

            public FloatProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key) 
                : base(apiVersion, handler, key)
            {
            }

            public FloatProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s) 
                : base(apiVersion, handler, key, s)
            {
            }
            #endregion

            private float mValue;

            [ElementPriority(2)]
            public float Data
            {
                get { return mValue; }
                set
                {
                    mValue = value;
                    OnElementChanged();
                }
            }

            #region Data I/O
            protected override void Parse(Stream s)
            {
                mValue = new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadFloat();
            }

            public override void UnParse(Stream s)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write(mValue);
            }
            #endregion

            public override ValueType ValueType
            {
                get { return ValueType.Float; }
            }
        }

        public class IntProperty : Property
        {
            #region Constructors
            public IntProperty(int apiVersion, EventHandler handler, FloatProperty basis)
                : base(apiVersion, handler, basis)
            {
            }

            public IntProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key)
                : base(apiVersion, handler, key)
            {
            }

            public IntProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s)
                : base(apiVersion, handler, key, s)
            {
            }
            #endregion

            private int mValue;

            [ElementPriority(2)]
            public int Data
            {
                get { return mValue; }
                set
                {
                    mValue = value;
                    OnElementChanged();
                }
            }

            #region Data I/O
            protected override void Parse(Stream s)
            {
                mValue = new BinaryStreamWrapper(s, ByteOrder.BigEndian).ReadInt32();
            }

            public override void UnParse(Stream s)
            {
                new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write(mValue);
            }
            #endregion

            public override ValueType ValueType
            {
                get { return ValueType.Int; }
            }
        }

        public class BoolProperty : Property
        {
            #region Constructors
            public BoolProperty(int apiVersion, EventHandler handler, FloatProperty basis)
                : base(apiVersion, handler, basis)
            {
            }

            public BoolProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key)
                : base(apiVersion, handler, key)
            {
            }

            public BoolProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s)
                : base(apiVersion, handler, key, s)
            {
            }
            #endregion

            private bool mValue;

            [ElementPriority(2)]
            public bool Data
            {
                get { return mValue; }
                set
                {
                    mValue = value;
                    OnElementChanged();
                }
            }

            #region Data I/O
            protected override void Parse(Stream s)
            {
                mValue = s.ReadByte() != 0;
            }

            public override void UnParse(Stream s)
            {
                byte value = (byte)(mValue ? 0xFF : 0x00);
                s.WriteByte(value);
            }
            #endregion

            public override ValueType ValueType
            {
                get { return ValueType.Bool; }
            }
        }

        public class FloatListProperty : Property
        {
            #region Constructors
            public FloatListProperty(int apiVersion, EventHandler handler, FloatProperty basis)
                : base(apiVersion, handler, basis)
            {
            }

            public FloatListProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key)
                : base(apiVersion, handler, key)
            {
                mValue = new DataList<FloatValue>(handler);
            }

            public FloatListProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s)
                : base(apiVersion, handler, key, s)
            {
            }
            #endregion

            private DataList<FloatValue> mValue;

            [ElementPriority(2)]
            public DataList<FloatValue> Data
            {
                get { return mValue; }
                set
                {
                    mValue = new DataList<FloatValue>(handler, value);
                    OnElementChanged();
                }
            }

            #region Data I/O
            protected override void Parse(Stream s)
            {
                mValue = new DataList<FloatValue>(handler, s);
            }

            public override void UnParse(Stream s)
            {
                mValue.UnParse(s);
            }
            #endregion

            public override ValueType ValueType
            {
                get { return ValueType.FloatList; }
            }
        }

        public class IntListProperty : Property
        {
            #region Constructors
            public IntListProperty(int apiVersion, EventHandler handler, FloatProperty basis)
                : base(apiVersion, handler, basis)
            {
            }

            public IntListProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key)
                : base(apiVersion, handler, key)
            {
                mValue = new DataList<UInt32Value>(handler);
            }

            public IntListProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s)
                : base(apiVersion, handler, key, s)
            {
            }
            #endregion

            private DataList<UInt32Value> mValue;

            [ElementPriority(2)]
            public DataList<UInt32Value> Data
            {
                get { return mValue; }
                set
                {
                    mValue = new DataList<UInt32Value>(handler, value);
                    OnElementChanged();
                }
            }

            #region Data I/O
            protected override void Parse(Stream s)
            {
                mValue = new DataList<UInt32Value>(handler, s);
            }

            public override void UnParse(Stream s)
            {
                mValue.UnParse(s);
            }
            #endregion

            public override ValueType ValueType
            {
                get { return ValueType.IntList; }
            }
        }

        public class BoolListProperty : Property
        {
            #region Constructors
            public BoolListProperty(int apiVersion, EventHandler handler, FloatProperty basis)
                : base(apiVersion, handler, basis)
            {
            }

            public BoolListProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key)
                : base(apiVersion, handler, key)
            {
                mValue = new DataList<ByteValue>(handler);
            }

            public BoolListProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s)
                : base(apiVersion, handler, key, s)
            {
            }
            #endregion

            private DataList<ByteValue> mValue;

            [ElementPriority(2)]
            public DataList<ByteValue> Data
            {
                get { return mValue; }
                set
                {
                    mValue = new DataList<ByteValue>(handler, value);
                    OnElementChanged();
                }
            }

            #region Data I/O
            protected override void Parse(Stream s)
            {
                mValue = new DataList<ByteValue>(handler, s);
            }

            public override void UnParse(Stream s)
            {
                mValue.UnParse(s);
            }
            #endregion

            public override ValueType ValueType
            {
                get { return ValueType.FloatList; }
            }
        }

        public class ResourceKeyProperty : Property
        {
            #region Constructors
            public ResourceKeyProperty(int apiVersion, EventHandler handler, ResourceKeyProperty basis) 
                : base(apiVersion, handler, basis)
            {
            }

            public ResourceKeyProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key) 
                : base(apiVersion, handler, key)
            {
            }

            public ResourceKeyProperty(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s) 
                : base(apiVersion, handler, key, s)
            {
            }
            #endregion

            private ulong mValue;

            [ElementPriority(2)]
            public ulong Data
            {
                get { return mValue; }
                set
                {
                    mValue = value;
                    OnElementChanged();
                }
            }

            #region Data I/O
            protected override void Parse(Stream s)
            {
                mValue = new BinaryStreamWrapper(s, ByteOrder.LittleEndian).ReadUInt64();
            }

            public override void UnParse(Stream s)
            {
                new BinaryStreamWrapper(s, ByteOrder.LittleEndian).Write(mValue);
            }
            #endregion

            public override ValueType ValueType
            {
                get { return ValueType.ResourceKey; }
            }
        }

        #endregion

        #region Attributes
        private ulong mHashedName;
        private ShaderType mShader = ShaderType.FluidEffect;
        private PropertyList mProperties;
        #endregion

        #region Constructors
        public Material(int apiVersion, EventHandler handler, ISection section) 
            : base(apiVersion, handler, section)
        {
            mProperties = new PropertyList(handler);
        }


        public Material(int apiVersion, EventHandler handler, ISection section, Stream s) 
            : base(apiVersion, handler, section, s)
        {
        }

        public Material(int apiVersion, EventHandler handler, Material basis) 
            : base(apiVersion, handler, basis)
        {
        }
        #endregion

        public override string ToString()
        {
            return string.Format("Shader(0x{0:X16})", HashedName);
        }

        #region Content Fields
        [ElementPriority(1)]
        public ulong HashedName
        {
            get { return mHashedName; }
            set
            {
                mHashedName = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public ShaderType Shader
        {
            get { return mShader; }
            set
            {
                mShader = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public PropertyList Properties
        {
            get { return mProperties; }
            set
            {
                mProperties = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            HashedName = s.ReadUInt64();
            Shader = (ShaderType) s.ReadUInt64();

            mProperties = new PropertyList(handler, stream);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(HashedName);
            s.Write((ulong) Shader);
            mProperties.UnParse(stream);
        }
        #endregion

        public bool Equals(Material other)
        {
            return mHashedName.Equals(other.mHashedName);
        }
    }
}
