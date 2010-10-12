using System;

namespace s3piwrappers.RigEditor.Geometry
{
    public struct Vector3
    {
        public double X, Y, Z;
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public override string ToString()
        {
            return String.Format("[{0,8:0.00000}\n{1,8:0.00000}\n{2,8:0.00000}]", X, Y, Z);
        }
        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public void Normalize()
        {

            double magnitude = Magnitude();
            X /= magnitude;
            Y /= magnitude;
            Z /= magnitude;
        }
    }
}
