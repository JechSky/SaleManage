using CommonTools;
using DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMManager
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Common.Info();
            CommonTool.Info();
            //if (!CommonTool.commonTool.StartOnlyProcess())
            //{
            //    Application.Exit();
            //    return;
            //}

            //FrmLogin objForm = new FrmLogin();
            //DialogResult result = objForm.ShowDialog();

            //if (result == DialogResult.OK)
            //{
            //    Application.Run(new FrmMain());
            //}
            //else
            //{
            //    Application.Exit();
            //}

            Application.Run(new FrmMain());


        }
    }
}
