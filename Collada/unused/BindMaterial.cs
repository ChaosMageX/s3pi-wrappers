using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class BindMaterial
    {
        private Extra[] extraField;
        private param_type[] paramField;

        //private instance_material_type[] technique_commonField;

        private Technique[] techniqueField;


        [XmlElement("param")]
        public param_type[] param
        {
            get { return paramField; }
            set { paramField = value; }
        }


        //[XmlArrayItem("instance_material", IsNullable = false)]
        //public instance_material_type[] technique_common
        //{
        //    get { return this.technique_commonField; }
        //    set
        //    {
        //        this.technique_commonField = value;
        //       
        //    }
        //}


        [XmlElement("technique")]
        public Technique[] technique
        {
            get { return techniqueField; }
            set { techniqueField = value; }
        }


        [XmlElement("extra")]
        public Extra[] extra
        {
            get { return extraField; }
            set { extraField = value; }
        }
    }
}