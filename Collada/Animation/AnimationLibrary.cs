using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Animation
{
    
    
    public class AnimationLibrary : AssetComponent
    {
        public AnimationLibrary()
        {
            Items = new List<Animation>();
        }
        [XmlElement("animation")]
        public List<Animation> Items { get; set; }
    }
}