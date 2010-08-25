using System;
using System.Xml.Serialization;

namespace s3piwrappers.Collada.Scene
{
    public class Matrix : NodeTransformer
    {
        private readonly double[][] m;

        public Matrix()
        {
            m = new double[4][];
            for (int i = 0; i < 4; i++)
            {
                m[i] = new double[4];
            }
        }


        [XmlIgnore]
        public double this[int row, int col]
        {
            get { return m[row][col]; }
            set { m[row][col] = value; }
        }

        [XmlText]
        public string Text
        {
            get
            {
                return String.Format
                    (
                        "{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15}"
                        , m[0][0], m[1][0], m[2][0], m[3][0]
                        , m[0][1], m[1][1], m[2][1], m[3][1]
                        , m[0][2], m[1][2], m[2][2], m[3][2]
                        , m[0][3], m[1][3], m[2][3], m[3][3]
                    );
            }
            set
            {
                var vals = value.Split(' ');
                m[0][0] = double.Parse(vals[0]);
                m[1][0] = double.Parse(vals[1]);
                m[2][0] = double.Parse(vals[2]);
                m[3][0] = double.Parse(vals[3]);

                m[0][1] = double.Parse(vals[4]);
                m[1][1] = double.Parse(vals[5]);
                m[2][1] = double.Parse(vals[6]);
                m[3][1] = double.Parse(vals[7]);

                m[0][2] = double.Parse(vals[8]);
                m[1][2] = double.Parse(vals[9]);
                m[2][2] = double.Parse(vals[10]);
                m[3][2] = double.Parse(vals[11]);

                m[0][3] = double.Parse(vals[12]);
                m[1][3] = double.Parse(vals[13]);
                m[2][3] = double.Parse(vals[14]);
                m[3][3] = double.Parse(vals[15]);
            }
        }
    }
}