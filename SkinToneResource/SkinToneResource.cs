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
    public class SkinToneResource : AResource
    {

        #region Nested Type: ShaderKey
        public class ShaderKey : AHandlerElement, IEquatable<ShaderKey>
        {
            #region Fields
            private AgeGenderFlags mAgeGenderFlags;
            private Color mEdgeColour;
            private Color mSpecularColour;
            private float mSpecularPower;
            private bool mIsGenetic;
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
            public Color EdgeColour
            {
                get { return mEdgeColour; }
                set { mEdgeColour = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Color SpecularColour
            {
                get { return mSpecularColour; }
                set { mSpecularColour = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public float SpecularPower
            {
                get { return mSpecularPower; }
                set { mSpecularPower = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public bool IsGenetic
            {
                get { return mIsGenetic; }
                set { mIsGenetic = value; OnElementChanged(); }
            }
            #endregion

            #region AHandlerElement
            public ShaderKey(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public ShaderKey(int APIversion, EventHandler handler, ShaderKey basis)
                : base(APIversion, handler)
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
                mEdgeColour = Color.FromArgb(br.ReadInt32());
                mSpecularColour = Color.FromArgb(br.ReadInt32());
                mSpecularPower = br.ReadSingle();
                mIsGenetic = br.ReadBoolean();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((UInt32)mAgeGenderFlags);
                bw.Write(mEdgeColour.ToArgb());
                bw.Write(mSpecularColour.ToArgb());
                bw.Write(mSpecularPower);
                bw.Write(mIsGenetic);
            }

        }
        #endregion

        #region Nested Type: TextureKey
        public class TextureKey : AHandlerElement, IEquatable<TextureKey>
        {
            #region Fields
            private SkinToneResource mOwner;
            private AgeGenderFlags mAgeGenderFlags;
            private DataTypeFlags mTypeFlags;
            private UInt32 mSpecularKeyIndex;
            private UInt32 mDetailDarkKeyIndex;
            private UInt32 mDetailLightKeyIndex;
            private UInt32 mNormalMapKeyIndex;
            private UInt32 mOverlayKeyIndex;
            #endregion

            #region Properties

            public string Value
            {
                get
                {
                    string str = "";
                    foreach (string field in this.ContentFields)
                    {
                        if (!field.Equals("Value") && !field.Equals("References"))
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
            public DataTypeFlags TypeFlags
            {
                get { return mTypeFlags; }
                set { mTypeFlags = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            [TGIBlockListContentField("References")]
            public UInt32 SpecularKeyIndex
            {
                get { return mSpecularKeyIndex; }
                set { mSpecularKeyIndex = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            [TGIBlockListContentField("References")]
            public UInt32 DetailDarkKeyIndex
            {
                get { return mDetailDarkKeyIndex; }
                set { mDetailDarkKeyIndex = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            [TGIBlockListContentField("References")]
            public UInt32 DetailLightKeyIndex
            {
                get { return mDetailLightKeyIndex; }
                set { mDetailLightKeyIndex = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            [TGIBlockListContentField("References")]
            public UInt32 NormalMapKeyIndex
            {
                get { return mNormalMapKeyIndex; }
                set { mNormalMapKeyIndex = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            [TGIBlockListContentField("References")]
            public UInt32 OverlayKeyIndex
            {
                get { return mOverlayKeyIndex; }
                set { mOverlayKeyIndex = value; OnElementChanged(); }
            }
            //Note: this is only here so elements can use TGIBlockListContentFieldAttribute
            public TGIBlockList References
            {
                get { return mOwner.References; }
                set { mOwner.References = value; }
            }
            #endregion

            #region AHandlerElement

            public TextureKey(int APIversion, EventHandler handler, SkinToneResource owner)
                : base(APIversion, handler)
            {
                mOwner = owner;
            }
            public TextureKey(int APIversion, EventHandler handler, TextureKey basis)
                : base(APIversion, handler)
            {
                MemoryStream ms = new MemoryStream();
                basis.UnParse(ms);
                ms.Position = 0L;
                Parse(ms);
            }
            public TextureKey(int APIversion, EventHandler handler, Stream s, SkinToneResource owner) : this(APIversion, handler, owner) { Parse(s); }

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

            public bool Equals(TextureKey other)
            {
                return base.Equals(other);
            }
            #endregion

            public void Parse(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                mAgeGenderFlags = (AgeGenderFlags)br.ReadUInt32();
                mTypeFlags = (DataTypeFlags)br.ReadUInt32();
                mSpecularKeyIndex = br.ReadUInt32();
                mDetailDarkKeyIndex = br.ReadUInt32();
                mDetailLightKeyIndex = br.ReadUInt32();
                mNormalMapKeyIndex = br.ReadUInt32();
                mOverlayKeyIndex = br.ReadUInt32();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write((UInt32)mAgeGenderFlags);
                bw.Write((UInt32)mTypeFlags);
                bw.Write(mSpecularKeyIndex);
                bw.Write(mDetailDarkKeyIndex);
                bw.Write(mDetailLightKeyIndex);
                bw.Write(mNormalMapKeyIndex);
                bw.Write(mOverlayKeyIndex);
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

        #region Nested Type: TextureKeyList
        public class TextureKeyList : DependentList<TextureKey>
        {
            private SkinToneResource mOwner;
            public TextureKeyList(EventHandler handler, SkinToneResource owner)
                : base(handler)
            {
                mOwner = owner;
            }
            public TextureKeyList(EventHandler handler, Stream s, SkinToneResource owner)
                : this(handler, owner)
            {
                Parse(s);
            }
            public override void Add()
            {
                this.Add(new TextureKey(0, null, mOwner));
            }

            protected override TextureKey CreateElement(Stream s)
            {
                TextureKey obj = new TextureKey(0, null, mOwner);
                obj.Parse(s);
                return obj;
            }

            protected override void WriteElement(Stream s, TextureKey element)
            {
                element.UnParse(s);
            }
        }
        #endregion

        #region Fields
        private UInt32 mVersion;
        private ShaderKeyList mShaderKeyList;
        private UInt32 mSkinRampIndex1;
        private UInt32 mSkinRampIndex2;
        private TextureKeyList mTextureKeyList;
        private Byte mIsDominant;
        private TGIBlockList mReferences;
        #endregion

        #region Properties
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
        [TGIBlockListContentField("References")]
        public UInt32 SkinRampIndex1
        {
            get { return mSkinRampIndex1; }
            set { mSkinRampIndex1 = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        [TGIBlockListContentField("References")]
        public UInt32 SkinRampIndex2
        {
            get { return mSkinRampIndex2; }
            set { mSkinRampIndex2 = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(5)]
        public TextureKeyList TextureKeys
        {
            get { return mTextureKeyList; }
            set { mTextureKeyList = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(6)]
        public Byte IsDominant
        {
            get { return mIsDominant; }
            set { mIsDominant = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(7)]
        public TGIBlockList References
        {
            get { return mReferences; }
            set { mReferences = value; OnResourceChanged(this, new EventArgs()); }
        }
        public String Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version: 0x{0:X8}\n", mVersion);
                if (mShaderKeyList.Count > 0)
                {
                    sb.AppendFormat("\nShaderKeys:\n");
                    for (int i = 0; i < mShaderKeyList.Count; i++)
                    {
                        sb.AppendFormat("==[{0}]==\n{1}\n", i, mShaderKeyList[i].Value);
                    }
                }
                sb.AppendFormat("SkinRampIndex1: 0X{0:x8}\n", mSkinRampIndex1);
                sb.AppendFormat("SkinRampIndex2: 0X{0:x8}\n", mSkinRampIndex2);

                if (mTextureKeyList.Count > 0)
                {
                    sb.AppendFormat("TextureKeys:\n");
                    for (int i = 0; i < mTextureKeyList.Count; i++)
                    {
                        sb.AppendFormat("==[{0}]==\n{1}\n", i, mTextureKeyList[i].Value);
                    }
                }
                sb.AppendFormat("IsDominant: {0}\n", mIsDominant);
                if (mReferences.Count > 0)
                {
                    sb.AppendFormat("References[{0}]:\n", mReferences.Count);
                    for (int i = 0; i < mReferences.Count; i++)
                    {
                        sb.AppendFormat("[0x{0:X8}]{1}\n", i, mReferences[i].Value);
                    }
                }
                return sb.ToString();
            }
        }
        #endregion


        public SkinToneResource(int apiVersion, Stream s)
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
            long tgioffset = br.ReadUInt32() + s.Position;
            long tgisize = br.ReadUInt32();
            mShaderKeyList = new ShaderKeyList(this.OnResourceChanged, s);
            mSkinRampIndex1 = br.ReadUInt32();
            mSkinRampIndex2 = br.ReadUInt32();
            mTextureKeyList = new TextureKeyList(this.OnResourceChanged, s, this);
            mIsDominant = br.ReadByte();
            mReferences = new TGIBlockList(this.OnResourceChanged, s, tgioffset, tgisize);
        }
        protected override Stream UnParse()
        {
            Stream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(mVersion);
            long pos = s.Position;
            bw.Write(0);
            bw.Write(0);
            if (mShaderKeyList == null) mShaderKeyList = new ShaderKeyList(OnResourceChanged);
            mShaderKeyList.UnParse(s);
            bw.Write(mSkinRampIndex1);
            bw.Write(mSkinRampIndex2);
            if (mTextureKeyList == null) mTextureKeyList = new TextureKeyList(OnResourceChanged, this);
            mTextureKeyList.UnParse(s);
            bw.Write(mIsDominant);
            if (mReferences == null) mReferences = new TGIBlockList(OnResourceChanged, false);
            mReferences.UnParse(s, pos);
            return s;
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}

