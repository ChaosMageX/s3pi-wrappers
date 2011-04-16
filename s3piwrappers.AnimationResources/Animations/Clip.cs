using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using s3pi.Interfaces;
using System.Reflection;
using System.Globalization;
using s3pi.Settings;

namespace s3piwrappers
{
    /// <summary>
    /// Animation frame data
    /// </summary>
    public class Clip : AHandlerElement
    {

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
                if (mTracks.Count > 0)
                {
                    sb.AppendFormat("Tracks:\n");
                    foreach (var c in mTracks) sb.AppendFormat("{0}\n", c.Value);


                }
                return sb.ToString();


            }
        }
        #endregion

        #region Constructors
        public Clip(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
            mTracks = new TrackList(handler);
        }
        public Clip(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler)
        {
            s.Position = 0L;
            Parse(s);
        }
        #endregion

        #region I/O
        protected void Parse(Stream s)
        {
            BinaryReader br = new BinaryReader(s);
            if (FOURCC(br.ReadUInt64()) != "_pilC3S_")
                throw new Exception("Bad clip header: Expected \"_S3Clip_\"");
            mVersion = br.ReadUInt32();
            mUnknown01 = br.ReadUInt32();
            mFrameDuration = br.ReadSingle();
            var maxFrameCount = br.ReadUInt16();
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
                p.Base = br.ReadSingle();
                p.Scale = br.ReadSingle();
                p.FrameCount = br.ReadUInt16();
                p.Flags = new CurveDataFlags(br.ReadByte());
                p.Type = (CurveType)br.ReadByte();
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
            for (int i = 0; i < indexedFloatCount; i++) { indexedFloats.Add(br.ReadSingle()); }

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
            bw.Write(FOURCC("_S3Clip_"));
            bw.Write(mVersion);
            bw.Write(mUnknown01);
            bw.Write(mFrameDuration);
            List<float> indexedFloats = new List<float>();
            //TODO: Index, compress float data
            throw new NotImplementedException("Writing Clips is not yet implemented.");

        }
        #endregion
        const int kRecommendedApiVersion = 1;

        public override AHandlerElement Clone(EventHandler handler)
        {
            throw new NotImplementedException();
        }

        public override List<string> ContentFields
        {
            get { throw new NotImplementedException(); }
        }

        public override int RecommendedApiVersion
        {
            get { throw new NotImplementedException(); }
        }
    }
}
