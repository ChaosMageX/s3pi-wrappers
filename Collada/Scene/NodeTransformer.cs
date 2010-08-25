using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Scene
{
    [XmlInclude(typeof (LookAt))]
    [XmlInclude(typeof (Matrix))]
    [XmlInclude(typeof (Rotate))]
    [XmlInclude(typeof (Scale))]
    [XmlInclude(typeof (Skew))]
    [XmlInclude(typeof (Translate))]
    public abstract class NodeTransformer : TargetableSubComponent
    {
    }
}