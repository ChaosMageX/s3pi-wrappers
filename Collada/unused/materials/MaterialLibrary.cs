using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DAESim.DAE
{
    
    
    public class MaterialLibrary : Asset
    {
        [XmlElement("material")]
        public List<Material> Items { get; set; }
    }
}