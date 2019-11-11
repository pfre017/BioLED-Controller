using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Controls;

namespace LED_Controller.Common
{
    public static class LEDControllerHelper
    {
        public static Color WavelengthToColor(double Wavelength)
        {
            double R;
            double G;
            double B;
            double SSS;
            if (Wavelength >= 380 & Wavelength < 440)
            {
                R = -(Wavelength - 440) / (440 - 350);
                G = 0.0;
                B = 1.0;
            }
            else if (Wavelength >= 440 & Wavelength < 490)
            {
                R = 0.0;
                G = (Wavelength - 440) / (490 - 440);
                B = 1.0;
            }
            else if (Wavelength >= 490 & Wavelength < 510)
            {
                R = 0.0;
                G = 1.0;
                B = -(Wavelength - 510) / (510 - 490);
            }
            else if (Wavelength >= 510 & Wavelength < 580)
            {
                R = (Wavelength - 510) / (580 - 510);
                G = 1.0;
                B = 0.0;
            }
            else if (Wavelength >= 580 & Wavelength < 645)
            {
                R = 1.0;
                G = -(Wavelength - 645) / (645 - 580);
                B = 0.0;
            }
            else if (Wavelength >= 645 & Wavelength <= 780)
            {
                R = 1.0;
                G = 0.0;
                B = 0.0;
            }
            else
            {
                R = 0.0;
                G = 0.0;
                B = 0.0;
            }

            //# intensity correction
            if (Wavelength >= 380 & Wavelength < 420)
                SSS = 0.3 + 0.7 * (Wavelength - 350) / (420 - 350);
            else if (Wavelength >= 420 & Wavelength <= 700)
                SSS = 1.0;
            else if (Wavelength > 700 & Wavelength <= 780)
                SSS = 0.3 + 0.7 * (780 - Wavelength) / (780 - 700);
            else
            {
                SSS = 0.0;
                SSS *= 255;
            }

            return Color.FromArgb(255, Convert.ToByte(SSS * R * 255), Convert.ToByte(SSS * G * 255), Convert.ToByte(SSS * B * 255));
        }

    }

    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }

    public class NotZeroValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Cannot be null");
            if (int.TryParse(value as string, out int result))
            {
                if (result == 0) return new ValidationResult(false, "Value cannot be zero");
                else return new ValidationResult(true, null);
            }
            else
                return new ValidationResult(false, "Value not valid number");
        }
    }
}
