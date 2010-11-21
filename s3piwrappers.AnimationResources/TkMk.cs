using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using System.IO;
using s3pi.Settings;
namespace s3piwrappers
{
    public class TkMk : ARCOLBlock
    {
        private UInt32 mVersion;
        private byte[] mUnused;
        private TrackMaskList mTrackMasks;

        public class TrackMaskList : SimpleList<Single>
        {
            public TrackMaskList(EventHandler handler) : base(handler, ReadElement, WriteElement) { }
            public TrackMaskList(EventHandler handler, IList<HandlerElement<Single>> ilt) : base(handler, ilt, ReadElement, WriteElement) { }
            public TrackMaskList(EventHandler handler, Stream s) : base(handler, s, ReadElement, WriteElement) { }
            static Single ReadElement(Stream s) { return new BinaryReader(s).ReadSingle(); }
            static void WriteElement(Stream s, Single element) { new BinaryWriter(s).Write(element); }
        }
        public TkMk(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }
        public TkMk(int APIversion, EventHandler handler, TkMk basis): this(APIversion, handler, basis.Version,basis.Unused,basis.TrackMasks){}
        public TkMk(int APIversion, EventHandler handler) : this(APIversion, handler, 0x00000200, null,null) { }
        public TkMk(int APIversion, EventHandler handler, uint version, byte[] unused, TrackMaskList trackMasks) : base(APIversion, handler, null)
        {
            mVersion = version;
            mUnused = unused??new byte[48];
            mTrackMasks = trackMasks??new TrackMaskList(handler);
        }

        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { if (mVersion != value) { mVersion = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(2)]
        public byte[] Unused
        {
            get { return mUnused; }
            set { if (mUnused != value) { mUnused = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        [ElementPriority(3)]
        public TrackMaskList TrackMasks
        {
            get { return mTrackMasks; }
            set { if (mTrackMasks != value) { mTrackMasks = value; OnRCOLChanged(this, new EventArgs()); } }
        }
        protected override void Parse(Stream s)
        {
            var br = new BinaryReader(s);
            if (FOURCC(br.ReadUInt32()) != Tag)
                throw new InvalidDataException("Invalid Tag, Expected \"TkMk\"");
            mVersion = br.ReadUInt32();
            mUnused = br.ReadBytes(48);
            mTrackMasks = new TrackMaskList(handler, s);
            if (s.Position != s.Length)
                throw new InvalidDataException("Unexpected End of File");
        }

        public override Stream UnParse()
        {
            var s = new MemoryStream();
            var bw = new BinaryWriter(s);
            bw.Write((uint)FOURCC(Tag));
            bw.Write(mVersion);
            if (mUnused == null) mUnused = new byte[48];
            bw.Write(mUnused);
            if (mTrackMasks == null) mTrackMasks = new TrackMaskList(handler);
            mTrackMasks.UnParse(s);
            return s;
        }

        public string Value
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\r\n", mVersion);
                if (mTrackMasks.Count > 0)
                {
                    sb.AppendLine("Values:");
                    for (int i = 0; i < mTrackMasks.Count; i++)
                    {
                        sb.AppendFormat("[{0}]{1}\n", i, mTrackMasks[i]);
                    }
                }
                return sb.ToString();

            }
        }
        public override AHandlerElement Clone(EventHandler handler)
        {
            return new TkMk(0, handler, this);
        }

        public override uint ResourceType
        {
            get { return 0x033260E3; }
        }

        public override string Tag
        {
            get { return "TkMk"; }
        }
        private static bool checking = Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}
