﻿using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Scene
{
    public class NodeLibrary : AssetComponent
    {
        public NodeLibrary()
        {
            Nodes = new List<Node>();
        }

        [XmlElement("node")]
        public List<Node> Nodes { get; set; }
    }
}