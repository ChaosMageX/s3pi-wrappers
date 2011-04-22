using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers
{
    [ConstructorParameters(new object[] {CurveType.Orientation})]
    public class OrientationCurve : Curve
    {
        public OrientationCurve(int apiVersion, EventHandler handler, OrientationCurve basis) : base(apiVersion, handler, basis) { }
        public OrientationCurve(int apiVersion, EventHandler handler, CurveType type) : base(apiVersion, handler, type) { }
        public OrientationCurve(int apiVersion, EventHandler handler, CurveType type, Stream s, CurveDataInfo info, IList<float> indexedFloats) : base(apiVersion, handler, type, s, info, indexedFloats) { }
    }
}