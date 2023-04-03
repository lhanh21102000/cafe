using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quan_Ly_Quan_Cafe.DTO
{
    public class BillInfo
    {
        private int iD;
        public int ID { get => iD; set => iD = value; }
        
        private int billID;
        public int BillID { get => billID; set => billID = value; }
        
        private int drinkID;
        public int DrinkID { get => drinkID; set => drinkID = value; }
        

        private int count;
        public int Count { get => count; set => count = value; }

        public BillInfo(int id, int billID, int drinkID, int count)
        {
            this.ID = id;
            this.BillID = billID;
            this.DrinkID = drinkID;
            this.Count = count;
        }

        public BillInfo(DataRow row)
        {
            this.ID = (int)row["id"];
            this.BillID = (int)row["idbill"];
            this.DrinkID = (int)row["iddrink"];
            this.Count = (int)row["count"];
        }
    }
}
