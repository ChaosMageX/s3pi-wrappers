using System;
using System.Xml.Serialization;

namespace s3piwrappers.Collada.Interface
{
    public abstract class TargetableSubComponent : ITargetable
    {
        [XmlAttribute("sid", DataType = "NCName")]
        public string Sid { get; set; }

        public string GetUri()
        {
            return String.Format("{0}/{1}", null, Sid);
        }
    }
}