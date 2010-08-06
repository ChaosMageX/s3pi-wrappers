using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.Geometry
{
    public class Vector2
    {
        public Vector2() { }
        public Vector2(double x, double y) { X = x; Y = y; }
        public double X { get; set; }
        public double Y { get; set; }
        public override string ToString()
        {
            return string.Format("({0,6:0.0000},{1,6:0.0000})", X, Y);
        }

    }
    public class Vector3 : Vector2
    {
        public Vector3() : base() { }
        public Vector3(double x, double y, double z) : base(x, y) { Z = z; }
        public double Z { get; set; }
        public override string ToString()
        {
            return string.Format("({0,6:0.0000},{1,6:0.0000},{2,6:0.0000})",X,Y,Z);
        }
    }
    public class Vector4 : Vector3
    {
        public Vector4() : base() { }
        public Vector4(double x, double y, double z, double w) : base(x, y, z) { W = w; }
        public double W { get; set; }
        public override string ToString()
        {
            return string.Format("({0,6:0.0000},{1,6:0.0000},{2,6:0.0000},{3,6:0.0000})",X,Y,Z,W);
        }
    }
}
