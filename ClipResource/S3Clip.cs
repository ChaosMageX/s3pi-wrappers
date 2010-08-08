using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using s3pi.Custom;
using System.Linq;
using s3pi.Interfaces;
using System.Reflection;
using System.Globalization;

namespace s3piwrappers
{

    public enum ChannelType
    {
        PositionConst = 0x0103,
        PositionStatic = 0x10B,
        Position = 0x0112,
        Rotation = 0x0214,
        U0x20C = 0x20C,
        U0x211 = 0x211

    }
    public class ClipReadContext
    {
        public Stream Stream { get; private set; }
        public ClipReadContext(Stream s)
        {
            FloatConstants = new List<Single>();
            Stream = s;
        }

        public UInt16 MaxFrameCount { get; set; }
        public UInt32 ChannelCount { get; set; }
        public List<Single> FloatConstants { get; set; }
    }

    public class ChannelReadContext : ClipReadContext
    {
        public ClipReadContext ClipContext { get; private set; }
        public ChannelReadContext(ClipReadContext ctx)
            : base(ctx.Stream)
        {
            ClipContext = ctx;
            FloatConstants = ctx.FloatConstants;
            Floats = new List<double>();
        }

        public List<double> Floats { get; set; }
        public long FrameDataOffset { get; set; }
        public uint FrameCount { get; set; }
        public uint BoneHash { get; set; }
        public Single Offset { get; set; }
        public Single Scalar { get; set; }
    }
    /// <summary>
    /// Animation frame data
    /// </summary>
    public class S3Clip : DependentElement
    {
        public class ChannelList : AHandlerList<Channel>, IGenericAdd
        {
            public ChannelList(EventHandler handler)
                : base(handler)
            {
            }
            public ChannelList(EventHandler handler, ClipReadContext ctx)
                : base(handler)
            {
                Parse(ctx);
            }
            private void Parse(ClipReadContext ctx)
            {

                BinaryReader br = new BinaryReader(ctx.Stream);
                for (int i = 0; i < ctx.ChannelCount; i++)
                {
                    ChannelReadContext channelContext = new ChannelReadContext(ctx);
                    channelContext.FrameDataOffset = br.ReadUInt32();
                    channelContext.BoneHash = br.ReadUInt32();
                    channelContext.Offset = br.ReadSingle();
                    channelContext.Scalar = br.ReadSingle();
                    channelContext.FrameCount = br.ReadUInt16();
                    Channel c = Channel.CreateInstance(0, base.handler, channelContext, (ChannelType)br.ReadUInt16());
                    base.Add(c);
                }
            }
            public void UnParse(Stream s)
            {

            }

            public bool Add(params object[] fields)
            {
                if (fields.Length == 0) return false;
                if (fields.Length == 1 && typeof(Channel).IsAssignableFrom(fields[0].GetType()))
                {
                    base.Add((Channel)fields[0]);
                    return true;
                }
                if (fields.Length == 1 && typeof(ChannelType).IsAssignableFrom(fields[0].GetType()))
                {
                    base.Add((Channel)Activator.CreateInstance(typeof(Channel), new object[] { 0, handler, fields[0] }));
                }
                return false;
            }

            public void Add()
            {
                throw new NotImplementedException();
            }
        }
        #region Nested Type: Channel
        public class FrameList<TFrame> : FrameList
            where TFrame : Frame
        {
            public FrameList(EventHandler handler) : base(handler)
            {
            }

            public FrameList(EventHandler handler, ChannelReadContext ctx) : base(handler, ctx)
            {
            }

            protected override Frame CreateElement(int apiVersion, EventHandler handler, ChannelReadContext context)
            {
                return (TFrame)Activator.CreateInstance(typeof(TFrame), new object[] { 0, handler, context });
            }
            public override void Add()
            {
                base.Add((TFrame)Activator.CreateInstance(typeof(TFrame), new object[] { 0, handler }));
            }
        }
        public abstract class FrameList : AHandlerList<Frame>, IGenericAdd
        {
            public FrameList(EventHandler handler) : base(handler) { }
            public FrameList(EventHandler handler, ChannelReadContext ctx)
                : base(handler)
            {
                Parse(ctx);
            }
            private void Parse(ChannelReadContext context)
            {
                long pos = context.Stream.Position;
                context.Stream.Seek(context.FrameDataOffset, SeekOrigin.Begin);

                for (int i = 0; i < context.FrameCount; i++)
                {
                    base.Add(CreateElement(0, handler, context));
                }
                context.Stream.Seek(pos, SeekOrigin.Begin);
                if(context.Floats.Count >0)
                {
                    double min = context.Floats.Min(x=>x);
                    double max = context.Floats.Max(x => x);
                    double avg = context.Floats.Average(x => x);
                }
            }

