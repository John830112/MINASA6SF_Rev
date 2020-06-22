using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINASA6SF_Rev.Models
{
    public class BlockParaModel1 
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
                if (blockNum == value)
                {
                    return;
                }
                blockNum = value;
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
                if (blockData.Equals(value))
                {
                    return;
                }
                blockData = value;
            }
        }        
    }
}
