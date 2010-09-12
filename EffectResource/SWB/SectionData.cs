using System;
using System.IO;

namespace s3piwrappers.SWB
{
    public class SectionData : ExportableDataElement, IEquatable<SectionData>
    {
        public SectionData(int apiVersion, EventHandler handler, ISection section)
            : base(apiVersion, handler)
        {
            if(section == null)throw new ArgumentException("Argument cannot be null","section");
            mSection = section;
        }

        public SectionData(int apiVersion, EventHandler handler, ISection section, Stream s)
            : this(apiVersion, handler,section)
        {
            Parse(s);
        }

        public SectionData(int apiVersion, EventHandler handler, SectionData basis)
            : base(apiVersion, handler)
        {
            mSection = basis.mSection;
            MemoryStream ms = new MemoryStream();
            basis.UnParse(ms);
            ms.Position = 0L;
            Parse(ms);
        }
        protected ISection mSection;

        public ISection Section
        {
            get { return mSection; }
        }

        protected override void Parse(Stream s){}
        public override void UnParse(Stream s){}

        public bool Equals(SectionData other)
        {
            return base.Equals(other);
        }
    }
}