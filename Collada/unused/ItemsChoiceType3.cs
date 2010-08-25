using System.Xml.Serialization;

namespace Proycon.Collada
{
    public enum ItemsChoiceType3
    {
        [XmlEnum("prismatic")]
        Prismatic,
        [XmlEnum("revolute")]
        Revolute,
    }
}