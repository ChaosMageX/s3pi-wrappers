using System.Collections.Generic;
using System.Xml.Serialization;
using s3piwrappers.Collada.Common;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Geometry
{


    public class Vertices : TargetableNamedExtendableComponent
    {

        public Vertices()
        {
            Inputs = new List<InputLocalOffset>();
        }

        [XmlElement("input")]
        public List<InputLocalOffset> Inputs { get; set; }
        
    }
}