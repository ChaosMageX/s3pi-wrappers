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

        #region Nested Type: ShaderKey
        public class ShaderKey : AHandlerElement, IEquatable<ShaderKey>
        {
            #region Fields
            private AgeGenderFlags mAgeGenderFlags;
            private bool mIsGenetic;
            private Color mDiffuseColour;
            private Color mRootColour;
            private Color mHighlightColour;
            private Color mTipColour;
            private Color mHaloHighColour;
            private Color mHaloLowColour;
            private float mHaloBlur;
            #endregion

            #region Properties

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
            [ElementPriority(1)]
            public AgeGenderFlags AgeGenderFlags
            {
                get { return mAgeGenderFlags; }
                set { mAgeGenderFlags = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public bool IsGenetic
            {
                get { return mIsGenetic; }
                set { mIsGenetic = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Color DiffuseColour
            {
                get { return mDiffuseColour; }
                set { mDiffuseColour = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public Color RootColour
            {
                get { return mRootColour; }
                set { mRootColour = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public Color HighlightColour
            {
                get { return mHighlightColour; }
                set { mHighlightColour = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public Color TipColour
            {
                get { return mTipColour; }
                set { mTipColour = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public Color HaloHighColour
            {
                get { return mHaloHighColour; }
                set { mHaloHighColour = value; OnElementChanged(); }
            }
            [ElementPriority(8)]

            public Color HaloLowColour
            {
                get { return mHaloLowColour; }
                set { mHaloLowColour = value; OnElementChanged(); }
            }
            [ElementPriority(9)]
            public float HaloBlur
            {
                get { return mHaloBlur; }
                set { mHaloBlur = value; OnElementChanged(); }
            }

            
            

            #endregion

            #region AHandlerElement
            public ShaderKey(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public ShaderKey(int APIversion, EventHandler handler,ShaderKey basis) : base(APIversion, handler)
            {
                MemoryStream ms = new MemoryStream();
                basis.UnParse(ms);
                ms.Position = 0L;
                Parse(ms);
            }
            public ShaderKey(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }


            public override AHandlerElement Clone(EventHandler handler)
            {
                return (AHandlerElement)MemberwiseClone();
            }

            public override List<string> ContentFields
            {
                get
                {
                    return AApiVersionedFields.GetContentFields(base.requestedApiVersion, base.GetType());
                }
            }

            public override int RecommendedApiVersion
            {
                get { return 1; }
            }

            public bool Equals(ShaderKey other)
            {
                return base.Equals(other);
            }
            #endregion

            public void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mAgeGenderFlags = (AgeGenderFlags)br.ReadUInt32();
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
                bw.Write((UInt32)mAgeGenderFlags);
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
        #endregion

        #region Nested Type: ShaderKeyList
        public class ShaderKeyList : DependentList<ShaderKey>
        {
            public ShaderKeyList(EventHandler handler) : base(handler) { }
            public ShaderKeyList(EventHandler handler, Stream s) : base(handler, s) { }
            public override void Add()
            {
                this.Add(new ShaderKey(0, null));
            }

            protected override ShaderKey CreateElement(Stream s)
            {
                ShaderKey obj = new ShaderKey(0, null);
                obj.Parse(s);
                return obj;
            }

            protected override void WriteElement(Stream s, ShaderKey element)
            {
                element.UnParse(s);
            }
        }
        #endregion
        
        #region Fields
        private UInt32 mVersion;
        private ShaderKeyList mShaderKeyList;
        private Byte mIsDominant;
        #endregion

        #region Properties

        public String Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version: 0x{0:X8}\n", mVersion);
                sb.AppendFormat("\nShaderKeys:\n");
                for (int i = 0; i < mShaderKeyList.Count; i++) sb.AppendFormat("==[{0}]==\n{1}\n", i, mShaderKeyList[i].Value);
                sb.AppendFormat("IsDominant: {0}\n", mIsDominant);
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
        public ShaderKeyList ShaderKeys
        {
            get { return mShaderKeyList; }
            set { mShaderKeyList = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public Byte IsDominant
        {
            get { return mIsDominant; }
            set { mIsDominant = value; OnResourceChanged(this, new EventArgs()); }
        }
        #endregion


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
            if(mShaderKeyList == null)mShaderKeyList = new ShaderKeyList(OnResourceChanged);
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

