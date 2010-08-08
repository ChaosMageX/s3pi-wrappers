using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using s3pi.Interfaces;

namespace s3piwrappers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Matrix3x3
    {
        public Vector3 M1 { get; set; }
        public Vector3 M2 { get; set; }
        public Vector3 M3 { get; set; }
    }
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Matrix4x3
    {
        public Vector3 M1 { get; set; }
        public Vector3 M2 { get; set; }
        public Vector3 M3 { get; set; }
        public Vector3 M4 { get; set; }
    }
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Matrix4x4
    {
        public Vector4 M1 { get; set; }
        public Vector4 M2 { get; set; }
        public Vector4 M3 { get; set; }
        public Vector4 M4 { get; set; }
    }

    public class EulerAngle : Vector3
    {
        public EulerAngle(){}
        public EulerAngle(double x, double y, double z) : base(x,y,z){}

        public static EulerAngle Parse(string s)
        {
            string[] split =s.Replace("Euler", "").TrimStart('(').TrimEnd(')').Split(',');
            double x, y, z;
            if(!(double.TryParse(split[0],out x)&&double.TryParse(split[1],out y) && double.TryParse(split[2],out z))) 
                throw new Exception("Unable to parse EulerAngle");
            return new EulerAngle(x,y,z);

        }
        public Quaternion ToQuaternion()
        {
            double c1 = Math.Cos(Geometry.DegToRad(Y) / 2);
            double c2 = Math.Cos(Geometry.DegToRad(Z) / 2);
            double c3 = Math.Cos(Geometry.DegToRad(X) / 2);
            double s1 = Math.Sin(Geometry.DegToRad(Y) / 2);
            double s2 = Math.Sin(Geometry.DegToRad(Z) / 2);
            double s3 = Math.Sin(Geometry.DegToRad(X) / 2);
            double w = Math.Sqrt(1.0 + c1 * c2 + c1 * c3 - s1 * s2 * s3 + c2 * c3) / 2;
            double x = (c2 * s3 + c1 * s3 + s1 * s2 * c3) / (4.0 * w);
            double y = (s1 * c2 + s1 * c3 + c1 * s2 * s3) / (4.0 * w);
            double z = (-s1 * s3 + c1 * s2 * c3 + s2) / (4.0 * w);
            return new Quaternion(x, y, z, w);
        }
    }
    [TypeConverter(typeof(Vector3TypeConverter))]
    public class Vector3
    {
        public Vector3()
        {
                
        }
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public override string ToString()
        {
            return string.Format("({0,6:0.0000},{1,6:0.0000},{2,6:0.0000})", X, Y, Z);
        }

        public static Vector3 Parse(string s)
        {
            string[] split = s.TrimStart('(').TrimEnd(')').Split(',');
            double x, y, z;
            if (!(double.TryParse(split[0], out x) && double.TryParse(split[1], out y) && double.TryParse(split[2], out z)))
                throw new Exception("Unable to parse Vector3");
            return new Vector3(x, y, z);

        }
    }
    [TypeConverter(typeof(Vector4TypeConverter))]
    public class Vector4 : Vector3
    {
        public Vector4(){}
        public Vector4(double x, double y, double z, double w):base(x,y,z){}
        
        public double W { get; set; }

        public override string ToString()
        {
            return string.Format("({0,6:0.0000},{1,6:0.0000},{2,6:0.0000},{3,6:0.0000})", X, Y, Z,W);
        }

        public static Vector4 Parse(string s)
        {
            string[] split = s.TrimStart('(').TrimEnd(')').Split(',');
            double x, y, z,w;
            if (!(double.TryParse(split[0], out x) && double.TryParse(split[1], out y) && double.TryParse(split[2], out z) && double.TryParse(split[3], out w)))
                throw new Exception("Unable to parse Vector4");
            return new Vector4(x, y, z,w);

        }
    }
    public class Quaternion : Vector4
    {
        public Quaternion() {  }
        public Quaternion(double x, double y, double z, double w) : base(x,y,z,w) { }

        public EulerAngle ToEuler()
        {
            Normalise();
            double sqX, sqY, sqZ, sqW;
            double x, y, z;
            sqX = Math.Pow(X, 2D);
            sqY = Math.Pow(Y, 2D);
            sqZ = Math.Pow(Z, 2D);
            sqW = Math.Pow(W, 2D);
            double poleTest = X * Y + Z * W;
            if (poleTest > 0.499D)
            {
                y = Geometry.RadToDeg(2 * Math.Atan2(X, W));
                z = Geometry.RadToDeg(Math.PI / 2);
                x = Geometry.RadToDeg(0);
            }
            else if (poleTest < 0.499D)
            {
                y = Geometry.RadToDeg(-2 * Math.Atan2(X, W));
                z = Geometry.RadToDeg(-Math.PI / 2);
                x = Geometry.RadToDeg(0);
            }
            else
            {
                y = Geometry.RadToDeg(Math.Atan2(2D * Y * W - 2 * X * Z, 1 - 2 * sqY - 2 * sqZ));
                z = Geometry.RadToDeg(Math.Asin(2 * poleTest));
                x = Geometry.RadToDeg(Math.Atan2(2D * X * W - 2 * Y * Z, 1 - 2 * sqX - 2 * sqZ));
            }
            return new EulerAngle(x,y,z);

        }
        public double Magnitude
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z + W * W); }
        }
        public void Normalise()
        {
            X /= Magnitude;
            Y /= Magnitude;
            Z /= Magnitude;
            W /= Magnitude;
        }

    }
    public class Geometry
    {
        public static double RadToRad(double rad)
        {
            return rad;
        }

        public static double RadToDeg(double rad)
        {
            return (rad * 180D) / Math.PI;
        }
        public static double DegToRad(double deg)
        {
            return (deg / 180D) * Math.PI;
        }
    }
    public class Vector3TypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) { return true; }
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return Vector3.Parse(value.ToString());
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return value.ToString();
        }
    }
    public class Vector4TypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) { return true; }
            return base.CanConvertFrom(context,sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return Vector4.Parse(value.ToString());
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            return value.ToString();
        }
    }
}
