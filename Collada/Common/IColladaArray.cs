using System;
using System.Collections;
using System.Xml.Serialization;

namespace s3piwrappers.Collada.Common
{
    [XmlInclude(typeof(IdRefArray))]
    [XmlInclude(typeof(NameArray))]
    [XmlInclude(typeof(SidRefArray))]
    [XmlInclude(typeof(BoolArray))]
    [XmlInclude(typeof(FloatArray))]
    [XmlInclude(typeof(IntArray))]
    [XmlInclude(typeof(TokenArray))]
    public interface IColladaArray
    {
        String Value { get; set; }
        IList Items { get; set; }
        Int32 Count { get; set; }
    }
}