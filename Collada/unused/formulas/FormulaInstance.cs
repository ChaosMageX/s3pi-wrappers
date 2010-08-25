using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class FormulaInstance : InstanceBase
    {
        public FormulaInstance(formula_type src):base(src)
        {
                
        }
        private string nameField;
        private formula_setparam_type[] setparamField;

        private string sidField;

        private string urlField;


        [XmlElement("setparam")]
        public formula_setparam_type[] setparam
        {
            get { return setparamField; }
            set { setparamField = value; }
        }


        [XmlAttribute(DataType = "NCName")]
        public string sid
        {
            get { return sidField; }
            set { sidField = value; }
        }


        [XmlAttribute(DataType = "token")]
        public string name
        {
            get { return nameField; }
            set { nameField = value; }
        }


        [XmlAttribute(DataType = "anyURI")]
        public string url
        {
            get { return urlField; }
            set { urlField = value; }
        }
    }
}