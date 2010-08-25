using System.ComponentModel;
using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Common
{
    public class FloatArray : ColladaArray<float>
    {
        public FloatArray()
        {
            Digits = ((6));
            Magnitude = ((38));
        }


        [XmlAttribute]
        [DefaultValue(typeof (byte), "6")]
        public byte Digits { get; set; }


        [XmlAttribute]
        [DefaultValue(typeof (short), "38")]
        public short Magnitude { get; set; }



        protected override float Parse(string s)
        {
            return float.Parse(s);
        }
    }
}