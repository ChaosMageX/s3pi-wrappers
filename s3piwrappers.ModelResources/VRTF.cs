using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace s3piwrappers
{
    public class VRTF : ARCOLBlock
    {
        public static VRTF CreateDefaultForMesh(MLOD.Mesh mesh)
        {
            return mesh.IsShadowCaster ? CreateDefaultForSunShadow() : CreateDefaultForDropShadow();
        }
        public static VRTF CreateDefaultForSunShadow()
        {
            VRTF v = new VRTF(0, null);
            v.Stride = 8;
            v.Layouts.Add(new ElementLayout(0, null, ElementFormat.UShort4N, 0,
                                                                  ElementUsage.Position, 0));
            return v;
        }
        public static VRTF CreateDefaultForDropShadow()
        {
            VRTF v = new VRTF(0, null);
            v.Stride = VRTF.ByteSizeFromFormat(ElementFormat.UShort4N) + VRTF.ByteSizeFromFormat(ElementFormat.Short4);
            v.Layouts.Add(new ElementLayout(0, null, ElementFormat.UShort4N, 0,
                                                                  ElementUsage.Position, 0));
            v.Layouts.Add(new ElementLayout(0, null, ElementFormat.Short4, (byte)VRTF.ByteSizeFromFormat(ElementFormat.UShort4N),
                                                                  ElementUsage.UV, 0));
            return v;
        }
        public enum ElementUsage : byte
        {
            Position,
            Normal,
            UV,
            BlendIndex,
            BlendWeight,
            Tangent,
            Colour
        }
        public enum ElementFormat : byte
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
        public static int FloatCountFromFormat(ElementFormat f)
        {
            switch (f)
            {
                case ElementFormat.Float1:
                    return 1;
                case ElementFormat.Float2:
                case ElementFormat.UShort2N:
                case ElementFormat.Short2:
                    return 2;
                case ElementFormat.Short4:
                case ElementFormat.Short4N:
                case ElementFormat.ColorUByte4:
                case ElementFormat.UByte4N:
                case ElementFormat.UShort4N:
                case ElementFormat.Float3:
                    return 3;
                case ElementFormat.Float4:
                    return 4;
                default:
                    throw new NotImplementedException();
            }
            
        }
        public static int ByteSizeFromFormat(ElementFormat f)
        {
            switch (f)
            {
                case ElementFormat.Float1:
                case ElementFormat.UByte4:
                case ElementFormat.ColorUByte4:
                case ElementFormat.UByte4N:
                case ElementFormat.UShort2N:
                case ElementFormat.Short2:
                    return 4;
                case ElementFormat.UShort4N:
                case ElementFormat.Float2:
                case ElementFormat.Short4:
                case ElementFormat.Short4N:
                    return 8;
                case ElementFormat.Float3:
                    return 12;
                case ElementFormat.Float4:
                    return 16;
                default:
                    throw new NotImplementedException();
            }
        }
        public class VertexElementLayoutList : DependentList<ElementLayout>
        {
            public VertexElementLayoutList(EventHandler handler)
                : base(handler)
            {
            }

            public VertexElementLayoutList(EventHandler handler, Stream s, int count)
                : base(handler)
            {
                Parse(s, count);
            }

            public VertexElementLayoutList(EventHandler handler, IEnumerable<ElementLayout> ilt) : base(handler, ilt) {}

            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override void WriteCount(Stream s, int count){}
            private void Parse(Stream s, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    ((IList<ElementLayout>)this).Add(CreateElement(s));
                }
            }
            protected override ElementLayout CreateElement(Stream s)
            {
                return new ElementLayout(0, handler, s);
            }

            protected override void WriteElement(Stream s, ElementLayout element)
            {
                element.UnParse(s);
            }
        }
        public class ElementLayout : AHandlerElement, IEquatable<ElementLayout>
        {
            private ElementUsage mUsage;
            private byte mUsageIndex;
            private ElementFormat mFormat;
            private byte mOffset;

            public ElementLayout(int APIversion, EventHandler handler): base(APIversion, handler){}
            public ElementLayout(int APIversion, EventHandler handler, ElementLayout basis): this(APIversion, handler,basis.Format,basis.Offset,basis.Usage,basis.UsageIndex){}
            public ElementLayout(int APIversion, EventHandler handler, Stream s): base(APIversion, handler){Parse(s);}
            public ElementLayout(int APIversion, EventHandler handler, ElementFormat format, byte offset, ElementUsage usage, byte usageIndex) : base(APIversion, handler)
            {
                mFormat = format;
                mOffset = offset;
                mUsage = usage;
                mUsageIndex = usageIndex;
            }

            [ElementPriority(1)]
            public ElementUsage Usage
            {
                get { return mUsage; }
                set { if(mUsage!=value){mUsage = value; OnElementChanged();} }
            }
            [ElementPriority(2)]
            public byte UsageIndex
            {
                get { return mUsageIndex; }
                set { if(mUsageIndex!=value){mUsageIndex = value; OnElementChanged();} }
            }
            [ElementPriority(3)]
            public ElementFormat Format
            {
                get { return mFormat; }
                set { if(mFormat!=value){mFormat = value; OnElementChanged();} }
            }
            [ElementPriority(4)]
            public byte Offset
            {
                get { return mOffset; }
                set { if(mOffset!=value){mOffset = value; OnElementChanged();} }
            }

            public string Value
            {
                get
                {
                    return ValueBuilder;
                    /*
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Usage:\t{0}\n", mUsage);
                    sb.AppendFormat("Index:\t0x{0:X2}\n", mUsageIndex);
                    sb.AppendFormat("Format:\t{0}\n", mFormat);
                    sb.AppendFormat("Offset:\t0x{0:X2}\n", mOffset);
                    return sb.ToString();
                    /**/
                }
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mUsage = (ElementUsage)br.ReadByte();
                mUsageIndex = br.ReadByte();
                mFormat = (ElementFormat)br.ReadByte();
                mOffset = br.ReadByte();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((byte)mUsage);
                bw.Write((byte)mUsageIndex);
                bw.Write((byte)mFormat);
                bw.Write((byte)mOffset);
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new ElementLayout(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(ElementLayout other)
            {
                return base.Equals(other);
            }
        }
        private UInt32 mVersion;
        private Int32 mStride;
        private bool mExtendedFormat;
        private VertexElementLayoutList mLayouts;

        public VRTF(int APIversion, EventHandler handler)
            : base(APIversion, handler, null)
        {
            mVersion = 0x00000002;
        }
        public VRTF(int APIversion, EventHandler handler, VRTF basis): this(APIversion, handler,basis.Version,basis.Stride,new VertexElementLayoutList(handler,basis.Layouts), basis.ExtendedFormat){}
        public VRTF(int APIversion, EventHandler handler, Stream s): base(APIversion, handler, s){}
        public VRTF(int APIversion, EventHandler handler, uint version, int stride, VertexElementLayoutList layouts, bool extendedFormat) : base(APIversion, handler, null)
        {
            mExtendedFormat = extendedFormat;
            mLayouts = layouts;
            mStride = stride;
            mVersion = version;
        }

        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { if(mVersion!=value){mVersion = value; OnRCOLChanged(this, new EventArgs());} }
        }
        [ElementPriority(2)]
        public int Stride
        {
            get { return mStride; }
            set { if(mStride!=value){mStride = value; OnRCOLChanged(this, new EventArgs());} }
        }
        [ElementPriority(3)]
        public bool ExtendedFormat
        {
            get { return mExtendedFormat; }
            set { if(mExtendedFormat!=value){mExtendedFormat = value; OnRCOLChanged(this, new EventArgs());} }
        }
        [ElementPriority(4)]
        public VertexElementLayoutList Layouts
        {
            get { return mLayouts; }
            set { if(mLayouts!=value){mLayouts = value; OnRCOLChanged(this, new EventArgs());} }
        }

        public string Value
        {
            get
            {
                return ValueBuilder;
                /*
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Stride:\t0x{0:X8}\n", mStride);
                sb.AppendFormat("Extended Format:\t{0}\n", mExtendedFormat);
                if (mLayouts.Count > 0)
                {
                    sb.AppendFormat("Vertex Element Layouts:\n");
                    for (int i = 0; i < mLayouts.Count; i++)
                    {
                        sb.AppendFormat("==ElementLayout[{0}]==\n{1}\n", i, mLayouts[i].Value);
                    }
                }
                return sb.ToString();
                /**/
            }
        }

        protected override void Parse(Stream s)
        {

            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            mStride = br.ReadInt32();
            int count = br.ReadInt32();
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