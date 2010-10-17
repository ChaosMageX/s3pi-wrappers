using System;

namespace s3piwrappers.RigEditor.Geometry
{
    public struct Matrix
    {
        public static Matrix Identity = new Matrix(1d, 0d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 0d, 1d, 0d, 0d, 0d, 0d, 1d);
        public double M00, M01, M02, M03;
        public double M10, M11, M12, M13;
        public double M20, M21, M22, M23;
        public double M30, M31, M32, M33;


        public Matrix(double m00, double m01, double m02, double m03, double m10, double m11, double m12, double m13, double m20, double m21, double m22, double m23, double m30, double m31, double m32, double m33)
        {
            M00 = m00;
            M01 = m01;
            M02 = m02;
            M03 = m03;

            M10 = m10;
            M11 = m11;
            M12 = m12;
            M13 = m13;

            M20 = m20;
            M21 = m21;
            M22 = m22;
            M23 = m23;

            M30 = m30;
            M31 = m31;
            M32 = m32;
            M33 = m33;
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
            M00 = xx + (cos * (1d - xx));
            M01 = (xy - (cos * xy)) + (sin * z);
            M02 = (xz - (cos * xz)) - (sin * y);
            M03 = 0d;

            M10 = (xy - (cos * xy)) - (sin * z);
            M11 = yy + (cos * (1d - yy));
            M12 = (yz - (cos * yz)) + (sin * x);
            M13 = 0d;

            M20 = (xz - (cos * xz)) + (sin * y);
            M21 = (yz - (cos * yz)) - (sin * x);
            M22 = zz + (cos * (1d - zz));
            M23 = 0d;

            M30 = 0d;
            M31 = 0d;
            M32 = 0d;
            M33 = 1d;
        }

