using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using System.Text;

namespace s3piwrappers
{
    public class BoundingBox : AHandlerElement, IEquatable<BoundingBox>
    {
        private Single mMinX;
        private Single mMinY;
        private Single mMinZ;
        private Single mMaxX;
        private Single mMaxY;
        private Single mMaxZ;

        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Min:(X={0,8:0.00000},Y={1,8:0.00000},Z={2,8:0.00000})\n", mMinX, mMinY, mMinZ);
                sb.AppendFormat("Max:(X={0,8:0.00000},Y={1,8:0.00000},Z={2,8:0.00000})\n", mMaxX, mMaxY, mMaxZ);
                return sb.ToString();
            }
        }
        public BoundingBox(int APIversion, EventHandler handler)
            : base(APIversion, handler)
        {
        }
        public BoundingBox(int APIversion, EventHandler handler, BoundingBox basis)
            : base(APIversion, handler)
        {
            mMinX = basis.mMinX;
            mMaxX = basis.mMaxX;
            mMinY = basis.mMinY;
            mMaxY = basis.mMaxY;
            mMinZ = basis.mMinZ;
            mMaxZ = basis.mMaxZ;
        }
        public BoundingBox(int APIversion, EventHandler handler, Stream s)
            : base(APIversion, handler)
        {
            Parse(s);
        }
        [ElementPriority(1)]
        public float MinX
        {
            get { return mMinX; }
            set { mMinX = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public float MinY
        {
            get { return mMinY; }
            set { mMinY = value; OnElementChanged(); }
        }
        [ElementPriority(3)]
        public float MinZ
        {
            get { return mMinZ; }
            set { mMinZ = value; OnElementChanged(); }
        }
        [ElementPriority(4)]
        public float MaxX
        {
            get { return mMaxX; }
            set { mMaxX = value; OnElementChanged(); }
        }
        [ElementPriority(5)]
        public float MaxY
        {
            get { return mMaxY; }
            set { mMaxY = value; OnElementChanged(); }
        }
        [ElementPriority(6)]
        public float MaxZ
        {
            get { return mMaxZ; }
            set { mMaxZ = value; OnElementChanged(); }
        }

        private void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            mMinX = br.ReadSingle();
            mMinY = br.ReadSingle();
            mMinZ = br.ReadSingle();
            mMaxX = br.ReadSingle();
            mMaxY = br.ReadSingle();
            mMaxZ = br.ReadSingle();
        }
        public void UnParse(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(mMinX);
            bw.Write(mMinY);
            bw.Write(mMinZ);
            bw.Write(mMaxX);
            bw.Write(mMaxY);
            bw.Write(mMaxZ);
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
    public class BoundingBoxList : AResource.DependentList<BoundingBox>
    {
        public BoundingBoxList(EventHandler handler)
            : base(handler)
        {
        }

        public BoundingBoxList(EventHandler handler, Stream s)
            : base(handler, s)
        {
        }

        public BoundingBoxList(EventHandler handler, IList<BoundingBox> ilt) : base(handler, ilt) {}

        public override void Add()
        {
            base.Add(new object[] { });
        }

        protected override BoundingBox CreateElement(Stream s)
        {
            return new BoundingBox(0, this.handler, s);
        }

        protected override void WriteElement(Stream s, BoundingBox element)
        {
            element.UnParse(s);
        }
    }
}