using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Controllers
{
    
    
    public class JointLimit : TargetableNamedSubComponent
    {
        [XmlElement("value")]
        public double Value { get; set; }
    }
}