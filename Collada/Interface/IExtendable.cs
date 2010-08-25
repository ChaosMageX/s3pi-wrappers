using System.Collections.Generic;
using System.Xml.Serialization;
using s3piwrappers.Collada.Common;

namespace s3piwrappers.Collada.Interface
{
    public interface IExtendable
    {
        [XmlElement("extra")]
        IList<Extra> Extra { get; set; }
    }
}