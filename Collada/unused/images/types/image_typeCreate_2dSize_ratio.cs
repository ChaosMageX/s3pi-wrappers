using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeCreate_2dSize_ratio
    {
        private float heightField;
        private float widthField;


        [XmlAttribute]
        public float width
        {
            get { return widthField; }
            set { widthField = value; }
        }


        [XmlAttribute]
        public float height
        {
            get { return heightField; }
            set { heightField = value; }
        }
    }
}