using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class SequenceEffect : Effect, IEquatable<SequenceEffect>
    {
        #region Constructors
        public SequenceEffect(int apiVersion, EventHandler handler, SequenceEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public SequenceEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mElements = new DataList<Element>(handler);
        }

        public SequenceEffect(int apiVersion, EventHandler handler, ISection section, Stream s) 
            : base(apiVersion, handler, section, s)
        {
        }
        #endregion

        public class Element : DataElement, IEquatable<Element>
        {
            #region Constructors
            public Element(int apiVersion, EventHandler handler) : base(apiVersion, handler)
            {
                mTimeRange = new Vector2ValueLE(apiVersion, handler, 1.0f, 1.0f);
            }

            public Element(int apiVersion, EventHandler handler, Element basis)
                : base(apiVersion, handler)
            {
                mTimeRange = new Vector2ValueLE(apiVersion, handler, basis.mTimeRange);
                mEffectName = basis.mEffectName;
            }
            
            public Element(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler)
            {
                Parse(s);
            }
            #endregion

            #region Attributes
            private Vector2ValueLE mTimeRange;
            private string mEffectName = string.Empty;
            #endregion

            #region Content Fields
            [ElementPriority(1)]
            public Vector2ValueLE TimeRange
            {
                get { return mTimeRange; }
                set
                {
                    mTimeRange = new Vector2ValueLE(requestedApiVersion, handler, value);
                    OnElementChanged();
                }
            }
            
            [ElementPriority(2)]
            public string EffectName
            {
                get { return mEffectName; }
                set
                {
                    mEffectName = value ?? string.Empty;
                    OnElementChanged();
                }
            }
            #endregion

            #region Data I/O
            protected override void Parse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mTimeRange = new Vector2ValueLE(requestedApiVersion, handler, stream);
                s.Read(out mEffectName, StringType.ZeroDelimited);
            }

            public override void UnParse(Stream stream)
            {
                var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                mTimeRange.UnParse(stream);
                s.Write(mEffectName, StringType.ZeroDelimited);
            }
            #endregion

            public bool Equals(Element other)
            {
                return base.Equals(other);
            }
        }

        #region Attributes
        private DataList<Element> mElements;
        private uint mFlags;
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public DataList<Element> Elements
        {
            get { return mElements; }
            set
            {
                mElements = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public uint Flags
        {
            get { return mFlags; }
            set
            {
                mFlags = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            mElements = new DataList<Element>(handler, stream);
            s.Read(out mFlags);
            //mFlags &= 0xF;
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            mElements.UnParse(stream);
            s.Write(mFlags);
        }
        #endregion

        public bool Equals(SequenceEffect other)
        {
            return base.Equals(other);
        }
    }
}
