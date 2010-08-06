using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace s3piwrappers.Geometry
{
    public class EulerAngle : Vector3
    {
        public EulerAngle() : base() { }
        public EulerAngle(double x, double y, double z) : base(x, y, z) { }
        public Quaternion ToQuaternion()
        {
            Quaternion q = new Quaternion();
            return q;
        }
        public override string ToString()
        {
            return "Euler"+base.ToString();
        }
    }
}
