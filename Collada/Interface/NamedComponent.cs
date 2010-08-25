using System.Xml.Serialization;

namespace s3piwrappers.Collada.Interface
{
    public abstract class NamedComponent:INamed
    {
        [XmlAttribute("name", DataType = "NCName")]
        public string Name { get; set; }
    }
}