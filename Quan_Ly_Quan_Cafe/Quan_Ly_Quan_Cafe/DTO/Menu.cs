using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quan_Ly_Quan_Cafe.DTO
{
    public class Menu
    {
        private string drinkName;
        public string DrinkName { get => drinkName; set => drinkName = value; }
        

        private int count;
        public int Count { get => count; set => count = value; }
        

        private float price;
        public float Price { get => price; set => price = value; }
        
        private float totalPrice;
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }

        public Menu(string drinkName, int count, float price, float totalPrice = 0)
        {
            this.DrinkName = drinkName;
            this.Count = count;
            this.Price = price;
            this.TotalPrice = totalPrice;

        }
        public Menu(DataRow row)
        {
            this.DrinkName = row["Name"].ToString();
            this.Count = (int)row["count"];
            this.Price =(float)Convert.ToDouble(row["price"].ToString());
            this.TotalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());

        }


    }
}
