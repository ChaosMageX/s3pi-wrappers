using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;

namespace s3piwrappers.SWB
{
    public class UInt32Value : ValueElement<uint>, IEquatable<UInt32Value>
    {
        public UInt32Value(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
        public UInt32Value(int apiVersion, EventHandler handler, UInt32Value basis) : base(apiVersion, handler, basis) { }
        public UInt32Value(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s){}

        protected override void Parse(Stream s)
        {
            new BinaryStreamWrapper(s).Read(out mData, ByteOrder.BigEndian);
        }
        public override void UnParse(Stream s)
        {
            new BinaryStreamWrapper(s).Write(mData, ByteOrder.BigEndian);

        }

        [ElementPriority(1)]
        public UInt32 Data
        {
            get { return mData; }
            set { mData = value; OnElementChanged(); }
        }
        public bool Equals(UInt32Value other)
        {
            return mData == other.mData;
        }
    }
}