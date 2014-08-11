using System;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers.SWB
{
    public class BoundingBoxValue : DataElement, IEquatable<BoundingBoxValue>
    {
        public BoundingBoxValue(int apiVersion, EventHandler handler, BoundingBoxValue basis)
            : base(apiVersion, handler, basis)
        {
        }

        public BoundingBoxValue(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
        }

        public BoundingBoxValue(int apiVersion, EventHandler handler) : base(apiVersion, handler)
        {
            mMin = new Vector3ValueLE(apiVersion, handler);
            mMax = new Vector3ValueLE(apiVersion, handler);
        }

        protected override void Parse(Stream stream)
        {
            mMin = new Vector3ValueLE(requestedApiVersion, handler, stream);
            mMax = new Vector3ValueLE(requestedApiVersion, handler, stream);
        }

        public override void UnParse(Stream stream)
        {
            mMin.UnParse(stream);
            mMax.UnParse(stream);
        }

        private Vector3ValueLE mMin, mMax;

        [ElementPriority(1)]
        public Vector3ValueLE Min
        {
            get { return mMin; }
            set
            {
                mMin = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public Vector3ValueLE Max
        {
            get { return mMax; }
            set
            {
                mMax = value;
                OnElementChanged();
            }
        }

        public bool Equals(BoundingBoxValue other)
        {
            return base.Equals(other);
        }
    }
}
