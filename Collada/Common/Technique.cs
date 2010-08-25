using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace s3piwrappers.Collada.Common
{
    public class Technique
    {
        public Technique()
        {
            Data = new List<XmlElement>();
        }

        [XmlAttribute("profile",DataType = "NMTOKEN")]
        public string Profile { get; set; }

        [XmlAnyElement]
        public List<XmlElement> Data { get; set; }
    }
}