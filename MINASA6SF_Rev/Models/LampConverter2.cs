using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MINASA6SF_Rev.Models
{
    [ValueConversion(typeof(int), typeof(String))]
    class LampConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool lampValue = (bool)value;
            if (lampValue)
            {
                return @"..\RadioLamp\LED-green.png";
            }
            else
            {
                return @"..\RadioLamp\LED-gray.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
