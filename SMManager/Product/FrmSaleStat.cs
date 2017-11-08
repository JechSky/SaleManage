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
    public partial class FrmSaleStat : Form
    {
        SaleListService saleService = new SaleListService();
        ListDataView view = new ListDataView();

        //List<String> colsHeaderText_V = new List<String>();
        //List<String> colsHeaderText_H = new List<String>();

        public FrmSaleStat()
        {
            InitializeComponent();

            //colsHeaderText_V.Add("PHONE1");
            //colsHeaderText_V.Add("PHONE2");

            //colsHeaderText_H.Add("IMAGEINDEX");
            //colsHeaderText_H.Add("PARENTID");
            //colsHeaderText_H.Add("DEPARTMENT");
            //colsHeaderText_H.Add("LOCATION");
        }
        private void FrmSaleStat_Load(object sender, EventArgs e)
        {
            cbCategory.Items.AddRange(Common.listCategoryItem.ToArray());
            dtpS.Value = DateTime.Now.AddMonths(-3);

            BindData(DateTime.Now.AddMonths(-3), DateTime.Now , int.Parse(lblCurNum.Text.Trim()));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime dts = dtpS.Value;
            DateTime dte = dtpE.Value;
            string serialNum = txtSerialNum.Text.Trim();
            string productId = txtProductId.Text.Trim();
            string productName = txtProductName.Text.Trim();
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

            BindData(dts, dte, int.Parse(lblCurNum.Text.Trim()),serialNum,productId,productName, categoryId);
        }

        void BindData(DateTime dts, DateTime dte,int curNum=1, string serialNum="", string productId = "", string productName = "", int? categoryId = null)
        {
            view = saleService.GetListBySearch(dts, dte, curNum, serialNum, productId, productName, categoryId);
            this.lblTotalNum.Text = view.PageCount.ToString();
            List<SalesListDetail> list = (List<SalesListDetail>)view.ListData;

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = list;

            #region MyRegion
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    for (int j = i+1; j < dataGridView1.Rows.Count; j++)
            //    {
            //        if (dataGridView1.Rows[i].Cells["SerialNum"].Value.ToString().Equals(dataGridView1.Rows[j].Cells["SerialNum"].Value.ToString()))
            //        {
            //            dataGridView1
            //        }
            //    }  

            //} 
            #endregion

            double total = 0.00;
            total = Convert.ToDouble(view.Remark);

            lblProfit.Text = total.ToString();
        }


        private void btnPre_Click(object sender, EventArgs e)
        {
            if(view.PageIndex<=view.PageCount&& view.PageIndex>1)
            {
                view.PageIndex--;
            }
            else
            {
                view.PageIndex = 1;
            }
            lblCurNum.Text = view.PageIndex.ToString();
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
            
            lblCurNum.Text = view.PageIndex.ToString();
            MyEventArgs arg = new MyEventArgs(false);
            btnSearch_Click(null, arg);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if(!new Regex(@"^[0-9]\d*$").IsMatch(txtGoPageNum.Text.Trim()))
            {
                MessageBox.Show("请输入正确的页数！");
                return;
            }
            else
            {
                int goPageNum = Convert.ToInt32(txtGoPageNum.Text.Trim());
                if(goPageNum>=1&&goPageNum<=Convert.ToInt32(lblTotalNum.Text.Trim()))
                {
                    view.PageIndex = Convert.ToInt32(txtGoPageNum.Text.Trim());
                    lblCurNum.Text = view.PageIndex.ToString();
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

        
        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            #region MyRegion

            //foreach (string fieldHeaderText in colsHeaderText_H)
            //{
            //    //纵向合并
            //    if (e.ColumnIndex >= 0 && this.dataGridView1.Columns[e.ColumnIndex].HeaderText == fieldHeaderText && e.RowIndex >= 0)
            //    {
            //        using (
            //            Brush gridBrush = new SolidBrush(this.dataGridView1.GridColor),
            //            backColorBrush = new SolidBrush(e.CellStyle.BackColor))
            //        {
            //            using (Pen gridLinePen = new Pen(gridBrush))
            //            {
            //                // 擦除原单元格背景
            //                e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

            //                /****** 绘制单元格相互间隔的区分线条，datagridview自己会处理左侧和上边缘的线条，因此只需绘制下边框和和右边框
            //                 DataGridView控件绘制单元格时，不绘制左边框和上边框，共用左单元格的右边框，上一单元格的下边框*****/

            //                //不是最后一行且单元格的值不为null
            //                if (e.RowIndex < this.dataGridView1.RowCount - 1 && this.dataGridView1.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null)
            //                {
            //                    //若与下一单元格值不同
            //                    if (e.Value.ToString() != this.dataGridView1.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
            //                    {
            //                        //下边缘的线
            //                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
            //                        e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
            //                        //绘制值
            //                        if (e.Value != null)
            //                        {
            //                            e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
            //                                Brushes.Crimson, e.CellBounds.X + 2,
            //                                e.CellBounds.Y + 2, StringFormat.GenericDefault);
            //                        }
            //                    }
            //                    //若与下一单元格值相同 
            //                    else
            //                    {
            //                        //背景颜色
            //                        //e.CellStyle.BackColor = Color.LightPink;   //仅在CellFormatting方法中可用
            //                        this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightBlue;
            //                        this.dataGridView1.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Style.BackColor = Color.LightBlue;
            //                        //只读（以免双击单元格时显示值）
            //                        this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            //                        this.dataGridView1.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].ReadOnly = true;
            //                    }
            //                }
            //                //最后一行或单元格的值为null
            //                else
            //                {
            //                    //下边缘的线
            //                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
            //                        e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

            //                    //绘制值
            //                    if (e.Value != null)
            //                    {
            //                        e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
            //                            Brushes.Crimson, e.CellBounds.X + 2,
            //                            e.CellBounds.Y + 2, StringFormat.GenericDefault);
            //                    }
            //                }

            //                ////左侧的线（）
            //                //e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
            //                //    e.CellBounds.Top, e.CellBounds.Left,
            //                //    e.CellBounds.Bottom - 1);

            //                //右侧的线
            //                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
            //                    e.CellBounds.Top, e.CellBounds.Right - 1,
            //                    e.CellBounds.Bottom - 1);

            //                //设置处理事件完成（关键点），只有设置为ture,才能显示出想要的结果。
            //                e.Handled = true;
            //            }
            //        }
            //    }
            //}

            //foreach (string fieldHeaderText in colsHeaderText_V)
            //{
            //    //横向合并
            //    if (e.ColumnIndex >= 0 && this.dataGridView1.Columns[e.ColumnIndex].HeaderText == fieldHeaderText && e.RowIndex >= 0)
            //    {
            //        using (
            //            Brush gridBrush = new SolidBrush(this.dataGridView1.GridColor),
            //            backColorBrush = new SolidBrush(e.CellStyle.BackColor))
            //        {
            //            using (Pen gridLinePen = new Pen(gridBrush))
            //            {
            //                // 擦除原单元格背景
            //                e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

            //                /****** 绘制单元格相互间隔的区分线条，datagridview自己会处理左侧和上边缘的线条，因此只需绘制下边框和和右边框
            //                 DataGridView控件绘制单元格时，不绘制左边框和上边框，共用左单元格的右边框，上一单元格的下边框*****/

            //                //不是最后一列且单元格的值不为null
            //                if (e.ColumnIndex < this.dataGridView1.ColumnCount - 1 && this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value != null)
            //                {
            //                    if (e.Value.ToString() != this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value.ToString())
            //                    {
            //                        //右侧的线
            //                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top,
            //                            e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
            //                        //绘制值
            //                        if (e.Value != null)
            //                        {
            //                            e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
            //                                Brushes.Crimson, e.CellBounds.X + 2,
            //                                e.CellBounds.Y + 2, StringFormat.GenericDefault);
            //                        }
            //                    }
            //                    //若与下一单元格值相同 
            //                    else
            //                    {
            //                        //背景颜色
            //                        //e.CellStyle.BackColor = Color.LightPink;   //仅在CellFormatting方法中可用
            //                        this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightPink;
            //                        this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Style.BackColor = Color.LightPink;
            //                        //只读（以免双击单元格时显示值）
            //                        this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            //                        this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].ReadOnly = true;
            //                    }
            //                }
            //                else
            //                {
            //                    //右侧的线
            //                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top,
            //                        e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

            //                    //绘制值
            //                    if (e.Value != null)
            //                    {
            //                        e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
            //                            Brushes.Crimson, e.CellBounds.X + 2,
            //                            e.CellBounds.Y + 2, StringFormat.GenericDefault);
            //                    }
            //                }
            //                //下边缘的线
            //                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
            //                                            e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
            //                e.Handled = true;
            //            }
            //        }

            //    }
            //} 

            #endregion

        }
    }
}
