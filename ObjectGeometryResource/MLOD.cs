using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
namespace s3piwrappers
{
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
        #region GroupList
        public class GroupList : AResource.DependentList<Group>
        {
            private MLOD mOwner;
            public GroupList(EventHandler handler, MLOD owner)
                : base(handler)
            {
                mOwner = owner;
            }

            public GroupList(EventHandler handler, MLOD owner, Stream s)
                : this(handler, owner)
            {
                Parse(s);
            }

            public override void Add()
            {
                ((IList<Group>)this).Add(new Group(0, this.elementHandler, mOwner));
            }

            protected override Group CreateElement(Stream s)
            {
                return new Group(0, handler, mOwner, s);
            }

            protected override void WriteElement(Stream s, Group element)
            {
                element.UnParse(s);
            }
        }
        #endregion

        #region Group

        public class Group : AHandlerElement, IEquatable<Group>
        {
            private UInt32 mGroupNameHash;
            private UInt32 mMATDIndex01;
            private UInt32 mVRTFIndex;
            private UInt32 mVBUFIndex;
            private UInt32 mIBUFIndex;
            private UInt32 mVBUFType;
            private UInt64 mVBUFOffset;
            private UInt64 mIBUFOffset;
            private UInt32 mVBUFCount;
            private UInt32 mIBUFCount;
            private UInt32 mSKINIndex;
            private UInt32 mMATDIndex02;


            private JointReferenceList mJointReferences;
            private BoundingBox mBounds;
            private GeometryStateList mGeometryStates;
            private Single mUnknown01;
            private Single mUnknown02;
            private Single mUnknown03;
            private Single mUnknown04;
            private Single mUnknown05;
            private MLOD mOwner;


