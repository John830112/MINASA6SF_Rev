using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MINASA6SF_Rev.Models
{
    public class IndexToXamlStringConverter : IValueConverter
    {
        string blocksettingDialogs;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((int)value)
            {
                case 0:
                    blocksettingDialogs = "IncPosition_Page1.xaml";
                    return blocksettingDialogs;
                case 1:
                    blocksettingDialogs = "Abs_Position_Page2.xaml";
                    return blocksettingDialogs;
                case 2:
                    blocksettingDialogs = "JOG_Operation_Page3.xaml";
                    return blocksettingDialogs;
                case 3:
                    blocksettingDialogs = "HomeReturn_Page4.xaml";
                    return blocksettingDialogs;
                case 4:
                    blocksettingDialogs = "DecStop_Page5.xaml";
                    return blocksettingDialogs;
                case 5:
                    blocksettingDialogs = "SpeedUpdate_Page6.xaml";
                    return blocksettingDialogs;
                case 6:
                    blocksettingDialogs = "DecrementCount_Page7.xaml";
                    return blocksettingDialogs;
                case 7:
                    blocksettingDialogs = "OutPutSignal_Page8.xaml";
                    return blocksettingDialogs;
                case 8:
                    blocksettingDialogs = "Jump_Page9.xaml";
                    return blocksettingDialogs;
                case 9:
                    blocksettingDialogs = "ConditionDiv_Page10.xaml";
                    return blocksettingDialogs;
                case 10:
                    blocksettingDialogs = "ConditionDiv_Page11.xaml";
                    return blocksettingDialogs;
                case 11:
                    blocksettingDialogs = "ConditionDiv_Page12.xaml";
                    return blocksettingDialogs;
            }
            return blocksettingDialogs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
