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
        public bool IsLoggedin { get; set; }
        public string phone { get; set; }
        public bool isAdmin { get; set; }

        public DataUser(string username, string password, Address address, string phone)
        {
            this.username = username;
            this.password = password;
            this.IsLoggedin = false;
            this.phone = phone;
            this.isAdmin = false;
        }

        public DataUser(string username, string password, string phone)
        {
            this.username = username;
            this.password = password;
            IsLoggedin = false;
            this.phone = phone;
            this.isAdmin = false;
        }

        public DataUser()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is DataUser user &&
                   username.Equals(user.username) &&
                   password.Equals(user.password) &&
                   phone.Equals(user.phone);

        }
    }
}