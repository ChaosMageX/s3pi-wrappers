using System;
using System.Xml.Serialization;

namespace s3piwrappers.Collada.Interface
{
    public abstract class TargetableComponent : ITargetable
    {
        [XmlAttribute("id", DataType = "NCName")]
        public string Id { get; set; }

        public virtual string GetUri()
        {
            return String.Format("#{0}", Id);
        }
    }
}