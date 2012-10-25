using System;
using System.IO;

namespace s3piwrappers.SWB
{
    public class SectionDataList<T> : DataList<SectionData>
        where T : SectionData, IEquatable<SectionData>
    {
        public SectionDataList(EventHandler handler, ISection section)
            : base(handler)
        {
            mSection = section;
        }

        public SectionDataList(EventHandler handler, ISection section, Stream s)
            : this(handler, section)
        {
            Parse(s);
        }

        protected ISection mSection;

        protected override SectionData CreateElement(Stream s)
        {
            return (SectionData) Activator.CreateInstance(typeof (T), new object[] {0, handler, mSection, s});
        }

        public override void Add(Type t)
        {
            var instance = (SectionData) Activator.CreateInstance(t,0,handler, mSection);
            base.Add(instance);
        }
    }
}
