using System;
using s3pi.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace s3piwrappers
{

    public class Track : AHandlerElement, IEquatable<Track>
    {
        public Track(int APIversion, EventHandler handler) : base(APIversion, handler) { }
        public Track(int APIversion, EventHandler handler, Track basis) : this(APIversion, handler, basis.mTrackKey, basis.Curves) { }
        public Track(int APIversion, EventHandler handler, UInt32 trackKey, IList<Curve> curves)
            : this(APIversion, handler)
        {
            mTrackKey = trackKey;
            mCurves = new CurveList(handler, curves);
        }

        private UInt32 mTrackKey;
        private CurveList mCurves;

        /// <summary>
        /// Hashed bone name from the Rig
        /// </summary>
        [ElementPriority(1)]
        public uint TrackKey
        {
            get { return mTrackKey; }
            set { mTrackKey = value; OnElementChanged(); }
        }
        [ElementPriority(2)]
        public CurveList Curves { get { return mCurves; } set { mCurves = value; OnElementChanged(); } }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return new Track(0, handler, this);
        }

        public override List<string> ContentFields
        {
            get { return GetContentFields(requestedApiVersion, GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
        public string Value
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Key:\t0x{0:X8}\n", mTrackKey);
                if (mCurves.Count > 0)
                {
                    sb.AppendFormat("Curves:\n");
                    for (int i = 0; i < mCurves.Count; i++)
                    {
                        sb.AppendFormat("Curve[{0}]:\n{1}\n",i, mCurves[i].Value);
                    }
                }
                return sb.ToString();
            }
        }

        public bool Equals(Track other)
        {
            return mTrackKey.Equals(other.mTrackKey);
        }
    }
}