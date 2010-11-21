using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using System.IO;
using System.Linq;
using s3pi.Settings;
namespace s3piwrappers
{
    [Flags]
    public enum LODInfoFlags : uint
    {
        Portal = 0x00000001,
        Door = 0x00000002
    }
    public enum LODId : uint
    {
        HighDetail = 0x00000000,
        MediumDetail = 0x00000001,
        LowDetail = 0x00000002,
        HighDetailShadow = 0x00010000,
        MediumDetailShadow = 0x00010001,
        LowDetailShadow = 0x00010002
    }
    public class MODL : ARCOLBlock
    {
        public class BoundingBoxList : AResource.DependentList<BoundingBox>
        {
            public BoundingBoxList(EventHandler handler) : base(handler) { }
            public BoundingBoxList(EventHandler handler, Stream s) : base(handler, s) { }
            public BoundingBoxList(EventHandler handler, IList<BoundingBox> ilt) : base(handler, ilt) { }
            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override BoundingBox CreateElement(Stream s)
            {
                return new BoundingBox(0, this.handler, s);
            }
            protected override void WriteElement(Stream s, BoundingBox element)
            {
                element.UnParse(s);
            }
        }
        public class LODEntryList : AResource.DependentList<LODEntry>
        {
            public LODEntryList(EventHandler handler)
                : base(handler)
            {
            }
            public LODEntryList(EventHandler handler, Stream s, int count)
                : base(handler)
            {
                Parse(s, count);
            }

            public LODEntryList(EventHandler handler, IList<LODEntry> ilt) : base(handler, ilt) { }

            private void Parse(Stream s, int count)
            {
                BinaryReader br = new BinaryReader(s);
                for (uint i = 0; i < count; i++)
                {
                    ((IList<LODEntry>)this).Add(CreateElement(s));
                }
            }
            public override void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                foreach (var element in this)
                {
                    WriteElement(s, element);
                }
            }
            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override LODEntry CreateElement(Stream s)
            {
                return new LODEntry(0, handler, s);
            }

            protected override void WriteElement(Stream s, LODEntry element)
            {
                element.UnParse(s);
            }
        }

        public class LODEntry : AHandlerElement, IEquatable<LODEntry>
        {
            private UInt32 mModelLodIndex;
            private LODInfoFlags mFlags;
            private LODId mId;
            private float mMinZValue;
            private float mMaxZValue;

            public LODEntry(int APIversion, EventHandler handler) : this(APIversion, handler, 0, (LODInfoFlags)0, LODId.LowDetail, float.MinValue, float.MaxValue) { }
            public LODEntry(int APIversion, EventHandler handler, LODEntry basis) : this(APIversion, handler, basis.ModelLodIndex, basis.Flags, basis.Id, basis.mModelLodIndex, basis.MaxZValue) { }
            public LODEntry(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }
            public LODEntry(int APIversion, EventHandler handler, uint modelLodIndex, LODInfoFlags flags, LODId id, float minZValue, float maxZValue)
                : base(APIversion, handler)
            {
                mModelLodIndex = modelLodIndex;
                mFlags = flags;
                mId = id;
                mMinZValue = minZValue;
                mMaxZValue = maxZValue;
            }

            public string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Index:\t0x{0:X8}\n", mModelLodIndex);
                    sb.AppendFormat("Flags:\t{0}\n", this["Flags"]);
                    sb.AppendFormat("Id:\t{0}\n", mId);
                    sb.AppendFormat("MinZValue:\t{0}\n", mMinZValue);
                    sb.AppendFormat("MaxZValue:\t{0}\n", mMaxZValue);

