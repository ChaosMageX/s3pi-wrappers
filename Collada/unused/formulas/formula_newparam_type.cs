using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class formula_newparam_type
    {
        private object itemField;

        private string sidField;


        [XmlElement("SIDREF", typeof (string))]
        [XmlElement("bool", typeof (bool))]
        [XmlElement("float", typeof (double))]
        [XmlElement("int", typeof (long))]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }


        [XmlAttribute(DataType = "NCName")]
        public string sid
        {
            get { return sidField; }
            set { sidField = value; }
        }
    }
}