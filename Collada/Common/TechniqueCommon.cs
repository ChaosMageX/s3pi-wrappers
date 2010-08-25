using System.Xml.Serialization;

namespace s3piwrappers.Collada.Common
{
    public class TechniqueCommon
    {
        public TechniqueCommon()
        {
            Accessor = new Accessor();
        }
        [XmlElement("accessor")]
        public Accessor Accessor { get; set; }
    }
}