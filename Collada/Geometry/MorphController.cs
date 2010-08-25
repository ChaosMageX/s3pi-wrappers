using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using s3piwrappers.Collada.Common;
using s3piwrappers.Collada.Controllers;

namespace s3piwrappers.Collada.Geometry
{
    public class MorphController : ControllerData
    {
        public MorphController()
        {
            Method = MorphMethod.NORMALIZED;
            Sources = new List<Source>();
        }


        [XmlElement("source")]
        public List<Source> Sources { get; set; }

        [XmlElement("targets")]
        public MorphTargets Targets { get; set; }

        [XmlAttribute("method")]
        [DefaultValue(MorphMethod.NORMALIZED)]
        public MorphMethod Method { get; set; }

    }
}