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
            Extra = new List<Extra>();
        }

        public ExtendableInstance(TTarget src) : base(src)
        {
            Extra = new List<Extra>();
        }

        [XmlElement("extra")]
        public IList<Extra> Extra { get; set; }
    }
}