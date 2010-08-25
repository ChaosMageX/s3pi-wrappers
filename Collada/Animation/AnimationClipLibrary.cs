using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Animation
{
    
    
    public class AnimationClipLibrary : AssetComponent
    {
        public AnimationClipLibrary()
        {
            Items = new List<AnimationClip>();
        }

        [XmlElement("animation_clip")]
        public List<AnimationClip> Items { get; set; }
       
    }
}