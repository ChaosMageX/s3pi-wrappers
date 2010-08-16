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
        public class HSVBase : AHandlerElement,IEquatable<HSVBase>
        {
            private UInt32 mValue;
            public HSVBase(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
            }
            public HSVBase(int APIversion, EventHandler handler, UInt32 value)
                : base(APIversion, handler)
            {
                mValue = value;
            }
            public HSVBase(int APIversion, EventHandler handler, HSVBase basis)
                : this(APIversion, handler, basis.mValue)
            {

            }
            public HSVBase(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            private void Parse(Stream s)
            {
                mValue = new BinaryReader(s).ReadUInt32();
            }
            public void UnParse(Stream s)
            {
                new BinaryWriter(s).Write(mValue);
            }

            public string Value
            {
                get { return String.Format("HSV Base: {0}",mValue); }
            }
            [ElementPriority(1)]
            public UInt32 Base
            {
                get { return mValue; }
                set { mValue = value; OnElementChanged(); }
            }

            public bool Equals(HSVBase other)
            {
                return mValue.Equals(other.mValue);
            }


            public override AHandlerElement Clone(EventHandler handler)
            {
                return new HSVBase(base.requestedApiVersion, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        
        }
        public class HSVBaseList : DependentList<HSVBase>
        {
            public HSVBaseList(EventHandler handler)
                : base(handler)
            {
            }

            public HSVBaseList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public override void Add()
            {
                base.Add(new object[] { });
            }

            protected override HSVBase CreateElement(Stream s)
            {
                return new HSVBase(0, elementHandler, s);
            }

            protected override void WriteElement(Stream s, HSVBase element)
            {
                element.UnParse(s);
            }
        }
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

            public string Value
            {
                get { return Colour.ToString(); }
            }
            [ElementPriority(1)]
            public Color Colour
            {
                get { return System.Drawing.Color.FromArgb(mValue); }
                set { mValue = value.ToArgb(); OnElementChanged(); }
            }

            public bool Equals(ARGB other)
            {
                return mValue.Equals(other.mValue);
            }


            public override AHandlerElement Clone(EventHandler handler)
            {
                return new ARGB(base.requestedApiVersion, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion,GetType()); }
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
        public class FabricInfo : AHandlerElement, IEquatable<FabricInfo>
        {
            #region Fields

            private Byte mXmlIndex;
            private ARGBList mColours;
            private HSVBaseList mHSVBases;
            #endregion

            public FabricInfo(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
                mColours = new ARGBList(handler);
                mHSVBases=new HSVBaseList(handler);
            }
            public FabricInfo(int APIversion, EventHandler handler,FabricInfo basis)
                : base(APIversion, handler)
            {
                System.IO.Stream s = new MemoryStream();
                basis.UnParse(s);
                s.Position = 0L;
                Parse(s);
            }
            public FabricInfo(int APIversion, EventHandler handler, Stream s)
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
            public ARGBList Colours
            {
                get { return mColours; }
                set { mColours = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public HSVBaseList HSVBases
            {
                get { return mHSVBases; }
                set { mHSVBases = value; OnElementChanged(); }
            }

            public string Value
            {
                get
                {

                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("XmlIndex: 0x{0:X8}\n", mXmlIndex);
                    sb.AppendFormat("\nColours:\n");
                    for (int i = 0; i < mColours.Count; i++) sb.AppendFormat("[{0,2:00}]{1}\n", i, mColours[i].Value);
                    sb.AppendFormat("\nHSV Bases:\n");
                    for (int i = 0; i < mHSVBases.Count; i++) sb.AppendFormat("[{0,2:00}]{1}\n", i, mHSVBases[i].Value);
                    return sb.ToString();
                }
            }
            private void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mXmlIndex = br.ReadByte();
                mColours = new ARGBList(handler,s);
                mHSVBases = new HSVBaseList(handler,s);

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mXmlIndex);
                if(mColours == null) mColours = new ARGBList(handler);
                mColours.UnParse(s);
                if(mHSVBases == null)mHSVBases = new HSVBaseList(handler);
                mHSVBases.UnParse(s);
            }
            public bool Equals(FabricInfo other)
            {
                return base.Equals(other);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new FabricInfo(base.requestedApiVersion, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
        }
        public class FabricInfoList : DependentList<FabricInfo>
        {
            public FabricInfoList(EventHandler handler) : base(handler)
            {
            }

            public FabricInfoList(EventHandler handler, Stream s) : base(handler, s)
            {
            }

            public override void Add()
            {
                base.Add(new object[] {});
            }

            protected override FabricInfo CreateElement(Stream s)
            {
                return new FabricInfo(0,elementHandler,s);
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

 

        #region Fields
        private UInt32 mVersion;
        private eUsageSubCategory mUsageSubCategory;
        private ARGBList mColours;
        private FabricInfoList mFabrics;
        private CountedTGIBlockList mReferences;

        #endregion

        #region Properties
        
        public String Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version: 0x{0:X8}\n", mVersion);
                sb.AppendFormat("Usage: {0}\n", this["UsageSubCategory"]);

                sb.AppendFormat("\nColours:\n");
                for (int i = 0; i < mColours.Count; i++) sb.AppendFormat("[{0,2:00}]{1}\n", i, mColours[i].Value);
                sb.AppendFormat("\nFabrics:\n");
                for (int i = 0; i < mFabrics.Count; i++) sb.AppendFormat("==[{0}]==\n{1}\n", i, mFabrics[i].Value);
                sb.AppendFormat("\nReferences[{0}]:\n",mReferences.Count);
                for (int i = 0; i < mReferences.Count;i++ ) sb.AppendFormat("[0x{0:X8}]{1}\n",i,mReferences[i].Value);
                return sb.ToString();
            }
        }
        [ElementPriority(1)]
        public UInt32 Version
        {
            get { return mVersion; }
            set { mVersion = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(2)]
        public eUsageSubCategory UsageSubCategory
        {
            get { return mUsageSubCategory; }
            set { mUsageSubCategory = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public ARGBList Colours
        {
            get { return mColours; }
            set { mColours = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        public FabricInfoList Fabrics
        {
            get { return mFabrics; }
            set { mFabrics = value; OnResourceChanged(this, new EventArgs()); }
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
            mUsageSubCategory = (eUsageSubCategory)br.ReadUInt16();
            mColours = new ARGBList(this.OnResourceChanged,s);
            mFabrics = new FabricInfoList(this.OnResourceChanged, s);
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
            bw.Write((UInt16)mUsageSubCategory);
            if (mColours == null) mColours = new ARGBList(OnResourceChanged);
            mColours.UnParse(s);
            if (mFabrics == null) mFabrics = new FabricInfoList(OnResourceChanged);
            mFabrics.UnParse(s);
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

