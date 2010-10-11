using System;

namespace s3piwrappers.BoneTool.Geometry
{
    public struct Quaternion
    {
        public static Quaternion Identity = new Quaternion(0f, 0f, 0f, 1f);
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
            double y = e.Yaw * 0.5f;
            double siny = Math.Sin(y);
            double cosy = Math.Cos(y);
            double p = e.Pitch * 0.5f;
            double sinp = Math.Sin(p);
            double cosp = Math.Cos(p);
            double r = e.Roll * 0.5f;
            double sinr = Math.Sin(r);
            double cosr = Math.Cos(r);
            X = ((cosy * sinp) * cosr) + ((siny * cosp) * sinr);
            Y = ((siny * cosp) * cosr) - ((cosy * sinp) * sinr);
            Z = ((cosy * cosp) * sinr) - ((siny * sinp) * cosr);
            W = ((cosy * cosp) * cosr) + ((siny * sinp) * sinr);

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
            X /= magnitude;
            Y /= magnitude;
            Z /= magnitude;
            W /= magnitude;
        }
    }
}
