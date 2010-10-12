using System;

namespace s3piwrappers.RigEditor.Common
{
    internal class IntBox : FormattedTextBox<int>
    {
        protected override bool Validate(string s)
        {
            Int32 i;
            return Int32.TryParse(s, out i);
        }

        protected override Int32 Parse(string s)
        {
            return Int32.Parse(s);
        }

        protected override string FormatValue(Int32 val)
        {
            return val.ToString();
        }
    }
}