using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.Helpers.IO;
using s3piwrappers.SWB.Structures;

namespace s3piwrappers.SWB
{
    public class Vector3ValueLE : ValueElement<Vector3>, IEquatable<Vector3ValueLE>
    {
        #region Constructors
        public Vector3ValueLE(int apiVersion, EventHandler handler, Vector3ValueLE basis) 
            : base(apiVersion, handler, basis)
        {
        }

        public Vector3ValueLE(int apiVersion, EventHandler handler) 
            : base(apiVersion, handler)
        {
        }

        public Vector3ValueLE(int apiVersion, EventHandler handler, Vector3 data) 
            : base(apiVersion, handler, data)
        {
        }/* */

        public Vector3ValueLE(int apiVersion, EventHandler handler, float x, float y, float z)
            : base(apiVersion, handler, new Vector3(x, y, z))
        {
        }

        public Vector3ValueLE(int apiVersion, EventHandler handler, Stream s) 
            : base(apiVersion, handler, s)
        {
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.LittleEndian);
            s.Read(out mData.X);
            s.Read(out mData.Y);
            s.Read(out mData.Z);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.LittleEndian);
            s.Write(mData.X);
            s.Write(mData.Y);
            s.Write(mData.Z);
        }
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public float X
        {
            get { return mData.X; }
            set
            {
                mData.X = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public float Y
        {
            get { return mData.Y; }
            set
            {
                mData.Y = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public float Z
        {
            get { return mData.Z; }
            set
            {
                mData.Z = value;
                OnElementChanged();
            }
        }
        #endregion

        public bool Equals(Vector3ValueLE other)
        {
            return mData.Equals(other.mData);
        }
    }
}
