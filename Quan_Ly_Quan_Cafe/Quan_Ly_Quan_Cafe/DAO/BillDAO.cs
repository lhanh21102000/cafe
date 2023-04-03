using Quan_Ly_Quan_Cafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quan_Ly_Quan_Cafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value;} 
        }
        private BillDAO() { }
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select *from dbo.Bill where idTable = " + id + " and status = 0");
            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;//thanh cong thi tra ra billID nguoc lai khong thanh cong tra ra -1;
        }
        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec usp_InsertBill @idTable", new object[] {id}); 
        }
        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("select MAX(id) from dbo.Bill   ");
            }
            
            catch 
            {
                return 1;
                
            }
        }
        public DataTable GetListBillByDate( DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("exec usp_GetListBillByDate @checkIn , @checkOut", new object[] {checkIn, checkOut});
        }
        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "update  dbo.Bill set DateCheckOut = GETDATE(), status = 1, " + "discount = " + discount  + ", totalPrice = " + totalPrice*1000 + "where id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
    }
}
