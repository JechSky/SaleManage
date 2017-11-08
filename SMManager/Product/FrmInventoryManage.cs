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
    public partial class FrmInventoryManage : Form
    {
        ProductInventoryService proInventoryService = new ProductInventoryService();
        ListDataView view = new ListDataView();
        string delProId = string.Empty;

        public FrmInventoryManage()
        {
            InitializeComponent();
        }

        private void FrmInventoryManage_Load(object sender, EventArgs e)
        {
            cbCategory.Items.AddRange(Common.listCategoryItem.ToArray());
            cbCategory.SelectedIndex = -1;
            DataBind("", "", null, 1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string proId = txtProductId.Text;
            string proName = txtProductName.Text.Trim();
            int? categoryId = null;
            if (cbCategory.SelectedItem != null)
            {
                categoryId = Convert.ToInt32((cbCategory.SelectedItem as ListItem).ID);
            }

            MyEventArgs arg = (e as MyEventArgs);
            if (arg == null)
            {
                view.PageIndex = 1;
                lblCurNum.Text = "1";
            }
            DataBind(proId, proName, categoryId, int.Parse(lblCurNum.Text.Trim()));
        }
        void DataBind(string proId, string proName, int? categoryId, int curNum)
        {
            view = proInventoryService.GetListBySearch(proId, proName, categoryId, int.Parse(lblCurNum.Text.Trim()));
            List<Products> list = (List<Products>)view.ListData;

            this.lblTotalNum.Text = view.PageCount.ToString();


            //设置不自动显示数据库中未绑定的列
            dgvProduct.AutoGenerateColumns = false;
            dgvProduct.DataSource = list;
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            if (view.PageIndex <= view.PageCount && view.PageIndex > 1)
            {
                view.PageIndex--;
            }
            else
            {
                view.PageIndex = 1;
            }
            this.lblCurNum.Text = view.PageIndex.ToString();
            MyEventArgs arg = new MyEventArgs(false);
            btnSearch_Click(null, arg);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (view.PageIndex < view.PageCount && view.PageIndex >= 1)
            {
                view.PageIndex++;
            }
            else
            {
                view.PageIndex = view.PageCount;
            }
            this.lblCurNum.Text = view.PageIndex.ToString();
            MyEventArgs arg = new MyEventArgs(false);
            btnSearch_Click(null, arg);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (!new Regex(@"^[0-9]\d*$").IsMatch(txtGoPageNum.Text.Trim()))
            {
                MessageBox.Show("请输入正确的页数！");
                return;
            }
            else
            {
                int goPageNum = Convert.ToInt32(txtGoPageNum.Text.Trim());
                if (goPageNum >= 1 && goPageNum <= Convert.ToInt32(lblTotalNum.Text.Trim()))
                {
                    view.PageIndex = Convert.ToInt32(txtGoPageNum.Text.Trim());
                    this.lblCurNum.Text = view.PageIndex.ToString();
                }
                else
                {
                    MessageBox.Show("没有此页！");
                    return;
                }

            }

            MyEventArgs arg = new MyEventArgs(false);
            btnSearch_Click(null, arg);
        }

        private void dgvProduct_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Point pt = dgvProduct.PointToClient(Control.MousePosition);
            DataGridView.HitTestInfo info = dgvProduct.HitTest(pt.X, pt.Y);

            if (info.Type != DataGridViewHitTestType.ColumnHeader)
            {
                foreach (DataGridViewColumn item in dgvProduct.Columns)
                {
                   
                    if (item.Name.Equals("TotalCount"))
                    {
                        item.ReadOnly = false;
                    }
                    else
                    {
                        item.ReadOnly = true;
                    }
                }

            }
        }

        private void dgvProduct_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvr = dgvProduct.Rows[e.RowIndex];
            DataGridViewColumn dgvc = dgvProduct.Columns[e.ColumnIndex];
            string value = dgvr.Cells[e.ColumnIndex].Value.ToString();
            string proId = string.Empty;

            if (dgvr.Cells["ProductId"].Value != null)
            {
                proId = dgvr.Cells["ProductId"].Value.ToString();
            }
            int result = 0;
            result = proInventoryService.UpdateEntity(proId, value);
            if (result>0)
            {
                MessageBox.Show("编号为："+ proId + "的库存更新成功！");
            }

        }

        private void dgvProduct_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = dgvProduct.PointToClient(Control.MousePosition);
            DataGridView.HitTestInfo info = dgvProduct.HitTest(pt.X, pt.Y);
            if (info.Type != DataGridViewHitTestType.ColumnHeader)
            {
                if (e.Button == MouseButtons.Right)
                {
                    foreach (DataGridViewRow item in dgvProduct.Rows)
                    {
                        if (item.Index == info.RowIndex)
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            item.Selected = false;
                        }
                    }
                    delProId = dgvProduct.Rows[info.RowIndex].Cells["ProductId"].Value.ToString();
                }
            }

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int result = 0;
            DialogResult dr = MessageBox.Show(string.Format("确定删除编号为{0}的商品吗？", delProId), "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                result = proInventoryService.DelEntity(delProId);
                if (result > 0)
                {
                    MessageBox.Show(string.Format("编号为{0}的商品删除成功！", delProId));
                    btnSearch_Click(null, null);
                }
            }
        }

    }
}
