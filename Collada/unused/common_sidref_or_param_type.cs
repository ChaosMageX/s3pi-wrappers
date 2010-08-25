using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    //[XmlInclude(typeof (bind_kinematics_model_type))]
    
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class common_sidref_or_param_type
    {
        [XmlElement("SIDREF", typeof (string))]
        [XmlElement("param", typeof (common_param_type))]
        public object Item { get; set; }
    }
}