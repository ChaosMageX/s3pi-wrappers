using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Geometry
{
    [XmlInclude(typeof(Triangles))]
    public abstract class MeshElement : ExtendableComponent
    {

        [XmlAttribute("name", DataType = "token")]
        public string Name { get; set; }
    }
}
