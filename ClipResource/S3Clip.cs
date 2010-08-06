using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using s3piwrappers.Geometry;

namespace s3piwrappers
{

    internal enum ChannelType
    {
        PositionConst = 0x0103,
        Position = 0x0112,
        Orientation = 0x0214
    }
    internal class ClipReadContext
    {
        public Stream Stream { get; private set; }
        public ClipReadContext(Stream s)
        {
            Stream = s;
        }
        public List<Single> FloatConstants { get; set; }
    }

    internal class ChannelReadContext : ClipReadContext
    {
        public ClipReadContext ClipContext { get; private set; }
        public ChannelReadContext(ClipReadContext ctx):base(ctx.Stream)
        {
            ClipContext = ctx;
            FloatConstants = ctx.FloatConstants;
        }
        public Single Offset { get; set; }
        public Single Scalar { get; set; }
    }
    /// <summary>
    /// Animation frame data
    /// </summary>
    /// <remarks>Incomplete; not S3PE-friendly at all yet, no saving</remarks>
    public class S3Clip
    {
        #region Nested Type: Channel
        public class Channel
        {
            private UInt32 mBoneHash;
            private UInt16 mType;
            private List<Frame> mFrames;
            internal Channel(ClipReadContext context)
            {
                Parse(context);
            }
            private void Parse(ClipReadContext context)
            {
                BinaryReader br = new BinaryReader(context.Stream);
                ChannelReadContext trackContext = new ChannelReadContext(context);
                long curveDataOffset = br.ReadUInt32();
                mBoneHash = br.ReadUInt32();
                trackContext.Offset = br.ReadSingle();
                trackContext.Scalar = br.ReadSingle();
                UInt16 frameCount = br.ReadUInt16();
                mType = br.ReadUInt16();
                long pos = context.Stream.Position;
                context.Stream.Seek(curveDataOffset, SeekOrigin.Begin);
                mFrames = new List<Frame>();
                for (int i = 0; i < frameCount; i++)
                {
                    Frame f = Frame.CreateInstance(trackContext, mType);
                    mFrames.Add(f);
                }
                context.Stream.Seek(pos, SeekOrigin.Begin);

            }
            public void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
            }

            public override string ToString()
            {

                return string.Format("Channel: 0x{0:X8}({1})", mBoneHash,(ChannelType)mType);
            }
        } 
        #endregion

        #region Nested Type: Frame
        public abstract class Frame
        {
            protected UInt16 mFrameIndex;
            protected UInt16 mFlags;
            /// <summary>
            /// Unknown bits that follow the sign bits
            /// </summary>
            public int ExtraFlags
            {
                get { return mFlags >> 4; }
            }
            internal Frame(ChannelReadContext ctx)
            {
                Parse(ctx);
            }
            public static Frame CreateInstance(UInt16 type)
            {
                return CreateInstance(null, type);
            }
            internal static Frame CreateInstance(ChannelReadContext ctx, UInt16 type)
            {
                switch (type)
                {
                    case 0x0103: return new ConstPositionFrame(ctx);
                    case 0x0112: return new PositionFrame(ctx);
                    case 0x0214: return new RotationFrame(ctx);
                    default: throw new NotImplementedException(String.Format("Frame type 0x{0:X8} not implemented.", type));
                }
            }
            internal virtual void Parse(ChannelReadContext ctx)
            {
                BinaryReader br = new BinaryReader(ctx.Stream);
                mFrameIndex = br.ReadUInt16();
                mFlags = br.ReadUInt16();
            }
            public virtual void UnParse(Stream s)
            {
                BinaryWriter bw = new BinaryWriter(s);
                bw.Write(mFrameIndex);
                bw.Write(mFlags);
            }
            public override string ToString()
            {
                return String.Format("[{0:X2}]", ExtraFlags);
            }

