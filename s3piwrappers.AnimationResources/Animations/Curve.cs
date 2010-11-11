using System;
using System.IO;
using System.Text;
using System.Linq;
using s3pi.Interfaces;
using System.Collections.Generic;

namespace s3piwrappers
{
    public abstract class Curve : AHandlerElement, IEquatable<Curve>
    {
        protected Curve(int apiVersion, EventHandler handler, CurveType type)
            : base(apiVersion, handler)
        {
            mType = type;
        }
        protected Curve(int apiVersion, EventHandler handler, Curve basis)
            : this(apiVersion, handler,basis.mType)
        {
            mFlags = basis.mFlags;
            mFrames = new FrameList(handler,mType,basis.Frames);
        }
        public Curve(int apiVersion, EventHandler handler, CurveType type, Stream s, CurveDataInfo info, IList<float> indexedFloats)
            : this(apiVersion, handler,type)
        {
            mFlags = info.Flags;
            Parse(s, info, indexedFloats);
        }
        protected CurveType mType;
        protected CurveDataFlags mFlags;
        protected FrameList mFrames;
        public static Type GetCurveType(CurveType t)
        {
            switch (t)
            {
                case CurveType.Position: return typeof(PositionCurve);
                case CurveType.Orientation: return typeof(OrientationCurve);
                default: throw new NotSupportedException("Curve Type " + t.ToString() + " is not supported.");
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
                default: throw new NotSupportedException("Curve Type " + t.ToString() + " is not supported.");
            }
        }
        public static Curve CreateInstance(int apiVersion,EventHandler handler,CurveType t)
        {
            switch (t)
            {
                case CurveType.Position:
                    return new PositionCurve(apiVersion, handler, t);
                case CurveType.Orientation:
                    return new OrientationCurve(apiVersion, handler, t);
                default: throw new NotSupportedException("Curve Type " + t.ToString() + " is not supported.");
            }
        }

        [ElementPriority(2)]
        public CurveDataFlags Flags
        {
            get { return mFlags; }
            set { mFlags = value; }
        }
        [ElementPriority(3)]
        public CurveType Type
        {
            get { return mType; }
        }
        [ElementPriority(4)]
        public FrameList Frames
        {
            get { return mFrames; }
            set { mFrames = value; OnElementChanged(); }
        }
        protected virtual void Parse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            mFrames = new FrameList(handler,mType,s,info,indexedFloats);
            var vals = SelectFloats();
            var a = vals.Select(x => Math.Abs(x));
            var min = a.Min();
            var max = a.Max();
            var rng = max - min;
            var med = rng/2f;
            var scl = med + min;
        }
        public virtual void UnParse(Stream s, CurveDataInfo info, IList<float> indexedFloats)
        {
            mFrames.UnParse(s, info, indexedFloats);
        }
        protected virtual float CalculateBase(IEnumerable<float> src)
        {
            return src.Select(x => Math.Abs(x)).Average();
        }
        protected virtual float CalculateOffset(IEnumerable<float> src)
        {
            return 0f;
        }

        protected virtual IEnumerable<float> SelectFloats()
        {
            var floats = new List<float>();
            foreach(var f in mFrames)floats.AddRange(f.GetFloatValues());
            return floats;
        }

        public bool Equals(Curve other)
        {
            return base.Equals(other);
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return (AHandlerElement)Activator.CreateInstance(GetType(), new object[] { 0, handler, this });
        }

        public override List<string> ContentFields
        {
            get { return GetContentFields(0, GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
        public override string ToString()
        {
            String s = string.Format("Curve: ({0})({1})", mType, mFlags.Format);
            if (mFlags.Static) s += "(Static)";
            return s;
        }

        public virtual string Value
        {
            get
            {

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Type:\t{0}\n", mType);
                sb.AppendFormat("DataType:\t{0}\n", mFlags.Type);
                sb.AppendFormat("Format:\t{0}\n", mFlags.Format);
                sb.AppendFormat("Static:\t{0}\n", mFlags.Static);

                if (mFrames.Count > 0)
                {
                    sb.AppendFormat("Frames:\n");
                    for (int i = 0; i < mFrames.Count; i++)
                    {
                        sb.AppendFormat("Frame[{0}]:{1}\n",i, mFrames[i].ToString());
                    }
                }
                return sb.ToString();
            }
        }
    }
}