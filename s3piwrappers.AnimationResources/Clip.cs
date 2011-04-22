using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using s3pi.Interfaces;
using s3pi.Settings;

namespace s3piwrappers
{
    /// <summary>
    ///   Animation frame data
    /// </summary>
    public class Clip : AHandlerElement
    {
        #region Fields

        private String mAnimName;
        private Single mFrameDuration;
        private UInt16 mMaxFrameCount;
        private String mSrcName;
        private TrackList mTracks;
        private UInt32 mUnknown01;
        private UInt16 mUnknown02;
        private UInt32 mVersion;

        #endregion

        #region ContentFields

        [ElementPriority(1)]
        public UInt32 Version
        {
            get { return mVersion; }
            set
            {
                mVersion = value;
                OnElementChanged();
            }
        }

        [ElementPriority(2)]
        public UInt32 Unknown01
        {
            get { return mUnknown01; }
            set
            {
                mUnknown01 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(3)]
        public Single FrameDuration
        {
            get { return mFrameDuration; }
            set
            {
                mFrameDuration = value;
                OnElementChanged();
            }
        }

        [ElementPriority(4)]
        public UInt16 MaxFrameCount
        {
            get { return mMaxFrameCount; }
            set
            {
                mMaxFrameCount = value;
                OnElementChanged();
            }
        }

        [ElementPriority(5)]
        public UInt16 Unknown02
        {
            get { return mUnknown02; }
            set
            {
                mUnknown02 = value;
                OnElementChanged();
            }
        }

        [ElementPriority(6)]
        public String AnimName
        {
            get { return mAnimName; }
            set
            {
                mAnimName = value;
                OnElementChanged();
            }
        }

        [ElementPriority(7)]
        public String SrcName
        {
            get { return mSrcName; }
            set
            {
                mSrcName = value;
                OnElementChanged();
            }
        }

        [ElementPriority(8)]
        public TrackList Tracks
        {
            get { return mTracks; }
            set
            {
                mTracks = value;
                OnElementChanged();
            }
        }

        public string Value { get { return ValueBuilder; } }

        #endregion

        #region Constructors

        public Clip(int apiVersion, EventHandler handler) : base(apiVersion, handler) { mTracks = new TrackList(handler); }

        public Clip(int apiVersion, EventHandler handler, Stream s) : base(apiVersion, handler)
        {
            s.Position = 0L;
            Parse(s);
        }

        #endregion

        #region I/O

        protected void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            var foo = FOURCC(br.ReadUInt64());
            if (foo != "_pilC3S_")
                throw new Exception("Bad clip header: Expected \"_S3Clip_\"");
            mVersion = br.ReadUInt32();
            mUnknown01 = br.ReadUInt32();
            mFrameDuration = br.ReadSingle();
            mMaxFrameCount = br.ReadUInt16();
            mUnknown02 = br.ReadUInt16();

            uint curveCount = br.ReadUInt32();
            uint indexedFloatCount = br.ReadUInt32();
            uint curveDataOffset = br.ReadUInt32();
            uint frameDataOffset = br.ReadUInt32();
            uint animNameOffset = br.ReadUInt32();
            uint srcNameOffset = br.ReadUInt32();

            if (Settings.Checking && s.Position != curveDataOffset)
                throw new InvalidDataException("Bad Curve Data Offset");

            List<CurveDataInfo> curveDataInfos = new List<CurveDataInfo>();
            for (int i = 0; i < curveCount; i++)
            {
                CurveDataInfo p = new CurveDataInfo();
                p.FrameDataOffset = br.ReadUInt32();
                p.TrackKey = br.ReadUInt32();
                p.Offset = br.ReadSingle();
                p.Scale = br.ReadSingle();
                p.FrameCount = br.ReadUInt16();
                p.Flags = new CurveDataFlags(br.ReadByte());
                p.Type = (CurveType) br.ReadByte();
                curveDataInfos.Add(p);
            }

            if (Settings.Checking && s.Position != animNameOffset)
                throw new InvalidDataException("Bad Name Offset");
            mAnimName = ClipResource.ReadZString(br);
            if (Settings.Checking && s.Position != srcNameOffset)
                throw new InvalidDataException("Bad SourceName Offset");
            mSrcName = ClipResource.ReadZString(br);

            if (Settings.Checking && s.Position != frameDataOffset)
                throw new InvalidDataException("Bad Indexed Floats Offset");
            List<float> indexedFloats = new List<float>();
            for (int i = 0; i < indexedFloatCount; i++)
            {
                indexedFloats.Add(br.ReadSingle());
            }

            Dictionary<uint, List<Curve>> trackMap = new Dictionary<uint, List<Curve>>();
            for (int i = 0; i < curveDataInfos.Count; i++)
            {
                CurveDataInfo curveDataInfo = curveDataInfos[i];
                if (Settings.Checking && s.Position != curveDataInfo.FrameDataOffset)
                    throw new InvalidDataException("Bad FrameData offset.");
                Curve c = Curve.CreateInstance(0, handler, curveDataInfo.Type, s, curveDataInfo, indexedFloats);
                if (!trackMap.ContainsKey(curveDataInfo.TrackKey)) trackMap[curveDataInfo.TrackKey] = new List<Curve>();
                trackMap[curveDataInfo.TrackKey].Add(c);
            }

            List<Track> tracks = new List<Track>();
            foreach (var k in trackMap.Keys)
            {
                tracks.Add(new Track(0, handler, k, trackMap[k]));
            }
            mTracks = new TrackList(handler, tracks);

            if (Settings.Checking && s.Position != s.Length)
                throw new InvalidDataException("Unexpected End of Clip.");
        }