        public Matrix(EulerAngle euler) : this(new Quaternion(euler)) { }
        public Matrix(Quaternion q)
        {
            double sqx = q.X * q.X;
            double sqy = q.Y * q.Y;
            double sqz = q.Z * q.Z;
            double sqw = q.W * q.W;

            double xy = q.X * q.Y;
            double zw = q.Z * q.W;
            double xz = q.X * q.Z;
            double yw = q.Y * q.W;
            double yz = q.Y * q.Z;
            double xw = q.X * q.W;
            M00 = (sqx - sqy - sqz + sqw);
            M01 = 2.0 * (xy - zw);
            M02 = 2.0 * (xz + yw);
            M03 = 0d;

            M10 = 2.0 * (xy + zw);
            M11 = (-sqx + sqy - sqz + sqw);
            M12 = 2.0 * (yz - xw);
            M13 = 0d;

            M20 = 2.0 * (xz - yw);
            M21 = 2.0 * (yz + xw);
            M22 = (-sqx - sqy + sqz + sqw);
            M23 = 0d;

            M30 = 0d;
            M31 = 0d;
            M32 = 0d;
            M33 = 1d;
        }
        public void Scale(double d)
        {
            M00 *= d;
            M01 *= d;
            M02 *= d;
            M03 *= d;

            M10 *= d;
            M11 *= d;
            M12 *= d;
            M13 *= d;

            M20 *= d;
            M21 *= d;
            M22 *= d;
            M23 *= d;

            M30 *= d;
            M31 *= d;
            M32 *= d;
            M33 *= d;
        }
        public Matrix GetInverse()
        {
            Matrix m;
            m.M00 = M12 * M23 * M31 - M13 * M22 * M31 + M13 * M21 * M32 - M11 * M23 * M32 - M12 * M21 * M33 + M11 * M22 * M33;
            m.M01 = M03 * M22 * M31 - M02 * M23 * M31 - M03 * M21 * M32 + M01 * M23 * M32 + M02 * M21 * M33 - M01 * M22 * M33;
            m.M02 = M02 * M13 * M31 - M03 * M12 * M31 + M03 * M11 * M32 - M01 * M13 * M32 - M02 * M11 * M33 + M01 * M12 * M33;
            m.M03 = M03 * M12 * M21 - M02 * M13 * M21 - M03 * M11 * M22 + M01 * M13 * M22 + M02 * M11 * M23 - M01 * M12 * M23;

            m.M10 = M13 * M22 * M30 - M12 * M23 * M30 - M13 * M20 * M32 + M10 * M23 * M32 + M12 * M20 * M33 - M10 * M22 * M33;
            m.M11 = M02 * M23 * M30 - M03 * M22 * M30 + M03 * M20 * M32 - M00 * M23 * M32 - M02 * M20 * M33 + M00 * M22 * M33;
            m.M12 = M03 * M12 * M30 - M02 * M13 * M30 - M03 * M10 * M32 + M00 * M13 * M32 + M02 * M10 * M33 - M00 * M12 * M33;
            m.M13 = M02 * M13 * M20 - M03 * M12 * M20 + M03 * M10 * M22 - M00 * M13 * M22 - M02 * M10 * M23 + M00 * M12 * M23;

            m.M20 = M11 * M23 * M30 - M13 * M21 * M30 + M13 * M20 * M31 - M10 * M23 * M31 - M11 * M20 * M33 + M10 * M21 * M33;
            m.M21 = M03 * M21 * M30 - M01 * M23 * M30 - M03 * M20 * M31 + M00 * M23 * M31 + M01 * M20 * M33 - M00 * M21 * M33;
            m.M22 = M01 * M13 * M30 - M03 * M11 * M30 + M03 * M10 * M31 - M00 * M13 * M31 - M01 * M10 * M33 + M00 * M11 * M33;
            m.M23 = M03 * M11 * M20 - M01 * M13 * M20 - M03 * M10 * M21 + M00 * M13 * M21 + M01 * M10 * M23 - M00 * M11 * M23;

            m.M30 = M12 * M21 * M30 - M11 * M22 * M30 - M12 * M20 * M31 + M10 * M22 * M31 + M11 * M20 * M32 - M10 * M21 * M32;
            m.M31 = M01 * M22 * M30 - M02 * M21 * M30 + M02 * M20 * M31 - M00 * M22 * M31 - M01 * M20 * M32 + M00 * M21 * M32;
            m.M32 = M02 * M11 * M30 - M01 * M12 * M30 - M02 * M10 * M31 + M00 * M12 * M31 + M01 * M10 * M32 - M00 * M11 * M32;
            m.M33 = M01 * M12 * M20 - M02 * M11 * M20 + M02 * M10 * M21 - M00 * M12 * M21 - M01 * M10 * M22 + M00 * M11 * M22;
            m.Scale(1 / Determinant());
            return m;
        }
        public double Determinant()
        {
            return M03 * M12 * M21 * M30 - M02 * M13 * M21 * M30 -
                    M03 * M11 * M22 * M30 + M01 * M13 * M22 * M30 +
                    M02 * M11 * M23 * M30 - M01 * M12 * M23 * M30 -
                    M03 * M12 * M20 * M31 + M02 * M13 * M20 * M31 +
                    M03 * M10 * M22 * M31 - M00 * M13 * M22 * M31 -
                    M02 * M10 * M23 * M31 + M00 * M12 * M23 * M31 +
                    M03 * M11 * M20 * M32 - M01 * M13 * M20 * M32 -
                    M03 * M10 * M21 * M32 + M00 * M13 * M21 * M32 +
                    M01 * M10 * M23 * M32 - M00 * M11 * M23 * M32 -
                    M02 * M11 * M20 * M33 + M01 * M12 * M20 * M33 +
                    M02 * M10 * M21 * M33 - M00 * M12 * M21 * M33 -
                    M01 * M10 * M22 * M33 + M00 * M11 * M22 * M33;

        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            Matrix m;
            m.M00 = a.M00 * b.M00 + a.M01 * b.M10 + a.M02 * b.M20 + a.M03 * b.M30;
            m.M01 = a.M00 * b.M01 + a.M01 * b.M11 + a.M02 * b.M21 + a.M03 * b.M31;
            m.M02 = a.M00 * b.M02 + a.M01 * b.M12 + a.M02 * b.M22 + a.M03 * b.M32;
            m.M03 = a.M00 * b.M03 + a.M01 * b.M13 + a.M02 * b.M23 + a.M03 * b.M33;

            m.M10 = a.M10 * b.M00 + a.M11 * b.M10 + a.M12 * b.M20 + a.M13 * b.M30;
            m.M11 = a.M10 * b.M01 + a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31;
            m.M12 = a.M10 * b.M02 + a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32;
            m.M13 = a.M10 * b.M03 + a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33;

            m.M20 = a.M20 * b.M00 + a.M21 * b.M10 + a.M22 * b.M20 + a.M23 * b.M30;
            m.M21 = a.M20 * b.M01 + a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31;
            m.M22 = a.M20 * b.M02 + a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32;
            m.M23 = a.M20 * b.M03 + a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33;

            m.M30 = a.M30 * b.M00 + a.M31 * b.M10 + a.M32 * b.M20 + a.M33 * b.M30;
            m.M31 = a.M30 * b.M01 + a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31;
            m.M32 = a.M30 * b.M02 + a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32;
            m.M33 = a.M30 * b.M03 + a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33;

            return m;
        }
        public Vector3 RightVector
        {
            get
            {
                return new Vector3(M00, M10, M20);
            }
            set
            {
                M00 = value.X;
                M10 = value.Y;
                M20 = value.Z;
            }
        }
        public Vector3 UpVector
        {
            get { return new Vector3(M01, M11, M21); }
            set
            {
                M01 = value.X;
                M11 = value.Y;
                M21 = value.Z;
            }
        }
        public Vector3 BackVector
        {
            get { return new Vector3(M02, M12, M22); }
            set
            {
                M02 = value.X;
                M12 = value.Y;
                M22 = value.Z;
            }
        }
        public Vector3 Translation
        {
            get
            {
                return new Vector3(M03, M13, M23);
            }
            set
            {
                M03 = value.X;
                M13 = value.Y;
                M23 = value.Z;
            }
        }
        public override string ToString()
        {
            return String.Format("{0}\r\n{1}\r\n{2}\r\n{3}\r\n", RightVector, UpVector, BackVector, Translation);
        }
    }
}
