using System;
using System.Text;
using s3pi.Interfaces;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using s3pi.Settings;

namespace s3piwrappers
{
    public class ColourInfoResource : AResource
    {
        public class ARGB:AHandlerElement,IEquatable<ARGB>
        {
            private Color mVal;
            public ARGB(int APIversion, EventHandler handler) : this(APIversion, handler,Color.Empty) { }
            public ARGB(int APIversion, EventHandler handler, Int32 raw) : this(APIversion, handler, Color.FromArgb(raw)) { }
            public ARGB(int APIversion, EventHandler handler, Int32 a,Int32 r,Int32 g,Int32 b) : this(APIversion, handler, Color.FromArgb(a,r,g,b)) { }
            public ARGB(int APIversion, EventHandler handler, ARGB basis) : this(APIversion, handler,basis.Val) {}
            public ARGB(int APIversion, EventHandler handler, Color val) : base(APIversion, handler){mVal = val;}
            [ElementPriority(1)]
            public Color Val
            {
                get { return mVal; }
                set { if (mVal != value){mVal = value;OnElementChanged();} }
            }
            [ElementPriority(1)]
            public Int32 Raw
            {
                get { return mVal.ToArgb(); }
                set { var newVal = Color.FromArgb(value); if (mVal != newVal) {mVal = newVal; OnElementChanged();} }
            }

