using System.Collections.Generic;
using System.Xml.Serialization;
using s3piwrappers.Collada.Common;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Geometry
{
    
    
    public class Mesh : ExtendableComponent
    {

        public Mesh()
        {
            Vertices = new Vertices();
            Elements = new List<MeshElement>();
            Sources = new List<Source>();
        }
        public Vertices Vertices { get; set; }
        [XmlElement("triangles", typeof (Triangles))]
        public List<MeshElement> Elements { get; set; }
        [XmlElement("source")]
        public List<Source> Sources { get; set; }
    }
}