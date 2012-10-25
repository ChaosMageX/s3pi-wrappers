using System;
using System.Collections.Generic;
using System.IO;
using s3pi.Interfaces;

namespace s3piwrappers.SWB
{
    public abstract class ExportableDataElement : DataElement
    {
        protected ExportableDataElement(int apiVersion, EventHandler handler)
            : base(apiVersion, handler)
        {
        }

        protected ExportableDataElement(int apiVersion, EventHandler handler, Stream s)
            : base(apiVersion, handler, s)
        {
        }

        protected ExportableDataElement(int apiVersion, EventHandler handler, ExportableDataElement basis)
            : base(apiVersion, handler, basis)
        {
        }

        protected abstract override void Parse(Stream s);
        public abstract override void UnParse(Stream s);

        [ElementPriority(0)]
        public virtual BinaryReader Data
        {
            get
            {
                var s = new MemoryStream();
                UnParse(s);
                s.Position = 0L;
                return new BinaryReader(s);
            }
            set
            {
                if (value.BaseStream.CanSeek)
                {
                    value.BaseStream.Position = 0L;
                    Parse(value.BaseStream);
                }
                else
                {
                    var s = new MemoryStream();
                    var buffer = new byte[0x100000];
                    for (int i = value.BaseStream.Read(buffer, 0, buffer.Length); i > 0; i = value.BaseStream.Read(buffer, 0, buffer.Length))
                    {
                        s.Write(buffer, 0, i);
                    }
                    Parse(s);
                }
                OnElementChanged();
            }
        }

        protected override List<string> ValueBuilderFields
        {
            get
            {
                List<string> b = base.ValueBuilderFields;
                b.Remove("Data");
                return b;
            }
        }

        public override AHandlerElement Clone(EventHandler handler)
        {
            var s = new MemoryStream();
            UnParse(s);
            return (AHandlerElement) Activator.CreateInstance(GetType(), 0, handler, s);
        }
    }
}
