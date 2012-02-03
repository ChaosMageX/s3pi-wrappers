using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using System.ComponentModel;

namespace s3piwrappers
{
    public class AudioTunerResource : AResource
    {
        public class BlockList : DependentList<DataBlock>
        {

            public BlockList(EventHandler handler)
                : base(handler)
            {
            }

            public BlockList(EventHandler handler, IEnumerable<DataBlock> ilt)
                : base(handler, ilt)
            {
            }

            public BlockList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }
            protected override int ReadCount(Stream s)
            {
                return new BinaryStreamWrapper(s).ReadInt32(ByteOrder.BigEndian);
            }
            protected override void WriteCount(Stream s, int count)
            {
                new BinaryStreamWrapper(s).Write(count);
            }

            protected override void WriteElement(Stream s, DataBlock element)
            {
                element.UnParse(s);
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override DataBlock CreateElement(Stream s)
            {
                return new Block(0, handler, s);
            }
            class Block : DataBlock
            {
                public Block(int APIversion, EventHandler handler, DataType typeCode, ushort repCode) : base(APIversion, handler, typeCode, repCode)
                {
                }

                public Block(int APIversion, EventHandler handler, byte dataFlags, DataType typeCode, ushort repCode, uint id, TypedData blockData) : base(APIversion, handler, dataFlags, typeCode, repCode, id, blockData)
                {
                }

                public Block(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s)
                {
                }
            }
        }

        public class DataList : DependentList<TypedData>, IEquatable<DataList>
        {
            private readonly DataBlock mOwner;
            private UInt32 mUnknown1;
            public DataList(EventHandler handler, DataBlock owner)
                : base(handler)
            {
                mOwner = owner;
            }

            public DataList(EventHandler handler, IEnumerable<TypedData> ilt, DataBlock owner)
                : base(handler, ilt)
            {
                mOwner = owner;
            }

            public DataList(EventHandler handler, Stream s, DataBlock owner)
                : this(handler, owner)
            {
                this.Parse(s);
            }

            protected override int ReadCount(Stream s)
            {
                var br = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                var c = br.ReadInt32();
                mUnknown1 = br.ReadUInt32();
                return c;

            }
            protected override void WriteCount(Stream s, int count)
            {
                var bw = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                bw.Write(count);
                bw.Write(mUnknown1);
            }
            protected override TypedData CreateElement(Stream s)
            {
                return DataBlock.ReadTypedData(0, this.handler, this.mOwner.TypeCode, s);
            }

            protected override void WriteElement(Stream s, TypedData element)
            {
                element.UnParse(s);
            }

            public override void Add()
            {
                Add(new object[] { mOwner.TypeCode });
            }

            public bool Equals(DataList other)
            {
                return Equals((AHandlerList<TypedData>)other);
            }
        }

        public class ListData : TypedData<DataList>
        {
            private readonly DataBlock mOwner;
            public ListData(int APIversion, EventHandler handler, DataBlock owner)
                : base(APIversion, handler)
            {
                mOwner = owner;
            }

            public ListData(int APIversion, EventHandler handler, Stream s, DataBlock owner)
                : this(APIversion, handler, owner)
            {
                this.Parse(s);
            }

            public ListData(int APIversion, EventHandler handler, DataList data, DataBlock owner)
                : base(APIversion, handler, data)
            {
                mOwner = owner;
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                var items = Data.Select(x => (TypedData)x.Clone(handler));
                return new ListData(0, handler, new DataList(handler, items, mOwner), mOwner);
            }
            public DataType TypeCode
            {
                get { return mOwner.TypeCode; }
            }

            protected override void Parse(Stream s)
            {
                Data = new DataList(handler, s, mOwner);
            }

            public override void UnParse(Stream stream)
            {
                throw new NotImplementedException();
            }
        }

        public abstract class TypedData : AHandlerElement, IEquatable<TypedData>
        {
            private object mData;

            protected TypedData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            protected TypedData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            protected TypedData(int APIversion, EventHandler handler, object data)
                : base(APIversion, handler)
            {
                mData = data;
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }
            protected object Data_Internal
            {
                get { return mData; }
                set { if (mData != value) { mData = value; OnElementChanged(); } }
            }
            public abstract override AHandlerElement Clone(EventHandler handler);
            protected abstract void Parse(Stream s);
            public abstract void UnParse(Stream s);

            public bool Equals(TypedData other)
            {
                return mData.Equals(other.mData);
            }

            public string Value
            {
                get { return ValueBuilder; }
            }
        }

        public abstract class TypedData<T> : TypedData where T : IEquatable<T>
        {
            protected TypedData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            protected TypedData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }

            protected TypedData(int APIversion, EventHandler handler, T data)
                : base(APIversion, handler, data)
            {
            }

            public abstract override AHandlerElement Clone(EventHandler handler);

            protected abstract override void Parse(Stream s);
            public abstract override void UnParse(Stream s);
            public T Data
            {
                get { return (T)base.Data_Internal; }
                set { base.Data_Internal = value; }
            }
            public String Value { get { return ValueBuilder; } }

        }

