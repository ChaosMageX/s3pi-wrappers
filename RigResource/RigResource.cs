using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Settings;
using s3pi.Interfaces;
using System.IO;
namespace s3piwrappers
{
    public class RigResource : AResource
    {
        public class GrannyBoneReferenceList : DependentList<GrannyBoneReference>
        {
            public GrannyBoneReferenceList(EventHandler handler)
                : base(handler)
            {
            }

            public GrannyBoneReferenceList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override GrannyBoneReference CreateElement(Stream s)
            {
                return new GrannyBoneReference(0, handler, s);
            }

            protected override void WriteElement(Stream s, GrannyBoneReference element)
            {
                element.UnParse(s);
            }
        }
        public class GrannyBoneReference : AHandlerElement, IEquatable<GrannyBoneReference>
        {
            private Int32 mIndex;

            public GrannyBoneReference(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public GrannyBoneReference(int APIversion, EventHandler handler, GrannyBoneReference basis)
                : base(APIversion, handler)
            {
                mIndex = basis.mIndex;
            }

            public GrannyBoneReference(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            [ElementPriority(1)]
            public Int32 Index
            {
                get { return mIndex; }
                set { mIndex = value; OnElementChanged(); }
            }

            private void Parse(Stream s)
            {
                mIndex = new BinaryReader(s).ReadInt32();
            }
            public void UnParse(Stream s)
            {
                new BinaryWriter(s).Write(mIndex);
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new GrannyBoneReference(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(kRecommendedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(GrannyBoneReference other)
            {
                return mIndex.Equals(other.mIndex);
            }
        }
        public class IkLinkList : AHandlerList<IkLink>, IGenericAdd
        {
            public IkLinkList(EventHandler handler)
                : base(handler)
            {
            }

            public bool Add(params object[] fields)
            {
                if (fields.Length == 1 && typeof(IkLink).IsAssignableFrom(fields[0].GetType()))
                {
                    base.Add((IkLink)fields[0]);
                    return true;
                }
                return false;
            }

            public void Add()
            {
                base.Add(new IkLink(0, handler));
            }
        }
        public class IkLink : AHandlerElement, IEquatable<IkLink>
        {
            private UInt32 mUnknown;
            private Int32 mEnd;
            private Int32 mStart;

            public IkLink(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public IkLink(int APIversion, EventHandler handler, IkLink basis)
                : this(APIversion, handler, basis.mUnknown, basis.mEnd, basis.mStart)
            {
            }
            public IkLink(int APIversion, EventHandler handler, UInt32 unk, Int32 end, Int32 start)
                : base(APIversion, handler)
            {
                mUnknown = unk;
                mEnd = end;
                mStart = start;
            }

            [ElementPriority(1)]
            public uint Unknown
            {
                get { return mUnknown; }
                set { mUnknown = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public Int32 End
            {
                get { return mEnd; }
                set { mEnd = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Int32 Start
            {
                get { return mStart; }
                set { mStart = value; OnElementChanged(); }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IkLink(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(kRecommendedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(IkLink other)
            {
                return mStart.Equals(other.mStart) && mEnd.Equals(other.mEnd);
            }
        }
        public class IkChainList : DependentList<IkChain>
        {
            public IkChainList(EventHandler handler)
                : base(handler)
            {
            }

            public IkChainList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override void Parse(Stream s)
            {
                var br = new BinaryReader(s);
                uint count = ReadCount(s);
                long[] offsets = new long[count];
                long startOffset = s.Position;
                for (int i = 0; i < count; i++)
                {
                    offsets[i] = startOffset + br.ReadUInt32();
                }
                for (int i = 0; i < count; i++)
                {
                    if (s.Position != offsets[i])
                        throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", offsets[i], s.Position));
                    ((IList<IkChain>)this).Add(CreateElement(s));
                }
            }
            public override void UnParse(Stream s)
            {
                var bw = new BinaryWriter(s);
                WriteCount(s, (uint)base.Count);
                long[] offsets = new long[Count];
                long startOffset = s.Position;
                s.Seek(4 * Count, SeekOrigin.Current);
                for (int i = 0; i < Count; i++)
                {
                    offsets[i] = s.Position - startOffset;
                    WriteElement(s, this[i]);
                }
                long endPos = s.Position;
                s.Seek(startOffset, SeekOrigin.Begin);
                for (int i = 0; i < offsets.Length; i++)
                {
                    bw.Write((uint)(offsets[i]));
                }
                s.Seek(endPos, SeekOrigin.Begin);


            }
            protected override IkChain CreateElement(Stream s)
            {
                return new IkChain(0, handler, s);
            }

            protected override void WriteElement(Stream s, IkChain element)
            {
                element.UnParse(s);
            }
        }
        public class IkChain : AHandlerElement, IEquatable<IkChain>
        {
            private UInt32 mUnknown01;
            private Byte[] mUnknown02;
            private Int32 mIkPole;
            private Int32 mIkPoleRoot;
            private Int32 mSlotInfo;
            private Int32 mSlotInfoRoot;
            private Int32 mSlotOffset;
            private Int32 mSlotOffsetRoot;
            private UInt32 mUnknown03;
            private Int32 mInfoNode01;
            private Int32 mInfoNode02;
            private Int32 mInfoNode03;
            private Int32 mInfoNode04;
            private Int32 mInfoNode05;
            private Int32 mInfoNode06;
            private Int32 mInfoNode07;
            private Int32 mInfoNode08;
            private Int32 mInfoNode09;
            private Int32 mInfoNode10;
            private Int32 mInfoRoot;
            private UInt32 mUnknown04;
            private IkLinkList mLinks;

            [ElementPriority(1)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set { mUnknown01 = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public byte[] Unknown02
            {
                get { return mUnknown02; }
                set { mUnknown02 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Int32 IkPole
            {
                get { return mIkPole; }
                set { mIkPole = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public Int32 IkPoleRoot
            {
                get { return mIkPoleRoot; }
                set { mIkPoleRoot = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public Int32 SlotInfo
            {
                get { return mSlotInfo; }
                set { mSlotInfo = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public Int32 SlotInfoRoot
            {
                get { return mSlotInfoRoot; }
                set { mSlotInfoRoot = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public Int32 SlotOffset
            {
                get { return mSlotOffset; }
                set { mSlotOffset = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public Int32 SlotOffsetRoot
            {
                get { return mSlotOffsetRoot; }
                set { mSlotOffsetRoot = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public uint Unknown03
            {
                get { return mUnknown03; }
                set { mUnknown03 = value; OnElementChanged(); }
            }
            [ElementPriority(10)]
            public Int32 InfoNode01
            {
                get { return mInfoNode01; }
                set { mInfoNode01 = value; OnElementChanged(); }
            }
            [ElementPriority(11)]
            public Int32 InfoNode02
            {
                get { return mInfoNode02; }
                set { mInfoNode02 = value; OnElementChanged(); }
            }
            [ElementPriority(12)]
            public Int32 InfoNode03
            {
                get { return mInfoNode03; }
                set { mInfoNode03 = value; OnElementChanged(); }
            }
            [ElementPriority(13)]
            public Int32 InfoNode04
            {
                get { return mInfoNode04; }
                set { mInfoNode04 = value; OnElementChanged(); }
            }
            [ElementPriority(14)]
            public Int32 InfoNode05
            {
                get { return mInfoNode05; }
                set { mInfoNode05 = value; OnElementChanged(); }
            }
            [ElementPriority(15)]
            public Int32 InfoNode06
            {
                get { return mInfoNode06; }
                set { mInfoNode06 = value; OnElementChanged(); }
            }
            [ElementPriority(16)]
            public Int32 InfoNode07
            {
                get { return mInfoNode07; }
                set { mInfoNode07 = value; OnElementChanged(); }
            }
            [ElementPriority(17)]
            public Int32 InfoNode08
            {
                get { return mInfoNode08; }
                set { mInfoNode08 = value; OnElementChanged(); }
            }
            [ElementPriority(18)]
            public Int32 InfoNode09
            {
                get { return mInfoNode09; }
                set { mInfoNode09 = value; OnElementChanged(); }
            }
            [ElementPriority(19)]
            public Int32 InfoNode10
            {
                get { return mInfoNode10; }
                set { mInfoNode10 = value; OnElementChanged(); }
            }
            [ElementPriority(20)]
            public Int32 InfoRoot
            {
                get { return mInfoRoot; }
                set { mInfoRoot = value; OnElementChanged(); }
            }
            [ElementPriority(21)]
            public uint Unknown04
            {
                get { return mUnknown04; }
                set { mUnknown04 = value; OnElementChanged(); }
            }
            [ElementPriority(22)]
            public IkLinkList Links
            {
                get { return mLinks; }
                set { mLinks = value; OnElementChanged(); }
            }

            private void Parse(Stream s)
            {
                var br = new BinaryReader(s);
                mUnknown01 = br.ReadUInt32();
                uint linkCount = br.ReadUInt32();
                mUnknown02 = br.ReadBytes(32);
                mIkPole = br.ReadInt32();
                mIkPoleRoot = br.ReadInt32();
                mSlotInfo = br.ReadInt32();
                mSlotInfoRoot = br.ReadInt32();
                mSlotOffset = br.ReadInt32();
                mSlotOffsetRoot = br.ReadInt32();
                mUnknown03 = br.ReadUInt32();
                mInfoNode01 = br.ReadInt32();
                mInfoNode02 = br.ReadInt32();
                mInfoNode03 = br.ReadInt32();
                mInfoNode04 = br.ReadInt32();
                mInfoNode05 = br.ReadInt32();
                mInfoNode06 = br.ReadInt32();
                mInfoNode07 = br.ReadInt32();
                mInfoNode08 = br.ReadInt32();
                mInfoNode09 = br.ReadInt32();
                mInfoNode10 = br.ReadInt32();
                mInfoRoot = br.ReadInt32();
                mLinks = new IkLinkList(handler);
                uint[] linkUnks = new uint[linkCount];
                for (int i = 0; i < linkCount; i++)
                {
                    linkUnks[i] = br.ReadUInt32();
                }
                mUnknown04 = br.ReadUInt32();
                for (int i = 0; i < linkCount; i++)
                {
                    Int32 end = br.ReadInt32();
                    Int32 start = br.ReadInt32();
                    mLinks.Add(new IkLink(0, handler, linkUnks[i], end, start));
                }
            }
            public void UnParse(Stream s)
            {
                var bw = new BinaryWriter(s);
                bw.Write(mUnknown01);
                bw.Write(mLinks.Count);
                bw.Write(mUnknown02);
                bw.Write(mIkPole);
                bw.Write(mIkPoleRoot);
                bw.Write(mSlotInfo);
                bw.Write(mSlotInfoRoot);
                bw.Write(mSlotOffset);
                bw.Write(mSlotOffsetRoot);
                bw.Write(mUnknown03);
                bw.Write(mInfoNode01);
                bw.Write(mInfoNode02);
                bw.Write(mInfoNode03);
                bw.Write(mInfoNode04);
                bw.Write(mInfoNode05);
                bw.Write(mInfoNode06);
                bw.Write(mInfoNode07);
                bw.Write(mInfoNode08);
                bw.Write(mInfoNode09);
                bw.Write(mInfoNode10);
                bw.Write(mInfoRoot);
                for (int i = 0; i < mLinks.Count; i++)
                {
                    bw.Write(mLinks[i].Unknown);
                }
                bw.Write(mUnknown04);
                for (int i = 0; i < mLinks.Count; i++)
                {
                    bw.Write((uint)mLinks[i].End);
                    bw.Write((uint)mLinks[i].Start);
                }
            }

            public IkChain(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mLinks = new IkLinkList(handler);
                mUnknown02 = new byte[32];

            }
            public IkChain(int APIversion, EventHandler handler, IkChain basis)
                : base(APIversion, handler)
            {
                MemoryStream s = new MemoryStream();
                basis.UnParse(s);
                s.Position = 0L;
                Parse(s);
            }

            public IkChain(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IkChain(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(kRecommendedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }



            public bool Equals(IkChain other)
            {
                return base.Equals(other);
            }
        }
        public RigResource(int APIversion, Stream s)
            : base(APIversion, s)
        {
            if (base.stream == null)
            {
                base.stream = this.UnParse();
                this.OnResourceChanged(this, new EventArgs());
            }
            base.stream.Position = 0L;
            Parse(base.stream);
        }

        private Boolean mHasIkData;
        private UInt32 mUnknown01;
        private Byte[] mUnknown02;
        private RigData mRigData;
        private IkChainList mIkChains;
        private Byte[] mUnknown03;
        private GrannyBoneReferenceList mIkTargets;
        private GrannyBoneReferenceList mInfoNodes;
        private GrannyBoneReferenceList mCompressNodes;
        private GrannyBoneReferenceList mBones;
        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (hasGranny2Dll) sb.AppendFormat("===Skeleton Data===\n{0}\n", mRigData.Value);
                if (!HasIkData) return sb.ToString();
                sb.AppendLine("===IK Data===");
                sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                sb.AppendFormat("Unknown02:\n");
                for (int i = 0; i < mUnknown02.Length; i++)
                {
                    sb.AppendFormat("{0:X2}", mUnknown02[i]);
                }
                sb.Append("\n");
                if (mIkChains.Count > 0)
                {
                    sb.AppendFormat("Ik Chains:\n");
                    for (int i = 0; i < mIkChains.Count; i++)
                    {
                        var ikc = mIkChains[i];
                        sb.AppendFormat("==Ik Chain[0x{0:X8}]==\n", i);
                        sb.AppendFormat("Unknown01:\t0x{0:X8}\n", ikc.Unknown01);
                        sb.AppendFormat("Unknown02:\n");
                        for (int j = 0; j < mIkChains[i].Unknown02.Length; j++)
                        {
                            sb.AppendFormat("{0:X2}", ikc.Unknown02[j]);
                        }
                        sb.Append("\n");
                        sb.AppendFormat("IkPole:\t{0}\n", GetBoneIndexName(ikc.IkPole));
                        sb.AppendFormat("IkPoleRoot:\t{0}\n", GetBoneIndexName(ikc.IkPoleRoot));
                        sb.AppendFormat("SlotInfo:\t{0}\n", GetBoneIndexName(ikc.SlotInfo));
                        sb.AppendFormat("SlotInfoRoot:\t{0}\n", GetBoneIndexName(ikc.SlotInfoRoot));
                        sb.AppendFormat("SlotOffset:\t{0}\n", GetBoneIndexName(ikc.SlotOffset));
                        sb.AppendFormat("SlotOffsetRoot:\t{0}\n", GetBoneIndexName(ikc.SlotOffsetRoot));
                        sb.AppendFormat("Unknown03:\t0x{0:X8}\n", ikc.Unknown03);
                        sb.AppendFormat("InfoNode01:\t{0}\n", GetBoneIndexName(ikc.InfoNode01));
                        sb.AppendFormat("InfoNode02:\t{0}\n", GetBoneIndexName(ikc.InfoNode02));
                        sb.AppendFormat("InfoNode03:\t{0}\n", GetBoneIndexName(ikc.InfoNode03));
                        sb.AppendFormat("InfoNode04:\t{0}\n", GetBoneIndexName(ikc.InfoNode04));
                        sb.AppendFormat("InfoNode05:\t{0}\n", GetBoneIndexName(ikc.InfoNode05));
                        sb.AppendFormat("InfoNode06:\t{0}\n", GetBoneIndexName(ikc.InfoNode06));
                        sb.AppendFormat("InfoNode07:\t{0}\n", GetBoneIndexName(ikc.InfoNode07));
                        sb.AppendFormat("InfoNode08:\t{0}\n", GetBoneIndexName(ikc.InfoNode08));
                        sb.AppendFormat("InfoNode09:\t{0}\n", GetBoneIndexName(ikc.InfoNode09));
                        sb.AppendFormat("InfoNode10:\t{0}\n", GetBoneIndexName(ikc.InfoNode10));
                        sb.AppendFormat("InfoRoot:\t{0}\n", GetBoneIndexName(ikc.InfoRoot));
                        sb.AppendFormat("Unknown04:\t0x{0:X8}\n", ikc.Unknown04);
                        if (ikc.Links.Count > 0)
                        {
                            sb.AppendFormat("Ik Links:\n");
                            for (int j = 0; j < ikc.Links.Count; j++)
                            {
                                var ikcl = ikc.Links[j];
                                sb.AppendFormat("==Link[{0}]==", j);
                                sb.AppendFormat("Unknown01:\t0x{0:X8}\n", ikcl.Unknown);
                                sb.AppendFormat("End:\t{0}\n", GetBoneIndexName(ikcl.End));
                                sb.AppendFormat("Start:\t{0}\n", GetBoneIndexName(ikcl.Start));
                            }
                        }
                    }
                }
                sb.AppendFormat("Unknown03:\n");
                for (int i = 0; i < mUnknown03.Length; i++)
                {
                    sb.AppendFormat("{0:X2}", mUnknown03[i]);
                }
                sb.Append("\n");
                if (mIkTargets.Count > 0)
                {
                    sb.AppendFormat("Ik Targets:\n");
                    for (int i = 0; i < mIkTargets.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, GetBoneIndexName(mIkTargets[i].Index));
                    }
                }
                if (mInfoNodes.Count > 0)
                {
                    sb.AppendFormat("Info Nodes:\n");
                    for (int i = 0; i < mInfoNodes.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, GetBoneIndexName(mInfoNodes[i].Index));
                    }
                }
                if (mCompressNodes.Count > 0)
                {
                    sb.AppendFormat("Compress Nodes:\n");
                    for (int i = 0; i < mCompressNodes.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, GetBoneIndexName(mCompressNodes[i].Index));
                    }
                }
                if (mBones.Count > 0)
                {
                    sb.AppendFormat("Bones:\n");
                    for (int i = 0; i < mBones.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, GetBoneIndexName(mBones[i].Index));
                    }
                }
                return sb.ToString();
            }
        }
        private string GetBoneIndexName(Int32 Index)
        {
            if (Index == -1) return "NULL";
            if (!hasGranny2Dll) return "0x" + Index.ToString("X8");
            GrannyRigData grd = mRigData as GrannyRigData;
            if (Index >= grd.Skeleton.Bones.Count) return "INVALID";
            return grd.Skeleton.Bones[Index].Name;
        }


        [ElementPriority(1)]
        public bool HasIkData
        {
            get { return mHasIkData; }
            set { mHasIkData = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(2)]
        public RigData RigData
        {
            get { return mRigData; }
            set { mRigData = value; OnResourceChanged(this, new EventArgs()); }
        }

        [ElementPriority(3)]
        public uint Unknown01
        {
            get { return mUnknown01; }
            set { mUnknown01 = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        public byte[] Unknown02
        {
            get { return mUnknown02; }
            set { mUnknown02 = value; OnResourceChanged(this, new EventArgs()); }
        }

        [ElementPriority(5)]
        public IkChainList IkChains
        {
            get { return mIkChains; }
            set { mIkChains = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(6)]
        public byte[] Unknown03
        {
            get { return mUnknown03; }
            set { mUnknown03 = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(7)]
        public GrannyBoneReferenceList IkTargets
        {
            get { return mIkTargets; }
            set { mIkTargets = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(8)]
        public GrannyBoneReferenceList InfoNodes
        {
            get { return mInfoNodes; }
            set { mInfoNodes = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(9)]
        public GrannyBoneReferenceList CompressNodes
        {
            get { return mCompressNodes; }
            set { mCompressNodes = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(10)]
        public GrannyBoneReferenceList Bones
        {
            get { return mBones; }
            set { mBones = value; OnResourceChanged(this, new EventArgs()); }
        }

        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            UInt32 type = br.ReadUInt32();
            mHasIkData = type == 0x8EAF13DE;
            if (!mHasIkData)
            {

                mUnknown02 = new byte[16];
                mUnknown03 = new byte[56];
                mIkChains = new IkChainList(this.OnResourceChanged);
                mIkTargets = new GrannyBoneReferenceList(this.OnResourceChanged);
                mInfoNodes = new GrannyBoneReferenceList(this.OnResourceChanged);
                mCompressNodes = new GrannyBoneReferenceList(this.OnResourceChanged);
                mBones = new GrannyBoneReferenceList(this.OnResourceChanged);

                s.Position = 0L;
                mRigData = CreateRigData(0, this.OnResourceChanged, s);
                return;
            }
            mUnknown01 = br.ReadUInt32();
            int grannySize = br.ReadInt32();
            long grannyOffset = br.ReadUInt32() + s.Position - 4;
            long ikChainsOffset = br.ReadUInt32() + s.Position - 4;
            long ikTargetsOffset = br.ReadUInt32() + s.Position - 4;
            long infoNodesOffset = br.ReadUInt32() + s.Position - 4;
            long compressNodesOffset = br.ReadUInt32() + s.Position - 4;
            long fullBoneListOffset = br.ReadUInt32() + s.Position - 4;
            mUnknown02 = br.ReadBytes(16);
            if (checking && s.Position != grannyOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", grannyOffset, s.Position));

            byte[] buffer = br.ReadBytes(grannySize);
            mRigData = CreateRigData(0, OnResourceChanged, new MemoryStream(buffer));

            s.Seek(4 - (s.Position % 4), SeekOrigin.Current); //correct pos
            if (checking && s.Position != ikChainsOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", ikChainsOffset, s.Position));
            mIkChains = new IkChainList(this.OnResourceChanged, s);
            mUnknown03 = br.ReadBytes(56);
            if (checking && s.Position != ikTargetsOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", ikTargetsOffset, s.Position));
            mIkTargets = new GrannyBoneReferenceList(this.OnResourceChanged, s);
            if (checking && s.Position != infoNodesOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", infoNodesOffset, s.Position));
            mInfoNodes = new GrannyBoneReferenceList(this.OnResourceChanged, s);
            if (checking && s.Position != compressNodesOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", compressNodesOffset, s.Position));
            mCompressNodes = new GrannyBoneReferenceList(this.OnResourceChanged, s);
            if (checking && s.Position != fullBoneListOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", fullBoneListOffset, s.Position));
            mBones = new GrannyBoneReferenceList(this.OnResourceChanged, s);


        }
        protected override Stream UnParse()
        {
            if (mRigData == null) mRigData = RigResource.CreateRigData(0, OnResourceChanged);
            if (!mHasIkData)
            {
                return mRigData.UnParse();
            }
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);

            long grannyOffset = 0L;
            long ikChainsOffset = 0L;
            long ikTargetsOffset = 0L;
            long infoNodesOffset = 0L;
            long compressNodesOffset = 0L;
            long fullBoneListOffset = 0L;
            if (mUnknown02 == null) mUnknown02 = new byte[16];
            if (mUnknown03 == null) mUnknown03 = new byte[56];
            if (mIkChains == null) mIkChains = new IkChainList(this.OnResourceChanged);
            if (mIkTargets == null) mIkTargets = new GrannyBoneReferenceList(this.OnResourceChanged);
            if (mInfoNodes == null) mInfoNodes = new GrannyBoneReferenceList(this.OnResourceChanged);
            if (mCompressNodes == null) mCompressNodes = new GrannyBoneReferenceList(this.OnResourceChanged);
            if (mBones == null) mBones = new GrannyBoneReferenceList(this.OnResourceChanged);

            bw.Write(0x8EAF13DE);
            bw.Write(mUnknown01);
            byte[] buffer = null;


            Stream gr2 = mRigData.UnParse();
            buffer = new byte[gr2.Length];
            gr2.Read(buffer, 0, buffer.Length);
            gr2.Close();
            bw.Write(buffer.Length);


            long offsetPosition = s.Position;
            s.Seek(24, SeekOrigin.Current);
            bw.Write(mUnknown02);

            grannyOffset = s.Position;

            bw.Write(buffer);

            bw.Write(new byte[4 - (s.Position % 4)]);


            ikChainsOffset = s.Position;
            mIkChains.UnParse(s);
            bw.Write(mUnknown03);

            ikTargetsOffset = s.Position;
            mIkTargets.UnParse(s);

            infoNodesOffset = s.Position;
            mInfoNodes.UnParse(s);

            compressNodesOffset = s.Position;
            mCompressNodes.UnParse(s);

            fullBoneListOffset = s.Position;
            mBones.UnParse(s);
            long endPos = s.Position;
            s.Seek(offsetPosition, SeekOrigin.Begin);

            bw.Write((uint)(grannyOffset - s.Position));
            bw.Write((uint)(ikChainsOffset - s.Position));
            bw.Write((uint)(ikTargetsOffset - s.Position));
            bw.Write((uint)(infoNodesOffset - s.Position));
            bw.Write((uint)(compressNodesOffset - s.Position));
            bw.Write((uint)(fullBoneListOffset - s.Position));
            s.Seek(endPos, SeekOrigin.Begin);
            return s;
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedApiVersion; }
        }
        public override List<string> ContentFields
        {
            get
            {
                if (!mHasIkData)
                {
                    return new List<string>() { "RigData", "HasIkData" };
                }
                else
                {
                    var contentFields = GetContentFields(base.requestedApiVersion, GetType());
                    return contentFields;
                }
            }
        }
        static RigData CreateRigData(int apiVersion, EventHandler handler, Stream s)
        {
            return hasGranny2Dll ? (RigData)new GrannyRigData(apiVersion, handler, s) : (RigData)new RawRigData(apiVersion, handler, s);
        }
        static RigData CreateRigData(int apiVersion, EventHandler handler)
        {
            return hasGranny2Dll ? (RigData)new GrannyRigData(apiVersion, handler) : (RigData)new RawRigData(apiVersion, handler);
        }
        static RigResource()
        {
            string dir = Path.GetDirectoryName(typeof(RigResource).Assembly.Location);
            hasGranny2Dll = File.Exists(Path.Combine(dir, "granny2.dll"));
        }
        private static bool hasGranny2Dll = false;
        private const int kRecommendedApiVersion = 1;
        private static bool checking = Settings.Checking;
    }
}
