using System;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class circle_type
    {
        private extra_type[] extraField;
        private double radiusField;


        public double radius
        {
            get { return radiusField; }
            set { radiusField = value; }
        }


        [XmlElement("extra")]
        public extra_type[] extra
        {
            get { return extraField; }
            set { extraField = value; }
        }
    }
}