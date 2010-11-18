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

        public class ShaderKey : AHandlerElement, IEquatable<ShaderKey>
        {
            private AgeGenderFlags mAgeGenderFlags;
            private Color mEdgeColour;
            private Color mSpecularColour;
            private float mSpecularPower;
            private bool mIsGenetic;

            public ShaderKey(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public ShaderKey(int APIversion, EventHandler handler, ShaderKey basis) : this(APIversion, handler, basis.AgeGenderFlags, basis.EdgeColour, basis.SpecularColour, basis.SpecularPower, basis.IsGenetic) { }
            public ShaderKey(int APIversion, EventHandler handler, AgeGenderFlags ageGenderFlags, Color edgeColour, Color specularColour, float specularPower, bool isGenetic)
                : base(APIversion, handler)
            {
                mAgeGenderFlags = ageGenderFlags;
                mEdgeColour = edgeColour;
                mSpecularColour = specularColour;
                mSpecularPower = specularPower;
                mIsGenetic = isGenetic;
            }

            [ElementPriority(1)]
            public AgeGenderFlags AgeGenderFlags
            {
                get { return mAgeGenderFlags; }
                set { if (mAgeGenderFlags != value) { mAgeGenderFlags = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public Color EdgeColour
            {
                get { return mEdgeColour; }
                set { if (mEdgeColour != value) { mEdgeColour = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            public Color SpecularColour
            {
                get { return mSpecularColour; }
                set { if (mSpecularColour != value) { mSpecularColour = value; OnElementChanged(); } }
            }
            [ElementPriority(4)]
            public float SpecularPower
            {
                get { return mSpecularPower; }
                set { if (mSpecularPower != value) { mSpecularPower = value; OnElementChanged(); } }
            }
            [ElementPriority(5)]
            public bool IsGenetic
            {
                get { return mIsGenetic; }
                set { if (mIsGenetic != value) { mIsGenetic = value; OnElementChanged(); } }
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

            public ShaderKey(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }

            public override AHandlerElement Clone(EventHandler handler)
            {
                return new ShaderKey(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(base.requestedApiVersion, base.GetType()); }
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

        public class TextureKey : AHandlerElement, IEquatable<TextureKey>
        {
            private SkinToneResource mOwner;
            private AgeGenderFlags mAgeGenderFlags;
            private DataTypeFlags mTypeFlags;
            private UInt32 mSpecularKeyIndex;
            private UInt32 mDetailDarkKeyIndex;
            private UInt32 mDetailLightKeyIndex;
            private UInt32 mNormalMapKeyIndex;
            private UInt32 mOverlayKeyIndex;
            private UInt32 mMuscleNormalMapKeyIndex;
            private UInt32 mCleavageNormalMapKeyIndex;


            public TextureKey(int APIversion, EventHandler handler, SkinToneResource owner): base(APIversion, handler){mOwner = owner;}
            public TextureKey(int APIversion, EventHandler handler, TextureKey basis): this(APIversion, handler, basis.mOwner, basis.AgeGenderFlags, basis.mTypeFlags, basis.SpecularKeyIndex, basis.DetailDarkKeyIndex, basis.DetailLightKeyIndex, basis.NormalMapKeyIndex, basis.OverlayKeyIndex,basis.MuscleNormalMapKeyIndex,basis.CleavageNormalMapKeyIndex){}
            public TextureKey(int APIversion, EventHandler handler, Stream s, SkinToneResource owner): this(APIversion, handler, owner) { Parse(s); }
            public TextureKey(int APIversion, EventHandler handler, SkinToneResource owner, AgeGenderFlags ageGenderFlags, DataTypeFlags typeFlags, uint specularKeyIndex, uint detailDarkKeyIndex, uint detailLightKeyIndex, uint normalMapKeyIndex, uint overlayKeyIndex, uint muscleNormalMapMapKeyIndex, uint cleavageNormalMapKeyIndex)
                : this(APIversion, handler, owner)
            {
                mAgeGenderFlags = ageGenderFlags;
                mTypeFlags = typeFlags;
                mSpecularKeyIndex = specularKeyIndex;
                mDetailDarkKeyIndex = detailDarkKeyIndex;
                mDetailLightKeyIndex = detailLightKeyIndex;
                mNormalMapKeyIndex = normalMapKeyIndex;
                mOverlayKeyIndex = overlayKeyIndex;
                mMuscleNormalMapKeyIndex = muscleNormalMapMapKeyIndex;
                mCleavageNormalMapKeyIndex = cleavageNormalMapKeyIndex;
            }

            [ElementPriority(1)]
            public AgeGenderFlags AgeGenderFlags
            {
                get { return mAgeGenderFlags; }
                set { if (mAgeGenderFlags != value) { mAgeGenderFlags = value; OnElementChanged(); } }
            }
            [ElementPriority(2)]
            public DataTypeFlags TypeFlags
            {
                get { return mTypeFlags; }
                set { if (mTypeFlags != value) { mTypeFlags = value; OnElementChanged(); } }
            }
            [ElementPriority(3)]
            [TGIBlockListContentField("References")]
            public UInt32 SpecularKeyIndex
            {
                get { return mSpecularKeyIndex; }
                set { if (mSpecularKeyIndex != value) { mSpecularKeyIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(4)]
            [TGIBlockListContentField("References")]
            public UInt32 DetailDarkKeyIndex
            {
                get { return mDetailDarkKeyIndex; }
                set { if (mDetailDarkKeyIndex != value) { mDetailDarkKeyIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(5)]
            [TGIBlockListContentField("References")]
            public UInt32 DetailLightKeyIndex
            {
                get { return mDetailLightKeyIndex; }
                set { if (mDetailLightKeyIndex != value) { mDetailLightKeyIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(6)]
            [TGIBlockListContentField("References")]
            public UInt32 NormalMapKeyIndex
            {
                get { return mNormalMapKeyIndex; }
                set { if (mNormalMapKeyIndex != value) { mNormalMapKeyIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(7)]
            [TGIBlockListContentField("References")]
            public UInt32 OverlayKeyIndex
            {
                get { return mOverlayKeyIndex; }
                set { if (mOverlayKeyIndex != value) { mOverlayKeyIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(8)]
            [TGIBlockListContentField("References")]
            public UInt32 MuscleNormalMapKeyIndex
            {
                get { return mMuscleNormalMapKeyIndex; }
                set { if (mMuscleNormalMapKeyIndex != value) { mMuscleNormalMapKeyIndex = value; OnElementChanged(); } }
            }
            [ElementPriority(9)]
            [TGIBlockListContentField("References")]
            public UInt32 CleavageNormalMapKeyIndex
            {
                get { return mCleavageNormalMapKeyIndex; }
                set { if (mCleavageNormalMapKeyIndex != value) { mCleavageNormalMapKeyIndex = value; OnElementChanged(); } }
            }
            public TGIBlockList References
            {
                get { return mOwner.References; }
                set { mOwner.References = value; }
            }
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
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new TextureKey(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get
                {
                    var fields =GetContentFields(base.requestedApiVersion, base.GetType());
                    if(mOwner.Version<6)
                    {
                        fields.Remove("MuscleNormalMapKeyIndex");
                        fields.Remove("CleavageNormalMapKeyIndex");
                    }
                    return fields;
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
                if (mOwner.Version >= 6)
                {
                    mMuscleNormalMapKeyIndex = br.ReadUInt32();
                    mCleavageNormalMapKeyIndex = br.ReadUInt32();
                }
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
                if (mOwner.Version >= 6)
                {
                    bw.Write(mMuscleNormalMapKeyIndex);
                    bw.Write(mCleavageNormalMapKeyIndex);
                }
            }
        }


        public class ShaderKeyList : DependentList<ShaderKey>
        {
            public ShaderKeyList(EventHandler handler) : base(handler) { }
            public ShaderKeyList(EventHandler handler, Stream s) : base(handler, s) { }
            public ShaderKeyList(EventHandler handler, IList<ShaderKey> ilt) : base(handler, ilt) { }

            public override void Add()
            {
                this.Add(new object[] { });
            }

            protected override ShaderKey CreateElement(Stream s)
            {
                return new ShaderKey(0, handler, s);
            }

            protected override void WriteElement(Stream s, ShaderKey element)
            {
                element.UnParse(s);
            }
        }


        public class TextureKeyList : DependentList<TextureKey>
        {
            private SkinToneResource mOwner;
            public TextureKeyList(EventHandler handler, SkinToneResource owner) : base(handler) { mOwner = owner; }
            public TextureKeyList(EventHandler handler, Stream s, SkinToneResource owner) : this(handler, owner) { Parse(s); }
            public TextureKeyList(EventHandler handler, IList<TextureKey> ilt, SkinToneResource owner) : base(handler, ilt) { mOwner = owner; }

            public override void Add()
            {
                this.Add(new object[] { mOwner });
            }

            protected override TextureKey CreateElement(Stream s)
            {
                return new TextureKey(0, handler, s, mOwner);
            }

            protected override void WriteElement(Stream s, TextureKey element)
            {
                element.UnParse(s);
            }
        }


        private UInt32 mVersion;
        private ShaderKeyList mShaderKeyList;
        private UInt32 mSkinSubSRampIndex;
        private UInt32 mToneRampIndex;
        private TextureKeyList mTextureKeyList;
        private Byte mIsDominant;
        private TGIBlockList mReferences;


        [ElementPriority(1)]
        public UInt32 Version
        {
            get { return mVersion; }
            set { if (mVersion != value) { mVersion = value; OnResourceChanged(this, new EventArgs()); } }
        }
        [ElementPriority(2)]
        public ShaderKeyList ShaderKeys
        {
            get { return mShaderKeyList; }
            set { if (mShaderKeyList != value) { mShaderKeyList = value; OnResourceChanged(this, new EventArgs()); } }
        }
        [ElementPriority(3)]
        [TGIBlockListContentField("References")]
        public UInt32 SkinSubSRampIndex
        {
            get { return mSkinSubSRampIndex; }
            set { if (mSkinSubSRampIndex != value) { mSkinSubSRampIndex = value; OnResourceChanged(this, new EventArgs()); } }
        }
        [ElementPriority(4)]
        [TGIBlockListContentField("References")]
        public UInt32 ToneRampIndex
        {
            get { return mToneRampIndex; }
            set { if (mToneRampIndex != value) { mToneRampIndex = value; OnResourceChanged(this, new EventArgs()); } }
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
            set { if (mIsDominant != value) { mIsDominant = value; OnResourceChanged(this, new EventArgs()); } }
        }
        [ElementPriority(7)]
        public TGIBlockList References
        {
            get { return mReferences; }
            set { if (mReferences != value) { mReferences = value; OnResourceChanged(this, new EventArgs()); } }
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
                sb.AppendFormat("SkinRampIndex1: 0X{0:x8}\n", mSkinSubSRampIndex);
                sb.AppendFormat("SkinRampIndex2: 0X{0:x8}\n", mToneRampIndex);

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



        public SkinToneResource(int apiVersion, Stream s)
            : base(apiVersion, s)
        {
            if (base.stream == null)
            {
                base.stream = this.UnParse();
                base.OnResourceChanged(this,new EventArgs());
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
            mSkinSubSRampIndex = br.ReadUInt32();
            mToneRampIndex = br.ReadUInt32();
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
            bw.Write(mSkinSubSRampIndex);
            bw.Write(mToneRampIndex);
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

