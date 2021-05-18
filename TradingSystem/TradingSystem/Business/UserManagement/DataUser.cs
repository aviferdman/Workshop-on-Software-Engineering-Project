using TradingSystem.Business.Market;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.UserManagement
{
    public class DataUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool isLoggedin { get; set; }
        public Address address { get; set; }
        public string phone { get; set; }
        public bool isAdmin { get; set; }

        public DataUser(string username, string password, Address address, string phone)
        {
            this.username = username;
            this.password = password;
            this.address = address;
            this.isLoggedin = false;
            this.phone = phone;
        }

        public DataUser()
        {
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