using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class TrackMask : ARCOLBlock
    {
        public const UInt32 kDefaultVersion = 0x00000200U;
        private TrackMaskList mTrackMasks;
        private byte[] mUnused;
        private UInt32 mVersion;

        public TrackMask(int APIversion, EventHandler handler, Stream s) : base(APIversion, handler, s) { }

        public TrackMask(int APIversion, EventHandler handler, TrackMask basis) : this(APIversion, handler, basis.Version, basis.Unused, basis.TrackMasks) { }

        public TrackMask(int APIversion, EventHandler handler) : this(APIversion, handler, kDefaultVersion, null, null) { }

        public TrackMask(int APIversion, EventHandler handler, uint version, byte[] unused, TrackMaskList trackMasks) : base(APIversion, handler, null)
        {
            mVersion = version;
            mUnused = unused ?? new byte[48];
            mTrackMasks = trackMasks ?? new TrackMaskList(handler);
        }

        [ElementPriority(1)]
        public uint Version
        {
            get { return mVersion; }
            set
            {
                if (mVersion != value)
                {
                    mVersion = value;
                    OnRCOLChanged(this, new EventArgs());
                }
            }
        }

        [ElementPriority(2)]
        public byte[] Unused
        {
            get { return mUnused; }
            set
            {
                if (mUnused != value)
                {
                    mUnused = value;
                    OnRCOLChanged(this, new EventArgs());
                }
            }
        }

        [ElementPriority(3)]
        public TrackMaskList TrackMasks
        {
            get { return mTrackMasks; }
            set
            {
                if (mTrackMasks != value)
                {
                    mTrackMasks = value;
                    OnRCOLChanged(this, new EventArgs());
                }
            }
        }

        public string Value { get { return ValueBuilder; } }

        public override uint ResourceType { get { return 0x033260E3; } }

        public override string Tag { get { return "TkMk"; } }

        protected override void Parse(Stream s)
        {
            var br = new BinaryReader(s);
            if (FOURCC(br.ReadUInt32()) != Tag)
                throw new InvalidDataException("Invalid Tag, Expected "+Tag);
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
            bw.Write((uint) FOURCC(Tag));
            bw.Write(mVersion);
            if (mUnused == null) mUnused = new byte[48];
            bw.Write(mUnused);
            if (mTrackMasks == null) mTrackMasks = new TrackMaskList(handler);
            mTrackMasks.UnParse(s);
            return s;
        }

        public override AHandlerElement Clone(EventHandler handler) { return new TrackMask(0, handler, this); }

        #region Nested type: TrackMaskList

        public class TrackMaskList : SimpleList<Single>
        {
            public TrackMaskList(EventHandler handler) : base(handler, ReadElement, WriteElement) { }

            public TrackMaskList(EventHandler handler, IEnumerable<float> ilt) : base(handler, ilt, ReadElement, WriteElement) { }

            public TrackMaskList(EventHandler handler, Stream s) : base(handler, s, ReadElement, WriteElement) { }
            private static Single ReadElement(Stream s) { return new BinaryReader(s).ReadSingle(); }
            private static void WriteElement(Stream s, Single element) { new BinaryWriter(s).Write(element); }
        }

        #endregion
    }
}