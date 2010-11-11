using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using s3pi.Interfaces;
using s3pi.Settings;

namespace s3piwrappers
{
    public class BodyRig : AbstractRig
    {

        public class GrannyBoneReferenceList : AResource.SimpleList<Int32>
        {
            public GrannyBoneReferenceList(EventHandler handler) : base(handler, CreateElement, WriteElement) { }
            public GrannyBoneReferenceList(EventHandler handler, IList<Int32> ilt) : base(handler, ilt, CreateElement, WriteElement) { }
            public GrannyBoneReferenceList(EventHandler handler, Stream s) : base(handler, s, CreateElement, WriteElement) { }
            static new Int32 CreateElement(Stream s)
            {
                return new BinaryReader(s).ReadInt32();
            }
            static void WriteElement(Stream s, Int32 element)
            {
                new BinaryWriter(s).Write(element);
            }
        }

        public class IkLinkList : AResource.DependentList<IkLink>
        {
            public IkLinkList(EventHandler handler) : base(handler) { }
            public IkLinkList(EventHandler handler, IList<IkLink> ilt) : base(handler, ilt) { }

            public override void Add()
            {
                base.Add(new object[] { });
            }
            #region Unused
            protected override IkLink CreateElement(Stream s)
            {
                throw new NotImplementedException();
            }

            protected override void WriteElement(Stream s, IkLink element)
            {
                throw new NotImplementedException();
            } 
            #endregion
        }

