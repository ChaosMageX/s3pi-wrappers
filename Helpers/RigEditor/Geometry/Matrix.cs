using System;

namespace s3piwrappers.RigEditor.Geometry
{
    public struct Matrix
    {
        public static Matrix Identity = new Matrix(1d, 0d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 0d, 1d);
        public double M11, M12, M13, M14;
        public double M21, M22, M23, M24;
        public double M31, M32, M33, M34;
        public double M41, M42, M43, M44;


        public Matrix(double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double m41, double m42, double m43, double m44)
        {
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M21 = m21;
            M22 = m22;
            M23 = m23;
            M24 = m24;
            M31 = m31;
            M32 = m32;
            M33 = m33;
            M34 = m34;
            M41 = m41;
            M42 = m42;
            M43 = m43;
            M44 = m44;
        }

        public Matrix(AngleAxis a)
        {
            double x = a.Axis.X;
            double y = a.Axis.Y;
            double z = a.Axis.Z;
            double sin = Math.Sin(a.Angle);
            double cos = Math.Cos(a.Angle);
            double xx = x * x;
            double yy = y * y;
            double zz = z * z;
            double xy = x * y;
            double xz = x * z;
            double yz = y * z;
            M11 = xx + (cos * (1d - xx));
            M12 = (xy - (cos * xy)) + (sin * z);
            M13 = (xz - (cos * xz)) - (sin * y);
            M14 = 0d;
            M21 = (xy - (cos * xy)) - (sin * z);
            M22 = yy + (cos * (1d - yy));
            M23 = (yz - (cos * yz)) + (sin * x);
            M24 = 0d;
            M31 = (xz - (cos * xz)) + (sin * y);
            M32 = (yz - (cos * yz)) - (sin * x);
            M33 = zz + (cos * (1d - zz));
            M34 = 0d;
            M41 = 0d;
            M42 = 0d;
            M43 = 0d;
            M44 = 1d;
        }

        public Matrix(EulerAngle euler) : this(new Quaternion(euler)) { }
        public Matrix(Quaternion quaternion)
        {
            double sqx = quaternion.X * quaternion.X;
            double sqy = quaternion.Y * quaternion.Y;
            double sqz = quaternion.Z * quaternion.Z;
            double xy = quaternion.X * quaternion.Y;
            double zw = quaternion.Z * quaternion.W;
            double zx = quaternion.Z * quaternion.X;
            double yw = quaternion.Y * quaternion.W;
            double yz = quaternion.Y * quaternion.Z;
            double xw = quaternion.X * quaternion.W;
            M11 = 1d - (2D * (sqy + sqz));
            M12 = 2f * (xy + zw);
            M13 = 2f * (zx - yw);
            M14 = 0d;
            M21 = 2f * (xy - zw);
            M22 = 1d - (2D * (sqz + sqx));
            M23 = 2f * (yz + xw);
            M24 = 0d;
            M31 = 2f * (zx + yw);
            M32 = 2f * (yz - xw);
            M33 = 1d - (2D * (sqy + sqx));
            M34 = 0d;
            M41 = 0d;
            M42 = 0d;
            M43 = 0d;
            M44 = 1d;
        }
        public  Matrix GetInverse()
        {
            Matrix output;
            double a = (M33 * M44) - (M34 * M43);
            double b = (M32 * M44) - (M34 * M42);
            double c = (M32 * M43) - (M33 * M42);
            double d = (M31 * M44) - (M34 * M41);
            double e = (M31 * M43) - (M33 * M41);
            double f = (M31 * M42) - (M32 * M41);
            double g = ((M22 * a) - (M23 * b)) + (M24 * c);
            double h = -(((M21 * a) - (M23 * d)) + (M24 * e));
            double i = ((M21 * b) - (M22 * d)) + (M24 * f);
            double j = -(((M21 * c) - (M22 * e)) + (M23 * f));
            double k = 1d / ((((M11 * g) + (M12 * h)) + (M13 * i)) + (M14 * j));
            output.M11 = g * k;
            output.M21 = h * k;
            output.M31 = i * k;
            output.M41 = j * k;
            output.M12 = -(((M12 * a) - (M13 * b)) + (M14 * c)) * k;
            output.M22 = (((M11 * a) - (M13 * d)) + (M14 * e)) * k;
            output.M32 = -(((M11 * b) - (M12 * d)) + (M14 * f)) * k;
            output.M42 = (((M11 * c) - (M12 * e)) + (M13 * f)) * k;
            double l = (M23 * M44) - (M24 * M43);
            double m = (M22 * M44) - (M24 * M42);
            double n = (M22 * M43) - (M23 * M42);
            double o = (M21 * M44) - (M24 * M41);
            double p = (M21 * M43) - (M23 * M41);
            double q = (M21 * M42) - (M22 * M41);
            output.M13 = (((M12 * l) - (M13 * m)) + (M14 * n)) * k;
            output.M23 = -(((M11 * l) - (M13 * o)) + (M14 * p)) * k;
            output.M33 = (((M11 * m) - (M12 * o)) + (M14 * q)) * k;
            output.M43 = -(((M11 * n) - (M12 * p)) + (M13 * q)) * k;
            double r = (M23 * M34) - (M24 * M33);
            double s = (M22 * M34) - (M24 * M32);
            double t = (M22 * M33) - (M23 * M32);
            double u = (M21 * M34) - (M24 * M31);
            double v = (M21 * M33) - (M23 * M31);
            double w = (M21 * M32) - (M22 * M31);
            output.M14 = -(((M12 * r) - (M13 * s)) + (M14 * t)) * k;
            output.M24 = (((M11 * r) - (M13 * u)) + (M14 * v)) * k;
            output.M34 = -(((M11 * s) - (M12 * u)) + (M14 * w)) * k;
            output.M44 = (((M11 * t) - (M12 * v)) + (M13 * w)) * k;
            return output;
        }
        public Vector3 Translation
        {
            get
            {
                Vector3 vector;
                vector.X = M41;
                vector.Y = M42;
                vector.Z = M43;
                return vector;
            }
            set
            {
                M41 = value.X;
                M42 = value.Y;
                M43 = value.Z;
            }
        }
        public Vector3 RightVector
        {
            get
            {
                return new Vector3(M11, M12, M13);
            }
            set
            {
                M11 = value.X;
                M12 = value.Y;
                M13 = value.Z;
            }
        }
        public Vector3 UpVector
        {
            get { return new Vector3(M21, M22, M23); }
            set
            {
                M21 = value.X;
                M22 = value.Y;
                M23 = value.Z;
            }
        }
        public Vector3 BackVector
        {
            get { return new Vector3(M31, M32, M33); }
            set
            {
                M31 = value.X;
                M32 = value.Y;
                M33 = value.Z;
            }
        }
        public override string ToString()
        {
            return String.Format("{0}\n{1}\n{2}\n{3}\n", RightVector, UpVector, BackVector, Translation);
        }
    }
}
