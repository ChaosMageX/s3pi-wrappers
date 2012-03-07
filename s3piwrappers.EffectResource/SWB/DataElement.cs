using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;
using System.Collections;
using System.Text;

namespace s3piwrappers.SWB
{
    public abstract class DataElement : AHandlerElement
    {
        protected DataElement(int APIversion, EventHandler handler, DataElement basis) : base(APIversion, handler)
        {
            MemoryStream ms = new MemoryStream();
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
            return (AHandlerElement) Activator.CreateInstance(GetType(), new object[] {0, handler, this});
        }

        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}
