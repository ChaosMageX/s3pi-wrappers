using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class kinematics_index_type : common_int_or_param_type
    {
        private string semanticField;


        [XmlAttribute(DataType = "NMTOKEN")]
        public string semantic
        {
            get { return semanticField; }
            set { semanticField = value; }
        }
    }
}