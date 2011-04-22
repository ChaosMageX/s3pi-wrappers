using System;
using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class Track : AHandlerElement,
                         IEquatable<Track>
    {
        private CurveList mCurves;
        private UInt32 mTrackKey;
        public Track(int APIversion, EventHandler handler) : base(APIversion, handler) { mCurves = new CurveList(handler); }

        public Track(int APIversion, EventHandler handler, Track basis) : this(APIversion, handler, basis.mTrackKey, basis.Curves) { }

        public Track(int APIversion, EventHandler handler, UInt32 trackKey, IEnumerable<Curve> curves) : this(APIversion, handler)
        {
            mTrackKey = trackKey;
            mCurves = new CurveList(handler, curves);
        }

        [ElementPriority(1)]
        public uint TrackKey
        {
            get { return mTrackKey; }
            set
            {
                if (mTrackKey != value)
                {
                    mTrackKey = value;
                    OnElementChanged();
                }
            }
        }

        [ElementPriority(2)]
        public CurveList Curves
        {
            get { return mCurves; }
            set
            {
                mCurves = value;
                OnElementChanged();
            }
        }
        public override List<string> ContentFields { get { return GetContentFields(requestedApiVersion, GetType()); } }
        public override int RecommendedApiVersion { get { return 1; } }
        public string Value { get { return ValueBuilder; } }
        #region IEquatable<Track> Members
        public bool Equals(Track other) { return mTrackKey.Equals(other.mTrackKey); }
        #endregion
        public override AHandlerElement Clone(EventHandler handler) { return new Track(0, handler, this); }
    }
}