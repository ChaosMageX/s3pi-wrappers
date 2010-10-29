using System;
using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    [DataGridExpandable(true)]
    public class Vector3 : AHandlerElement
    {
        private float mX, mY, mZ;
        public Vector3(int APIversion, EventHandler handler, float x, float y, float z)
            : base(APIversion, handler)
        {
            mX = x;
            mY = y;
            mZ = z;
        }

        public Vector3(int APIversion, EventHandler handler) : base(APIversion, handler) { }
        public Vector3(int APIversion, EventHandler handler, Vector3 basis) : this(APIversion, handler, basis.mX, basis.mY, basis.mZ) { }
        [ElementPriority(1)]
        public float X
        {
            get { return mX; }
            set { mX = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public float Y
        {
            get { return mY; }
            set { mY = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public float Z
        {
            get { return mZ; }
            set { mZ = value; OnElementChanged(); }
        }

        public override string ToString()
        {
            return String.Format("[{0:0.000000},{1:0.000000},{2:0.000000}]", X, Y, Z);
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new Vector3(0, handler, this);
        }

        public override List<string> ContentFields
        {
            get { return GetContentFields(requestedApiVersion, GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}