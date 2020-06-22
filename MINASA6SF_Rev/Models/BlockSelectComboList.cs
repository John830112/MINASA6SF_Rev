using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINASA6SF_Rev.Models
{
    public class BlockSelectComboList
    {
        int[] combolist;

        public BlockSelectComboList()
        {
             for(int i=0; i<255; i++)
            {
                combolist[i] = i;
            }
        }

        public int[] Combolist { get => combolist; set => combolist = value; }
    }
}
