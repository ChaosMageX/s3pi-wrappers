using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeInit_from : image_source_type
    {
        private bool mips_generateField;

        public image_typeInit_from()
        {
            mips_generateField = true;
        }


        [XmlAttribute]
        [DefaultValue(true)]
        public bool mips_generate
        {
            get { return mips_generateField; }
            set { mips_generateField = value; }
        }
    }
}