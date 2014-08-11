using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB.Structures;

namespace s3piwrappers.SWB
{
    public class RectangleValue : ValueElement<Rectangle>, IEquatable<RectangleValue>
    {
        #region Constructors
        public RectangleValue(int apiVersion, EventHandler handler, RectangleValue basis)
            : base(apiVersion, handler, basis)
        {
        }

        public RectangleValue(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
        }

        public RectangleValue(int apiVersion, EventHandler handler, Rectangle data)
            : base(apiVersion, handler, data)
        {
        }/* */

        public RectangleValue(int apiVersion, EventHandler handler, float left, float top, float right, float bottom)
            : base(apiVersion, handler, new Rectangle(left, top, right, bottom))
        {
        }

        public RectangleValue(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mData.Left);
            s.Read(out mData.Top);
            s.Read(out mData.Right);
            s.Read(out mData.Bottom);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mData.Left);
            s.Write(mData.Top);
            s.Write(mData.Right);
            s.Write(mData.Bottom);
        }
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public float Left
        {
            get { return mData.Left; }
            set
            {
                mData.Left = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public float Top
        {
            get { return mData.Top; }
            set
            {
                mData.Top = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public float Right
        {
            get { return mData.Right; }
            set
            {
                mData.Right = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public float Bottom
        {
            get { return mData.Bottom; }
            set
            {
                mData.Bottom = value;
                OnElementChanged();
            }
        }
        #endregion

        public bool Equals(RectangleValue other)
        {
            return mData.Equals(other.mData);
        }
    }
}
