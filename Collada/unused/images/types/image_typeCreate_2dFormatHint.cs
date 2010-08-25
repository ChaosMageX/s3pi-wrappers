using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DAESim.DAE
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public class image_typeCreate_2dFormatHint
    {
        private image_format_hint_channels_enum channelsField;

        private image_format_hint_precision_enum precisionField;
        private image_format_hint_range_enum rangeField;

        private string spaceField;

        public image_typeCreate_2dFormatHint()
        {
            precisionField = image_format_hint_precision_enum.DEFAULT;
        }


        [XmlAttribute]
        public image_format_hint_channels_enum channels
        {
            get { return channelsField; }
            set { channelsField = value; }
        }


        [XmlAttribute]
        public image_format_hint_range_enum range
        {
            get { return rangeField; }
            set { rangeField = value; }
        }


        [XmlAttribute]
        [DefaultValue(image_format_hint_precision_enum.DEFAULT)]
        public image_format_hint_precision_enum precision
        {
            get { return precisionField; }
            set { precisionField = value; }
        }


        [XmlAttribute(DataType = "token")]
        public string space
        {
            get { return spaceField; }
            set { spaceField = value; }
        }
    }
}