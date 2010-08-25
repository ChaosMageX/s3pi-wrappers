using System.Xml.Serialization;
using s3piwrappers.Collada.Metadata;

namespace s3piwrappers.Collada.Interface
{
    public abstract class AssetComponent : TargetableNamedExtendableComponent
    {
        [XmlElement("asset")]
        public Asset Asset { get; set; }
    }
}