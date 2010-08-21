using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
namespace s3piwrappers
{
    public class VRTF :ARCOLBlock
    {
        public enum VertexElementUsage : byte
        {
            Position,
            Normal,
            UV,
            Assignment,
            Weight,
            Tangent,
            Unknown
        }
        public enum VertexElementFormat : byte
        {
            F3_3I16_S1U16 = 0x07,
            F2_2I16 = 0x06,
            F3_3I8_S1U8 = 0x05,
            I4 = 0x04

        }
        public class VertexElementLayoutList : AResource.DependentList<VertexElementLayout>
        {
            public VertexElementLayoutList(EventHandler handler) : base(handler)
            {
            }

            public VertexElementLayoutList(EventHandler handler, Stream s, uint count) : base(handler)
            {
                Parse(s, count);
            }

            public override void Add()
            {
                base.Add(new object[] {});
            }
            protected override void WriteCount(Stream s, uint count)
            {
                
            }
            private void Parse(Stream s, uint count)
            {
                for(int i=0;i<count;i++)
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
            private VertexElementUsage mVertexElementUsage;
            private byte mUsageIndex;
            private VertexElementFormat mVertexElementFormat;
            private byte mOffset;
            [ElementPriority(1)]
            public VertexElementUsage VertexElementUsage
            {
                get { return mVertexElementUsage; }
                set { mVertexElementUsage = value; OnElementChanged(); }
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
                get { return mVertexElementFormat; }
                set { mVertexElementFormat = value; OnElementChanged(); }
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
                    sb.AppendFormat("Usage:\t{0}\n", mVertexElementUsage);
                    sb.AppendFormat("Index:\t0x{0:X2}\n", mUsageIndex);
                    sb.AppendFormat("Format:\t{0}\n", mVertexElementFormat);
                    sb.AppendFormat("Offset:\t0x{0:X2}\n", mOffset);
                    return sb.ToString();
                }
            }
            public VertexElementLayout(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
            }
            public VertexElementLayout(int APIversion, EventHandler handler,VertexElementLayout basis)
                : base(APIversion, handler)
            {
                mVertexElementUsage = basis.mVertexElementUsage;
                mUsageIndex = basis.mUsageIndex;
                mVertexElementFormat = basis.mVertexElementFormat;
                mOffset = basis.mOffset;
            }

            public VertexElementLayout(int APIversion, EventHandler handler,Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mVertexElementUsage = (VertexElementUsage)br.ReadByte();
                mUsageIndex = br.ReadByte();
                mVertexElementFormat = (VertexElementFormat) br.ReadByte();
                mOffset = br.ReadByte();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((byte)mVertexElementUsage);
                bw.Write((byte)mUsageIndex);
                bw.Write((byte)mVertexElementFormat);
                bw.Write((byte)mOffset);
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new VertexElementLayout(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion,GetType()); }
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
            : base(APIversion, handler,null)
        {
            mVersion = 0x00000002;
        }
        public VRTF(int APIversion, EventHandler handler,VRTF basis)
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
        public uint VertexSize
        {
            get { return mVertexSize; }
            set { mVertexSize = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public uint Unknown01
        {
            get { return mUnknown01; }
            set { mUnknown01 = value; OnRCOLChanged(this, new EventArgs()); }
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
                sb.AppendFormat("Vertex Size:\t0x{0:X8}\n", mVertexSize);
                sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
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
        private UInt32 mUnknown01;
        private UInt32 mVertexSize;
        private VertexElementLayoutList mLayouts;
        protected override void Parse(Stream s)
        {
            try
            {
                BinaryReader br = new BinaryReader(s);
                string tag = FOURCC(br.ReadUInt32());
                if (checking && tag != Tag)
                {
                    throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
                }
                mVersion = br.ReadUInt32();
                mVertexSize = br.ReadUInt32();
                uint count = br.ReadUInt32();
                mUnknown01 = br.ReadUInt32();
                mLayouts = new VertexElementLayoutList(handler, s, count);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public override Stream UnParse()
        {
            try
            {
                if (mLayouts == null) mLayouts = new VertexElementLayoutList(handler);
                MemoryStream s = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(s);

                bw.Write((uint)FOURCC(Tag));
                bw.Write(mVersion);
                bw.Write(mVertexSize);
                bw.Write(mLayouts.Count);
                bw.Write(mUnknown01);
                mLayouts.UnParse(s);
                return s;
            }
            catch (Exception)
            {
                
                throw;
            }
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