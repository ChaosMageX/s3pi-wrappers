using System;
using System.IO;
using s3piwrappers.SWB;

namespace s3piwrappers.Resources
{
    public abstract class Resource : SectionData
    {
        protected Resource(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler, section)
        {
        }

        protected Resource(int apiVersion, EventHandler handler, ISection section, Stream s)
            : base(apiVersion, handler, section, s)
        {
        }

        protected Resource(int apiVersion, EventHandler handler, Resource basis)
            : base(apiVersion, handler, basis)
        {
        }

        protected abstract override void Parse(Stream s);

        public abstract override void UnParse(Stream s);
    }
}
