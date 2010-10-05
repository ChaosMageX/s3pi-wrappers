using System;
using System.Collections.Generic;
using s3pi.Interfaces;

namespace s3piwrappers.Granny2
{
    public abstract class GrannyElement : AHandlerElement
    {
        protected GrannyElement(int APIversion, EventHandler handler)
            : base(APIversion, handler) { }
        public virtual string Value { get { return ToString(); } }
        
        public override AHandlerElement Clone(EventHandler handler)
        {
            return (AHandlerElement)Activator.CreateInstance(GetType(), new object[] {0, handler, this});
        }
        public override List<string> ContentFields
        {
            get { return GetContentFields(requestedApiVersion,GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
    public class GrannyElementList<TElement> : AResource.DependentList<TElement>
        where TElement : GrannyElement,IEquatable<TElement>
    {
        public GrannyElementList(EventHandler handler) : base(handler) {}
        public GrannyElementList(EventHandler handler, long size) : base(handler, size) {}
        public GrannyElementList(EventHandler handler, IList<TElement> ilt) : base(handler, ilt) {}
        public GrannyElementList(EventHandler handler, long size, IList<TElement> ilt) : base(handler, size, ilt) {}

        public override void Add()
        {
            base.Add(new object[] {});
        }

        #region Unused
        public override void UnParse(System.IO.Stream s)
        {
            throw new NotSupportedException();
        }
        protected override void Parse(System.IO.Stream s)
        {
            throw new NotSupportedException();
        }
        protected override TElement CreateElement(System.IO.Stream s)
        {
            throw new NotSupportedException();
        }
        protected override void WriteElement(System.IO.Stream s, TElement element)
        {
            throw new NotSupportedException();
        } 
        #endregion
    }
}