using SMManager.AdminManager;
using SMManager.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMManager
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        void FrmStartPosition(Form objFrm)
        {
            objFrm.Location = new Point(this.Location.X, this.Location.Y + this.panel1.Height - objFrm.Height + 45);

        }

        private void 用户管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAdminManage objAdminManage = new FrmAdminManage();
            objAdminManage.StartPosition = FormStartPosition.CenterScreen;
            objAdminManage.ShowDialog();
           // FrmStartPosition(objAdminManage);
        }

        private void 添加商品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAddProduct objAddProduct = new FrmAddProduct();
            objAddProduct.StartPosition = FormStartPosition.CenterScreen;
            objAddProduct.ShowDialog();

        }

        private void 商品维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmProductManage objFrm = new FrmProductManage();
            objFrm.StartPosition = FormStartPosition.CenterScreen;
            objFrm.ShowDialog();
        }

        private void 销售管理ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmSaleStat objFrm = new FrmSaleStat();
            objFrm.StartPosition = FormStartPosition.CenterScreen;
            objFrm.ShowDialog();
        }

        private void 库存管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInventoryManage objFrm = new FrmInventoryManage();
            objFrm.StartPosition = FormStartPosition.CenterScreen;
            objFrm.ShowDialog();
        }
    }
}
