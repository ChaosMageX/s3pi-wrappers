using System;
using System.Windows.Controls;

namespace FootprintViewer.Validation
{
    class HexValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            uint parameter = 0;

            try
            {
                parameter = Convert.ToUInt32('0' + value.ToString().TrimStart('0','x','X'),16);


            }
            catch (Exception)
            {
                return new ValidationResult(false, "*");
            }


            return new ValidationResult(true, null);

        }
    }
}