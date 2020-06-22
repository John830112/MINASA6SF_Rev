using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINASA6SF_Rev.Models
{
    public class BlockParaModel1 : ViewModelBase
    {
        int blockNum;
        public int BlockNum
        {
            get
            {
                return blockNum;
            }
            set
            {
                SetProperty(ref blockNum, value);
            }
        }

        string blockData="";
        public string BlockData
        {
            get
            {
                return blockData;
            }
            set
            {
                SetProperty(ref blockData, value);
            }
        }        
    }
}
