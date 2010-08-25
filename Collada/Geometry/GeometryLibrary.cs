using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Geometry
{
    
    
    public class GeometryLibrary : AssetComponent
    {
        public GeometryLibrary()
        {
            Items = new List<Geometry>();
        }

        //[XmlElement("geometry")]
        [XmlElement("geometry",typeof(Geometry))]
        public List<Geometry> Items { get; set; }
    }
}