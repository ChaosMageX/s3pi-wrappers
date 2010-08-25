using System.Xml.Serialization;

namespace Proycon.Collada
{
    public enum ItemChoiceType
    {
        [XmlEnum("float")]
        Float,
        [XmlEnum("float2")]
        Float2,
        [XmlEnum("float3")]
        Float3,
        [XmlEnum("float4")]
        Float4,
        [XmlEnum("sampler2D")]
        Sampler2D
    }
}