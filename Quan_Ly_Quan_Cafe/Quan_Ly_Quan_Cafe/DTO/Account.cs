using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quan_Ly_Quan_Cafe.DTO
{
    public class Account
    {

        private string userName;
        public string UserName 
        {
            get {  return userName; }
            set { userName = value; }
        }
        

        private string disPlayName;
        public string DisPlayName
        {
            get { return disPlayName; }
            set { disPlayName = value; }
        }
        

        private string passWord;
        public string PassWord 
        {
            get { return passWord; }
            set { passWord = value; } 
        }
        

        private int type;
        public int Type
        {
            get { return type; }
            set { type = value;} 
        }


        public Account(string userName, string disPlayName, int type, string passWord = null)
        {
            this.UserName = userName;
            this.DisPlayName = disPlayName;
            this.Type = type;
            this.PassWord = passWord; 
        }

        public Account(DataRow row)
        {
            this.UserName = row["userName"].ToString();
            this.DisPlayName = row["disPlayName"].ToString(); ;
            this.Type = (int)row["type"];
            this.PassWord = row["passWord"].ToString(); ;
        }
    }
}
