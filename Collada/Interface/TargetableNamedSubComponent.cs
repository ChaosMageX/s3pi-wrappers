using System.Xml.Serialization;

namespace s3piwrappers.Collada.Interface
{
    public abstract class TargetableNamedSubComponent : TargetableSubComponent
    {
        [XmlAttribute("name", DataType = "NCName")]
        public string Name { get; set; }
    }
}