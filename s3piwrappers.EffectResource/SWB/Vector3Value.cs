using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB.Structures;

namespace s3piwrappers.SWB
{
    public class Vector3Value : ValueElement<Vector3>, IEquatable<Vector3Value>
    {

        public Vector3Value(int apiVersion, EventHandler handler, Vector3Value basis): base(apiVersion, handler, basis) { }
        public Vector3Value(int apiVersion, EventHandler handler): base(apiVersion, handler) { }
        public Vector3Value(int apiVersion, EventHandler handler, Stream s): base(apiVersion, handler,s){}
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mData.X);
            s.Read(out mData.Y);
            s.Read(out mData.Z);
        }
        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mData.X);
            s.Write(mData.Y);
            s.Write(mData.Z);
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
        [ElementPriority(3)]
        public float Z
        {
            get { return mData.Z; }
            set { mData.Z = value; OnElementChanged(); }
        }

        public bool Equals(Vector3Value other)
        {
            return mData.Equals(other.mData);
        }
    }
}