using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB.Structures;

namespace s3piwrappers.SWB
{
    public class ColorValue : ValueElement<Color>, IEquatable<ColorValue>
    {
        #region Constructors
        public ColorValue(int apiVersion, EventHandler handler) 
            : base(apiVersion, handler)
        {
        }

        public ColorValue(int apiVersion, EventHandler handler, ColorValue basis) 
            : base(apiVersion, handler, basis)
        {
        }

        public ColorValue(int apiVersion, EventHandler handler, Color data)
            : base(apiVersion, handler, data)
        {
        }/* */

        public ColorValue(int apiVersion, EventHandler handler, float r, float g, float b)
            : base(apiVersion, handler, new Color(r, g, b))
        {
        }

        public ColorValue(int apiVersion, EventHandler handler, Stream s) 
            : base(apiVersion, handler, s)
        {
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream s)
        {
            var sw = new BinaryStreamWrapper(s, ByteOrder.LittleEndian);
            sw.Read(out mData.R);
            sw.Read(out mData.G);
            sw.Read(out mData.B);
        }

        public override void UnParse(Stream s)
        {
            var sw = new BinaryStreamWrapper(s, ByteOrder.LittleEndian);
            sw.Write(mData.R);
            sw.Write(mData.G);
            sw.Write(mData.B);
        }
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public float R
        {
            get { return mData.R; }
            set
            {
                mData.R = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public float G
        {
            get { return mData.G; }
            set
            {
                mData.G = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public float B
        {
            get { return mData.B; }
            set
            {
                mData.B = value;
                OnElementChanged();
            }
        }
        #endregion

        public bool Equals(ColorValue other)
        {
            return mData.Equals(other.mData);
        }
    }
}
