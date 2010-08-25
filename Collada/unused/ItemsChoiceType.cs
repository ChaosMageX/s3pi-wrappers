using System.Xml.Serialization;

namespace Proycon.Collada
{
    public enum ItemsChoiceType
    {
        [XmlEnum("aspect_ratio")]
        AspectRatio,
        [XmlEnum("xmag")]
        Xmag,
        [XmlEnum("ymag")]
        Ymag
    }
}