using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using s3pi.Settings;
namespace s3piwrappers
{
    [Flags]
    public enum MeshFlags : uint
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
            private UInt32 mState;
            private UInt32 mStartIndex;
            private UInt32 mMinVertexIndex;
            private UInt32 mVertexCount;
            private UInt32 mPrimitiveCount;

            public GeometryState(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public GeometryState(int APIversion, EventHandler handler, GeometryState basis) : this(APIversion, handler, basis.State, basis.StartIndex, basis.MinVertexIndex, basis.VertexCount, basis.PrimitiveCount) { }
            public GeometryState(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }
            public GeometryState(int APIversion, EventHandler handler, uint state, uint startIndex, uint minVertexIndex, uint vertexCount, uint primitiveCount)
                : base(APIversion, handler)
            {
                mState = state;
                mStartIndex = startIndex;
                mMinVertexIndex = minVertexIndex;
                mVertexCount = vertexCount;
                mPrimitiveCount = primitiveCount;
            }

            [ElementPriority(1)]
            public UInt32 State
            {
                get { return mState; }
                set { if (mState != value) { mState = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public UInt32 StartIndex
            {
                get { return mStartIndex; }
                set { if (mStartIndex != value) { mStartIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public UInt32 MinVertexIndex
            {
                get { return mMinVertexIndex; }
                set { if (mMinVertexIndex != value) { mMinVertexIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(4)]
            public UInt32 VertexCount
            {
                get { return mVertexCount; }
                set { if (mVertexCount != value) { mVertexCount = value; OnElementChanged(); } }
            }
            [ElementPriority(5)]
            public UInt32 PrimitiveCount
            {
                get { return mPrimitiveCount; }
                set { if (mPrimitiveCount != value) { mPrimitiveCount = value; OnElementChanged(); } }
            }

            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mState = br.ReadUInt32();
                mStartIndex = br.ReadUInt32();
                mMinVertexIndex = br.ReadUInt32();
                mVertexCount = br.ReadUInt32();
                mPrimitiveCount = br.ReadUInt32();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mState);
                bw.Write(mStartIndex);
                bw.Write(mMinVertexIndex);
                bw.Write(mVertexCount);
                bw.Write(mPrimitiveCount);
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
                return mState.Equals(other.mState);
            }

            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("State: 0x{0:X8}\n", mState);
                    sb.AppendFormat("Start Index:\t{0}\n", mStartIndex);
                    sb.AppendFormat("Min Vertex Index:\t{0}\n", mMinVertexIndex);
                    sb.AppendFormat("Vertex Count:\t{0}\n", mVertexCount);
                    sb.AppendFormat("Primitive Count:\t{0}\n", mPrimitiveCount);
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
                mName = basis.mName;
            }
            public JointReference(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            public String Value { get { return String.Format("0x{0:X8}", mName); } }
            private UInt32 mName;
            [ElementPriority(0)]
            public uint Name
            {
                get { return mName; }
                set { if (mName != value) { mName = value; OnElementChanged(); } }
            }

            private void Parse(Stream s)
            {
                mName = new BinaryReader(s).ReadUInt32();
            }
            public void UnParse(Stream s)
            {
                new BinaryWriter(s).Write(mName);
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
                return mName.Equals(other.mName);
            }
        }
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

            public MeshList(EventHandler handler, MLOD owner, IList<Mesh> ilt)
                : base(handler, ilt)
            {
                mOwner = owner;
            }

            public override void Add()
            {
                base.Add(new object[] { mOwner });
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



        public class Mesh : AHandlerElement, IEquatable<Mesh>
        {
            private UInt32 mName;
            private UInt32 mMaterialIndex;
            private UInt32 mVertexFormatIndex;
            private UInt32 mVertexBufferIndex;
            private UInt32 mIndexBufferIndex;
            private ModelPrimitiveType mPrimitiveType;
            private MeshFlags mFlags;
            private UInt32 mStreamOffset;
            private UInt32 mStartVertex;
            private UInt32 mStartIndex;
            private UInt32 mMinVertexIndex;
            private UInt32 mVertexCount;
            private UInt32 mPrimitiveCount;
            private UInt32 mSkinControllerIndex;
            private UInt32 mScaleOffsetIndex;
            private JointReferenceList mJointReferences;
            private BoundingBox mBounds;
            private GeometryStateList mGeometryStates;
            private UInt32 mParentName;
            private Vector4 mMirrorPlane;
            private MLOD mOwner;


            public Mesh(int APIversion, EventHandler handler, MLOD owner)
                : base(APIversion, handler)
            {
                mOwner = owner;
                mBounds = new BoundingBox(0, handler);
                mJointReferences = new JointReferenceList(handler);
                mGeometryStates = new GeometryStateList(handler);
                mMirrorPlane = new Vector4(0, handler, 0, 0, 1, 0);
            }
            public Mesh(int APIversion, EventHandler handler, Mesh basis) : this(APIversion, handler, basis.Bounds,basis.Flags,basis.GeometryStates,basis.IndexBufferIndex,basis.JointReferences,basis.MaterialIndex,basis.MinVertexIndex,basis.MirrorPlane,basis.Name,basis.mOwner,basis.ParentName,basis.PrimitiveCount,basis.PrimitiveType,basis.ScaleOffsetIndex,basis.SkinControllerIndex,basis.StartIndex,basis.StartVertex,basis.StreamOffset,basis.VertexBufferIndex,basis.VertexCount,basis.VertexFormatIndex) {  }
            public Mesh(int APIversion, EventHandler handler, MLOD owner, Stream s) : this(APIversion, handler, owner) { Parse(s); }
            public Mesh(int APIversion, EventHandler handler, BoundingBox bounds, MeshFlags flags, GeometryStateList geometryStates, uint indexBufferIndex, JointReferenceList jointReferences, uint materialIndex, uint minVertexIndex, Vector4 mirrorPlane, uint name, MLOD owner, uint parentName, uint primitiveCount, ModelPrimitiveType primitiveType, uint scaleOffsetIndex, uint skinControllerIndex, uint startIndex, uint startVertex, uint streamOffset, uint vertexBufferIndex, uint vertexCount, uint vertexFormatIndex)
                : base(APIversion, handler)
            {
                mBounds = bounds;
                mFlags = flags;
                mGeometryStates = geometryStates;
                mIndexBufferIndex = indexBufferIndex;
                mJointReferences = jointReferences;
                mMaterialIndex = materialIndex;
                mMinVertexIndex = minVertexIndex;
                mMirrorPlane = mirrorPlane;
                mName = name;
                mOwner = owner;
                mParentName = parentName;
                mPrimitiveCount = primitiveCount;
                mPrimitiveType = primitiveType;
                mScaleOffsetIndex = scaleOffsetIndex;
                mSkinControllerIndex = skinControllerIndex;
                mStartIndex = startIndex;
                mStartVertex = startVertex;
                mStreamOffset = streamOffset;
                mVertexBufferIndex = vertexBufferIndex;
                mVertexCount = vertexCount;
                mVertexFormatIndex = vertexFormatIndex;
            }

            [ElementPriority(1)]
            public UInt32 Name
            {
                get { return mName; }
                set { if (mName != value) { mName = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public UInt32 MaterialIndex
            {
                get { return mMaterialIndex; }
                set { if (mMaterialIndex != value) { mMaterialIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public UInt32 VertexFormatIndex
            {
                get { return mVertexFormatIndex; }
                set { if (mVertexFormatIndex != value) { mVertexFormatIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(4)]
            public UInt32 VertexBufferIndex
            {
                get { return mVertexBufferIndex; }
                set { if (mVertexBufferIndex != value) { mVertexBufferIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(5)]
            public UInt32 IndexBufferIndex
            {
                get { return mIndexBufferIndex; }
                set { if (mIndexBufferIndex != value) { mIndexBufferIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(6)]
            public ModelPrimitiveType PrimitiveType
            {
                get { return mPrimitiveType; }
                set { if (mPrimitiveType != value) { mPrimitiveType = value; OnElementChanged(); } }
            }
            [ElementPriority(7)]
            public MeshFlags Flags
            {
                get { return mFlags; }
                set { if (mFlags != value) { mFlags = value; OnElementChanged(); } }
            }
            [ElementPriority(8)]
            public UInt32 StreamOffset
            {
                get { return mStreamOffset; }
                set { if (mStreamOffset != value) { mStreamOffset = value; OnElementChanged(); } }
            }
            [ElementPriority(9)]
            public UInt32 StartVertex
            {
                get { return mStartVertex; }
                set { if (mStartVertex != value) { mStartVertex = value; OnElementChanged(); } }
            }
            [ElementPriority(10)]
            public UInt32 StartIndex
            {
                get { return mStartIndex; }
                set { if (mStartIndex != value) { mStartIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(11)]
            public UInt32 MinVertexIndex
            {
                get { return mMinVertexIndex; }
                set { if (mMinVertexIndex != value) { mMinVertexIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(12)]
            public UInt32 VertexCount
            {
                get { return mVertexCount; }
                set { if (mVertexCount != value) { mVertexCount = value; OnElementChanged(); } }
            }
            [ElementPriority(13)]
            public UInt32 PrimitiveCount
            {
                get { return mPrimitiveCount; }
                set { if (mPrimitiveCount != value) { mPrimitiveCount = value; OnElementChanged(); } }
            }
            [ElementPriority(14)]
            public BoundingBox Bounds
            {
                get { return mBounds; }
                set { if (mBounds != value) { mBounds = value; OnElementChanged(); } }
            }
            [ElementPriority(15)]
            public UInt32 SkinControllerIndex
            {
                get { return mSkinControllerIndex; }
                set { if (mSkinControllerIndex != value) { mSkinControllerIndex = value; OnElementChanged(); } }
            }

            [ElementPriority(16)]
            public UInt32 ScaleOffsetIndex
            {
                get { return mScaleOffsetIndex; }
                set { if (mScaleOffsetIndex != value) { mScaleOffsetIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(17)]
            public JointReferenceList JointReferences
            {
                get { return mJointReferences; }
                set { if (mJointReferences != value) { mJointReferences = value; OnElementChanged(); } }
            }
            [ElementPriority(18)]
            public GeometryStateList GeometryStates
            {
                get { return mGeometryStates; }
                set { if (mGeometryStates != value) { mGeometryStates = value; OnElementChanged(); } }
            }
            [ElementPriority(19)]
            public UInt32 ParentName
            {
                get { return mParentName; }
                set { if (mParentName != value) { mParentName = value; OnElementChanged(); } }
            }
            [ElementPriority(20)]
            public Vector4 MirrorPlane
            {
                get { return mMirrorPlane; }
                set { if (mMirrorPlane != value) { mMirrorPlane = value; OnElementChanged(); } }
            }




            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                long expectedSize = br.ReadUInt32();
                long start = s.Position;
                mName = br.ReadUInt32();
                mMaterialIndex = br.ReadUInt32();
                mVertexFormatIndex = br.ReadUInt32();
                mVertexBufferIndex = br.ReadUInt32();
                mIndexBufferIndex = br.ReadUInt32();
                uint val = br.ReadUInt32();
                mPrimitiveType = (ModelPrimitiveType)(val & 0x000000FF);
                mFlags = (MeshFlags)(val >> 8);
                mStreamOffset = br.ReadUInt32();
                mStartVertex = br.ReadUInt32();
                mStartIndex = br.ReadUInt32();
                mMinVertexIndex = br.ReadUInt32();
                mVertexCount = br.ReadUInt32();
                mPrimitiveCount = br.ReadUInt32();
                mBounds = new BoundingBox(0, handler, s);
                mSkinControllerIndex = br.ReadUInt32();
                mJointReferences = new JointReferenceList(handler, s);
                mScaleOffsetIndex = br.ReadUInt32();
                mGeometryStates = new GeometryStateList(handler, s);
                if (mOwner.Version > 0x00000201)
                {
                    mParentName = br.ReadUInt32();
                    mMirrorPlane = new Vector4(0, handler, br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
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
                bw.Write(mName);
                bw.Write(mMaterialIndex);
                bw.Write(mVertexFormatIndex);
                bw.Write(mVertexBufferIndex);
                bw.Write(mIndexBufferIndex);
                bw.Write((UInt32)mPrimitiveType | ((UInt32)mFlags << 8));
                bw.Write(mStreamOffset);
                bw.Write(mStartVertex);
                bw.Write(mStartIndex);
                bw.Write(mMinVertexIndex);
                bw.Write(mVertexCount);
                bw.Write(mPrimitiveCount);
                mBounds.UnParse(s);
                bw.Write(mSkinControllerIndex);
                mJointReferences.UnParse(s);
                bw.Write(mScaleOffsetIndex);
                mGeometryStates.UnParse(s);
                if (mOwner.Version > 0x00000201)
                {
                    bw.Write(mParentName);
                    bw.Write(mMirrorPlane.X);
                    bw.Write(mMirrorPlane.Y);
                    bw.Write(mMirrorPlane.Z);
                    bw.Write(mMirrorPlane.W);
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
                    sb.AppendFormat("Name: 0x{0:X8}\n", mName);
                    sb.AppendFormat("Material: 0x{0:X8}\n", mMaterialIndex);
                    sb.AppendFormat("VertexFormat: 0x{0:X8}\n", mVertexFormatIndex);
                    sb.AppendFormat("VertexBuffer: 0x{0:X8}\n", mVertexBufferIndex);
                    sb.AppendFormat("PrimitiveType: {0}\n", this["PrimitiveType"]);
                    sb.AppendFormat("Flags: {0}\n", this["Flags"]);
                    sb.AppendFormat("StreamOffset: 0x{0:X8}\n", mStreamOffset);
                    sb.AppendFormat("StartVertex: 0x{0:X8}\n", mStartVertex);
                    sb.AppendFormat("StartIndex: 0x{0:X8}\n", mStartIndex);
                    sb.AppendFormat("MinVertexIndex Offset: 0x{0:X8}\n", mMinVertexIndex);
                    sb.AppendFormat("Vertex Count: {0}\n", mVertexCount);
                    sb.AppendFormat("Primitive Count: {0}\n", mPrimitiveCount);
                    sb.AppendFormat("Bounds:\n{0}\n", mBounds.Value);
                    sb.AppendFormat("SkinController: 0x{0:X8}\n", mSkinControllerIndex);

                    if (mJointReferences.Count > 0)
                    {
                        sb.AppendFormat("Joints:\n");
                        for (int i = 0; i < mJointReferences.Count; i++)
                        {
                            sb.AppendFormat("[{0:00}]\t{1}\n", i, mJointReferences[i].Value);
                        }
                    }
                    sb.AppendFormat("Scale\\Offsets: 0x{0:X8}\n", mScaleOffsetIndex);
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
                        sb.AppendFormat("MirrorPlane: {0}\n", mMirrorPlane);
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
                        fields.Remove("MirrorPlane");
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


        public MLOD(int APIversion, EventHandler handler, MLOD basis) : this(APIversion, handler, basis.Version, new MeshList(handler,basis, basis.mMeshes)) { }
        public MLOD(int APIversion, EventHandler handler) : base(APIversion, handler, null) { }
        public MLOD(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        public MLOD(int APIversion, EventHandler handler, uint version, MeshList meshes)
            : base(APIversion, handler, null)
        {
            mVersion = version;
            mMeshes = meshes;
        }

        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { if (mVersion != value) { mVersion = value; OnRCOLChanged(this, new EventArgs()); } }
        }

        [ElementPriority(2)]
        public MeshList Meshes
        {
            get { return mMeshes; }
            set { if (mMeshes != value) { mMeshes = value; OnRCOLChanged(this, new EventArgs()); } }
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

        private UInt32 mVersion = 0x00000202;
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

        private static bool checking = Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}