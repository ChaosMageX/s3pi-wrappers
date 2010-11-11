using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using System.Text;

namespace s3piwrappers
{
    public class BoundingBox : AHandlerElement, IEquatable<BoundingBox>
    {
        private Vector3 mMin;
        private Vector3 mMax;
        public string Value
        {
            get { return string.Format("Min:{0}\nMax{1}", mMin, mMax); }
        }
        public BoundingBox(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
        }
        public BoundingBox(int APIversion, EventHandler handler, BoundingBox basis)
            : this(APIversion, handler, new Vector3(0, handler, basis.Min), new Vector3(0, handler, basis.mMax)) { }
        public BoundingBox(int APIversion, EventHandler handler, Vector3 min, Vector3 max)
            : base(APIversion, handler)
        {
            mMin = min;
            mMax = max;
        }
        public BoundingBox(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler)
        {
            Parse(s);
        }
        [ElementPriority(1)]
        public Vector3 Min
        {
            get { return mMin; }
            set { if (mMin != value) { mMin = value; OnElementChanged(); } }
        }
        [ElementPriority(2)]
        public Vector3 Max
        {
            get { return mMax; }
            set { if (mMax != value) { mMax = value; OnElementChanged(); } }
        }

        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            mMin = new Vector3(0, handler, br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            mMax = new Vector3(0, handler, br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        }
        public void UnParse(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(mMin.X);
            bw.Write(mMin.Y);
            bw.Write(mMin.Z);
            bw.Write(mMax.X);
            bw.Write(mMax.Y);
            bw.Write(mMax.Z);
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new BoundingBox(0, handler, this);
        }

        public override List<string> ContentFields
        {
            get { return GetContentFields(base.requestedApiVersion, GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }

        public bool Equals(BoundingBox other)
        {
            return base.Equals(other);
        }
    }

}