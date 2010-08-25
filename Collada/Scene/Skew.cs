using System;
using System.Xml.Serialization;

namespace s3piwrappers.Collada.Scene
{
    public class Skew : NodeTransformer
    {
        [XmlIgnore]
        public double Angle { get; set; }

        [XmlIgnore]
        public double RotationAxisX { get; set; }

        [XmlIgnore]
        public double RotationAxisY { get; set; }

        [XmlIgnore]
        public double RotationAxisZ { get; set; }

        [XmlIgnore]
        public double TranslationAxisX { get; set; }

        [XmlIgnore]
        public double TranslationAxisY { get; set; }

        [XmlIgnore]
        public double TranslationAxisZ { get; set; }

        [XmlText]
        public string Text
        {
            get
            {
                String s = Angle.ToString();
                s += String.Format(" {0} {1} {2}", RotationAxisX, RotationAxisY, RotationAxisZ);
                s += String.Format(" {0} {1} {2}", TranslationAxisX, TranslationAxisY, TranslationAxisZ);
                return s;
            }
            set
            {
                var vals = value.Split(' ');
                Angle = Double.Parse(vals[0]);
                RotationAxisX = Double.Parse(vals[0]);
                RotationAxisY = Double.Parse(vals[0]);
                RotationAxisZ = Double.Parse(vals[0]);
                TranslationAxisX = Double.Parse(vals[0]);
                TranslationAxisY = Double.Parse(vals[0]);
                TranslationAxisZ = Double.Parse(vals[0]);
            }
        }
    }
}