using System;
using System.Globalization;
using System.Windows.Controls;

namespace s3piwrappers.RigEditor.Validation
{
    internal class RequiredFieldValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return !String.IsNullOrEmpty(value.ToString()) ? new ValidationResult(true, null) : new ValidationResult(false, "Field cannot be blank.");
        }
    }
}
