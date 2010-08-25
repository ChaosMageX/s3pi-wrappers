using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Controllers
{
    
    
    public class JointAxisConstraint : TargetableSubComponent
    {
        public JointAxisConstraint()
        {
        }

        [XmlElement("axis")]
        public JointAxis Axis { get; set; }


        [XmlElement("limits")]
        public JointLimits Limits { get; set; }

    }
}