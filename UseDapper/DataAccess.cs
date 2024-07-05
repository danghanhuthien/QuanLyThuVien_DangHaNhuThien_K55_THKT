using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseDapper
{
    public class DataAccess
    {
        private string connecStr;

        public DataAccess(string connecStr)
        {
            this.connecStr = connecStr;
        }
    }
}
