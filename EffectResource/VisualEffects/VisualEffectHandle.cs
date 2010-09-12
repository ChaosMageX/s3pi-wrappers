using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.SWB.IO;

namespace s3piwrappers
{
    public class VisualEffectHandle : DataElement, IEquatable<VisualEffectHandle>
    {
        public VisualEffectHandle(int apiVersion, EventHandler handler, VisualEffectHandle basis)
            : base(apiVersion, handler)
        {
            mEffectName = basis.EffectName;
            mIndex = basis.Index;
        }
        public VisualEffectHandle(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
        public VisualEffectHandle(int apiVersion, EventHandler handler, Stream s, uint index) : base(apiVersion, handler, s) { mIndex = index; }


        #region Fields
        private UInt32 mIndex;
        private string mEffectName = "<Insert Effect Name>";

        #endregion

        #region Properties
        [ElementPriority(2)]
        public String EffectName
        {
            get { return mEffectName; }
            set { mEffectName = value; OnElementChanged(); }
        }
        [ElementPriority(1)]
        public uint Index
        {
            get { return mIndex; }
            set { mIndex = value; OnElementChanged(); }
        }

        #endregion


        protected override void Parse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            mEffectName = s.ReadString(StringType.ZeroDelimited);
        }

        public override void UnParse(Stream stream)
        {
            BinaryStreamWrapper s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mIndex);
            s.Write(mEffectName, StringType.ZeroDelimited);
        }


        #region IComparable<VisualEffectHandle> Members

        public int CompareTo(VisualEffectHandle other)
        {
            return mEffectName.CompareTo(other.mEffectName);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return mEffectName.CompareTo(((VisualEffectHandle)obj).mEffectName);
        }

        #endregion

        public override string ToString()
        {
            return EffectName;
        }

        public bool Equals(VisualEffectHandle other)
        {
            return mEffectName.Equals(other);
        }
    }
}