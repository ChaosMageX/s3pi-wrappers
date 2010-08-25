using System.Xml.Serialization;

namespace s3piwrappers.Collada.Metadata
{
    public class Coverage
    {
        [XmlElement("geographic_location_type")]
        public LocationType GeographicLocationType { get; set; }
    }
}