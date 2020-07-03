using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINASA6SF_Rev.Models
{
    public class ByteArray
    {
        List<byte> list = new List<byte>();

        public byte[] array
        {
            get { return list.ToArray(); }
        }

        public ByteArray()
        {
            list = new List<byte>();
        }

        public ByteArray(int size)
        {
            list = new List<byte>(size);
        }

        public void Clear()
        {
            list = new List<byte>();
        }

        public void Add(byte item)
        {
            list.Add(item);
        }

        public void Add(byte[] items)
        {
            list.AddRange(items);
        }
    }
}
