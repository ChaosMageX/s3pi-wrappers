using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using s3pi.Interfaces;
using s3pi.Settings;
namespace s3piwrappers
{
    public class VBUF2 : VBUF
    {
        public VBUF2(int apiVersion, EventHandler handler, VBUF basis)
            : base(apiVersion, handler, basis)
        {
        }

        public VBUF2(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
        }

        public VBUF2(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
        }

        public override uint ResourceType
        {
            get
            {
                return 0x0229684B;
            }
        }
    }
    public class VBUF : ARCOLBlock
    {
        [Flags]
        public enum VertexBufferFlags : uint
        {
            Collapsed = 0x4,
            DifferencedVertices = 0x2,
            Dynamic = 0x1,
            None = 0x0
        }



        private UInt32 mVersion = 0x00000101;
        private VertexBufferFlags mFlags;
        private UInt32 mSwizzleInfo;
        private Byte[] mBuffer;
        public VBUF(int apiVersion, EventHandler handler, VBUF basis): this(apiVersion, handler, basis.Version, basis.Flags, basis.SwizzleInfo, (Byte[])basis.Buffer.Clone()){}
        public VBUF(int apiVersion, EventHandler handler): base(apiVersion, handler, null){}
        public VBUF(int apiVersion, EventHandler handler, Stream s): base(apiVersion, handler, s){}
        public VBUF(int APIversion, EventHandler handler, uint version, VertexBufferFlags flags, uint swizzleInfo, byte[] buffer) : this(APIversion, handler)
        {
            mVersion = version;
            mFlags = flags;
            mSwizzleInfo = swizzleInfo;
            mBuffer = buffer;
        }

        [ElementPriority(1)]
        public UInt32 Version { get { return mVersion; } set { if(mVersion!=value){mVersion = value; OnRCOLChanged(this, new EventArgs());} } }
        [ElementPriority(2)]
        public VertexBufferFlags Flags { get { return mFlags; } set { if(mFlags!=value){mFlags = value; OnRCOLChanged(this, new EventArgs());} } }
        [ElementPriority(3)]
        public UInt32 SwizzleInfo { get { return mSwizzleInfo; } set { if(mSwizzleInfo!=value){mSwizzleInfo = value; OnRCOLChanged(this, new EventArgs());} } }
        [ElementPriority(4)]
        public Byte[] Buffer { get { return mBuffer; } set { if(mBuffer!=value){mBuffer = value; OnRCOLChanged(this, new EventArgs());} } }
        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Flags:\t{0}\n", this["Flags"]);
                sb.AppendFormat("Swizzle Info:\t0x{0:X8}\n", mSwizzleInfo);
                sb.AppendFormat("Buffer[{0}]\n", mBuffer.Length);
                return sb.ToString();
            }
        }

        protected override void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            string tag = FOURCC(br.ReadUInt32());
            if (checking && tag != Tag)
            {
                throw new InvalidDataException(string.Format("Invalid Tag read: '{0}'; expected: '{1}'; at 0x{1:X8}", tag, Tag, s.Position));
            }
            mVersion = br.ReadUInt32();
            mFlags = (VertexBufferFlags)br.ReadUInt32();
            mSwizzleInfo = br.ReadUInt32();
            mBuffer = new Byte[s.Length - s.Position];
            s.Read(mBuffer, 0, mBuffer.Length);
        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((UInt32)FOURCC(Tag));
            bw.Write(mVersion);
            bw.Write((UInt32)mFlags);
            bw.Write(mSwizzleInfo);
            if (mBuffer == null) mBuffer = new byte[0];
            bw.Write(mBuffer);
            return s;
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new VBUF(0, handler, this);
        }
        public override uint ResourceType
        {
            get { return 0x01D0E6FB; }
        }

        public override string Tag
        {
            get { return "VBUF"; }
        }
        static bool checking = Settings.Checking;
    }
}
