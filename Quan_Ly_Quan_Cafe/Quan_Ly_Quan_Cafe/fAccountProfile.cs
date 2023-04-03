using Quan_Ly_Quan_Cafe.DAO;
using Quan_Ly_Quan_Cafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quan_Ly_Quan_Cafe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        void ChangeAccount(Account acc)
        {
            txtUserName.Text = LoginAccount.UserName;
            txtDisplayName.Text = LoginAccount.DisPlayName;
        }
        public fAccountProfile(Account acc)
        {
            InitializeComponent();
            LoginAccount = acc;
        }
        private event EventHandler <AccountEvent> updateAccount;

        public event EventHandler <AccountEvent> UpdateAccount
            {
                add { updateAccount += value; }
                remove { updateAccount -= value; }
            }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void UpdateAccountInfo()
        {
            string displayname = txtDisplayName.Text;
            string password = txtPassWord.Text;
            string newpassword = txtNewPassWord.Text;
            string renewPassword = txtReNewPassWord.Text;
            string username = txtUserName.Text;

            if (!newpassword.Equals(renewPassword))
            {
                MessageBox.Show("Mật khẩu và mật khẩu nhập lại không giống nhau !!");

            }
            else 
            {
                if(AccountDAO.Instance.UpdateAccount(username, displayname, password, renewPassword))
                {
                    MessageBox.Show("Cập nhật thành công !!");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(username)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu !!!"); 
                }
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
        public class AccountEvent:EventArgs
        {
            private Account acc;

            public Account Acc { get => acc; set => acc = value; }
            public AccountEvent(Account acc)
            {
                this.Acc = acc;
                
            }
        }


    
    }
}
