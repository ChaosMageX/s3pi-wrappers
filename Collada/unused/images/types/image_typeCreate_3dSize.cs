using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeCreate_3dSize
    {
        private uint depthField;
        private uint heightField;
        private uint widthField;


        [XmlAttribute]
        public uint width
        {
            get { return widthField; }
            set { widthField = value; }
        }


        [XmlAttribute]
        public uint height
        {
            get { return heightField; }
            set { heightField = value; }
        }


        [XmlAttribute]
        public uint depth
        {
            get { return depthField; }
            set { depthField = value; }
        }
    }
}