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
            private BoneIndex mIndex;

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
            public BoneIndex Index
            {
                get { return mIndex; }
                set { mIndex = value; OnElementChanged(); }
            }

            private void Parse(Stream s)
            {
                mIndex = (BoneIndex)new BinaryReader(s).ReadUInt32();
            }
            public void UnParse(Stream s)
            {
                new BinaryWriter(s).Write((uint)mIndex);
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
            private BoneIndex mEnd;
            private BoneIndex mStart;

            public IkLink(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public IkLink(int APIversion, EventHandler handler, IkLink basis)
                : this(APIversion, handler, basis.mUnknown, basis.mEnd, basis.mStart)
            {
            }
            public IkLink(int APIversion, EventHandler handler, UInt32 unk, BoneIndex end, BoneIndex start)
                : base(APIversion, handler)
            {
                mUnknown = unk;
                mEnd = end;
                mStart = start;
            }
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Unknown:\t0x{0:X8}\n", mUnknown);
                    sb.AppendFormat("End:\t{0}\n", mEnd);
                    sb.AppendFormat("Start:\t{0}\n", mStart);
                    return sb.ToString();
                }
            }
            [ElementPriority(1)]
            public uint Unknown
            {
                get { return mUnknown; }
                set { mUnknown = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public BoneIndex End
            {
                get { return mEnd; }
                set { mEnd = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public BoneIndex Start
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
            public IkChainList(EventHandler handler) : base(handler)
            {
            }

            public IkChainList(EventHandler handler, Stream s) : base(handler, s)
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
                    offsets[i] = startOffset+ br.ReadUInt32();
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
            private BoneIndex mIkPole;
            private BoneIndex mIkPoleRoot;
            private BoneIndex mSlotInfo;
            private BoneIndex mSlotInfoRoot;
            private BoneIndex mSlotOffset;
            private BoneIndex mSlotOffsetRoot;
            private UInt32 mUnknown03;
            private BoneIndex mInfoNode01;
            private BoneIndex mInfoNode02;
            private BoneIndex mInfoNode03;
            private BoneIndex mInfoNode04;
            private BoneIndex mInfoNode05;
            private BoneIndex mInfoNode06;
            private BoneIndex mInfoNode07;
            private BoneIndex mInfoNode08;
            private BoneIndex mInfoNode09;
            private BoneIndex mInfoNode10;
            private BoneIndex mInfoRoot;
            private UInt32 mUnknown04;
            private IkLinkList mLinks;
            public String Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var field in ContentFields)
                    {
                        if (field == "Unknown02")
                        {
                            sb.AppendFormat("Unknown02:\n");
                            for (int i = 0; i < mUnknown02.Length; i++)
                            {
                                sb.AppendFormat("{0:X2}", mUnknown02[i]);
                            }
                            sb.Append("\n");
                        }
                        else if (field == "Links")
                        {
                            if (mLinks.Count > 0)
                            {
                                sb.AppendFormat("Ik Links:\n");
                                for (int i = 0; i < mLinks.Count; i++)
                                {
                                    sb.AppendFormat("==Link[{0}]==\n{1}", i, mLinks[i].Value);
                                }
                            }
                        }
                        else if(field == "Value")
                        {
                            continue;
                        }
                        else
                        {
                            sb.AppendFormat("{0}:\t{1}\n",field,this[field]);
                        }
                    }
                    return sb.ToString();
                }
            }
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
            public BoneIndex IkPole
            {
                get { return mIkPole; }
                set { mIkPole = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public BoneIndex IkPoleRoot
            {
                get { return mIkPoleRoot; }
                set { mIkPoleRoot = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public BoneIndex SlotInfo
            {
                get { return mSlotInfo; }
                set { mSlotInfo = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public BoneIndex SlotInfoRoot
            {
                get { return mSlotInfoRoot; }
                set { mSlotInfoRoot = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public BoneIndex SlotOffset
            {
                get { return mSlotOffset; }
                set { mSlotOffset = value; OnElementChanged(); }
            }
            [ElementPriority(8)]
            public BoneIndex SlotOffsetRoot
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
            public BoneIndex InfoNode01
            {
                get { return mInfoNode01; }
                set { mInfoNode01 = value; OnElementChanged(); }
            }
            [ElementPriority(11)]
            public BoneIndex InfoNode02
            {
                get { return mInfoNode02; }
                set { mInfoNode02 = value; OnElementChanged(); }
            }
            [ElementPriority(12)]
            public BoneIndex InfoNode03
            {
                get { return mInfoNode03; }
                set { mInfoNode03 = value; OnElementChanged(); }
            }
            [ElementPriority(13)]
            public BoneIndex InfoNode04
            {
                get { return mInfoNode04; }
                set { mInfoNode04 = value; OnElementChanged(); }
            }
            [ElementPriority(14)]
            public BoneIndex InfoNode05
            {
                get { return mInfoNode05; }
                set { mInfoNode05 = value; OnElementChanged(); }
            }
            [ElementPriority(15)]
            public BoneIndex InfoNode06
            {
                get { return mInfoNode06; }
                set { mInfoNode06 = value; OnElementChanged(); }
            }
            [ElementPriority(16)]
            public BoneIndex InfoNode07
            {
                get { return mInfoNode07; }
                set { mInfoNode07 = value; OnElementChanged(); }
            }
            [ElementPriority(17)]
            public BoneIndex InfoNode08
            {
                get { return mInfoNode08; }
                set { mInfoNode08 = value; OnElementChanged(); }
            }
            [ElementPriority(18)]
            public BoneIndex InfoNode09
            {
                get { return mInfoNode09; }
                set { mInfoNode09 = value; OnElementChanged(); }
            }
            [ElementPriority(19)]
            public BoneIndex InfoNode10
            {
                get { return mInfoNode10; }
                set { mInfoNode10 = value; OnElementChanged(); }
            }
            [ElementPriority(20)]
            public BoneIndex InfoRoot
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
                mIkPole = (BoneIndex)br.ReadUInt32();
                mIkPoleRoot = (BoneIndex)br.ReadUInt32();
                mSlotInfo = (BoneIndex)br.ReadUInt32();
                mSlotInfoRoot = (BoneIndex)br.ReadUInt32();
                mSlotOffset = (BoneIndex)br.ReadUInt32();
                mSlotOffsetRoot = (BoneIndex)br.ReadUInt32();
                mUnknown03 = br.ReadUInt32();
                mInfoNode01 = (BoneIndex)br.ReadUInt32();
                mInfoNode02 = (BoneIndex)br.ReadUInt32();
                mInfoNode03 = (BoneIndex)br.ReadUInt32();
                mInfoNode04 = (BoneIndex)br.ReadUInt32();
                mInfoNode05 = (BoneIndex)br.ReadUInt32();
                mInfoNode06 = (BoneIndex)br.ReadUInt32();
                mInfoNode07 = (BoneIndex)br.ReadUInt32();
                mInfoNode08 = (BoneIndex)br.ReadUInt32();
                mInfoNode09 = (BoneIndex)br.ReadUInt32();
                mInfoNode10 = (BoneIndex)br.ReadUInt32();
                mInfoRoot = (BoneIndex)br.ReadUInt32();
                mLinks = new IkLinkList(handler);
                uint[] linkUnks = new uint[linkCount];
                for (int i = 0; i < linkCount; i++)
                {
                    linkUnks[i] = br.ReadUInt32();
                }
                mUnknown04 = br.ReadUInt32();
                for (int i = 0; i < linkCount; i++)
                {
                    BoneIndex end = (BoneIndex)br.ReadUInt32();
                    BoneIndex start = (BoneIndex)br.ReadUInt32();
                    mLinks.Add(new IkLink(0, handler, linkUnks[i], end, start));
                }
            }
            public void UnParse(Stream s)
            {
                var bw = new BinaryWriter(s);
                bw.Write(mUnknown01);
                bw.Write(mLinks.Count);
                bw.Write(mUnknown02);
                bw.Write((UInt32)mIkPole);
                bw.Write((UInt32)mIkPoleRoot);
                bw.Write((UInt32)mSlotInfo);
                bw.Write((UInt32)mSlotInfoRoot);
                bw.Write((UInt32)mSlotOffset);
                bw.Write((UInt32)mSlotOffsetRoot);
                bw.Write((UInt32)mUnknown03);
                bw.Write((UInt32)mInfoNode01);
                bw.Write((UInt32)mInfoNode02);
                bw.Write((UInt32)mInfoNode03);
                bw.Write((UInt32)mInfoNode04);
                bw.Write((UInt32)mInfoNode05);
                bw.Write((UInt32)mInfoNode06);
                bw.Write((UInt32)mInfoNode07);
                bw.Write((UInt32)mInfoNode08);
                bw.Write((UInt32)mInfoNode09);
                bw.Write((UInt32)mInfoNode10);
                bw.Write((UInt32)mInfoRoot);
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
        private Byte[] mGranny2Data;
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
                if (!HasIkData) return String.Empty;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Unknown01:\t0x{0:X8}", mUnknown01);
                sb.AppendFormat("Unknown02:\n");
                for (int i = 0; i < mUnknown02.Length; i++)
                {
                    sb.AppendFormat("{0:X2}", mUnknown02[i]);
                }
                sb.Append("\n");
                if(mIkChains.Count > 0)
                {
                    sb.AppendFormat("Ik Chains:\n");
                    for (int i = 0; i < mIkChains.Count; i++)
                    {
                        sb.AppendFormat("==Ik Chain[0x{0:X8}]==\n{1}\n", i, mIkChains[i].Value);
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
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, mIkTargets[i].Index);
                    }
                }
                if (mInfoNodes.Count > 0)
                {
                    sb.AppendFormat("Info Nodes:\n");
                    for (int i = 0; i < mInfoNodes.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, mInfoNodes[i].Index);
                    }
                }
                if (mCompressNodes.Count > 0)
                {
                    sb.AppendFormat("Compress Nodes:\n");
                    for (int i = 0; i < mCompressNodes.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, mCompressNodes[i].Index);
                    }
                }
                if (mBones.Count > 0)
                {
                    sb.AppendFormat("Bones:\n");
                    for (int i = 0; i < mBones.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, mBones[i].Index);
                    }
                }
                return sb.ToString();
            }
        }
        [ElementPriority(1)]
        public bool HasIkData
        {
            get { return mHasIkData; }
            set { mHasIkData = value; OnResourceChanged(this, new EventArgs()); }
        }

        [ElementPriority(2)]
        public BinaryReader Granny2Data
        {
            get
            {
                MemoryStream s = new MemoryStream(mGranny2Data);
                s.Position = 0L;
                return new BinaryReader(s);
            }
            set
            {
                if (value.BaseStream.CanSeek)
                {
                    value.BaseStream.Position = 0L;
                    mGranny2Data = value.ReadBytes((int)value.BaseStream.Length);
                }
                else
                {
                    MemoryStream s = new MemoryStream();
                    byte[] buffer = new byte[0x100000];
                    for (int i = value.BaseStream.Read(buffer, 0, buffer.Length); i > 0; i = value.BaseStream.Read(buffer, 0, buffer.Length))
                    {
                        s.Write(buffer, 0, i);
                    }
                    mGranny2Data = new BinaryReader(s).ReadBytes((int)s.Length);
                }
                OnResourceChanged(this, new EventArgs());
            }
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
                s.Position = 0L;
                mGranny2Data = br.ReadBytes((int)s.Length);
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
            mGranny2Data = br.ReadBytes(grannySize);
            s.Seek(4-(s.Position % 4), SeekOrigin.Current); //correct pos
            if (checking && s.Position != ikChainsOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", ikChainsOffset, s.Position));
            mIkChains = new IkChainList(this.OnResourceChanged,s);
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
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            if (mGranny2Data == null) mGranny2Data = new byte[0];
            if (!mHasIkData)
            {
                s.Position = 0L;
                bw.Write(mGranny2Data);
                return s;
            }

            long grannyOffset = 0L;
            long ikChainsOffset = 0L;
            long ikTargetsOffset = 0L;
            long infoNodesOffset = 0L;
            long compressNodesOffset = 0L;
            long fullBoneListOffset = 0L;
            if (mUnknown02 == null) mUnknown02 = new byte[16];
            if (mUnknown03 == null) mUnknown02 = new byte[56];
            if (mIkChains == null) mIkChains = new IkChainList(this.OnResourceChanged);
            if (mIkTargets == null) mIkTargets = new GrannyBoneReferenceList(this.OnResourceChanged);
            if (mInfoNodes == null) mInfoNodes = new GrannyBoneReferenceList(this.OnResourceChanged);
            if (mCompressNodes == null) mCompressNodes = new GrannyBoneReferenceList(this.OnResourceChanged);
            if (mBones == null) mBones = new GrannyBoneReferenceList(this.OnResourceChanged);

            bw.Write(0x8EAF13DE);
            bw.Write(mUnknown01);
            bw.Write(mGranny2Data.Length);
            long offsetPosition = s.Position;
            s.Seek(24, SeekOrigin.Current);
            bw.Write(mUnknown02);

            grannyOffset = s.Position;
            bw.Write(mGranny2Data);
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
                    return new List<string>() { "Granny2Data", "HasIkData" };
                }
                else
                {
                    return base.ContentFields;
                }
            }
        }
        private const int kRecommendedApiVersion = 1;
        private static bool checking = Settings.Checking;
    }
}
