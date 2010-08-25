using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    //[XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_source_type
    {
        private object itemField;


        [XmlElement("hex", typeof (image_source_typeHex))]
        [XmlElement("ref", typeof (string), DataType = "anyURI")]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }
    }
}