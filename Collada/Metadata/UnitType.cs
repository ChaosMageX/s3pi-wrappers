using System.ComponentModel;
using System.Xml.Serialization;

namespace s3piwrappers.Collada.Metadata
{
    public class UnitType
    {
        [DefaultValue(1)]
        [XmlAttribute("meter")]
        public double Meter { get; set; }

        [DefaultValue("meter")]
        [XmlAttribute("name", DataType = "NMTOKEN")]
        public string Name { get; set; }
    }
}