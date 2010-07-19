using System;
using System.Collections.Generic;
using System.Text;
using s3pi.Interfaces;
using System.IO;

namespace s3piwrappers
{
    public abstract class ExportContentResource : AResource
    {
        public abstract class Property : AHandlerElement, IEquatable<Property>
        {

            public bool Equals(Property other)
            {
                throw new NotImplementedException();
            }

            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override List<string> ContentFields
            {
                get { throw new NotImplementedException(); }
            }

            public override int RecommendedApiVersion
            {
                get { throw new NotImplementedException(); }
            }
        }
        public class PropertyList : DependentList<Property>
        {

            public override void Add()
            {
                throw new NotImplementedException();
            }

            protected override Property CreateElement(Stream s)
            {
                throw new NotImplementedException();
            }

            protected override void WriteElement(Stream s, Property element)
            {
                throw new NotImplementedException();
            }
        }
        public class ExportContentDataNode : AHandlerElement
        {


            public override AHandlerElement Clone(EventHandler handler)
            {
                throw new NotImplementedException();
            }

            public override List<string> ContentFields
            {
                get { throw new NotImplementedException(); }
            }

            public override int RecommendedApiVersion
            {
                get { throw new NotImplementedException(); }
            }
        }

        protected abstract override Stream UnParse();
        public override int RecommendedApiVersion
        {
            get { return 1; }
        }
    }
}
