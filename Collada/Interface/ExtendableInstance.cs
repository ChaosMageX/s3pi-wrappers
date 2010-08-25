using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Common;

namespace s3piwrappers.Collada.Interface
{
    public class ExtendableInstance<TTarget> : Instance<TTarget>, IExtendable
        where TTarget : ITargetable
    {
        public ExtendableInstance()
        {
            ExtendedData = new List<Extra>();
        }

        public ExtendableInstance(TTarget src) : base(src)
        {
            ExtendedData = new List<Extra>();
        }

        [XmlElement("extra")]
        public List<Extra> ExtendedData { get; set; }
    }
}