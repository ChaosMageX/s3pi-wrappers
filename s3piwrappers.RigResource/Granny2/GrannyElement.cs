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
            return (AHandlerElement)Activator.CreateInstance(GetType(), new object[] { 0, handler, this });
        }
        public override List<string> ContentFields
        {
            get { return GetContentFields(requestedApiVersion, GetType()); }
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}