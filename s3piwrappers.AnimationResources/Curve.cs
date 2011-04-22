using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public abstract class Curve : AHandlerElement,
                                  IEquatable<Curve>
    {
        protected FrameList mFrames;
        protected CurveType mType;

        protected Curve(int apiVersion, EventHandler handler, CurveType type) : base(apiVersion, handler)
        {
            mType = type;
            mFrames = new FrameList(handler, type);
        }

        protected Curve(int apiVersion, EventHandler handler, Curve basis) : this(apiVersion, handler, basis.mType) { mFrames = new FrameList(handler, mType, basis.Frames); }

        public Curve(int apiVersion, EventHandler handler, CurveType type, Stream s, CurveDataInfo info, IList<float> indexedFloats) : this(apiVersion, handler, type) { Parse(s, info, indexedFloats); }

        [ElementPriority(2)]
        public CurveType Type { get { return mType; } }

        [ElementPriority(3)]
        public FrameList Frames
        {
            get { return mFrames; }
            set
            {
                if (mFrames != value)
                {
                    mFrames = value;
                    OnElementChanged();
                }
            }
        }

        public override List<string> ContentFields { get { return GetContentFields(0, GetType()); } }

        public override int RecommendedApiVersion { get { return 1; } }
        public virtual string Value { get { return ValueBuilder; } }

        #region IEquatable<Curve> Members

        public bool Equals(Curve other) { return base.Equals(other); }

        #endregion

        public static Type GetCurveType(CurveType t)
        {
            switch (t)
            {
                case CurveType.Position:
                    return typeof (PositionCurve);
                case CurveType.Orientation:
                    return typeof (OrientationCurve);
                default:
                    throw new NotSupportedException("Curve Type " + t + " is not supported.");
            }
        }

        public static Curve CreateInstance(int apiVersion, EventHandler handler, CurveType t, Stream s, CurveDataInfo info, IList<float> floats)
        {
            switch (t)
            {
                case CurveType.Position:
                    return new PositionCurve(apiVersion, handler, t, s, info, floats);
                case CurveType.Orientation:
                    return new OrientationCurve(apiVersion, handler, t, s, info, floats);
                default:
                    throw new NotSupportedException("Curve Type " + t + " is not supported.");
            }
        }

        public static Curve CreateInstance(int apiVersion, EventHandler handler, CurveType t)
        {
            switch (t)
            {
                case CurveType.Position:
                    return new PositionCurve(apiVersion, handler, t);
                case CurveType.Orientation:
                    return new OrientationCurve(apiVersion, handler, t);
                default:
                    throw new NotSupportedException("Curve Type " + t + " is not supported.");
            }
        }

        protected virtual void Parse(Stream s, CurveDataInfo info, IList<float> indexedFloats) { mFrames = new FrameList(handler, mType, s, info, indexedFloats); }
        public virtual void UnParse(Stream s, CurveDataInfo info, IList<float> indexedFloats) { mFrames.UnParse(s, info, indexedFloats); }
        public override AHandlerElement Clone(EventHandler handler) { return (AHandlerElement) Activator.CreateInstance(GetType(), new object[] {0, handler, this}); }

        public virtual IEnumerable<float> SelectFloats()
        {
            var floats = new List<float>();
            foreach (var f in mFrames) floats.AddRange(f.GetFloatValues());
            return floats;
        }
    }
}