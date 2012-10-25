using System;

namespace s3piwrappers.RigEditor.Geometry
{
    public struct Quaternion
    {
        public static Quaternion Identity = new Quaternion(0d, 0d, 0d, 1d);
        public double X, Y, Z, W;

        public Quaternion(Quaternion q) : this(q.X, q.Y, q.Z, q.W)
        {
        }

        public Quaternion(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Quaternion(EulerAngle e)
        {
            double c1 = Math.Cos(e.Yaw*0.5d);
            double c2 = Math.Cos(e.Pitch*0.5d);
            double c3 = Math.Cos(e.Roll*0.5d);
            double s1 = Math.Sin(e.Yaw*0.5d);
            double s2 = Math.Sin(e.Pitch*0.5d);
            double s3 = Math.Sin(e.Roll*0.5d);
            W = c1*c2*c3 - s1*s2*s3;
            X = s1*s2*c3 + c1*c2*s3;
            Y = s1*c2*c3 + c1*s2*s3;
            Z = c1*s2*c3 - s1*c2*s3;
        }

        public Quaternion(Matrix m1)
        {
            double tr = m1.M00 + m1.M11 + m1.M22;
            if (tr > 0)
            {
                double S = Math.Sqrt(tr + 1.0)*2;
                W = 0.25*S;
                X = (m1.M21 - m1.M12)/S;
                Y = (m1.M02 - m1.M20)/S;
                Z = (m1.M10 - m1.M01)/S;
            }
            else if ((m1.M00 > m1.M11) & (m1.M00 > m1.M22))
            {
                double S = Math.Sqrt(1.0 + m1.M00 - m1.M11 - m1.M22)*2;
                W = (m1.M21 - m1.M12)/S;
                X = 0.25*S;
                Y = (m1.M01 + m1.M10)/S;
                Z = (m1.M02 + m1.M20)/S;
            }
            else if (m1.M11 > m1.M22)
            {
                double S = Math.Sqrt(1.0 + m1.M11 - m1.M00 - m1.M22)*2;
                W = (m1.M02 - m1.M20)/S;
                X = (m1.M01 + m1.M10)/S;
                Y = 0.25*S;
                Z = (m1.M12 + m1.M21)/S;
            }
            else
            {
                double S = Math.Sqrt(1.0 + m1.M22 - m1.M00 - m1.M11)*2;
                W = (m1.M10 - m1.M01)/S;
                X = (m1.M02 + m1.M20)/S;
                Y = (m1.M12 + m1.M21)/S;
                Z = 0.25*S;
            }
        }

        public double Magnitude()
        {
            return Math.Sqrt(X*X + Y*Y + Z*Z + W*W);
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
