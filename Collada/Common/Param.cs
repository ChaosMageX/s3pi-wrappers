using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Common
{
    public class Param : TargetableNamedSubComponent
    {
        [XmlAttribute("semantic", DataType = "NMTOKEN")]
        public string Semantic { get; set; }

        [XmlAttribute("type", DataType = "NMTOKEN")]
        public string Type { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}