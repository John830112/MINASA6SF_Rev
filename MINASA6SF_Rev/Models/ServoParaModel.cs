using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINASA6SF_Rev.Models
{
    public class ServoParaModel : ViewModelBase
    {
        private string mainIndex;
        public string MainIndex
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

        private int subIndex;
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

        private string paraName;
        public string ParaName
        {
            get
            {
                return paraName;
            }
            set
            {
                SetProperty(ref paraName, value);
            }
        }

        private string Range;
        public string range
        {
            get
            {
                return Range;
            }
            set
            {
                SetProperty(ref Range, value);
            }
        }

        private string Unitval;
        public string unitVal
        {
            get
            {
                return Unitval;
            }
            set
            {
                SetProperty(ref Unitval, value);
            }
        }


        private double setval;
        public double SetVal
        {
            get
            {
                return setval;
            }
            set
            {
                SetProperty(ref setval, value);
            }
        }
    }
}
