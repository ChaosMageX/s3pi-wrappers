using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_source_typeHex
    {
        private string formatField;

        private byte[][] textField;


        [XmlAttribute(DataType = "token")]
        public string format
        {
            get { return formatField; }
            set { formatField = value; }
        }


        [XmlText(DataType = "hexBinary")]
        public byte[][] Text
        {
            get { return textField; }
            set { textField = value; }
        }
    }
}