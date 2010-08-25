using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    //[XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class Image : Asset
    {
        private object itemField;

        private image_typeRenderable renderableField;
        private string sidField;


        public image_typeRenderable renderable
        {
            get { return renderableField; }
            set { renderableField = value; }
        }


        [XmlElement("create_2d", typeof (image_typeCreate_2d))]
        [XmlElement("create_3d", typeof (image_typeCreate_3d))]
        [XmlElement("create_cube", typeof (image_typeCreate_cube))]
        [XmlElement("init_from", typeof (image_typeInit_from))]
        public object Item
        {
            get { return itemField; }
            set { itemField = value; }
        }


        [XmlAttribute(DataType = "NCName")]
        public string sid
        {
            get { return sidField; }
            set { sidField = value; }
        }
    }
}