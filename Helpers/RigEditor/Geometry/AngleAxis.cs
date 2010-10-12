using System;

namespace s3piwrappers.RigEditor.Geometry
{
    public struct AngleAxis
    {
        public Vector3 Axis;
        public double Angle;

        public AngleAxis(AngleAxis a) : this(a.Axis, a.Angle) { }
        public AngleAxis(Vector3 axis, double angle)
        {
            Angle = angle;
            Axis = axis;
        }

        public AngleAxis(Quaternion q)
        {
            if (q.W > 1) q.Normalize();
            Angle = 2 * Math.Acos(q.W);
            double s = Math.Sqrt(1 - q.W * q.W);
            Axis.X = q.X;
            Axis.Y = q.Y;
            Axis.Z = q.Z;
            if (s >= 0.001)
            {
                Axis.X /= s;
                Axis.Y /= s;
                Axis.Z /= s;
            } 
        }

        public AngleAxis(EulerAngle e) : this(new Quaternion(e)) { }
    }
}