        public class OptionalNodeReference : AHandlerElement
        {
            private Int32 mIndex;
            private bool mEnabled;
            public OptionalNodeReference(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public OptionalNodeReference(int APIversion, EventHandler handler, OptionalNodeReference basis) : this(APIversion, handler, basis.Enabled, basis.Index) { }
            public OptionalNodeReference(int APIversion, EventHandler handler, bool enabled, int index)
                : base(APIversion, handler)
            {
                mEnabled = enabled;
                mIndex = index;
            }
            [ElementPriority(1)]
            public int Index
            {
                get { return mIndex; }
                set { if (mIndex != value) { mIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public bool Enabled
            {
                get { return mEnabled; }
                set { if (mEnabled != value) { mEnabled = value; OnElementChanged(); } }
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new OptionalNodeReference(0, handler, this);
            }
            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }
            public override int RecommendedApiVersion
            {
                get { return 1; }
            }
        }



        public class IkChainList : AResource.DependentList<IkChain>
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
                    if (Settings.Checking && s.Position != offsets[i])
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
                s.Seek(4 * Count,SeekOrigin.Current);
                for (int i = 0; i < Count; i++)
                {
                    offsets[i] = s.Position - startOffset;
                    WriteElement(s, this[i]);
                }
                long dataEnd = s.Position;
                s.Seek(startOffset, SeekOrigin.Begin);
                for (int i = 0; i < Count; i++)
                {
                    bw.Write((UInt32)offsets[i]);
                }
                s.Seek(dataEnd, SeekOrigin.Begin);

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
                : this(APIversion, handler)
            {
                mUnknown = unk;
                mEnd = end;
                mStart = start;
            }

            [ElementPriority(1)]
            public uint Unknown
            {
                get { return mUnknown; }
                set { if (mUnknown != value) { mUnknown = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public Int32 End
            {
                get { return mEnd; }
                set { if (mEnd != value) { mEnd = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public Int32 Start
            {
                get { return mStart; }
                set { if (mStart != value) { mStart = value; OnElementChanged(); } }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IkLink(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            public bool Equals(IkLink other)
            {
                return mStart.Equals(other.mStart) && mEnd.Equals(other.mEnd);
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
            private OptionalNodeReference[] mInfoNodes;
            private Int32 mInfoRoot;
            private UInt32 mUnknown04;
            private IkLinkList mLinks;
            public IkChain(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
                mLinks = new IkLinkList(handler);
                mUnknown02 = new byte[32];
                mInfoNodes = new OptionalNodeReference[10];
                for (int i = 0; i < 10; i++) mInfoNodes[i] = new OptionalNodeReference(0, handler);
            }
            public IkChain(int APIversion, EventHandler handler, IkChain basis) : this(APIversion, handler, basis.IkPole, basis.IkPoleRoot, (OptionalNodeReference[])basis.InfoNodes.Clone(), basis.InfoRoot, new IkLinkList(handler, basis.Links), basis.SlotInfo, basis.SlotInfoRoot, basis.SlotOffset, basis.SlotOffsetRoot, basis.Unknown01, (byte[])basis.Unknown02, basis.Unknown04) { }
            public IkChain(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }
            public IkChain(int APIversion, EventHandler handler, int ikPole, int ikPoleRoot, OptionalNodeReference[] infoNodes, int infoRoot, IkLinkList links, int slotInfo, int slotInfoRoot, int slotOffset, int slotOffsetRoot, uint unknown01, byte[] unknown02, uint unknown04)
                : base(APIversion, handler)
            {
                mIkPole = ikPole;
                mIkPoleRoot = ikPoleRoot;
                mInfoNodes = infoNodes;
                mInfoRoot = infoRoot;
                mLinks = links;
                mSlotInfo = slotInfo;
                mSlotInfoRoot = slotInfoRoot;
                mSlotOffset = slotOffset;
                mSlotOffsetRoot = slotOffsetRoot;
                mUnknown01 = unknown01;
                mUnknown02 = unknown02;
                mUnknown04 = unknown04;
            }

            [ElementPriority(1)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set { if (mUnknown01 != value) { mUnknown01 = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public byte[] Unknown02
            {
                get { return mUnknown02; }
                set { if (mUnknown02 != value) { mUnknown02 = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public Int32 IkPole
            {
                get { return mIkPole; }
                set { if (mIkPole != value) { mIkPole = value; OnElementChanged(); } }
            }
            [ElementPriority(4)]
            public Int32 IkPoleRoot
            {
                get { return mIkPoleRoot; }
                set { if (mIkPoleRoot != value) { mIkPoleRoot = value; OnElementChanged(); } }
            }
            [ElementPriority(5)]
            public Int32 SlotInfo
            {
                get { return mSlotInfo; }
                set { if (mSlotInfo != value) { mSlotInfo = value; OnElementChanged(); } }
            }
            [ElementPriority(6)]
            public Int32 SlotInfoRoot
            {
                get { return mSlotInfoRoot; }
                set { if (mSlotInfoRoot != value) { mSlotInfoRoot = value; OnElementChanged(); } }
            }
            [ElementPriority(7)]
            public Int32 SlotOffset
            {
                get { return mSlotOffset; }
                set { if (mSlotOffset != value) { mSlotOffset = value; OnElementChanged(); } }
            }
            [ElementPriority(8)]
            public Int32 SlotOffsetRoot
            {
                get { return mSlotOffsetRoot; }
                set { if (mSlotOffsetRoot != value) { mSlotOffsetRoot = value; OnElementChanged(); } }
            }
            [ElementPriority(9)]
            public OptionalNodeReference[] InfoNodes
            {
                get { return mInfoNodes; }
                set { if (mInfoNodes != value) { mInfoNodes = value; OnElementChanged(); } }
            }
            [ElementPriority(10)]
            public Int32 InfoRoot
            {
                get { return mInfoRoot; }
                set { if (mInfoRoot != value) { mInfoRoot = value; OnElementChanged(); } }
            }
            [ElementPriority(11)]
            public uint Unknown04
            {
                get { return mUnknown04; }
                set { if (mUnknown04 != value) { mUnknown04 = value; OnElementChanged(); } }
            }
            [ElementPriority(12)]
            public IkLinkList Links
            {
                get { return mLinks; }
                set { if (mLinks != value) { mLinks = value; OnElementChanged(); } }
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
                var InfoNodeFlags = br.ReadUInt32();
                mInfoNodes = new OptionalNodeReference[10];
                for (int i = 0; i < 10; i++) mInfoNodes[i] = new OptionalNodeReference(0, handler, ((1U << i) & InfoNodeFlags) != 0, br.ReadInt32());
                mInfoRoot = br.ReadInt32();
                mLinks = new IkLinkList(handler);
                uint[] linkUnks = new uint[linkCount];
                for (int i = 0; i < linkCount; i++) linkUnks[i] = br.ReadUInt32();
                mUnknown04 = br.ReadUInt32();
                for (int i = 0; i < linkCount; i++) mLinks.Add(new IkLink(0, handler, linkUnks[i], br.ReadInt32(), br.ReadInt32()));
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
                var flags = 0U;
                for (int i = 0; i < 10; i++) if (mInfoNodes[i].Enabled) flags |= (1U << i);
                bw.Write(flags);
                for (int i = 0; i < 10; i++) bw.Write(mInfoNodes[i].Index);
                bw.Write(mInfoRoot);
                for (int i = 0; i < mLinks.Count; i++) bw.Write(mLinks[i].Unknown);
                bw.Write(mUnknown04);
                for (int i = 0; i < mLinks.Count; i++)
                {
                    bw.Write((uint)mLinks[i].End);
                    bw.Write((uint)mLinks[i].Start);
                }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new IkChain(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }
            public override int RecommendedApiVersion
            {
                get { return 1; }
            }
            public bool Equals(IkChain other)
            {
                return base.Equals(other);
            }
        }


        public BodyRig(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
            mUnknown02 = new byte[16];
            mUnknown03 = new byte[56];
            mIkChains = new IkChainList(handler);
            mSlotJoints = new GrannyBoneReferenceList(handler);
            mExportJoints = new GrannyBoneReferenceList(handler);
            mGrowthJoints = new GrannyBoneReferenceList(handler);
            mSortedJoints = new GrannyBoneReferenceList(handler);
        }
        public BodyRig(int APIversion, EventHandler handler, GrannyData grannyData)
            : this(APIversion, handler)
        {
            GrannyData = grannyData;
        }

        public BodyRig(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }

        private UInt32 mUnknown01;
        private Byte[] mUnknown02;
        private IkChainList mIkChains;
        private Byte[] mUnknown03;
        private GrannyBoneReferenceList mSlotJoints;
        private GrannyBoneReferenceList mExportJoints;
        private GrannyBoneReferenceList mGrowthJoints;
        private GrannyBoneReferenceList mSortedJoints;
        [ElementPriority(1)]
        public uint Unknown01
        {
            get { return mUnknown01; }
            set { if (mUnknown01 != value) { mUnknown01 = value; OnElementChanged(); } }
        }
        [ElementPriority(2)]
        public byte[] Unknown02
        {
            get { return mUnknown02; }
            set { if (mUnknown02 != value) { mUnknown02 = value; OnElementChanged(); } }
        }

        [ElementPriority(3)]
        public IkChainList IkChains
        {
            get { return mIkChains; }
            set { if (mIkChains != value) { mIkChains = value; OnElementChanged(); } }
        }
        [ElementPriority(4)]
        public byte[] Unknown03
        {
            get { return mUnknown03; }
            set { if (mUnknown03 != value) { mUnknown03 = value; OnElementChanged(); } }
        }
        [ElementPriority(5)]
        public GrannyBoneReferenceList SlotJoints
        {
            get { return mSlotJoints; }
            set { if (mSlotJoints != value) { mSlotJoints = value; OnElementChanged(); } }
        }
        [ElementPriority(6)]
        public GrannyBoneReferenceList ExportJoints
        {
            get { return mExportJoints; }
            set { if (mExportJoints != value) { mExportJoints = value; OnElementChanged(); } }
        }
        [ElementPriority(7)]
        public GrannyBoneReferenceList GrowthJoints
        {
            get { return mGrowthJoints; }
            set { if (mGrowthJoints != value) { mGrowthJoints = value; OnElementChanged(); } }
        }
        [ElementPriority(8)]
        public GrannyBoneReferenceList SortedJoints
        {
            get { return mSortedJoints; }
            set { if (mSortedJoints != value) { mSortedJoints = value; OnElementChanged(); } }
        }
        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            mUnknown01 = br.ReadUInt32();
            int grannySize = br.ReadInt32();
            long grannyOffset = s.Position + br.ReadUInt32();
            long ikChainsOffset = s.Position + br.ReadUInt32();
            long ikTargetsOffset = s.Position + br.ReadUInt32();
            long infoNodesOffset = s.Position + br.ReadUInt32();
            long compressNodesOffset = s.Position + br.ReadUInt32();
            long fullBoneListOffset = s.Position + br.ReadUInt32();
            mUnknown02 = br.ReadBytes(16);
            if (checking && s.Position != grannyOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", grannyOffset, s.Position));

            byte[] buffer = br.ReadBytes(grannySize);
            mGrannyData = GrannyData.CreateInstance(0, handler, new MemoryStream(buffer));

            while ((s.Position % 4) != 0)
                if (br.ReadByte() != 0x00 && checking)
                    throw new InvalidDataException("Expected padding char 0x00");

            if (checking && s.Position != ikChainsOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", ikChainsOffset, s.Position));
            mIkChains = new IkChainList(handler, s);
            mUnknown03 = br.ReadBytes(56);
            if (checking && s.Position != ikTargetsOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", ikTargetsOffset, s.Position));
            mSlotJoints = new GrannyBoneReferenceList(handler, s);
            if (checking && s.Position != infoNodesOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", infoNodesOffset, s.Position));
            mExportJoints = new GrannyBoneReferenceList(handler, s);
            if (checking && s.Position != compressNodesOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", compressNodesOffset, s.Position));
            mGrowthJoints = new GrannyBoneReferenceList(handler, s);
            if (checking && s.Position != fullBoneListOffset)
                throw new InvalidDataException(String.Format("Bad offset, expected {0} but got {1}", fullBoneListOffset, s.Position));
            mSortedJoints = new GrannyBoneReferenceList(handler, s);


        }
        public override void UnParse(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);

            long grannyOffset = 0L;
            long ikChainsOffset = 0L;
            long ikTargetsOffset = 0L;
            long infoNodesOffset = 0L;
            long compressNodesOffset = 0L;
            long fullBoneListOffset = 0L;

            bw.Write(mUnknown01);
            byte[] buffer = null;


            Stream gr2 = mGrannyData.UnParse();
            buffer = new byte[gr2.Length];
            gr2.Read(buffer, 0, buffer.Length);
            gr2.Close();
            bw.Write(buffer.Length);
            if ((s.Position % 4) != 0) bw.Write(new byte[4 - (s.Position % 4)]);

            long offsetPosition = s.Position;
            s.Seek(24, SeekOrigin.Current);
            bw.Write(mUnknown02);

            grannyOffset = s.Position;

            bw.Write(buffer);

            //00 padding to next DWORD
            while ((s.Position % 4) != 0)
                bw.Write((byte)0x00);


            ikChainsOffset = s.Position;
            mIkChains.UnParse(s);
            bw.Write(mUnknown03);

            ikTargetsOffset = s.Position;
            mSlotJoints.UnParse(s);

            infoNodesOffset = s.Position;
            mExportJoints.UnParse(s);

            compressNodesOffset = s.Position;
            mGrowthJoints.UnParse(s);

            fullBoneListOffset = s.Position;
            mSortedJoints.UnParse(s);
            long endPos = s.Position;
            s.Seek(offsetPosition, SeekOrigin.Begin);

            bw.Write((uint)(grannyOffset - s.Position));
            bw.Write((uint)(ikChainsOffset - s.Position));
            bw.Write((uint)(ikTargetsOffset - s.Position));
            bw.Write((uint)(infoNodesOffset - s.Position));
            bw.Write((uint)(compressNodesOffset - s.Position));
            bw.Write((uint)(fullBoneListOffset - s.Position));
            s.Seek(endPos, SeekOrigin.Begin);
        }
        private string GetBoneIndexName(Int32 Index)
        {
            if (Index == -1) return "NULL";
            WrappedGrannyData grd = mGrannyData as WrappedGrannyData;
            if (grd == null)
                return "0x" + Index.ToString("X8");
            if (Index >= grd.FileInfo.Skeleton.Bones.Count) return "INVALID";
            return grd.FileInfo.Skeleton.Bones[Index].Name;
        }
        public override string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder(base.Value);
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
                        sb.AppendFormat("SlotExport:\t{0}\n", GetBoneIndexName(ikc.SlotInfo));
                        sb.AppendFormat("SlotExportRoot:\t{0}\n", GetBoneIndexName(ikc.SlotInfoRoot));
                        sb.AppendFormat("SlotExportOffset:\t{0}\n", GetBoneIndexName(ikc.SlotOffset));
                        sb.AppendFormat("SlotExportOffsetRoot:\t{0}\n", GetBoneIndexName(ikc.SlotOffsetRoot));
                        for (int j = 0; j < 10; j++)
                        {
                            if (ikc.InfoNodes[j].Enabled)
                                sb.AppendFormat("ExportJoint[{0}]:\t{1}\n", j, GetBoneIndexName(ikc.InfoNodes[j].Index));
                        }
                        sb.AppendFormat("ExportRoot:\t{0}\n", GetBoneIndexName(ikc.InfoRoot));
                        sb.AppendFormat("Unknown04:\t0x{0:X8}\n", ikc.Unknown04);
                        if (ikc.Links.Count > 0)
                        {
                            sb.AppendFormat("Ik Links:\n");
                            for (int j = 0; j < ikc.Links.Count; j++)
                            {
                                var ikcl = ikc.Links[j];
                                sb.AppendFormat("==Link[{0}]==\n", j);
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
                if (mSlotJoints.Count > 0)
                {
                    sb.AppendFormat("Slot Joints:\n");
                    for (int i = 0; i < mSlotJoints.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, GetBoneIndexName(mSlotJoints[i].Val));
                    }
                }
                if (mExportJoints.Count > 0)
                {
                    sb.AppendFormat("Export Joints:\n");
                    for (int i = 0; i < mExportJoints.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, GetBoneIndexName(mExportJoints[i].Val));
                    }
                }
                if (mGrowthJoints.Count > 0)
                {
                    sb.AppendFormat("Growth Joints:\n");
                    for (int i = 0; i < mGrowthJoints.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, GetBoneIndexName(mGrowthJoints[i].Val));
                    }
                }
                if (mSortedJoints.Count > 0)
                {
                    sb.AppendFormat("Sorted Joints:\n");
                    for (int i = 0; i < mSortedJoints.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}] {1}\n", i, GetBoneIndexName(mSortedJoints[i].Val));
                    }
                }
                return sb.ToString();
            }
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}