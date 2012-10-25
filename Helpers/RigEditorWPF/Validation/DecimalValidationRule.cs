using System;
using System.Globalization;
using System.Windows.Controls;

namespace s3piwrappers.RigEditor.Validation
{
    internal class DecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double parameter = 0;

            try
            {
                if (((string) value).Length > 0)
                {
                    parameter = Double.Parse((String) value);
                }
            }
            catch (Exception)
            {
                return new ValidationResult(false, "*");
            }


            return new ValidationResult(true, null);
        }
    }
}
