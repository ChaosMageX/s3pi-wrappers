using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Animation
{
    
    
    public class AnimationClip : AssetComponent
    {
        public AnimationClip()
        {
            Start = 0;
            Animations = new List<Instance<Animation>>();
        }

        [XmlElement("instance_animation")]
        public List<Instance<Animation>> Animations { get; set; }


        [XmlAttribute("start"), DefaultValue(0)]
        public double Start { get; set; }


        [XmlAttribute("end")]
        public double End { get; set; }
        [XmlIgnore]
        public bool EndSpecified { get; set; }
    }
}