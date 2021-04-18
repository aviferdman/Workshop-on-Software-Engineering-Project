using TradingSystem.Business.Market;

using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.UserManagement
{
    public class DataUser
    {
        private string username;
        private string password;
        private bool isLoggedin;
        private Address address;
        private string phone;

        public DataUser(string username, string password, Address address, string phone)
        {
            this.username = username;
            this.password = password;
            this.address = address;
            this.isLoggedin = false;
            this.phone = phone;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public Address Address { get => address; set => address = value; }
        public bool IsLoggedin { get => isLoggedin; set => isLoggedin = value; }
        public string Phone { get => phone; set => phone = value; }

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