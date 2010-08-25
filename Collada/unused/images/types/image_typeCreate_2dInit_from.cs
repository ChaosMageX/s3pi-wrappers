using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeCreate_2dInit_from : image_source_type
    {
        public image_typeCreate_2dInit_from()
        {
            array_index = ((0));
        }


        [XmlAttribute]
        public uint mip_index { get; set; }


        [XmlAttribute, DefaultValue(typeof (uint), "0")]
        public uint array_index { get; set; }
    }
}