using System.Collections.Generic;
using System.Xml.Serialization;
using s3piwrappers.Collada.Common;

namespace s3piwrappers.Collada.Interface
{
    public abstract class NamedExtendableComponent : NamedComponent, IExtendable
    {
        protected NamedExtendableComponent()
        {
            Extra = new List<Extra>();
        }

        [XmlElement("extra")]
        public IList<Extra> Extra { get; set; }
    }
}