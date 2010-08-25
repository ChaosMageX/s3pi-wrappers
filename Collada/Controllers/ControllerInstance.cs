using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Controllers
{
    
    
    public class ControllerInstance : ExtendableInstance<Controller>
    {
        public ControllerInstance()
        {
            Skeletons = new List<string>();
        }
        [XmlElement("skeleton", DataType = "anyURI")]
        public List<String> Skeletons { get; set; }

        //public BindMaterial bind_material { get; set; }



    }
}