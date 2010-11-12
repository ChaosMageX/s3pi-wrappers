using System.IO;
using s3pi.Interfaces;
using s3pi.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using s3pi.GenericRCOLResource;
using System.Linq;
using System.Collections;
namespace s3piwrappers
{
    public class GEOM : ARCOLBlock
    {
        public enum VertexElementUsage
        {
            Position = 1,
            Normal = 2,
            UV = 3,
            Assignment = 4,
            Weights = 5,
            Tangent = 6,
            Colour = 7,
            VertexId = 10
        }
        public enum VertexElementType
        {
            Float = 1,
            Byte = 2,
            ARGB = 3,
            UInt32 = 4
        }
        public class IndexList : AResource.DependentList<UInt32>
        {
            private byte mFormat = 2;

            public IndexList(EventHandler handler): base(handler){}
            public IndexList(EventHandler handler, Stream s): base(handler, s){}
            public IndexList(EventHandler handler, IList<uint> ilt) : base(handler, ilt) { }
            protected override void Parse(Stream s)
            {
                mFormat = new BinaryReader(s).ReadByte();
                base.Parse(s);
            }
            public override void UnParse(Stream s)
            {
                mFormat = (byte)(Count > 0 ? (this.Max() > ushort.MaxValue ? 4 : 2) : 2);
                new BinaryWriter(s).Write(mFormat);
                base.UnParse(s);
            }
            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override UInt32 CreateElement(Stream s)
            {
                var br = new BinaryReader(s);
                switch (mFormat)
                {
                    case 0x02: return br.ReadUInt16();
                    case 0x04: return br.ReadUInt32();
                    default: throw new Exception("Unknown index format " + mFormat);
                }
            }

            protected override void WriteElement(Stream s, UInt32 element)
            {
                var bw = new BinaryWriter(s);
                switch (mFormat)
                {
                    case 0x02: bw.Write((UInt16)element); break;
                    case 0x04: bw.Write((UInt32)element); break;
                    default: throw new Exception("Unknown index format " + mFormat);
                }
            }
        }

