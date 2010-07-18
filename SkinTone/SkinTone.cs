using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace SkinToneResource
{
    [Flags]
    public enum AgeGenderFlags : uint
    {
        None = 0x00000000,
        Baby = 0x00000001,
        Toddler = 0x00000002,
        Child = 0x00000004,
        Teen = 0x00000008,
        YoungAdult = 0x00000010,
        Adult = 0x00000020,
        Elder = 0x00000040,
        Male = 0x00001000,
        Female = 0x00002000
    }
    [Flags]
    public enum PartType : uint
    {
        Hair = 0x00000001,
        Scalp = 0x00000002,
        FaceOverlay = 0x00000004,
        Body = 0x00000008,
        Accessory = 0x000000010
    }
    public class SkinToneResourceHandler : AResourceHandler
    {
        public SkinToneResourceHandler()
        {
            Add(typeof(SkinToneResource),new List<String>(new string[]{"0x0354796A"}));
        }
    }
    
    public class SkinToneResource : AResource
    {

        #region Nested Type: ShaderKey
        public class ShaderKey : AHandlerElement, IEquatable<ShaderKey>
        {
            #region Fields
            private UInt32 mUnknown1;
            private Color mColour1;
            private Color mColour2;
            private float mUnknown2;
            private bool mUnknown3;
            #endregion

            #region Properties
            [ElementPriority(1)]
            public AgeGenderFlags Flags
            {
                get { return (AgeGenderFlags)mUnknown1; }
                set { mUnknown1 = (UInt32)value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public Color EdgeColour
            {
                get { return mColour1; }
                set { mColour1 = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public Color SpecularColour
            {
                get { return mColour2; }
                set { mColour2 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public float SpecularPower
            {
                get { return mUnknown2; }
                set { mUnknown2 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public bool IsGenetic
            {
                get { return mUnknown3; }
                set { mUnknown3 = value; OnElementChanged(); }
            }
            #endregion

            #region AHandlerElement
            public ShaderKey(int APIversion, EventHandler handler, ShaderKey basis)
                : base(APIversion, handler)
            {
                mColour1 = basis.mColour1;
                mColour2 = basis.mColour2;
                mUnknown1 = basis.mUnknown1;
                mUnknown2 = basis.mUnknown2;
                mUnknown3 = basis.mUnknown3;

            }
            public ShaderKey(int APIversion, EventHandler handler) : base(APIversion, handler) { }
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
                mUnknown1 = br.ReadUInt32();
                mColour1 = Color.FromArgb(br.ReadInt32());
                mColour2 = Color.FromArgb(br.ReadInt32());
                mUnknown2 = br.ReadSingle();
                mUnknown3 = br.ReadBoolean();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mUnknown1);
                bw.Write(mColour1.ToArgb());
                bw.Write(mColour2.ToArgb());
                bw.Write(mUnknown2);
                bw.Write(mUnknown3);
            }

        } 
        #endregion

        #region Nested Type: TextureKey
        public class TextureKey : AHandlerElement, IEquatable<TextureKey>
        {
            #region Fields
            private UInt32 mAgeGenderFlags;
            private UInt32 mPartType;
            private UInt32 mIndex1;
            private UInt32 mIndex2;
            private UInt32 mIndex3;
            private UInt32 mIndex4;
            private UInt32 mIndex5;
            #endregion

            #region Properties
            [ElementPriority(1)]
            public AgeGenderFlags AgeGenderFlags
            {
                get { return (AgeGenderFlags)mAgeGenderFlags; }
                set { mAgeGenderFlags = (UInt32)value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public PartType TypeFlags
            {
                get { return (PartType)mPartType; }
                set { mPartType = (UInt32)value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public UInt32 SpecularMapIndex
            {
                get { return mIndex1; }
                set { mIndex1 = value; OnElementChanged(); }
            }
            [ElementPriority(4)]
            public UInt32 DetailDarkIndex
            {
                get { return mIndex2; }
                set { mIndex2 = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public UInt32 DetailLightIndex
            {
                get { return mIndex3; }
                set { mIndex3 = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public UInt32 NormalMapIndex
            {
                get { return mIndex4; }
                set { mIndex4 = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public UInt32 OverlayIndex
            {
                get { return mIndex5; }
                set { mIndex5 = value; OnElementChanged(); }
            }
            #endregion

            #region AHandlerElement

            public TextureKey(int APIversion, EventHandler handler,TextureKey basis) : base(APIversion, handler) 
            {
                mAgeGenderFlags = basis.mAgeGenderFlags;
                mPartType = basis.mPartType;
                mIndex1 = basis.mIndex1;
                mIndex2 = basis.mIndex2;
                mIndex3 = basis.mIndex3;
                mIndex4 = basis.mIndex4;
                mIndex5 = basis.mIndex5;
            }
            public TextureKey(int APIversion, EventHandler handler) : base(APIversion, handler) { }
            public TextureKey(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler) { Parse(s); }

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
                mAgeGenderFlags = br.ReadUInt32();
                mPartType = br.ReadUInt32();
                mIndex1 = br.ReadUInt32();
                mIndex2 = br.ReadUInt32();
                mIndex3 = br.ReadUInt32();
                mIndex4 = br.ReadUInt32();
                mIndex5 = br.ReadUInt32();
            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mAgeGenderFlags);
                bw.Write(mPartType);
                bw.Write(mIndex1);
                bw.Write(mIndex2);
                bw.Write(mIndex3);
                bw.Write(mIndex4);
                bw.Write(mIndex5);
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
                this.Add(new object[]{});
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
            public TextureKeyList(EventHandler handler) : base(handler) { }
            public TextureKeyList(EventHandler handler, Stream s) : base(handler, s) { }
            public override void Add()
            {
                base.Add(new object[]{});
            }

            protected override TextureKey CreateElement(Stream s)
            {
                TextureKey obj = new TextureKey(0, null);
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
        private ShaderKeyList mItemAList;
        private UInt32 mUnknown1;
        private UInt32 mUnknown2;
        private TextureKeyList mItemBList;
        private bool mUnknown3;
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
            get { return mItemAList; }
            set { mItemAList = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(3)]
        public UInt32 SubSkinRampIndex
        {
            get { return mUnknown1; }
            set { mUnknown1 = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(4)]
        public UInt32 SkinRampIndex
        {
            get { return mUnknown2; }
            set { mUnknown2 = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(5)]
        public TextureKeyList TextureKeys
        {
            get { return mItemBList; }
            set { mItemBList = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(6)]
        public bool IsGenetic
        {
            get { return mUnknown3; }
            set { mUnknown3 = value; OnResourceChanged(this, new EventArgs()); }
        }
        [ElementPriority(7)]
        public TGIBlockList References
        {
            get { return mReferences; }
            set { mReferences = value; }
        }

        #endregion

        
        public SkinToneResource(int apiVersion,Stream s) : base(apiVersion,s)
        {
            if (s == null)
            {
                stream = new MemoryStream();
                mItemAList = new ShaderKeyList(this.OnResourceChanged);
                mItemBList = new TextureKeyList(this.OnResourceChanged);
                mReferences = new TGIBlockList(this.OnResourceChanged);
                UnParse();
            }
            else
            {
                Parse(s);
            }
        }
        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            mVersion = br.ReadUInt32();
            long tgioffset = br.ReadUInt32() + s.Position;
            long tgisize = br.ReadUInt32();
            mItemAList = new ShaderKeyList(this.OnResourceChanged, s);
            mUnknown1 = br.ReadUInt32();
            mUnknown2 = br.ReadUInt32();
            mItemBList = new TextureKeyList(this.OnResourceChanged, s);
            mUnknown3 = br.ReadBoolean();
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
            mItemAList.UnParse(s);
            bw.Write(mUnknown1);
            bw.Write(mUnknown2);
            mItemBList.UnParse(s);
            bw.Write(mUnknown3);
            mReferences.UnParse(s, pos);
            return s;
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}

