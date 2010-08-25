using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class kinematics_connect_param_type
    {
        private string refField;


        [XmlAttribute(DataType = "token")]
        public string @ref
        {
            get { return refField; }
            set { refField = value; }
        }
    }
}