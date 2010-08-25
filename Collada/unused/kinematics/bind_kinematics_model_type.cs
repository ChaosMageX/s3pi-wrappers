using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class bind_kinematics_model_type : common_sidref_or_param_type
    {
        private string nodeField;


        [XmlAttribute(DataType = "token")]
        public string node
        {
            get { return nodeField; }
            set { nodeField = value; }
        }
    }
}