using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.Geometry
{
    public class Quaternion : Vector4
    {
        public Quaternion() : base() { }
        public Quaternion(double x, double y, double z, double w) : base(x, y, z, w) { }
        public EulerAngle Euler
        {
            get
            {
                double sqX, sqY, sqZ, sqW;
                EulerAngle euler = new EulerAngle();
                sqX = Math.Pow(X, 2D);
                sqY = Math.Pow(Y, 2D);
                sqZ = Math.Pow(Z, 2D);
                sqW = Math.Pow(W, 2D);
                euler.Y = Math.Atan2(2D * Y * W - 2 * X * Z, 1 - 2 * sqY - 2 * sqZ);
                euler.Z = Math.Asin(2 * (X * Y + Z * W));
                euler.X = Math.Atan2(2D * X * W - 2 * Y * Z, 1 - 2 * sqX - 2 * sqZ);
                return euler;
            }
        }
        public override string ToString()
        {
            return "Quat"+base.ToString();
        }
    }
}
