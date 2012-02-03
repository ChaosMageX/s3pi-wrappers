using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;

namespace s3piwrappers.SWB
{
    public class ByteValue : ValueElement<byte>, IEquatable<ByteValue>
    {
        public ByteValue(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
        public ByteValue(int apiVersion, EventHandler handler, ByteValue basis) : base(apiVersion, handler, basis) { }
        public ByteValue(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler,s){}

        protected override void Parse(Stream s)
        {
            new BinaryStreamWrapper(s, ByteOrder.BigEndian).Read(out mData);
        }
        public override void UnParse(Stream s)
        {
            new BinaryStreamWrapper(s, ByteOrder.BigEndian).Write(mData);

        }

        [ElementPriority(1)]
        public Byte Data
        {
            get { return mData; }
            set { mData = value; OnElementChanged(); }
        }
        public bool Equals(ByteValue other)
        {
            return mData == other.mData;
        }
    }
}