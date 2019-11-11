using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LED_Controller.Common
{
    public class LedRangeRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            
            if (Int32.TryParse(new String(((string)value).Where(Char.IsDigit).ToArray()), out int intensity) == false)
                return new ValidationResult(false, "Value could not be parsed to an integer");

            if (intensity < 0)
                return new ValidationResult(false, "Value out of range");
            if (intensity > 100)
                return new ValidationResult(false, "Value out of range");

            return ValidationResult.ValidResult;

        }
    }
}
