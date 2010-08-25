using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeCreate_cubeArray
    {
        private uint lengthField;


        [XmlAttribute]
        public uint length
        {
            get { return lengthField; }
            set { lengthField = value; }
        }
    }
}