using Quan_Ly_Quan_Cafe.DAO;
using Quan_Ly_Quan_Cafe.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Quan_Ly_Quan_Cafe.fAccountProfile;

namespace Quan_Ly_Quan_Cafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get {  return loginAccount; } 
            set {  loginAccount = value; ChangeAccount(loginAccount.Type); } 
        }

        public fTableManager(Account acc)
        {
            
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbbSwitchTable);
        }
        #region Method
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += "( " + loginAccount.DisPlayName + " )";
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbbCategoryDrink.DataSource = listCategory;
            cbbCategoryDrink.DisplayMember = "Name";
        }
        void LoadDrinkListByCategoryID(int id)
        {
            List<Drink> listDrink = DrinkDAO.Instance.GetDrinkByCategoryID(id);
            cbbDrink.DataSource = listDrink;
            cbbDrink.DisplayMember = "Name";
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button(){ Width = TableDAO.TableWidth, Height = TableDAO.TableHeight};
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Font = new Font("Time new roman", 14);
                btn.Click += btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Yellow;
                        break;
                    default:
                        btn.BackColor = Color.OrangeRed;
                        break;
                }
                flpTable.Controls.Add(btn);

            }
        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<Quan_Ly_Quan_Cafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);

            float totalPrice = 0;
            foreach (Quan_Ly_Quan_Cafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvitem = new ListViewItem(item.DrinkName.ToString());
                lsvitem.SubItems.Add(item.Count.ToString());
                lsvitem.SubItems.Add(item.Price.ToString());
                lsvitem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvitem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            txtTotalPrice.Text = totalPrice.ToString("c", culture);
            
        }

        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion

        #region Events
        private void btn_Click(object? sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }

        private void đăngXuấtToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ThôngtincánhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        private void f_UpdateAccount(object? sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản ( " + e.Acc.DisPlayName + ")";
        }

        private  void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.InsertDrink += F_InsertDrink;
            f.UpdateDrink += F_UpdateDrink;
            f.DeleteDrink += F_DeleteDrink;
            f.ShowDialog();
        }

        private void F_DeleteDrink(object? sender, EventArgs e)
        {
            LoadDrinkListByCategoryID((cbbCategoryDrink.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_UpdateDrink(object? sender, EventArgs e)
        {
            LoadDrinkListByCategoryID((cbbCategoryDrink.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_InsertDrink(object? sender, EventArgs e)
        {
            LoadDrinkListByCategoryID((cbbCategoryDrink.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void cbbCategoryDrink_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadDrinkListByCategoryID(id);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if(table == null)
            {
                MessageBox.Show("Vui lòng chọn bàn !!!");
                return;
            }
            
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int idDrink = (cbbDrink.SelectedItem as Drink).ID;
            int count = (int)nmDrinkCount.Value;

            if(idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), idDrink, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idDrink, count); 
            }
            ShowBill(table.ID);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;

            double totalPrice = Convert.ToDouble(txtTotalPrice.Text.Split(' ')[0]);
            double finalTotalPrice = totalPrice - (totalPrice/100)*discount;


            if(idBill != -1)
            {
                if(MessageBox.Show(String.Format("Bạn có chắc thanh toán cho bàn {0}\n Tổng tiền - (Tổng tiền / 100) x Giảm giá \n => {1} - ({1}/100) x {2} = {3}",  table.Name,totalPrice, discount, finalTotalPrice*1000), "Thông báo !!!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK) ;
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    ShowBill(table.ID);

                    LoadTable();
                }

            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            
            int id1 = (lsvBill.Tag as Table).ID;
            int id2 = (cbbSwitchTable.SelectedItem as Table).ID;

            if (MessageBox.Show(String.Format("Bạn muốn chuyển bàn {0} qua bàn {1} ", (lsvBill.Tag as Table).Name, (cbbSwitchTable.SelectedItem as Table).Name),"Thong bao !!!", MessageBoxButtons.OKCancel)==System.Windows.Forms.DialogResult.OK) 
            { 
            TableDAO.Instance.SwitchTable(id1, id2);

            LoadTable();
            }
        }
        #endregion


    }
}
