using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class Products
    {
        //扩展
        public int Quantity { get; set; }
        public int Num { get; set; }
        public double SubTotal { get; set; }

        public int TotalCount { get; set; }
        

        //销售总量
        public int SaleCount { get; set; }

        public string CategoryName { get; set; }

        public string SupplierName { get ; set; }
    }
}
