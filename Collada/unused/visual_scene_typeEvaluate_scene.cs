using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class visual_scene_typeEvaluate_scene : AssetBase
    {
        private bool enableField;

        private visual_scene_typeEvaluate_sceneRender[] renderField;
        private string sidField;

        public visual_scene_typeEvaluate_scene()
        {
            enableField = true;
        }
        

        [XmlElement("render")]
        public visual_scene_typeEvaluate_sceneRender[] render
        {
            get { return renderField; }
            set { renderField = value; }
        }

        [XmlAttribute(DataType = "NCName")]
        public string sid
        {
            get { return sidField; }
            set { sidField = value; }
        }

        [XmlAttribute]
        [DefaultValue(true)]
        public bool enable
        {
            get { return enableField; }
            set { enableField = value; }
        }
    }
}