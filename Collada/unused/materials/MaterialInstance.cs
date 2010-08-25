using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DAESim.DAE
{
    
    public class MaterialInstance: ExtendableInstance<Material>
    {
        public MaterialInstance()
        {
            Binds = new List<MaterialInstanceBind>();
            BindVertexInputs = new List<MaterialInstanceBindVertexInput>();
        }
        [XmlElement("bind")]
        public List<MaterialInstanceBind> Binds { get; set; }

        [XmlElement("bind_vertex_input")]
        public List<MaterialInstanceBindVertexInput> BindVertexInputs { get; set; }


        [XmlAttribute("symbol", DataType = "NCName")]
        public string Symbol { get; set; }

        [XmlAttribute("target", DataType = "anyURI")]
        public string Target { get; set; }
    }
}