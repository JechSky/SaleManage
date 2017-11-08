using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CallFactory
    {
        public static CallFather CreateCallFather(int type)
        {
            CallFather cal = null;
            switch (type)
            {
                case 0:
                    cal = new CallNormal();
                    break;
                case 1:
                    cal = new CallRate(0.5);
                    break;
                case 2:
                    cal = new CallMN(500, 100);
                    break;

                default:
                    break;
            }
            return cal;
        }

    }
}
