using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ListDataView
    {
        int pageIndex = 1;
        public int PageIndex { get { return pageIndex; } set { pageIndex = value; } }
        int pageCount = 1;
        public int PageCount { get { return pageCount; }  set { pageCount = value; }  }
        int totalCount = 1;
        public int TotalCount { get { return totalCount; } set { totalCount = value; } }
        public object ListData { get; set; }

        public object Remark { get; set; }

    }
}
