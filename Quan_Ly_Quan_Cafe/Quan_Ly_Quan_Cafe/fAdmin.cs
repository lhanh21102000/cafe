using Quan_Ly_Quan_Cafe.DAO;
using Quan_Ly_Quan_Cafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quan_Ly_Quan_Cafe
{
    public partial class fAdmin : Form
    {
        BindingSource DrinkList = new BindingSource();

        BindingSource accountList = new BindingSource();

        public Account loginAccount;

        public fAdmin()
        {
            InitializeComponent();
            Load();
        }

        #region Methods

        List<Drink> SearchDrinkByName(string name)
        {
            List<Drink> listDrink = DrinkDAO.Instance.SearchDrinkByName(name);

            return listDrink;
        }

        void AddDrinkBinding()
        {
            txtDrinkName.DataBindings.Add(new Binding("Text", dtgvDrink.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txtDrinkID.DataBindings.Add(new Binding("text", dtgvDrink.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmDrinkPrice.DataBindings.Add(new Binding("value", dtgvDrink.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        void Load()
        {
            dtgvDrink.DataSource = DrinkList;
            dtgvAccount.DataSource = accountList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListDrink();
            LoadAccount();
            LoadCategoryIntoCombobox(cbbCategory);

            AddDrinkBinding();
            AddAccountBinding();
        }

        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "name";
        }
        
        void LoadListDrink()
        {
            DrinkList.DataSource = DrinkDAO.Instance.GetListDrink();

        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource =  BillDAO.Instance.GetListBillByDate(checkIn, checkOut);
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }


        #endregion


        #region Events
        private void btnSearchDrink_Click(object sender, EventArgs e)
        {
            DrinkList.DataSource = SearchDrinkByName(txtSearchDrinkName.Text);
        }
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
        private void btnShowDrink_Click(object sender, EventArgs e)
        {
            LoadListDrink();
        }

        private void txtDrinkID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvDrink.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvDrink.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    cbbCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbbCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbbCategory.SelectedIndex = index;

                }
            }
            catch
            {

            }
        }
        private void btnAdđrink_Click(object sender, EventArgs e)
        {
            

            string name = txtDrinkName.Text;
            int categoryID = (cbbCategory.SelectedItem as Category).ID;
            float price = (float)nmDrinkPrice.Value;

            if (DrinkDAO.Instance.InsertDrink(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công !!!");
                LoadListDrink();
                if (insertDrink != null)
                    insertDrink(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thất bại !!!");
            }
        }

        private void btnEditDrink_Click(object sender, EventArgs e)
        {
            string name = txtDrinkName.Text;
            int categoryID = (cbbCategory.SelectedItem as Category).ID;
            float price = (float)nmDrinkPrice.Value;
            int id = Convert.ToInt32(txtDrinkID.Text);

            if (DrinkDAO.Instance.UpdateDrink(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công !!!");
                LoadListDrink();
                if (updateDrink != null)
                    updateDrink(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Thất bại !!!");
            }
        }

        private void btnDeleteDrink_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtDrinkID.Text);

            if (DrinkDAO.Instance.DeleteDrink(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListDrink();
                if (deleteDrink != null)
                    deleteDrink(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }

        private event EventHandler insertDrink;
        public event EventHandler InsertDrink
        {
            add { insertDrink += value; }
            remove { insertDrink -= value; }
        }

        private event EventHandler deleteDrink;
        public event EventHandler DeleteDrink
        {
            add { deleteDrink += value; }
            remove { deleteDrink -= value; }
        }

        private event EventHandler updateDrink;
        public event EventHandler UpdateDrink
        {
            add { updateDrink += value; }
            remove { updateDrink -= value; }
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)nmType.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            int type = (int)nmType.Value;

            EditAccount(userName, displayName, type);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;

            ResetPass(userName);
        }


        #endregion
    }
}
