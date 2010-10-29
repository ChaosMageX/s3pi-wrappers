using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
namespace s3piwrappers
{
    [Flags]
    public enum MeshFlags
    {
        BasinInterior = 0x00000001,
        HDExteriorLit = 0x00000002,
        PortalSide = 0x00000004,
        DropShadow = 0x00000008,
        ShadowCaster = 0x00000010,
        Foundation = 0x00000020,
        Pickable = 0x00000040

    }
    public enum ModelPrimitiveType
    {
        PointList,
        LineList,
        LineStrip,
        TriangleList,
        TriangleFan,
        TriangleStrip,
        RectList,
        QuadList,
        DisplayList
    }



    public class MLOD : ARCOLBlock
    {
        public class GeometryStateList : AResource.DependentList<GeometryState>
        {
            public GeometryStateList(EventHandler handler)
                : base(handler)
            {
            }

            public GeometryStateList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public override void Add()
            {
                Add(new object[] { });
            }

            protected override GeometryState CreateElement(Stream s)
            {
                return new GeometryState(0, handler, s);
            }

            protected override void WriteElement(Stream s, GeometryState element)
            {
                element.UnParse(s);
            }
        }
        public class GeometryState : AHandlerElement, IEquatable<GeometryState>
        {
            private UInt32 mStateNameHash;
            private Int32 mIBUFIndex;
            private Int32 mVBUFIndex;
            private Int32 mVertexCount;
            private Int32 mFaceCount;

