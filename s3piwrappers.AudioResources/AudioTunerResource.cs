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
        public enum SoundProperty : uint
        {
            AddGroups = 0xf58ca0be,
            Age = 0x489c3762,
            Aggregate = 0x5a32a0ce,
            AtmosphericCurve = 0xbdc98ce5,
            AtmosphericHiPassCurve = 0x8adce22f,
            AttenuatedGain = 0x6325c989,
            Attenuation = 0x1e4db1eb,
            AttenuationCurve = 0x6e3d22e0,
            Codec = 0xba03d0cd,
            DacOutputSampleRate = 0x6a371e0,
            DefaultToMaster = 0x6b16ae3d,
            Delay = 0x15525baa,
            DopplerCurve = 0x9bfc37ac,
            Duration = 0xb1f42539,
            EffectiveGain = 0xc2d1f579,
            EmitterType = 0x6610c2bd,
            FadeIn = 0x98ac8996,
            FadeOut = 0xe8ab99b9,
            Feedback = 0xfc047eec,
            Gain = 0x25df0108,
            Groups = 0x7cb81a35,
            HardStop = 0x222b8006,
            HighPass = 0xa26a765f,
            IgnorePauseGain = 0xd1daef42,
            Is3d = 0x12d2a4d4,
            IsLooped = 0x6dd08218,
            IsVirtual = 0xba134192,
            LoopPlaylist = 0x3c10c7b9,
            LowPass = 0x647a7836,
            MaxDistance = 0x3593710,
            MaxGainChange = 0xc742ceb8,
            MinDistance = 0xf616b72,
            MinPerformanceLevel = 0xb2550953,
            MuteAll = 0x899eb157,
            NoteDuration = 0xcd928a9,
            NoteNumber = 0x66ef9aba,
            NoteVelocity = 0xe4dd1818,
            Pan = 0x4a77693a,
            PanDistance = 0x6eb7a717,
            PanSize = 0xce78d695,
            PanTwist = 0xa4d2160b,
            Parent = 0x5f6317d5,
            Pause = 0xb85523e5,
            PerformanceLevel = 0x6df5822d,
            PickupIndex = 0xed1f36d5,
            Pitch = 0x71bc3009,
            PitchShiftModifier = 0x779a5ff4,
            Polyphony = 0xbaf7f4f9,
            PolyphonyMode = 0x7f28acc,
            PositionX = 0x50c5d67,
            PositionY = 0x50c5d66,
            PositionZ = 0x50c5d65,
            PreFade = 0x6257eb2a,
            PrimitiveHashes = 0xe39eb081,
            Primitives = 0x3da0a727,
            Priority = 0x393f7f7d,
            Probability = 0x8fc9308e,
            RandomPitch = 0x56e7c242,
            RemoveGroups = 0x14043549,
            RingModFreq = 0x703d682b,
            Rolloff = 0x2406a047,
            SampleLength = 0xd24d4aff,
            SamplePosition = 0x893f8476,
            Samples = 0x701ed91e,
            SeekTime = 0x14f9bdf6,
            Send = 0x2fe09c83,
            Skip = 0x310330cc,
            StartDelay = 0x29e8b9f8,
            StreamBufferReadSize = 0xb03a6504,
            StreamBufferSize = 0x3c2b4ea4,
            SurroundOn = 0xfe3c587c,
            Symbols = 0x4b2a3424,
            TimeInvariantPitch = 0x82beafe0,
            WetLevel = 0x49eb68db,
            WetLevelCurve = 0x173c2710
        }

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

            public SoundProperty Name
            {
                get { return (SoundProperty)mId; }
                set { if (mId != (uint)value) { mId = (uint)value; OnElementChanged(); } }
            }

            public UInt32 Id
            {
                get { return mId; }
                set { if (mId != value) { mId = value; OnElementChanged(); } }
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
