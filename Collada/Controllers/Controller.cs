using System.Xml.Serialization;
using s3piwrappers.Collada.Geometry;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Controllers
{
    
    
    public abstract class Controller : AssetComponent
    {
        [XmlElement("skin", typeof (SkinController))]
        [XmlElement("morph", typeof (MorphController))]
        public ControllerData Data { get; set; }
    }
}