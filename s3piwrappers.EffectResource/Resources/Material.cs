﻿using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;

namespace s3piwrappers.Resources
{
    public class Material : Resource, IEquatable<Material>
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

        
        public class PropertyList : AResource.DependentList<Property>
        {
            public PropertyList(EventHandler handler) : base(handler) { }
            public PropertyList(EventHandler handler, Stream s) : base(handler, s) { }
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
                throw new NotSupportedException();
            }

            protected override Property CreateElement(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                PropertyKey key = (PropertyKey)s.ReadUInt64();
                ValueType val = (ValueType)s.BaseStream.ReadByte();
                return Property.CreateInstance(0, elementHandler, val, key, stream);
            }

            protected override void WriteElement(Stream stream, Property element)
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
        

        
        public abstract class Property : DataElement, IEquatable<Property>, IComparable<Property>
        {
            protected Property(int apiVersion, EventHandler handler, Property basis)
                : base(apiVersion, handler,basis)
            {
                mKey = basis.Key;
                mValType = basis.ValueType;
            }
            protected Property(int apiVersion, EventHandler handler, ValueType type, PropertyKey key)
                : base(apiVersion, handler)
            {
                mKey = key;
                mValType = type;
            }
            protected Property(int apiVersion, EventHandler handler, ValueType type, PropertyKey key, Stream s)
                : base(apiVersion, handler,s)
            {
                mKey = key;
                mValType = type;
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

            protected abstract override void Parse(Stream s);
            public abstract override void UnParse(Stream s);


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
        private ShaderType mShader = ShaderType.FluidEffect;
        private PropertyList mProperties;

        public Material(int apiVersion, EventHandler handler, ISection section) : base(apiVersion, handler, section)
        {
            mProperties=new PropertyList(handler);
        }


        public Material(int apiVersion, EventHandler handler, ISection section, Stream s) : base(apiVersion, handler, section, s)
        {
        }

        public Material(int apiVersion, EventHandler handler, Material basis) : base(apiVersion, handler, basis)
        {
        }

        public override string ToString()
        {
            return String.Format("Shader(0x{0:X16})", HashedName);
        }

            
        [ElementPriority(1)]
        public ulong HashedName
        {
            get { return mHashedName; }
            set { mHashedName = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public ShaderType Shader
        {
            get { return mShader; }
            set { mShader = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public PropertyList Properties
        {
            get { return mProperties; }
            set { mProperties = value; OnElementChanged(); }
        }


        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            HashedName = s.ReadUInt64();
            Shader = (ShaderType)s.ReadUInt64();

            mProperties = new PropertyList(handler, stream);
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(HashedName);
            s.Write((UInt64)Shader);
            mProperties.UnParse(stream);
        }
            

        public bool Equals(Material other)
        {
            return mHashedName.Equals(other.mHashedName);
        }

    }
}