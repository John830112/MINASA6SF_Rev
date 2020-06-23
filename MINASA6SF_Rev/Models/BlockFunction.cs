using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINASA6SF_Rev.Models
{
    public class BlockFunction : ViewModelBase
    {
        private int _id;
        private string _name;
        public int Id
        {
            get { return _id;}
            set { SetProperty(ref _id, value);}
        }
        public string Name
        {
            get { return _name;}
            set { SetProperty(ref _name, value);}
        }
    }
}
