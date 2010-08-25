using System.Xml.Serialization;

namespace Proycon.Collada
{
    public enum ItemsChoiceType2
    {
        [XmlEnum("axis")]
        Axis,

        [XmlEnum("direction")]
        Direction,

        [XmlEnum("origin")]
        Origin,
    }
}