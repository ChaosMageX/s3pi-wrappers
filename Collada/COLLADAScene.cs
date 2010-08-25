using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;
using s3piwrappers.Collada.Scene;

namespace s3piwrappers.Collada
{
    public class COLLADAScene : ExtendableComponent
    {
        public COLLADAScene()
        {
            VisualSceneInstances = new List<VisualSceneInstance>();
        }

        [XmlElement("instance_visual_scene")]
        public List<VisualSceneInstance> VisualSceneInstances { get; set; }
    }
}