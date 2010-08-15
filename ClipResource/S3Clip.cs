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

    public enum TrackType
    {
        Position = 0x01,
        Orientation = 0x02
    }


    /// <summary>
    /// Animation frame data
    /// </summary>
    public class S3Clip : DependentElement
    {
        public class TrackList : AHandlerList<Track>, IGenericAdd
        {
            public TrackList(EventHandler handler)
                : base(handler)
            {
            }
            public TrackList(EventHandler handler, ClipReadContext ctx)
                : base(handler)
            {
                Parse(ctx);
            }
            private void Parse(ClipReadContext ctx)
            {

                BinaryReader br = new BinaryReader(ctx.Stream);
                for (int i = 0; i < ctx.TrackCount; i++)
                {
                    TrackReadContext trackContext = new TrackReadContext(ctx);
                    trackContext.FrameDataOffset = br.ReadUInt32();
                    trackContext.BoneHash = br.ReadUInt32();
                    trackContext.Offset = br.ReadSingle();
                    trackContext.Scalar = br.ReadSingle();
                    trackContext.FrameCount = br.ReadUInt16();
                    trackContext.Flags = new Track.DataFlags( br.ReadByte());
                    Track c = Track.CreateInstance(0, base.handler, trackContext, (TrackType)br.ReadByte());
                    base.Add(c);
                }
            }
            public void UnParse(Stream s)
            {

            }

            public bool Add(params object[] fields)
            {
                if (fields.Length == 0) return false;
                if (fields.Length == 1 && typeof(Track).IsAssignableFrom(fields[0].GetType()))
                {
                    base.Add((Track)fields[0]);
                    return true;
                }
                if (fields.Length == 1 && typeof(TrackType).IsAssignableFrom(fields[0].GetType()))
                {
                    base.Add((Track)Activator.CreateInstance(typeof(Track), new object[] { 0, handler, fields[0] }));
                }
                return false;
            }

            public void Add()
            {
                throw new NotImplementedException();
            }
        }
        #region Nested Type: Track
        public class FrameList<TFrame> : FrameList
            where TFrame : Frame
        {
            public FrameList(EventHandler handler)
                : base(handler)
            {
            }

            public FrameList(EventHandler handler, TrackReadContext ctx)
                : base(handler, ctx)
            {
            }

            protected override Frame CreateElement(int apiVersion, EventHandler handler, TrackReadContext context)
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
            public FrameList(EventHandler handler, TrackReadContext ctx)
                : base(handler)
            {
                Parse(ctx);
            }
            private void Parse(TrackReadContext context)
            {
                long pos = context.Stream.Position;
                context.Stream.Seek(context.FrameDataOffset, SeekOrigin.Begin);

                for (int i = 0; i < context.FrameCount; i++)
                {
                    base.Add(CreateElement(0, handler, context));
                }
                context.Stream.Seek(pos, SeekOrigin.Begin);
            }

            protected abstract Frame CreateElement(int apiVersion, EventHandler handler, TrackReadContext context);
            public void UnParse(TrackWriteContext context)
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
        [ConstructorParameters(new object[] { TrackType.Orientation })]
        public class OrientationTrack : Track<OrientationFrame>
        {
            public OrientationTrack(int apiVersion, EventHandler handler, TrackType type, OrientationTrack basis)
                : base(apiVersion, handler, basis) { }
            public OrientationTrack(int apiVersion, EventHandler handler, TrackType type)
                : base(apiVersion, handler, type) { }
            public OrientationTrack(int apiVersion, EventHandler handler, TrackType type, TrackReadContext context)
                : base(apiVersion, handler, type, context) { }
        }
        [ConstructorParameters(new object[] { TrackType.Position })]
        public class PositionTrack : Track<PositionFrame>
        {
            public PositionTrack(int apiVersion, EventHandler handler, TrackType type)
                : base(apiVersion, handler, type) { }
            public PositionTrack(int apiVersion, EventHandler handler, TrackType type, PositionTrack basis)
                : base(apiVersion, handler, basis) { }
            public PositionTrack(int apiVersion, EventHandler handler, TrackType type, TrackReadContext context)
                : base(apiVersion, handler, type, context) { }
        }


        public abstract class Track<TFrame> : Track
            where TFrame : Frame
        {

            protected Track(int apiVersion, EventHandler handler, TrackType type)
                : base(apiVersion, handler, type)
            {
                mFrames = new FrameList<TFrame>(handler);
            }
            protected Track(int apiVersion, EventHandler handler, Track<TFrame> basis)
                : base(apiVersion, handler, basis)
            {
                mFrames = new FrameList<TFrame>(handler);
                foreach (var frame in basis.mFrames)
                {
                    mFrames.Add((TFrame)Activator.CreateInstance(typeof(TFrame), new object[] { 0, handler, frame }));
                }
            }
            public Track(int apiVersion, EventHandler handler, TrackType type, TrackReadContext context)
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

            private uint fcount;
            protected override void Parse(TrackReadContext context)
            {
                fcount = context.FrameCount;
                mFrames = new FrameList<TFrame>(handler,context);
                Calc(context);
            }
            public override void UnParse(Stream s)
            {
                throw new NotImplementedException();
            }
            public override string Value
            {
                get
                {
                    StringBuilder sb = new StringBuilder(base.Value);
                    foreach(var f in mFrames)sb.AppendFormat("\n{0}",f.ToString());
                    return sb.ToString();
                }
            }
        }
        public abstract class Track : AHandlerElement, IEquatable<Track>
        {
            public enum DataType : byte
            {
                Float1 = 0x01,
                Float2 = 0x02,
                Float3 = 0x03,
                Float4 = 0x04
            }
            public enum DataFormat : byte
            {
                Indexed = 0x00,
                Packed = 0x01
            }
            public struct DataFlags
            {
                public DataFlags(byte flags) : this()
                {
                    Type = (DataType)((flags & (byte)0x07) >> 0);
                    Static = ((flags & (byte)0x08) >> 3) == 1?true:false;
                    Format = (DataFormat)((flags & (byte)0xF0) >> 4);
                }

                public DataType Type { get; set; }
                public Boolean Static { get; set; }
                public DataFormat Format { get; set; }
                public Byte Raw 
                { 
                    get { return (byte)((byte)Format<<4 | (byte)((Static?1:0)<<3) | (byte)Type<<0); } 
                }

            }
            protected UInt32 mBoneHash;
            protected TrackType mType;
            protected DataFlags mFlags;


            [ElementPriority(1)]
            public uint BoneHash
            {
                get { return mBoneHash; }
                set { mBoneHash = value; OnElementChanged(); }
            }
            [ElementPriority(2)]
            public DataFlags Flags
            {
                get { return mFlags; }
                set { mFlags = value; }
            }
            [ElementPriority(3)]
            public TrackType Type
            {
                get { return mType; }
            }
            public  virtual string Value
            {
                get { return ToString(); }
            }
            public abstract Int32 FrameCount { get; }
            public static Track CreateInstance(int apiVersion, EventHandler handler, TrackReadContext ctx, TrackType type)
            {
                switch (type)
                {
                    case TrackType.Position: return new PositionTrack(apiVersion, handler, type, ctx);
                    case TrackType.Orientation: return new OrientationTrack(apiVersion, handler, type, ctx);
                    default: throw new NotImplementedException(String.Format("Frame type 0x{0} not implemented.", type));
                }
            }
            protected Track(int apiVersion, EventHandler handler, TrackType type)
                : base(apiVersion, handler)
            {
                mType = type;
            }
            protected Track(int apiVersion, EventHandler handler, Track basis)
                : base(apiVersion, handler)
            {
                mType = basis.mType;
                mFlags = basis.mFlags;
                mBoneHash = basis.mBoneHash;
            }
            public Track(int apiVersion, EventHandler handler, TrackType type, TrackReadContext context)
                : base(apiVersion, handler)
            {
                if (context != null)
                {
                    mType = type;
                    mBoneHash = context.BoneHash;
                    mFlags = context.Flags;
                    Parse(context);
                }
            }

            protected abstract void Parse(TrackReadContext context);
            public abstract void UnParse(Stream s);



            public bool Equals(Track other)
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
                String s = string.Format("Track: 0x{0:X8}({1})({2})", mBoneHash, mType, mFlags.Format);
                if (mFlags.Static) s += "(Static)";
                return s;
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
            }
            public Frame(int apiVersion, EventHandler handler, TrackReadContext ctx)
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

            public virtual void Parse(TrackReadContext ctx)
            {
                BinaryReader br = new BinaryReader(ctx.Stream);
                mFrameIndex = br.ReadUInt16();
                mFlags = br.ReadByte();
                mExtraFlags = br.ReadByte();
                if (mExtraFlags != 0)
                {
                    int i = 1;
                }
            }
            public virtual void UnParse(TrackWriteContext context)
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

        static void Calc(TrackReadContext ctx)
        {
            var values = ctx.Floats;
            var max = values.Max();
            var maxAbs = Math.Abs(max);
            var sclMax = max / max;
            var sclMaxAbs = Math.Abs(sclMax);

            var min = values.Min();
            var minAbs = Math.Abs(min);
            var sclMin = min / max;
            var sclMinAbs = Math.Abs(min)/max;

            var rng = max - min;
            var rngAbs = maxAbs - minAbs;
            var sclRng = sclMax - sclMin;
            var sclAbsRng = sclMaxAbs - sclMinAbs;

            var med = rng / 2D;
            var medAbs = rngAbs / 2D;
            var sclMed = sclRng / 2D;
            var sclAbsMed = sclAbsRng / 2D;
            

            var avg = values.Average(x => x);
            var absAvg = values.Average(x => Math.Abs(x));
            var sclAvg = values.Average(x => x / max);
            var sclAbsAvg = values.Average(x => Math.Abs(x / max));

            var var = med - avg;
            var varAbs = medAbs - absAvg;
            var sclVar = sclMed - sclAvg;
            var sclAbsVar = sclAbsMed - sclAbsAvg;
            

        }
        public class PositionFrame : Vector3Frame
        {
            public PositionFrame(int apiVersion, EventHandler handler, PositionFrame basis)
                : base(apiVersion, handler, basis)
            {
            }
            public PositionFrame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public PositionFrame(int apiVersion, EventHandler handler, TrackReadContext ctx) : base(0, handler, ctx) { }


            //stub until recompression code is implemented
            private UInt32 mPacked;
            private UInt16 mIndexed0;
            private UInt16 mIndexed1;
            private UInt16 mIndexed2;
            
            private void ReadIndexed(TrackReadContext ctx)
            {
                base.Parse(ctx);
                BinaryReader br = new BinaryReader(ctx.Stream);
                int signX = ((mFlags & 0x01) == 0x01 ? -1 : 1);
                int signZ = ((mFlags & 0x02) == 0x02 ? -1 : 1);
                int signY = ((mFlags & 0x04) == 0x04 ? -1 : 1);
                ushort index0 = br.ReadUInt16();
                ushort index1 = br.ReadUInt16();
                ushort index2 = br.ReadUInt16();
                mX = ctx.Offset * signX + ctx.FloatConstants[index0] * ctx.Scalar;
                mZ = ctx.Offset * signZ + ctx.FloatConstants[index1] * ctx.Scalar;
                mY = ctx.Offset * signY + ctx.FloatConstants[index2] * ctx.Scalar;

                //stub until recompression code is implemented
                mIndexed0 = index0;
                mIndexed1 = index1;
                mIndexed2 = index2;
            }
            private void ReadPacked(TrackReadContext ctx)
            {
                BinaryReader br = new BinaryReader(ctx.Stream);
                UInt32 packed = br.ReadUInt32();
                
                int signX = ((mFlags & 0x01) == 0x01 ? -1 : 1);
                int signZ = ((mFlags & 0x02) == 0x02 ? -1 : 1);
                int signY = ((mFlags & 0x04) == 0x04 ? -1 : 1);
                mX = ctx.Offset * signX + ((double)((packed & 0x000003FF) >> 0) / 1023) * ctx.Scalar;
                mZ = ctx.Offset * signZ + ((double)((packed & 0x000FFC00) >> 10) / 1023) * ctx.Scalar;
                mY = ctx.Offset * signY + ((double)((packed & 0x3FF00000) >> 20) / 1023) * ctx.Scalar;


                //stub until recompression code is implemented
                mPacked = packed;

            }
            private void WriteIndexed(TrackWriteContext ctx)
            {
                //stub until recompression code is implemented
                BinaryWriter bw = new BinaryWriter(ctx.Stream);
                bw.Write(mIndexed0);
                bw.Write(mIndexed1);
                bw.Write(mIndexed2);
            }
            private void WritePacked(TrackWriteContext ctx)
            {
                //stub until recompression code is implemented
                BinaryWriter bw = new BinaryWriter(ctx.Stream);
                bw.Write(mPacked);
            }
            public override void Parse(TrackReadContext ctx)
            {
                base.Parse(ctx);
                switch(ctx.Flags.Format)
                {
                    case Track.DataFormat.Indexed:
                        ReadIndexed(ctx);
                        break;
                    case Track.DataFormat.Packed:
                        ReadPacked(ctx);
                        break;
                    default:
                        throw new Exception("Unable to parse format "+ctx.Flags.Format.ToString());
                }

                //for testing only
                ctx.Floats.Add(mX);
                ctx.Floats.Add(mY);
                ctx.Floats.Add(mZ);
                
            }
            public override void UnParse(TrackWriteContext context)
            {
                base.UnParse(context);
                BinaryWriter bw = new BinaryWriter(context.Stream);
                switch (context.Flags.Format)
                {
                    case Track.DataFormat.Indexed:
                        WriteIndexed(context);
                        break;
                    case Track.DataFormat.Packed:
                        WritePacked(context);
                        break;
                    default:
                        throw new Exception("Unable to parse format " + ctx.Flags.Format.ToString());
                }
            }

        }
        public abstract class Vector3Frame : Frame
        {
            protected Vector3Frame(int apiVersion, EventHandler handler, Vector3Frame basis)
                : base(apiVersion, handler, basis)
            {
            }
            protected Vector3Frame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            protected Vector3Frame(int apiVersion, EventHandler handler, TrackReadContext ctx) : base(0, handler, ctx) { }
            protected double mX;
            protected double mY;
            protected double mZ;
            [ElementPriority(4)]
            public double X
            {
                get { return mX; }
                set { mX = value; OnElementChanged(); }
            }
            [ElementPriority(5)]

            public double Y
            {
                get { return mY; }
                set { mY = value; OnElementChanged(); }
            }
            [ElementPriority(6)]

            public double Z
            {
                get { return mZ; }
                set { mZ = value; OnElementChanged(); }
            }

            public override string ToString()
            {
                return base.ToString() + String.Format("[{0,8:0.00000},{1,8:0.00000},{2,8:0.00000}]", mX, mY, mZ);
            }
        }
        public class OrientationFrame : Frame
        {
            private double mX;
            private double mY;
            private double mZ;
            private double mW;
            public OrientationFrame(int apiVersion, EventHandler handler, OrientationFrame basis)
                : base(apiVersion, handler, basis)
            {
                mX = basis.mX;
                mY = basis.mY;
                mZ = basis.mZ;
                mW = basis.mW;
            }
            public OrientationFrame(int apiVersion, EventHandler handler) : base(apiVersion, handler) { }
            public OrientationFrame(int apiVersion, EventHandler handler, TrackReadContext ctx) : base(0, handler, ctx) { }

            [ElementPriority(4)]
            public double X
            {
                get { return mX; }
                set { mX = value; OnElementChanged(); }
            }
            [ElementPriority(5)]
            public double Y
            {
                get { return mY; }
                set { mY = value; OnElementChanged(); }
            }
            [ElementPriority(6)]
            public double Z
            {
                get { return mZ; }
                set { mZ = value; OnElementChanged(); }
            }
            [ElementPriority(7)]
            public double W
            {
                get { return mW; }
                set { mW = value; OnElementChanged(); }
            }

            public override void Parse(TrackReadContext ctx)
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


                ctx.Floats.Add(mX);
                ctx.Floats.Add(mY);
                ctx.Floats.Add(mZ);
                ctx.Floats.Add(mW);

            }
            public override void UnParse(TrackWriteContext context)
            {
                base.UnParse(context);
                BinaryWriter bw = new BinaryWriter(context.Stream);
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

        #endregion

        #region Fields
        private UInt32 mVersion;
        private UInt32 mUnknown01;
        private Single mFrameDuration;
        private ushort mUnknown02;
        private string mAnimName;
        private string mSrcName;
        private TrackList mTracks;
        #endregion
        #region ContentFields
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
        public TrackList Tracks { get { return mTracks; } set { mTracks = value; OnElementChanged(); } }
        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Version:\t0x{0:X8}\n", mVersion);
                sb.AppendFormat("Unknown01:\t0x{0:X8}\n", mUnknown01);
                sb.AppendFormat("Frame Duration:\t{0:0.00000}\n", mFrameDuration);
                sb.AppendFormat("Unknown02:\t0x{0:X4}\n", mUnknown02);
                sb.AppendFormat("Animation Name:\t{0}\n", mAnimName);
                sb.AppendFormat("Source Name:\t{0}\n", mSrcName);
                if(mTracks.Count > 0)
                {
                    sb.AppendFormat("Tracks:\n");
                    foreach (var c in mTracks) sb.AppendFormat("{0}\n", c.Value);

                    
                }
                return sb.ToString();

                
            }
        }
        #endregion

        #region Constructors
        public S3Clip(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
            mTracks = new TrackList(handler);
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
            if (FOURCC(br.ReadUInt64()) != "_pilC3S_")
                throw new Exception("Bad clip header: Expected \"_S3Clip_\"");
            mVersion = br.ReadUInt32();
            mUnknown01 = br.ReadUInt32();
            mFrameDuration = br.ReadSingle();
            context.MaxFrameCount = br.ReadUInt16();
            mUnknown02 = br.ReadUInt16();
            context.TrackCount = br.ReadUInt32();
            UInt32 constFloatCount = br.ReadUInt32();
            long trackOffset = br.ReadUInt32();
            long constFloatOffset = br.ReadUInt32();
            long animNameOffset = br.ReadUInt32();
            long srcNameOffset = br.ReadUInt32();

            s.Seek(constFloatOffset, SeekOrigin.Begin);
            for (int i = 0; i < constFloatCount; i++) { context.FloatConstants.Add(br.ReadSingle()); }
            s.Seek(trackOffset, SeekOrigin.Begin);
            mTracks = new TrackList(handler, context);

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
            bw.Write(FOURCC("_S3Clip_"));
            bw.Write(mVersion);
            bw.Write(mUnknown01);
            bw.Write(mFrameDuration);
            int maxFrameCount = 0;

            //Count frames, and index float constants...
            foreach (var track in mTracks)
            {
                if (track.FrameCount > maxFrameCount) maxFrameCount = track.FrameCount;
                if (track.Flags.Format == Track.DataFormat.Indexed)
                {
                    PositionTrack positionTrack = track as PositionTrack;
                    if (positionTrack != null)
                    {
                        foreach (var frame in positionTrack.Frames)
                        {
                            PositionFrame positionFrame = (PositionFrame)frame;
                            if (!context.FloatConstants.Contains(positionFrame.X))
                                context.FloatConstants.Add(positionFrame.X);
                            if (!context.FloatConstants.Contains(positionFrame.Y))
                                context.FloatConstants.Add(positionFrame.Y);
                            if (!context.FloatConstants.Contains(positionFrame.Z))
                                context.FloatConstants.Add(positionFrame.Z);

                        }
                    }
                    OrientationTrack orientationTrack = track as OrientationTrack;
                    if (orientationTrack != null)
                    {
                        foreach (var frame in orientationTrack.Frames)
                        {
                            OrientationFrame orientationFrame = (OrientationFrame)frame;
                            if (!context.FloatConstants.Contains(orientationFrame.X))
                                context.FloatConstants.Add(orientationFrame.X);
                            if (!context.FloatConstants.Contains(orientationFrame.Y))
                                context.FloatConstants.Add(orientationFrame.Y);
                            if (!context.FloatConstants.Contains(orientationFrame.Z))
                                context.FloatConstants.Add(orientationFrame.Z);
                            if (!context.FloatConstants.Contains(orientationFrame.W))
                                context.FloatConstants.Add(orientationFrame.W);

                        }
                    }
                }
            }
            bw.Write(maxFrameCount);
            bw.Write(mUnknown02);
            bw.Write(mTracks.Count);
            bw.Write(context.FloatConstants.Count);
            long offsetStart = s.Position;
            long trackOffset = 0;
            long floatsOffset = 0;
            long animNameOffset = 0;
            long srcNameOffset = 0;
            s.Seek(16L, SeekOrigin.Current); //skip over offsets

            trackOffset = s.Position;
            s.Seek(20L * mTracks.Count, SeekOrigin.Current); //skip over tracks

            animNameOffset = s.Position;
            bw.WriteZString(mAnimName);

            srcNameOffset = s.Position;
            bw.WriteZString(mSrcName);

            floatsOffset = s.Position;
            for (int i = 0; i < context.FloatConstants.Count; i++)
            {
                bw.Write(context.FloatConstants[i]);
            }
            for (int i = 0; i < mTracks.Count; i++)
            {

            }
            s.Seek(offsetStart, SeekOrigin.Begin);
            bw.Write((uint)trackOffset);
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
            FrameDataMap = new Dictionary<S3Clip.Track, long>();
            Stream = s;
        }
        public Stream Stream { get; private set; }
        public List<double> FloatConstants { get; set; }
        public Dictionary<S3Clip.Track, long> FrameDataMap { get; set; }
    }
    public class TrackWriteContext : ClipWriteContext
    {
        public TrackWriteContext(ClipWriteContext context)
            : base(context.Stream)
        {
            FloatConstants = context.FloatConstants;
            FrameDataMap = context.FrameDataMap;
        }

        public double Offset { get; set; }
        public double Scalar { get; set; }
        public S3Clip.Track.DataFlags Flags { get; set; }

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
        public UInt32 TrackCount { get; set; }
        public List<Single> FloatConstants { get; set; }
    }

    public class TrackReadContext : ClipReadContext
    {
        public ClipReadContext ClipContext { get; private set; }
        public TrackReadContext(ClipReadContext ctx)
            : base(ctx.Stream)
        {
            ClipContext = ctx;
            FloatConstants = ctx.FloatConstants;
            Floats = new List<double>();
        }

        public S3Clip.Track.DataFlags Flags { get; set; }
        public List<double> Floats { get; set; }
        public long FrameDataOffset { get; set; }
        public uint FrameCount { get; set; }
        public uint BoneHash { get; set; }
        public Single Offset { get; set; }
        public Single Scalar { get; set; }
    }
}
