using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using s3piwrappers.Collada.Animation;
using s3piwrappers.Collada.Common;
using s3piwrappers.Collada.Controllers;
using s3piwrappers.Collada.Geometry;
using s3piwrappers.Collada.Interface;
using s3piwrappers.Collada.Metadata;
using s3piwrappers.Collada.Scene;

namespace s3piwrappers.Collada
{
    [XmlRoot("COLLADA", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
    public class COLLADA: IExtendable
    {
        public COLLADA()
        {
            Asset = new Asset();
            Scene = new COLLADAScene();
        }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlElement("asset", typeof(Asset))]
        public Asset Asset { get; set; }

        [XmlElement("library_animations", typeof(AnimationLibrary))]
        public AnimationLibrary Animations { get; set; }

        [XmlElement("library_animation_clips", typeof(AnimationClipLibrary))]
        public AnimationClipLibrary Clips { get; set; }

        [XmlElement("library_controllers", typeof(ControllerLibrary))]
        public ControllerLibrary Controllers { get; set; }

        [XmlElement("library_geometries", typeof(GeometryLibrary))]
        public GeometryLibrary Geometries { get; set; }

        [XmlElement("library_nodes", typeof(NodeLibrary))]
        public NodeLibrary Nodes { get; set; }

        [XmlElement("library_visual_scenes", typeof(VisualSceneLibrary))]
        public VisualSceneLibrary VisualScenes { get; set; }

        [XmlElement("scene")]
        public COLLADAScene Scene { get; set; }

        [XmlElement("extra")]
        public IList<Extra> Extra { get; set; }

        public void Save(Stream s)
        {
            var xns = new XmlSerializerNamespaces();
            xns.Add("", "http://www.collada.org/2005/11/COLLADASchema");
            var writer = XmlWriter.Create(s, new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(false) });
            var xs = new XmlSerializer(typeof(COLLADA));
            xs.Serialize(writer, this, xns);
            writer.Flush();
            writer.Close();
        }

        public void Load(Stream s)
        {
            var xs = new XmlSerializer(typeof(COLLADA));
            xs.Deserialize(s);
        }
    }
}