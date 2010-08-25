using System.Xml.Serialization;

namespace s3piwrappers.Collada.Metadata
{
    public class LocationType
    {
        [XmlElement("longitude")]
        public float Longitude { get; set; }
        [XmlElement("latitude")]
        public float Latitude { get; set; }
        [XmlElement("altitude")]
        public AltitudeType Altitude { get; set; }
    }
}