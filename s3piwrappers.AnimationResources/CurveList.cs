using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public class CurveList : DependentList<Curve>
    {
        public CurveList(EventHandler handler, Stream s) : base(handler, s) { }

        public CurveList(EventHandler handler, IEnumerable<Curve> ilt) : base(handler, ilt) { }

        public CurveList(EventHandler handler) : base(handler) { }

        public override void Add() { throw new NotImplementedException(); }

        public override bool Add(params object[] fields)
        {
            if (fields.Length > 0 && typeof (Curve).IsAssignableFrom(fields[0].GetType()))
            {
                ((IList<Curve>) this).Add((Curve) fields[0]);
            }
            return base.Add(fields);
        }

        protected override Type GetElementType(params object[] fields)
        {
            if (fields.Length > 0 && typeof (Curve).IsAssignableFrom(fields[0].GetType()))
            {
                return Curve.GetCurveType(((Curve) fields[0]).Type);
            }
            return base.GetElementType(fields);
        }

        #region Unused

        protected override Curve CreateElement(Stream s) { throw new NotSupportedException(); }

        protected override void WriteElement(Stream s, Curve element) { throw new NotSupportedException(); }

        #endregion
    }
}