using System.Xml.Serialization;

namespace s3piwrappers.Collada.Common
{
    public class InputGlobal
    {
        [XmlAttribute("semantic", DataType = "NMTOKEN")]
        public string Semantic { get; set; }


        [XmlAttribute("source", DataType = "anyURI")]
        public string Source { get; set; }
    }
}