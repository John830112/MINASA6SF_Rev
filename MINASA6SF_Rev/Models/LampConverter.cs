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
    class LampConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int lampValue = (int)value;
            switch (lampValue)
            {
                case 1:
                    return @"..\RadioLamp\LED-green.png";
                    break;
                default:
                    return @"..\RadioLamp\LED-gray.png";
                    break;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
