using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Scene
{
    public class NodeInstance : ExtendableInstance<Node>
    {
        public NodeInstance()
        {
        }

        public NodeInstance(Node src) : base(src)
        {
        }

        [XmlAttribute("proxy", DataType = "anyURI")]
        public string Proxy { get; set; }
    }
}