using System;
namespace s3piwrappers.BoneTool.Geometry
{
    public struct EulerAngle
    {
        public double Yaw, Pitch, Roll;
        public EulerAngle(double y, double p, double r)
        {
            Yaw = y;
            Pitch = p;
            Roll = r;
        }

        public EulerAngle(Quaternion q)
        {
            double poleTest = q.X * q.Y + q.Z * q.W;
            if (poleTest > 0.499)
            {
                Yaw = 2 * Math.Atan2(q.X, q.W);
                Pitch = Math.PI / 2;
                Roll = 0;
                return;
            }
            if (poleTest < -0.499)
            {
                Yaw = -2 * Math.Atan2(q.X, q.W);
                Pitch = -Math.PI / 2;
                Roll = 0;
                return;
            }
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;
            Yaw = Math.Atan2(2 * q.Y * q.W - 2 * q.X * q.Z, 1 - 2 * sqy - 2 * sqz);
            Pitch = Math.Asin(2 * poleTest);
            Roll = Math.Atan2(2 * q.X * q.W - 2 * q.Y * q.Z, 1 - 2 * sqx - 2 * sqz);
        }
        public EulerAngle(AngleAxis a):this(new Quaternion(a)){}
        public EulerAngle(Matrix m)
        {
            if (m.M21 > 0.998)
            {
                Yaw = Math.Atan2(m.M13, m.M33);
                Pitch = Math.PI / 2;
                Roll = 0;
                return;
            }
            if (m.M21 < -0.998)
            {
                Yaw = Math.Atan2(m.M13, m.M33);
                Pitch = -Math.PI / 2;
                Roll = 0;
                return;
            }
            Yaw = Math.Atan2(-m.M31, m.M11);
            Pitch = Math.Atan2(-m.M23, m.M22);
            Roll = Math.Asin(m.M21);

        }


    }
}