            protected abstract Frame CreateElement(int apiVersion, EventHandler handler, ChannelReadContext context);
            public void UnParse(ChannelWriteContext context)
            {
                foreach (var f in this) { f.UnParse(context); }
            }
            public bool Add(params object[] fields)
            {
                if (fields.Length == 0) return false;
                if (fields.Length == 1 && typeof(Frame).IsAssignableFrom(fields[0].GetType()))
                {
                    base.Add((Frame)fields[0]);
                    return true;
                }
                return false;
            }

            public abstract void Add();
        }
        [ConstructorParameters(new object[] { ChannelType.Rotation })]
        public class RotationChannel : Channel<RotationFrame>
        {
            public RotationChannel(int apiVersion, EventHandler handler, ChannelType type, RotationChannel basis)
                : base(apiVersion, handler, basis) { }
            public RotationChannel(int apiVersion, EventHandler handler, ChannelType type)
                : base(apiVersion, handler, type) { }
            public RotationChannel(int apiVersion, EventHandler handler, ChannelType type, ChannelReadContext context)
                : base(apiVersion, handler, type, context) { }
        }
        [ConstructorParameters(new object[] { ChannelType.PositionConst })]
        public class PositionConstChannel : Channel<PositionConstFrame>
        {
            public PositionConstChannel(int apiVersion, EventHandler handler, ChannelType type, PositionConstChannel basis)
                : base(apiVersion, handler, basis) { }
            public PositionConstChannel(int apiVersion, EventHandler handler, ChannelType type)
                : base(apiVersion, handler, type) { }
            public PositionConstChannel(int apiVersion, EventHandler handler, ChannelType type, ChannelReadContext context)
                : base(apiVersion, handler, type, context) { }
        }
        [ConstructorParameters(new object[] { ChannelType.PositionStatic })]
        public class PositionStaticChannel : Channel<Frame>
        {
            public PositionStaticChannel(int apiVersion, EventHandler handler, ChannelType type)
                : base(apiVersion, handler, type) { }
            public PositionStaticChannel(int apiVersion, EventHandler handler, ChannelType type, PositionStaticChannel basis)
                : base(apiVersion, handler, basis) { }
            public PositionStaticChannel(int apiVersion, EventHandler handler, ChannelType type, ChannelReadContext context)
                : base(apiVersion, handler, type, context) { }
        }
        [ConstructorParameters(new object[] { ChannelType.Position })]
        public class PositionChannel : Channel<PositionFrame>
        {
            public PositionChannel(int apiVersion, EventHandler handler, ChannelType type)
                : base(apiVersion, handler, type) { }
            public PositionChannel(int apiVersion, EventHandler handler, ChannelType type, PositionChannel basis)
                : base(apiVersion, handler, basis) { }
            public PositionChannel(int apiVersion, EventHandler handler, ChannelType type, ChannelReadContext context)
                : base(apiVersion, handler, type, context) { }
        }

        [ConstructorParameters(new object[] { ChannelType.U0x211 })]
        public class U0x211Channel : Channel<Frame>
        {
            public U0x211Channel(int apiVersion, EventHandler handler, ChannelType type, U0x211Channel basis)
                : base(apiVersion, handler, basis) { }
            public U0x211Channel(int apiVersion, EventHandler handler, ChannelType type)
                : base(apiVersion, handler, type) { }
            public U0x211Channel(int apiVersion, EventHandler handler, ChannelType type, ChannelReadContext context)
                : base(apiVersion, handler, type, context) { }
        }
        [ConstructorParameters(new object[] { ChannelType.U0x20C })]
        public class U0x20CChannel : Channel<Frame>
        {
            public U0x20CChannel(int apiVersion, EventHandler handler, ChannelType type, U0x20CChannel basis)
                : base(apiVersion, handler, basis) { }
            public U0x20CChannel(int apiVersion, EventHandler handler, ChannelType type)
                : base(apiVersion, handler, type) { }
            public U0x20CChannel(int apiVersion, EventHandler handler, ChannelType type, ChannelReadContext context)
                : base(apiVersion, handler, type, context) { }
        }

