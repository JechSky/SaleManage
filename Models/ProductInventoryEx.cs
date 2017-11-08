using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class ProductInventory
    {
        public string ProductName { get; set; }

        string categoryId = "1";
        public string CategoryId { get { return categoryId; } set { categoryId = value; } }
        public string CategoryName { get; set; }
        

    }
}
