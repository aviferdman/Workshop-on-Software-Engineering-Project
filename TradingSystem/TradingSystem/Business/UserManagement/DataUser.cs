using System;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.UserManagement
{
    class DataUser
    {
        private string username;
        private string password;
        private bool isLoggedin;
        private string address;

        public DataUser(string username, string password, string address)
        {
            this.username = username;
            this.password = password;
            this.address = address;
            this.isLoggedin = false;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Address { get => address; set => address = value; }

        public override bool Equals(object obj)
        {
            return obj is DataUser user &&
                   username.Equals(user.username) &&
                   password.Equals(user.password) &&
                   address.Equals(user.address);
        }
    }
}
