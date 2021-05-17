using TradingSystem.Business.Market;

using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.UserManagement
{
    public class DataUser
    {
        private string username { get; set; }
        private string password { get; set; }
        private bool isLoggedin { get; set; }
        private Address address { get; set; }
        private string phone { get; set; }
        public bool isAdmin { get; set; }

        public DataUser(string username, string password, Address address, string phone)
        {
            this.username = username;
            this.password = password;
            this.address = address;
            this.isLoggedin = false;
            this.phone = phone;
        }

        

        public override bool Equals(object obj)
        {
            return obj is DataUser user &&
                   username.Equals(user.username) &&
                   password.Equals(user.password) &&
                   address.Equals(user.address) &&
                   phone.Equals(user.phone);

        }
    }
}