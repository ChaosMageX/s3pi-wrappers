using System;
using System.Globalization;
using System.Windows.Controls;

namespace s3piwrappers.AnimatedTextureEditor.Validation
{
    internal class DecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                Double.Parse((String) value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "*");
            }
            return new ValidationResult(true, null);
        }
    }
}
