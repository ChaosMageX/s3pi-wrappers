using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeRenderable
    {
        private bool shareField;


        [XmlAttribute]
        public bool share
        {
            get { return shareField; }
            set { shareField = value; }
        }
    }
}