                    return sb.ToString();
                }
            }
            [ElementPriority(1)]
            public UInt32 ModelLodIndex
            {
                get { return mModelLodIndex; }
                set { if (mModelLodIndex != value) { mModelLodIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public LODInfoFlags Flags
            {
                get { return mFlags; }
                set { if (mFlags != value) { mFlags = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public LODId Id
            {
                get { return mId; }
                set { if (mId != value) { mId = value; OnElementChanged(); } }
            }
            [ElementPriority(4)]
            public float MinZValue
            {
                get { return mMinZValue; }
                set { if (mMinZValue != value) { mMinZValue = value; OnElementChanged(); } }
            }
            [ElementPriority(5)]
            public float MaxZValue
            {
                get { return mMaxZValue; }
                set { if (mMaxZValue != value) { mMaxZValue = value; OnElementChanged(); } }
            }

            private void Parse(Stream s)
            {

                BinaryReader br = new BinaryReader(s);
                mModelLodIndex = br.ReadUInt32();
                mFlags = (LODInfoFlags)br.ReadUInt32();
                mId = (LODId)br.ReadUInt32();
                mMinZValue = br.ReadSingle();
                mMaxZValue = br.ReadSingle();
            }

            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mModelLodIndex);
                bw.Write((UInt32)mFlags);
                bw.Write((UInt32)mId);
                bw.Write(mMinZValue);
                bw.Write(mMaxZValue);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new LODEntry(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
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

        public MODL(int APIversion, EventHandler handler) : this(APIversion, handler, 256, new BoundingBox(0, handler), new BoundingBoxList(handler), 0, 0f, new LODEntryList(handler)) { }
        public MODL(int APIversion, EventHandler handler, MODL basis) : this(APIversion, handler, basis.Version, new BoundingBox(0, handler, basis.Bounds), new BoundingBoxList(handler, basis.ExtraBounds),basis.FadeType,basis.CustomFadeDistance, new LODEntryList(handler, basis.Entries)) { }
        public MODL(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        public MODL(int APIversion, EventHandler handler, uint version, BoundingBox bounds, BoundingBoxList extraBounds, uint fadeType, float customFadeDistance, LODEntryList entries)
            : base(APIversion, handler, null)
        {
            mVersion = version;
            mBounds = bounds;
            mExtraBounds = extraBounds;
            mFadeType = fadeType;
            mCustomFadeDistance = customFadeDistance;
            mEntries = entries;
        }
        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { if (mVersion != value) { mVersion = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(2)]
        public BoundingBox Bounds
        {
            get { return mBounds; }
            set { if (mBounds != value) { mBounds = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(3)]
        public BoundingBoxList ExtraBounds
        {
            get { return mExtraBounds; }
            set { if (mExtraBounds != value) { mExtraBounds = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(4)]
        public uint FadeType
        {
            get { return mFadeType; }
            set { if (mFadeType != value) { mFadeType = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(5)]
        public float CustomFadeDistance
        {
            get { return mCustomFadeDistance; }
            set { if (mCustomFadeDistance != value) { mCustomFadeDistance = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(6)]
        public LODEntryList Entries
        {
            get { return mEntries; }
            set { if (mEntries != value) { mEntries = value; OnRCOLChanged(this, new EventArgs()); } }
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new MODL(0, handler, this);
        }

        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (Settings.Checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            int count = br.ReadInt32();
            mBounds = new BoundingBox(0, handler, s);
            if (mVersion >= 258)
            {
                mExtraBounds = new BoundingBoxList(handler, s);
                mFadeType = br.ReadUInt32();
                mCustomFadeDistance = br.ReadSingle();
            }
            else
            {
                mExtraBounds = new BoundingBoxList(handler);
            }
            mEntries = new LODEntryList(handler, s, count);
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
        public override List<string> ContentFields
        {
            get
            {
                var fields = base.ContentFields;
                if (mVersion < 258)
                {
                    fields.Remove("ExtraBounds");
                    fields.Remove("FadeType");
                    fields.Remove("CustomFadeDistance");
                }
                return fields;
            }
        }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Bounds:\n{0}\n", mBounds.Value);
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
        public override uint ResourceType
        {
            get { return 0x01661233; }
        }

        public override string Tag
        {
            get { return "MODL"; }
        }

        private const int kRecommendedApiVersion = 1;
    }
}
