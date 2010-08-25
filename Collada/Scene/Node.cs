using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Scene
{
    public class Node : AssetComponent, ITargetable
    {
        public Node()
        {
            Type = NodeType.Node;
            Transformers = new List<NodeTransformer>();
            Nodes = new List<NodeInstance>();
            Children = new List<Node>();
        }

        [XmlElement("lookat", typeof (LookAt))]
        [XmlElement("matrix", typeof (Matrix))]
        [XmlElement("rotate", typeof (Rotate))]
        [XmlElement("scale", typeof (Scale))]
        [XmlElement("skew", typeof (Skew))]
        [XmlElement("translate", typeof (Translate))]
        public List<NodeTransformer> Transformers { get; set; }


        [XmlElement("instance_node")]
        public List<NodeInstance> Nodes { get; set; }

        [XmlElement("node")]
        public List<Node> Children { get; set; }


        [XmlAttribute("type")]
        [DefaultValue(NodeType.Node)]
        public NodeType Type { get; set; }

        [XmlAttribute("layer", DataType = "Name")]
        public List<string> Layers { get; set; }


        [XmlAttribute("sid", DataType = "NCName")]
        public string Sid { get; set; }


        public override string GetUri()
        {
            return String.Format("{0}/{1}", Id, Sid);
        }
    }
}