using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace s3piwrappers.Collada.Common
{
    public class Accessor
    {
        public Accessor()
        {
            Offset = ((ulong) (0m));
            Stride = ((ulong) (1m));
            Params = new List<Param>();
        }


        [XmlElement("param")]
        public List<Param> Params { get; set; }


        [XmlAttribute("offset")]
        [DefaultValue(typeof (ulong), "0")]
        public ulong Offset { get; set; }

        [XmlAttribute("stride")]
        [DefaultValue(typeof (ulong), "1")]
        public ulong Stride { get; set; }

        [XmlAttribute("source", DataType = "anyURI")]
        public string Source { get; set; }

        [XmlAttribute("count")]
        public Int32 Count
        {
            get { return Params.Count; }
            set { }
        }
    }
}