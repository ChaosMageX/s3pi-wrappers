using System;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers.SWB
{
    public class Matrix3x3Value : DataElement, IEquatable<Matrix3x3Value>
    {
        public Matrix3x3Value(int apiVersion, EventHandler handler, Matrix3x3Value basis)
            : base(apiVersion, handler, basis)
        {
        }

        public Matrix3x3Value(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
        }

        public Matrix3x3Value(int apiVersion, EventHandler handler) : base(apiVersion, handler)
        {
            mX = new Vector3ValueLE(0, handler);
            mY = new Vector3ValueLE(0, handler);
            mZ = new Vector3ValueLE(0, handler);
        }

        protected override void Parse(Stream stream)
        {
            mX = new Vector3ValueLE(0, handler, stream);
            mY = new Vector3ValueLE(0, handler, stream);
            mZ = new Vector3ValueLE(0, handler, stream);
        }

        public override void UnParse(Stream stream)
        {
            mX.UnParse(stream);
            mY.UnParse(stream);
            mZ.UnParse(stream);
        }

        private Vector3ValueLE mX, mY, mZ;

        [ElementPriority(1)]
        public Vector3ValueLE X
        {
            get { return mX; }
            set
            {
                mX = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public Vector3ValueLE Y
        {
            get { return mY; }
            set
            {
                mY = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public Vector3ValueLE Z
        {
            get { return mZ; }
            set
            {
                mZ = value;
                OnElementChanged();
            }
        }

        public bool Equals(Matrix3x3Value other)
        {
            return base.Equals(other);
        }
    }
}
