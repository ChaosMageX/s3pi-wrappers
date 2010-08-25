using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeCreate_2d
    {
        private image_typeCreate_2dArray arrayField;

        private image_typeCreate_2dFormat formatField;

        private image_typeCreate_2dInit_from[] init_fromField;
        private object item1Field;


        [XmlElement("size_exact", typeof (image_typeCreate_2dSize_exact)), XmlElement("size_ratio", typeof (image_typeCreate_2dSize_ratio))]
        public object SizeType { get; set; }


        [XmlElement("mips", typeof (image_mips_type))]
        [XmlElement("unnormalized", typeof (object))]
        public object Item1
        {
            get { return item1Field; }
            set { item1Field = value; }
        }


        public image_typeCreate_2dArray array
        {
            get { return arrayField; }
            set { arrayField = value; }
        }


        public image_typeCreate_2dFormat format
        {
            get { return formatField; }
            set { formatField = value; }
        }


        [XmlElement("init_from")]
        public image_typeCreate_2dInit_from[] init_from
        {
            get { return init_fromField; }
            set { init_fromField = value; }
        }
    }
}