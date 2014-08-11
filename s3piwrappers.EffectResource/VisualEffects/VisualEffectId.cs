using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers
{
    public class VisualEffectId : DataElement, IEquatable<VisualEffectId>, IComparable<VisualEffectId>
    {
        #region Constructors
        public VisualEffectId(int apiVersion, EventHandler handler, VisualEffectId basis)
            : base(apiVersion, handler)
        {
            mEffectId = basis.EffectId;
            mIndex = basis.Index;
        }

        public VisualEffectId(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
        }

        public VisualEffectId(int apiVersion, EventHandler handler, Stream s, uint index)
            : base(apiVersion, handler, s)
        {
            mIndex = index;
        }
        #endregion

        #region Attributes
        private uint mIndex;
        private ulong mEffectId = 0;
        #endregion

        #region Content Fields
        [ElementPriority(2)]
        public ulong EffectId
        {
            get { return mEffectId; }
            set
            {
                mEffectId = value;
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
            mEffectId = s.ReadUInt64();
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mIndex);
            s.Write(mEffectId);
        }
        #endregion

        public int CompareTo(VisualEffectId other)
        {
            return mEffectId.CompareTo(other.mEffectId);
        }

        public int CompareTo(object obj)
        {
            return mEffectId.CompareTo(((VisualEffectId)obj).mEffectId);
        }

        public override string ToString()
        {
            return string.Concat("0x", mEffectId.ToString("X16"));
        }

        public bool Equals(VisualEffectId other)
        {
            return mEffectId.Equals(other);
        }
    }
}