        public class SoundKeyData : TypedData<SoundKey>
        {
            public SoundKeyData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public SoundKeyData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }

            public SoundKeyData(int APIversion, EventHandler handler, SoundKey data)
                : base(APIversion, handler, data)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new SoundKeyData(0, handler, this.Data);
            }

            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                var key = new SoundKey();
                key.Instance = s.ReadUInt64();
                key.Unknown1 = s.ReadUInt32();
                key.Unknown2 = s.ReadUInt32();
                Data = key;
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write((ulong)Data.Instance);
                s.Write((uint)Data.Unknown1);
                s.Write((uint)Data.Unknown2);
            }
        }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public struct SoundKey : IEquatable<SoundKey>
        {
            public SoundKey(ulong instance, uint unknown1, uint unknown2)
                : this()
            {
                Instance = instance;
                Unknown1 = unknown1;
                Unknown2 = unknown2;
            }

            public UInt64 Instance;

            public UInt32 Unknown1;

            public UInt32 Unknown2;

            public bool Equals(SoundKey other)
            {
                return Instance.Equals(other.Instance) && Unknown1.Equals(other.Unknown1) && Unknown2.Equals(other.Unknown2);
            }
            public override string ToString()
            {
                return String.Format("{0:X16}-{1:X8}-{2:X8}", Instance, Unknown1, Unknown2);
            }
        }

        public class ByteData : TypedData<Byte>
        {
            public ByteData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public ByteData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }

            public ByteData(int APIversion, EventHandler handler, byte data)
                : base(APIversion, handler, data)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new ByteData(0, handler, Data);
            }

            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                Data = s.ReadByte();
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(Data);
            }
        }

        public class IntData : TypedData<Int32>
        {
            public IntData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public IntData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }

            public IntData(int APIversion, EventHandler handler, int data)
                : base(APIversion, handler, data)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IntData(0, handler, Data);
            }

            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                Data = s.ReadInt32();
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(Data);
            }
        }

        public class UIntData : TypedData<UInt32>
        {
            public UIntData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public UIntData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }

            public UIntData(int APIversion, EventHandler handler, uint data)
                : base(APIversion, handler, data)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new UIntData(0, handler, Data);
            }

            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                Data = s.ReadUInt32();
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(Data);
            }
        }

        public class FloatData : TypedData<Single>
        {
            public FloatData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public FloatData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }

            public FloatData(int APIversion, EventHandler handler, float data)
                : base(APIversion, handler, data)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new FloatData(0, handler, Data);
            }

            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                Data = s.ReadFloat();
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(Data);
            }
        }

        public class StringData : TypedData<string>
        {
            public StringData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public StringData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }

            public StringData(int APIversion, EventHandler handler, String data)
                : base(APIversion, handler, data)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new StringData(0, handler, Data);
            }


            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                Data = s.ReadPascalString(32, Encoding.BigEndianUnicode, ByteOrder.BigEndian);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.WritePascalString(Data, 32, Encoding.BigEndianUnicode, ByteOrder.BigEndian);
            }
        }

        public enum DataType
        {
            Byte = 0x0001,
            Int = 0x0009,
            UInt = 0x000A,
            Float = 0x00D,
            String = 0x0013,
            SoundKey = 0x00E8
        }

        public abstract class DataBlock : AHandlerElement, IEquatable<DataBlock>
        {
            byte mDataFlags;
            DataType mTypeCode;
            ushort mRepCode;

            private uint mId;
            private TypedData mBlockData;


            public DataBlock(int APIversion, EventHandler handler, DataType typeCode, ushort repCode)
                : base(APIversion, handler)
            {
                mTypeCode = typeCode;
                mRepCode = repCode;
            }

            public DataBlock(int APIversion, EventHandler handler, byte dataFlags, DataType typeCode, ushort repCode, uint id, TypedData blockData)
                : this(APIversion, handler, typeCode, repCode)
            {
                mDataFlags = dataFlags;
                mId = id;
                mBlockData = blockData;
            }

            public DataBlock(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }


            public UInt32 Id
            {
                get { return mId; }
                set { mId = value; if (mId != value) { mId = value; OnElementChanged(); } }
            }
            public ushort RepCode
            {
                get { return mRepCode; }
            }
            public byte DataFlags
            {
                get { return mDataFlags; }
            }
            public DataType TypeCode
            {
                get { return mTypeCode; }
            }
            public TypedData BlockData
            {
                get { return mBlockData; }
                set { mBlockData = value; if (mBlockData != value) { mBlockData = value; OnElementChanged(); } }
            }
            public string Value
            {
                get { return ValueBuilder; }
            }


            private void Parse(Stream s)
            {
                var br = new BinaryStreamWrapper(s);
                br.ByteOrder = ByteOrder.BigEndian;
                mId = br.ReadUInt32();
                mDataFlags = br.ReadByte();
                mTypeCode = (DataType)br.ReadByte();
                mRepCode = br.ReadUInt16();
                if (mRepCode == 0)
                {
                    BlockData = ReadTypedData(0, handler, mTypeCode, s);

                }
                else
                {
                    BlockData = new ListData(0, handler, s, this);
                }

            }
            public void UnParse(Stream s)
            {
                throw new NotImplementedException();
            }
            public static TypedData ReadTypedData(int requestedApiVersion, EventHandler handler, DataType type, Stream stream)
            {
                switch (type)
                {
                    case DataType.Byte: return new ByteData(0, handler, stream);
                    case DataType.Int: return new IntData(0, handler, stream);
                    case DataType.UInt: return new UIntData(0, handler, stream);
                    case DataType.Float: return new FloatData(0, handler, stream);
                    case DataType.SoundKey: return new SoundKeyData(requestedApiVersion, handler, stream);
                    case DataType.String: return new StringData(0, handler, stream);
                    default: Debug.WriteLine("Unknown Data Type: " + type.ToString("X8")); return null;

                }
            }
            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public override List<string> ContentFields
            {
                get
                {
                    var test = GetContentFields(0, GetType());
                    
                    return GetContentFields(0, GetType());
                }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public bool Equals(DataBlock other)
            {
                throw new NotImplementedException();
            }
        }

        [ConstructorParameters(new object[] { DataType.Byte, (byte)0 })]
        public class ByteDataBlock : DataBlock
        {
            public ByteDataBlock(int APIversion, EventHandler handler, DataType typeCode, ushort repCode) : base(APIversion, handler, typeCode, repCode) { }
            public ByteDataBlock(int APIversion, EventHandler handler, byte dataFlags, DataType typeCode, ushort repCode, uint id, TypedData blockData) : base(APIversion, handler, dataFlags, typeCode, repCode, id, blockData) { }
            public ByteDataBlock(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        }
        [ConstructorParameters(new object[] { DataType.Float, (byte)0 })]
        public class FloatDataBlock : DataBlock
        {
            public FloatDataBlock(int APIversion, EventHandler handler, DataType typeCode, ushort repCode) : base(APIversion, handler, typeCode, repCode) { }
            public FloatDataBlock(int APIversion, EventHandler handler, byte dataFlags, DataType typeCode, ushort repCode, uint id, TypedData blockData) : base(APIversion, handler, dataFlags, typeCode, repCode, id, blockData) { }
            public FloatDataBlock(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        }
        [ConstructorParameters(new object[] { DataType.Int, (byte)0 })]
        public class IntDataBlock : DataBlock
        {
            public IntDataBlock(int APIversion, EventHandler handler, DataType typeCode, ushort repCode) : base(APIversion, handler, typeCode, repCode) { }
            public IntDataBlock(int APIversion, EventHandler handler, byte dataFlags, DataType typeCode, ushort repCode, uint id, TypedData blockData) : base(APIversion, handler, dataFlags, typeCode, repCode, id, blockData) { }
            public IntDataBlock(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        }
        [ConstructorParameters(new object[] { DataType.SoundKey, (byte)0 })]
        public class SoundKeyDataBlock : DataBlock
        {
            public SoundKeyDataBlock(int APIversion, EventHandler handler, DataType typeCode, ushort repCode) : base(APIversion, handler, typeCode, repCode) { }
            public SoundKeyDataBlock(int APIversion, EventHandler handler, byte dataFlags, DataType typeCode, ushort repCode, uint id, TypedData blockData) : base(APIversion, handler, dataFlags, typeCode, repCode, id, blockData) { }
            public SoundKeyDataBlock(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        }
        [ConstructorParameters(new object[] { DataType.String, (byte)0 })]
        public class StringKeyDataBlock : DataBlock
        {
            public StringKeyDataBlock(int APIversion, EventHandler handler, DataType typeCode, ushort repCode) : base(APIversion, handler, typeCode, repCode) { }
            public StringKeyDataBlock(int APIversion, EventHandler handler, byte dataFlags, DataType typeCode, ushort repCode, uint id, TypedData blockData) : base(APIversion, handler, dataFlags, typeCode, repCode, id, blockData) { }
            public StringKeyDataBlock(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        }
        public AudioTunerResource(int APIversion, Stream s)
            : base(APIversion, s)
        {
            Parse(s);
        }

        private BlockList mBlocks;
        public BlockList Blocks
        {
            get { return mBlocks; }
            set { mBlocks = value; OnResourceChanged(this, EventArgs.Empty); }
        }

        private void Parse(Stream s)
        {
            Blocks = new BlockList(OnResourceChanged, s);
        }

        private const Int32 kRecommendedApiVersion = 1;
        public override int RecommendedApiVersion
        {
            get { return kRecommendedApiVersion; }
        }
        public string Value
        {
            get { return ValueBuilder; }
        }
        protected override Stream UnParse()
        {
            var stream = new MemoryStream();
            var bw = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);


            return stream;
        }
    }
}
