using System;
using System.Xml.Serialization;

namespace s3piwrappers.Collada.Scene
{
    public class Rotate : NodeTransformer
    {
        [XmlIgnore]
        public double X { get; set; }

        [XmlIgnore]
        public double Y { get; set; }

        [XmlIgnore]
        public double Z { get; set; }

        [XmlIgnore]
        public double Angle { get; set; }

        [XmlText]
        public string Text
        {
            get { return String.Format("{0} {1} {2} {3}", X, Y, Z, Angle); }
            set
            {
                var vals = value.Split(' ');
                X = Double.Parse(vals[0]);
                Y = Double.Parse(vals[1]);
                Z = Double.Parse(vals[2]);
                Angle = Double.Parse(vals[3]);
            }
        }
    }
}