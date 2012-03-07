using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Effects
{
    public class SequenceEffect : Effect, IEquatable<SequenceEffect>
    {
        public SequenceEffect(int apiVersion, EventHandler handler, SequenceEffect basis)
            : base(apiVersion, handler, basis)
        {
        }

        public SequenceEffect(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mElements = new DataList<Element>(handler);
        }

        public SequenceEffect(int apiVersion, EventHandler handler, ISection section, Stream s) : base(apiVersion, handler, section, s)
        {
        }


        public class Element : DataElement, IEquatable<Element>
        {
            public Element(int apiVersion, EventHandler handler) : base(apiVersion, handler)
            {
            }

            public Element(int apiVersion, EventHandler handler, Element basis)
                : base(apiVersion, handler)
            {
                mFloat01 = basis.Float01;
                mFloat02 = basis.Float02;
                mString01 = basis.mString01;
            }

            public Element(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler)
            {
                Parse(s);
            }

            private float mFloat01 = 1.0f;
            private float mFloat02 = 1.0f;
            private string mString01 = string.Empty;

            [ElementPriority(1)]
            public float Float01
            {
                get { return mFloat01; }
                set
                {
                    mFloat01 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(2)]
            public float Float02
            {
                get { return mFloat02; }
                set
                {
                    mFloat02 = value;
                    OnElementChanged();
                }
            }

            [ElementPriority(3)]
            public string EffectName
            {
                get { return mString01; }
                set
                {
                    mString01 = value;
                    OnElementChanged();
                }
            }

            protected override void Parse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Read(out mFloat01, ByteOrder.LittleEndian);
                s.Read(out mFloat02, ByteOrder.LittleEndian);
                s.Read(out mString01, StringType.ZeroDelimited);
            }


            public override void UnParse(Stream stream)
            {
                BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
                s.Write(mFloat01, ByteOrder.LittleEndian);
                s.Write(mFloat02, ByteOrder.LittleEndian);
                s.Write(mString01, StringType.ZeroDelimited);
            }

            public bool Equals(Element other)
            {
                return base.Equals(other);
            }
        }


        private DataList<Element> mElements;
        private UInt32 mInt01;


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
        public uint Int01
        {
            get { return mInt01; }
            set
            {
                mInt01 = value;
                OnElementChanged();
            }
        }


        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            mElements = new DataList<Element>(handler, stream);
            s.Read(out mInt01);
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            mElements.UnParse(stream);
            s.Write(mInt01);
        }


        public bool Equals(SequenceEffect other)
        {
            return base.Equals(other);
        }
    }
}
