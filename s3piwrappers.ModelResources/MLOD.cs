using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using s3pi.Settings;
using s3pi.GenericRCOLResource;

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
        public static int IndexCountFromPrimitiveType(ModelPrimitiveType t)
        {
            switch (t)
            {
                case ModelPrimitiveType.TriangleList:
                    return 3;
                default:
                    throw new NotImplementedException();
            }
        }
        public class GeometryStateList : DependentList<GeometryState>
        {
            public GeometryStateList(EventHandler handler)
                : base(handler)
            {
            }

            public GeometryStateList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public GeometryStateList(EventHandler handler, IEnumerable<GeometryState> basis)
                : base(handler, basis)
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
            private UInt32 mName;
            private Int32 mStartIndex;
            private Int32 mMinVertexIndex;
            private Int32 mVertexCount;
            private Int32 mPrimitiveCount;

            public GeometryState(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public GeometryState(int APIversion, EventHandler handler, GeometryState basis) : this(APIversion, handler, basis.Name, basis.StartIndex, basis.MinVertexIndex, basis.VertexCount, basis.PrimitiveCount) { }
            public GeometryState(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }
            public GeometryState(int APIversion, EventHandler handler, uint name, int startIndex, int minVertexIndex, int vertexCount, int primitiveCount)
                : base(APIversion, handler)
            {
                mName = name;
                mStartIndex = startIndex;
                mMinVertexIndex = minVertexIndex;
                mVertexCount = vertexCount;
                mPrimitiveCount = primitiveCount;
            }

            [ElementPriority(1)]
            public UInt32 Name
            {
                get { return mName; }
                set { if (mName != value) { mName = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public Int32 StartIndex
            {
                get { return mStartIndex; }
                set { if (mStartIndex != value) { mStartIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public Int32 MinVertexIndex
            {
                get { return mMinVertexIndex; }
                set { if (mMinVertexIndex != value) { mMinVertexIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(4)]
            public Int32 VertexCount
            {
                get { return mVertexCount; }
                set { if (mVertexCount != value) { mVertexCount = value; OnElementChanged(); } }
            }
            [ElementPriority(5)]
            public Int32 PrimitiveCount
            {
                get { return mPrimitiveCount; }
                set { if (mPrimitiveCount != value) { mPrimitiveCount = value; OnElementChanged(); } }
            }

            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mName = br.ReadUInt32();
                mStartIndex = br.ReadInt32();
                mMinVertexIndex = br.ReadInt32();
                mVertexCount = br.ReadInt32();
                mPrimitiveCount = br.ReadInt32();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mName);
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
                return
                    mName.Equals(other.mName)
                    && mStartIndex.Equals(other.mStartIndex)
                    && mMinVertexIndex.Equals(other.mMinVertexIndex)
                    && mVertexCount.Equals(other.mVertexCount)
                    && mPrimitiveCount.Equals(other.mPrimitiveCount)
                    ;
           }
            public override bool Equals(object obj)
            {
                return obj as GeometryState != null ? this.Equals(obj as GeometryState) : false;
            }
            public override int GetHashCode()
            {
                return
                    mName.GetHashCode()
                    ^ mStartIndex.GetHashCode()
                    ^ mMinVertexIndex.GetHashCode()
                    ^ mVertexCount.GetHashCode()
                    ^ mPrimitiveCount.GetHashCode()
                    ;
            }

            public string Value
            {
                get
                {
                    return ValueBuilder;
                    /*
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Name: 0x{0:X8}\n", mName);
                    sb.AppendFormat("Start Index:\t{0}\n", mStartIndex);
                    sb.AppendFormat("Min Vertex Index:\t{0}\n", mMinVertexIndex);
                    sb.AppendFormat("Vertex Count:\t{0}\n", mVertexCount);
                    sb.AppendFormat("Primitive Count:\t{0}\n", mPrimitiveCount);
                    return sb.ToString();
                    /**/
                }
            }
        }
        public class MeshList : DependentList<Mesh>
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

            public MeshList(EventHandler handler, MLOD owner, IEnumerable<Mesh> ilt)
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
            private GenericRCOLResource.ChunkReference mMaterialIndex;
            private GenericRCOLResource.ChunkReference mVertexFormatIndex;
            private GenericRCOLResource.ChunkReference mVertexBufferIndex;
            private GenericRCOLResource.ChunkReference mIndexBufferIndex;
            private ModelPrimitiveType mPrimitiveType;
            private MeshFlags mFlags;
            private UInt32 mStreamOffset;
            private Int32 mStartVertex;
            private Int32 mStartIndex;
            private Int32 mMinVertexIndex;
            private Int32 mVertexCount;
            private Int32 mPrimitiveCount;
            private GenericRCOLResource.ChunkReference mSkinControllerIndex;
            private GenericRCOLResource.ChunkReference mScaleOffsetIndex;
            private UIntList mJointReferences;
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
                mJointReferences = new UIntList(handler);
                mGeometryStates = new GeometryStateList(handler);
                mMirrorPlane = new Vector4(0, handler, 0, 0, 1, 0);
                mIndexBufferIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, 0);
                mMaterialIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, 0);
                mScaleOffsetIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, 0);
                mSkinControllerIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, 0);
                mVertexBufferIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, 0);
                mVertexFormatIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, 0);
            }
            public Mesh(int APIversion, EventHandler handler, Mesh basis) : this(APIversion, handler, basis.Bounds,basis.Flags,basis.GeometryStates,basis.IndexBufferIndex,basis.JointReferences,basis.MaterialIndex,basis.MinVertexIndex,basis.MirrorPlane,basis.Name,basis.mOwner,basis.ParentName,basis.PrimitiveCount,basis.PrimitiveType,basis.ScaleOffsetIndex,basis.SkinControllerIndex,basis.StartIndex,basis.StartVertex,basis.StreamOffset,basis.VertexBufferIndex,basis.VertexCount,basis.VertexFormatIndex) {  }
            public Mesh(int APIversion, EventHandler handler, MLOD owner, Stream s) : this(APIversion, handler, owner) { Parse(s); }
            public Mesh(int APIversion, EventHandler handler, BoundingBox bounds, MeshFlags flags, GeometryStateList geometryStates,
                GenericRCOLResource.ChunkReference indexBufferIndex, UIntList jointReferences,
                GenericRCOLResource.ChunkReference materialIndex, int minVertexIndex, Vector4 mirrorPlane, uint name,
                MLOD owner, uint parentName, int primitiveCount, ModelPrimitiveType primitiveType,
                GenericRCOLResource.ChunkReference scaleOffsetIndex,
                GenericRCOLResource.ChunkReference skinControllerIndex, int startIndex, int startVertex, uint streamOffset,
                GenericRCOLResource.ChunkReference vertexBufferIndex, int vertexCount,
                GenericRCOLResource.ChunkReference vertexFormatIndex)
                : base(APIversion, handler)
            {
                mBounds = bounds;
                mFlags = flags;
                mGeometryStates = geometryStates;
                mIndexBufferIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, indexBufferIndex);
                mJointReferences = jointReferences;
                mMaterialIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, materialIndex);
                mMinVertexIndex = minVertexIndex;
                mMirrorPlane = mirrorPlane;
                mName = name;
                mOwner = owner;
                mParentName = parentName;
                mPrimitiveCount = primitiveCount;
                mPrimitiveType = primitiveType;
                mScaleOffsetIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, scaleOffsetIndex);
                mSkinControllerIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, skinControllerIndex);
                mStartIndex = startIndex;
                mStartVertex = startVertex;
                mStreamOffset = streamOffset;
                mVertexBufferIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, vertexBufferIndex);
                mVertexCount = vertexCount;
                mVertexFormatIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, vertexFormatIndex);
            }

            [ElementPriority(1)]
            public UInt32 Name
            {
                get { return mName; }
                set { if (mName != value) { mName = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public GenericRCOLResource.ChunkReference MaterialIndex
            {
                get { return mMaterialIndex; }
                set { if (mMaterialIndex != value) { mMaterialIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, value); OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public GenericRCOLResource.ChunkReference VertexFormatIndex
            {
                get { return mVertexFormatIndex; }
                set { if (mVertexFormatIndex != value) { mVertexFormatIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, value); OnElementChanged(); } }
            }
            [ElementPriority(4)]
            public GenericRCOLResource.ChunkReference VertexBufferIndex
            {
                get { return mVertexBufferIndex; }
                set { if (mVertexBufferIndex != value) { mVertexBufferIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, value); OnElementChanged(); } }
            }
            [ElementPriority(5)]
            public GenericRCOLResource.ChunkReference IndexBufferIndex
            {
                get { return mIndexBufferIndex; }
                set { if (mIndexBufferIndex != value) { mIndexBufferIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, value); OnElementChanged(); } }
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
            public Int32 StartVertex
            {
                get { return mStartVertex; }
                set { if (mStartVertex != value) { mStartVertex = value; OnElementChanged(); } }
            }
            [ElementPriority(10)]
            public Int32 StartIndex
            {
                get { return mStartIndex; }
                set { if (mStartIndex != value) { mStartIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(11)]
            public Int32 MinVertexIndex
            {
                get { return mMinVertexIndex; }
                set { if (mMinVertexIndex != value) { mMinVertexIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(12)]
            public Int32 VertexCount
            {
                get { return mVertexCount; }
                set { if (mVertexCount != value) { mVertexCount = value; OnElementChanged(); } }
            }
            [ElementPriority(13)]
            public Int32 PrimitiveCount
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
            public GenericRCOLResource.ChunkReference SkinControllerIndex
            {
                get { return mSkinControllerIndex; }
                set { if (mSkinControllerIndex != value) { mSkinControllerIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, value); OnElementChanged(); } }
            }

            [ElementPriority(16)]
            public GenericRCOLResource.ChunkReference ScaleOffsetIndex
            {
                get { return mScaleOffsetIndex; }
                set { if (mScaleOffsetIndex != value) { mScaleOffsetIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, value); OnElementChanged(); } }
            }
            [ElementPriority(17)]
            public UIntList JointReferences
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
            public bool IsShadowCaster
            {
                get { return (mFlags & MeshFlags.ShadowCaster) != 0; }
            }




            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                long expectedSize = br.ReadUInt32();
                long start = s.Position;
                mName = br.ReadUInt32();
                mMaterialIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, s);
                mVertexFormatIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, s);
                mVertexBufferIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, s);
                mIndexBufferIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, s);
                uint val = br.ReadUInt32();
                mPrimitiveType = (ModelPrimitiveType)(val & 0x000000FF);
                mFlags = (MeshFlags)(val >> 8);
                mStreamOffset = br.ReadUInt32();
                mStartVertex = br.ReadInt32();
                mStartIndex = br.ReadInt32();
                mMinVertexIndex = br.ReadInt32();
                mVertexCount = br.ReadInt32();
                mPrimitiveCount = br.ReadInt32();
                mBounds = new BoundingBox(0, handler, s);
                mSkinControllerIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, s);
                mJointReferences = new UIntList(handler, s);
                mScaleOffsetIndex = new GenericRCOLResource.ChunkReference(requestedApiVersion, handler, s);
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
                mMaterialIndex.UnParse(s);
                mVertexFormatIndex.UnParse(s);
                mVertexBufferIndex.UnParse(s);
                mIndexBufferIndex.UnParse(s);
                bw.Write((UInt32)mPrimitiveType | ((UInt32)mFlags << 8));
                bw.Write(mStreamOffset);
                bw.Write(mStartVertex);
                bw.Write(mStartIndex);
                bw.Write(mMinVertexIndex);
                bw.Write(mVertexCount);
                bw.Write(mPrimitiveCount);
                mBounds.UnParse(s);
                mSkinControllerIndex.UnParse(s);
                mJointReferences.UnParse(s);
                mScaleOffsetIndex.UnParse(s);
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
                    return ValueBuilder;
                    /*
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Name: 0x{0:X8}\n", mName);
                    sb.AppendFormat("Material: 0x{0:X8}\n", mMaterialIndex);
                    sb.AppendFormat("VertexFormat: 0x{0:X8}\n", mVertexFormatIndex);
                    sb.AppendFormat("VertexBuffer: 0x{0:X8}\n", mVertexBufferIndex);
                    sb.AppendFormat("IndexBuffer: 0x{0:X8}\n", mIndexBufferIndex);
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
                            sb.AppendFormat("=Geometry Name[{0}]=\n{1}\n", i, mGeometryStates[i].Value);
                        }
                    }
                    if (mOwner.Version >= 0x00000202)
                    {
                        sb.AppendFormat("ParentName: 0x{0:X8}\n", mParentName);
                        sb.AppendFormat("MirrorPlane: {0}\n", mMirrorPlane);
                    }
                    return sb.ToString();
                    /**/
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
                return mName.Equals(other.mName)
                    && mMaterialIndex.Equals(other.mMaterialIndex)
                    && mVertexFormatIndex.Equals(other.mVertexFormatIndex)
                    && mVertexBufferIndex.Equals(other.mVertexBufferIndex)
                    && mIndexBufferIndex.Equals(other.mIndexBufferIndex)
                    && mPrimitiveType.Equals(other.mPrimitiveType)
                    && mFlags.Equals(other.mFlags)
                    && mStreamOffset.Equals(other.mStreamOffset)
                    && mStartVertex.Equals(other.mStartVertex)
                    && mStartIndex.Equals(other.mStartIndex)
                    && mMinVertexIndex.Equals(other.mMinVertexIndex)
                    && mVertexCount.Equals(other.mVertexCount)
                    && mPrimitiveCount.Equals(other.mPrimitiveCount)
                    && mSkinControllerIndex.Equals(other.mSkinControllerIndex)
                    && mScaleOffsetIndex.Equals(other.mScaleOffsetIndex)
                    && mJointReferences.Equals(other.mJointReferences)
                    && mBounds.Equals(other.mBounds)
                    && mGeometryStates.Equals(other.mGeometryStates)
                    && mParentName.Equals(other.mParentName)
                    && mMirrorPlane.Equals(other.mMirrorPlane)
                    && mOwner.Equals(other.mOwner)
                    ;
            }
            public override bool Equals(object obj)
            {
                return obj as Mesh != null ? this.Equals(obj as Mesh) : false;
            }
            public override int GetHashCode()
            {
                return mName.GetHashCode()
                    ^ mMaterialIndex.GetHashCode()
                    ^ mVertexFormatIndex.GetHashCode()
                    ^ mVertexBufferIndex.GetHashCode()
                    ^ mIndexBufferIndex.GetHashCode()
                    ^ mPrimitiveType.GetHashCode()
                    ^ mFlags.GetHashCode()
                    ^ mStreamOffset.GetHashCode()
                    ^ mStartVertex.GetHashCode()
                    ^ mStartIndex.GetHashCode()
                    ^ mMinVertexIndex.GetHashCode()
                    ^ mVertexCount.GetHashCode()
                    ^ mPrimitiveCount.GetHashCode()
                    ^ mSkinControllerIndex.GetHashCode()
                    ^ mScaleOffsetIndex.GetHashCode()
                    ^ mJointReferences.GetHashCode()
                    ^ mBounds.GetHashCode()
                    ^ mGeometryStates.GetHashCode()
                    ^ mParentName.GetHashCode()
                    ^ mMirrorPlane.GetHashCode()
                    ^ mOwner.GetHashCode()
                    ;
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
                return ValueBuilder;
                /*
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
                /**/
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