        public class JointList : AResource.SimpleList<UInt32>
        {
            public JointList(EventHandler handler) : base(handler, ReadElement, WriteElement, sFormat) { }
            public JointList(EventHandler handler, Stream s) : base(handler, s, ReadElement, WriteElement, sFormat) { }
            public JointList(EventHandler handler, IList<HandlerElement<UInt32>> ilt) : base(handler, ilt, ReadElement, WriteElement, sFormat) { }
            private static string sFormat = "0x{1:X8}\n";
            static UInt32 ReadElement(Stream s) { return new BinaryReader(s).ReadUInt32(); }
            static void WriteElement(Stream s, UInt32 element) { new BinaryWriter(s).Write(element); }
        }
        public class VertexList : AResource.DependentList<Vertex>
        {
            private GEOM mRoot;
            public VertexList(EventHandler handler, GEOM root) : base(handler) { mRoot = root; }
            public VertexList(EventHandler handler, GEOM root, Stream s, uint count) : this(handler, root) { Parse(s, count); }
            public VertexList(EventHandler handler, GEOM root, IList<Vertex> ilt) : base(handler, ilt) { mRoot = root; }
            public override void Add()
            {
                Add(new object[] { mRoot });
            }
            protected override Vertex CreateElement(Stream s)
            {
                return new Vertex(0, handler, s, mRoot);
            }
            protected override void WriteElement(Stream s, Vertex element)
            {
                element.UnParse(s);
            }
            protected override void WriteCount(Stream s, uint count) { }
            protected override void Parse(Stream s) { throw new NotSupportedException("Use Parse(Stream,uint) instead!"); }
            protected virtual void Parse(Stream s, uint count)
            {
                for (uint i = 0; i < count; i++) ((IList<Vertex>)this).Add(CreateElement(s));
            }
        }
        public class Vertex : AHandlerElement, IEquatable<Vertex>
        {
            private GEOM mRoot;
            private Vector3 mPosition;
            private Vector3 mNormal;
            private Vector2 mUV;
            private UByte4 mAssignments;
            private Vector4 mWeights;
            private Vector3 mTangent;
            private UByte4 mColour;
            private UInt32 mVertexId;
            public Vertex(int APIversion, EventHandler handler, GEOM root)
                : base(APIversion, handler)
            {
                mRoot = root;
                mPosition = new Vector3(0, handler);
                mNormal = new Vector3(0, handler);
                mUV = new Vector2(0, handler);
                mWeights = new Vector4(0, handler);
                mTangent = new Vector3(0, handler);

            }
            public Vertex(int APIversion, EventHandler handler, Vertex basis, GEOM root)
                : base(APIversion, handler)
            {
                mRoot = root;
                mPosition = new Vector3(0, handler, basis.mPosition);
                mNormal = new Vector3(0, handler, basis.mNormal);
                mUV = new Vector2(0, handler, basis.mUV);
                mAssignments = new UByte4(0, handler, basis.mAssignments);
                mWeights = new Vector4(0, handler, basis.mWeights);
                mTangent = new Vector3(0, handler, basis.mTangent);
            }
            public Vertex(int APIversion, EventHandler handler, Stream s, GEOM root) : this(APIversion, handler, root) { Parse(s); }
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var f in ContentFields)
                    {
                        sb.AppendFormat("{0}:\t{1}\n", f, this[f]);
                    }
                    return sb.ToString();
                }
            }
            [ElementPriority(1)]
            public Vector3 Position
            {
                get { return mPosition; }
                set { if (mPosition != value) { mPosition = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public Vector3 Normal
            {
                get { return mNormal; }
                set { if (mNormal != value) { mNormal = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public Vector2 UV
            {
                get { return mUV; }
                set { if (mUV != value) { mUV = value; OnElementChanged(); } }
            }
            [ElementPriority(4)]
            public UByte4 Assignments
            {
                get { return mAssignments; }
                set { if (mAssignments != value) { mAssignments = value; OnElementChanged(); } }
            }
            [ElementPriority(5)]
            public Vector4 Weights
            {
                get { return mWeights; }
                set { if (mWeights != value) { mWeights = value; OnElementChanged(); } }
            }
            [ElementPriority(6)]
            public Vector3 Tangent
            {
                get { return mTangent; }
                set { if (mTangent != value) { mTangent = value; OnElementChanged(); } }
            }
            [ElementPriority(7)]
            public UByte4 Colour
            {
                get { return mColour; }
                set { if (mColour != value) { mColour = value; OnElementChanged(); } }
            }
            [ElementPriority(8)]
            public UInt32 VertexId
            {
                get { return mVertexId; }
                set { if (mVertexId != value) { mVertexId = value; OnElementChanged(); } }
            }

            protected void Parse(Stream s)
            {
                foreach (var format in mRoot.VertexFormat.Elements)
                {
                    switch (format.Usage)
                    {
                        case VertexElementUsage.Position: mPosition = new Vector3(0, handler, s); break;
                        case VertexElementUsage.Normal: mNormal = new Vector3(0, handler, s); break;
                        case VertexElementUsage.UV: mUV = new Vector2(0, handler, s); break;
                        case VertexElementUsage.Assignment: mAssignments = new UByte4(0, handler, s); break;
                        case VertexElementUsage.Weights: mWeights = new Vector4(0, handler, s); break;
                        case VertexElementUsage.Tangent: mTangent = new Vector3(0, handler, s); break;
                        case VertexElementUsage.Colour: mColour = new UByte4(0, handler, s); break;
                        case VertexElementUsage.VertexId: mVertexId = new BinaryReader(s).ReadUInt32(); break;
                    }
                }
            }
            public void UnParse(Stream s)
            {
                foreach (var format in mRoot.VertexFormat.Elements)
                {
                    switch (format.Usage)
                    {
                        case VertexElementUsage.Position: mPosition.UnParse(s); break;
                        case VertexElementUsage.Normal: mNormal.UnParse(s); break;
                        case VertexElementUsage.UV: mUV.UnParse(s); break;
                        case VertexElementUsage.Assignment: mAssignments.UnParse(s); break;
                        case VertexElementUsage.Weights: mWeights.UnParse(s); break;
                        case VertexElementUsage.Tangent: mTangent.UnParse(s); break;
                        case VertexElementUsage.Colour: mColour.UnParse(s); break;
                        case VertexElementUsage.VertexId: new BinaryWriter(s).Write(mVertexId); break;
                    }
                }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Vertex(0, handler, this, mRoot);
            }

            public override List<string> ContentFields
            {
                get
                {
                    List<string> fields = new List<string>();
                    foreach (var format in mRoot.VertexFormat.Elements)
                    {
                        switch (format.Usage)
                        {
                            case VertexElementUsage.Position: fields.Add("Position"); break;
                            case VertexElementUsage.Normal: fields.Add("Normal"); break;
                            case VertexElementUsage.UV: fields.Add("UV"); break;
                            case VertexElementUsage.Assignment: fields.Add("Assignments"); break;
                            case VertexElementUsage.Weights: fields.Add("Weights"); break;
                            case VertexElementUsage.Tangent: fields.Add("Tangent"); break;
                            case VertexElementUsage.Colour: fields.Add("Colour"); break;
                            case VertexElementUsage.VertexId: fields.Add("VertexId"); break;
                        }
                    }
                    return fields;
                }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(Vertex other)
            {
                return base.Equals(other);
            }
        }
        public class VertexElementFormatList : AResource.DependentList<VertexElementFormat>
        {
            public VertexElementFormatList(EventHandler handler): base(handler){}
            public VertexElementFormatList(EventHandler handler, Stream s): base(handler, s){}
            public VertexElementFormatList(EventHandler handler, IList<VertexElementFormat> ilt) : base(handler, ilt) { }
            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override VertexElementFormat CreateElement(Stream s)
            {
                return new VertexElementFormat(0, handler, s);
            }
            protected override void WriteElement(Stream s, VertexElementFormat element)
            {
                element.UnParse(s);
            }
        }
        public class VertexDataFormat : AHandlerElement
        {
            private VertexElementFormatList mElements;
            public VertexDataFormat(int APIversion, EventHandler handler): base(APIversion, handler){mElements = new VertexElementFormatList(handler);}
            public VertexDataFormat(int APIversion, EventHandler handler, VertexDataFormat basis): base(APIversion, handler){mElements = new VertexElementFormatList(handler,basis.Elements);}
            public VertexDataFormat(int APIversion, EventHandler handler, Stream s): base(APIversion, handler){Parse(s);}

            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var ef in mElements) sb.AppendFormat("{0}\n", ef.Value);
                    return sb.ToString();
                }
            }
            [ElementPriority(1)]
            public VertexElementFormatList Elements
            {
                get { return mElements; }
                set { if (mElements != value) { mElements = value; OnElementChanged(); } }
            }

            protected void Parse(Stream s)
            {
                mElements = new VertexElementFormatList(handler, s);

            }
            public void UnParse(Stream s)
            {
                mElements.UnParse(s);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new VertexDataFormat(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

        }
        public class VertexElementFormat : AHandlerElement, IEquatable<VertexElementFormat>, IComparable<VertexElementFormat>
        {
            protected VertexElementUsage mUsage;
            protected VertexElementType mType;
            protected Byte mSize;
            public VertexElementFormat(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public VertexElementFormat(int APIversion, EventHandler handler, VertexElementFormat basis) : this(APIversion, handler, basis.Type, basis.Size, basis.Usage) { }
            public VertexElementFormat(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }
            public VertexElementFormat(int APIversion, EventHandler handler, VertexElementType vertexElementType, byte size, VertexElementUsage vertexElementUsage)
                : base(APIversion, handler)
            {
                mType = vertexElementType;
                mSize = size;
                mUsage = vertexElementUsage;
            }
            [ElementPriority(1)]
            public VertexElementUsage Usage
            {
                get { return mUsage; }
                set { if (mUsage != value) { mUsage = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public VertexElementType Type
            {
                get { return mType; }
                set { if (mType != value) { mType = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public byte Size
            {
                get { return mSize; }
                set { if (mSize != value) { mSize = value; OnElementChanged(); } }
            }
            public string Value { get { return String.Format("{0}:{1}({2:X2})", Usage, Type, Size); } }
            protected void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mUsage = (VertexElementUsage)br.ReadUInt32();
                mType = (VertexElementType)br.ReadUInt32();
                mSize = br.ReadByte();

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((uint)mUsage);
                bw.Write((uint)mType);
                bw.Write((byte)mSize);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new VertexElementFormat(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(VertexElementFormat other)
            {
                return mUsage.Equals(other.Usage);
            }

            public int CompareTo(VertexElementFormat other)
            {
                return mUsage.CompareTo(other.Usage);
            }
        }

        private UInt32 mVersion;
        private UInt32 mShader;
        private byte[] mMaterialBlock;
        private UInt32 mMergeGroup;
        private UInt32 mSortOrder;
        private VertexDataFormat mVertexFormat;
        private VertexList mVertices;
        private IndexList mIndices;
        private UInt32 mSkinControllerIndex;
        private JointList mJoints;
        private AResource.TGIBlockList mReferences;

        public GEOM(int APIversion, EventHandler handler) : base(APIversion, handler, null) { }
        public GEOM(int APIversion, EventHandler handler, GEOM basis) : this(APIversion, handler, new IndexList(handler, basis.mIndices), new JointList(handler, basis.mJoints), basis.Shader, basis.MaterialBlock, basis.mMergeGroup, new AResource.TGIBlockList(handler, basis.mReferences), basis.mSkinControllerIndex, basis.mSortOrder, basis.mVersion, new VertexDataFormat(0, handler, basis.mVertexFormat), new VertexList(handler, basis, basis.mVertices)) { }
        public GEOM(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        public GEOM(int APIversion, EventHandler handler, IndexList indices, JointList joints, uint shader, byte[] material, uint mergeGroup, AResource.TGIBlockList references, uint skinControllerIndex, uint sortOrder, uint version, VertexDataFormat vertexFormat, VertexList vertices)
            : base(APIversion, handler, null)
        {
            mIndices = indices;
            mJoints = joints;
            mShader = shader;
            mMaterialBlock = material;
            mMergeGroup = mergeGroup;
            mReferences = references;
            mSkinControllerIndex = skinControllerIndex;
            mSortOrder = sortOrder;
            mVersion = version;
            mVertexFormat = vertexFormat;
            mVertices = vertices;
        }

        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { if (mVersion != value) { mVersion = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(2)]
        public UInt32 Shader
        {
            get { return mShader; }
            set { if (mShader != value) { mShader = value; OnElementChanged(); } }
        }
        [ElementPriority(3)]
        public byte[] MaterialBlock
        {
            get { return mMaterialBlock; }
            set { if (mMaterialBlock != value) { mMaterialBlock = value; OnElementChanged(); } }
        }
        [ElementPriority(4)]
        public uint MergeGroup
        {
            get { return mMergeGroup; }
            set { if (mMergeGroup != value) { mMergeGroup = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(5)]
        public uint SortOrder
        {
            get { return mSortOrder; }
            set { if (mSortOrder != value) { mSortOrder = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(6)]
        public VertexDataFormat VertexFormat
        {
            get { return mVertexFormat; }
            set { if (mVertexFormat != value) { mVertexFormat = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(6)]
        public VertexList Vertices
        {
            get { return mVertices; }
            set { if (mVertices != value) { mVertices = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(7)]
        public IndexList Indices
        {
            get { return mIndices; }
            set { if (mIndices != value) { mIndices = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(8)]
        [TGIBlockListContentField("References")]
        public UInt32 SkinControllerIndex
        {
            get { return mSkinControllerIndex; }
            set { if (mSkinControllerIndex != value) { mSkinControllerIndex = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(9)]
        public JointList Joints
        {
            get { return mJoints; }
            set { if (mJoints != value) { mJoints = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(10)]
        public AResource.TGIBlockList References
        {
            get { return mReferences; }
            set { if (mReferences != value) { mReferences = value; OnRCOLChanged(this, new EventArgs()); } }
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new GEOM(0, handler, this);
        }
        public String Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Shader:\t0x{0:X8}\n", mShader);
                sb.AppendFormat("MergeGroup:\t0x{0:X8}\n", mMergeGroup);
                sb.AppendFormat("SortOrder:\t0x{0:X8}\n", mSortOrder);
                sb.AppendFormat("Vertex Format:\n{0}\n", mVertexFormat.Value);
                sb.AppendFormat("SkinControllerIndex:\t0x{0:X8}\n", mSkinControllerIndex);
                sb.AppendFormat("Joint References:\n{0}", mJoints.Value);
                sb.AppendFormat("TGI References:\n{0}", mReferences.Value);
                return sb.ToString();
            }
        }

        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (tag != Tag && Settings.Checking)
                throw new InvalidDataException(String.Format(
                    "Invalid Tag read: '{0}'; expected: '{1}'; at 0x{2:X8}", tag, Tag, s.Position));
            mVersion = br.ReadUInt32();
            long tgiOffset = br.ReadUInt32() + s.Position;
            long tgiSize = br.ReadUInt32();
            mShader = br.ReadUInt32();
            mMaterialBlock = mShader == 0 ? new byte[0] :br.ReadBytes(br.ReadInt32());
            mMergeGroup = br.ReadUInt32();
            mSortOrder = br.ReadUInt32();
            uint vertexCount = br.ReadUInt32();
            mVertexFormat = new VertexDataFormat(0, handler, s);
            mVertices = new VertexList(handler, this, s, vertexCount);
            if (br.ReadUInt32() != 0x01 && Settings.Checking)
                throw new InvalidDataException("Expected 0x01 at 0x" + (s.Position - 1).ToString("X8"));
            mIndices = new IndexList(handler, s);
            mSkinControllerIndex = br.ReadUInt32();
            mJoints = new JointList(handler, s);
            mReferences = new AResource.TGIBlockList(handler, s, tgiOffset, tgiSize, false);
        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((uint)FOURCC(Tag));
            bw.Write(mVersion);
            long tgiOffsetPtr = s.Position;
            s.Seek(4, SeekOrigin.Current);
            long startOffset = s.Position;
            s.Seek(4, SeekOrigin.Current);
            if (mMaterialBlock == null) mMaterialBlock = new byte[0];
            bw.Write(mShader);
            if (mShader != 0x00000000) bw.Write(mMaterialBlock);
            bw.Write(mMergeGroup);
            bw.Write(mSortOrder);
            if (mVertices == null) mVertices = new VertexList(handler, this);
            bw.Write(mVertices.Count);
            if (mVertexFormat == null) mVertexFormat = new VertexDataFormat(0, handler);
            mVertexFormat.UnParse(s);
            mVertices.UnParse(s);
            bw.Write(1U);
            if (mIndices == null) mIndices = new IndexList(handler);
            mIndices.UnParse(s);
            bw.Write((uint)mSkinControllerIndex);
            if (mJoints == null) mJoints = new JointList(handler);
            mJoints.UnParse(s);
            if (mReferences == null) mReferences = new AResource.TGIBlockList(handler, false);
            long tgiOffset = s.Position;
            mReferences.UnParse(s);
            long endOffset = s.Position;
            long tgiSize = endOffset - tgiOffset;
            s.Seek(tgiOffsetPtr, SeekOrigin.Begin);
            bw.Write((uint)(tgiOffset - startOffset));
            bw.Write((uint)tgiSize);
            s.Seek(endOffset, SeekOrigin.Begin);
            return s;

        }
        public override uint ResourceType
        {
            get { return 0x015A1849; }
        }

        public override string Tag
        {
            get { return "GEOM"; }
        }

        private const int kRecommendedApiVersion = 1;
    }
}