using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;

namespace s3piwrappers.SWB
{
    public class FloatValue : ValueElement<float>, IEquatable<FloatValue>
    {
        public FloatValue(int apiVersion, EventHandler handler) : base(apiVersion, handler)
        {
        }

        public FloatValue(int apiVersion, EventHandler handler, FloatValue basis) : base(apiVersion, handler, basis)
        {
        }

        public FloatValue(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s)
        {
        }

        protected override void Parse(Stream s)
        {
            new BinaryStreamWrapper(s).Read(out mData, ByteOrder.BigEndian);
        }

        public override void UnParse(Stream s)
        {
            new BinaryStreamWrapper(s).Write(mData, ByteOrder.BigEndian);
        }


        [ElementPriority(1)]
        public float Data
        {
            get { return mData; }
            set
            {
                mData = value;
                OnElementChanged();
            }
        }

        public bool Equals(FloatValue other)
        {
            return mData == other.mData;
        }
    }
}
