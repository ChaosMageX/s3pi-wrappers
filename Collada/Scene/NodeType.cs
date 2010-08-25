using System.Xml.Serialization;

namespace s3piwrappers.Collada.Scene
{
    public enum NodeType
    {
        [XmlEnum("JOINT")]
        Joint,
        [XmlEnum("NODE")]
        Node
    }
}