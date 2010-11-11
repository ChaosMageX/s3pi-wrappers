using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using System.IO;
using s3pi.Settings;
namespace s3piwrappers
{
    public class IBUF2 : IBUF
    {
        public IBUF2(int apiVersion, EventHandler handler, IBUF basis)
            : base(apiVersion, handler, basis)
        {
        }

        public IBUF2(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
        }

        public IBUF2(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
        }

        public override uint ResourceType
        {
            get { return 0x0229684F; }
        }
    }
    public class IBUF : ARCOLBlock
    {
        [Flags]
        public enum IndexBufferFlags : uint
        {
            DifferencedIndices = 0x1,
            Uses32BitIndices = 0x2,
            IsDisplayList = 0x4
        }

        private UInt32 mVersion;
        private IndexBufferFlags mFlags;
        private UInt32 mDisplayListUsage;
        private Byte[] mBuffer;
        public IBUF(int apiVersion, EventHandler handler) : base(apiVersion, handler, null) { }
        public IBUF(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }
        public IBUF(int apiVersion, EventHandler handler, IBUF basis) : this(apiVersion, handler, basis.Version, basis.Flags, basis.DisplayListUsage, basis.Buffer) { }
        public IBUF(int APIversion, EventHandler handler, uint version, IndexBufferFlags flags, uint displayListUsage, byte[] buffer)
            : base(APIversion, handler, null)
        {
            mVersion = version;
            mFlags = flags;
            mDisplayListUsage = displayListUsage;
            mBuffer = buffer;
        }

        [ElementPriority(1)]
        public UInt32 Version { get { return mVersion; } set { if (mVersion != value) { mVersion = value; OnRCOLChanged(this, new EventArgs()); } } }
        [ElementPriority(2)]
        public IndexBufferFlags Flags { get { return mFlags; } set { if (mFlags != value) { mFlags = value; OnRCOLChanged(this, new EventArgs()); } } }
        [ElementPriority(3)]
        public UInt32 DisplayListUsage { get { return mDisplayListUsage; } set { if (mDisplayListUsage != value) { mDisplayListUsage = value; OnRCOLChanged(this, new EventArgs()); } } }
        [ElementPriority(4)]
        public Byte[] Buffer { get { return mBuffer; } set { if (mBuffer != value) { mBuffer = value; OnRCOLChanged(this, new EventArgs()); } } }

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Flags:\t{0}\n", this["Flags"]);
                sb.AppendFormat("DisplayListUsage:\t0x{0:X8}\n", mDisplayListUsage);
                sb.AppendFormat("Buffer[{0}]\n", mBuffer.Length);
                return sb.ToString();
            }
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new IBUF(0, handler, this);
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
            mFlags = (IndexBufferFlags)br.ReadUInt32();
            mDisplayListUsage = br.ReadUInt32();
            mBuffer = br.ReadBytes((int)(s.Length - s.Position));

        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            if (mBuffer == null) mBuffer = new byte[0];
            bw.Write((UInt32)FOURCC(Tag));
            bw.Write(mVersion);
            bw.Write((UInt32)mFlags);
            bw.Write(mDisplayListUsage);
            bw.Write(mBuffer);
            return s;
        }

        public override uint ResourceType
        {
            get { return 0x01D0E70F; }
        }

        public override string Tag
        {
            get { return "IBUF"; }
        }
        static bool checking = Settings.Checking;
    }
}
