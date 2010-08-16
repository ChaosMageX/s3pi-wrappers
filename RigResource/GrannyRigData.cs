using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using s3pi.Interfaces;
using System.IO;

namespace s3piwrappers
{
    public class GrannyRigData : RigData
    {
        [DataGridExpandable(true)]
        public class Triple : GrannyDataElement<Granny2.Triple>
        {
            private float mX;
            private float mY;
            private float mZ;

            public Triple(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public Triple(int APIversion, EventHandler handler, Triple basis)
                : base(APIversion, handler, basis)
            {
            }

            public Triple(int APIversion, EventHandler handler, Granny2.Triple data)
                : base(APIversion, handler, data)
            {
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

            protected override void Parse(Granny2.Triple data)
            {
                mX = data.X;
                mY = data.Y;
                mZ = data.Z;
            }

            public override Granny2.Triple UnParse()
            {
                var t = new Granny2.Triple();
                t.X = mX;
                t.Y = mY;
                t.Z = mZ;
                return t;
            }
            public override string ToString()
            {
                return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]", mX, mY, mZ);
            }
            public override string Value
            {
                get
                {
                    return ToString();
                }
            }
        }
        [DataGridExpandable(true)]
        public class Quad : GrannyDataElement<Granny2.Quad>
        {
            private float mX;
            private float mY;
            private float mZ;
            private float mW;

            public Quad(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }

            public Quad(int APIversion, EventHandler handler, Quad basis)
                : base(APIversion, handler, basis)
            {
            }

            public Quad(int APIversion, EventHandler handler, Granny2.Quad data)
                : base(APIversion, handler, data)
            {
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
            [ElementPriority(4)]
            public float W
            {
                get { return mW; }
                set { mW = value; OnElementChanged(); }
            }

            protected override void Parse(Granny2.Quad data)
            {
                mX = data.X;
                mY = data.Y;
                mZ = data.Z;
                mW = data.W;
            }

            public override Granny2.Quad UnParse()
            {
                var t = new Granny2.Quad();
                t.X = mX;
                t.Y = mY;
                t.Z = mZ;
                t.W = mW;
                return t;
            }
            public override string ToString()
            {
                return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]", mX, mY, mZ, mW);
            }
            public override string Value
            {
                get
                {
                    return ToString();
                }
            }
        }
        [DataGridExpandable(true)]
        public class Matrix4x4 : GrannyDataElement<Granny2.Matrix4x4>
        {
            private Quad mM0;
            private Quad mM1;
            private Quad mM2;
            private Quad mM3;

            public Matrix4x4(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mM0 = new Quad(0, handler);
                mM1 = new Quad(0, handler);
                mM2 = new Quad(0, handler);
                mM3 = new Quad(0, handler);
            }

            public Matrix4x4(int APIversion, EventHandler handler, Matrix4x4 basis)
                : base(APIversion, handler, basis)
            {
            }

