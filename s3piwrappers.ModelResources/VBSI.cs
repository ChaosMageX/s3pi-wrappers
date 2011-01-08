using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using s3pi.Settings;
namespace s3piwrappers
{
    public enum SwizzleCmd : uint
    {
        None = 0x0,
        Swizzle32 = 0x1,
        Swizzle16x2 = 0x2
    }

    public class VBSI : ARCOLBlock
    {
        public class SwizzleEntry : AHandlerElement, IEquatable<SwizzleEntry>
        {
            public SwizzleEntry(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {

            }
            public SwizzleEntry(int apiVersion, EventHandler handler, SwizzleEntry basis)
                : base(apiVersion, handler)
            {
                mCommand = basis.mCommand;
            }
            public SwizzleEntry(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler)
            {
                Parse(s);
            }
            [ElementPriority(1)]
            public SwizzleCmd Command
            {
                get { return mCommand; }
                set { if(mCommand!=value){mCommand = value; OnElementChanged();} }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new SwizzleEntry(0, handler, this);
            }
            private SwizzleCmd mCommand;
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mCommand = (SwizzleCmd)br.ReadUInt32();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((UInt32)mCommand);
            }
            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(SwizzleEntry other)
            {
                return base.Equals(other);
            }
            public override string ToString()
            {
                return mCommand.ToString();
            }
            public string Value { get { return ToString(); } }
        }
        public class SwizzleList : DependentList<SwizzleEntry>
        {
            public SwizzleList(EventHandler handler)
                : base(handler)
            {
            }

            public SwizzleList(EventHandler handler, Stream s, int count)
                : base(handler)
            {
                Parse(s, count);
            }

            public SwizzleList(EventHandler handler, IEnumerable<SwizzleEntry> ilt) : base(handler, ilt) { }

            private void Parse(Stream s, int count)
            {
                for (int i = 0; i < count; i++)
                {
                    ((IList<SwizzleEntry>)this).Add(CreateElement(s));
                }
            }
            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override void WriteCount(Stream s, int count){}
            protected override SwizzleEntry CreateElement(Stream s)
            {
                return new SwizzleEntry(0, handler, s);
            }

            protected override void WriteElement(Stream s, SwizzleEntry element)
            {
                element.UnParse(s);
            }
        }
        public class SegmentList : DependentList<SegmentInfo>
        {
            public SegmentList(EventHandler handler): base(handler){}
            public SegmentList(EventHandler handler, Stream s): base(handler, s){}
            public SegmentList(EventHandler handler, IEnumerable<SegmentInfo> ilt) : base(handler, ilt) { }
            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override SegmentInfo CreateElement(Stream s)
            {
                return new SegmentInfo(0, handler, s);
            }
            protected override void WriteElement(Stream s, SegmentInfo element)
            {
                element.UnParse(s);
            }
        }
        public class SegmentInfo : AHandlerElement, IEquatable<SegmentInfo>
        {
            private uint mVertexSize;
            private uint mVertexCount;
            private uint mByteOffset;
            private SwizzleList mSwizzles;

            public SegmentInfo(int APIversion, EventHandler handler): this(APIversion, handler,0,0,0,new SwizzleList(handler)){}
            public SegmentInfo(int APIversion, EventHandler handler, SegmentInfo basis): this(APIversion, handler,basis.VertexSize,basis.VertexCount,basis.ByteOffset,new SwizzleList(handler,basis.Swizzles)) {}
            public SegmentInfo(int APIversion, EventHandler handler, Stream s): base(APIversion, handler){Parse(s);}
            public SegmentInfo(int APIversion, EventHandler handler, uint vertexSize, uint vertexCount, uint byteOffset, SwizzleList swizzles) : base(APIversion, handler)
            {
                mVertexSize = vertexSize;
                mVertexCount = vertexCount;
                mByteOffset = byteOffset;
                mSwizzles = swizzles;
            }
            [ElementPriority(1)]
            public uint VertexSize
            {
                get { return mVertexSize; }
                set { if (mVertexSize != value) { mVertexSize = value; OnElementChanged();} }
            }
            [ElementPriority(2)]
            public uint VertexCount
            {
                get { return mVertexCount; }
                set { if(mVertexCount!=value){mVertexCount = value; OnElementChanged();} }
            }
            [ElementPriority(3)]
            public uint ByteOffset
            {
                get { return mByteOffset; }
                set { if(mByteOffset!=value){mByteOffset = value; OnElementChanged();} }
            }
            [ElementPriority(4)]
            public SwizzleList Swizzles
            {
                get { return mSwizzles; }
                set { if(mSwizzles!=value){mSwizzles = value; OnElementChanged();} }
            }

            public string Value
            {
                get
                {
                    var sb = new StringBuilder();
                    sb.AppendFormat("VertexSize:\t{0}\n", mVertexSize);
                    sb.AppendFormat("VertexCount:\t{0}\n", mVertexCount);
                    sb.AppendFormat("ByteOffset:\t{0}\n", mByteOffset);
                    if (mSwizzles.Count > 0)
                    {
                        sb.AppendLine("Swizzles:");
                        for (int i = 0; i < mSwizzles.Count; i++)
                        {
                            sb.AppendFormat("[{0}]:{1}\n", i, mSwizzles[i]["Command"]);
                        }
                    }
                    return sb.ToString();
                }
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mVertexSize = br.ReadUInt32();
                mVertexCount = br.ReadUInt32();
                mByteOffset = br.ReadUInt32();
                mSwizzles = new SwizzleList(handler, s, (int)mVertexSize / 4);
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mVertexSize);
                bw.Write(mVertexCount);
                bw.Write(mByteOffset);
                if (mSwizzles == null) mSwizzles = new SwizzleList(handler);
                mSwizzles.UnParse(s);

            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new SegmentInfo(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(SegmentInfo other)
            {
                return base.Equals(other);
            }
        }
        public VBSI(int APIversion, EventHandler handler): base(APIversion, handler, null){}
        public VBSI(int APIversion, EventHandler handler, VBSI basis): this(APIversion, handler, new SegmentList(handler,basis.Segments)){}
        public VBSI(int APIversion, EventHandler handler, Stream s): base(APIversion, handler, s){}
        public VBSI(int APIversion, EventHandler handler, SegmentList segments) : base(APIversion, handler, null)
        {
            mSegments = segments;
        }

        [ElementPriority(1)]
        public SegmentList Segments
        {
            get { return mSegments; }
            set { if(mSegments!=value){mSegments = value; OnRCOLChanged(this, new EventArgs());} }
        }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (mSegments.Count > 0)
                {
                    sb.AppendFormat("Segments:\n");
                    for (int i = 0; i < mSegments.Count; i++)
                    {
                        sb.AppendFormat("==SegmentInfo[{0}]==\n{1}\n", i, mSegments[i].Value);
                    }
                }
                return sb.ToString();
            }
        }

        private SegmentList mSegments;
        protected override void Parse(Stream s)
        {
            mSegments = new SegmentList(handler, s);
        }
        public override Stream UnParse()
        {
            if (mSegments == null) mSegments = new SegmentList(handler);
            MemoryStream s = new MemoryStream();
            mSegments.UnParse(s);
            return s;

        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new VBSI(0, handler, this);
        }

        public override string Tag
        {
            get { return "VBSI"; }
        }

        public override uint ResourceType
        {
            get { return 0xFFFFFFFF; }
        }

        private static bool checking = Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}