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
    public partial class FrmProductManage : Form
    {
        ProductService proService = new ProductService();
        ListDataView view = new ListDataView();

        //集合内容改变DataGridView显示内容同步更新
        //BindingList<Products> bindList = new BindingList<Products>();

        string delProId = string.Empty;

        public FrmProductManage()
        {
            InitializeComponent();
        }
        private void FrmProductManage_Load(object sender, EventArgs e)
        {
            cbCategory.Items.AddRange(Common.listCategoryItem.ToArray());
            cbCategory.SelectedIndex = -1;

            DataBind("", "", null, 0);
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

        void DataBind(string proId,string proName,int? categoryId,int curNum)
        {
            Dictionary<string, string> dics = new Dictionary<string, string>()
            {
                { "productId",proId},{ "productName",proName},{ "categoryId",categoryId.ToString()  }
            };
            view = proService.GetListBySearch(dics, int.Parse(lblCurNum.Text.Trim()));//proId, proName, categoryId, int.Parse(lblCurNum.Text.Trim()));
            List <Products> list = (List<Products>)view.ListData;

            this.lblTotalNum.Text = view.PageCount.ToString();


            //设置不自动显示数据库中未绑定的列
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = list;

            #region MyRegion

            //DataTable dt = new DataTable();
            //dt.Columns.Add("ID");
            //dt.Columns.Add("Name");

            //foreach (ListItem item in Common.listCategoryItem)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr[0] = item.ID;
            //    dr[1] = item.Name;
            //    dt.Rows.Add(dr);
            //} 

            //DataGridViewComboBoxColumn column = ((DataGridViewComboBoxColumn)dataGridView1.Columns["CategoryName"]);
            //column.DataSource = dt;
            //column.DisplayMember = "Name";
            //column.ValueMember = "ID";

            //column.Items.Add("aa");
            // this.CategoryName.DataSource = dt;
            //this.CategoryName.DisplayMember = "Name";
            //this.CategoryName.ValueMember = "ID";

            #endregion

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            new FrmAddProduct().ShowDialog();
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
                view.PageIndex ++;
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
            if(!new Regex(@"^[0-9]\d*$").IsMatch(txtGoPageNum.Text.Trim()) )
            {
                MessageBox.Show("请输入正确的页数！");
                return;
            }
            else
            {
                int goPageNum = Convert.ToInt32(txtGoPageNum.Text.Trim());
                if (goPageNum>=1&& goPageNum<= Convert.ToInt32(lblTotalNum.Text.Trim()))
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

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Point pt = dataGridView1.PointToClient(Control.MousePosition);
            DataGridView.HitTestInfo info = dataGridView1.HitTest(pt.X, pt.Y);

            if (info.Type != DataGridViewHitTestType.ColumnHeader)
            {
                foreach (DataGridViewColumn item in dataGridView1.Columns)
                {
                    //if (item.Name.Equals("CategoryName"))
                    //{ 
                    //    DataGridViewComboBoxColumn dgvcbc = new DataGridViewComboBoxColumn();
                    //    dgvcbc.DataPropertyName = "categorys";
                    //    dgvcbc.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    //    dgvcbc.DataSource = Common.listCategoryItem;
                    //    dgvcbc.DisplayMember = "Name";
                    //    dgvcbc.ValueMember = "ID";

                    //    // dgvcbc.Items.AddRange(Common.listCategoryItem);

                    //}

                    if (item.Name.Equals("Supplier") || item.Name.Equals("ProductName") ||  item.Name.Equals("InUnitPrice") || item.Name.Equals("UnitPrice")||item.Name.Equals("Discount") )
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvr = dataGridView1.Rows[e.RowIndex];
            DataGridViewColumn dgvc = dataGridView1.Columns[e.ColumnIndex];

            string propertyName = dgvc.DataPropertyName;
            //string proId = dgvr.Cells["ProductId"].Value.ToString();

            object value = null;
            string proId = string.Empty;
            if (dgvr.Cells[e.ColumnIndex].Value != null)
            {
                value = dgvr.Cells[e.ColumnIndex].Value;
            }
            if(dgvr.Cells["ProductId"].Value != null)
            {
                proId = dgvr.Cells["ProductId"].Value.ToString();
            }

            int result = proService.UpdateEntity(proId, propertyName, value);
            if(result>0)
            {
                MessageBox.Show("商品编号为："+proId +"的"+ dgvc.HeaderText + "更新成功！");
            }

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int result = 0;
            DialogResult dr=  MessageBox.Show(string.Format("确定删除编号为{0}的商品吗？", delProId), "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                result = proService.DelEntity(delProId);
                if (result > 0)
                {
                    MessageBox.Show(string.Format("编号为{0}的商品删除成功！", delProId));
                    btnSearch_Click(null, null);
                }
            }

        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Right)
            {
                //Point pt = dataGridView1.PointToClient(Control.MousePosition);
                DataGridView.HitTestInfo info = dataGridView1.HitTest(e.X,e.Y);

                if(info.Type!=DataGridViewHitTestType.ColumnHeader)
                {
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        if (item.Index == info.RowIndex)
                            item.Selected = true;
                        else
                        item.Selected = false;
                    }

                    //dataGridView1.Rows[info.RowIndex].Selected = true;
                    delProId = dataGridView1.Rows[info.RowIndex].Cells["ProductId"].Value.ToString();
                }

            }

        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            //DataGridViewComboBoxCell dgvCell = dataGridView1.Rows[e.RowIndex].Cells["CategoryName"] as DataGridViewComboBoxCell;
            //dgvCell.DataSource =Common.listCategoryItem; //  Enum.GetValues(typeof(DisCountEnum));
            //dgvCell.DisplayMember = "Name";
            //dgvCell.ValueMember = "ID";

            //int index= dataGridView1.CurrentCell.ColumnIndex;

            //if(index==3)
            //{

            //}



        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            //if (dataGridView1.CurrentCell.OwningColumn.Name == "CategoryName")
            //if (dataGridView1.CurrentCellAddress.X == dataGridView1.Columns["CategoryName"].Index)
            //{
            //    ComboBox cb = e.Control as ComboBox;

            //    if (cb != null)
            //    {
            //        cb.DropDownStyle = ComboBoxStyle.DropDown;
            //        cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //        cb.AutoCompleteSource = AutoCompleteSource.ListItems;
            //        cb.DataSource = Common.listCategoryItem;
            //        cb.DisplayMember = "Name";
            //        cb.ValueMember = "ID";

            //        //foreach (ListItem item in Common.listCategoryItem)
            //        //{
            //        //    ((DataGridViewComboBoxColumn)dataGridView1.Columns["CategoryName"]).Items.Add(item.Name);

            //        //}

            //    }
            //}

            


        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("ID");
            //dt.Columns.Add("Name");

            //foreach (ListItem item in Common.listCategoryItem)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr[0] = item.ID;
            //    dr[1] = item.Name;
            //    dt.Rows.Add(dr);
            //}

            // DataGridViewComboBoxColumn dgcbc = new DataGridViewComboBoxColumn();

            //dgcbc.DataSource = dt;
            //dgcbc.DisplayMember = "Name";
            //dgcbc.ValueMember = "ID";
            //dataGridView1.Columns.Add(dgcbc);

            //((DataGridViewComboBoxColumn)dataGridView1.Columns["CategoryName"]).DataSource = Common.listCategoryItem;

           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
