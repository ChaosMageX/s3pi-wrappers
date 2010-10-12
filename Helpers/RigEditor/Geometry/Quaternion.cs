using System;

namespace s3piwrappers.RigEditor.Geometry
{
    public struct Quaternion
    {
        public static Quaternion Identity = new Quaternion(0d, 0d, 0d, 1d);
        public double X, Y, Z, W;
        public Quaternion(Quaternion q):this(q.X,q.Y,q.Z,q.W){}
        public Quaternion(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Quaternion(EulerAngle e)
        {
            double c1 = Math.Cos(e.Yaw * 0.5d);
            double c2 = Math.Cos(e.Pitch * 0.5d);
            double c3 = Math.Cos(e.Roll * 0.5d);
            
            double s1 = Math.Sin(e.Yaw * 0.5d);
            double s2 = Math.Sin(e.Pitch * 0.5d);
            double s3 = Math.Sin(e.Roll * 0.5d);
            W = c1 * c2 * c3 - s1 * s2 * s3;
            X = s1 * s2 * c3 + c1 * c2 * s3;
            Y = s1 * c2 * c3 + c1 * s2 * s3;
            Z = c1 * s2 * c3 - s1 * c2 * s3;

        }
        public Quaternion(AngleAxis angleAxis)
        {
            double s = Math.Sin(angleAxis.Angle / 2);
            X = angleAxis.Axis.X * s;
            Y = angleAxis.Axis.Y * s;
            Z = angleAxis.Axis.Z * s;
            W = Math.Cos(angleAxis.Angle / 2);

        }
        public double Magnitude()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public void Normalize()
        {
            
            double magnitude = Magnitude();
            if (magnitude != 0)
            {
                X /= magnitude;
                Y /= magnitude;
                Z /= magnitude;
                W /= magnitude;
            }
        }
    }
}