            public Matrix4x4(int APIversion, EventHandler handler, Granny2.Matrix4x4 data)
                : base(APIversion, handler, data)
            {
            }
            [ElementPriority(1)]
            public Quad M0
            {
                get { return mM0; }
                set { mM0 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public Quad M1
            {
                get { return mM1; }
                set { mM1 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Quad M2
            {
                get { return mM2; }
                set { mM2 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public Quad M3
            {
                get { return mM3; }
                set { mM3 = value; OnElementChanged(); }
            }

            protected override void Parse(Granny2.Matrix4x4 data)
            {
                mM0 = new Quad(0, handler, data.m0);
                mM1 = new Quad(0, handler, data.m1);
                mM2 = new Quad(0, handler, data.m2);
                mM3 = new Quad(0, handler, data.m3);
            }

            public override Granny2.Matrix4x4 UnParse()
            {
                var m = new Granny2.Matrix4x4();
                m.m0 = mM0.UnParse();
                m.m1 = mM1.UnParse();
                m.m2 = mM2.UnParse();
                m.m3 = mM3.UnParse();
                return m;
            }

            public override string ToString()
            {
                return String.Format("{0}\n{1}\n{2}\n{3}", mM0, mM1, mM2, mM3);
            }
            public override string Value
            {
                get
                {
                    return ToString();
                }
            }
        }
        public class Transform : GrannyDataElement<Granny2.Transform>
        {
            private UInt32 mFlags;
            private Triple mPosition;
            private Quad mOrientation;
            private Triple mScaleShear_0;
            private Triple mScaleShear_1;
            private Triple mScaleShear_2;

            public Transform(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mPosition = new Triple(0, handler);
                mScaleShear_0 = new Triple(0, handler);
                mScaleShear_1 = new Triple(0, handler);
                mScaleShear_2 = new Triple(0, handler);
                mOrientation = new Quad(0, handler);
            }

            public Transform(int APIversion, EventHandler handler, Transform basis)
                : base(APIversion, handler, basis)
            {
            }

            public Transform(int APIversion, EventHandler handler, Granny2.Transform data)
                : base(APIversion, handler, data)
            {
            }
            [ElementPriority(1)]
            public uint Flags
            {
                get { return mFlags; }
                set { mFlags = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public Triple Position
            {
                get { return mPosition; }
                set { mPosition = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Quad Orientation
            {
                get { return mOrientation; }
                set { mOrientation = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public Triple ScaleShear0
            {
                get { return mScaleShear_0; }
                set { mScaleShear_0 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public Triple ScaleShear1
            {
                get { return mScaleShear_1; }
                set { mScaleShear_1 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public Triple ScaleShear2
            {
                get { return mScaleShear_2; }
                set { mScaleShear_2 = value; OnElementChanged(); }
            }

            protected override void Parse(Granny2.Transform data)
            {
                mFlags = data.Flags;
                mPosition = new Triple(0, handler, data.Position);
                mOrientation = new Quad(0, handler, data.Orientation);
                mScaleShear_0 = new Triple(0, handler, data.ScaleShear_0);
                mScaleShear_1 = new Triple(0, handler, data.ScaleShear_1);
                mScaleShear_2 = new Triple(0, handler, data.ScaleShear_2);
            }

            public override Granny2.Transform UnParse()
            {
                var t = new Granny2.Transform();
                t.Flags = mFlags;
                t.Position = mPosition.UnParse();

                t.Orientation = mOrientation.UnParse();
                t.ScaleShear_0 = mScaleShear_0.UnParse();
                t.ScaleShear_1 = mScaleShear_1.UnParse();
                t.ScaleShear_2 = mScaleShear_2.UnParse();
                return t;
            }
            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Flags:\t0x{0:X8}\n", mFlags);
                    sb.AppendFormat("Position:\n{0}\n", mPosition);
                    sb.AppendFormat("Orientation:\n{0}\n", mOrientation);
                    sb.AppendFormat("Scale/Shear:\n{0}\n{1}\n{2}", mScaleShear_0, mScaleShear_1, mScaleShear_2);
                    return sb.ToString();
                }
            }
        }

        public class ArtToolInfoElement : GrannyDataElement<Granny2.ArtToolInfo>
        {
            private String mFromArtToolName;
            private Int32 mArtToolMajorRevision;
            private Int32 mArtToolMinorRevision;
            private Single mUnitsPerMeter;
            private Triple mOrigin;
            private Triple mRightVector;
            private Triple mUpVector;
            private Triple mBackVector;


            public ArtToolInfoElement(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mOrigin = new Triple(0, handler);
                mRightVector = new Triple(0, handler);
                mUpVector = new Triple(0, handler);
                mBackVector = new Triple(0, handler);
            }

            public ArtToolInfoElement(int APIversion, EventHandler handler, ArtToolInfoElement basis)
                : base(APIversion, handler, basis)
            {
            }

            public ArtToolInfoElement(int APIversion, EventHandler handler, Granny2.ArtToolInfo data)
                : base(APIversion, handler, data)
            {
            }
            [ElementPriority(1)]
            public string FromArtToolName
            {
                get { return mFromArtToolName; }
                set { mFromArtToolName = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public int ArtToolMajorRevision
            {
                get { return mArtToolMajorRevision; }
                set { mArtToolMajorRevision = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public int ArtToolMinorRevision
            {
                get { return mArtToolMinorRevision; }
                set { mArtToolMinorRevision = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public float UnitsPerMeter
            {
                get { return mUnitsPerMeter; }
                set { mUnitsPerMeter = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public Triple Origin
            {
                get { return mOrigin; }
                set { mOrigin = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public Triple RightVector
            {
                get { return mRightVector; }
                set { mRightVector = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public Triple UpVector
            {
                get { return mUpVector; }
                set { mUpVector = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public Triple BackVector
            {
                get { return mBackVector; }
                set { mBackVector = value; OnElementChanged(); }
            }

            protected override void Parse(Granny2.ArtToolInfo data)
            {
                mFromArtToolName = data.FromArtToolName;
                mArtToolMajorRevision = data.ArtToolMajorRevision;
                mArtToolMinorRevision = data.ArtToolMinorRevision;
                mUnitsPerMeter = data.UnitsPerMeter;
                mOrigin = new Triple(0, handler, data.Origin);
                mRightVector = new Triple(0, handler, data.RightVector);
                mUpVector = new Triple(0, handler, data.UpVector);
                mBackVector = new Triple(0, handler, data.BackVector);

            }

            public override Granny2.ArtToolInfo UnParse()
            {
                var art = new Granny2.ArtToolInfo();
                art.FromArtToolName = mFromArtToolName;
                art.ArtToolMajorRevision = mArtToolMajorRevision;
                art.ArtToolMinorRevision = mArtToolMinorRevision;
                art.UnitsPerMeter = mUnitsPerMeter;
                art.Origin = mOrigin.UnParse();
                art.RightVector = mRightVector.UnParse();
                art.UpVector = mUpVector.UnParse();
                art.BackVector = mBackVector.UnParse();
                return art;
            }
            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("FromArtToolName:\t{0}\n", mFromArtToolName);
                    sb.AppendFormat("ArtToolMajorRevision:\t{0}\n", mArtToolMajorRevision);
                    sb.AppendFormat("ArtToolMinorRevision:\t{0}\n", mArtToolMinorRevision);
                    sb.AppendFormat("UnitsPerMeter:\t{0,8:0.00000}\n", mUnitsPerMeter);
                    sb.AppendFormat("Origin:\t{0}\n", mOrigin);
                    sb.AppendFormat("RightVector:\t{0}\n", mRightVector);
                    sb.AppendFormat("UpVector:\t{0}\n", mUpVector);
                    sb.AppendFormat("BackVector:\t{0}\n", mBackVector);
                    return sb.ToString();
                }
            }
            public override string ToString()
            {
                return mFromArtToolName.ToString();
            }
        }
        public class ModelElement : GrannyDataElement<Granny2.Model>, IEquatable<ModelElement>
        {
            private String mName;
            private Transform mInitialPlacement;


            public ModelElement(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mInitialPlacement = new Transform(0, handler);
            }

            public ModelElement(int APIversion, EventHandler handler, ModelElement basis)
                : base(APIversion, handler, basis)
            {
            }

            public ModelElement(int APIversion, EventHandler handler, Granny2.Model data)
                : base(APIversion, handler, data)
            {
            }
            [ElementPriority(1)]
            public string Name
            {
                get { return mName; }
                set { mName = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public Transform InitialPlacement
            {
                get { return mInitialPlacement; }
                set { mInitialPlacement = value; OnElementChanged(); }
            }

            protected override void Parse(Granny2.Model data)
            {
                mName = data.Name;
                mInitialPlacement = new Transform(0, handler, data.InitialPlacement);
            }

            public override Granny2.Model UnParse()
            {
                var m = new Granny2.Model();
                m.Name = mName;
                m.InitialPlacement = mInitialPlacement.UnParse();
                return m;
            }

            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Name:\t{0}\n", mName);
                    sb.AppendFormat("InitialPlacement:\n{0}\n", mInitialPlacement.Value);
                    return sb.ToString();
                }
            }
            public override string ToString()
            {
                return mName.ToString();
            }
            public bool Equals(ModelElement other)
            {
                return mName.Equals(other.mName);
            }
        }
        public class Bone : GrannyDataElement<Granny2.Bone>, IEquatable<Bone>
        {
            private String mName;
            private Int32 mParentIndex;
            private Transform mLocalTransform;
            private Matrix4x4 mInverseWorld4x4;
            private Single mLODError;


            public Bone(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mLocalTransform = new Transform(0, handler);
                mInverseWorld4x4 = new Matrix4x4(0, handler);
            }


            public Bone(int APIversion, EventHandler handler, Bone basis)
                : base(APIversion, handler, basis)
            {
            }

            public Bone(int APIversion, EventHandler handler, Granny2.Bone data)
                : base(APIversion, handler, data)
            {
            }
            [ElementPriority(1)]
            public string Name
            {
                get { return mName; }
                set { mName = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public int ParentIndex
            {
                get { return mParentIndex; }
                set { mParentIndex = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Transform LocalTransform
            {
                get { return mLocalTransform; }
                set { mLocalTransform = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public Matrix4x4 InverseWorld4X4
            {
                get { return mInverseWorld4x4; }
                set { mInverseWorld4x4 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public float LodError
            {
                get { return mLODError; }
                set { mLODError = value; OnElementChanged(); }
            }

            protected override void Parse(Granny2.Bone data)
            {
                mName = data.Name;
                mParentIndex = data.ParentIndex;
                mLocalTransform = new Transform(0, handler, data.LocalTransform);
                mInverseWorld4x4 = new Matrix4x4(0, handler, data.InverseWorld4x4);
                mLODError = data.LODError;
            }

            public override Granny2.Bone UnParse()
            {
                var b = new Granny2.Bone();
                b.Name = mName;
                b.ParentIndex = mParentIndex;
                b.LocalTransform = mLocalTransform.UnParse();
                b.InverseWorld4x4 = mInverseWorld4x4.UnParse();
                b.LODError = mLODError;
                return b;
            }
            public override string ToString()
            {
                return Name.ToString();
            }
            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Name:\t{0}\n", mName);
                    sb.AppendFormat("ParentIndex:\t0x{0:X8}\n", mParentIndex);
                    sb.AppendFormat("LocalTransform:\n{0}\n", mLocalTransform.Value);
                    sb.AppendFormat("InverseWorld4x4:\n{0}\n", mInverseWorld4x4.Value);
                    sb.AppendFormat("LODError:\t{0,8:0.00000}\n", mLODError);
                    return sb.ToString();
                }
            }
            public bool Equals(Bone other)
            {
                return mName.Equals(other.mName);
            }
        }
        public class SkeletonElement : GrannyDataElement<Granny2.Skeleton>, IEquatable<SkeletonElement>
        {
            private String mName;
            private ElementList<Bone> mBones;


            public SkeletonElement(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mBones = new ElementList<Bone>(handler);
            }

            public SkeletonElement(int APIversion, EventHandler handler, SkeletonElement basis)
                : base(APIversion, handler, basis)
            {
            }

            public SkeletonElement(int APIversion, EventHandler handler, Granny2.Skeleton data)
                : base(APIversion, handler, data)
            {
            }
            [ElementPriority(1)]
            public string Name
            {
                get { return mName; }
                set { mName = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public ElementList<Bone> Bones
            {
                get { return mBones; }
                set { mBones = value; OnElementChanged(); }
            }
            protected override void Parse(Granny2.Skeleton data)
            {
                mBones = new ElementList<Bone>(handler);
                mName = data.Name;
                Int32 count = data.BoneCount;
                Int32 elementSize = Marshal.SizeOf(typeof(Granny2.Bone));
                IntPtr pCur = data.Bones;

                for (int i = 0; i < count; i++)
                {
                    mBones.Add(new Bone(0, handler, pCur.S<Granny2.Bone>()));
                    pCur = new IntPtr(pCur.ToInt32() + elementSize);
                }
            }

            public override Granny2.Skeleton UnParse()
            {
                Granny2.Skeleton s = new Granny2.Skeleton();
                s.Name = mName;
                s.BoneCount = mBones.Count;
                Int32 elementSize = Marshal.SizeOf(typeof(Granny2.Bone));
                IntPtr pBones = Marshal.AllocHGlobal(mBones.Count * elementSize);
                IntPtr pCur = new IntPtr(pBones.ToInt32());
                for (int i = 0; i < mBones.Count; i++)
                {
                    Marshal.StructureToPtr(mBones[i].UnParse(), pCur, false);
                    pCur = new IntPtr(pCur.ToInt32() + elementSize);
                }
                s.Bones = pBones;
                return s;
            }
            public override string Value
            {
                get
                {

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Name:\t{0}\n", mName);
                    sb.AppendFormat("Bones:\n");
                    for (int i = 0; i < mBones.Count; i++)
                    {
                        sb.AppendFormat("==Bone[{0}]==\n{1}\n", i, mBones[i].Value);

                    }
                    return sb.ToString();
                }
            }
            public override string ToString()
            {
                return mName.ToString();
            }

            public bool Equals(SkeletonElement other)
            {
                return mName.Equals(other.mName);
            }
        }

        private ArtToolInfoElement mArtToolInfo;
        private SkeletonElement mSkeleton;
        private ModelElement mModel;
        private string mFromFileName;
        public override string ToString()
        {
            return mFromFileName.ToString();
        }
        public override string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("From Filename:\t{0}\n", mFromFileName);
                sb.AppendFormat("Art Tool Info:\n{0}\n", mArtToolInfo.Value);
                sb.AppendFormat("Model:\n{0}\n", mModel.Value);
                sb.AppendFormat("Skeleton:\n{0}\n", mSkeleton.Value);
                return sb.ToString();
            }
        }
        [ElementPriority(1)]
        public ArtToolInfoElement ArtToolInfoInfo
        {
            get { return mArtToolInfo; }
            set { mArtToolInfo = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public string FromFileName
        {
            get { return mFromFileName; }
            set { mFromFileName = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public ModelElement Model
        {
            get { return mModel; }
            set { mModel = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public SkeletonElement Skeleton
        {
            get { return mSkeleton; }
            set { mSkeleton = value; OnElementChanged(); }
        }
        public GrannyRigData(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
            mArtToolInfo = new ArtToolInfoElement(0, handler);
            mModel = new ModelElement(0, handler);
            mSkeleton = new SkeletonElement(0, handler);
        }
        public GrannyRigData(int APIversion, EventHandler handler, GrannyRigData basis)
            : base(APIversion, handler, basis)
        {
            
        }
        public GrannyRigData(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler, s)
        {
        }

        protected override void Parse(Stream s)
        {
            IntPtr pFile = Granny2.IO.FromMemory(s);
            IntPtr pFileInfo = Granny2.IO.GetFileInfo(pFile);
            var fileInfo = pFileInfo.S<Granny2.GrannyFileInfo>();
            mFromFileName = fileInfo.FromFileName;
            mArtToolInfo = new ArtToolInfoElement(0, handler, fileInfo.ArtToolInfo.S<Granny2.ArtToolInfo>());
            mSkeleton = new SkeletonElement(0, handler, fileInfo.Skeletons.S<IntPtr>().S<Granny2.Skeleton>());
            mModel = new ModelElement(0, handler, fileInfo.Models.S<IntPtr>().S<Granny2.Model>());


        }

        public override Stream UnParse()
        {
            var file = new Granny2.GrannyFileInfo();
            var artToolInfo = mArtToolInfo.UnParse();
            var skeleton = mSkeleton.UnParse();
            var model = mModel.UnParse();

            var pSkeleton = skeleton.Ptr();
            file.FromFileName = mFromFileName;
            file.ArtToolInfo = artToolInfo.Ptr();
            file.SkeletonCount = 1;
            file.ModelCount = 1;
            model.Skeleton = pSkeleton;
            file.Models = model.Ptr().Ptr();
            file.Skeletons = pSkeleton.Ptr();
            file.Save(kTempFile, Granny2.CompressionType.Oodle1Compression);
            byte[] buffer;
            using(var tempStream = File.OpenRead(kTempFile))
            {
                buffer = new byte[tempStream.Length];
                tempStream.Read(buffer,0,buffer.Length);
            }
            File.Delete(kTempFile);
            
            return new MemoryStream(buffer);
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new GrannyRigData(0, handler);
        }

        public override List<string> ContentFields
        {
            get { return GetContentFields(kRecommendedApiVersion, GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedApiVersion; }
        }

        private const string kLogFileName = "granny2log.txt";
        private const uint kGrannyWriteTag = 0x8000001C;
        private const string kTempFile = "tempRig.gr2";

        #region Bases
        public class ElementList<TElement> : AHandlerList<TElement>, IGenericAdd
            where TElement : IEquatable<TElement>
        {
            public ElementList(EventHandler handler)
                : base(handler)
            {
            }

            public bool Add(params object[] fields)
            {
                if (fields.Length == 1 && typeof(TElement).IsAssignableFrom(fields[0].GetType()))
                {
                    base.Add((TElement)fields[0]);
                    return true;
                }
                return false;
            }

            public void Add()
            {
                base.Add((TElement)Activator.CreateInstance(typeof(TElement), new object[] { 0, handler }));
            }
        }

        public abstract class GrannyDataElement<TGranny> : AHandlerElement
        {
            protected GrannyDataElement(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            protected GrannyDataElement(int APIversion, EventHandler handler, GrannyDataElement<TGranny> basis)
                : base(APIversion, handler)
            {
                Parse(basis.UnParse());
            }
            protected GrannyDataElement(int APIversion, EventHandler handler, TGranny data)
                : base(APIversion, handler)
            {
                Parse(data);
            }
            protected abstract void Parse(TGranny data);

            public abstract TGranny UnParse();
            public override AHandlerElement Clone(EventHandler handler)
            {
                return (AHandlerElement)Activator.CreateInstance(GetType(), new object[] { 0, handler, this });
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(kRecommendedApiVersion, GetType()); }
            }

            public abstract string Value { get; }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        }
        #endregion
    }
}
