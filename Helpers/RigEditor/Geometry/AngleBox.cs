using System.ComponentModel;
using System;
using s3piwrappers.RigEditor.Common;

namespace s3piwrappers.RigEditor.Geometry
{
    internal class AngleBox : FormattedTextBox<double>
    {
        public enum RotationUnit
        {
            Degrees, Radians
        }
        [Browsable(true)]
        public RotationUnit DisplayUnits { get; set; }
        private static double RadToDeg(double rad)
        {
            return (rad * 180f) / Math.PI;
        }
        private static double DegToRad(double deg)
        {
            return (deg / 180f) * Math.PI;
        }
        protected override bool Validate(string s)
        {
            double f;
            return double.TryParse(s, out f);
        }

        protected override double Parse(string s)
        {
            var val = double.Parse(s);
            if (DisplayUnits == RotationUnit.Degrees) val = DegToRad(val);
            return val;
        }
        protected override string FormatValue(double val)
        {
            if (DisplayUnits == RotationUnit.Degrees) val = RadToDeg(val);
            return val.ToString("N6");
        }
    }
}