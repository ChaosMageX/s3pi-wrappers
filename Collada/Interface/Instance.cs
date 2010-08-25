using System.Xml.Serialization;

namespace s3piwrappers.Collada.Interface
{
    public class Instance<TTarget> : TargetableNamedSubComponent
        where TTarget : ITargetable
    {
        public Instance()
        {
        }

        public Instance(TTarget src)
        {
            URL = src.GetUri();
        }


        [XmlAttribute("url", DataType = "anyURI")]
        public string URL { get; set; }
    }
}