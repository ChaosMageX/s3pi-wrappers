using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Geometry
{
    
    
    public class Geometry : AssetComponent
    {
        [XmlElement("mesh", typeof (Mesh))]
        public Mesh Item { get; set; }
    }
}