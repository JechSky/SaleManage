using DAL;
using DAL.Admin;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMManager
{
    public partial class FrmLogin : Form
    {
        SysLoginService sysService = new SysLoginService();
        Common common = new Common();
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.txtAccount.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入登录账号！", "登录提示");
                this.txtAccount.Focus();
                return;
            }
            if (this.txtPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入登录密码！", "登录提示");
                this.txtPwd.Focus();
                return;
            }

            SysAdmins sysObj = new SysAdmins();
            if(new Regex(@"^[0-9]\d*$").IsMatch(this.txtAccount.Text.Trim()))
            {
                sysObj.LoginId = Convert.ToInt32(this.txtAccount.Text.Trim());
            }
            else
            {
                sysObj.AdminName = this.txtAccount.Text.Trim();
            }
            sysObj.LoginPwd = this.txtPwd.Text.Trim();

            try
            {
                sysObj = sysService.AdminLogin(sysObj);
                if (sysObj == null)
                {
                    MessageBox.Show("登录账号或密码错误！", "登录失败!");
                }
                else
                {
                    LoginLogs objLogs = new LoginLogs()
                    {
                        LoginId = sysObj.LoginId,
                        SPName = sysObj.AdminName,
                        LoginTime = Common.GetServerTime(),
                        ServerName = Dns.GetHostName()
                    };
                    sysObj.LoginLogId = common.WriteLoginLog(objLogs);
                    Common.objSys = sysObj;

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "登录失败!");
                throw;
            }


        }
    }
}
