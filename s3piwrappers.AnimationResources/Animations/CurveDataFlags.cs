using System;

namespace s3piwrappers
{
    public struct CurveDataFlags
    {
        public CurveDataFlags(byte flags)
        {
            Raw = flags;
        }

        public CurveDataType Type
        {
            get { return (CurveDataType)((Raw & (byte)0x07) >> 0); }
            set
            {
                Raw &= 0x1F;
                Raw |= (byte)((byte)value << 0);
            }
        }
        public Boolean Static
        {
            get { return ((Raw & (byte)0x08) >> 3) == 1 ? true : false; }
            set
            {
                Raw &= 0xF7;
                Raw |= (byte)((value?1:0) << 3);
            }
        }
        public CurveDataFormat Format
        {
            get { return (CurveDataFormat)((Raw & (byte)0xF0) >> 4); }
            set 
            { 
                Raw &= 0x0F;
                Raw |= (byte)(((byte) value) << 4);
            }
        }
        public Byte Raw;

    }
}