        public void UnParse(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);
            bw.Write(Encoding.ASCII.GetBytes("_pilC3S_"));
            bw.Write(mVersion);
            bw.Write(mUnknown01);
            bw.Write(mFrameDuration);
            bw.Write(mMaxFrameCount);
            bw.Write(Unknown02);

            List<float> indexedFloats = new List<float>();
            List<CurveDataInfo> curveDataInfos = new List<CurveDataInfo>();


            UInt32 curveCount = 0;
            byte[] frameData;
            using (var frameStream = new MemoryStream())
            {
                foreach (var track in Tracks)
                {
                    foreach (var curve in track.Curves)
                    {
                        curveCount++;
                        Single scale, offset;
                        var values = curve.SelectFloats();
                        CalculateScaleOffset(values, out scale, out offset);
                        Boolean isIndexed = curve.Frames.Count == 0 ? true : curve.Type == CurveType.Position ? IsIndexed(curve.Frames.Cast<Float3Frame>()) : false;

                        var flags = new CurveDataFlags();
                        flags.Format = isIndexed ? CurveDataFormat.Indexed : CurveDataFormat.Packed;
                        flags.Type = curve.Type == CurveType.Position ? CurveDataType.Float3 : CurveDataType.Float4;
                        flags.Static = curve.Frames.Count == 0;

                        var curveDataInfo = new CurveDataInfo {Offset = offset, Flags = flags, FrameCount = curve.Frames.Count, FrameDataOffset = (UInt32) frameStream.Position, Scale = scale, TrackKey = track.TrackKey, Type = curve.Type};
                        curve.UnParse(frameStream, curveDataInfo, indexedFloats);
                        curveDataInfos.Add(curveDataInfo);
                    }
                }
                frameData = frameStream.ToArray();
            }

            bw.Write(curveCount);
            bw.Write(indexedFloats.Count);
            long offsets = s.Position;
            uint curveDataOffset = 0;
            uint frameDataOffset = 0;
            uint animNameOffset = 0;
            uint srcNameOffset = 0;
            s.Seek(4*sizeof (UInt32), SeekOrigin.Current);


            curveDataOffset = (uint) s.Position;
            uint frameOffset = (uint) (curveDataOffset + (20*curveDataInfos.Count) + mAnimName.Length + mSrcName.Length + 2 + (sizeof (Single)*indexedFloats.Count));
            foreach (var curveDataInfo in curveDataInfos)
            {
                bw.Write((curveDataInfo.FrameDataOffset + frameOffset));
                bw.Write(curveDataInfo.TrackKey);
                bw.Write(curveDataInfo.Offset);
                bw.Write(curveDataInfo.Scale);
                bw.Write((UInt16) curveDataInfo.FrameCount);
                bw.Write(curveDataInfo.Flags.Raw);
                bw.Write((Byte) curveDataInfo.Type);
            }

            animNameOffset = (uint) s.Position;
            ClipResource.WriteZString(bw, AnimName);
            srcNameOffset = (uint) s.Position;
            ClipResource.WriteZString(bw, SrcName);

            frameDataOffset = (uint) s.Position;
            foreach (var f in indexedFloats) bw.Write(f);
            bw.Write(frameData);
            s.Seek(offsets, SeekOrigin.Begin);
            bw.Write(curveDataOffset);
            bw.Write(frameDataOffset);
            bw.Write(animNameOffset);
            bw.Write(srcNameOffset);
            s.Position = s.Length;
        }

        #endregion

        private const int kRecommendedApiVersion = 1;
        public override List<string> ContentFields { get { return GetContentFields(0, GetType()); } }

        public override int RecommendedApiVersion { get { return 1; } }

        private static void CalculateScaleOffset(IEnumerable<Single> source, out Single scale, out Single offset)
        {
            if (!source.Any())
            {
                scale = 0f;
                offset = 0f;
                return;
            }
            var min = source.Min();
            var max = source.Max();
            offset = min;
            scale = min/max;
            return;
        }

        private static Boolean IsIndexed(IEnumerable<Float3Frame> source)
        {
            if (source.Count() < 5) return false;
            var x = source.Select(frame => frame.Data[0]).Distinct();
            var y = source.Select(frame => frame.Data[0]).Distinct();
            var z = source.Select(frame => frame.Data[0]).Distinct();

            return x.Count() == 1 && x.First() == 0 || y.Count() == 1 && y.First() == 0 || z.Count() == 1 && z.First() == 0;
        }

        public override AHandlerElement Clone(EventHandler handler) { throw new NotImplementedException(); }
    }
}