using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.SWB
{
    public class TransformElement : DataElement, IEquatable<TransformElement>
    {
        #region Constructors
        public TransformElement(int apiVersion, EventHandler handler, TransformElement basis)
            : base(apiVersion, handler, basis)
        {
        }

        public TransformElement(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
        }

        public TransformElement(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
            mOrientation = new Matrix3x3Value(0, handler);
            mPosition = new Vector3ValueLE(0, handler);
        }
        #endregion

        #region Attributes
        private ushort mFlags;
        private float mScale;
        private Matrix3x3Value mOrientation;
        private Vector3ValueLE mPosition;
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public ushort Flags
        {
            get { return mFlags; }
            set
            {
                mFlags = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public Matrix3x3Value Orientation
        {
            get { return mOrientation; }
            set
            {
                mOrientation = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public Vector3ValueLE Position
        {
            get { return mPosition; }
            set
            {
                mPosition = value;
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Read(out mFlags);
            s.Read(out mScale);
            mOrientation = new Matrix3x3Value(0, handler, stream);
            mPosition = new Vector3ValueLE(0, handler, stream);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            s.Write(mFlags);
            s.Write(mScale);
            mOrientation.UnParse(stream);
            mPosition.UnParse(stream);
        }

        public bool Equals(TransformElement other)
        {
            return base.Equals(other);
        }
        #endregion
    }
}