            public Group(int APIversion, EventHandler handler, MLOD owner)
                : base(APIversion, handler)
            {
                mOwner = owner;
                mBounds = new BoundingBox(0, handler);
                mJointReferences = new JointReferenceList(handler);
                mGeometryStates = new GeometryStateList(handler);
            }
            public Group(int APIversion, EventHandler handler, MLOD owner, Stream s)
                : this(APIversion, handler, owner)
            {
                Parse(s);
            }
            public Group(int APIversion, EventHandler handler, Group basis)
                : base(APIversion, handler)
            {
                mOwner = basis.mOwner;
                Stream s = new MemoryStream();
                basis.UnParse(s);
                s.Position = 0L;
                Parse(s);
            }
            [ElementPriority(1)]
            public uint GroupNameHash
            {
                get { return mGroupNameHash; }
                set { mGroupNameHash = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public uint MatdIndex01
            {
                get { return mMATDIndex01; }
                set { mMATDIndex01 = value; OnElementChanged(); }
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
            public uint VbufType
            {
                get { return mVBUFType; }
                set { mVBUFType = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public ulong VbufOffset
            {
                get { return mVBUFOffset; }
                set { mVBUFOffset = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public ulong IbufOffset
            {
                get { return mIBUFOffset; }
                set { mIBUFOffset = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public uint VbufCount
            {
                get { return mVBUFCount; }
                set { mVBUFCount = value; OnElementChanged(); }
            }
            [ElementPriority(10)]
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
            [ElementPriority(13)]
            public uint SkinIndex
            {
                get { return mSKINIndex; }
                set { mSKINIndex = value; OnElementChanged(); }
            }

            [ElementPriority(14)]
            public uint MatdIndex02
            {
                get { return mMATDIndex02; }
                set { mMATDIndex02 = value; }
            }
            [ElementPriority(15)]
            public JointReferenceList JointReferences
            {
                get { return mJointReferences; }
                set { mJointReferences = value; OnElementChanged(); }
            }
            [ElementPriority(16)]
            public GeometryStateList GeometryStates
            {
                get { return mGeometryStates; }
                set { mGeometryStates = value; OnElementChanged(); }
            }
            [ElementPriority(17)]
            public float Unknown01
            {
                get { return mUnknown01; }
                set { mUnknown01 = value; OnElementChanged(); }
            }
            [ElementPriority(18)]
            public float Unknown02
            {
                get { return mUnknown02; }
                set { mUnknown02 = value; OnElementChanged(); }
            }
            [ElementPriority(19)]
            public float Unknown03
            {
                get { return mUnknown03; }
                set { mUnknown03 = value; OnElementChanged(); }
            }
            [ElementPriority(20)]
            public float Unknown04
            {
                get { return mUnknown04; }
                set { mUnknown04 = value; OnElementChanged(); }
            }
            [ElementPriority(21)]
            public float Unknown05
            {
                get { return mUnknown05; }
                set { mUnknown05 = value; OnElementChanged(); }
            }




            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                long expectedSize = br.ReadUInt32();
                long start = s.Position;
                mGroupNameHash = br.ReadUInt32();
                mMATDIndex01 = br.ReadUInt32();
                mVRTFIndex = br.ReadUInt32();
                mVBUFIndex = br.ReadUInt32();
                mIBUFIndex = br.ReadUInt32();
                mVBUFType = br.ReadUInt32();
                mVBUFOffset = br.ReadUInt64();
                mIBUFOffset = br.ReadUInt64();
                mVBUFCount = br.ReadUInt32();
                mIBUFCount = br.ReadUInt32();
                mBounds = new BoundingBox(0, handler, s);
                mSKINIndex = br.ReadUInt32();
                mJointReferences = new JointReferenceList(handler, s);
                mMATDIndex02 = br.ReadUInt32();
                mGeometryStates = new GeometryStateList(handler, s);
                if (mOwner.Version > 0x00000201)
                {
                    mUnknown01 = br.ReadSingle();
                    mUnknown02 = br.ReadSingle();
                    mUnknown03 = br.ReadSingle();
                    mUnknown04 = br.ReadSingle();
                    mUnknown05 = br.ReadSingle();
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
                bw.Write(mGroupNameHash);
                bw.Write(mMATDIndex01);
                bw.Write(mVRTFIndex);
                bw.Write(mVBUFIndex);
                bw.Write(mIBUFIndex);
                bw.Write(mVBUFType);
                bw.Write(mVBUFOffset);
                bw.Write(mIBUFOffset);
                bw.Write(mVBUFCount);
                bw.Write(mIBUFCount);
                mBounds.UnParse(s);
                bw.Write(mSKINIndex);
                mJointReferences.UnParse(s);
                bw.Write(mMATDIndex02);
                mGeometryStates.UnParse(s);
                if (mOwner.Version > 0x00000201)
                {
                    bw.Write(mUnknown01);
                    bw.Write(mUnknown02);
                    bw.Write(mUnknown03);
                    bw.Write(mUnknown04);
                    bw.Write(mUnknown05);
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
                    sb.AppendFormat("Name: 0x{0:X8}\n", mGroupNameHash);
                    sb.AppendFormat("Material01 Index: 0x{0:X8}\n", mMATDIndex01);
                    sb.AppendFormat("VRTF: 0x{0:X8}\n", mVRTFIndex);
                    sb.AppendFormat("VBUF: 0x{0:X8}\n", mVBUFIndex);
                    sb.AppendFormat("VBUF Offset: 0x{0:X16}\n", mVBUFOffset);
                    sb.AppendFormat("IBUF Offset: 0x{0:X16}\n", mIBUFOffset);
                    sb.AppendFormat("Vertex Count: {0}\n", mVBUFCount);
                    sb.AppendFormat("Face Count: {0}\n", mIBUFCount);
                    sb.AppendFormat("Bounds:\n{0}\n", mBounds.Value);
                    sb.AppendFormat("SKIN: 0x{0:X8}\n", mSKINIndex);

                    if (mJointReferences.Count > 0)
                    {
                        sb.AppendFormat("Joint References:\n");
                        for (int i = 0; i < mJointReferences.Count; i++)
                        {
                            sb.AppendFormat("[{0:00}]\t{1}\n", i, mJointReferences[i].Value);
                        }
                    }
                    sb.AppendFormat("Material02 Index: 0x{0:X8}\n", mMATDIndex02);
                    if (mGeometryStates.Count > 0)
                    {
                        sb.AppendFormat("Geometry States:\n");
                        for (int i = 0; i < mGeometryStates.Count; i++)
                        {
                            sb.AppendFormat("=Geometry State[{0}]=\n{1}\n", i, mGeometryStates[i].Value);
                        }
                    }
                    sb.AppendFormat("Unknown01: {0,8:0.00000}\n", mUnknown01);
                    sb.AppendFormat("Unknown02: {0,8:0.00000}\n", mUnknown02);
                    sb.AppendFormat("Unknown03: {0,8:0.00000}\n", mUnknown03);
                    sb.AppendFormat("Unknown04: {0,8:0.00000}\n", mUnknown04);
                    sb.AppendFormat("Unknown05: {0,8:0.00000}\n", mUnknown05);
                    return sb.ToString();
                }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Group(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get
                {
                    var fields = GetContentFields(base.requestedApiVersion, GetType());
                    if (mOwner.Version < 0x00000202)
                    {
                        fields.Remove("Unknown01");
                        fields.Remove("Unknown02");
                        fields.Remove("Unknown03");
                        fields.Remove("Unknown04");
                        fields.Remove("Unknown05");
                    }
                    return fields;
                }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(Group other)
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
            mGroups = new GroupList(handler, this);
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
        public GroupList Groups
        {
            get { return mGroups; }
            set { mGroups = value; OnRCOLChanged(this, new EventArgs()); }
        }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                if (mGroups.Count > 0)
                {
                    sb.AppendFormat("Groups:\n");
                    for (int i = 0; i < mGroups.Count; i++)
                    {
                        sb.AppendFormat("==Group[{0}]==\n{1}\n", i, mGroups[i].Value);
                    }
                }
                return sb.ToString();
            }
        }

        private UInt32 mVersion;
        private GroupList mGroups;
        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            mGroups = new GroupList(handler, this, s);
        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((uint)FOURCC(Tag));
            bw.Write(mVersion);
            if (mGroups == null) mGroups = new GroupList(handler, this);
            mGroups.UnParse(s);
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