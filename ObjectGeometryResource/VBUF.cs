using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using s3pi.Interfaces;
using s3pi.Settings;
namespace s3piwrappers
{
    public class VBUF :ARCOLBlock
    {
        private UInt32 mVersion;
        private UInt32 mUnknown01;
        private UInt32 mVmapIndex;
        private Byte[] mBuffer;
        [ElementPriority(1)] 
        public UInt32 Version { get { return mVersion; } set { mVersion = value; OnRCOLChanged(this, new EventArgs()); } }
        [ElementPriority(2)]
        public UInt32 Unknown01 { get { return mUnknown01; } set { mUnknown01 = value; OnRCOLChanged(this, new EventArgs()); } }
        [ElementPriority(3)]
        public UInt32 VMAPIndex { get { return mVmapIndex; } set { mVmapIndex = value; OnRCOLChanged(this, new EventArgs()); } }
        [ElementPriority(4)]
        public Byte[] Buffer { get { return mBuffer; } set { mBuffer = value; OnRCOLChanged(this, new EventArgs()); } }
        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                sb.AppendFormat("VMAPIndex:\t0x{0:X8}\n", mVmapIndex);
                sb.AppendFormat("Buffer[{0}]\n", mBuffer.Length);
                return sb.ToString();
            }
        }
        public VBUF(int apiVersion, EventHandler handler,VBUF basis)
            : base(apiVersion, handler, null)
        {
            Stream s = basis.UnParse();
            s.Position = 0L;
            Parse(s);
        }
        public VBUF(int apiVersion, EventHandler handler)
            : base(apiVersion, handler, null)
        {
            mVersion = 0x00000101;
        }
        public VBUF(int apiVersion, EventHandler handler, Stream s)
            :base(apiVersion,handler,s)
        {

        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new VBUF(0, handler, this);
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
            mUnknown01 = br.ReadUInt32();
            mVmapIndex = br.ReadUInt32();
            mBuffer = new Byte[s.Length - s.Position];
            s.Read(mBuffer, 0, mBuffer.Length);
        }

        public override Stream UnParse()
        {
            MemoryStream s = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write((UInt32)FOURCC(Tag));
            bw.Write(mVersion);
            bw.Write(mUnknown01);
            bw.Write(mVmapIndex);
            if(mBuffer == null)mBuffer= new byte[0];
            bw.Write(mBuffer);
            return s;
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
