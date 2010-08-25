using System.Xml.Serialization;

namespace s3piwrappers.Collada.Controllers
{
    
    
    public class JointLimits
    {
        public JointLimits()
        {
            Min = new JointLimit();
            Max = new JointLimit();
        }

        [XmlElement("min")]
        public JointLimit Min { get; set; }

        [XmlElement("max")]
        public JointLimit Max { get; set; }
    }
}