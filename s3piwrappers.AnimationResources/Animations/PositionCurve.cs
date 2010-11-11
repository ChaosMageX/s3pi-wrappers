using System;
using s3pi.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace s3piwrappers
{
    [ConstructorParameters(new object[] { CurveType.Position })]
    public class PositionCurve : Curve
    {
        public PositionCurve(int apiVersion, EventHandler handler, CurveType type)
            : base(apiVersion, handler, type) { }
        public PositionCurve(int apiVersion, EventHandler handler, CurveType type, PositionCurve basis)
            : base(apiVersion, handler, basis) { }
        public PositionCurve(int apiVersion, EventHandler handler, CurveType type, Stream s, CurveDataInfo info, IList<float> indexedFloats)
            : base(apiVersion, handler, type, s, info, indexedFloats) { }
    }
}