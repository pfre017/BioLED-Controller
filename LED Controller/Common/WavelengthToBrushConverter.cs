using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace LED_Controller.Common
{
    public class WavelengthToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Brushes.Transparent;

            if (double.TryParse((string)value, out double wavelength))
            {
                return new SolidColorBrush(LEDControllerHelper.WavelengthToColor(wavelength));
            }
            return Brushes.Transparent;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
            throw new NotImplementedException();
        }
    }
}
