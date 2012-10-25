using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using CASPartResource;
using s3pi.Interfaces;

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

            public ShaderKey(int APIversion, EventHandler handler) : base(APIversion, handler)
            {
            }

            public ShaderKey(int APIversion, EventHandler handler, ShaderKey basis) : this(APIversion, handler, basis.AgeGenderFlags, basis.EdgeColour, basis.SpecularColour, basis.SpecularPower, basis.IsGenetic)
            {
            }

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
                set
                {
                    if (mAgeGenderFlags != value)
                    {
                        mAgeGenderFlags = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(2)]
            public Color EdgeColour
            {
                get { return mEdgeColour; }
                set
                {
                    if (mEdgeColour != value)
                    {
                        mEdgeColour = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(3)]
            public Color SpecularColour
            {
                get { return mSpecularColour; }
                set
                {
                    if (mSpecularColour != value)
                    {
                        mSpecularColour = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(4)]
            public float SpecularPower
            {
                get { return mSpecularPower; }
                set
                {
                    if (mSpecularPower != value)
                    {
                        mSpecularPower = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(5)]
            public bool IsGenetic
            {
                get { return mIsGenetic; }
                set
                {
                    if (mIsGenetic != value)
                    {
                        mIsGenetic = value;
                        OnElementChanged();
                    }
                }
            }

            public string Value
            {
                get
                {
                    string str = "";
                    foreach (string field in ContentFields)
                    {
                        if (!field.Equals("Value"))
                        {
                            str = str + string.Format("{0}:\t{1}\n", field, this[field]);
                        }
                    }
                    return str.Trim();
                }
            }

            public ShaderKey(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler)
            {
                Parse(s);
            }

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
                var br = new BinaryReader(s);

                mAgeGenderFlags = new AgeGenderFlags(0, handler, s);
                mEdgeColour = Color.FromArgb(br.ReadInt32());
                mSpecularColour = Color.FromArgb(br.ReadInt32());
                mSpecularPower = br.ReadSingle();
                mIsGenetic = br.ReadBoolean();
            }

            public void UnParse(Stream s)
            {
                var bw = new BinaryWriter(s);
                mAgeGenderFlags.UnParse(s);
                bw.Write(mEdgeColour.ToArgb());
                bw.Write(mSpecularColour.ToArgb());
                bw.Write(mSpecularPower);
                bw.Write(mIsGenetic);
            }
        }

        public class TextureKey : AHandlerElement, IEquatable<TextureKey>
        {
            private readonly SkinToneResource mOwner;
            private AgeGenderFlags mAgeGenderFlags;
            private DataTypeFlags mTypeFlags;
            private TGIBlock mSpecularKey;
            private TGIBlock mDetailDarkKey;
            private TGIBlock mDetailLightKey;
            private TGIBlock mNormalMapKey;
            private TGIBlock mOverlayKey;
            private TGIBlock mMuscleNormalMapKey;
            private TGIBlock mCleavageNormalMapKey;


            public TextureKey(int APIversion, EventHandler handler, SkinToneResource owner) : this(APIversion, handler, owner, new AgeGenderFlags(0, handler), 0, new TGIBlock(0, handler), new TGIBlock(0, handler), new TGIBlock(0, handler), new TGIBlock(0, handler), new TGIBlock(0, handler), new TGIBlock(0, handler), new TGIBlock(0, handler))
            {
            }

            public TextureKey(int APIversion, EventHandler handler, TextureKey basis) : this(APIversion, handler, basis.mOwner, basis.AgeGenderFlags, basis.mTypeFlags, basis.SpecularKey, basis.DetailDarkKey, basis.DetailLightKey, basis.NormalMapKey, basis.OverlayKey, basis.MuscleNormalMapKey, basis.CleavageNormalMapKey)
            {
            }

            public TextureKey(int APIversion, EventHandler handler, Stream s, SkinToneResource owner, ResourceKeyTable keys) : base(APIversion, handler)
            {
                mOwner = owner;
                Parse(s, keys);
            }

            public TextureKey(int APIversion, EventHandler handler, SkinToneResource owner, AgeGenderFlags ageGenderFlags, DataTypeFlags typeFlags, TGIBlock specularKey, TGIBlock detailDarkKey, TGIBlock detailLightKey, TGIBlock normalMapKey, TGIBlock overlayKey, TGIBlock muscleNormalMapKey, TGIBlock cleavageNormalMapKey)
                : base(APIversion, handler)
            {
                mOwner = owner;
                mAgeGenderFlags = ageGenderFlags;
                mTypeFlags = typeFlags;
                mSpecularKey = specularKey;
                mDetailDarkKey = detailDarkKey;
                mDetailLightKey = detailLightKey;
                mNormalMapKey = normalMapKey;
                mOverlayKey = overlayKey;
                mMuscleNormalMapKey = muscleNormalMapKey;
                mCleavageNormalMapKey = cleavageNormalMapKey;
            }

            [ElementPriority(1)]
            public AgeGenderFlags AgeGenderFlags
            {
                get { return mAgeGenderFlags; }
                set
                {
                    if (mAgeGenderFlags != value)
                    {
                        mAgeGenderFlags = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(2)]
            public DataTypeFlags TypeFlags
            {
                get { return mTypeFlags; }
                set
                {
                    if (mTypeFlags != value)
                    {
                        mTypeFlags = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(3)]
            public TGIBlock SpecularKey
            {
                get { return mSpecularKey; }
                set
                {
                    if (mSpecularKey != value)
                    {
                        mSpecularKey = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(4)]
            public TGIBlock DetailDarkKey
            {
                get { return mDetailDarkKey; }
                set
                {
                    if (mDetailDarkKey != value)
                    {
                        mDetailDarkKey = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(5)]
            public TGIBlock DetailLightKey
            {
                get { return mDetailLightKey; }
                set
                {
                    if (mDetailLightKey != value)
                    {
                        mDetailLightKey = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(6)]
            public TGIBlock NormalMapKey
            {
                get { return mNormalMapKey; }
                set
                {
                    if (mNormalMapKey != value)
                    {
                        mNormalMapKey = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(7)]
            public TGIBlock OverlayKey
            {
                get { return mOverlayKey; }
                set
                {
                    if (mOverlayKey != value)
                    {
                        mOverlayKey = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(8)]
            public TGIBlock MuscleNormalMapKey
            {
                get { return mMuscleNormalMapKey; }
                set
                {
                    if (mMuscleNormalMapKey != value)
                    {
                        mMuscleNormalMapKey = value;
                        OnElementChanged();
                    }
                }
            }

            [ElementPriority(9)]
            public TGIBlock CleavageNormalMapKey
            {
                get { return mCleavageNormalMapKey; }
                set
                {
                    if (mCleavageNormalMapKey != value)
                    {
                        mCleavageNormalMapKey = value;
                        OnElementChanged();
                    }
                }
            }

            public string Value
            {
                get
                {
                    string str = "";
                    foreach (string field in ContentFields)
                    {
                        if (!field.Equals("Value"))
                        {
                            str = str + string.Format("{0}: {1}\n", field, this[field]);
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
                    List<string> fields = GetContentFields(base.requestedApiVersion, base.GetType());
                    if (mOwner.Version < 6)
                    {
                        fields.Remove("MuscleNormalMapKey");
                        fields.Remove("CleavageNormalMapKey");
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

            public void Parse(Stream s, ResourceKeyTable keys)
            {
                var br = new BinaryReader(s);
                mAgeGenderFlags = new AgeGenderFlags(0, handler, s);
                mTypeFlags = (DataTypeFlags) br.ReadUInt32();
                mSpecularKey = new TGIBlock(0, handler, keys[br.ReadInt32()]);
                mDetailDarkKey = new TGIBlock(0, handler, keys[br.ReadInt32()]);
                mDetailLightKey = new TGIBlock(0, handler, keys[br.ReadInt32()]);
                mNormalMapKey = new TGIBlock(0, handler, keys[br.ReadInt32()]);
                mOverlayKey = new TGIBlock(0, handler, keys[br.ReadInt32()]);
                if (mOwner.Version >= 6)
                {
                    mMuscleNormalMapKey = new TGIBlock(0, handler, keys[br.ReadInt32()]);
                    mCleavageNormalMapKey = new TGIBlock(0, handler, keys[br.ReadInt32()]);
                }
                else
                {
                    mMuscleNormalMapKey = new TGIBlock(0, handler);
                    mCleavageNormalMapKey = new TGIBlock(0, handler);
                }
            }

            public void UnParse(Stream s, ResourceKeyTable keys)
            {
                var bw = new BinaryWriter(s);
                mAgeGenderFlags.UnParse(s);
                bw.Write((UInt32) mTypeFlags);
                bw.Write(keys.Add(mSpecularKey));
                bw.Write(keys.Add(mDetailDarkKey));
                bw.Write(keys.Add(mDetailLightKey));
                bw.Write(keys.Add(mNormalMapKey));
                bw.Write(keys.Add(mOverlayKey));
                if (mOwner.Version >= 6)
                {
                    bw.Write(keys.Add(mMuscleNormalMapKey));
                    bw.Write(keys.Add(mCleavageNormalMapKey));
                }
            }
        }


        public class ShaderKeyList : DependentList<ShaderKey>
        {
            public ShaderKeyList(EventHandler handler) : base(handler)
            {
            }

            public ShaderKeyList(EventHandler handler, Stream s) : base(handler, s)
            {
            }

            public ShaderKeyList(EventHandler handler, IEnumerable<ShaderKey> ilt) : base(handler, ilt)
            {
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
            private readonly SkinToneResource mOwner;

            public TextureKeyList(EventHandler handler, SkinToneResource owner) : base(handler)
            {
                mOwner = owner;
            }

            public TextureKeyList(EventHandler handler, Stream s, SkinToneResource owner, ResourceKeyTable keys) : this(handler, owner)
            {
                Parse(s, keys);
            }

            public TextureKeyList(EventHandler handler, IEnumerable<TextureKey> ilt, SkinToneResource owner) : base(handler, ilt)
            {
                mOwner = owner;
            }

            protected void Parse(Stream s, ResourceKeyTable keys)
            {
                int c = ReadCount(s);
                for (int i = 0; i < c; i++)
                {
                    ((IList<TextureKey>) this).Add(new TextureKey(0, handler, s, mOwner, keys));
                }
            }

            public void UnParse(Stream s, ResourceKeyTable keys)
            {
                WriteCount(s, Count);
                for (int i = 0; i < Count; i++)
                {
                    this[i].UnParse(s, keys);
                }
            }

            protected override TextureKey CreateElement(Stream s)
            {
                throw new NotSupportedException();
            }

            protected override void WriteElement(Stream s, TextureKey element)
            {
                throw new NotSupportedException();
            }
        }


        private UInt32 mVersion;
        private ShaderKeyList mShaderKeyList;
        private TGIBlock mSkinSubSRampKey;
        private TGIBlock mToneRampKey;
        private TextureKeyList mTextureKeyList;
        private bool mIsDominant;

        [ElementPriority(1)]
        public UInt32 Version
        {
            get { return mVersion; }
            set
            {
                if (mVersion != value)
                {
                    mVersion = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }

        [ElementPriority(2)]
        public ShaderKeyList ShaderKeys
        {
            get { return mShaderKeyList; }
            set
            {
                if (mShaderKeyList != value)
                {
                    mShaderKeyList = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }

        [ElementPriority(3)]
        public TGIBlock SkinSubSRampKey
        {
            get { return mSkinSubSRampKey; }
            set
            {
                if (mSkinSubSRampKey != value)
                {
                    mSkinSubSRampKey = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }

        [ElementPriority(4)]
        public TGIBlock ToneRampKey
        {
            get { return mToneRampKey; }
            set
            {
                if (mToneRampKey != value)
                {
                    mToneRampKey = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
        }

        [ElementPriority(5)]
        public TextureKeyList TextureKeys
        {
            get { return mTextureKeyList; }
            set
            {
                mTextureKeyList = value;
                OnResourceChanged(this, new EventArgs());
            }
        }

        [ElementPriority(6)]
        public bool IsDominant
        {
            get { return mIsDominant; }
            set
            {
                if (mIsDominant != value)
                {
                    mIsDominant = value;
                    OnResourceChanged(this, new EventArgs());
                }
            }
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
                //sb.AppendFormat("SkinSubSRampIndex: {0}\n", mSkinSubSRampKey);
                //sb.AppendFormat("ToneRampIndex: {0}\n", mToneRampKey);

                //if (mTextureKeyList.Count > 0)
                //{
                //    sb.AppendFormat("TextureKeys:\n");
                //    for (int i = 0; i < mTextureKeyList.Count; i++)
                //    {
                //        sb.AppendFormat("==[{0}]==\n{1}\n", i, mTextureKeyList[i].Value);
                //    }
                //}
                //sb.AppendFormat("IsDominant: {0}\n", mIsDominant);

                //return sb.ToString();
            }
        }


        public SkinToneResource(int apiVersion, Stream s)
            : base(apiVersion, s)
        {
            if (base.stream == null)
            {
                base.stream = UnParse();
                base.OnResourceChanged(this, new EventArgs());
            }
            base.stream.Position = 0L;
            Parse(base.stream);
        }

        private void Parse(Stream s)
        {
            var br = new BinaryReader(s);
            mVersion = br.ReadUInt32();
            var keys = new ResourceKeyTable();
            ResourceKeyTable.TablePtr ptr = keys.BeginRead(s);
            mShaderKeyList = new ShaderKeyList(OnResourceChanged, s);
            mSkinSubSRampKey = new TGIBlock(0, OnResourceChanged, keys[br.ReadInt32()]);
            mToneRampKey = new TGIBlock(0, OnResourceChanged, keys[br.ReadInt32()]);
            mTextureKeyList = new TextureKeyList(OnResourceChanged, s, this, keys);
            mIsDominant = br.ReadByte() == 1;
            keys.EndRead(s, ptr);
        }

        protected override Stream UnParse()
        {
            Stream s = new MemoryStream();
            var bw = new BinaryWriter(s);
            bw.Write(mVersion);
            var keys = new ResourceKeyTable();
            long startPos = keys.BeginWrite(s);
            if (mShaderKeyList == null) mShaderKeyList = new ShaderKeyList(OnResourceChanged);
            mShaderKeyList.UnParse(s);
            if (mSkinSubSRampKey == null) mSkinSubSRampKey = new TGIBlock(0, OnResourceChanged);
            if (mToneRampKey == null) mToneRampKey = new TGIBlock(0, OnResourceChanged);
            bw.Write(keys.Add(mSkinSubSRampKey));
            bw.Write(keys.Add(mToneRampKey));
            if (mTextureKeyList == null) mTextureKeyList = new TextureKeyList(OnResourceChanged, this);
            mTextureKeyList.UnParse(s, keys);
            bw.Write(mIsDominant ? (byte) 1 : (byte) 0);
            keys.EndWrite(s, startPos);
            return s;
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}
