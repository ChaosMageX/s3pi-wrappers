using System.ComponentModel;
using System.Xml.Serialization;

namespace s3piwrappers.Collada.Metadata
{
    public class AltitudeType
    {
        public AltitudeType()
        {
            Mode = CoverageMode.relativeToGround;
        }

        [XmlAttribute("mode")]
        [DefaultValue(CoverageMode.relativeToGround)]
        public CoverageMode Mode { get; set; }

        [XmlText]
        public float Value { get; set; }
    }
}