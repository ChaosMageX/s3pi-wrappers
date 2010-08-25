using System.Xml.Serialization;

namespace s3piwrappers.Collada
{
    public enum DocumentVersion
    {
        [XmlEnum("1.4.1")]
        Item141,
        [XmlEnum("1.5.0")] 
        Item150
    }
}