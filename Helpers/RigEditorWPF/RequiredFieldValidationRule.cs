using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace s3piwrappers.RigEditor
{
    class RequiredFieldValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            return !String.IsNullOrEmpty(value.ToString())?  new ValidationResult(true, null): new ValidationResult(false, "Field cannot be blank.");
        }
    }
}
