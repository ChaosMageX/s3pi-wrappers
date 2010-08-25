using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Metadata
{
    public class Asset : ExtendableComponent
    {
        public Asset()
        {
            Contributors = new List<Contributor>();
            Units = new UnitType();
            UpAxis = UpAxis.Y_UP;
        }


        [XmlElement("contributor")]
        public List<Contributor> Contributors { get; set; }

        [XmlElement("coverage_type")]
        public Coverage CoverageType { get; set; }

        [XmlElement("created")]
        public string Created { get; set; }


        [XmlElement("keywords", DataType = "token")]
        public string Keywords { get; set; }


        [XmlElement("modified")]
        public string Modified { get; set; }


        [XmlElement("revision")]
        public string Revision { get; set; }


        [XmlElement("subject")]
        public string Subject { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }


        [XmlElement("unit")]
        public UnitType Units { get; set; }


        [DefaultValue(UpAxis.Y_UP)]
        [XmlElement("up_axis")]
        public UpAxis UpAxis { get; set; }
    }
}