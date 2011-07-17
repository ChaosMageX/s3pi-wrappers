using System;
using System.Windows.Controls;

namespace s3piwrappers.AnimatedTextureEditor.Validation
{
    class HexadecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var str = ((string) value).Replace("x", "").Replace("X", "");
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