using Quan_Ly_Quan_Cafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quan_Ly_Quan_Cafe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance 
        {
            get { if (instance == null) instance = new MenuDAO();return MenuDAO.instance; } 
            private set {   MenuDAO.instance = value;}
        }
        private MenuDAO() { }
        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> listMenu = new List<Menu>();

            string query = "select d.name, bi.count, d.price, d.price*bi.count as totalPrice from dbo.BillInfo as bi, dbo.Bill as b, dbo.Drink as d where bi.idBill = b.id and bi.idDrink = d.id and b.status =0 and b.idTable = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            
            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                listMenu.Add(menu);
            }


            return listMenu;
        }
    }
}
