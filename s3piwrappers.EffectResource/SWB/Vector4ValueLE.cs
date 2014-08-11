using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB.Structures;

namespace s3piwrappers.SWB
{
    public class Vector4ValueLE : ValueElement<Vector4>, IEquatable<Vector4ValueLE>
    {
        #region Constructors
        public Vector4ValueLE(int apiVersion, EventHandler handler, Vector4ValueLE basis)
            : base(apiVersion, handler, basis)
        {
        }

        public Vector4ValueLE(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
        }

        public Vector4ValueLE(int apiVersion, EventHandler handler, Vector4 data)
            : base(apiVersion, handler, data)
        {
        }/* */

        public Vector4ValueLE(int apiVersion, EventHandler handler, float x, float y, float z, float w)
            : base(apiVersion, handler, new Vector4(x, y, z, w))
        {
        }

        public Vector4ValueLE(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.LittleEndian);
            s.Read(out mData.X);
            s.Read(out mData.Y);
            s.Read(out mData.Z);
            s.Read(out mData.W);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.LittleEndian);
            s.Write(mData.X);
            s.Write(mData.Y);
            s.Write(mData.Z);
            s.Write(mData.W);
        }
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public float X
        {
            get { return mData.X; }
            set
            {
                mData.X = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public float Y
        {
            get { return mData.Y; }
            set
            {
                mData.Y = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public float Z
        {
            get { return mData.Z; }
            set
            {
                mData.Z = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public float W
        {
            get { return mData.W; }
            set
            {
                mData.W = value;
                OnElementChanged();
            }
        }
        #endregion

        public bool Equals(Vector4ValueLE other)
        {
            return mData.Equals(other.mData);
        }
    }
}
