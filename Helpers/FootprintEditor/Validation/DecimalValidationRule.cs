﻿using System;
using System.Globalization;
using System.Windows.Controls;

namespace s3piwrappers.Validation
{
    internal class DecimalValidationRule : ValidationRule
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public DecimalValidationRule()
        {
            Min = double.MinValue;
            Max = double.MaxValue;
        }

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

            if (parameter < Min || parameter > Max)
            {
                return new ValidationResult(false, String.Format("Value must be in range of {0},{1}", Min, Max));
            }

            return new ValidationResult(true, null);
        }
    }
}
