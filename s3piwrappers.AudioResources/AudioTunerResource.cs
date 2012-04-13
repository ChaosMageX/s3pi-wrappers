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
            Body_ = 0x947CD0C8,
            Codec = 0xba03d0cd,
            DacOutputSampleRate = 0x6a371e0,
            DefaultToMaster = 0x6b16ae3d,
            Delay = 0x15525baa,
            DistanceCurve = 0xEC361CD1,
            Door_ = 0x80296368,
            DopplerCurve = 0x9bfc37ac,
            Duration = 0xb1f42539,
            EffectiveGain = 0xc2d1f579,
            EmitterType = 0x6610c2bd,
            FadeIn = 0x98ac8996,
            FadeOut = 0xe8ab99b9,
            Feedback = 0xfc047eec,
            Foot_ = 0x0FC82C78,
            Gain = 0x25df0108,
            Groups = 0x7cb81a35,
            HardStop = 0x222b8006,
            HighPass = 0xa26a765f,
            IgnorePauseGain = 0xd1daef42,
            Is3d = 0x12d2a4d4,
            IsLooped = 0x6dd08218,
            IsVirtual = 0xba134192,
            JGym_ = 0xA5ECAFE5,
            Layers = 0xC49B97A3,
            LoopPlaylist = 0x3c10c7b9,
            LowPass = 0x647a7836,
            MaxDistance = 0x3593710,
            MaxGainChange = 0xc742ceb8,
            MinDistance = 0xf616b72,
            MinPerformanceLevel = 0xb2550953,
            Music = 0xEDF036D6,
            MuteAll = 0x899eb157,
            NoteDuration = 0xcd928a9,
            NoteNumber = 0x66ef9aba,
            NoteVelocity = 0xe4dd1818,
            Obj_ = 0xE44A930B,
            OutsideCurve = 0xD9B1D9DF,
            Pan = 0x4a77693a,
            PanDistance = 0x6eb7a717,
            PanLFE = 0xB73EE5C5,
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
            PondEnabledCurve = 0x0774F9BC,
            PositionX = 0x50c5d67,
            PositionY = 0x50c5d66,
            PositionZ = 0x50c5d65,
            PreFade = 0x6257eb2a,
            PrimitiveHashes = 0xe39eb081,
            Primitives = 0x3da0a727,
            Priority = 0x393f7f7d,
            Probability = 0x8fc9308e,
            Radius = 0x2E65B70F,
            RandomPitch = 0x56e7c242,
            RemoveGroups = 0x14043549,
            RingModFreq = 0x703d682b,
            Rolloff = 0x2406a047,
            SampleLength = 0xd24d4aff,
            SamplePosition = 0x893f8476,
            Samples = 0x701ed91e,
            SeekTime = 0x14f9bdf6,
            Send = 0x2fe09c83,
            Sit_ = 0x19FE8F06,
            Skip = 0x310330cc,
            Sound = 0x25238AE2,
            StartDelay = 0x29e8b9f8,
            Sting = 0x687A9528,
            StreamBufferReadSize = 0xb03a6504,
            StreamBufferSize = 0x3c2b4ea4,
            SurroundOn = 0xfe3c587c,
            Symbols = 0x4b2a3424,
            TBox_ = 0xAB87B961,
            TimeInvariantPitch = 0x82beafe0,
            TimeOfDayCurve = 0x8AC7E06E,
            Type = 0xB10F785D,
            VoiceTemplate = 0x544E850B,
            UI = 0x5C770DB7,
            Vo_ = 0x20681CF3,
            Vox = 0x20681CD4,
            WaterEnabledCurve = 0xCE0EB28E,
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
                new BinaryStreamWrapper(s).Write(count, ByteOrder.BigEndian);
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
                return new DataBlock(0, elementHandler, s);
            }
            protected override Type GetElementType(params object[] fields)
            {
                Type t = null;
                var types = typeof(DataBlock).DeclaringType.GetNestedTypes();
                foreach (var type in types)
                {
                    if (!type.IsAbstract && ((ConstructorParametersAttribute)type.GetCustomAttributes(typeof(ConstructorParametersAttribute), true)[0]).parameters[0] == fields[0])
                    {
                        return type;
                    }
                }
                return base.GetElementType(fields);
            }
        }

        public class DataList : DependentList<TypedData>, IEquatable<DataList>
        {
            private readonly DataBlock mOwner;
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

            protected override TypedData CreateElement(Stream s)
            {
                throw new NotImplementedException();
            }

            protected override void WriteElement(Stream s, TypedData element)
            {
                throw new NotImplementedException();
            }
            protected override Type GetElementType(params object[] fields)
            {
                return DataBlock.GetTypedDataClass(mOwner.TypeCode);
            }
            public override bool Add(params object[] fields)
            {

                if (fields.Length == 1 && fields[0] is TypedData)
                {
                    ((IList<TypedData>)this).Add((TypedData)fields[0]);
                    return true;
                }
                else
                {
                    return base.Add(fields);
                }

            }
            public override void Add()
            {
                Add(new object[] {});
            }

            public bool Equals(DataList other)
            {
                return Equals((AHandlerList<TypedData>)other);
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
                if(typeof(T).IsValueType)
                {
                    Data = default(T);
                }
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
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class SoundKey :AHandlerElement, IEquatable<SoundKey>
        {
            private UInt64 mInstance;
            private UInt32 mUnknown1;
            private UInt32 mUnknown2;

            public SoundKey(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public SoundKey(int APIversion, EventHandler handler, Stream stream) : base(APIversion, handler) { this.Parse(stream); }
            public SoundKey(int APIversion, EventHandler handler, SoundKey basis) : this(APIversion, handler,basis.Instance,basis.Unknown1,basis.Unknown2) { }
            public SoundKey(int APIversion, EventHandler handler, ulong instance, uint unknown1, uint unknown2)
                : this(APIversion, handler)
            {
                mInstance = instance;
                mUnknown1 = unknown1;
                mUnknown2 = unknown2;
            }
            public UInt64 Instance
            {
                get { return mInstance; }
                set 
                { 
                    if (mInstance != value)
                    {
                        mInstance = value;OnElementChanged();
                    } 
                }
            }

            public UInt32 Unknown1
            {
                get { return mUnknown1; }
                set 
                { 
                    if (mUnknown1 != value){
                        mUnknown1 = value; OnElementChanged();
                    } 
                }
            }
            public UInt32 Unknown2
            {
                get { return mUnknown2; }
                set
                {
                    if (mUnknown2 != value)
                    {
                        mUnknown2 = value; OnElementChanged();
                    }
                }
            }
            public string Value
            {
                get { return ValueBuilder; }
            }
            private void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.LittleEndian);
                this.mInstance = s.ReadUInt64();
                this.mUnknown1 = s.ReadUInt32();
                this.mUnknown2 = s.ReadUInt32();
            }
            public void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.LittleEndian);
                s.Write((ulong)this.mInstance);
                s.Write((uint)this.mUnknown1);
                s.Write((uint)this.mUnknown2);
            }
            public bool Equals(SoundKey other)
            {
                return Instance.Equals(other.Instance) && Unknown1.Equals(other.Unknown1) && Unknown2.Equals(other.Unknown2);
            }
            public override string ToString()
            {
                return String.Format("{0:X16}-{1:X8}-{2:X8}", Instance, Unknown1, Unknown2);
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion,GetType()); }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new SoundKey(0,handler,this);
            }
        }
        public class SoundKeyData : TypedData<SoundKey>
        {
            public SoundKeyData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                Data = new SoundKey(APIversion,handler);
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
                Data = new SoundKey(0,handler,stream);
            }

            public override void UnParse(Stream stream)
            {
                Data.UnParse(stream);
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
                var s = new BinaryStreamWrapper(stream);
                Data = s.ReadByte();
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream);
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

        public class UnicodeStringData : TypedData<string>
        {
            public UnicodeStringData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public UnicodeStringData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }

            public UnicodeStringData(int APIversion, EventHandler handler, String data)
                : base(APIversion, handler, data)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new UnicodeStringData(0, handler, Data);
            }


            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream);
                Data = s.ReadPascalString(32, Encoding.BigEndianUnicode, ByteOrder.BigEndian);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream);
                s.WritePascalString(Data, 32, Encoding.BigEndianUnicode, ByteOrder.BigEndian);
            }
        }


        public class ASCIIStringData : TypedData<string>
        {
            public ASCIIStringData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public ASCIIStringData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }

            public ASCIIStringData(int APIversion, EventHandler handler, String data)
                : base(APIversion, handler, data)
            {
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new UnicodeStringData(0, handler, Data);
            }


            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream);
                Data = s.ReadPascalString(32, Encoding.ASCII, ByteOrder.BigEndian);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream);
                s.WritePascalString(Data, 32, Encoding.ASCII, ByteOrder.BigEndian);
            }
        }
        public enum DataType
        {
            Byte = 0x0001,
            Int = 0x0009,
            UInt = 0x000A,
            Float = 0x00D,
            ASCIIString = 0x012,
            UnicodeString = 0x0013,
            SoundKey = 0x03E8
        }

        public class DataBlock : AHandlerElement, IEquatable<DataBlock>
        {
            DataType mTypeCode;
            ushort mRepCode;
            private SoundProperty mId;
            private DataList mItems;

            public DataBlock(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }
            public DataBlock(int APIversion, EventHandler handler) 
                : base(APIversion, handler)
            {
                this.mItems  =new DataList(handler,this);
            }
            public DataBlock(int APIversion, EventHandler handler,DataBlock basis) : this(APIversion, handler,basis.TypeCode,basis.RepCode,basis.Id,basis.Items) { }
            public DataBlock(int APIversion, EventHandler handler, DataType typeCode, ushort repCode, SoundProperty id, IEnumerable<TypedData> items)
                : this(APIversion, handler)
            {
                mTypeCode = typeCode;
                mRepCode = repCode;
                mId = id;
                mItems.AddRange(items.Select(x=> x.Clone(handler)).Cast<TypedData>());
            }
            [ElementPriority(0)]
            public SoundProperty Id
            {
                get { return mId; }
                set { if (mId != value) { mId = value; OnElementChanged(); } }
            }
            [ElementPriority(1)]
            public ushort RepCode
            {
                get { return mRepCode; }
                set { if (mRepCode != value) { mRepCode = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public DataType TypeCode
            {
                get { return mTypeCode; }
                set
                {
                    if (mTypeCode != value)
                    {
                        mTypeCode = value;
                        mItems.Clear();
                        OnElementChanged();
                    }
                }
            }
            [ElementPriority(4)]
            public DataList Items
            {
                get { return mItems; }
                set { mItems = value; if (mItems != value) { mItems = value; OnElementChanged(); } }
            }
            [ElementPriority(5)]
            public TypedData Data
            {
                get
                {
                    if (mItems.Count == 0)
                    {
                        mItems.Add();
                    }
                    return mItems[0];
                }
                set
                {
                    mItems.Clear();
                    mItems.Add(value);
                }
            }
            [ElementPriority(3)]
            public Int32 ItemSize
            {
                get
                {
                    if (mRepCode == 0)
                        return mRepCode;
                    switch (TypeCode)
                    {
                        case DataType.ASCIIString:
                        case DataType.UnicodeString:
                        case DataType.SoundKey:
                            return 16;
                        default:
                            return 4;
                    }
                }
            }

            public bool HasTypeData
            {
                get
                {
                    return mRepCode != 0;
                }
            }
            public bool IsList
            {
                get
                {
                    return mRepCode == 0x9C;
                }
            }
            public string Value
            {
                get { return ValueBuilder; }
            }


            private void Parse(Stream s)
            {
                var br = new BinaryStreamWrapper(s);
                br.ByteOrder = ByteOrder.BigEndian;
                mId = (SoundProperty)br.ReadUInt32();

                mTypeCode = (DataType)br.ReadUInt16();
                mRepCode = br.ReadUInt16();
                var data = new List<TypedData>();
                if (!HasTypeData)
                {
                    data.Add(ReadTypedData(0, handler, mTypeCode, s));

                }
                else
                {
                    var count = br.ReadInt32();
                    var itemSize = br.ReadInt32();
                    if (itemSize != this.ItemSize)
                        throw new InvalidDataException(String.Format("Bad item size: Expected {0}, but got {1}", ItemSize, itemSize));
                    for (int i = 0; i < count; i++)
                    {
                        data.Add(ReadTypedData(0, handler, mTypeCode, s));
                    }
                }
                this.mItems = new DataList(handler, data, this);

            }
            public void UnParse(Stream s)
            {
                var br = new BinaryStreamWrapper(s, ByteOrder.BigEndian);
                br.Write((UInt32)mId);
                br.Write((ushort)mTypeCode);
                br.Write(mRepCode);
                if (!HasTypeData && mItems.Count == 0)
                {
                    mItems.Add(new object[] { 0, handler });
                }
                if (HasTypeData)
                {
                    br.Write(mItems.Count);
                    br.Write(this.ItemSize);
                }
                foreach (var block in mItems)
                {
                    block.UnParse(s);
                }
            }
            public static Type GetTypedDataClass(DataType typeCode)
            {
                switch (typeCode)
                {
                    case DataType.Byte: return typeof(ByteData);
                    case DataType.Int: return typeof(IntData);
                    case DataType.UInt: return typeof(UIntData);
                    case DataType.Float: return typeof(FloatData);
                    case DataType.SoundKey: return typeof(SoundKeyData);
                    case DataType.ASCIIString: return typeof(ASCIIStringData);
                    case DataType.UnicodeString: return typeof(UnicodeStringData);
                    default: throw new Exception("Unknown Data Type: " + typeCode.ToString());
                }

            }
            public static TypedData ReadTypedData(int requestedApiVersion, EventHandler handler, DataType typeCode, Stream stream)
            {
                Type t = GetTypedDataClass(typeCode);
                return (TypedData)Activator.CreateInstance(t, requestedApiVersion, handler, stream);
            }
            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public override List<string> ContentFields
            {
                get
                {
                    var fields = GetContentFields(0, GetType());
                    fields.Remove("HasTypeData");
                    fields.Remove("IsList");
                    if (!HasTypeData)
                    {
                        fields.Remove("ItemSize");
                    }
                    if (!IsList)
                    {
                        fields.Remove("Items");
                    }
                    else
                    {
                        fields.Remove("Data");
                    }

                    return fields;
                }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new DataBlock(0,handler,this);
            }

            public bool Equals(DataBlock other)
            {
                return this.Id.Equals(other.Id);
            }
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
        protected override Stream UnParse()
        {
            var stream = new MemoryStream();
            if (mBlocks == null)
                mBlocks = new BlockList(OnResourceChanged);
            mBlocks.UnParse(stream);
            stream.Flush();
            return stream;
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
    }
}
