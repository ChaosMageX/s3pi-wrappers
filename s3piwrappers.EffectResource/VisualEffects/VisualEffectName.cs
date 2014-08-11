using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers
{
    public class VisualEffectName : DataElement, IEquatable<VisualEffectName>, IComparable<VisualEffectName>
    {
        #region Constructors
        public VisualEffectName(int apiVersion, EventHandler handler, VisualEffectName basis)
            : base(apiVersion, handler)
        {
            mEffectName = basis.EffectName;
            mIndex = basis.Index;
        }

        public VisualEffectName(int apiVersion, EventHandler handler) 
            : base(apiVersion, handler)
        {
        }

        public VisualEffectName(int apiVersion, EventHandler handler, Stream s, uint index) 
            : base(apiVersion, handler, s)
        {
            mIndex = index;
        }
        #endregion

        #region Attributes
        private uint mIndex;
        private string mEffectName = "<Insert Effect Name>";
        #endregion

        #region Content Fields
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

        [ElementPriority(1)]
        public uint Index
        {
            get { return mIndex; }
            set
            {
                mIndex = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            mEffectName = s.ReadString(StringType.ZeroDelimited);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mIndex);
            s.Write(mEffectName, StringType.ZeroDelimited);
        }
        #endregion

        public int CompareTo(VisualEffectName other)
        {
            return mEffectName.CompareTo(other.mEffectName);
        }

        public int CompareTo(object obj)
        {
            return mEffectName.CompareTo(((VisualEffectName) obj).mEffectName);
        }

        public override string ToString()
        {
            return EffectName;
        }

        public bool Equals(VisualEffectName other)
        {
            return mEffectName.Equals(other);
        }
    }
}
