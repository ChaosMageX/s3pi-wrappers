using System.Collections.Generic;
using System.Xml.Serialization;
using s3piwrappers.Collada.Metadata;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Common
{
    public class Source : AssetComponent
    {


        [XmlElement("IDREF_array", typeof (IdRefArray))]
        [XmlElement("Name_array", typeof (NameArray))]
        [XmlElement("SIDREF_array", typeof (SidRefArray))]
        [XmlElement("bool_array", typeof (BoolArray))]
        [XmlElement("float_array", typeof (FloatArray))]
        [XmlElement("int_array", typeof (IntArray))]
        [XmlElement("token_array", typeof (TokenArray))]
        public IColladaArray Item { get; set; }

        [XmlElement("technique_common")]
        public TechniqueCommon TechniqueCommon { get; set; }
        
        [XmlElement("technique")]
        public List<Technique> Techniques { get; set; }
    }
}