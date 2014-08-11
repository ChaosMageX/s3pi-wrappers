using System;
using System.IO;
using s3pi.Interfaces;
using s3piwrappers.SWB;
using s3piwrappers.Helpers.IO;

namespace s3piwrappers.Resources
{
    public class Map : Resource, IEquatable<Map>
    {
        #region Constructors
        public Map(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
            mBounds = new RectangleValue(apiVersion, handler);
            mOpArgValue1 = new Vector4ValueLE(apiVersion, handler);
            mOpArgValue2 = new Vector4ValueLE(apiVersion, handler);
            mOpArgValue3 = new Vector4ValueLE(apiVersion, handler);
            mOpArgValue4 = new Vector4ValueLE(apiVersion, handler);
        }

        public Map(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }

        public Map(int apiVersion, EventHandler handler, Map basis)
            : base(apiVersion, handler, basis)
        {
        }
        #endregion

        #region Attributes
        private ulong mMapId;
        private uint mFlags;
        private byte mMapType;
        private ulong mImageId;
        private RectangleValue mBounds;
        private byte mChannel;
        private byte mOpKind;

        private ulong mOpArgMapId1;
        private ulong mOpArgMapId2;
        private ulong mOpArgMapId3;
        private ulong mOpArgMapId4;

        private Vector4ValueLE mOpArgValue1;
        private Vector4ValueLE mOpArgValue2;
        private Vector4ValueLE mOpArgValue3;
        private Vector4ValueLE mOpArgValue4;
        #endregion

        #region Content Fields
        [ElementPriority(1)]
        public ulong MapId
        {
            get { return mMapId; }
            set
            {
                mMapId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public uint Flags
        {
            get { return mFlags; }
            set
            {
                mFlags = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public byte MapType
        {
            get { return mMapType; }
            set
            {
                mMapType = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public ulong ImageId
        {
            get { return mImageId; }
            set
            {
                mImageId = value;
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public RectangleValue Bounds
        {
            get { return mBounds; }
            set
            {
                mBounds = new RectangleValue(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public byte Channel
        {
            get { return mChannel; }
            set
            {
                mChannel = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public byte OpKind
        {
            get { return mOpKind; }
            set
            {
                mOpKind = value;
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public ulong OpArgMapId1
        {
            get { return mOpArgMapId1; }
            set
            {
                mOpArgMapId1 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(9)]
        public ulong OpArgMapId2
        {
            get { return mOpArgMapId2; }
            set
            {
                mOpArgMapId2 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(10)]
        public ulong OpArgMapId3
        {
            get { return mOpArgMapId3; }
            set
            {
                mOpArgMapId3 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(11)]
        public ulong OpArgMapId4
        {
            get { return mOpArgMapId4; }
            set
            {
                mOpArgMapId4 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(12)]
        public Vector4ValueLE OpArgValue1
        {
            get { return mOpArgValue1; }
            set
            {
                mOpArgValue1 = new Vector4ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(13)]
        public Vector4ValueLE OpArgValue2
        {
            get { return mOpArgValue2; }
            set
            {
                mOpArgValue2 = new Vector4ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(14)]
        public Vector4ValueLE OpArgValue3
        {
            get { return mOpArgValue3; }
            set
            {
                mOpArgValue3 = new Vector4ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }

        [ElementPriority(15)]
        public Vector4ValueLE OpArgValue4
        {
            get { return mOpArgValue4; }
            set
            {
                mOpArgValue4 = new Vector4ValueLE(requestedApiVersion, handler, value);
                OnElementChanged();
            }
        }
        #endregion

        #region Data I/O
        protected override void Parse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);
            
            s.Read(out mMapId);

            s.Read(out mFlags);
            mFlags &= 0x3FF;
            
            s.Read(out mMapType);
            s.Read(out mImageId);
            mBounds = new RectangleValue(requestedApiVersion, handler, stream);
            s.Read(out mChannel);
            s.Read(out mOpKind);

            s.Read(out mOpArgMapId1);
            s.Read(out mOpArgMapId2);
            s.Read(out mOpArgMapId3);
            s.Read(out mOpArgMapId4);

            mOpArgValue1 = new Vector4ValueLE(requestedApiVersion, handler, stream);
            mOpArgValue2 = new Vector4ValueLE(requestedApiVersion, handler, stream);
            mOpArgValue3 = new Vector4ValueLE(requestedApiVersion, handler, stream);
            mOpArgValue4 = new Vector4ValueLE(requestedApiVersion, handler, stream);
        }

        public override void UnParse(Stream stream)
        {
            var s = new BinaryStreamWrapper(stream, ByteOrder.BigEndian);

            s.Write(mMapId);

            s.Write(mFlags);

            s.Write(mMapType);
            s.Write(mImageId);
            mBounds.UnParse(stream);
            s.Write(mChannel);
            s.Write(mOpKind);

            s.Write(mOpArgMapId1);
            s.Write(mOpArgMapId2);
            s.Write(mOpArgMapId3);
            s.Write(mOpArgMapId4);

            mOpArgValue1.UnParse(stream);
            mOpArgValue2.UnParse(stream);
            mOpArgValue3.UnParse(stream);
            mOpArgValue4.UnParse(stream);
        }
        #endregion

        public bool Equals(Map other)
        {
            return base.Equals(other);
        }
    }
}
