using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
using System.Linq;
namespace s3piwrappers
{
    public enum SwizzleCmd : uint
    {
        None = 0x0,
        Swizzle32 = 0x1,
        Swizzle16x2 = 0x2
    }

    /// <summary>
    /// Unnamed block in MODL/MLOD resources.  Referenced by VBUF, one entry for each group.
    /// </summary>
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
                set { mCommand = value; OnElementChanged(); }
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
        public class SwizzleList : AResource.DependentList<SwizzleEntry>
        {
            public SwizzleList(EventHandler handler)
                : base(handler)
            {
            }

            public SwizzleList(EventHandler handler, Stream s, uint count)
                : base(handler)
            {
                Parse(s, count);
            }

            public SwizzleList(EventHandler handler, IList<SwizzleEntry> ilt) : base(handler, ilt) { }

            private void Parse(Stream s, uint count)
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
            protected override void WriteCount(Stream s, uint count)
            {

            }

            protected override SwizzleEntry CreateElement(Stream s)
            {
                return new SwizzleEntry(0, handler, s);
            }

            protected override void WriteElement(Stream s, SwizzleEntry element)
            {
                element.UnParse(s);
            }
        }
        public class SegmentList : AResource.DependentList<SegmentInfo>
        {
            public SegmentList(EventHandler handler)
                : base(handler)
            {
            }

            public SegmentList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public SegmentList(EventHandler handler, IList<SegmentInfo> ilt) : base(handler, ilt) { }

            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override void WriteCount(Stream s, uint count)
            {

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

            private uint mVertexCount;
            private uint mByteOffset;
            private SwizzleList mSwizzles;
            [ElementPriority(1)]
            public uint VertexCount
            {
                get { return mVertexCount; }
                set { mVertexCount = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public uint ByteOffset
            {
                get { return mByteOffset; }
                set { mByteOffset = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public SwizzleList Swizzles
            {
                get { return mSwizzles; }
                set { mSwizzles = value; OnElementChanged(); }
            }

            public string Value
            {
                get
                {
                    var sb = new StringBuilder();
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
            public SegmentInfo(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mSwizzles = new SwizzleList(handler);
            }
            public SegmentInfo(int APIversion, EventHandler handler, SegmentInfo basis)
                : base(APIversion, handler)
            {
                mVertexCount = basis.mVertexCount;
                mByteOffset = basis.mByteOffset;
                mSwizzles = new SwizzleList(handler, basis.mSwizzles.Select(s => new SwizzleEntry(APIversion, handler, s)).ToList());
            }

            public SegmentInfo(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                uint expectedSize = br.ReadUInt32();
                mVertexCount = br.ReadUInt32();
                mByteOffset = br.ReadUInt32();
                long start = s.Position;
                uint count = expectedSize / 4;
                mSwizzles = new SwizzleList(handler, s, count);
                long end = s.Position;
                uint actualSize = (uint)(end - start);
                if (checking && actualSize != expectedSize)
                    throw new InvalidDataException(String.Format("Invalid SegmentInfo size. Expected {0} but got {1}.", expectedSize, actualSize));
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                uint size = 0;
                long sizeOffset = s.Position;
                bw.Write(size);
                bw.Write(mVertexCount);
                bw.Write(mByteOffset);
                long startOffset = s.Position;
                if (mSwizzles == null) mSwizzles = new SwizzleList(handler);
                mSwizzles.UnParse(s);
                long endOffset = s.Position;
                size = (uint)(endOffset - startOffset);
                s.Seek(sizeOffset, SeekOrigin.Begin);
                bw.Write(size);
                s.Seek(endOffset, SeekOrigin.Begin);

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
        public VBSI(int APIversion, EventHandler handler)
            : base(APIversion, handler, null)
        {
        }
        public VBSI(int APIversion, EventHandler handler, VBSI
            basis)
            : base(APIversion, handler, null)
        {
            mSegments = new SegmentList(handler, basis.mSegments.Select(s => new SegmentInfo(APIversion, handler, s)).ToList());
        }
        public VBSI(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler, s)
        {
        }



        [ElementPriority(1)]
        public SegmentList Segments
        {
            get { return mSegments; }
            set { mSegments = value; OnRCOLChanged(this, new EventArgs()); }
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
            BinaryReader br = new BinaryReader(s);
            mSegments = new SegmentList(handler, s);

        }
        public override Stream UnParse()
        {
            if (mSegments == null) mSegments = new SegmentList(handler);
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);

            bw.Write(mSegments.Count);
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

        private static bool checking = s3pi.Settings.Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}