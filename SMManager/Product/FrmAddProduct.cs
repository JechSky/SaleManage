using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMManager.Product
{
    public partial class FrmAddProduct : Form
    {
       
        ProductService proService = new ProductService();
        
        List<ListItem> listProductUnitItem = new List<ListItem>();

        public FrmAddProduct()
        {
            InitializeComponent();
        }

        private void FrmAddProduct_Load(object sender, EventArgs e)
        {
            txtInUnitPrice.Leave += CheckPrice;
            txtOutUnitPrice.Leave += CheckPrice;

            cboCategory.Items.AddRange(Common.listCategoryItem.ToArray());
            cboCategory.SelectedIndex = 0;

        }

        List<Control> listControls = new List<Control>();
        private void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (Control item in this.Controls)
            {
                AddControl(item);
            }
            listControls.Sort((con1, con2) => con1.TabIndex.CompareTo(con2.TabIndex));
            //listControls.Sort(Comparison);
            foreach (Control item in listControls)
            {
                if (item is TextBox)
                {
                    string str = (item as TextBox).Text;
                    if (string.IsNullOrEmpty(str) || new Regex("^\\s$").IsMatch(str))
                    {
                        MessageBox.Show(item.Tag + "不能为空");
                        item.Focus();
                        return;
                    }

                    if(item.Name.Equals("txtProductId")|| item.Name.Equals("txtCount"))
                    {
                        if (!new Regex(@"^[0-9]\d*$").IsMatch(str.Trim()))
                        {
                            MessageBox.Show(item.Tag + "必须是正整数！");
                            item.Focus();
                            return;
                        }
                    }

                }
            }

            //if (!new Regex(@"^[0-9]\d*$").IsMatch(this.txtProductId.Text.Trim()))
            //{
            //    MessageBox.Show("商品编码必须是正整数！");
            //    this.txtProductId.Focus();
            //    return;
            //}
            //if (!new Regex(@"^[0-9]\d*$").IsMatch(this.txtCount.Text.Trim()))
            //{
            //    MessageBox.Show("数量必须是正整数！");
            //    this.txtCount.Focus();
            //    return;
            //}


            Products pro = new Products()
            {
                SupplierName=txtSupplier.Text.Trim(),
                ProductId = txtProductId.Text.Trim(),
                CategoryId = Convert.ToInt32((cboCategory.SelectedItem as ListItem).ID),
                ProductName = txtProductName.Text.Trim(),
                InUnitPrice = Convert.ToDecimal(txtInUnitPrice.Text.Trim()),
                UnitPrice = Convert.ToDecimal(txtOutUnitPrice.Text.Trim()),
                TotalCount = int.Parse(txtCount.Text.Trim()),
                AddTime=Common.GetServerTime()
            };

            int result = proService.AddProducts(pro);
            if (result > 0)
            {
                MessageBox.Show("添加成功！");
                foreach (Control item in this.Controls)
                {
                    ClearControls(item);
                    txtSupplier.Focus();
                }
            }

        }

        void AddControl(Control control)
        {
            if (control == null) return;
            listControls.Add(control);

            foreach (Control item in control.Controls)
            {
                AddControl(item);
            }
        }

        void ClearControls(Control control)
        {
            if (control == null) return;
            if (control is TextBox)
            {
                control.Text = string.Empty;
                control.Focus();
            }
            foreach (Control item in control.Controls)
            {
                ClearControls(item);
            }
        }

        public int Comparison(Control con1,Control con2)
        {
            if (con1.TabIndex > con2.TabIndex)
            {
                return 1;
            }
            else if (con1.TabIndex < con2.TabIndex)
            {
                return -1;
            }
            else
                return 0;

        }

        void CheckPrice(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtInUnitPrice.Text) && !string.IsNullOrEmpty(txtOutUnitPrice.Text))
            {
                if (Convert.ToDecimal(txtOutUnitPrice.Text) < Convert.ToDecimal(txtInUnitPrice.Text))
                {
                    MessageBox.Show("售价小于进价，请确认后再输入！");
                    return;
                }
            }
      
        }

        bool CheckNum(TextBox txt)
        {
            if (!new Regex(@"^[0-9]\d*$").IsMatch(txt.Text.Trim()))
            {
                MessageBox.Show(txt.Tag+"必须是正整数！");
                txt.Focus();
                return false;
            }
            return true;
        }


    }
}
