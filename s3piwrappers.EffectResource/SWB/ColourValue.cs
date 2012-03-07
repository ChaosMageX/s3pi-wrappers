using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB.Structures;

namespace s3piwrappers.SWB
{
    public class ColourValue : ValueElement<Colour>, IEquatable<ColourValue>
    {
        public ColourValue(int apiVersion, EventHandler handler) : base(apiVersion, handler)
        {
        }

        public ColourValue(int apiVersion, EventHandler handler, ColourValue basis) : base(apiVersion, handler, basis)
        {
        }

        public ColourValue(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s)
        {
        }

        protected override void Parse(Stream s)
        {
            var sw = new BinaryStreamWrapper(s, ByteOrder.LittleEndian);
            sw.Read(out mData.R);
            sw.Read(out mData.G);
            sw.Read(out mData.B);
        }

        public override void UnParse(Stream s)
        {
            var sw = new BinaryStreamWrapper(s, ByteOrder.LittleEndian);
            sw.Write(mData.R);
            sw.Write(mData.G);
            sw.Write(mData.B);
        }

        [ElementPriority(1)]
        public float R
        {
            get { return mData.R; }
            set
            {
                mData.R = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public float G
        {
            get { return mData.G; }
            set
            {
                mData.G = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public float B
        {
            get { return mData.B; }
            set
            {
                mData.B = value;
                OnElementChanged();
            }
        }

        public bool Equals(ColourValue other)
        {
            return mData.Equals(other.mData);
        }
    }
}
