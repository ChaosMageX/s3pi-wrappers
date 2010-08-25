using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class formula_type : AssetBase
    {
        private string idField;

        private string nameField;
        private formula_newparam_type[] newparamField;

        private string sidField;
        private common_float_or_param_type targetField;

        private technique_type[] techniqueField;


        [XmlElement("newparam")]
        public formula_newparam_type[] newparam
        {
            get { return newparamField; }
            set { newparamField = value; }
        }


        public common_float_or_param_type target
        {
            get { return targetField; }
            set { targetField = value; }
        }


        [XmlElement("technique")]
        public technique_type[] technique
        {
            get { return techniqueField; }
            set { techniqueField = value; }
        }




        [XmlAttribute(DataType = "NCName")]
        public string sid
        {
            get { return sidField; }
            set { sidField = value; }
        }
    }
}