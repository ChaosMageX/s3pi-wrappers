using System;
using System.ComponentModel;
using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Common
{
    public class IntArray : ColladaArray<Int32>
    {

        public IntArray()
        {
            MinInclusive = -2147483648;
            MaxInclusive = 2147483647;
        }



        [XmlAttribute("minInclusive", DataType = "integer")]
        [DefaultValue("-2147483648")]
        public Int32 MinInclusive { get; set; }


        [XmlAttribute("maxInclusive", DataType = "integer")]
        [DefaultValue("2147483647")]
        public Int32 MaxInclusive { get; set; }



        protected override int Parse(string s)
        {
            return Int32.Parse(s);
        }
    }
}