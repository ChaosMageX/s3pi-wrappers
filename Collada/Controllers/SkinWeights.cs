using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using s3piwrappers.Collada.Common;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Controllers
{
    
    
    public class SkinWeights : ExtendableComponent
    {
        public SkinWeights()
        {
            Inputs = new List<InputLocalOffset>();
        }
        [XmlElement("input")]
        public List<InputLocalOffset> Inputs { get; set; }


        [XmlElement("vcount")]
        public string Vcount { get; set; }


        [XmlElement("v")]
        public Primitive Indices { get; set; }



    }
}