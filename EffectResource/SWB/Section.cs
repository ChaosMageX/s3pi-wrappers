using System;
using System.Collections;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers.SWB
{
    public abstract class Section : ExportableDataElement
    {
        protected ushort mVersion;
        protected ushort mType;
        protected DataList<SectionData> mItems;
        protected Section(int apiVersion, EventHandler handler, ushort type, ushort version)
            : base(apiVersion, handler)
        {
            mVersion = version;
            mType = type;
        }

        protected Section(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
            : this(apiVersion, handler,type,version)
        {
            Parse(s);
        }

        protected Section(int apiVersion, EventHandler handler, Section basis)
            : this(apiVersion, handler,basis.mType,basis.mVersion)
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
            set { mVersion = value; OnElementChanged(); }
        }
        [ElementPriority(1)]
        public ushort Type
        {
            get { return mType; }
            set {}
        }
        [ElementPriority(3)]
        public DataList<SectionData> Items
        {
            get { return mItems; } 
            set { mItems = value;OnElementChanged(); }
        }
    }
    public abstract class Section<T> : Section
        where T : SectionData, IEquatable<T>
    {


        protected Section(int apiVersion, EventHandler handler, ushort type, ushort version)
            : base(apiVersion, handler, type, version)
        {
            mItems = new SectionDataList<T>(handler, this);
        }

        protected Section(int apiVersion, EventHandler handler, ushort type, ushort version, Stream s)
            : base(apiVersion, handler, type, version, s)
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