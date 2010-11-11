using System;
using System.IO;
using s3pi.Interfaces;
using System.Collections.Generic;

namespace s3piwrappers
{
    public class OrientationFrame : Frame
    {
        public OrientationFrame(int apiVersion, EventHandler handler, OrientationFrame basis)
            : base(apiVersion, handler, basis)
        {
            mX = basis.mX;
            mY = basis.mY;
            mZ = basis.mZ;
            mW = basis.mW;
        }
        public OrientationFrame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }


        private float mX;
        private float mY;
        private float mZ;
        private float mW;
        public OrientationFrame(int apiVersion, EventHandler handler, Stream s, CurveDataInfo info, IList<float> indexedFloats) : base(apiVersion, handler, s, info, indexedFloats) {}
        public override IEnumerable<float> GetFloatValues()
        {
            yield return X;
            yield return Y;
            yield return Z;
            yield return W;
        }
        [ElementPriority(4)]
        public float X
        {
            get { return mX; }
            set { mX = value; OnElementChanged(); }
        }
        [ElementPriority(5)]
        public float Y
        {
            get { return mY; }
            set { mY = value; OnElementChanged(); }
        }
        [ElementPriority(6)]
        public float Z
        {
            get { return mZ; }
            set { mZ = value; OnElementChanged(); }
        }
        [ElementPriority(7)]
        public float W
        {
            get { return mW; }
            set { mW = value; OnElementChanged(); }
        }

        public override void Parse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            base.Parse(s,info,indexedFloats);
            BinaryReader br = new BinaryReader(s);
            float packedX = br.ReadUInt16();
            float packedY = br.ReadUInt16();
            float packedZ = br.ReadUInt16();
            float packedW = br.ReadUInt16();
            int signX = ((mFlags & 0x01) == 0x01 ? -1 : 1);
            int signY = ((mFlags & 0x02) == 0x02 ? -1 : 1);
            int signZ = ((mFlags & 0x04) == 0x04 ? -1 : 1);
            int signW = ((mFlags & 0x08) == 0x08 ? -1 : 1);
            mX = info.Base * signX + (packedX / 0x0FFF) * info.Scale;
            mY = info.Base * signY + (packedY / 0x0FFF) * info.Scale;
            mZ = info.Base * signZ + (packedZ / 0x0FFF) * info.Scale;
            mW = info.Base * signW + (packedW / 0x0FFF) * info.Scale;


        }
        public override void UnParse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            base.UnParse(s,info,indexedFloats);
            BinaryWriter bw = new BinaryWriter(s);
        }
        public override string ToString()
        {
            return base.ToString() + String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000},{3,8:0.00000}]", mX, mY, mZ, mW);
        }

        public bool Equals(OrientationFrame other)
        {
            return base.Equals(other);
        }
    }
}