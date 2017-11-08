using DAL;
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

namespace SaleManage
{
    public partial class FrmLogin : Form
    {
        UserLoginService userLoginService = new UserLoginService(); 
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
            if (!new Regex(@"^[1-9]\d*$").IsMatch(this.txtAccount.Text.Trim()))
            {
                MessageBox.Show("登录账号必须是正整数！", "登录提示");
                this.txtAccount.Focus();
                return;
            }

            SalesPerson objSalesPerson = new SalesPerson()
            {
                SalesPersonId = Convert.ToInt32(this.txtAccount.Text.Trim()),
                LoginPwd = this.txtPwd.Text.Trim()
            };

            try
            {
               objSalesPerson= userLoginService.UserLogin(objSalesPerson);
                if (objSalesPerson == null)
                {
                    MessageBox.Show("登录账号或密码错误！", "登录失败!");
                }
                else
                {
                    LoginLogs objLogs = new LoginLogs()
                    {
                        LoginId = objSalesPerson.SalesPersonId,
                        SPName = objSalesPerson.SPName,
                        LoginTime=Common.GetServerTime(),
                        ServerName = Dns.GetHostName()
                    };
                    objSalesPerson.LoginLogId = Common.common.WriteLoginLog(objLogs);
                    Common.objPerson = objSalesPerson;

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
