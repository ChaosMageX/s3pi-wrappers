using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using System.IO;
namespace s3piwrappers
{
    public class MODL : ARCOLBlock
    {
        public class LODEntryList : AResource.DependentList<LODEntry>
        {
            public LODEntryList(EventHandler handler) : base(handler)
            {
            }
            public LODEntryList(EventHandler handler,Stream s,uint  count)
                : base(handler)
            {
                Parse(s,count);
            }
            private void Parse(Stream s, uint count)
            {
                BinaryReader br = new BinaryReader(s);
                for(uint i=0;i<count;i++)
                {
                    base.Add(CreateElement(s));
                }
            }
            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                foreach (var element in this)
                {
                    WriteElement(s,element);
                }
            }
            public override void Add()
            {
                base.Add(new object[] {});
            }

            protected override LODEntry CreateElement(Stream s)
            {
                return new LODEntry(0,handler,s);
            }

            protected override void WriteElement(Stream s, LODEntry element)
            {
                element.UnParse(s);
            }
        }
        
        public class LODEntry : AHandlerElement,IEquatable<LODEntry>
        {
            public enum IndexTypes : ushort
            {
                Internal=0x0000,
                External=0x3000 
            }
            private UInt16 mIndex;
            private IndexTypes mIndexType;
            private UInt32 mUnknown01;
            private UInt32 mLodKey;
            private UInt32 mUnknown02;
            private UInt32 mUnknown03;
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Index:\t0x{0:X8}({1})\n", mIndex, mIndexType);
                    sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                    sb.AppendFormat("LOD Key:\t0x{0:X8}\n", mLodKey);
                    sb.AppendFormat("Unknown02:\t0x{0:X8}\n", mUnknown02);
                    sb.AppendFormat("Unknown03:\t0x{0:X8}\n", mUnknown03);

                    return sb.ToString();
                }
            }
            public LODEntry(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
            }
            public LODEntry(int APIversion, EventHandler handler,LODEntry basis)
                : base(APIversion, handler)
            {
                mIndex = basis.mIndex;
                mIndexType = basis.mIndexType;
                mUnknown01 = basis.mUnknown01;
                mLodKey = basis.mLodKey;
                mUnknown02 = basis.mUnknown01;
                mUnknown03 = basis.mUnknown01;
            }

            public LODEntry(int APIversion, EventHandler handler,Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            [ElementPriority(1)]
            public ushort Index
            {
                get { return mIndex; }
                set { mIndex = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public IndexTypes IndexType
            {
                get { return mIndexType; }
                set { mIndexType = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set { mUnknown01 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public uint LodKey
            {
                get { return mLodKey; }
                set { mLodKey = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public uint Unknown02
            {
                get { return mUnknown02; }
                set { mUnknown02 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public uint Unknown03
            {
                get { return mUnknown03; }
                set { mUnknown03 = value; OnElementChanged(); }
            }

            private void Parse(Stream s)
            {

                BinaryReader br = new BinaryReader(s);
                mIndex = br.ReadUInt16();
                mIndexType = (IndexTypes)br.ReadUInt16();
                mUnknown01 = br.ReadUInt32();
                mLodKey = br.ReadUInt32();
                mUnknown02 = br.ReadUInt32();
                mUnknown03 = br.ReadUInt32();
            }

            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mIndex);
                bw.Write((ushort)mIndexType);
                bw.Write(mUnknown01);
                bw.Write(mLodKey);
                bw.Write(mUnknown02);
                bw.Write(mUnknown03);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new LODEntry(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(LODEntry other)
            {
                return base.Equals(other);
            }
        }
        
        private UInt32 mVersion;
        private BoundingBox mBounds;
        private UInt32 mUnknown01;
        private UInt32 mUnknown02;
        private BoundingBoxList mExtraBounds;
        private LODEntryList mEntries;
        
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                    sb.AppendFormat("Bounds:\n{0}\n",mBounds.Value);
                    if (mVersion > 258)
                    {
                        if (mExtraBounds.Count > 0)
                        {
                            sb.AppendFormat("ExtraBounds:\n");
                            for (int i = 0; i < mExtraBounds.Count; i++)
                            {
                                sb.AppendFormat("=={0}==\n{1}\n", i, mExtraBounds[i].Value);
                            }
                        }
                        sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                        sb.AppendFormat("Unknown02:\t0x{0:X8}\n", mUnknown02);
                    }
                    if (mEntries.Count > 0)
                    {
                        sb.AppendFormat("LOD Entries:\n");
                        for (int i = 0; i < mEntries.Count; i++)
                        {
                            sb.AppendFormat("=={0}==\n{1}\n", i, mEntries[i].Value);
                        }
                    }
                    return sb.ToString();
                }
            }
        public MODL(int APIversion, EventHandler handler)
            : base(APIversion, handler, null)
        {
            mVersion = 256;
            mBounds = new BoundingBox(0, handler);
            mExtraBounds = new BoundingBoxList(handler);
            mEntries = new LODEntryList(handler);

        }
        public MODL(int APIversion, EventHandler handler,MODL basis)
            : base(APIversion, handler, null)
        {
            Stream s = basis.UnParse();
            s.Position = 0L;
            Parse(s);
        }
        public MODL(int APIversion, EventHandler handler, Stream s) 
            : base(APIversion, handler, s)
        {
            
        }
        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { mVersion = value; OnRCOLChanged(this,new EventArgs()); }
        }
        [ElementPriority(2)]
        public BoundingBox Bounds
        {
            get { return mBounds; }
            set { mBounds = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public BoundingBoxList ExtraBounds
        {
            get { return mExtraBounds; }
            set { mExtraBounds = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        public uint Unknown01
        {
            get { return mUnknown01; }
            set { mUnknown01 = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(5)]
        public uint Unknown02
        {
            get { return mUnknown02; }
            set { mUnknown02 = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(6)]
        public LODEntryList Entries
        {
            get { return mEntries; }
            set { mEntries = value; OnRCOLChanged(this, new EventArgs()); }
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new MODL(0, handler, this);
        }

        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking &&  tag!= Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag,Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            uint count = br.ReadUInt32();
            mBounds = new BoundingBox(0,handler,s);
            if(mVersion >= 258)
            {
                mExtraBounds = new BoundingBoxList(handler, s);
                mUnknown01 = br.ReadUInt32();
                mUnknown02 = br.ReadUInt32();
            }
            mEntries= new LODEntryList(handler,s,count);
        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s); 
            bw.Write((UInt32)FOURCC(Tag));
            if (mExtraBounds == null) mExtraBounds = new BoundingBoxList(handler);
            if (mEntries == null) mEntries = new LODEntryList(handler);
            if (mBounds == null) mBounds = new BoundingBox(0, handler);
            if (mVersion < 258 && mExtraBounds.Count > 0) mVersion = 258;
            bw.Write(mVersion);
            bw.Write(mEntries.Count);
            mBounds.UnParse(s);

            if (mVersion >= 258)
            {
                mExtraBounds.UnParse(s);
                bw.Write(mUnknown01);
                bw.Write(mUnknown02);
            }
            mEntries.UnParse(s);
            return s;
        }

        public override uint ResourceType
        {
            get { return 0x01661233; }
        }

        public override string Tag
        {
            get { return "MODL"; }
        }

        private static bool checking = s3pi.Settings.Settings.Checking;
        private const int kRecommendedApiVersion=1;
    }
}
