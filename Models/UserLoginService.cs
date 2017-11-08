using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class UserLoginService
    {
        /// <summary>
        /// 根据用户名和密码登录
        /// </summary>
        /// <param name="objSalesPerson"></param>
        /// <returns></returns>
        public SalesPerson UserLogin(SalesPerson objSalesPerson)
        {
            SalesPerson saleObj = new SalesPerson();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                saleObj = efdb.SalesPerson.SingleOrDefault(s => s.SalesPersonId.Equals(objSalesPerson.SalesPersonId) && s.LoginPwd.Equals(objSalesPerson.LoginPwd));
            }
            return saleObj;
        }
        public int WriteLoginLog(LoginLogs log)
        {
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                efdb.Entry<LoginLogs>(log).State= System.Data.Entity.EntityState.Added;
                return efdb.SaveChanges();
            }
            return 0;
        }
        public int WriteExitLog(int logId)
        {
            LoginLogs loginLog = new LoginLogs();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                loginLog= efdb.LoginLogs.SingleOrDefault(s => s.LoginId.Equals(logId));
                loginLog.ExitTime = Common.GetServerTime();//DateTime.Now;
                efdb.Entry<LoginLogs>(loginLog).State = System.Data.Entity.EntityState.Modified;
                return efdb.SaveChanges();
            }
        }

  


    }
}
