using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class library_formulas_type
    {
        private asset_type assetField;

        private extra_type[] extraField;
        private formula_type[] formulaField;

        private string idField;

        private string nameField;


        public asset_type asset
        {
            get { return assetField; }
            set { assetField = value; }
        }


        [XmlElement("formula")]
        public formula_type[] formula
        {
            get { return formulaField; }
            set { formulaField = value; }
        }


        [XmlElement("extra")]
        public extra_type[] extra
        {
            get { return extraField; }
            set { extraField = value; }
        }


        [XmlAttribute(DataType = "ID")]
        public string id
        {
            get { return idField; }
            set { idField = value; }
        }


        [XmlAttribute(DataType = "token")]
        public string name
        {
            get { return nameField; }
            set { nameField = value; }
        }
    }
}