using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Common;

namespace s3piwrappers.Collada.Geometry
{
    
    
    public class Triangles : MeshElement
    {
        public Triangles()
        {
            Inputs = new List<InputLocalOffset>();
            Indices = new Primitive();
        }

        [XmlElement("input")]
        public List<InputLocalOffset> Inputs { get; set; }

        [XmlElement("p")]
        public Primitive Indices { get; set; }


        [XmlAttribute("count")]
        public ulong Count { get; set; }


        [XmlAttribute("material",DataType = "NCName")]
        public string Material { get; set; }
    }
}