using System;
using s3pi.Interfaces;

namespace s3piwrappers.Granny2
{
    public class Matrix4x4 : GrannyElement
    {
        private Quad mM0;
        private Quad mM1;
        private Quad mM2;
        private Quad mM3;

        public Matrix4x4(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
            mM0 = new Quad(0, handler, 1, 0, 0, 0);
            mM1 = new Quad(0, handler, 0, 1, 0, 0);
            mM2 = new Quad(0, handler, 0, 0, 1, 0);
            mM3 = new Quad(0, handler, 0, 0, 0, 1);
        }
        public Matrix4x4(int APIversion, EventHandler handler, Quad m0, Quad m1, Quad m2, Quad m3)
            : base(APIversion, handler)
        {
            mM0 = new Quad(0, handler, m0);
            mM1 = new Quad(0, handler, m1);
            mM2 = new Quad(0, handler, m2);
            mM3 = new Quad(0, handler, m3);
        }
        public Matrix4x4(int APIversion, EventHandler handler, Matrix4x4 basis)
            : this(APIversion, handler, basis.mM0, basis.mM1, basis.mM2, basis.mM3) { }
        internal Matrix4x4(int APIversion, EventHandler handler, _Matrix4x4 m) : base(APIversion, handler) { FromStruct(m); }

        [ElementPriority(1)]
        public Quad M0
        {
            get { return mM0; }
            set { mM0 = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public Quad M1
        {
            get { return mM1; }
            set { mM1 = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public Quad M2
        {
            get { return mM2; }
            set { mM2 = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public Quad M3
        {
            get { return mM3; }
            set { mM3 = value; OnElementChanged(); }
        }
        internal void FromStruct(_Matrix4x4 data)
        {
            mM0 = new Quad(0, handler, data.m0);
            mM1 = new Quad(0, handler, data.m1);
            mM2 = new Quad(0, handler, data.m2);
            mM3 = new Quad(0, handler, data.m3);
        }

        internal _Matrix4x4 ToStruct()
        {
            var m = new _Matrix4x4();
            m.m0 = M0.ToStruct();
            m.m1 = M1.ToStruct();
            m.m2 = M2.ToStruct();
            m.m3 = M3.ToStruct();
            return m;
        }

        public override string ToString()
        {
            return String.Format("{0}\n{1}\n{2}\n{3}", mM0, mM1, mM2, mM3);
        }
        public override string Value
        {
            get { return ToString(); }
        }
    }
}