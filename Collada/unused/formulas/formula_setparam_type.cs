using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class formula_setparam_type
    {
        private object itemField;

        private string refField;


        [XmlElement("SIDREF", typeof (string))]
        [XmlElement("bool", typeof (bool))]
        //[XmlElement("connect_param", typeof (kinematics_connect_param_type))]
        [XmlElement("float", typeof (double))]
        [XmlElement("int", typeof (long))]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }


        [XmlAttribute(DataType = "token")]
        public string @ref
        {
            get { return refField; }
            set { refField = value; }
        }
    }
}