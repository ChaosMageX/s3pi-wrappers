using System;
using System.IO;
using s3pi.Interfaces;
using System.Text;
using System.Collections.Generic;
namespace s3piwrappers
{
    /// <summary>
    /// Unnamed block in MODL/MLOD resources.  Referenced by VBUF, one entry for each group.
    /// </summary>
    public class VMAP :ARCOLBlock
    {
        public class SubEntry : AHandlerElement, IEquatable<SubEntry>
        {
            public SubEntry(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {

            }
            public SubEntry(int apiVersion, EventHandler handler, SubEntry basis)
                : base(apiVersion, handler)
            {
                mData = basis.mData;
            }
            public SubEntry(int apiVersion, EventHandler handler, Stream s)
                : base(apiVersion, handler)
            {
                Parse(s);
            }
            [ElementPriority(1)]
            public uint Data
            {
                get { return mData; }
                set { mData = value; OnElementChanged(); }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new SubEntry(0, handler, this);
            }
            private UInt32 mData;
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mData = br.ReadUInt32();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mData);
            }
            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(SubEntry other)
            {
                return base.Equals(other);
            }
            public override string ToString()
            {
                return String.Format("0x:{0:X8}", mData);
            }
            public string Value { get { return ToString(); } }
        }
        public class SubEntryList : AResource.DependentList<SubEntry>
        {
            public SubEntryList(EventHandler handler)
                : base(handler)
            {
            }

            public SubEntryList(EventHandler handler, Stream s,uint count)
                : base(handler)
            {
                Parse(s,count);
            }
            private void Parse(Stream s, uint count)
            {
                for (int i = 0; i < count; i++)
                {
                    ((IList<SubEntry>)this).Add(CreateElement(s));
                }
            }
            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override void WriteCount(Stream s, uint count)
            {
                
            }

            protected override SubEntry CreateElement(Stream s)
            {
                return new SubEntry(0, handler, s);
            }

            protected override void WriteElement(Stream s, SubEntry element)
            {
                element.UnParse(s);
            }
        }
        public class EntryList : AResource.DependentList<Entry>
        {
            public EntryList(EventHandler handler) : base(handler)
            {
            }

            public EntryList(EventHandler handler, Stream s) : base(handler,s)
            {
            }

            public override void Add()
            {
                base.Add(new object[] {});
            }
            protected override void WriteCount(Stream s, uint count)
            {
                
            }
           
            protected override Entry CreateElement(Stream s)
            {
                return new Entry(0, handler, s);
            }

            protected override void WriteElement(Stream s, Entry element)
            {
                element.UnParse(s);
            }
        }
        public class Entry : AHandlerElement, IEquatable<Entry>
        {

            private uint mCount;
            private uint mStart;
            private SubEntryList mSubEntries;
            [ElementPriority(1)]
            public uint Count
            {
                get { return mCount; }
                set { mCount = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public uint Start
            {
                get { return mStart; }
                set { mStart = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public SubEntryList SubEntries
            {
                get { return mSubEntries; }
                set { mSubEntries = value; OnElementChanged(); }
            }

            public string Value
            {
                get
                {
                    var sb = new StringBuilder();
                    sb.AppendFormat("Count:\t{0}\n", mCount);
                    sb.AppendFormat("Offset:\t{0}\n", mStart);
                    if (mSubEntries.Count > 0)
                    {
                        sb.AppendLine("SubEntries:");
                        for (int i = 0; i < mSubEntries.Count; i++)
                        {
                            sb.AppendFormat("[{0}]:0x{1:X8}\n", i, mSubEntries[i].Data);
                        }
                    }
                    return sb.ToString();
                }
            }
            public Entry(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
            }
            public Entry(int APIversion, EventHandler handler,Entry basis)
                : base(APIversion, handler)
            {
                System.IO.Stream s = new MemoryStream();
                basis.UnParse(s);
                s.Position = 0L;
                Parse(s);
            }

            public Entry(int APIversion, EventHandler handler,Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                uint expectedSize = br.ReadUInt32();
                mCount = br.ReadUInt32();
                mStart = br.ReadUInt32();
                long start = s.Position;
                uint count = expectedSize/4;
                mSubEntries = new SubEntryList(handler,s,count);
                long end = s.Position;
                uint actualSize = (uint)(end-start);
                if (checking && actualSize != expectedSize)
                    throw new InvalidDataException(String.Format("Invalid Entry size. Expected {0} but got {1}.",expectedSize,actualSize));
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                uint size = 0;
                long sizeOffset = s.Position;
                bw.Write(size);
                bw.Write(mCount);
                bw.Write(mStart);
                long startOffset = s.Position;
                if (mSubEntries == null) mSubEntries = new SubEntryList(handler);
                mSubEntries.UnParse(s);
                long endOffset = s.Position;
                size = (uint) (endOffset - startOffset);
                s.Seek(sizeOffset, SeekOrigin.Begin);
                bw.Write(size);
                s.Seek(endOffset, SeekOrigin.Begin);

            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new Entry(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(Entry other)
            {
                return base.Equals(other);
            }
        }
        public VMAP(int APIversion, EventHandler handler)
            : base(APIversion, handler,null)
        {
        }
        public VMAP(int APIversion, EventHandler handler,VMAP
            basis)
            : base(APIversion, handler, null)
        {
            Stream s = basis.UnParse();
            s.Position = 0L;
            Parse(s);
        }
        public VMAP(int APIversion, EventHandler handler, Stream s) 
            : base(APIversion, handler, s)
        {
        }


        
        [ElementPriority(1)]
        public EntryList Entries
        {
            get { return mEntries; }
            set { mEntries = value; OnRCOLChanged(this, new EventArgs()); }
        }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (mEntries.Count > 0)
                {
                    sb.AppendFormat("Entries:\n");
                    for (int i = 0; i < mEntries.Count; i++)
                    {
                        sb.AppendFormat("==Entry[{0}]==\n{1}\n", i, mEntries[i].Value);
                    }
                }
                return sb.ToString();
            }
        }

        private EntryList mEntries;
        protected override void Parse(Stream s)
        {
                BinaryReader br = new BinaryReader(s);
                mEntries = new EntryList(handler, s);

        }
        public override Stream UnParse()
        {
                if (mEntries == null) mEntries = new EntryList(handler);
                MemoryStream s = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(s);

                bw.Write(mEntries.Count);
                mEntries.UnParse(s);
                return s;
           
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new VMAP(0, handler, this);
        }

        public override string Tag
        {
            get { return "VMAP"; }
        }

        public override uint ResourceType
        {
            get { return 0xFFFFFFFF; }
        }

        private static bool checking = s3pi.Settings.Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}