using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quan_Ly_Quan_Cafe.DTO
{
    public class Drink
    {
        private int iD;
        public int ID { get => iD; set => iD = value; }


        private int categoryID;
        public int CategoryID { get => categoryID; set => categoryID = value;}


        private string name;
        public string Name { get => name; set => name = value; }


        private float price;
        public float Price { get => price; set => price = value; }

        public Drink(int id,int categoryID, string name, float price)
        {
            this.ID = id;
            this.CategoryID = categoryID;
            this.Name = name;
            this.Price = price;

        }
     
        public Drink(DataRow row)
        {
            this.ID = (int)row["id"];
            this.CategoryID = (int)row["iddrinkcategory"];
            this.Name = row["name"].ToString();
            this.Price = (float)Convert.ToDouble(row["price"].ToString());

        }
    }
}