            public bool Equals(ARGB other)
            {
                return mVal.Equals(other.mVal);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new ARGB(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        }
        public class HSVBaseList : SimpleList<UInt32>
        {
            public HSVBaseList(EventHandler handler) : base(handler, ReadElement, WriteElement) { }
            public HSVBaseList(EventHandler handler, Stream s) : base(handler, s, ReadElement, WriteElement) { }
            static UInt32 ReadElement(Stream s) { return new BinaryReader(s).ReadUInt32(); }
            static void WriteElement(Stream s, UInt32 element) { new BinaryWriter(s).Write(element); }
        }
        public class ARGBList : DependentList<ARGB>
        {
            public ARGBList(EventHandler handler, Stream s) : base(handler, s) {}
            public ARGBList(EventHandler handler, IList<ARGB> ilt) : base(handler, ilt) {}
            public ARGBList(EventHandler handler) : base(handler) {}

            public override void Add()
            {
                base.Add(new object[] {});
            }

            protected override ARGB CreateElement(Stream s)
            {
                return new ARGB(0,handler,new BinaryReader(s).ReadInt32());
            }

            protected override void WriteElement(Stream s, ARGB element)
            {
                new BinaryWriter(s).Write(element.Raw);
            }
        }
        public class FabricInfo : AHandlerElement, IEquatable<FabricInfo>
        {

            private Byte mXmlIndex;
            private ARGBList mColours;
            private HSVBaseList mHSVBases;
            public FabricInfo(int APIversion, EventHandler handler): this(APIversion, handler,0,new ARGBList(handler),new HSVBaseList(handler) ){}
            public FabricInfo(int APIversion, EventHandler handler, FabricInfo basis): this(APIversion, handler,basis.XmlIndex,basis.Colours,basis.HSVBases){}
            public FabricInfo(int APIversion, EventHandler handler, Stream s): base(APIversion, handler){Parse(s);}
            public FabricInfo(int APIversion, EventHandler handler, byte xmlIndex, ARGBList colours, HSVBaseList hsvBases) : base(APIversion, handler)
            {
                mXmlIndex = xmlIndex;
                mColours = colours;
                mHSVBases = hsvBases;
            }

            [ElementPriority(1)]
            public byte XmlIndex
            {
                get { return mXmlIndex; }
                set { if(mXmlIndex!=value){mXmlIndex = value; OnElementChanged();} }
            }
            [ElementPriority(2)]
            public ARGBList Colours
            {
                get { return mColours; }
                set { if(mColours!=value){mColours = value; OnElementChanged();} }
            }
            [ElementPriority(3)]
            public HSVBaseList HSVBases
            {
                get { return mHSVBases; }
                set { if(mHSVBases!=value){mHSVBases = value; OnElementChanged();} }
            }

            public string Value
            {
                get
                {

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("XmlIndex: 0x{0:X8}\n", mXmlIndex);
                    sb.AppendFormat("\nColours:\n");
                    for (int i = 0; i < mColours.Count; i++) sb.AppendFormat("[{0,2:00}]{1}\n", i, mColours[i]);
                    sb.AppendFormat("\nHSV Bases:\n");
                    for (int i = 0; i < mHSVBases.Count; i++) sb.AppendFormat("[{0,2:00}]{1}\n", i, mHSVBases[i]);
                    return sb.ToString();
                }
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mXmlIndex = br.ReadByte();
                mColours = new ARGBList(handler, s);
                mHSVBases = new HSVBaseList(handler, s);

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mXmlIndex);
                if (mColours == null) mColours = new ARGBList(handler);
                mColours.UnParse(s);
                if (mHSVBases == null) mHSVBases = new HSVBaseList(handler);
                mHSVBases.UnParse(s);
            }
            public bool Equals(FabricInfo other)
            {
                return base.Equals(other);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new FabricInfo(base.requestedApiVersion, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        }
        public class FabricInfoList : DependentList<FabricInfo>
        {
            public FabricInfoList(EventHandler handler): base(handler){}
            public FabricInfoList(EventHandler handler, Stream s): base(handler, s){}
            public override void Add()
            {
                base.Add(new object[] { });
            }
            protected override FabricInfo CreateElement(Stream s)
            {
                return new FabricInfo(0, elementHandler, s);
            }

            protected override void WriteElement(Stream s, FabricInfo element)
            {
                element.UnParse(s);
            }
        }
        public enum eUsageSubCategory : ushort
        {
            MakeupBlush = 0x1,
            MakeupEyebrow = 0x5,
            MakeupEyeliner = 0x2,
            MakeupEyeshadow = 0x3,
            MakeupLipstick = 0x4,
            None = 0x0
        }



        private UInt32 mVersion = 0x00000004;
        private eUsageSubCategory mUsageSubCategory;
        private ARGBList mColours;
        private FabricInfoList mFabrics;
        private CountedTGIBlockList mReferences;

        public ColourInfoResource(int apiVersion, Stream s)
            : base(apiVersion, s)
        {
            if (base.stream == null)
            {
                base.stream = this.UnParse();
                this.OnResourceChanged(this, new EventArgs());
            }
            base.stream.Position = 0L;

            Parse(base.stream);
        }

        [ElementPriority(1)]
        public UInt32 Version
        {
            get { return mVersion; }
            set { if(mVersion!=value){mVersion = value; OnResourceChanged(this, new EventArgs());} }
        }
        [ElementPriority(2)]
        public eUsageSubCategory UsageSubCategory
        {
            get { return mUsageSubCategory; }
            set { if(mUsageSubCategory!=value){mUsageSubCategory = value; OnResourceChanged(this, new EventArgs());} }
        }
        [ElementPriority(3)]
        public ARGBList Colours
        {
            get { return mColours; }
            set { if(mColours!=value){mColours = value; OnResourceChanged(this, new EventArgs());} }
        }
        [ElementPriority(4)]
        public FabricInfoList Fabrics
        {
            get { return mFabrics; }
            set { if(mFabrics!=value){mFabrics = value; OnResourceChanged(this, new EventArgs());} }
        }
        [ElementPriority(5)]
        public CountedTGIBlockList References
        {
            get { return mReferences; }
            set { if(mReferences!=value){mReferences = value; OnResourceChanged(this, new EventArgs());} }
        }
        public String Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version: 0x{0:X8}\n", mVersion);
                sb.AppendFormat("Usage: {0}\n", this["UsageSubCategory"]);

                sb.AppendFormat("\nColours:\n");
                for (int i = 0; i < mColours.Count; i++) sb.AppendFormat("[{0,2:00}]{1}\n", i, mColours[i]);
                sb.AppendFormat("\nFabrics:\n");
                for (int i = 0; i < mFabrics.Count; i++) sb.AppendFormat("==[{0}]==\n{1}\n", i, mFabrics[i].Value);
                sb.AppendFormat("\nReferences[{0}]:\n", mReferences.Count);
                for (int i = 0; i < mReferences.Count; i++) sb.AppendFormat("[0x{0:X8}]{1}\n", i, mReferences[i].Value);
                return sb.ToString();
            }
        }

        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            mVersion = br.ReadUInt32();
            long offset = br.ReadUInt32() + s.Position;
            mUsageSubCategory = (eUsageSubCategory)br.ReadUInt16();
            mColours = new ARGBList(this.OnResourceChanged, s);
            mFabrics = new FabricInfoList(this.OnResourceChanged, s);
            if (checking && (offset != s.Position))
            {
                throw new InvalidDataException(string.Format("Position of TGIBlock read: 0x{0:X8}, actual: 0x{1:X8}", offset, s.Position));
            }
            uint count = br.ReadByte();
            mReferences = new CountedTGIBlockList(this.OnResourceChanged, "IGT", count, s);
        }
        protected override Stream UnParse()
        {
            Stream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(mVersion);
            long pos = s.Position;
            bw.Write(0);
            bw.Write((UInt16)mUsageSubCategory);
            if (mColours == null) mColours = new ARGBList(OnResourceChanged);
            mColours.UnParse(s);
            if (mFabrics == null) mFabrics = new FabricInfoList(OnResourceChanged);
            mFabrics.UnParse(s);
            long tgiOffset = s.Position - (pos + 4);
            if (mReferences == null) mReferences = new CountedTGIBlockList(OnResourceChanged, "IGT");
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

        private static bool checking = Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}

