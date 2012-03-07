using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using CASPartResource;

namespace s3piwrappers
{
    public class HairToneResource : AResource
    {

        public class ShaderKey : AHandlerElement, IEquatable<ShaderKey>
        {
            private AgeGenderFlags mAgeGenderFlags;
            private bool mIsGenetic;
            private Color mDiffuseColour;
            private Color mRootColour;
            private Color mHighlightColour;
            private Color mTipColour;
            private Color mHaloHighColour;
            private Color mHaloLowColour;
            private float mHaloBlur;

            public ShaderKey(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public ShaderKey(int APIversion, EventHandler handler, ShaderKey basis): this(APIversion, handler,basis.AgeGenderFlags,basis.IsGenetic,basis.DiffuseColour,basis.RootColour,basis.HighlightColour,basis.TipColour,basis.HaloHighColour,basis.HaloLowColour,basis.HaloBlur){}
            public ShaderKey(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }
            public ShaderKey(int APIversion, EventHandler handler, AgeGenderFlags ageGenderFlags, bool isGenetic, Color diffuseColour, Color rootColour, Color highlightColour, Color tipColour, Color haloHighColour, Color haloLowColour, float haloBlur) : base(APIversion, handler)
            {
                mAgeGenderFlags = ageGenderFlags;
                mIsGenetic = isGenetic;
                mDiffuseColour = diffuseColour;
                mRootColour = rootColour;
                mHighlightColour = highlightColour;
                mTipColour = tipColour;
                mHaloHighColour = haloHighColour;
                mHaloLowColour = haloLowColour;
                mHaloBlur = haloBlur;
            }

            [ElementPriority(1)]
            public AgeGenderFlags AgeGenderFlags
            {
                get { return mAgeGenderFlags; }
                set { if(mAgeGenderFlags!=value){mAgeGenderFlags = value; OnElementChanged();} }
            }
            [ElementPriority(2)]
            public bool IsGenetic
            {
                get { return mIsGenetic; }
                set { if(mIsGenetic!=value){mIsGenetic = value; OnElementChanged();} }
            }
            [ElementPriority(3)]
            public Color DiffuseColour
            {
                get { return mDiffuseColour; }
                set { if(mDiffuseColour!=value){mDiffuseColour = value; OnElementChanged();} }
            }
            [ElementPriority(4)]
            public Color RootColour
            {
                get { return mRootColour; }
                set { if(mRootColour!=value){mRootColour = value; OnElementChanged();} }
            }
            [ElementPriority(5)]
            public Color HighlightColour
            {
                get { return mHighlightColour; }
                set { if(mHighlightColour!=value){mHighlightColour = value; OnElementChanged();} }
            }
            [ElementPriority(6)]
            public Color TipColour
            {
                get { return mTipColour; }
                set { if(mTipColour!=value){mTipColour = value; OnElementChanged();} }
            }
            [ElementPriority(7)]
            public Color HaloHighColour
            {
                get { return mHaloHighColour; }
                set { if(mHaloHighColour!=value){mHaloHighColour = value; OnElementChanged();} }
            }
            [ElementPriority(8)]

            public Color HaloLowColour
            {
                get { return mHaloLowColour; }
                set { if(mHaloLowColour!=value){mHaloLowColour = value; OnElementChanged();} }
            }
            [ElementPriority(9)]
            public float HaloBlur
            {
                get { return mHaloBlur; }
                set { if(mHaloBlur!=value){mHaloBlur = value; OnElementChanged();} }
            }
            public string Value
            {
                get
                {
                    string str = "";
                    foreach (string field in this.ContentFields)
                    {
                        if (!field.Equals("Value"))
                        {
                            str = str + string.Format("{0}:\t{1}\n", field, this[field]);
                        }
                    }
                    return str.Trim();
                }
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new ShaderKey(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get {return GetContentFields(base.requestedApiVersion, base.GetType());}
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            public bool Equals(ShaderKey other)
            {
                return base.Equals(other);
            }


            public void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mAgeGenderFlags = new AgeGenderFlags(0,handler,s);
                mIsGenetic = br.ReadBoolean();
                mDiffuseColour = Color.FromArgb(br.ReadInt32());
                mRootColour = Color.FromArgb(br.ReadInt32());
                mHighlightColour = Color.FromArgb(br.ReadInt32());
                mTipColour = Color.FromArgb(br.ReadInt32());
                mHaloHighColour = Color.FromArgb(br.ReadInt32());
                mHaloLowColour = Color.FromArgb(br.ReadInt32());
                mHaloBlur = br.ReadSingle();

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                mAgeGenderFlags.UnParse(s);
                bw.Write(mIsGenetic);
                bw.Write(mDiffuseColour.ToArgb());
                bw.Write(mRootColour.ToArgb());
                bw.Write(mHighlightColour.ToArgb());
                bw.Write(mTipColour.ToArgb());
                bw.Write(mHaloHighColour.ToArgb());
                bw.Write(mHaloLowColour.ToArgb());
                bw.Write(mHaloBlur);
            }

        }


        public class ShaderKeyList : DependentList<ShaderKey>
        {
            public ShaderKeyList(EventHandler handler) : base(handler) { }
            public ShaderKeyList(EventHandler handler, Stream s) : base(handler, s) { }
            public ShaderKeyList(EventHandler handler, IEnumerable<ShaderKey> ilt) : base(handler, ilt) {}

            public override void Add()
            {
                this.Add(new object[]{});
            }

            protected override ShaderKey CreateElement(Stream s)
            {
                return new ShaderKey(0,handler, s);
            }

            protected override void WriteElement(Stream s, ShaderKey element)
            {
                element.UnParse(s);
            }
        }


        private UInt32 mVersion;
        private ShaderKeyList mShaderKeyList;
        private Byte mIsDominant;

        public HairToneResource(int apiVersion, Stream s)
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
        public ShaderKeyList ShaderKeys
        {
            get { return mShaderKeyList; }
            set { if(mShaderKeyList!=value){mShaderKeyList = value; OnResourceChanged(this, new EventArgs());} }
        }
        [ElementPriority(3)]
        public Byte IsDominant
        {
            get { return mIsDominant; }
            set { if(mIsDominant!=value){mIsDominant = value; OnResourceChanged(this, new EventArgs());} }
        }

        public String Value
        {
            get
            {
                return ValueBuilder;
                //StringBuilder sb = new StringBuilder();
                //sb.AppendFormat("Version: 0x{0:X8}\n", mVersion);
                //if (mShaderKeyList.Count > 0)
                //{
                //    sb.AppendFormat("\nShaderKeys:\n");
                //    for (int i = 0; i < mShaderKeyList.Count; i++)
                //    {
                //        sb.AppendFormat("==[{0}]==\n{1}\n", i, mShaderKeyList[i].Value);
                //    }
                //}
                //sb.AppendFormat("IsDominant: {0}\n", mIsDominant);
                //return sb.ToString();
            }
        }
        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            mVersion = br.ReadUInt32();
            mShaderKeyList = new ShaderKeyList(this.OnResourceChanged, s);
            mIsDominant = br.ReadByte();
        }
        protected override Stream UnParse()
        {
            Stream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(mVersion);
            if (mShaderKeyList == null) mShaderKeyList = new ShaderKeyList(OnResourceChanged);
            mShaderKeyList.UnParse(s);
            bw.Write(mIsDominant);
            return s;
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}

