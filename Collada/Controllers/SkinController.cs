using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Common;

namespace s3piwrappers.Collada.Controllers
{
    public class SkinController : ControllerData
    {
        public SkinController()
        {
            Sources = new List<Source>();
            Joints = new SkinJoints();
            VertexWeights = new SkinWeights();
        }

        [XmlElement("bind_shape_matrix")]
        public string BindShapeMatrix { get; set; }

        [XmlElement("joints")]
        public SkinJoints Joints { get; set; }

        [XmlElement("vertex_weights")]
        public SkinWeights VertexWeights { get; set; }

        [XmlAttribute("source", DataType = "anyURI")]
        public List<Source> Sources { get; set; }
    }
}