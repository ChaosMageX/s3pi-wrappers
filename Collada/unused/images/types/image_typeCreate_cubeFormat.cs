using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeCreate_cubeFormat
    {
        private string exactField;
        private image_typeCreate_cubeFormatHint hintField;


        public image_typeCreate_cubeFormatHint hint
        {
            get { return hintField; }
            set { hintField = value; }
        }


        [XmlElement(DataType = "token")]
        public string exact
        {
            get { return exactField; }
            set { exactField = value; }
        }
    }
}