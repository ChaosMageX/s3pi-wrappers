using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    
    
    public class Faces : ExtendedNamedTargetableComponent 
    {
        private ulong countField;


        [XmlElement("input")]
        public InputLocalOffset[] input { get; set; }

        [XmlElement("vcount")]
        public string vcount { get; set; }


        [XmlElement("p")]
        public Primitive Indices { get; set; }
        

        [XmlAttribute("count")]
        public ulong count
        {
            get { return countField; }
            set { countField = value; }
        }
    }
}