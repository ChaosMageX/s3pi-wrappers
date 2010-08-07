using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using s3pi.Interfaces;

namespace s3piwrappers
{
    public struct EulerAngle
    {
        public EulerAngle(double x, double y, double z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        [ElementPriority(1)]
        public double X { get; set; }

        [ElementPriority(2)]
        public double Y { get; set; }

        [ElementPriority(3)]
        public double Z { get; set; }

        public override string ToString()
        {
            return string.Format("Euler({0,6:0.0000},{1,6:0.0000},{2,6:0.0000})", X, Y, Z);
        }
        public Quaternion ToQuaternion()
        {
            Quaternion q = new Quaternion();
            return q;
        }
    }
    public struct Vector3
    {
        public Vector3(double x, double y, double z) :this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        [ElementPriority(1)]
        public double X { get; set; }

        [ElementPriority(2)]
        public double Y { get; set; }

        [ElementPriority(3)]
        public double Z { get; set; }

        public override string ToString()
        {
            return string.Format("({0,6:0.0000},{1,6:0.0000},{2,6:0.0000})", X, Y, Z);
        }
    }
    public struct Vector4
    {
        public Vector4( double x, double y, double z, double w):this()
        {
            X = x;
            Y = y;
            Z = z;
            W = w; 
        }


        [ElementPriority(1)]
        public double X { get; set; }

        [ElementPriority(2)]
        public double Y { get; set; }

        [ElementPriority(3)]
        public double Z { get; set; }

        [ElementPriority(4)]
        public double W { get; set; }

        public override string ToString()
        {
            return string.Format("({0,6:0.0000},{1,6:0.0000},{2,6:0.0000},{3,6:0.0000})", X, Y, Z, W);
        }
    }

    public struct Quaternion 
    {
         public Quaternion(double x, double y, double z, double w):this()
        {
            X = x;
            Y = y;
            Z = z;
            W = w; 
        }


        [ElementPriority(1)]
        public double X { get; set; }

        [ElementPriority(2)]
        public double Y { get; set; }

        [ElementPriority(3)]
        public double Z { get; set; }

        [ElementPriority(4)]
        public double W { get; set; }
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
                euler.Y = RadToDeg(Math.Atan2(2D * Y * W - 2 * X * Z, 1 - 2 * sqY - 2 * sqZ));
                euler.Z = RadToDeg(Math.Asin(2 * (X * Y + Z * W)));
                euler.X = RadToDeg(Math.Atan2(2D * X * W - 2 * Y * Z, 1 - 2 * sqX - 2 * sqZ));
                return euler;
            }
        }
        static double RadToDeg(double rad)
        {
            return (rad * 180D) / Math.PI;
        }
        static double DegToRad(double deg)
        {
            return (deg / 180D) * Math.PI;
        }
        public override string ToString()
        {
            return string.Format("Quat({0,6:0.0000},{1,6:0.0000},{2,6:0.0000},{3,6:0.0000})", X, Y, Z, W);
        }
    }
}
