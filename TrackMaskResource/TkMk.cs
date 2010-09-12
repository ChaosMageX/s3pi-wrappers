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
        public class TrackMaskList : AResource.DependentList<TrackMask>
        {
            public TrackMaskList(EventHandler handler)
                : base(handler)
            {
            }

            public TrackMaskList(EventHandler handler, Stream s)
                : base(handler, s)
            {
            }

            public override void Add()
            {
                base.Add(new object[0]);
            }

            protected override TrackMask CreateElement(Stream s)
            {
                return new TrackMask(0, handler, s);
            }

            protected override void WriteElement(Stream s, TrackMask element)
            {
                element.UnParse(s);
            }
        }
        public class TrackMask : AHandlerElement, IEquatable<TrackMask>
        {
            private float mMaskValue;
            public TrackMask(int APIversion, EventHandler handler)
                : base(APIversion, handler)
            {
            }
            public TrackMask(int APIversion, EventHandler handler, Stream s)
                : base(APIversion, handler)
            {
                Parse(s);
            }
            public TrackMask(int APIversion, EventHandler handler, TrackMask basis)
                : base(APIversion, handler)
            {
                mMaskValue = basis.mMaskValue;
            }
            [ElementPriority(1)]
            public float MaskValue
            {
                get { return mMaskValue; }
                set { mMaskValue = value; OnElementChanged(); }
            }
            public override string ToString()
            {
                return mMaskValue.ToString();
            }
            private void Parse(Stream s)
            {
                mMaskValue = new BinaryReader(s).ReadSingle();
            }
            public void UnParse(Stream s)
            {
                new BinaryWriter(s).Write(mMaskValue);
            }
            public override AHandlerElement Clone(EventHandler handler)
            {
                return new TrackMask(0, handler, this);
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(requestedApiVersion, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(TrackMask other)
            {
                return mMaskValue.Equals(other.mMaskValue);
            }
        }

        public TkMk(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler, s)
        {
        }

        [ElementPriority(2)]
        public byte[] Unused
        {
            get { return mUnused; }
            set { mUnused = value; OnRCOLChanged(this,new EventArgs()); }
        }

        [ElementPriority(3)]
        public TrackMaskList TrackMasks
        {
            get { return mTrackMasks; }
            set { mTrackMasks = value; OnRCOLChanged(this, new EventArgs()); }
        }
        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set { mVersion = value; OnRCOLChanged(this, new EventArgs()); }
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            var s = UnParse();
            s.Position = 0L;
            return new TkMk(0, handler, s);
        }

        public override uint ResourceType
        {
            get { return 0x033260E3; }
        }

        public override string Tag
        {
            get { return "TkMk"; }
        }

        private UInt32 mVersion;
        private byte[] mUnused;
        private TrackMaskList mTrackMasks;
        public string Value
        {
            get
            {
                var sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\r\n", mVersion);
                sb.AppendLine("Track Mask Values:");
                for (int i = 0; i < mTrackMasks.Count; i++)
                {
                    sb.AppendFormat("Track[0x{0:X8}]:{1,8:0.00000}\r\n", i, mTrackMasks[i]);
                }
                return sb.ToString();

            }
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
            if(mUnused==null)mUnused= new byte[48];
            bw.Write(mUnused);
            if (mTrackMasks == null) mTrackMasks = new TrackMaskList(handler);
            mTrackMasks.UnParse(s);
            return s;
        }

        private static bool checking = Settings.Checking;
        private const int kRecommendedApiVersion = 1;
    }
}
