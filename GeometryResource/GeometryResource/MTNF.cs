using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using s3pi.GenericRCOLResource;
using s3pi.Interfaces;
using s3pi.Settings;
using System.Text;

namespace s3piwrappers
{
    public class MTNF : AHandlerElement
    {
        public abstract class ShaderKeyData : AHandlerElement, IEquatable<ShaderKeyData>
        {
            protected ShaderKeyData(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
            }
            protected ShaderKeyData(int APIversion, EventHandler handler,ShaderKeyData basis)
                : base(APIversion, handler)
            {
                
            }

            public abstract void Parse(Stream s);
            public abstract void UnParse(Stream s);
            public override List<string> ContentFields
            {
                get { return GetContentFields(kRecommendedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }


            public bool Equals(ShaderKeyData other)
            {
                return base.Equals(other);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return (AHandlerElement) Activator.CreateInstance(GetType(), new object[] {0, handler, this});
            }
        }
        public abstract class ShaderKeyData<T> : ShaderKeyData
        {
            protected T mData;

            public ShaderKeyData(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
            }

            protected ShaderKeyData(int APIversion, EventHandler handler, ShaderKeyData<T> basis)
                : base(APIversion, handler, basis)
            {
            }
            [ElementPriority(1)]
            public T Data
            {
                get { return mData; }
                set { mData = value; OnElementChanged(); }
            }

            public abstract override void Parse(Stream s);
            public abstract override void UnParse(Stream s);
        }
        public class ShaderKeyFloatData : ShaderKeyData<float>
        {
            public ShaderKeyFloatData(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
            }

            public ShaderKeyFloatData(int APIversion, EventHandler handler, ShaderKeyFloatData basis)
                : base(APIversion, handler, basis)
            {
                mData = basis.mData;
            }

            public override void Parse(Stream s)
            {
                mData = new BinaryReader(s).ReadSingle();
            }

            public override void UnParse(Stream s)
            {
                new BinaryWriter(s).Write(mData);
            }
            public override string ToString()
            {
                return mData.ToString("0.00000");
            }
        }
        public class ShaderKeyIntData : ShaderKeyData<int>
        {
            public ShaderKeyIntData(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
            }

            public ShaderKeyIntData(int APIversion, EventHandler handler, ShaderKeyIntData basis)
                : base(APIversion, handler, basis)
            {
                mData = basis.mData;
            }
            public override void Parse(Stream s)
            {
                mData = new BinaryReader(s).ReadInt32();
            }

            public override void UnParse(Stream s)
            {
                new BinaryWriter(s).Write(mData);
            }
            public override string ToString()
            {
                return "0x" + mData.ToString("X8");
            }
        }
        public abstract class ShaderKeyDataList : AResource.DependentList<ShaderKeyData>
        {
            public ShaderKeyDataList(EventHandler handler)
                : base(handler)
            {
            }

            protected ShaderKeyDataList(EventHandler handler, Stream s) : base(handler, s)
            {
            }

            public abstract override void Add();
            protected abstract override ShaderKeyData CreateElement(Stream s);
            protected abstract override void WriteElement(Stream s, ShaderKeyData element);
        }
        public class ShaderKeyDataList<T> : ShaderKeyDataList
            where T : ShaderKeyData
        {
            public ShaderKeyDataList(EventHandler handler) : base(handler)
            {
            }

            public ShaderKeyDataList(EventHandler handler, Stream s) : base(handler, s)
            {
            }

            public override void Add()
            {
                ((IList<ShaderKeyData>)this).Add((T)Activator.CreateInstance(typeof (T), new object[] {0,handler}));
            }
            protected override void Parse(Stream s)
            {
                uint count = ReadCount(s);
                for (int i = 0; i < count; i++)
                {
                    Add();
                }
            }
            public override void UnParse(Stream s)
            {
                WriteCount(s,(uint)Count);
            }
            protected override ShaderKeyData CreateElement(Stream s)
            {
                return (T) Activator.CreateInstance(typeof (T), new object[] {0, handler, s});

            }

            protected override void WriteElement(Stream s, ShaderKeyData element)
            {
                element.UnParse(s);
            }
        }
        public abstract class ShaderKey : AHandlerElement, IEquatable<ShaderKey>
        {
            protected MATD.DataType mType;
            protected MATD.FieldType mField;
            protected ShaderKeyDataList mData;

            public ShaderKey(int APIversion, EventHandler handler, MATD.DataType type, MATD.FieldType field)
                : base(APIversion, handler)
            {
                mField = field;
                mType = type;
            }
            public ShaderKey(int APIversion, EventHandler handler, ShaderKey basis) : this(APIversion, handler, basis.mType, basis.mField) { }
            [ElementPriority(1)]
            public MATD.FieldType Field
            {
                get { return mField; }
                set { mField = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public MATD.DataType Type
            {
                get { return mType; }
            }

            [ElementPriority(3)]
            public ShaderKeyDataList Data
            {
                get { return mData; }
                set { mData = value; OnElementChanged(); }
            }

            public static Type GetShaderType(MATD.DataType type)
            {
                switch (type)
                {
                    case MATD.DataType.dtFloat: return typeof(FloatShaderKey);
                    case MATD.DataType.dtUInt32_1: return typeof(Int1ShaderKey);
                    case MATD.DataType.dtUInt32_2: return typeof(Int2ShaderKey);
                    default: throw new InvalidDataException("Invalid MTNF ShaderKey DataType:" + type.ToString());
                }
            }
            public static ShaderKey CreateInstance(int apiVersion, EventHandler handler, MATD.DataType type, MATD.FieldType field)
            {
                return (ShaderKey)Activator.CreateInstance(GetShaderType(type), new object[] { 0, handler, type, field });
            }
            public static ShaderKey CreateInstance(int apiVersion, EventHandler handler, MATD.DataType type)
            {
                return (ShaderKey)Activator.CreateInstance(GetShaderType(type), new object[] { 0, handler, type, MATD.FieldType.AlphaMap });
            }

            public abstract override AHandlerElement Clone(EventHandler handler);

            public override List<string> ContentFields
            {
                get { return GetContentFields(kRecommendedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public abstract void Parse(Stream s);
            public abstract void UnParse(Stream s);

            public bool Equals(ShaderKey other)
            {
                return mField.Equals(other.mField);
            }
        }
        public abstract class ShaderKey<T> : ShaderKey
            where T : ShaderKeyData
        {

            protected ShaderKey(int APIversion, EventHandler handler, MATD.DataType type, MATD.FieldType field)
                : base(APIversion, handler, type, field)
            {
                mData = new ShaderKeyDataList<T>(handler);
            }

            protected ShaderKey(int APIversion, EventHandler handler, ShaderKey<T> basis)
                : base(APIversion, handler, basis)
            {
                mData = new ShaderKeyDataList<T>(handler);
            }

            public abstract override AHandlerElement Clone(EventHandler handler);

            public override void Parse(Stream s)
            {
                mData = new ShaderKeyDataList<T>(handler, s);
            }

            public override void UnParse(Stream s)
            {
                mData.UnParse(s);
            }
            public override string ToString()
            {
                string s = mField.ToString() + " = ";
                foreach (var item in mData) s += item.ToString() + " ";
                return s;
            }
        }
        [ConstructorParameters(new object[] { MATD.DataType.dtFloat })]
        public class FloatShaderKey : ShaderKey<ShaderKeyFloatData>
        {
            public FloatShaderKey(int APIversion, EventHandler handler, MATD.DataType type, MATD.FieldType field)
                : base(APIversion, handler, type, field)
            {
            }

            public FloatShaderKey(int APIversion, EventHandler handler, FloatShaderKey basis)
                : base(APIversion, handler, basis)
            {
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new FloatShaderKey(0, handler, this);
            }

        }
        [ConstructorParameters(new object[] { MATD.DataType.dtUInt32_1 })]
        public class Int1ShaderKey : ShaderKey<ShaderKeyIntData>
        {
            public Int1ShaderKey(int APIversion, EventHandler handler, MATD.DataType type, MATD.FieldType field)
                : base(APIversion, handler, type, field)
            {
            }

            public Int1ShaderKey(int APIversion, EventHandler handler, Int1ShaderKey basis)
                : base(APIversion, handler, basis)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Int1ShaderKey(0, handler, this);
            }

        }
        [ConstructorParameters(new object[] { MATD.DataType.dtUInt32_2 })]
        public class Int2ShaderKey : ShaderKey<ShaderKeyIntData>
        {
            public Int2ShaderKey(int APIversion, EventHandler handler, MATD.DataType type, MATD.FieldType field)
                : base(APIversion, handler, type, field)
            {
            }

            public Int2ShaderKey(int APIversion, EventHandler handler, Int2ShaderKey basis)
                : base(APIversion, handler, basis)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Int2ShaderKey(0, handler, this);
            }

        }
        public class ShaderKeyList : AResource.DependentList<ShaderKey>
        {
            public ShaderKeyList(EventHandler handler) : base(handler)
            {
            }

            public ShaderKeyList(EventHandler handler, Stream s) : base(handler, s)
            {
            }
            protected override void Parse(Stream s)
            {
                var br = new BinaryReader(s);
                long dataLength = br.ReadUInt32();
                uint count = ReadCount(s);
                MATD.FieldType[] fields = new MATD.FieldType[count];
                uint[] offsets = new uint[count];
                for (int i = 0; i < count; i++)
                {
                    var field = (MATD.FieldType) br.ReadUInt32();
                    var type = (MATD.DataType) br.ReadUInt32();
                    var skey = ShaderKey.CreateInstance(0, handler, type, field);
                    skey.Parse(s);
                    offsets[i] = br.ReadUInt32();
                    ((IList<ShaderKey>)this).Add(skey);

                }
                long dataStart = s.Position;
                for (int i = 0; i < count; i++)
                {
                    if(checking && s.Position != offsets[i])
                        throw new InvalidDataException(String.Format("Bad offset: Expected 0x{0:X8}, but got 0x{1:X8}",offsets[i],s.Position));
                    for (int j = 0; j < this[i].Data.Count; j++)
                    {
                        this[i].Data[j].Parse(s);
                    }
                }
                long dataEnd = s.Position;
                long actualSize = dataEnd - dataStart;
                if (checking && actualSize != dataLength)
                    throw new InvalidDataException(String.Format("Bad data length: Expected 0x{0:X8}, but got 0x{1:X8}", dataLength, actualSize));
            }
            public override void UnParse(Stream s)
            {
                var bw = new BinaryWriter(s);
                int headerSize = Count * 16;
                long headerPos = s.Position;
                long dataStart = s.Position + (headerSize) + 8;
                uint[] offsets = new uint[Count];
                s.Seek(dataStart,SeekOrigin.Begin);
                for (int i = 0; i < Count; i++)
                {
                    offsets[i] = (uint)s.Position;
                    for (int j = 0; j < this[i].Data.Count; j++)
                    {
                        this[i].Data[j].UnParse(s);
                    }
                }
                
                long dataEnd = s.Position;
                long dataLength = dataEnd - dataStart;
                s.Seek(headerPos, SeekOrigin.Begin);
                bw.Write((uint)dataLength);
                bw.Write((uint)Count);
                for (int i = 0; i < Count; i++)
                {
                    bw.Write((uint)this[i].Field);
                    bw.Write((uint)this[i].Type);
                    bw.Write((uint)this[i].Data.Count);
                    bw.Write(offsets[i]);
                }
                s.Seek(dataEnd, SeekOrigin.Begin);


            }
            
            public override void Add()
            {
                throw new NotImplementedException();
            }

            protected override ShaderKey CreateElement(Stream s)
            {
                throw new NotImplementedException();
            }
            public override bool Add(params object[] fields)
            {
                if(fields.Length > 0 && typeof(MATD.DataType).IsAssignableFrom(fields[0].GetType()))
                {
                    ((IList<ShaderKey>)this).Add(ShaderKey.CreateInstance(0, handler, (MATD.DataType)fields[0]));
                    return true;
                }
                return false;
            }

            protected override void WriteElement(Stream s, ShaderKey element)
            {
                throw new NotImplementedException();
            }
        }
        private const String Tag = "MTNF";
        public MTNF(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
            mShaderKeys = new ShaderKeyList(handler);
        }
        public MTNF(int APIversion, EventHandler handler, MTNF basis)
            : base(APIversion, handler)
        {
            MemoryStream ms = new MemoryStream();
            basis.UnParse(ms);
            ms.Position = 0L;
            Parse(ms);
        }
        public MTNF(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler)
        {
            Parse(s);
        }

        private UInt32 mUnknown01;
        private ShaderKeyList mShaderKeys;
        [ElementPriority(1)]
        public uint Unknown01
        {
            get { return mUnknown01; }
            set { mUnknown01 = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public ShaderKeyList ShaderKeys
        {
            get { return mShaderKeys; }
            set { mShaderKeys = value; OnElementChanged(); }
        }
        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                if(mShaderKeys.Count >0)
                {
                    sb.AppendFormat("Shader Keys:\n", mUnknown01);
                    for (int i = 0; i < mShaderKeys.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}]:{1}\n", i, mShaderKeys[i].ToString());
                    }
                }
                return sb.ToString();
            }
        }
        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
                throw new InvalidDataException(String.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{2:X8}", tag, Tag, s.Position));
            mUnknown01 = br.ReadUInt32();
            mShaderKeys = new ShaderKeyList(handler,s);
            
        }
        public void UnParse(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((uint)FOURCC(Tag));
            bw.Write(mUnknown01);
            mShaderKeys.UnParse(s);
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new MTNF(0, handler, this);
        }

        public override List<string> ContentFields
        {
            get
            {
                return GetContentFields(0, GetType());
            }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
        private const int kRecommendedApiVersion = 1;
        private static bool checking = Settings.Checking;
    }
}