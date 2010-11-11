using System;
using System.IO;
using s3pi.Interfaces;
using System.Collections.Generic;

namespace s3piwrappers
{
    public class PositionFrame : Frame
    {
        public PositionFrame(int apiVersion, EventHandler handler, PositionFrame basis)
            : base(apiVersion, handler, basis)
        {
        }
        public PositionFrame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
       
        
        protected float mX;
        protected float mY;
        protected float mZ;
        public PositionFrame(int apiVersion, EventHandler handler, Stream s, CurveDataInfo info, IList<float> indexedFloats) : base(apiVersion, handler, s, info, indexedFloats) {}
        public override IEnumerable<float> GetFloatValues()
        {
            yield return X;
            yield return Y;
            yield return Z;
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

        public override string ToString()
        {
            return base.ToString() + String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]", mX, mY, mZ);
        }


        private void ReadIndexed(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            BinaryReader br = new BinaryReader(s);
            int signX = ((mFlags & 0x01) == 0x01 ? -1 : 1);
            int signZ = ((mFlags & 0x02) == 0x02 ? -1 : 1);
            int signY = ((mFlags & 0x04) == 0x04 ? -1 : 1);
            ushort index0 = br.ReadUInt16();
            ushort index1 = br.ReadUInt16();
            ushort index2 = br.ReadUInt16();
            mX = info.Base * signX + indexedFloats[index0] * info.Scale;
            mZ = info.Base * signZ + indexedFloats[index1] * info.Scale;
            mY = info.Base * signY + indexedFloats[index2] * info.Scale;
        }
        private void ReadPacked(Stream s, CurveDataInfo info)
        {
            BinaryReader br = new BinaryReader(s);
            UInt32 packed = br.ReadUInt32();
                
            int signX = ((mFlags & 0x01) == 0x01 ? -1 : 1);
            int signZ = ((mFlags & 0x02) == 0x02 ? -1 : 1);
            int signY = ((mFlags & 0x04) == 0x04 ? -1 : 1);
            mX = info.Base * signX + ((float)((packed & 0x000003FF) >> 0) / 1023) * info.Scale;
            mZ = info.Base * signZ + ((float)((packed & 0x000FFC00) >> 10) / 1023) * info.Scale;
            mY = info.Base * signY + ((float)((packed & 0x3FF00000) >> 20) / 1023) * info.Scale;
        }
        private void WriteIndexed(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(indexedFloats.IndexOf(mX));
            bw.Write(indexedFloats.IndexOf(mY));
            bw.Write(indexedFloats.IndexOf(mZ));
        }
        private void WritePacked(Stream s, CurveDataInfo info)
        {
            //stub until recompression code is implemented
            BinaryWriter bw = new BinaryWriter(s);
            throw new NotImplementedException();
        }
        public override void Parse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            base.Parse(s, info, indexedFloats);
            switch(info.Flags.Format)
            {
                case CurveDataFormat.Indexed:
                    ReadIndexed(s, info,indexedFloats);
                    break;
                case CurveDataFormat.Packed:
                    ReadPacked(s, info);
                    break;
                default:
                    throw new Exception("Unable to parse format "+info.Flags.Format.ToString());
            }
        }
        public override void UnParse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            base.UnParse(s, info, indexedFloats);
            switch (info.Flags.Format)
            {
                case CurveDataFormat.Indexed:
                    WriteIndexed(s, info, indexedFloats);
                    break;
                case CurveDataFormat.Packed:
                    WritePacked(s, info);
                    break;
                default:
                    throw new Exception("Unable to parse format " + info.Flags.Format.ToString());
            }
        }

    }
}