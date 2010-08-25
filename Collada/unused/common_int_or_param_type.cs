using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    //[XmlInclude(typeof (kinematics_index_type))]
    
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class common_int_or_param_type
    {
        private object itemField;


        [XmlElement("int", typeof (long))]
        [XmlElement("param", typeof (common_param_type))]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }
    }
}