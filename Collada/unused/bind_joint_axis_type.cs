using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class bind_joint_axis_type
    {
        [XmlAttribute("axis")]
        public common_sidref_or_param_type axis { get; set; }
        [XmlAttribute("value")]
        public common_float_or_param_type value { get; set; }
        [XmlAttribute("target", DataType = "token")]
        public string target { get; set; }
    }
}