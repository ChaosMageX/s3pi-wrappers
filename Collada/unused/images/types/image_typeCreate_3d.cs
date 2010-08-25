using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeCreate_3d
    {
        private image_typeCreate_3dArray arrayField;

        private image_typeCreate_3dFormat formatField;

        private image_typeCreate_3dInit_from[] init_fromField;
        private image_mips_type mipsField;
        private image_typeCreate_3dSize sizeField;


        public image_typeCreate_3dSize size
        {
            get { return sizeField; }
            set { sizeField = value; }
        }


        public image_mips_type mips
        {
            get { return mipsField; }
            set { mipsField = value; }
        }


        public image_typeCreate_3dArray array
        {
            get { return arrayField; }
            set { arrayField = value; }
        }


        public image_typeCreate_3dFormat format
        {
            get { return formatField; }
            set { formatField = value; }
        }


        [XmlElement("init_from")]
        public image_typeCreate_3dInit_from[] init_from
        {
            get { return init_fromField; }
            set { init_fromField = value; }
        }
    }
}