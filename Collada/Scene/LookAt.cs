using System;
using System.Xml.Serialization;

namespace s3piwrappers.Collada.Scene
{
    public class LookAt : NodeTransformer
    {
        [XmlIgnore]
        public double PX { get; set; }

        [XmlIgnore]
        public double PY { get; set; }

        [XmlIgnore]
        public double PZ { get; set; }

        [XmlIgnore]
        public double IX { get; set; }

        [XmlIgnore]
        public double IY { get; set; }

        [XmlIgnore]
        public double IZ { get; set; }

        [XmlIgnore]
        public double UpX { get; set; }

        [XmlIgnore]
        public double UpY { get; set; }

        [XmlIgnore]
        public double UpZ { get; set; }

        [XmlText]
        public string Text
        {
            get
            {
                String s = "";
                s += String.Format("{0} {1} {2}\n", PX, PY, PZ);
                s += String.Format("{0} {1} {2}\n", IX, IY, IZ);
                s += String.Format("{0} {1} {2}", UpX, UpY, UpZ);
                return s;
            }
            set
            {
                string s = value.Replace("n", "").Replace("\r", "");
                var split = s.Split(' ');
                PX = double.Parse(split[0]);
                PY = double.Parse(split[1]);
                PZ = double.Parse(split[2]);

                IX = double.Parse(split[3]);
                IX = double.Parse(split[4]);
                IX = double.Parse(split[5]);

                UpX = double.Parse(split[6]);
                UpX = double.Parse(split[7]);
                UpX = double.Parse(split[8]);
            }
        }
    }
}