using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Common;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Animation
{
    
    
    public class Animation : AssetComponent
    {
        public Animation()
        {
            Children = new List<Animation>();
            Channels = new List<Channel>();
            Samplers = new List<Sampler>();
            Sources = new List<Source>();
        }

        [XmlElement("animation", typeof(Animation))]
        public List<Animation> Children { get; set; }
        [XmlElement("channel", typeof(Channel))]
        public List<Channel> Channels { get; set; }
        [XmlElement("sampler", typeof(Sampler))]
        public List<Sampler> Samplers { get; set; }
        [XmlElement("source", typeof(Source))]
        public List<Source> Sources { get; set; }
        
        
        
        public class Channel
        {
            [XmlAttribute("source")]
            public string Source { get; set; }


            [XmlAttribute("target", DataType = "token")]
            public string Target { get; set; }
        }
        
        
        public class Sampler
        {
            
            
            public enum Behaviour
            {
                CONSTANT,
                CYCLE,
                CYCLE_RELATIVE,
                GRADIENT,
                OSCILLATE,
                UNDEFINED
            }
            [XmlElement("input")]
            public List<InputLocal> Inputs { get; set; }


            [XmlAttribute("id",DataType = "ID")]
            public string Id { get; set; }


            [XmlAttribute("pre_behavior")]
            public Behaviour PreBehavior { get; set; }


            [XmlIgnore]
            public bool PreBehaviorSpecified { get { return PreBehavior != Behaviour.UNDEFINED; } }


            [XmlAttribute("pre_behavior")]
            public Behaviour PostBehavior { get; set; }


            [XmlIgnore]
            public bool PostBehaviorSpecified { get; set; }
        }

    }
}