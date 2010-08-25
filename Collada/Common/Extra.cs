using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;
using s3piwrappers.Collada.Metadata;

namespace s3piwrappers.Collada.Common
{
    public class Extra : TargetableComponent
    {
        public Extra()
        {
            Techniques = new List<Technique>();
        }


        [XmlElement("asset")]
        public Asset Asset { get; set; }

        [XmlElement("technique")]
        public List<Technique> Techniques { get; set; }

        [XmlAttribute("type", DataType = "NMTOKEN")]
        public string Type { get; set; }
    }
}