﻿using System;
using System.Windows.Controls;

namespace s3piwrappers.Validation
{
    class DecimalValidationRule :ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            double parameter = 0;

            try
            {
                if (((string)value).Length > 0)
                {
                    parameter = Double.Parse((String)value);
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
