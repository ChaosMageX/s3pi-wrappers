using System;
using System.Collections;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers.SWB
{
    public abstract class Section : DataElement, ISection
    {
        protected ushort mVersion;
        protected DataList<SectionData> mItems;

        protected Section(int apiVersion, EventHandler handler) : this(apiVersion, handler, 0)
        {
        }

        protected Section(int apiVersion, EventHandler handler, ushort version) : base(apiVersion, handler)
        {
            mVersion = version;
            mItems = new DataList<SectionData>(handler);
        }

        protected Section(int apiVersion, EventHandler handler, ushort version, Stream s)
            : this(apiVersion, handler, version)
        {
            Parse(s);
        }

        protected Section(int apiVersion, EventHandler handler, Section basis)
            : this(apiVersion, handler, basis.mVersion)
        {
            var s = new MemoryStream();
            basis.UnParse(s);
            s.Position = 0L;
            Parse(s);
        }


        protected abstract override void Parse(Stream s);
        public abstract override void UnParse(Stream s);

        [ElementPriority(2)]
        public ushort Version
        {
            get { return mVersion; }
            set
            {
                mVersion = value;
                OnElementChanged();
            }
        }

        [ElementPriority(1)]
        public abstract ushort Type { get; }

        [ElementPriority(3)]
        public DataList<SectionData> Items
        {
            get { return mItems; }
            set
            {
                mItems = value;
                OnElementChanged();
            }
        }

        IEnumerable ISection.Items
        {
            get { return mItems; }
        }
    }

    public abstract class Section<T> : Section
        where T : SectionData, IEquatable<T>
    {
        protected Section(int apiVersion, EventHandler handler, ushort version)
            : base(apiVersion, handler, version)
        {
            mItems = new SectionDataList<T>(handler, this);
            mVersion = version;
        }

        protected Section(int apiVersion, EventHandler handler, ushort version, Stream s)
            : base(apiVersion, handler, version, s)
        {
        }

        protected Section(int apiVersion, EventHandler handler, Section basis)
            : base(apiVersion, handler, basis)
        {
        }

        protected override void Parse(Stream stream)
        {
            mItems = new SectionDataList<T>(handler, this, stream);
        }

        public override void UnParse(Stream stream)
        {
            mItems.UnParse(stream);
        }
    }
}
