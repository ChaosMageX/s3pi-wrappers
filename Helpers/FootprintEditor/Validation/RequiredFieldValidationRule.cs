using System;
using System.Windows.Controls;

namespace FootprintViewer.Validation
{
    class RequiredFieldValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return !String.IsNullOrEmpty(value.ToString())?  new ValidationResult(true, null): new ValidationResult(false, "Field cannot be blank.");
        }
    }
}