            public GeometryState(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public GeometryState(int APIversion, EventHandler handler, GeometryState basis)
                : base(APIversion, handler)
            {
                mStateNameHash = basis.mStateNameHash;
                mIBUFIndex = basis.mIBUFIndex;
                mVBUFIndex = basis.mVBUFIndex;
                mVertexCount = basis.mVertexCount;
                mFaceCount = basis.mFaceCount;
            }
            public GeometryState(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            [ElementPriority(1)]
            public uint StateNameHash
            {
                get { return mStateNameHash; }
                set { mStateNameHash = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public int IbufIndex
            {
                get { return mIBUFIndex; }
                set { mIBUFIndex = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public int VbufIndex
            {
                get { return mVBUFIndex; }
                set { mVBUFIndex = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public int VertexCount
            {
                get { return mVertexCount; }
                set { mVertexCount = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public int FaceCount
            {
                get { return mFaceCount; }
                set { mFaceCount = value; OnElementChanged(); }
            }

            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mStateNameHash = br.ReadUInt32();
                mIBUFIndex = br.ReadInt32();
                mVBUFIndex = br.ReadInt32();
                mVertexCount = br.ReadInt32();
                mFaceCount = br.ReadInt32();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mStateNameHash);
                bw.Write(mIBUFIndex);
                bw.Write(mVBUFIndex);
                bw.Write(mVertexCount);
                bw.Write(mFaceCount);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new GeometryState(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(GeometryState other)
            {
                return mStateNameHash.Equals(other.mStateNameHash);
            }

            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("State Name Hash: 0x{0:X8}\n", mStateNameHash);
                    sb.AppendFormat("IBUF Index:\t{0}\n", mIBUFIndex);
                    sb.AppendFormat("VBUF Index:\t{0}\n", mVBUFIndex);
                    sb.AppendFormat("Vertex Count:\t{0}\n", mVertexCount);
                    sb.AppendFormat("Face Count:\t{0}\n", mFaceCount);
                    return sb.ToString();
                }
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
                Add(new object[] { });
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
                mJointNameHash = basis.mJointNameHash;
            }
            public JointReference(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            public String Value { get { return String.Format("0x{0:X8}", mJointNameHash); } }
            private UInt32 mJointNameHash;
            [ElementPriority(0)]
            public uint JointNameHash
            {
                get { return mJointNameHash; }
                set { mJointNameHash = value; OnElementChanged(); }
            }

            private void Parse(Stream s)
            {
                mJointNameHash = new BinaryReader(s).ReadUInt32();
            }
            public void UnParse(Stream s)
            {
                new BinaryWriter(s).Write(mJointNameHash);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new JointReference(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(JointReference other)
            {
                return mJointNameHash.Equals(other.mJointNameHash);
            }
        }
        #region MeshList
        public class MeshList : AResource.DependentList<Mesh>
        {
            private MLOD mOwner;
            public MeshList(EventHandler handler, MLOD owner)
                : base(handler)
            {
                mOwner = owner;
            }

            public MeshList(EventHandler handler, MLOD owner, Stream s)
                : this(handler, owner)
            {
                Parse(s);
            }

            public override void Add()
            {
                ((IList<Mesh>)this).Add(new Mesh(0, this.elementHandler, mOwner));
            }

            protected override Mesh CreateElement(Stream s)
            {
                return new Mesh(0, handler, mOwner, s);
            }

            protected override void WriteElement(Stream s, Mesh element)
            {
                element.UnParse(s);
            }
        }
        #endregion

        #region Mesh

        public class Mesh : AHandlerElement, IEquatable<Mesh>
        {
            private UInt32 mNameHash;
            private UInt32 mMATDIndex;
            private UInt32 mVRTFIndex;
            private UInt32 mVBUFIndex;
            private UInt32 mIBUFIndex;
            private ModelPrimitiveType mPrimitiveType;
            private MeshFlags mFlags;
            private UInt64 mVBUFOffset;
            private UInt64 mIBUFOffset;
            private UInt32 mVBUFCount;
            private UInt32 mIBUFCount;
            private UInt32 mSKINIndex;
            private UInt32 mUvIndex;


            private JointReferenceList mJointReferences;
            private BoundingBox mBounds;
            private GeometryStateList mGeometryStates;
            private UInt32 mParentName;
            private Single mMirrorPlaneX;
            private Single mMirrorPlaneY;
            private Single mMirrorPlaneZ;
            private Single mMirrorPlaneW;
            private MLOD mOwner;


            public Mesh(int APIversion, EventHandler handler, MLOD owner)
                : base(APIversion, handler)
            {
                mOwner = owner;
                mBounds = new BoundingBox(0, handler);
                mJointReferences = new JointReferenceList(handler);
                mGeometryStates = new GeometryStateList(handler);
            }
            public Mesh(int APIversion, EventHandler handler, MLOD owner, Stream s)
                : this(APIversion, handler, owner)
            {
                Parse(s);
            }
            public Mesh(int APIversion, EventHandler handler, Mesh basis)
                : base(APIversion, handler)
            {
                mOwner = basis.mOwner;
                Stream s = new MemoryStream();
                basis.UnParse(s);
                s.Position = 0L;
                Parse(s);
            }
            [ElementPriority(1)]
            public uint NameHash
            {
                get { return mNameHash; }
                set { mNameHash = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public uint MatdIndex
            {
                get { return mMATDIndex; }
                set { mMATDIndex = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public uint VrtfIndex
            {
                get { return mVRTFIndex; }
                set { mVRTFIndex = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public uint VbufIndex
            {
                get { return mVBUFIndex; }
                set { mVBUFIndex = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public uint IbufIndex
            {
                get { return mIBUFIndex; }
                set { mIBUFIndex = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public ModelPrimitiveType PrimitiveType
            {
                get { return mPrimitiveType; }
                set { mPrimitiveType = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public MeshFlags Flags
            {
                get { return mFlags; }
                set { mFlags = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public ulong VbufOffset
            {
                get { return mVBUFOffset; }
                set { mVBUFOffset = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public ulong IbufOffset
            {
                get { return mIBUFOffset; }
                set { mIBUFOffset = value; OnElementChanged(); }
            }
            [ElementPriority(10)]
            public uint VbufCount
            {
                get { return mVBUFCount; }
                set { mVBUFCount = value; OnElementChanged(); }
            }
            [ElementPriority(11)]
            public uint IbufCount
            {
                get { return mIBUFCount; }
                set { mIBUFCount = value; OnElementChanged(); }
            }
            [ElementPriority(11)]
            public BoundingBox Bounds
            {
                get { return mBounds; }
                set { mBounds = value; OnElementChanged(); }
            }
            [ElementPriority(12)]
            public uint SkinIndex
            {
                get { return mSKINIndex; }
                set { mSKINIndex = value; OnElementChanged(); }
            }

            [ElementPriority(13)]
            public uint UvIndex
            {
                get { return mUvIndex; }
                set { mUvIndex = value; }
            }
            [ElementPriority(14)]
            public JointReferenceList JointReferences
            {
                get { return mJointReferences; }
                set { mJointReferences = value; OnElementChanged(); }
            }
            [ElementPriority(15)]
            public GeometryStateList GeometryStates
            {
                get { return mGeometryStates; }
                set { mGeometryStates = value; OnElementChanged(); }
            }
            [ElementPriority(16)]
            public UInt32 ParentName
            {
                get { return mParentName; }
                set { mParentName = value; OnElementChanged(); }
            }
            [ElementPriority(17)]
            public float MirrorPlaneX
            {
                get { return mMirrorPlaneX; }
                set { mMirrorPlaneX = value; OnElementChanged(); }
            }
            [ElementPriority(18)]
            public float MirrorPlaneY
            {
                get { return mMirrorPlaneY; }
                set { mMirrorPlaneY = value; OnElementChanged(); }
            }
            [ElementPriority(19)]
            public float MirrorPlaneZ
            {
                get { return mMirrorPlaneZ; }
                set { mMirrorPlaneZ = value; OnElementChanged(); }
            }
            [ElementPriority(20)]
            public float MirrorPlaneW
            {
                get { return mMirrorPlaneW; }
                set { mMirrorPlaneW = value; OnElementChanged(); }
            }




            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                long expectedSize = br.ReadUInt32();
                long start = s.Position;
                mNameHash = br.ReadUInt32();
                mMATDIndex = br.ReadUInt32();
                mVRTFIndex = br.ReadUInt32();
                mVBUFIndex = br.ReadUInt32();
                mIBUFIndex = br.ReadUInt32();
                uint val = br.ReadUInt32();
                mPrimitiveType = (ModelPrimitiveType)(val & 0x000000FF);
                mFlags = (MeshFlags)(val >> 8);
                mVBUFOffset = br.ReadUInt64();
                mIBUFOffset = br.ReadUInt64();
                mVBUFCount = br.ReadUInt32();
                mIBUFCount = br.ReadUInt32();
                mBounds = new BoundingBox(0, handler, s);
                mSKINIndex = br.ReadUInt32();
                mJointReferences = new JointReferenceList(handler, s);
                mUvIndex = br.ReadUInt32();
                mGeometryStates = new GeometryStateList(handler, s);
                if (mOwner.Version > 0x00000201)
                {
                    mParentName = br.ReadUInt32();
                    mMirrorPlaneX = br.ReadSingle();
                    mMirrorPlaneY = br.ReadSingle();
                    mMirrorPlaneZ = br.ReadSingle();
                    mMirrorPlaneW = br.ReadSingle();
                }
                long actualSize = s.Position - start;
                if (checking && actualSize != expectedSize)
                    throw new Exception(String.Format("Expected end at {0}, actual end was {1}", expectedSize,
                                                      actualSize));

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                long sizeOffset = s.Position;
                bw.Write(0);
                long start = s.Position;
                bw.Write(mNameHash);
                bw.Write(mMATDIndex);
                bw.Write(mVRTFIndex);
                bw.Write(mVBUFIndex);
                bw.Write(mIBUFIndex);
                bw.Write((UInt32)mPrimitiveType | ((UInt32)mFlags << 8));
                bw.Write(mVBUFOffset);
                bw.Write(mIBUFOffset);
                bw.Write(mVBUFCount);
                bw.Write(mIBUFCount);
                mBounds.UnParse(s);
                bw.Write(mSKINIndex);
                mJointReferences.UnParse(s);
                bw.Write(mUvIndex);
                mGeometryStates.UnParse(s);
                if (mOwner.Version > 0x00000201)
                {
                    bw.Write(mParentName);
                    bw.Write(mMirrorPlaneX);
                    bw.Write(mMirrorPlaneY);
                    bw.Write(mMirrorPlaneZ);
                    bw.Write(mMirrorPlaneW);
                }
                long end = s.Position;
                long size = end - start;
                s.Seek(sizeOffset, SeekOrigin.Begin);
                bw.Write((uint)size);
                s.Seek(end, SeekOrigin.Begin);
            }
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Name: 0x{0:X8}\n", mNameHash);
                    sb.AppendFormat("Material: 0x{0:X8}\n", mMATDIndex);
                    sb.AppendFormat("Vertex Format: 0x{0:X8}\n", mVRTFIndex);
                    sb.AppendFormat("Vertex Buffer: 0x{0:X8}\n", mVBUFIndex);
                    sb.AppendFormat("Primitive Type: {0}\n", this["PrimitiveType"]);
                    sb.AppendFormat("Flags: {0}\n", this["Flags"]);
                    sb.AppendFormat("VBUF Offset: 0x{0:X16}\n", mVBUFOffset);
                    sb.AppendFormat("IBUF Offset: 0x{0:X16}\n", mIBUFOffset);
                    sb.AppendFormat("Vertex Count: {0}\n", mVBUFCount);
                    sb.AppendFormat("Face Count: {0}\n", mIBUFCount);
                    sb.AppendFormat("Bounds:\n{0}\n", mBounds.Value);
                    sb.AppendFormat("Skin Controller: 0x{0:X8}\n", mSKINIndex);

                    if (mJointReferences.Count > 0)
                    {
                        sb.AppendFormat("Joints:\n");
                        for (int i = 0; i < mJointReferences.Count; i++)
                        {
                            sb.AppendFormat("[{0:00}]\t{1}\n", i, mJointReferences[i].Value);
                        }
                    }
                    sb.AppendFormat("UV Settings: 0x{0:X8}\n", mUvIndex);
                    if (mGeometryStates.Count > 0)
                    {
                        sb.AppendFormat("Geometry States:\n");
                        for (int i = 0; i < mGeometryStates.Count; i++)
                        {
                            sb.AppendFormat("=Geometry State[{0}]=\n{1}\n", i, mGeometryStates[i].Value);
                        }
                    }
                    if (mOwner.Version >= 0x00000202)
                    {
                        sb.AppendFormat("ParentName: 0x{0:X8}\n", mParentName);
                        sb.AppendFormat("MirrorPlaneX: {0,8:0.00000}\n", mMirrorPlaneX);
                        sb.AppendFormat("MirrorPlaneY: {0,8:0.00000}\n", mMirrorPlaneY);
                        sb.AppendFormat("MirrorPlaneZ: {0,8:0.00000}\n", mMirrorPlaneZ);
                        sb.AppendFormat("MirrorPlaneW: {0,8:0.00000}\n", mMirrorPlaneW);
                    }
                    return sb.ToString();
                }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Mesh(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get
                {
                    var fields = GetContentFields(base.requestedApiVersion, GetType());
                    if (mOwner.Version < 0x00000202)
                    {
                        fields.Remove("ParentName");
                        fields.Remove("MirrorPlaneX");
                        fields.Remove("MirrorPlaneY");
                        fields.Remove("MirrorPlaneZ");
                        fields.Remove("MirrorPlaneW");
                    }
                    return fields;
                }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(Mesh other)
            {
                return base.Equals(other);
            }
        }
        #endregion

        public MLOD(int APIversion, EventHandler handler, MLOD basis)
            : base(APIversion, handler, null)
        {
            Stream s = basis.UnParse();
            s.Position = 0L;
            Parse(s);
        }
        public MLOD(int APIversion, EventHandler handler)
            : base(APIversion, handler, null)
        {
            mVersion = 0x00000203;
            mMeshes = new MeshList(handler, this);
        }
        public MLOD(int APIversion, EventHandler handler, Stream s)
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
        public MeshList Meshes
        {
            get { return mMeshes; }
            set { mMeshes = value; OnRCOLChanged(this, new EventArgs()); }
        }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                if (mMeshes.Count > 0)
                {
                    sb.AppendFormat("Meshes:\n");
                    for (int i = 0; i < mMeshes.Count; i++)
                    {
                        sb.AppendFormat("==Mesh[{0}]==\n{1}\n", i, mMeshes[i].Value);
                    }
                }
                return sb.ToString();
            }
        }

        private UInt32 mVersion;
        private MeshList mMeshes;
        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            mMeshes = new MeshList(handler, this, s);
        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((uint)FOURCC(Tag));
            bw.Write(mVersion);
            if (mMeshes == null) mMeshes = new MeshList(handler, this);
            mMeshes.UnParse(s);
            return s;
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new MLOD(0, handler, this);
        }

        public override string Tag
        {
            get { return "MLOD"; }
        }

        public override uint ResourceType
        {
            get { return 0x01D10F34; }
        }

        private static bool checking = s3pi.Settings.Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}