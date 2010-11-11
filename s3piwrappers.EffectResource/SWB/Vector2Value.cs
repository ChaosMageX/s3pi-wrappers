using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB.IO;
using s3piwrappers.SWB.Structures;

namespace s3piwrappers.SWB
{
    public class Vector2Value : ValueElement<Vector2>, IEquatable<Vector2Value>
    {
     
        public Vector2Value(int apiVersion, EventHandler handler, Vector2Value basis): base(apiVersion, handler, basis) { }
        public Vector2Value(int apiVersion, EventHandler handler): base(apiVersion, handler) { }
        public Vector2Value(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler, s) { }

        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mData.X);
            s.Read(out mData.Y);
        }
        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mData.X);
            s.Write(mData.Y);
        }

        [ElementPriority(1)]
        public float X
        {
            get { return mData.X; }
            set { mData.X = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public float Y
        {
            get { return mData.Y; }
            set { mData.Y = value; OnElementChanged(); }
        }
        public bool Equals(Vector2Value other)
        {
            return mData.Equals(other.mData);
        }
    }
}