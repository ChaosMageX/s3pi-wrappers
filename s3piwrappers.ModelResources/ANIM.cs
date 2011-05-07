using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.Settings;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class ANIM : ARCOLBlock
    {
        public class TextureFrameComparer : IComparer<TextureFrame>
        {
            public int Compare(TextureFrame x, TextureFrame y) { return x.Ordinal.CompareTo(y.Ordinal); }
        }
        public class TextureFrame : AHandlerElement, IEquatable<TextureFrame>, IResource
        {
            public TextureFrame(int APIversion, EventHandler handler) : this(APIversion, handler,new byte[0],0){}
            public TextureFrame(int APIversion, EventHandler handler, TextureFrame basis) : this(APIversion, handler, basis.mData,basis.Ordinal) { }
            public TextureFrame(int APIversion, EventHandler handler, Byte[] data,Int32 ordinal)
                : base(APIversion, handler)
            {
                mData = data;
                mOrdinal = ordinal;
            }

            private Byte[] mData;
            private Int32 mOrdinal;

            [ElementPriority(1)]
            public virtual BinaryReader DDSTexture
            {
                get
                {
                    return new BinaryReader(new MemoryStream(mData));
                }
                set
                {
                    mData = new byte[value.BaseStream.Length];
                    value.BaseStream.Position = 0L;
                    value.BaseStream.Read(mData, 0, (int)value.BaseStream.Length);
                    OnElementChanged();
                }
            }
            public Int32 Ordinal
            {
                get { return mOrdinal; }
                set { if(mOrdinal!=value){mOrdinal = value; OnElementChanged();} }
            }
            public Byte[] Data
            {
                get { return mData; }
                set { if (mData != value) { mData = value; OnElementChanged(); } }
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new TextureFrame(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0,GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public byte[] AsBytes
            {
                get { return mData; }
            }

            public event EventHandler ResourceChanged;
            protected override void OnElementChanged()
            {
                base.OnElementChanged();
                if (ResourceChanged != null) ResourceChanged(this, new EventArgs());
            }
            public Stream Stream
            {
                get { return new MemoryStream(mData); }
            }

            public bool Equals(TextureFrame other)
            {
                return base.Equals(other);
            }
        }
        public class TextureList : DependentList<TextureFrame>
        {
            public TextureList(EventHandler handler) : base(handler) { }
            public TextureList(EventHandler handler, IEnumerable<TextureFrame> ilt) : base(handler, ilt) { }
            public TextureList(EventHandler handler, Stream s) : base(handler, s) { }
            protected override void Parse(Stream s)
            {
                var br = new BinaryReader(s);
                var offsets = new UInt32[br.ReadUInt32()];
                for (int i = 0; i < offsets.Length; i++)
                {
                    offsets[i] = (uint)(br.ReadUInt32() + s.Position);
                }
                int j = 0;
                while (j < offsets.Length)
                {
                    if (checking && s.Position != offsets[j]) throw new InvalidDataException("Bad offset");
                    var len = (++j < offsets.Length ? offsets[j] : (uint)s.Length) - s.Position;
                    var buffer = new byte[len];
                    s.Read(buffer, 0, buffer.Length);
                    ((IList<TextureFrame>)this).Add(new TextureFrame(0, elementHandler, buffer,j));
                }

                this.Sort(new TextureFrameComparer());
            }
            public override void UnParse(Stream s)
            {
                this.Sort(new TextureFrameComparer());
                var bw = new BinaryWriter(s);
                bw.Write(Count);
                var start = s.Position;
                uint[] offsets = new uint[Count];
                for (int i = 0; i < offsets.Length; i++) { bw.Write(offsets[i]); }
                for (int i = 0; i < Count; i++)
                {
                    offsets[i] = (uint)s.Position;
                    bw.Write(this[i].AsBytes);
                }
                var end = s.Position;
                s.Seek(start, SeekOrigin.Begin);
                for (int i = 0; i < offsets.Length; i++) { bw.Write((uint)(offsets[i] - (s.Position + 4))); }
                s.Seek(end, SeekOrigin.Begin);
            }
            public override void Add()
            {
                base.Add(new object[] {new byte[0],Count+1 });
            }

            protected override TextureFrame CreateElement(Stream s)
            {
                throw new NotImplementedException();
            }

            protected override void WriteElement(Stream s, TextureFrame element)
            {
                throw new NotImplementedException();
            }
        }
        private const int kRecommendedApiVersion = 1;
        private static bool checking = Settings.Checking;

        public ANIM(int APIversion, EventHandler handler, ANIM basis) : this(APIversion, handler, basis.mVersion, basis.mFramerate, basis.mTextures) { }
        public ANIM(int APIversion, EventHandler handler) : base(APIversion, handler, null) { }
        public ANIM(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        public ANIM(int APIversion, EventHandler handler, uint version, float framerate, IList<TextureFrame> textures)
            : this(APIversion, handler)
        {
            mVersion = version;
            mFramerate = framerate;
            mTextures = new TextureList(handler, textures);
        }
        private UInt32 mVersion;
        private Single mFramerate;
        private TextureList mTextures;

        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { if(mVersion!=value){mVersion = value; OnRCOLChanged(this, new EventArgs());} }
        }
        [ElementPriority(2)]
        public float Framerate
        {
            get { return mFramerate; }
            set { if(mFramerate!=value){mFramerate = value; OnRCOLChanged(this, new EventArgs());} }
        }
        [ElementPriority(3)]
        public TextureList Textures
        {
            get { return mTextures; }
            set { if(mTextures!=value){mTextures = value; OnRCOLChanged(this, new EventArgs());} }
        }

        protected override void Parse(Stream s)
        {
            var br = new BinaryReader(s);
            var tag = FOURCC((ulong)br.ReadUInt32());
            if (checking && !tag.Equals(Tag)) throw new InvalidDataException("Bad tag: expected " + Tag + ", but got " + tag);
            mVersion = br.ReadUInt32();
            mFramerate = br.ReadSingle();
            mTextures = new TextureList(handler, s);

        }


        public override Stream UnParse()
        {
            var s = new MemoryStream();
            var bw = new BinaryWriter(s);
            bw.Write((uint)FOURCC(Tag));
            bw.Write(mVersion);
            bw.Write(mFramerate);
            if (mTextures == null) mTextures = new TextureList(handler);
            mTextures.UnParse(s);
            return s;
        }
        public virtual string Value
        {
            get 
            {
                return ValueBuilder;
                /*
                var sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\r\n", mVersion);
                sb.AppendFormat("Framerate:\t{0}\r\n", mFramerate);
                sb.AppendFormat("Textures[{0}]\r\n", mTextures.Count);
                return sb.ToString();
                /**/
            }
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new ANIM(0, handler, this);
        }
        public override uint ResourceType
        {
            get { return 0x63A33EA7; }
        }

        public override string Tag
        {
            get { return "ANIM"; }
        }
    }
}