            public static Quaternion UnpackQuat(UInt64 packed, double offset, double scale, UInt16 flags)
            {
                int signX = ((flags & 0x01) == 0x01 ? -1 : 1);
                int signY = ((flags & 0x02) == 0x02 ? -1 : 1);
                int signZ = ((flags & 0x04) == 0x04 ? -1 : 1);
                int signW = ((flags & 0x08) == 0x08 ? -1 : 1);
                Quaternion q = new Quaternion();
                q.X = offset + ((double)((packed & (0xFFFFUL << 0)) >> 0) / 4095) * signX * scale;
                q.Y = offset + ((double)((packed & (0xFFFFUL << 16)) >> 16) / 4095) * signY * scale;
                q.Z = offset + ((double)((packed & (0xFFFFUL << 32)) >> 32) / 4095) * signZ * scale;
                q.W = offset + ((double)((packed & (0xFFFFUL << 48)) >> 48) / 4095) * signW * scale;
                return q;

            }
            public static Vector3 UnpackVector3(UInt32 packed, double offset, double scale, UInt16 flags)
            {
                int signX = ((flags & 0x01) == 0x01 ? -1 : 1);
                int signY = ((flags & 0x02) == 0x02 ? -1 : 1);
                int signZ = ((flags & 0x04) == 0x04 ? -1 : 1);
                Vector3 v = new Vector3();
                v.X = offset + ((double)((packed & 0x000003FF) >> 0) / 1023) * signX * scale;
                v.Y = offset + ((double)((packed & 0x000FFC00) >> 10) / 1023) * signY * scale;
                v.Z = offset + ((double)((packed & 0x3FF00000) >> 20) / 1023) * signZ * scale;
                return v;
            }
        }
        public class ConstPositionFrame : Frame
        {
            private Vector3 mPos;
            internal ConstPositionFrame(ChannelReadContext ctx) : base(ctx) { mPos = new Vector3(); }
            internal override void Parse(ChannelReadContext ctx)
            {
                base.Parse(ctx);
                BinaryReader br = new BinaryReader(ctx.Stream);
                int signX = ((mFlags & 0x01) == 0x01 ? -1 : 1);
                int signY = ((mFlags & 0x02) == 0x02 ? -1 : 1);
                int signZ = ((mFlags & 0x04) == 0x04 ? -1 : 1);
                mPos.X = ctx.Offset + ctx.FloatConstants[br.ReadUInt16()] * signX * ctx.Scalar;
                mPos.Y = ctx.Offset + ctx.FloatConstants[br.ReadUInt16()] * signY * ctx.Scalar;
                mPos.Z = ctx.Offset + ctx.FloatConstants[br.ReadUInt16()] * signZ * ctx.Scalar;

            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
            }

            public override string ToString()
            {
                return base.ToString()+mPos.ToString();
            }
        }
        public class PositionFrame : Frame
        {
            private Vector3 mPos;
            internal PositionFrame(ChannelReadContext ctx) : base(ctx) { mPos = new Vector3(); }
            internal override void Parse(ChannelReadContext ctx)
            {
                base.Parse(ctx);
                BinaryReader br = new BinaryReader(ctx.Stream);
                mPos = UnpackVector3(br.ReadUInt32(), ctx.Offset, ctx.Scalar, mFlags);                
            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
            }
            public override string ToString()
            {
                return base.ToString() + mPos.ToString();
            }
        }
        public class RotationFrame : Frame
        {
            private Quaternion mRotation;
            internal RotationFrame(ChannelReadContext ctx) : base(ctx) { }

            internal override void Parse(ChannelReadContext ctx)
            {
                base.Parse(ctx);
                BinaryReader br = new BinaryReader(ctx.Stream);
                mRotation = UnpackQuat(br.ReadUInt64(), ctx.Offset, ctx.Scalar, mFlags);

            }
            public override void UnParse(Stream s)
            {
                base.UnParse(s);
                BinaryWriter bw = new BinaryWriter(s);
            }
            public override string ToString()
            {
                return base.ToString() + mRotation.ToString();
            }
        }

        #endregion

        #region Fields
        private UInt32 mVersion;
        private UInt32 mUnknown01;
        private Single mFrameDuration;
        private UInt16 mFrameCount;
        private ushort mUnknown02;
        private string mAnimName;
        private string mSrcName;
        public List<Channel> mTracks; 
        #endregion

        #region Constructors
        public S3Clip(Stream s)
        {
            mTracks = new List<Channel>();
            Parse(s);
        } 
        #endregion

        #region I/O
        private void Parse(Stream s)
        {
            ClipReadContext context = new ClipReadContext(s);
            BinaryReader br = new BinaryReader(s);
            long clipOffset = s.Position;
            string _s3Clip_ = Encoding.ASCII.GetString(br.ReadBytes(8));
            if (_s3Clip_ != "_pilC3S_") throw new Exception(String.Format("Bad clip header: Expected \"_S3Clip_\", but got {0}", _s3Clip_));
            mVersion = br.ReadUInt32();
            mUnknown01 = br.ReadUInt32();
            mFrameDuration = br.ReadSingle();
            mFrameCount = br.ReadUInt16();
            mUnknown02 = br.ReadUInt16();
            UInt32 trackCount = br.ReadUInt32();
            UInt32 constFloatCount = br.ReadUInt32();
            long trackOffset = br.ReadUInt32() + clipOffset;
            long constFloatOffset = br.ReadUInt32() + clipOffset;
            long animNameOffset = br.ReadUInt32() + clipOffset;
            long srcNameOffset = br.ReadUInt32() + clipOffset;

            context.FloatConstants = new List<Single>();
            s.Seek(constFloatOffset, SeekOrigin.Begin);
            for (int i = 0; i < constFloatCount; i++)
            {
                context.FloatConstants[i] = br.ReadSingle();
            }
            s.Seek(trackOffset, SeekOrigin.Begin);
            for (uint i = 0; i < trackCount; i++)
            {
                Channel track = new Channel(context);
                mTracks.Add(track);
            }
            s.Seek(animNameOffset, SeekOrigin.Begin);
            mAnimName = br.ReadZString();
            s.Seek(srcNameOffset, SeekOrigin.Begin);
            mSrcName = br.ReadZString();
        }
        public void UnParse(Stream s)
        {

        } 
        #endregion
    }
}
