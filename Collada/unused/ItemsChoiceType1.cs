using System.Xml.Serialization;

namespace Proycon.Collada
{
    public enum ItemsChoiceType1
    {
        [XmlEnum("aspect_ratio")]
        AspectRatio,

        [XmlEnum("xfov")]
        Xfov,

        [XmlEnum("yfov")]
        Yfov,
    }
}