        public abstract class Channel<TFrame> : Channel
            where TFrame : Frame
        {

            protected Channel(int apiVersion, EventHandler handler, ChannelType type)
                : base(apiVersion, handler, type)
            {
                mFrames = new FrameList<TFrame>(handler);
            }
            protected Channel(int apiVersion, EventHandler handler, Channel<TFrame> basis)
                : base(apiVersion, handler, basis)
            {
                mFrames = new FrameList<TFrame>(handler);
                foreach (var frame in basis.mFrames)
                {
                    mFrames.Add((TFrame)Activator.CreateInstance(typeof(TFrame), new object[] { 0, handler, frame }));
                }
            }
            public Channel(int apiVersion, EventHandler handler, ChannelType type, ChannelReadContext context)
                : base(apiVersion, handler, type, context) { }
            private FrameList<TFrame> mFrames;
            [ElementPriority(3)]
            public FrameList<TFrame> Frames
            {
                get { return mFrames; }
                set { mFrames = value; OnElementChanged(); }
            }
            public override int FrameCount
            {
                get { return mFrames.Count; }
            }
            protected override void Parse(ChannelReadContext context)
            {
                mFrames = new FrameList<TFrame>(handler, context);
            }
            public override void UnParse(Stream s)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {

                return string.Format("Channel: 0x{0:X8}({1})[{2:0000}]", mBoneHash, (ChannelType)mType, mFrames.Count);
            }
        }
        public abstract class Channel : AHandlerElement, IEquatable<Channel>
        {
            protected UInt32 mBoneHash;
            protected ChannelType mType;
            [ElementPriority(1)]
            public uint BoneHash
            {
                get { return mBoneHash; }
                set { mBoneHash = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public ChannelType Type
            {
                get { return mType; }
            }

            public abstract Int32 FrameCount { get; }
            public static Channel CreateInstance(int apiVersion, EventHandler handler, ChannelReadContext ctx, ChannelType type)
            {
                switch (type)
                {
                    case ChannelType.PositionConst: return new PositionConstChannel(apiVersion, handler, type, ctx);
                    case ChannelType.Position: return new PositionChannel(apiVersion, handler, type, ctx);
                    case ChannelType.Rotation: return new RotationChannel(apiVersion, handler, type, ctx);
                    case ChannelType.PositionStatic: return new PositionStaticChannel(apiVersion, handler, type, ctx);
                    case ChannelType.U0x20C: return new U0x20CChannel(apiVersion, handler, type, ctx);
                    case ChannelType.U0x211: return new U0x211Channel(apiVersion, handler, type, ctx);
                    default: throw new NotImplementedException(String.Format("Frame type 0x{0} not implemented.", type));
                }
            }
            protected Channel(int apiVersion, EventHandler handler, ChannelType type)
                : base(apiVersion, handler)
            {
                mType = type;
            }
            protected Channel(int apiVersion, EventHandler handler, Channel basis)
                : base(apiVersion, handler)
            {
                mType = basis.mType;
                mBoneHash = basis.mBoneHash;
            }
            public Channel(int apiVersion, EventHandler handler, ChannelType type, ChannelReadContext context)
                : base(apiVersion, handler)
            {
                if (context != null)
                {
                    mType = type;
                    mBoneHash = context.BoneHash;
                    Parse(context);
                }
            }

            protected abstract void Parse(ChannelReadContext context);
            public abstract void UnParse(Stream s);



            public bool Equals(Channel other)
            {
                return base.Equals(other);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }
            public override string ToString()
            {

                return string.Format("Channel: 0x{0:X8}({1})", mBoneHash, (ChannelType)mType);
            }
        }
        #endregion

        #region Nested Type: Frame
        public abstract class Frame : AHandlerElement, IEquatable<Frame>
        {
            protected UInt16 mFrameIndex;
            protected Byte mFlags;
            protected Byte mExtraFlags;

            protected Frame(int apiVersion, EventHandler handler)
                : base(apiVersion, handler)
            {
            }
            protected Frame(int apiVersion, EventHandler handler, Frame basis)
                : base(apiVersion, handler)
            {
                mFrameIndex = basis.mFrameIndex;
                mFlags = basis.mFlags;
                mExtraFlags = basis.mExtraFlags;
                if((mFlags >>4 ) >0)
                {
                    int i = 0;
                }
            }
            public Frame(int apiVersion, EventHandler handler, ChannelReadContext ctx)
                : base(apiVersion, handler)
            {
                Parse(ctx);
            }
            [ElementPriority(1)]
            public ushort FrameIndex
            {
                get { return mFrameIndex; }
                set { mFrameIndex = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public byte Flags
            {
                get { return mFlags; }
                set { mFlags = value; OnElementChanged(); }
            }
            [ElementPriority(3)]
            public byte ExtraFlags
            {
                get { return mExtraFlags; }
                set { mExtraFlags = value; OnElementChanged(); }
            }

            public virtual void Parse(ChannelReadContext ctx)
            {
                BinaryReader br = new BinaryReader(ctx.Stream);
                mFrameIndex = br.ReadUInt16();
                mFlags = br.ReadByte();
                mExtraFlags = br.ReadByte();
            }
            public virtual void UnParse(ChannelWriteContext context)
            {
                BinaryWriter bw = new BinaryWriter(context.Stream);
                bw.Write(mFrameIndex);
                bw.Write(mFlags);
            }
            public override string ToString()
            {
                return String.Format("[{0:X2}]", mExtraFlags);
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override List<string> ContentFields
            {
                get { return GetContentFields(0, GetType()); }
            }

            public override int RecommendedApiVersion
            {
                get { return kRecommendedApiVersion; }
            }

            public bool Equals(Frame other)
            {
                return base.Equals(other);
            }
        }
        public class PositionConstFrame : Vector3Frame
        {
            public PositionConstFrame(int apiVersion, EventHandler handler, PositionConstFrame basis) : base(apiVersion, handler, basis) { }
            public PositionConstFrame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public PositionConstFrame(int apiVersion, EventHandler handler, ChannelReadContext ctx) : base(0, handler, ctx) { }

            public override void Parse(ChannelReadContext ctx)
            {
                base.Parse(ctx);
                BinaryReader br = new BinaryReader(ctx.Stream);
                int signX = ((mFlags & 0x01) == 0x01 ? -1 : 1);
                int signZ = ((mFlags & 0x02) == 0x02 ? -1 : 1);
                int signY = ((mFlags & 0x04) == 0x04 ? -1 : 1);
                mPos.X = ctx.Offset * signX + ctx.FloatConstants[br.ReadUInt16()] * ctx.Scalar;
                mPos.Z = ctx.Offset * signZ + ctx.FloatConstants[br.ReadUInt16()] * ctx.Scalar;
                mPos.Y = ctx.Offset * signY + ctx.FloatConstants[br.ReadUInt16()] * ctx.Scalar;

            }
            public override void UnParse(ChannelWriteContext context)
            {
                base.UnParse(context);
                BinaryWriter bw = new BinaryWriter(context.Stream);
            }
        }
        public class PositionFrame : Vector3Frame
        {
            public PositionFrame(int apiVersion, EventHandler handler, PositionFrame basis)
                : base(apiVersion, handler, basis)
            {
            }
            public PositionFrame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public PositionFrame(int apiVersion, EventHandler handler, ChannelReadContext ctx) : base(0, handler, ctx) { }
            

            public override void Parse(ChannelReadContext ctx)
            {
                base.Parse(ctx);
                BinaryReader br = new BinaryReader(ctx.Stream);
                UInt32 packed = br.ReadUInt32();
                int signX = ((mFlags & 0x01) == 0x01 ? -1 : 1);
                int signZ = ((mFlags & 0x02) == 0x02 ? -1 : 1);
                int signY = ((mFlags & 0x04) == 0x04 ? -1 : 1);
                mPos = new Vector3();
                mPos.X = ctx.Offset * signX + ((double)((packed & 0x000003FF) >> 0) / 1023) * ctx.Scalar;
                mPos.Z = ctx.Offset * signZ + ((double)((packed & 0x000FFC00) >> 10) / 1023) * ctx.Scalar;
                mPos.Y = ctx.Offset * signY + ((double)((packed & 0x3FF00000) >> 20) / 1023)  * ctx.Scalar;
                ctx.Floats.Add(mPos.X);
                ctx.Floats.Add(mPos.Y);
                ctx.Floats.Add(mPos.Z);
            }
            public override void UnParse(ChannelWriteContext context)
            {
                base.UnParse(context);
                BinaryWriter bw = new BinaryWriter(context.Stream);
            }

        }
        public abstract class Vector3Frame : Frame
        {
            protected Vector3Frame(int apiVersion, EventHandler handler, Vector3Frame basis)
                : base(apiVersion, handler, basis)
            {
                mPos = basis.mPos;
            }
            protected Vector3Frame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            protected Vector3Frame(int apiVersion, EventHandler handler, ChannelReadContext ctx) : base(0, handler, ctx) { }

            protected Vector3 mPos;
            [ElementPriority(4)]
            public Vector3 Pos
            {
                get { return mPos; }
                set { mPos = value; OnElementChanged(); }
            }
            public override string ToString()
            {
                return base.ToString() + mPos.ToString();
            }
        }
        public class RotationFrame : Frame
        {
            private double mX;
            private double mY;
            private double mZ;
            private double mW;
            public RotationFrame(int apiVersion, EventHandler handler, RotationFrame basis)
                : base(apiVersion, handler, basis)
            {
                mX = basis.mX;
                mY = basis.mY;
                mZ = basis.mZ;
                mW = basis.mW;
            }
            public RotationFrame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public RotationFrame(int apiVersion, EventHandler handler, ChannelReadContext ctx) : base(0, handler, ctx) { }
            [ElementPriority(4)]
            public Quaternion Quaternion
            {
                get { return new Quaternion(mX, mY, mZ, mW); }
                set
                {
                    mX = value.X;
                    mY = value.Y;
                    mZ = value.Z;
                    mW = value.W;
                    OnElementChanged();
                }
            }
            [ElementPriority(5)]
            public EulerAngle Euler
            {

                get { return new Quaternion(mX, mY, mZ, mW).ToEuler(); }
                set
                {
                    Quaternion q = value.ToQuaternion();
                    mX = q.X;
                    mY = q.Y;
                    mZ = q.Z;
                    mW = q.W;
                    OnElementChanged();
                }
            }

            public override void Parse(ChannelReadContext ctx)
            {
                base.Parse(ctx);
                BinaryReader br = new BinaryReader(ctx.Stream);
                double packedX = br.ReadUInt16();
                double packedY = br.ReadUInt16();
                double packedZ = br.ReadUInt16();
                double packedW = br.ReadUInt16();
                int signX = ((mFlags & 0x01) == 0x01 ? -1 : 1);
                int signY = ((mFlags & 0x02) == 0x02 ? -1 : 1);
                int signZ = ((mFlags & 0x04) == 0x04 ? -1 : 1);
                int signW = ((mFlags & 0x08) == 0x08 ? -1 : 1);
                mX = ctx.Offset * signX + (packedX / 4095) * ctx.Scalar;
                mY = ctx.Offset * signY + (packedY / 4095) * ctx.Scalar;
                mZ = ctx.Offset * signZ + (packedZ / 4095) * ctx.Scalar;
                mW = ctx.Offset * signW + (packedW / 4095) * ctx.Scalar;

            }
            public override void UnParse(ChannelWriteContext context)
            {
                base.UnParse(context);
                BinaryWriter bw = new BinaryWriter(context.Stream);
            }
            public override string ToString()
            {
                return base.ToString() + this.Quaternion.ToEuler().ToString();
            }

            public bool Equals(RotationFrame other)
            {
                return base.Equals(other);
            }
        }

        #endregion

        #region Fields
        private UInt32 mVersion;
        private UInt32 mUnknown01;
        private Single mFrameDuration;
        private ushort mUnknown02;
        private string mAnimName;
        private string mSrcName;
        private ChannelList mChannels;
        [ElementPriority(1)]
        public UInt32 Version { get { return mVersion; } set { mVersion = value; OnElementChanged(); } }
        [ElementPriority(2)]
        public UInt32 Unknown01 { get { return mUnknown01; } set { mUnknown01 = value; OnElementChanged(); } }
        [ElementPriority(3)]
        public Single FrameDuration { get { return mFrameDuration; } set { mFrameDuration = value; OnElementChanged(); } }
        [ElementPriority(4)]
        public ushort Unknown02 { get { return mUnknown02; } set { mUnknown02 = value; OnElementChanged(); } }
        [ElementPriority(5)]
        public String AnimName { get { return mAnimName; } set { mAnimName = value; OnElementChanged(); } }
        [ElementPriority(6)]
        public String SrcName { get { return mSrcName; } set { mSrcName = value; OnElementChanged(); } }
        [ElementPriority(7)]
        public ChannelList Channels { get { return mChannels; } set { mChannels = value; OnElementChanged(); } }
        #endregion

        #region Constructors
        public S3Clip(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
            mChannels = new ChannelList(handler);
        }
        public S3Clip(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
            s.Position = 0L;
            Parse(s);
        }
        #endregion

        #region I/O
        protected override void Parse(Stream s)
        {
            ClipReadContext context = new ClipReadContext(s);
            BinaryReader br = new BinaryReader(s);
            if (br.ReadUInt64() != 0x5F5333436C69705FUL) throw new Exception("Bad clip header: Expected \"_S3Clip_\"");
            mVersion = br.ReadUInt32();
            mUnknown01 = br.ReadUInt32();
            mFrameDuration = br.ReadSingle();
            context.MaxFrameCount = br.ReadUInt16();
            mUnknown02 = br.ReadUInt16();
            context.ChannelCount = br.ReadUInt32();
            UInt32 constFloatCount = br.ReadUInt32();
            long channelOffset = br.ReadUInt32();
            long constFloatOffset = br.ReadUInt32();
            long animNameOffset = br.ReadUInt32();
            long srcNameOffset = br.ReadUInt32();

            s.Seek(constFloatOffset, SeekOrigin.Begin);
            for (int i = 0; i < constFloatCount; i++) { context.FloatConstants.Add(br.ReadSingle()); }
            s.Seek(channelOffset, SeekOrigin.Begin);
            mChannels = new ChannelList(handler, context);
            s.Seek(animNameOffset, SeekOrigin.Begin);
            mAnimName = br.ReadZString();
            s.Seek(srcNameOffset, SeekOrigin.Begin);
            mSrcName = br.ReadZString();
        }
        /// <summary>
        /// Incomplete: Do not use
        /// </summary>
        public override void UnParse(Stream s)
        {
            ClipWriteContext context = new ClipWriteContext(s);
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(0x5F5333436C69705FUL); // _S3Clip_
            bw.Write(mVersion);
            bw.Write(mUnknown01);
            bw.Write(mFrameDuration);
            int maxFrameCount = 0;

            foreach (var channel in mChannels)
            {
                if (channel.FrameCount > maxFrameCount) maxFrameCount = channel.FrameCount;
                if (channel.Type == ChannelType.PositionConst)
                {
                    PositionConstChannel pcc = (PositionConstChannel)channel;
                    if (pcc.Frames.Count > maxFrameCount) maxFrameCount = pcc.Frames.Count;
                    foreach (var frame in pcc.Frames)
                    {
                        PositionConstFrame pcf = (PositionConstFrame) frame;
                        if (!context.FloatConstants.Contains(pcf.Pos.X))
                            context.FloatConstants.Add(pcf.Pos.X);
                        if (!context.FloatConstants.Contains(pcf.Pos.Y))
                            context.FloatConstants.Add(pcf.Pos.Y);
                        if (!context.FloatConstants.Contains(pcf.Pos.Z))
                            context.FloatConstants.Add(pcf.Pos.Z);

                    }
                }
            }
            bw.Write(maxFrameCount);
            bw.Write(mUnknown02);
            bw.Write(mChannels.Count);
            bw.Write(context.FloatConstants.Count);
            long offsetStart = s.Position;
            long channelOffset = 0;
            long floatsOffset = 0;
            long animNameOffset = 0;
            long srcNameOffset = 0;
            s.Seek(16L, SeekOrigin.Current); //skip over offsets

            channelOffset = s.Position;
            s.Seek(20L * mChannels.Count, SeekOrigin.Current); //skip channels

            animNameOffset = s.Position;
            bw.WriteZString(mAnimName);

            srcNameOffset = s.Position;
            bw.WriteZString(mSrcName);

            floatsOffset = s.Position;
            for (int i = 0; i < context.FloatConstants.Count; i++)
            {
                bw.Write(context.FloatConstants[i]);
            }
            for (int i = 0; i < mChannels.Count; i++)
            {

            }
            s.Seek(offsetStart, SeekOrigin.Begin);
            bw.Write((uint)channelOffset);
            bw.Write((uint)floatsOffset);
            bw.Write((uint)animNameOffset);
            bw.Write((uint)srcNameOffset);
        }
        #endregion
        const int kRecommendedApiVersion = 1;
    }

    public class ClipWriteContext
    {
        public ClipWriteContext(Stream s)
        {
            FloatConstants = new List<double>();
            FrameDataMap = new Dictionary<S3Clip.Channel, long>();
            Stream = s;
        }
        public Stream Stream { get; private set; }
        public List<double> FloatConstants { get; set; }
        public Dictionary<S3Clip.Channel, long> FrameDataMap { get; set; }
    }
    public class ChannelWriteContext : ClipWriteContext
    {
        public ChannelWriteContext(ClipWriteContext context)
            : base(context.Stream)
        {

        }

        public double Offset { get; set; }
        public double Scalar { get; set; }

    }
}
