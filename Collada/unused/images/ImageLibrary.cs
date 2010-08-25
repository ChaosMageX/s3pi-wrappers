using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace DAESim.DAE
{
    [Serializable]
    //[XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class ImageLibrary : Asset
    {
        public ImageLibrary()
        {
            Items = new List<Image>();
        }

        [XmlElement("image")]
        public List<Image> Items { get; set; }
    }
}