using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Controllers
{
    
    
    public class JointLibrary : AssetComponent
    {
        public JointLibrary()
        {
            Items = new List<Joint>();
        }

        [XmlElement("joint")]
        public List<Joint> Items { get; set; }
    }
}