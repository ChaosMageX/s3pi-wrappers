using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB.Structures;

namespace s3piwrappers.SWB
{
    public class Vector2ValueLE : ValueElement<Vector2>, IEquatable<Vector2ValueLE>
    {
        #region Constructors
        public Vector2ValueLE(int apiVersion, EventHandler handler, Vector2ValueLE basis) 
            : base(apiVersion, handler, basis)
        {
        }

        public Vector2ValueLE(int apiVersion, EventHandler handler) 
            : base(apiVersion, handler)
        {
        }

        public Vector2ValueLE(int apiVersion, EventHandler handler, Vector2 data) 
            : base(apiVersion, handler, data)
        {
        }/* */

        public Vector2ValueLE(int apiVersion, EventHandler handler, float x, float y)
            : base(apiVersion, handler, new Vector2(x, y))
        {
        }

        public Vector2ValueLE(int apiVersion, EventHandler handler, Stream s) 
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
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.LittleEndian);
            s.Write(mData.X);
            s.Write(mData.Y);
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
        #endregion

        public bool Equals(Vector2ValueLE other)
        {
            return mData.Equals(other.mData);
        }
    }
}
