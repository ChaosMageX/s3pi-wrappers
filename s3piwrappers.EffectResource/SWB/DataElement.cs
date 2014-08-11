using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers.SWB
{
    public abstract class DataElement : AHandlerElement
    {
        protected DataElement(int apiVersion, EventHandler handler, DataElement basis) 
            : base(apiVersion, handler)
        {
            var ms = new MemoryStream();
            basis.UnParse(ms);
            ms.Position = 0L;
            Parse(ms);
        }

        protected DataElement(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
        }

        protected DataElement(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler)
        {
            Parse(s);
        }

        protected abstract void Parse(Stream s);
        public abstract void UnParse(Stream s);

        public override List<string> ContentFields
        {
            get { return GetContentFields(base.requestedApiVersion, GetType()); }
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            return (AHandlerElement) Activator.CreateInstance(GetType(), 0, handler, this);
        }

        public virtual string Value
        {
            get { return ValueBuilder; }
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}
