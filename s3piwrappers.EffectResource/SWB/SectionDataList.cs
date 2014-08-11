using System;
using System.Collections.Generic;
using System.IO;

namespace s3piwrappers.SWB
{
    public class SectionDataList<T> : DataList<SectionData>
        where T : SectionData, IEquatable<SectionData>
    {
        #region Constructors
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

        public SectionDataList(EventHandler handler, ISection section, SectionDataList<T> ilt)
            : this(handler, section)
        {
            T item;
            int count = ilt.Count;
            for (int i = 0; i < count; i++)
            {
                item = (T)ilt[i];
                this.Add((SectionData)item.Clone(handler));
            }
        }
        #endregion

        protected ISection mSection;

        protected override SectionData CreateElement(Stream s)
        {
            return (SectionData) Activator.CreateInstance(typeof (T), new object[] {0, handler, mSection, s});
        }

        public override void Add()
        {
            if (typeof (T).IsAbstract)
                throw new NotImplementedException();

            Add(typeof (T));
        }

        public override void Add(Type t)
        {
            var instance = (SectionData) Activator.CreateInstance(t, 0, handler, mSection);
            base.Add(instance);
        }
    }
}
