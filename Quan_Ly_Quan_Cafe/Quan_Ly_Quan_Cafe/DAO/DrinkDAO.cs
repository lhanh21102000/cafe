using Quan_Ly_Quan_Cafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quan_Ly_Quan_Cafe.DAO
{
    public class DrinkDAO
    {
        private static DrinkDAO instance;

        public static DrinkDAO Instance 
        {
            get { if (instance == null) instance = new DrinkDAO(); return DrinkDAO.instance; }
            private set { DrinkDAO.instance = value; } 
        }

        private DrinkDAO() { }
        

        public List<Drink> GetDrinkByCategoryID(int id)
        {
            List<Drink> list = new List<Drink>();

            string query = "select *from Drink where idDrinkCategory = " +id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Drink drink = new Drink(item);
                list.Add(drink);
            }

            return list;
        }
        public List<Drink> GetListDrink()
        {
            List<Drink> list = new List<Drink>();

            string query = "select * from dbo.Drink";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Drink drink = new Drink(item);
                list.Add(drink);
            }

            return list;
        }
        public List<Drink> SearchDrinkByName(string name)
        {

            List<Drink> list = new List<Drink>();

            string query = string.Format("SELECT * FROM dbo.Drink WHERE dbo.fuConvertToUnsign1(name) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Drink drink = new Drink(item);
                list.Add(drink);
            }

            return list;
        }

        public bool InsertDrink(string name, int id, float price)
        {
            string query = string.Format("insert into dbo.Drink ( idDrinkCategory , name , price ) values ({0} , N'{1}', {2})", id, name, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }


        public bool UpdateDrink(int idDrink, string name, int id, float price)
        {
            string query = string.Format("UPDATE dbo.Drink SET name = N'{0}', idDrinkCategory = {1}, price = {2} WHERE id = {3}", name, id, price, idDrink);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteDrink(int idDrink)
        {
            BillInfoDAO.Instance.DeleteBillInfoByDrinkID(idDrink);

            string query = string.Format("Delete dbo.Drink where id = {0}", idDrink);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
