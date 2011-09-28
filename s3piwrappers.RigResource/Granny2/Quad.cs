using System;
using s3pi.Interfaces;

namespace s3piwrappers.Granny2
{
    public class Quad : GrannyElement, IEquatable<Quad>
    {
        private float mX;
        private float mY;
        private float mZ;
        private float mW;


        public Quad(int APIversion, EventHandler handler) : base(APIversion, handler) {}

        public Quad(int APIversion, EventHandler handler, float x, float y, float z, float w) : base(APIversion, handler)
        {
            mX = x;
            mY = y;
            mZ = z;
            mW = w;
        }
        public Quad(int APIversion, EventHandler handler, Quad basis) : this(APIversion, handler, basis.mX, basis.mY, basis.mZ, basis.mW) { }
        internal Quad(int APIversion, EventHandler handler,_Quad q) : base(APIversion, handler) { FromStruct(q);}

        [ElementPriority(1)]
        public float X
        {
            get { return mX; }
            set { if(mX!=value){mX = value; OnElementChanged();} }
        }
        [ElementPriority(2)]
        public float Y
        {
            get { return mY; }
            set { if(mY!=value){mY = value; OnElementChanged();} }
        }
        [ElementPriority(3)]
        public float Z
        {
            get { return mZ; }
            set { if(mZ!=value){mZ = value; OnElementChanged();} }
        }
        [ElementPriority(4)]
        public float W
        {
            get { return mW; }
            set { if(mW!=value){mW = value; OnElementChanged();} }
        }

        internal void FromStruct(_Quad data)
        {
            X = data.X;
            Y = data.Y;
            Z = data.Z;
            W = data.W;
        }

        internal _Quad ToStruct()
        {
            var t = new _Quad();
            t.X = X;
            t.Y = Y;
            t.Z = Z;
            t.W = W;
            return t;
        }
        public override string ToString()
        {
            return String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]", mX, mY, mZ, mW);
        }
        public override string Value
        {
            get { return ToString(); }
        }

        public bool Equals(Quad other)
        {
            return this.mX == other.mX && this.mY == other.mY && this.mZ == other.mZ && this.mW == other.mW;
        }
    }
}