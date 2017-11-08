using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Common
    {

        public static SalesPerson objPerson = null;
        public static DateTime? GetServerTime()
        {
            DateTime? obj = null;
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                obj = efdb.Database.SqlQuery<DateTime>("select getdate()").FirstOrDefault();
            }
            return obj;
        }

    }
}
