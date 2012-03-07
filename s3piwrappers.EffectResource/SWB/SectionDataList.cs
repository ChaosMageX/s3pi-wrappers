using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;

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

        public override void Add()
        {
            Add(new object[] {mSection});
        }

        public override bool Add(params object[] fields)
        {
            if (fields == null)
            {
                return false;
            }

            Type c = typeof (T);
            Type[] types = new Type[0x2 + fields.Length];
            types[0] = typeof (int);
            types[1] = typeof (EventHandler);
            for (int i = 0x0; i < fields.Length; i++)
            {
                types[2 + i] = fields[i].GetType();
            }
            object[] destinationArray = new object[0x2 + fields.Length];
            destinationArray[0] = 0;
            destinationArray[1] = this.elementHandler;
            Array.Copy(fields, 0, destinationArray, 2, fields.Length);
            if (c.GetConstructor(types) == null)
            {
                return false;
            }
            ((IList) this).Add((T) c.GetConstructor(types).Invoke(destinationArray));
            return true;
        }
    }
}
