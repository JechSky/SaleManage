using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MyEventArgs: EventArgs
    {
        public bool bl = false;
        public MyEventArgs(bool bl)
        {
            this.bl = bl;
        }
    }

}
