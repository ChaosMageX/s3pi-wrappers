using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class common_param_type
    {
        [XmlText(DataType = "token")]
        public string Value { get; set; }
    }
}