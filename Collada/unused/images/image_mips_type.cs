using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    //[XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_mips_type
    {
        private bool auto_generateField;
        private uint levelsField;


        [XmlAttribute]
        public uint levels
        {
            get { return levelsField; }
            set { levelsField = value; }
        }


        [XmlAttribute]
        public bool auto_generate
        {
            get { return auto_generateField; }
            set { auto_generateField = value; }
        }
    }
}