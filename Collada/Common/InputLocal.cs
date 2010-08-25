using System.Xml.Serialization;

namespace s3piwrappers.Collada.Common
{
    public class InputLocal
    {
        [XmlAttribute("semantic", DataType = "NMTOKEN")]
        public string Semantic { get; set; }


        [XmlAttribute("source")]
        public string Source { get; set; }
    }
}