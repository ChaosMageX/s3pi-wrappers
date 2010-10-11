using System;

namespace s3piwrappers.BoneTool
{
    internal class DoubleBox : FormattedTextBox<double>
    {
        protected override bool Validate(string s)
        {
            double f;
            return double.TryParse(s, out f);
        }

        protected override double Parse(string s)
        {
            return double.Parse(s);
        }

        protected override string FormatValue(double val)
        {
            return val.ToString("N6");
        }
    }
}