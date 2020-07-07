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
    class LampConverter1 : IValueConverter
    {      
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int lampValue = (int)value;
            if (lampValue == 1)
                return @"..\RadioLamp\LED-red.png";
            else
                return @"..\RadioLamp\LED-green.png";
        }
     
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
