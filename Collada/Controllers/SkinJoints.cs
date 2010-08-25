using System.Collections.Generic;
using System.Xml.Serialization;
using s3piwrappers.Collada.Common;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Controllers
{
    
    
    public class SkinJoints :ExtendableComponent
    {
        public SkinJoints()
        {
            Inputs = new List<InputLocal>();
        }
        [XmlElement("input")]
        public List<InputLocal> Inputs { get; set; }
    }
}