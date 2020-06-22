using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINASA6SF_Rev.Models
{
    public class BlockParaModel2 : ViewModelBase
    {
        int mainIndex;
        public int MainIndex
        {
            get
            {
                return mainIndex;
            }
            set
            {
                SetProperty(ref mainIndex, value);
            }
        }

        int subIndex;
        public int SubIndex
        {
            get
            {
                return subIndex;
            }
            set
            {
                SetProperty(ref subIndex, value);
            }
        }

        string parameterName;
        public string ParameterName
        {
            get
            {
                return parameterName;
            }
            set
            {
                SetProperty(ref parameterName, value);
            }
        }

        string range;
        public string Range
        {
            get
            {
                return range;
            }
            set
            {
                SetProperty(ref range, value);
            }
        }

        Int32 settingValue;
        public Int32 SettingValue
        {
            get
            {
                return settingValue;
            }
            set
            {
                SetProperty(ref settingValue, value);
            }
        }

        string unit;
        public string Unit
        {
            get
            {
                return unit;
            }
            set
            {
                SetProperty(ref unit, value);
            }
        }
    }
}
