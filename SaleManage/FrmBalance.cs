using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaleManage
{
    public partial class FrmBalance : Form
    {
        public FrmBalance(string totalMoney)
        {
            InitializeComponent();
            this.lblTotalMoney.Text = totalMoney;
            this.txtRealReceive.Text = totalMoney;
            this.txtRealReceive.Focus();
        }

        private void txtMemberId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.txtMemberId.Text.Trim().Length == 0)
                {
                    this.Tag = this.txtRealReceive.Text.Trim();//将实收款存入窗体标签
                }
                else//将实收款和会员卡号返回
                {
                    //判断该会员卡是否存在
                    //封装实收款和会员卡号
                    this.Tag = this.txtRealReceive.Text.Trim() + "|" + this.txtMemberId.Text.Trim();
                }
                this.DialogResult = DialogResult.OK;
               // FrmSaleManage.clarFrm.Invoke();
                this.Close();
            }
            else if (e.KeyValue == 115)//F4键：代表用户放弃所有商品购买
            {
                this.Tag = "F4";
                FrmSaleManage.clarFrm.Invoke();
                this.Close();
            }
            else if (e.KeyValue == 116)//F5键：考虑用户减去商品
            {
                this.Tag = "F5";
                FrmSaleManage.clarFrm.Invoke();
                this.Close();
            }
        }

        //void GoPay()
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("");

        //}


    }
}
