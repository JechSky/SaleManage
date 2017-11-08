using CommonTools;
using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaleManage
{
    public delegate void DelClearFrm();
    public partial class FrmSaleManage : Form
    {
        public static DelClearFrm clarFrm;

        #region 变量

        private SaleListService objSaleListService = new SaleListService();
        private UserLoginService objUserLoginService = new UserLoginService();
        private ProductService objProductService = new ProductService();
        private List<Products> productList = new List<Products>();

        ListItem[] listDisItem = new ListItem[]
        {
            new ListItem(DisCountEnum.CallNormal.ToString(),CommonTool.commonTool.GetEnumDiscription(DisCountEnum.CallNormal)),
            new ListItem(DisCountEnum.CallRate.ToString(),CommonTool.commonTool.GetEnumDiscription(DisCountEnum.CallRate)),
            new ListItem(DisCountEnum.CallMN.ToString(),CommonTool.commonTool.GetEnumDiscription(DisCountEnum.CallMN))
        };

        #endregion

        public FrmSaleManage()
        {
            InitializeComponent();

            lblSalePerson.Text = Common.objPerson.SPName;
            lblSerialNum.Text = CreateSerialNo();
            //clarFrm = new DelClearFrm(GiveUpRest);

            foreach (Control control in this.Controls)
            {
                if(control is TextBox||control is ComboBox)
                control.KeyDown += EnterAndOpera;
            }

            this.cbDiscount.Items.AddRange(listDisItem);
            this.cbDiscount.SelectedIndex = 0;
        }
         

        #region 生成流水帐号
        /// <summary>
        /// 生成流水帐号
        /// </summary>
        /// <returns></returns>
        private string CreateSerialNo()
        {
            string sNo = Common.GetServerTime().ToString("yyyyMMddHHmmss");
            Random rd = new Random();
            sNo += rd.Next(10, 99).ToString();
            return sNo;
        }
        #endregion

        #region 窗体关闭前执行
        /// <summary>
        /// 窗体关闭前执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSaleManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确认退出吗？", "退出询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                Common.common.WriteExitLog(Common.objPerson.LoginLogId);
            }
        }

        #endregion

        #region 删除不需要的商品更新序号
        private void dgvProductList_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //更新商品的总金额
            //this.lblTotalMoney.Text = this.productList.Sum(p => p.SubTotal).ToString();
            GetResultMoney();
            //更新序号
            for (int i = 0; i < this.productList.Count; i++)
            {
                this.productList[i].Num = i + 1;
            }
        }
        #endregion
        

        #region 在列表中添加或更新商品
        private void AddOrUpdateList()
        {
            //数据验证
            if (!ValidateInput())
            {
                return;
            }
            if (!IsExisted())
            {
                //如果商品不存在，则添加新商品
                AddNewProduct();
            }
            //更新商品总金额 应付
            GetResultMoney();

            //显示当前商品列表
            this.dgvProductList.AutoGenerateColumns = false;
            this.dgvProductList.DataSource = null;
            this.dgvProductList.DataSource = productList;

            for (int i = 0; i < dgvProductList.Rows.Count; i++)
            {
                if (dgvProductList.Rows[i].Cells["ProductId"].Value.Equals(txtProductId.Text.Trim()))
                {
                    dgvProductList.Rows[i].Selected = true;
                    this.txtQuantity.Text = dgvProductList.Rows[i].Cells["Quantity"].Value.ToString();
                }
                else
                {
                    dgvProductList.Rows[i].Selected = false;
                }
            }

            //清空商品的相关信息
            this.txtProductId.Clear();
            this.txtProductId.Focus();

        }
        #endregion

        #region 验证输入框
        /// <summary>
        /// 验证输入框
        /// </summary>
        /// <returns></returns>
        private bool ValidateInput()
        {
            if (this.txtProductId.Text.Trim().Length == 0)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 判断当前列表中是否已存在该商品
        private bool IsExisted()
        {
            var pList = productList.Where(p => p.ProductId.Equals(this.txtProductId.Text.Trim()));
            if (pList.Count() == 0) return false;
            else
            {
                Products objProduct = pList.First<Products>();
                objProduct.Quantity += Convert.ToInt32(this.txtQuantity.Text.Trim());
                objProduct.SubTotal = Convert.ToDouble(objProduct.Quantity * objProduct.UnitPrice);
            }
            return true;
        }
        #endregion

        #region 如果当前列表中不存在，则添加新商品
        private void AddNewProduct()
        {
            Products objProduct = objProductService.GetProductById(this.txtProductId.Text.Trim());
            if (objProduct == null)
            {
                MessageBox.Show("没有此产品，请输入正确的产品id！");
                return;
            }
            else//如果数据库中存在该商品信息，则显示单价和折扣
            {
                this.txtDiscount.Text = objProduct.Discount.ToString();
            }
            //封装商品数量
            if(!string.IsNullOrEmpty(this.txtQuantity.Text))
            {
                if (new Regex(@"^[1-9]\d*$").IsMatch(txtQuantity.Text.Trim()))
                {
                    objProduct.Quantity = Convert.ToInt32(this.txtQuantity.Text.Trim());
                }
                else
                {
                    MessageBox.Show("数量必须大于0！");
                    return;
                }
            }
            else
            {
                MessageBox.Show("数量不能为空！");
                return;
            }

            //根据商品数量计算该商品的小计金额
            objProduct.SubTotal = Convert.ToDouble(objProduct.Quantity) * Convert.ToDouble(objProduct.UnitPrice);
            if (objProduct.Discount != 0)
            {
                objProduct.SubTotal *= (Convert.ToDouble(objProduct.Discount) / 10.0);
            }
            objProduct.SubTotal = Math.Round(objProduct.SubTotal, 2);//保留两位小数
            objProduct.Num = this.productList.Count + 1;//添加商品序号
            //商品添加到集合
            productList.Add(objProduct);

            CommonFunc();

        }
        #endregion

        #region 结算方法
        private void Balance()
        {
            string total = this.lblTotalMoney.Text.Trim();
            //显示结算窗口
            FrmBalance objBalance = new FrmBalance(this.lblTotalMoney.Text.Trim());
            DialogResult result = objBalance.ShowDialog();
            if (result != DialogResult.OK)//商品结算被取消
            {
                if(objBalance.Tag!=null)
                {
                    if (objBalance.Tag.ToString() == "F4")//用户放弃商品购买
                    {
                        GiveUpRest();
                        return;
                    }
                    else if (objBalance.Tag.ToString() == "F5")//用户放弃部分商品购买
                    {
                        this.txtProductId.Focus();
                        return;
                    }
                }
                GiveUpRest();
                return;
            }
            //显示结算窗口的返回值（实收款）
            string memberId = string.Empty;
            if (objBalance.Tag.ToString().Contains("|"))
            {
                string[] info = objBalance.Tag.ToString().Split('|');
                this.lblReceivedMoney.Text = info[0];
                memberId = info[1];
            }
            else
            {
                this.lblReceivedMoney.Text = objBalance.Tag.ToString();
            }
            //显示商品找零
            this.lblReturnMoney.Text = (Convert.ToDouble(this.lblReceivedMoney.Text.Trim()) - Convert.ToDouble(this.lblTotalMoney.Text.ToString())).ToString();
            SalesList objSalesList = new SalesList()
            {
                SerialNum = this.lblSerialNum.Text.Trim(),
                TotalMoney = Convert.ToDecimal(this.lblTotalMoney.Text.Trim()),
                RealReceive = Convert.ToDecimal(this.lblReceivedMoney.Text.Trim()),
                ReturnMoney = Convert.ToDecimal(this.lblReturnMoney.Text.Trim()),
                SalesPersonId = Common.objPerson.SalesPersonId,
                SaleDate = Common.GetServerTime(),
                SaleDisCount = Convert.ToInt32(Enum.Parse(typeof(DisCountEnum),(this.cbDiscount.SelectedItem as ListItem).ID))
            };
            List<SalesListDetail> detailList = new List<SalesListDetail>();
            foreach (Products item in productList)
            {
                detailList.Add(new SalesListDetail()
                {
                    SerialNum = this.lblSerialNum.Text.Trim(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                    //,
                    //SubTotalMoney = (decimal)item.SubTotal
                });
            }
            int points = 0;
            try
            {
                objSaleListService.SaveSaleInfo(objSalesList, detailList);//保存
                //如果顾客有会员卡，则将销售金额转换成积分，存入会员卡
                if (!string.IsNullOrEmpty(memberId))
                {
                    points = (int)(Convert.ToDouble(this.lblTotalMoney.Text) / 10.0);
                    objSaleListService.AddPoints(memberId, points);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存销售信息发生错误：" + ex.Message, "错误提示");
                return;
            }

            //MessageBox.Show("请确认打印！","打印小票");

            ////打印小票（串行端口）
            //SerialPort objPort = new SerialPort();
            //objPort.PortName = "COM1";
            //objPort.Open();
            ////输出内容
            //objPort.WriteLine("");
            //objPort.WriteLine("");
            //objPort.WriteLine("");
            //objPort.WriteLine("");
            //objPort.Close();
            //MessageBox.Show("小票打印成功！");

            //打印小票（商品明细、积分总数、本次积分）弹出钱箱
            //this.printDocument1.Print();

            RestForm();
            this.lblTotalMoney.Text = total;

        }
        #endregion

        #region 重新生成流水号，重置
        //重新生成流水号，重置
        private void RestForm()
        {
            this.lblSerialNum.Text = CreateSerialNo();
            this.dgvProductList.DataSource = null;
            this.productList.Clear();
            this.txtProductId.Focus();
            txtQuantity.Text = "1";
            txtDiscount.Text = "0";

            //cbDiscount.SelectedIndex = 0; 
        }
        private void GiveUpRest()
        {
            RestForm();
            lblTotalMoney.Text = "0.00";
            lblReceivedMoney.Text = "0.00";
            lblReturnMoney.Text = "0.00";
        }


        #endregion

        /// <summary>
        /// dgv KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvProductList_KeyDown(object sender, KeyEventArgs e)
        {
            Opera(e);
        }


        void Opera(KeyEventArgs e)
        {
            if (e.KeyValue == 46)//删除当前行
            {
                if (this.dgvProductList.RowCount == 0) return;
                Products pro = productList.Where(p => p.ProductId.Equals(this.dgvProductList.SelectedRows[0].Cells["ProductId"].Value.ToString())).FirstOrDefault();
                productList.Remove(pro);
                this.dgvProductList.DataSource = null;
                this.dgvProductList.DataSource = productList;

                SetCurTxtValue();

            }
            else if (e.KeyValue == 112)//F1键，则进入商品结算
            {
                if (this.dgvProductList.RowCount == 0) return;
                Balance();
            }
            else if (e.KeyValue == 38)//向上移动键
            {
                if (this.dgvProductList.RowCount == 0 || this.dgvProductList.RowCount == 1) return;
            }
            else if (e.KeyValue == 40)//向下移动键
            {
                if (this.dgvProductList.RowCount == 0 || this.dgvProductList.RowCount == 1) return;
            }
        }

        void OperaEnterAddPro(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)//如果是回车键
            {
                if ((sender as TextBox).Name.Equals("txtProductId"))
                {
                    if (!string.IsNullOrEmpty(this.txtProductId.Text) && !new Regex(@"^\s*$", RegexOptions.IgnoreCase).IsMatch(txtProductId.Text))
                    {
                        AddOrUpdateList();
                    }
                    else
                    {
                        MessageBox.Show("商品编号不能为空");
                        return;
                    }
                }
                else
                {
                    AddOrUpdateList();
                }

            }
        }

        void EnterAndOpera(object sender, KeyEventArgs e)
        {
            OperaEnterAddPro(sender,e);
            Opera(e);
        }

        void SetCurTxtValue()
        {
            if (this.dgvProductList.Rows.Count>0)
            {
                this.txtQuantity.Text = this.dgvProductList.SelectedRows[0].Cells["Quantity"].Value.ToString();
                this.txtDiscount.Text = (Convert.ToDouble( this.dgvProductList.SelectedRows[0].Cells["Discount"].Value.ToString())/10.0).ToString();
            }
        }

        private void dgvProductList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SetCurTxtValue();
            }
        }

        private void cbDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetResultMoney();
        }

        CallFather GetCal(Enum type)
        {
            CallFather cal = null;

            #region 简单工厂

            //cal = CallFactory.CreateCallFather((int)(DisCountEnum)type);
            //switch ((int)(DisCountEnum)type)
            //{
            //    case 0:
            //        cal = new CallNormal();
            //        break;
            //    case 1:
            //        cal = new CallRate(0.5);
            //        break;
            //    case 2:
            //        cal = new CallMN(500, 100);
            //        break;
            //    default:
            //        break;
            //}

            #endregion


            #region 反射创建对象

            string aseNM = "Models";
            Assembly aseObj = Assembly.Load(aseNM);
            string classNM = aseNM + "." + (DisCountEnum)type;

            object[] args = null;
            switch ((int)(DisCountEnum)type)
            {
                case 0:
                    args = null;
                    break;
                case 1:
                    args = new object[1];
                    args[0] = 0.5;
                    break;
                case 2:
                    args = new object[2];
                    args[0] = 300;
                    args[1] = 100;
                    break;
                default:
                    break;
            }

            cal = (CallFather)aseObj.CreateInstance(classNM, true, BindingFlags.Default, null, args, null, null);

            #endregion

            return cal;
        }
        
        private void txtQuantity_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtProductId.Text) && !new Regex(@"^\s*$", RegexOptions.IgnoreCase).IsMatch(txtProductId.Text))
            {
                if (IsExisted())
                {
                    CommonFunc();
                }
            }
            else
            {
                CommonFunc();
            }
            
        }


        void CommonFunc()
        {
            if (dgvProductList.Rows.Count > 0 && dgvProductList.SelectedRows.Count > 0)
            {
                if (!string.IsNullOrEmpty(this.txtQuantity.Text))
                {
                    if (new Regex(@"^[1-9]\d*$").IsMatch(txtQuantity.Text.Trim()))
                    {
                        int quantity = Convert.ToInt32(this.txtQuantity.Text.Trim());
                        string proId = dgvProductList.SelectedRows[0].Cells["ProductId"].Value.ToString();
                        int dgvQuantity = Convert.ToInt32(dgvProductList.SelectedRows[0].Cells["Quantity"].Value);
                        if (quantity != dgvQuantity)
                        {
                            //修改productlist
                            Products pros = this.productList.Where(p => p.ProductId.Equals(proId)).FirstOrDefault();
                            pros.Quantity = quantity;
                            pros.SubTotal = pros.Quantity * Convert.ToDouble(pros.UnitPrice);

                            GetResultMoney();

                            //修改dgv
                            dgvProductList.SelectedRows[0].Cells["Quantity"].Value = quantity.ToString();
                            dgvProductList.SelectedRows[0].Cells["SubTotal"].Value = pros.SubTotal.ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("数量必须大于0！");
                    }
                }
            }
        }
        void GetResultMoney()
        {
            if (dgvProductList.Rows.Count > 0)
            {
                CallFather cal = GetCal((DisCountEnum)Enum.Parse(typeof(DisCountEnum), (this.cbDiscount.SelectedItem as ListItem).ID));
                double totalMoney = productList.Sum(p => p.SubTotal);
                this.lblTotalMoney.Text = cal.GetTotalMoney(Convert.ToDouble(totalMoney)).ToString();
            }
        }

    }
}
