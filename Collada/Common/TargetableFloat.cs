using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Common
{
    public class TargetableFloat : TargetableSubComponent
    {
        [XmlText]
        public double Value { get; set; }
    }
}