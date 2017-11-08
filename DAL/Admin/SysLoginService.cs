using CommonTools;
using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Admin
{
    public class SysLoginService
    {
        public SysAdmins AdminLogin(SysAdmins admin)
        {
            SysAdmins sysObj = new SysAdmins();
            using (SaleManagerDBEntities efdb = new SaleManagerDBEntities())
            {
                //SqlParameter[] para = new SqlParameter[] { new SqlParameter("@LoginId", admin.LoginId), new SqlParameter("@LoginPwd", admin.LoginPwd) };
                //efdb.Database.ExecuteSqlCommand("execute usp_AdminLogin @LoginId  @LoginPwd", para);
                //efdb.Database.SqlQuery<SysAdmins>("execute usp_AdminLogin @LoginId @LoginPwd", para);

                ObjectResult<usp_AdminLogin_Result> result = efdb.usp_AdminLogin(admin.LoginId, admin.LoginPwd);
                sysObj = CommonTool.commonTool.ChangeTypeTToV(result.FirstOrDefault(), sysObj);
                //sysObj = efdb.SysAdmins.SingleOrDefault(s =>( s.LoginId.Equals(admin.LoginId)|| s.AdminName.Equals(admin.AdminName) ) && s.LoginPwd.Equals(admin.LoginPwd));
            }
            return sysObj;
        }

    }
}
