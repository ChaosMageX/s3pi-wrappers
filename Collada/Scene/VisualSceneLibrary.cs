using System.Xml.Serialization;
using System.Collections.Generic;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Scene
{
    public class VisualSceneLibrary : AssetComponent
    {
        public VisualSceneLibrary()
        {
            Items = new List<VisualScene>();
        }

        [XmlElement("visual_scene")]
        public List<VisualScene> Items { get; set; }
    }
}