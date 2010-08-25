using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class common_float_or_param_type
    {
        private object itemField;


        [XmlElement("float", typeof (double))]
        [XmlElement("param", typeof (common_param_type))]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }
    }
}