using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CallMN : CallFather
    {
        public double M { get; set; }
        double N { get; set; }

        public CallMN(double m,double n)
        {
            this.M = m;
            this.N = n;
        }

        public override double GetTotalMoney(double realMoney)
        {
            if(realMoney>=M)
            {
               return  realMoney - Math.Floor(realMoney / M) * N;
            }
            else
            {
                return realMoney;
            }
        }
    }
}
