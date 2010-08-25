using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Controllers
{
    
    
    public class ControllerLibrary : AssetComponent
    {
        public ControllerLibrary()
        {
            Items = new List<Controller>();
        }

        [XmlElement("controller")]
        public List<Controller> Items { get; set; }
    }
}