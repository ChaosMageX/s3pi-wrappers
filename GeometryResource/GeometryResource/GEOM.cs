using System.IO;
using s3pi.Interfaces;
using s3pi.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using s3pi.GenericRCOLResource;
using System.Drawing;
namespace s3piwrappers
{
    public class MTNF : AHandlerElement
    {
        enum DataType
        {
            Float =0x00000001,
            UInt32 = 0x00000004,
        }
        public class ShaderKey : AHandlerElement
        {
            public ShaderKey(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public ShaderKey(int APIversion, EventHandler handler, ShaderKey basis)
                : base(APIversion, handler)
            {
            }
            public ShaderKey(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
            }

            private MATD.FieldType mType;

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new ShaderKey(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

        }

        private const String Tag = "MTNF";
        public MTNF(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
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
        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (tag != Tag)
                throw new InvalidDataException(String.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{2:X8}", tag, Tag, s.Position));
            mUnknown01 = br.ReadUInt32();
            UInt32 dataLength = br.ReadUInt32();
            UInt32 count = br.ReadUInt32();

        }
        public void UnParse(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((uint)FOURCC(Tag));
            bw.Write(mUnknown01);
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new MTNF(0, handler, this);
        }

        public override List<string> ContentFields
        {
            get { return GetContentFields(0, GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
        private const int kRecommendedApiVersion = 1;
        private static bool checking = Settings.Checking;
    }
    public class GEOM : ARCOLBlock
    {
        public enum VertexElementType
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
        public enum VertexDataType
        {
            Float = 1,
            Byte = 2,
            ARGB = 3,
            UInt32 = 4
        }
        public class MaterialBlock : AHandlerElement
        {
            private ShaderName mShader;
            private byte[] mMaterialNameFile;
            public MaterialBlock(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public MaterialBlock(int APIversion, EventHandler handler, MaterialBlock basis)
                : base(APIversion, handler)
            {
            }
            public MaterialBlock(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            public override string ToString()
            {
                return mShader.ToString();
            }
            public string Value
            {
                get { return ToString(); }
            }
            [ElementPriority(1)]
            public ShaderName Shader
            {
                get { return mShader; }
                set { mShader = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public byte[] MaterialNameFile
            {
                get { return mMaterialNameFile; }
                set { mMaterialNameFile = value; OnElementChanged(); }
            }

            protected void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mShader = (ShaderName)br.ReadUInt32();
                if (mShader != ShaderName.None)
                {
                    int len = br.ReadInt32();
                    mMaterialNameFile = br.ReadBytes(len);
                }
                else
                {
                    mMaterialNameFile = new byte[0];
                }

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((UInt32)mShader);
                if (mMaterialNameFile == null) mMaterialNameFile = new byte[0];
                if (mShader != ShaderName.None)
                {
                    bw.Write(mMaterialNameFile.Length);
                    bw.Write(mMaterialNameFile);
                }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new MaterialBlock(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        }
        public class IndexDataList : AResource.DependentList<IndexData>
        {

            public IndexDataList(EventHandler handler)
                : base(handler)
            {
            }

            public IndexDataList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }
            protected override uint ReadCount(Stream s)
            {
                return base.ReadCount(s) / 3;
            }
            protected override void WriteCount(Stream s, uint count)
            {
                base.WriteCount(s, count * 3);
            }
            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override IndexData CreateElement(Stream s)
            {
                return new IndexData(0, handler, s);
            }

            protected override void WriteElement(Stream s, IndexData element)
            {
                element.UnParse(s);
            }
        }
        public class IndexData : AHandlerElement, IEquatable<IndexData>
        {
            public IndexData(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public IndexData(int APIversion, EventHandler handler, IndexData basis)
                : base(APIversion, handler)
            {
                mIndex0 = basis.mIndex0;
                mIndex1 = basis.mIndex1;
                mIndex2 = basis.mIndex2;
            }
            public IndexData(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }

            private UInt16 mIndex0;
            private UInt16 mIndex1;
            private UInt16 mIndex2;
            public string Value { get { return ToString(); } }
            public override string ToString()
            {
                return String.Format("[{0:X4},{1:X4},{2:X4}]", mIndex0, mIndex1, mIndex2);
            }
            [ElementPriority(1)]
            public ushort Index0
            {
                get { return mIndex0; }
                set { mIndex0 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public ushort Index1
            {
                get { return mIndex1; }
                set { mIndex1 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public ushort Index2
            {
                get { return mIndex2; }
                set { mIndex2 = value; OnElementChanged(); }
            }

            protected void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mIndex0 = br.ReadUInt16();
                mIndex1 = br.ReadUInt16();
                mIndex2 = br.ReadUInt16();

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mIndex0);
                bw.Write(mIndex1);
                bw.Write(mIndex2);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IndexData(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(IndexData other)
            {
                return base.Equals(other);
            }
        }
        public class JointReferenceList : AResource.DependentList<JointReference>
        {

            public JointReferenceList(EventHandler handler)
                : base(handler)
            {
            }

            public JointReferenceList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override JointReference CreateElement(Stream s)
            {
                return new JointReference(0, handler, s);
            }

            protected override void WriteElement(Stream s, JointReference element)
            {
                element.UnParse(s);
            }
        }
        public class JointReference : AHandlerElement, IEquatable<JointReference>
        {
            public JointReference(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public JointReference(int APIversion, EventHandler handler, JointReference basis)
                : base(APIversion, handler)
            {
                mJointName = basis.mJointName;
            }
            public JointReference(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }

            private UInt32 mJointName;
            public String Value { get { return ToString(); } }
            public override string ToString()
            {
                return String.Format("0x{0:X8}", mJointName);
            }
            [ElementPriority(1)]
            public UInt32 JointName
            {
                get { return mJointName; }
                set { mJointName = value; OnElementChanged(); }
            }


            protected void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mJointName = br.ReadUInt32();

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mJointName);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new JointReference(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(JointReference other)
            {
                return mJointName.Equals(other.mJointName);
            }
        }
        public class IndexElementFormatList : AResource.DependentList<IndexElementFormat>
        {

            public IndexElementFormatList(EventHandler handler)
                : base(handler)
            {
            }

            public IndexElementFormatList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override IndexElementFormat CreateElement(Stream s)
            {
                return new IndexElementFormat(0, handler, s);
            }

            protected override void WriteElement(Stream s, IndexElementFormat element)
            {
                element.UnParse(s);
            }
        }
        public class IndexElementFormat : AHandlerElement, IEquatable<IndexElementFormat>
        {
            public IndexElementFormat(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public IndexElementFormat(int APIversion, EventHandler handler, IndexElementFormat basis)
                : base(APIversion, handler)
            {

            }
            public IndexElementFormat(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            public override string ToString()
            {
                return String.Format("IndexSize: 0x{0:X2}", mIndexSize);
            }

            public String Value { get { return ToString(); } }
            private byte mIndexSize;
            [ElementPriority(1)]
            public byte IndexSize
            {
                get { return mIndexSize; }
                set { mIndexSize = value; OnElementChanged(); }
            }

            protected void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mIndexSize = br.ReadByte();

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mIndexSize);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IndexElementFormat(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(IndexElementFormat other)
            {
                return mIndexSize.Equals(other.mIndexSize);
            }
        }
        public class VertexElementList : AHandlerList<VertexElement>
        {
            public VertexElementList(EventHandler handler) : base(handler) { }
        }
        public class VertexTangent : VertexElement
        {
            private float mX;
            private float mY;
            private float mZ;

            public VertexTangent(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public VertexTangent(int APIversion, EventHandler handler, VertexTangent basis)
                : base(APIversion, handler, basis)
            {
                mX = basis.mX;
                mY = basis.mY;
                mZ = basis.mZ;
            }

            public VertexTangent(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }
            public string Value { get { return ToString(); } }
            public override string ToString()
            {
                return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]", mX, mY, mZ);
            }
            [ElementPriority(1)]
            public float X
            {
                get { return mX; }
                set { mX = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Y
            {
                get { return mY; }
                set { mY = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public float Z
            {
                get { return mZ; }
                set { mZ = value; OnElementChanged(); }
            }

            public override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mX = br.ReadSingle();
                mY = br.ReadSingle();
                mZ = br.ReadSingle();
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mX);
                bw.Write(mY);
                bw.Write(mZ);
            }
        }
        public class VertexNormal : VertexElement
        {
            private float mX;
            private float mY;
            private float mZ;

            public VertexNormal(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public VertexNormal(int APIversion, EventHandler handler, VertexNormal basis)
                : base(APIversion, handler, basis)
            {
                mX = basis.mX;
                mY = basis.mY;
                mZ = basis.mZ;
            }

            public VertexNormal(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }
            public string Value { get { return ToString(); } }
            public override string ToString()
            {
                return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]", mX, mY, mZ);
            }
            [ElementPriority(1)]
            public float X
            {
                get { return mX; }
                set { mX = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Y
            {
                get { return mY; }
                set { mY = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public float Z
            {
                get { return mZ; }
                set { mZ = value; OnElementChanged(); }
            }

            public override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mX = br.ReadSingle();
                mY = br.ReadSingle();
                mZ = br.ReadSingle();
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mX);
                bw.Write(mY);
                bw.Write(mZ);
            }
        }
        public class VertexPosition : VertexElement
        {
            private float mX;
            private float mY;
            private float mZ;

            public VertexPosition(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public VertexPosition(int APIversion, EventHandler handler, VertexPosition basis)
                : base(APIversion, handler, basis)
            {
                mX = basis.mX;
                mY = basis.mY;
                mZ = basis.mZ;
            }

            public VertexPosition(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }
            public string Value { get { return ToString(); } }
            public override string ToString()
            {
                return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]", mX, mY, mZ);
            }
            [ElementPriority(1)]
            public float X
            {
                get { return mX; }
                set { mX = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Y
            {
                get { return mY; }
                set { mY = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public float Z
            {
                get { return mZ; }
                set { mZ = value; OnElementChanged(); }
            }

            public override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mX = br.ReadSingle();
                mY = br.ReadSingle();
                mZ = br.ReadSingle();
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mX);
                bw.Write(mY);
                bw.Write(mZ);
            }
        }
        public class VertexWeights : VertexElement
        {
            private float mWeight0;
            private float mWeight1;
            private float mWeight2;
            private float mWeight3;

            public VertexWeights(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public VertexWeights(int APIversion, EventHandler handler, VertexWeights basis)
                : base(APIversion, handler, basis)
            {
                mWeight0 = basis.mWeight0;
                mWeight1 = basis.mWeight1;
                mWeight2 = basis.mWeight2;
                mWeight3 = basis.mWeight3;
            }

            public VertexWeights(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }
            public string Value { get { return ToString(); } }
            public override string ToString()
            {
                return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]", mWeight0, mWeight1, mWeight2, mWeight3);
            }
            [ElementPriority(1)]
            public float Weight0
            {
                get { return mWeight0; }
                set { mWeight0 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float Weight1
            {
                get { return mWeight1; }
                set { mWeight1 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public float Weight2
            {
                get { return mWeight2; }
                set { mWeight2 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public float Weight3
            {
                get { return mWeight3; }
                set { mWeight3 = value; OnElementChanged(); }
            }

            public override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mWeight0 = br.ReadSingle();
                mWeight1 = br.ReadSingle();
                mWeight2 = br.ReadSingle();
                mWeight3 = br.ReadSingle();
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mWeight0);
                bw.Write(mWeight1);
                bw.Write(mWeight2);
                bw.Write(mWeight3);
            }
        }
        public class VertexAssignment : VertexElement
        {
            private byte mAssignment0;
            private byte mAssignment1;
            private byte mAssignment2;
            private byte mAssignment3;

            public VertexAssignment(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public VertexAssignment(int APIversion, EventHandler handler, VertexAssignment basis)
                : base(APIversion, handler, basis)
            {
                mAssignment0 = basis.mAssignment0;
                mAssignment1 = basis.mAssignment1;
                mAssignment2 = basis.mAssignment2;
                mAssignment3 = basis.mAssignment3;
            }

            public VertexAssignment(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }
            public string Value { get { return ToString(); } }
            public override string ToString()
            {
                return String.Format("[{0:X2},{1:X2},{2:X2},{3:X2}]", mAssignment0, mAssignment1, mAssignment2, mAssignment3);
            }
            [ElementPriority(1)]
            public byte Assignment0
            {
                get { return mAssignment0; }
                set { mAssignment0 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public byte Assignment1
            {
                get { return mAssignment1; }
                set { mAssignment1 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public byte Assignment2
            {
                get { return mAssignment2; }
                set { mAssignment2 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public byte Assignment3
            {
                get { return mAssignment3; }
                set { mAssignment3 = value; OnElementChanged(); }
            }

            public override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mAssignment0 = br.ReadByte();
                mAssignment1 = br.ReadByte();
                mAssignment2 = br.ReadByte();
                mAssignment3 = br.ReadByte();
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mAssignment0);
                bw.Write(mAssignment1);
                bw.Write(mAssignment2);
                bw.Write(mAssignment3);
            }
        }
        public class VertexColour : VertexElement
        {
            private Int32 mColour;

            public VertexColour(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public VertexColour(int APIversion, EventHandler handler, VertexColour basis)
                : base(APIversion, handler, basis)
            {
                mColour = basis.mColour;
            }

            public VertexColour(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }
            public string Value { get { return ToString(); } }
            public override string ToString()
            {
                return Colour.ToString();
            }
            [ElementPriority(1)]
            public Color Colour
            {
                get { return Color.FromArgb(mColour); }
                set { mColour = value.ToArgb(); OnElementChanged(); }
            }
            public override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mColour = br.ReadInt32();
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mColour);
            }
        }
        public class VertexId : VertexElement
        {
            private UInt32 mId;

            public VertexId(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public VertexId(int APIversion, EventHandler handler, VertexId basis)
                : base(APIversion, handler, basis)
            {
                mId = basis.mId;
            }

            public VertexId(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }
            public string Value { get { return ToString(); } }
            public override string ToString()
            {
                return String.Format("0x{0:X8}", mId);
            }
            [ElementPriority(1)]
            public UInt32 Id
            {
                get { return mId; }
                set { mId = value; OnElementChanged(); }
            }


            public override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mId = br.ReadUInt32();
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mId);
            }
        }
        public class VertexUV : VertexElement
        {
            private float mU;
            private float mV;

            public VertexUV(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public VertexUV(int APIversion, EventHandler handler, VertexUV basis)
                : base(APIversion, handler, basis)
            {
                mU = basis.mU;
                mV = basis.mV;
            }

            public VertexUV(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler, s)
            {
            }
            public string Value { get { return ToString(); } }
            public override string ToString()
            {
                return String.Format("[{0,8:0.00000},{1,8:0.00000}]", mU, mV);
            }
            [ElementPriority(1)]
            public float U
            {
                get { return mU; }
                set { mU = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public float V
            {
                get { return mV; }
                set { mV = value; OnElementChanged(); }
            }
            public override void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mU = br.ReadSingle();
                mV = br.ReadSingle();
            }

            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mU);
                bw.Write(mV);
            }
        }
        public abstract class VertexElement : AHandlerElement, IEquatable<VertexElement>
        {
            public VertexElement(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public VertexElement(int APIversion, EventHandler handler, VertexElement basis)
                : base(APIversion, handler)
            {

            }
            public VertexElement(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                if (s != null) Parse(s);
            }
            public abstract void Parse(Stream s);

            public abstract void UnParse(Stream s);

            public override AHandlerElement Clone(EventHandler handler)
            {
                return (AHandlerElement)Activator.CreateInstance(GetType(), new object[] { 0, handler, this });
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(VertexElement other)
            {
                return base.Equals(other);
            }
        }
        public class VertexDataList : AHandlerList<VertexData>, IGenericAdd
        {
            private GEOM mRoot;
            public VertexDataList(EventHandler handler, GEOM root)
                : base(handler)
            {
                mRoot = root;
            }

            public void Add()
            {
                base.Add(new VertexData(0, handler, mRoot));
            }


            public bool Add(params object[] fields)
            {
                if (fields.Length == 1 && typeof(VertexData).IsAssignableFrom(fields[0].GetType()))
                {
                    base.Add((VertexData)fields[0]);
                    return true;
                }
                return false;
            }
        }
        public class VertexData : AHandlerElement, IEquatable<VertexData>
        {
            private GEOM mRoot;
            private VertexPosition mPosition;
            private VertexNormal mNormal;
            private VertexUV mUV;
            private VertexAssignment mAssignments;
            private VertexWeights mWeights;
            private VertexTangent mTangent;
            private VertexColour mColour;
            private VertexId mVertexId;
            public VertexData(int APIversion, EventHandler handler, GEOM root)
                : base(APIversion, handler)
            {
                mRoot = root;
                mPosition = new VertexPosition(0, handler);
                mNormal = new VertexNormal(0, handler);
                mUV = new VertexUV(0, handler);
                mAssignments = new VertexAssignment(0, handler);
                mWeights = new VertexWeights(0, handler);
                mTangent = new VertexTangent(0, handler);
                mColour = new VertexColour(0, handler);
                mVertexId = new VertexId(0, handler);

            }
            public VertexData(int APIversion, EventHandler handler, VertexData basis)
                : base(APIversion, handler)
            {
            }
            public VertexData(int APIversion, EventHandler handler, Stream s, GEOM root)
                : this(APIversion, handler, root)
            {
                Parse(s);
            }
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
            public VertexPosition Position
            {
                get { return mPosition; }
                set { mPosition = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public VertexNormal Normal
            {
                get { return mNormal; }
                set { mNormal = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public VertexUV UV
            {
                get { return mUV; }
                set { mUV = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public VertexAssignment Assignments
            {
                get { return mAssignments; }
                set { mAssignments = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public VertexWeights Weights
            {
                get { return mWeights; }
                set { mWeights = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public VertexTangent Tangent
            {
                get { return mTangent; }
                set { mTangent = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public VertexColour Colour
            {
                get { return mColour; }
                set { mColour = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public VertexId VertexId
            {
                get { return mVertexId; }
                set { mVertexId = value; OnElementChanged(); }
            }

            protected void Parse(Stream s)
            {
                foreach (var format in mRoot.VertexFormat.Elements)
                {
                    switch (format.VertexElementType)
                    {
                        case VertexElementType.Position: mPosition.Parse(s); break;
                        case VertexElementType.Normal: mNormal.Parse(s); break;
                        case VertexElementType.UV: mUV.Parse(s); break;
                        case VertexElementType.Assignment: mAssignments.Parse(s); break;
                        case VertexElementType.Weights: mWeights.Parse(s); break;
                        case VertexElementType.Tangent: mTangent.Parse(s); break;
                        case VertexElementType.Colour: mColour.Parse(s); break;
                        case VertexElementType.VertexId: mVertexId.Parse(s); break;
                    }
                }
            }
            public void UnParse(Stream s)
            {
                foreach (var format in mRoot.VertexFormat.Elements)
                {
                    switch (format.VertexElementType)
                    {
                        case VertexElementType.Position: mPosition.UnParse(s); break;
                        case VertexElementType.Normal: mNormal.UnParse(s); break;
                        case VertexElementType.UV: mUV.UnParse(s); break;
                        case VertexElementType.Assignment: mAssignments.UnParse(s); break;
                        case VertexElementType.Weights: mWeights.UnParse(s); break;
                        case VertexElementType.Tangent: mTangent.UnParse(s); break;
                        case VertexElementType.Colour: mColour.UnParse(s); break;
                        case VertexElementType.VertexId: mVertexId.UnParse(s); break;
                    }
                }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new VertexData(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get
                {
                    List<string> fields = new List<string>();
                    foreach (var format in mRoot.VertexFormat.Elements)
                    {
                        switch (format.VertexElementType)
                        {
                            case VertexElementType.Position: fields.Add("Position"); break;
                            case VertexElementType.Normal: fields.Add("Normal"); break;
                            case VertexElementType.UV: fields.Add("UV"); break;
                            case VertexElementType.Assignment: fields.Add("Assignments"); break;
                            case VertexElementType.Weights: fields.Add("Weights"); break;
                            case VertexElementType.Tangent: fields.Add("Tangent"); break;
                            case VertexElementType.Colour: fields.Add("Colour"); break;
                            case VertexElementType.VertexId: fields.Add("VertexId"); break;
                        }
                    }
                    return fields;
                }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(VertexData other)
            {
                return base.Equals(other);
            }
        }
        public class VertexElementFormatList : AResource.DependentList<VertexElementFormat>
        {
            public VertexElementFormatList(EventHandler handler)
                : base(handler)
            {
            }

            public VertexElementFormatList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

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
            public VertexDataFormat(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mElements = new VertexElementFormatList(handler);
            }
            public VertexDataFormat(int APIversion, EventHandler handler, VertexDataFormat basis)
                : base(APIversion, handler)
            {
                mElements = new VertexElementFormatList(handler);
                foreach (var e in basis.mElements) mElements.Add(e.Clone(handler));
            }
            public VertexDataFormat(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }

            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var ef in mElements) sb.AppendFormat("{0}\n", ef.Value);
                    return sb.ToString();
                }
            }
            private VertexElementFormatList mElements;
            [ElementPriority(1)]
            public VertexElementFormatList Elements
            {
                get { return mElements; }
                set { mElements = value; OnElementChanged(); }
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
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

        }
        public class IndexDataFormat : AHandlerElement
        {
            public IndexDataFormat(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mElements = new IndexElementFormatList(handler);
            }
            public IndexDataFormat(int APIversion, EventHandler handler, IndexDataFormat basis)
                : base(APIversion, handler)
            {
                mElements = new IndexElementFormatList(handler);
                foreach (var e in basis.mElements) mElements.Add(e.Clone(handler));
            }
            public IndexDataFormat(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var ef in mElements) sb.AppendFormat("{0}\n", ef.Value);
                    return sb.ToString();
                }
            }
            private IndexElementFormatList mElements;
            [ElementPriority(1)]
            public IndexElementFormatList Elements
            {
                get { return mElements; }
                set { mElements = value; OnElementChanged(); }
            }

            protected void Parse(Stream s)
            {
                mElements = new IndexElementFormatList(handler, s);

            }
            public void UnParse(Stream s)
            {
                mElements.UnParse(s);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IndexDataFormat(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

        }
        public class VertexElementFormat : AHandlerElement, IEquatable<VertexElementFormat>, IComparable<VertexElementFormat>
        {
            protected VertexElementType mVertexElementType;
            protected VertexDataType mVertexDataType;
            protected Byte mVertexElementSize;
            public VertexElementFormat(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public VertexElementFormat(int APIversion, EventHandler handler, VertexElementFormat basis)
                : base(APIversion, handler)
            {

            }
            public VertexElementFormat(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            public string Value
            {
                get { return String.Format("{0}:{1}({2:X2})", mVertexElementType, mVertexDataType, mVertexElementSize); }
            }
            [ElementPriority(1)]
            public VertexElementType VertexElementType
            {
                get { return mVertexElementType; }
                set { mVertexElementType = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public VertexDataType VertexDataType
            {
                get { return mVertexDataType; }
                set { mVertexDataType = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public byte VertexElementSize
            {
                get { return mVertexElementSize; }
                set { mVertexElementSize = value; OnElementChanged(); }
            }

            protected void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mVertexElementType = (VertexElementType)br.ReadUInt32();
                mVertexDataType = (VertexDataType)br.ReadUInt32();
                mVertexElementSize = br.ReadByte();

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((uint)mVertexElementType);
                bw.Write((uint)mVertexDataType);
                bw.Write((byte)mVertexElementSize);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new VertexElementFormat(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(VertexElementFormat other)
            {
                return mVertexElementType.Equals(mVertexElementType);
            }

            public int CompareTo(VertexElementFormat other)
            {
                return mVertexElementType.CompareTo(other.mVertexElementType);
            }
        }

        public GEOM(int APIversion, EventHandler handler)
            : base(APIversion, handler, null)
        {
        }
        public GEOM(int APIversion, EventHandler handler, GEOM basis)
            : base(APIversion, handler, null)
        {
            Stream s = basis.UnParse();
            s.Position = 0L;
            Parse(s);
        }
        public GEOM(int APIversion, EventHandler handler, Stream s)
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
        public MaterialBlock Material
        {
            get { return mMaterial; }
            set { mMaterial = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public uint Unknown01
        {
            get { return mUnknown01; }
            set { mUnknown01 = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        public uint Unknown02
        {
            get { return mUnknown02; }
            set { mUnknown02 = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(5)]
        public VertexDataFormat VertexFormat
        {
            get { return mVertexFormat; }
            set { mVertexFormat = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(6)]
        public VertexDataList VertexDataEntries
        {
            get { return mVertexDataEntries; }
            set { mVertexDataEntries = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(7)]
        public IndexDataFormat IndexFormat
        {
            get { return mIndexDataFormat; }
            set { mIndexDataFormat = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(8)]
        public IndexDataList IndexDataEntries
        {
            get { return mIndexDataEntries; }
            set { mIndexDataEntries = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(9)]
        public UInt32 Unknown03
        {
            get { return mUnknown03; }
            set { mUnknown03 = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(10)]
        public JointReferenceList JointReferences
        {
            get { return mJointReferences; }
            set { mJointReferences = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(11)]
        public AResource.TGIBlockList References
        {
            get { return mReferences; }
            set { mReferences = value; OnRCOLChanged(this, new EventArgs()); }
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
                sb.AppendFormat("Material:\n{0}\n", mMaterial.Value);
                sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                sb.AppendFormat("Unknown02:\t0x{0:X8}\n", mUnknown02);
                sb.AppendFormat("\nVertex Format:\n{0}\n", mVertexFormat.Value);
                if (mVertexDataEntries.Count > 0)
                {
                    sb.AppendFormat("Vertex Data:\n");
                    for (int i = 0; i < mVertexDataEntries.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}]\n{1}\n", i, mVertexDataEntries[i].Value);
                    }
                }

                sb.AppendFormat("Index Format:\n{0}\n", mIndexDataFormat.Value);
                if (mIndexDataEntries.Count > 0)
                {
                    sb.AppendFormat("Index Data:\n");
                    for (int i = 0; i < mIndexDataEntries.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}]\n{1}\n", i, mIndexDataEntries[i].Value);
                    }
                }
                sb.AppendFormat("Unknown03:\t0x{0:X8}\n", mUnknown03);
                if (mJointReferences.Count > 0)
                {
                    sb.AppendFormat("Joint References:\n");
                    for (int i = 0; i < mJointReferences.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}]{1}\n", i, mJointReferences[i].Value);
                    }
                }
                if (mReferences.Count > 0)
                {
                    sb.AppendFormat("TGI References:\n");
                    for (int i = 0; i < mReferences.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}]{1}\n", i, mReferences[i].Value);
                    }
                }
                return sb.ToString();
            }
        }
        private UInt32 mVersion;
        private MaterialBlock mMaterial;
        private UInt32 mUnknown01;
        private UInt32 mUnknown02;
        private VertexDataFormat mVertexFormat;
        private VertexDataList mVertexDataEntries;
        private IndexDataFormat mIndexDataFormat;
        private IndexDataList mIndexDataEntries;
        private UInt32 mUnknown03;
        private JointReferenceList mJointReferences;
        private AResource.TGIBlockList mReferences;

        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (tag != Tag)
                throw new InvalidDataException(String.Format(
                    "Invalid Tag read: '{0}'; expected: '{1}'; at 0x{2:X8}", tag, Tag, s.Position));
            mVersion = br.ReadUInt32();
            long tgiOffset = br.ReadUInt32() + s.Position;
            long tgiSize = br.ReadUInt32();
            mMaterial = new MaterialBlock(0, handler, s);
            mUnknown01 = br.ReadUInt32();
            mUnknown02 = br.ReadUInt32();
            uint vertexCount = br.ReadUInt32();
            mVertexFormat = new VertexDataFormat(0, handler, s);
            mVertexDataEntries = new VertexDataList(handler, this);
            for (uint i = 0; i < vertexCount; i++)
            {
                mVertexDataEntries.Add(new VertexData(0, handler, s, this));
            }
            mIndexDataFormat = new IndexDataFormat(0, handler, s);
            mIndexDataEntries = new IndexDataList(handler, s);
            mUnknown03 = br.ReadUInt32();
            mJointReferences = new JointReferenceList(handler, s);
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
            if (mMaterial == null) mMaterial = new MaterialBlock(0, handler);
            mMaterial.UnParse(s);
            bw.Write(mUnknown01);
            bw.Write(mUnknown02);
            if (mVertexDataEntries == null) mVertexDataEntries = new VertexDataList(handler, this);
            bw.Write(mVertexDataEntries.Count);
            if (mVertexFormat == null) mVertexFormat = new VertexDataFormat(0, handler);
            mVertexFormat.UnParse(s);
            foreach (var v in mVertexDataEntries) v.UnParse(s);
            if (mIndexDataFormat == null) mIndexDataFormat = new IndexDataFormat(0, handler);
            mIndexDataFormat.UnParse(s);
            if (mIndexDataEntries == null) mIndexDataEntries = new IndexDataList(handler);
            mIndexDataEntries.UnParse(s);
            bw.Write((uint)mUnknown03);
            if (mJointReferences == null) mJointReferences = new JointReferenceList(handler);
            mJointReferences.UnParse(s);
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
            get { return 0x00000000; }
        }

        public override string Tag
        {
            get { return "GEOM"; }
        }

        private const int kRecommendedApiVersion = 1;
        private static bool checking = Settings.Checking;
    }
}