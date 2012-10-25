using System;
using System.Globalization;
using System.Windows.Controls;

namespace s3piwrappers.AnimatedTextureEditor.Validation
{
    internal class HexadecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string str = ((string) value).Replace("x", "").Replace("X", "");
                Convert.ToUInt32(str, 16);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "*");
            }
            return new ValidationResult(true, null);
        }
    }
}
