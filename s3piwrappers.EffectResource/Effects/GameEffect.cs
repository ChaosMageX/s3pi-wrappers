using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class GameEffect : Effect, IEquatable<GameEffect>
    {
        #region Constructors
        public GameEffect(int apiVersion, EventHandler handler, GameEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public GameEffect(int apiVersion, EventHandler handler, ISection section) 
            : base(apiVersion, handler, section)
        {
        }

        public GameEffect(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        #region Attributes
        private uint mFlags;
        private uint mMessageId;
        private uint mMessageData1;
        private uint mMessageData2;
        private uint mMessageData3;
        private uint mMessageData4;
        private string mMessageString = string.Empty;
        private float mLife;
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public uint Flags
        {
            get { return mFlags; }
            set
            {
                mFlags = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public uint MessageId
        {
            get { return mMessageId; }
            set
            {
                mMessageId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public uint MessageData1
        {
            get { return mMessageData1; }
            set
            {
                mMessageData1 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public uint MessageData2
        {
            get { return mMessageData2; }
            set
            {
                mMessageData2 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public uint MessageData3
        {
            get { return mMessageData3; }
            set
            {
                mMessageData3 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public uint MessageData4
        {
            get { return mMessageData4; }
            set
            {
                mMessageData4 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public string MessageString
        {
            get { return mMessageString; }
            set
            {
                mMessageString = value ?? string.Empty;
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public float Life
        {
            get { return mLife; }
            set
            {
                mLife = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            //mFlags &= 0x3FF;

            s.Read(out mMessageId);
            s.Read(out mMessageData1);
            s.Read(out mMessageData2);
            s.Read(out mMessageData3);
            s.Read(out mMessageData4);
            s.Read(out mMessageString, StringType.ZeroDelimited);
            s.Read(out mLife);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);

            s.Write(mMessageId);
            s.Write(mMessageData1);
            s.Write(mMessageData2);
            s.Write(mMessageData3);
            s.Write(mMessageData4);
            s.Write(mMessageString, StringType.ZeroDelimited);
            s.Write(mLife);
        }
        #endregion

        public bool Equals(GameEffect other)
        {
            return base.Equals(other);
        }
    }
}
