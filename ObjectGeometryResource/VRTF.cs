using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
namespace s3piwrappers
{
    public class VRTF : ARCOLBlock
    {
        public enum VertexElementUsage : byte
        {
            Position,
            Normal,
            UV,
            BlendIndex,
            BlendWeight,
            Tangent,
            Colour
        }
        public enum VertexElementFormat : byte
        {
            Float1,
            Float2,
            Float3,
            Float4,
            UByte4,
            ColorUByte4,
            Short2,
            Short4,
            UByte4N,
            Short2N,
            Short4N,
            UShort2N,
            UShort4N,
            Dec3N,
            UDec3N,
            Float16_2,
            Float16_4


        }
        public class VertexElementLayoutList : AResource.DependentList<VertexElementLayout>
        {
            public VertexElementLayoutList(EventHandler handler)
                : base(handler)
            {
            }

            public VertexElementLayoutList(EventHandler handler, Stream s, uint count)
                : base(handler)
            {
                Parse(s, count);
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override void WriteCount(Stream s, uint count)
            {

            }
            private void Parse(Stream s, uint count)
            {
                for (int i = 0; i < count; i++)
                {
                    base.Add(CreateElement(s));
                }
            }
            protected override VertexElementLayout CreateElement(Stream s)
            {
                return new VertexElementLayout(0, handler, s);
            }

            protected override void WriteElement(Stream s, VertexElementLayout element)
            {
                element.UnParse(s);
            }
        }
        public class VertexElementLayout : AHandlerElement, IEquatable<VertexElementLayout>
        {
            private VertexElementUsage mUsage;
            private byte mUsageIndex;
            private VertexElementFormat mType;
            private byte mOffset;
            [ElementPriority(1)]
            public VertexElementUsage VertexElementUsage
            {
                get { return mUsage; }
                set { mUsage = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public byte UsageIndex
            {
                get { return mUsageIndex; }
                set { mUsageIndex = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public VertexElementFormat VertexElementFormat
            {
                get { return mType; }
                set { mType = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public byte Offset
            {
                get { return mOffset; }
                set { mOffset = value; OnElementChanged(); }
            }

            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Usage:\t{0}\n", mUsage);
                    sb.AppendFormat("Index:\t0x{0:X2}\n", mUsageIndex);
                    sb.AppendFormat("Format:\t{0}\n", mType);
                    sb.AppendFormat("Offset:\t0x{0:X2}\n", mOffset);
                    return sb.ToString();
                }
            }
            public VertexElementLayout(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public VertexElementLayout(int APIversion, EventHandler handler, VertexElementLayout basis)
                : base(APIversion, handler)
            {
                mUsage = basis.mUsage;
                mUsageIndex = basis.mUsageIndex;
                mType = basis.mType;
                mOffset = basis.mOffset;
            }

            public VertexElementLayout(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mUsage = (VertexElementUsage)br.ReadByte();
                mUsageIndex = br.ReadByte();
                mType = (VertexElementFormat)br.ReadByte();
                mOffset = br.ReadByte();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((byte)mUsage);
                bw.Write((byte)mUsageIndex);
                bw.Write((byte)mType);
                bw.Write((byte)mOffset);
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new VertexElementLayout(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(VertexElementLayout other)
            {
                return base.Equals(other);
            }
        }
        public VRTF(int APIversion, EventHandler handler)
            : base(APIversion, handler, null)
        {
            mVersion = 0x00000002;
        }
        public VRTF(int APIversion, EventHandler handler, VRTF basis)
            : base(APIversion, handler, null)
        {
            Stream s = basis.UnParse();
            s.Position = 0L;
            Parse(s);
        }
        public VRTF(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler, s)
        {
        }


        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { mVersion = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(2)]
        public uint Stride
        {
            get { return mStride; }
            set { mStride = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public bool ExtendedFormat
        {
            get { return mExtendedFormat; }
            set { mExtendedFormat = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        public VertexElementLayoutList Layouts
        {
            get { return mLayouts; }
            set { mLayouts = value; OnRCOLChanged(this, new EventArgs()); }
        }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Stride:\t0x{0:X8}\n", mStride);
                sb.AppendFormat("Extended Format:\t{0}\n", mExtendedFormat);
                if (mLayouts.Count > 0)
                {
                    sb.AppendFormat("Vertex Element Layouts:\n");
                    for (int i = 0; i < mLayouts.Count; i++)
                    {
                        sb.AppendFormat("==VertexElementLayout[{0}]==\n{1}\n", i, mLayouts[i].Value);
                    }
                }
                return sb.ToString();
            }
        }

        private UInt32 mVersion;
        private UInt32 mStride;
        private bool mExtendedFormat;
        private VertexElementLayoutList mLayouts;
        protected override void Parse(Stream s)
        {

            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            mStride = br.ReadUInt32();
            uint count = br.ReadUInt32();
            mExtendedFormat = br.ReadUInt32() > 0 ? true : false;
            mLayouts = new VertexElementLayoutList(handler, s, count);


        }
        public override Stream UnParse()
        {

            if (mLayouts == null) mLayouts = new VertexElementLayoutList(handler);
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);

            bw.Write((uint)FOURCC(Tag));
            bw.Write(mVersion);
            bw.Write(mStride);
            bw.Write(mLayouts.Count);
            bw.Write(mExtendedFormat?1:0);
            mLayouts.UnParse(s);
            return s;
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new VRTF(0, handler, this);
        }

        public override string Tag
        {
            get { return "VRTF"; }
        }

        public override uint ResourceType
        {
            get { return 0x01D0E723; }
        }

        private static bool checking = s3pi.Settings.Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}