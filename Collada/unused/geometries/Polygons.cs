using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Proycon.Collada.Common;

namespace Proycon.Collada.Geometry
{
    
    
    public class Polygons : MeshElement
    {
        public Polygons()
        {
            Inputs = new List<InputLocalOffset>();
            Indices = new Primitive();
        }
        [XmlElement("input")]
        public List<InputLocalOffset> Inputs { get; set; }


        [XmlElement("p", typeof (Primitive))]
        public Primitive Indices { get; set; }

        

        [XmlAttribute("count")]
        public Int32 Count { get { return Indices.Items.Count; } set{}}


        [XmlAttribute("material",DataType = "NCName")]
        public string Material { get; set; }
    }
}