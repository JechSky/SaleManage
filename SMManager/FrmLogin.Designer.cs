﻿namespace SMManager
{
    partial class FrmLogin
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtAccount = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtAccount
            // 
            this.txtAccount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAccount.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAccount.ForeColor = System.Drawing.Color.Black;
            this.txtAccount.Location = new System.Drawing.Point(76, 66);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(176, 23);
            this.txtAccount.TabIndex = 3;
            this.txtAccount.Text = "2000";
            // 
            // txtPwd
            // 
            this.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPwd.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPwd.ForeColor = System.Drawing.Color.Black;
            this.txtPwd.Location = new System.Drawing.Point(76, 120);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(176, 23);
            this.txtPwd.TabIndex = 4;
            this.txtPwd.Text = "123456";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(76, 186);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(176, 36);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 297);
            this.Controls.Add(this.txtAccount);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.btnLogin);
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "后台登录";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAccount;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Button btnLogin;
    }
}

