using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeCreate_cube
    {
        private image_typeCreate_cubeArray arrayField;

        private image_typeCreate_cubeFormat formatField;

        private image_typeCreate_cubeInit_from[] init_fromField;
        private image_mips_type mipsField;
        private image_typeCreate_cubeSize sizeField;


        public image_typeCreate_cubeSize size
        {
            get { return sizeField; }
            set { sizeField = value; }
        }


        public image_mips_type mips
        {
            get { return mipsField; }
            set { mipsField = value; }
        }


        public image_typeCreate_cubeArray array
        {
            get { return arrayField; }
            set { arrayField = value; }
        }


        public image_typeCreate_cubeFormat format
        {
            get { return formatField; }
            set { formatField = value; }
        }


        [XmlElement("init_from")]
        public image_typeCreate_cubeInit_from[] init_from
        {
            get { return init_fromField; }
            set { init_fromField = value; }
        }
    }
}