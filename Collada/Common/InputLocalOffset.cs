using System.Xml.Serialization;

namespace s3piwrappers.Collada.Common
{
    public class InputLocalOffset
    {
        [XmlAttribute("offset")]
        public ulong Offset { get; set; }


        [XmlAttribute("semantic", DataType = "NMTOKEN")]
        public string Semantic { get; set; }


        [XmlAttribute("source")]
        public string Source { get; set; }


        [XmlAttribute("set")]
        public ulong Set { get; set; }


        [XmlIgnore]
        public bool SetSpecified { get; set; }
    }
}