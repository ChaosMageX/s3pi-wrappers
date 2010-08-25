using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Controllers
{
    
    
    public class Joint : AssetComponent
    {
        public Joint()
        {
        }

        [XmlElement("prismatic")]
        public JointAxisConstraint Prismatic { get; set; }

        [XmlElement("revolute")]
        public JointAxisConstraint Revolute { get; set; }
    }
}