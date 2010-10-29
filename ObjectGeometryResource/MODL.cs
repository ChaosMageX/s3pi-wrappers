using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using System.IO;
namespace s3piwrappers
{
    [Flags]
    public enum LODInfoFlag : uint
    {
        Door = 0x2,
        Portal = 0x1
    }

 

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
            private LODInfoFlag mFlags;
            private UInt32 mId;
            private float mMinZValue = float.MinValue;
            private float mMaxZValue = float.MaxValue;
            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Index:\t0x{0:X8}({1})\n", mIndex, mIndexType);
                    sb.AppendFormat("Flags:\t{0}\n", this["Flags"]);
                    sb.AppendFormat("Id:\t0x{0:X8}\n", mId);
                    sb.AppendFormat("MinZValue:\t{0}\n", mMinZValue);
                    sb.AppendFormat("MaxZValue:\t{0}\n", mMaxZValue);

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
                mFlags = basis.mFlags;
                mId = basis.mId;
                mMinZValue = basis.mMinZValue;
                mMaxZValue = basis.mMaxZValue;
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
            public LODInfoFlag Flags
            {
                get { return mFlags; }
                set { mFlags = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public uint Id
            {
                get { return mId; }
                set { mId = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public float MinZValue
            {
                get { return mMinZValue; }
                set { mMinZValue = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public float MaxZValue
            {
                get { return mMaxZValue; }
                set { mMaxZValue = value; OnElementChanged(); }
            }

            private void Parse(Stream s)
            {

                BinaryReader br = new BinaryReader(s);
                mIndex = br.ReadUInt16();
                mIndexType = (IndexTypes)br.ReadUInt16();
                mFlags = (LODInfoFlag)br.ReadUInt32();
                mId = br.ReadUInt32();
                mMinZValue = br.ReadSingle();
                mMaxZValue = br.ReadSingle();
            }

            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mIndex);
                bw.Write((ushort)mIndexType);
                bw.Write((UInt32)mFlags);
                bw.Write(mId);
                bw.Write(mMinZValue);
                bw.Write(mMaxZValue);
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
        private UInt32 mFadeType;
        private float mCustomFadeDistance;
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
                        sb.AppendFormat("FadeType:\t0x{0:X8}\n", mFadeType);
                        sb.AppendFormat("CustomFadeDistance:\t{0:0.00000}\n", mCustomFadeDistance);
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
        public uint FadeType
        {
            get { return mFadeType; }
            set { mFadeType = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(5)]
        public float CustomFadeDistance
        {
            get { return mCustomFadeDistance; }
            set { mCustomFadeDistance = value; OnRCOLChanged(this, new EventArgs()); }
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
                mFadeType = br.ReadUInt32();
                mCustomFadeDistance = br.ReadSingle();
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
                bw.Write(mFadeType);
                bw.Write(mCustomFadeDistance);
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
