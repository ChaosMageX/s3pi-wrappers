using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers
{

    public class CurveList : DependentList<Curve>
    {
        public CurveList(EventHandler handler) : base(handler) {}
        public CurveList(EventHandler handler, IList<Curve> ilt) : base(handler, ilt) {}
        public CurveList(EventHandler handler, long size) : base(handler, size) {}
        
        #region Unused
        public override void Add()
        {
            throw new NotSupportedException();
        }

        protected override Curve CreateElement(Stream s)
        {
            throw new NotSupportedException();
        }

        protected override void WriteElement(Stream s, Curve element)
        {
            throw new NotSupportedException();
        } 
        #endregion
    }
}