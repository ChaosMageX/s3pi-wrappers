using System;
using System.Text;
using s3pi.Interfaces;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace s3piwrappers
{
    public class ColourInfoResource : AResource
    {
        public class ARGB : AHandlerElement, IEquatable<ARGB>
        {
            private Int32 mValue;

            public ARGB(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
            }
            public ARGB(int APIversion, EventHandler handler, Int32 value)
                : base(APIversion, handler)
            {
                mValue = value;
            }
            public ARGB(int APIversion, EventHandler handler, ARGB basis)
                : this(APIversion, handler, basis.mValue)
            {

            }
            public ARGB(int APIversion, EventHandler handler,Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            private void Parse(Stream s)
            {
                mValue = new BinaryReader(s).ReadInt32();
            }
            public void UnParse(Stream s)
            {
                new BinaryWriter(s).Write(mValue);
            }
            [ElementPriority(1)]
            public Color Colour
            {
                get { return System.Drawing.Color.FromArgb(mValue); }
                set { mValue = value.ToArgb(); OnElementChanged(); }
            }
            [ElementPriority(2)]
            public Int32 Raw
            {
                get { return mValue; }
                set { mValue = value; OnElementChanged(); }
            }

            public bool Equals(ARGB other)
            {
                return mValue.Equals(other.mValue);
            }


            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        }
        public class ARGBList : DependentList<ARGB>
        {
            public ARGBList(EventHandler handler) : base(handler)
            {
            }

            public ARGBList(EventHandler handler, Stream s) : base(handler, s)
            {
            }

            public override void Add()
            {
                base.Add(new object[] {});
            }

            protected override ARGB CreateElement(Stream s)
            {
                return new ARGB(0, elementHandler, s);
            }

            protected override void WriteElement(Stream s, ARGB element)
            {
                element.UnParse(s);
            }
        }
        public class Entry : AHandlerElement, IEquatable<Entry>
        {
            #region Fields

            private Byte mXmlIndex;
            private ARGBList mColourList;
            private UInt32 mUnknown01;
            #endregion

            public Entry(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
                mColourList = new ARGBList(handler);
            }
            public Entry(int APIversion, EventHandler handler,Entry basis)
                : base(APIversion, handler)
            {
                System.IO.Stream s = new MemoryStream();
                basis.UnParse(s);
                s.Position = 0L;
                Parse(s);
            }
            public Entry(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            [ElementPriority(1)]
            public byte XmlIndex
            {
                get { return mXmlIndex; }
                set { mXmlIndex = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public ARGBList ColourList1
            {
                get { return mColourList; }
                set { mColourList = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public uint Unknown01
            {
                get { return mUnknown01; }
                set { mUnknown01 = value; OnElementChanged(); }
            }

            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mXmlIndex = br.ReadByte();
                mColourList = new ARGBList(handler,s);
                mUnknown01 = br.ReadUInt32();

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mXmlIndex);
                if(mColourList == null) mColourList = new ARGBList(handler);
                mColourList.UnParse(s);
                bw.Write(mUnknown01);
            }
            public bool Equals(Entry other)
            {
                return base.Equals(other);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        }
        public class EntryList : DependentList<Entry>
        {
            public EntryList(EventHandler handler) : base(handler)
            {
            }

            public EntryList(EventHandler handler, Stream s) : base(handler, s)
            {
            }

            public override void Add()
            {
                base.Add(new object[] {});
            }

            protected override Entry CreateElement(Stream s)
            {
                return new Entry(0,elementHandler,s);
            }

            protected override void WriteElement(Stream s, Entry element)
            {
                element.UnParse(s);
            }
        }
        
        #region Fields
        private UInt32 mVersion;
        private UInt16 mUnknown01;
        private ARGBList mColours;
        private EntryList mEntries;
        private CountedTGIBlockList mReferences;

        #endregion

        #region Properties
        [ElementPriority(1)]
        public UInt32 Version
        {
            get { return mVersion; }
            set { mVersion = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(2)]
        public ushort Unknown01
        {
            get { return mUnknown01; }
            set { mUnknown01 = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public ARGBList Colours
        {
            get { return mColours; }
            set { mColours = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        public EntryList Entries
        {
            get { return mEntries; }
            set { mEntries = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(5)]
        public CountedTGIBlockList References
        {
            get { return mReferences; }
            set { mReferences = value; OnResourceChanged(this, new EventArgs()); }
        }

        #endregion


        public ColourInfoResource(int apiVersion, Stream s)
            : base(apiVersion, s)
        {
            mVersion = 0x00000004;
            if (base.stream == null)
            {
                base.stream = this.UnParse();
                this.OnResourceChanged(this, new EventArgs());
            }
            base.stream.Position = 0L;

            Parse(base.stream);
        }
        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            mVersion = br.ReadUInt32();
            long offset = br.ReadUInt32() + s.Position;
            mUnknown01 = br.ReadUInt16();
            mColours = new ARGBList(this.OnResourceChanged,s);
            mEntries = new EntryList(this.OnResourceChanged, s);
            if (checking && (offset != s.Position))
            {
                throw new InvalidDataException(string.Format("Position of TGIBlock read: 0x{0:X8}, actual: 0x{1:X8}", offset, s.Position));
            }
            uint count = br.ReadByte();
            mReferences = new CountedTGIBlockList(this.OnResourceChanged,"IGT",count,s);
        }
        protected override Stream UnParse()
        {
            Stream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(mVersion);
            long pos = s.Position;
            bw.Write(0);
            bw.Write(mUnknown01);
            if (mColours == null) mColours = new ARGBList(OnResourceChanged);
            mColours.UnParse(s);
            if (mEntries == null) mEntries = new EntryList(OnResourceChanged);
            mEntries.UnParse(s);
            long tgiOffset = s.Position - (pos + sizeof(uint));
            if (mReferences == null) mReferences = new CountedTGIBlockList(OnResourceChanged,"IGT");
            bw.Write((byte)mReferences.Count);
            mReferences.UnParse(s);

            s.Seek(pos, SeekOrigin.Begin);
            bw.Write((uint)tgiOffset);
            return s;
        }

        public override int RecommendedApiVersion
        {
            get { return kRecommendedApiVersion; }
        }

        private static bool checking = s3pi.Settings.Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}

