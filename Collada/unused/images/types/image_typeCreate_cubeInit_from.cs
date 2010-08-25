using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeCreate_cubeInit_from : image_source_type
    {
        private uint array_indexField;
        private image_face_enum faceField;

        private uint mip_indexField;

        public image_typeCreate_cubeInit_from()
        {
            array_indexField = ((0));
        }


        [XmlAttribute]
        public image_face_enum face
        {
            get { return faceField; }
            set { faceField = value; }
        }


        [XmlAttribute]
        public uint mip_index
        {
            get { return mip_indexField; }
            set { mip_indexField = value; }
        }


        [XmlAttribute]
        [DefaultValue(typeof (uint), "0")]
        public uint array_index
        {
            get { return array_indexField; }
            set { array_indexField = value; }
        }
    }
}