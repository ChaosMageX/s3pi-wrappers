using System;
using s3pi.Interfaces;

namespace s3piwrappers.Granny2
{
    public class Triple : GrannyElement
    {

        private float mX;
        private float mY;
        private float mZ;
        public Triple(int APIversion, EventHandler handler) : base(APIversion, handler) {}

        public Triple(int APIversion, EventHandler handler, float x, float y, float z) : base(APIversion, handler)
        {
            mX = x;
            mY = y;
            mZ = z;
        }
        public Triple(int APIversion, EventHandler handler,Triple t) : this(APIversion, handler,t.mX,t.mY,t.mZ) { }
        internal Triple(int APIversion, EventHandler handler,_Triple t) : base(APIversion, handler) {FromStruct(t); }

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

        internal void FromStruct(_Triple data)
        {
            X = data.X;
            Y = data.Y;
            Z = data.Z;
        }

        internal _Triple ToStruct()
        {
            var t = new _Triple();
            t.X = X;
            t.Y = Y;
            t.Z = Z;
            return t;
        }
        public override string ToString()
        {
            return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]", mX, mY, mZ);
        }
        public override string Value
        {
            get { return ToString(); }
        }
    }
}