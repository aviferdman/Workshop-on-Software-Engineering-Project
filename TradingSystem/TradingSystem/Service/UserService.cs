using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.Business.UserManagement;

namespace TradingSystem.Service
{
    class UserService
    {
        private UserManagement userManagement;
        private static readonly Lazy<UserService>
        lazy =
        new Lazy<UserService>
            (() => new UserService());

        private UserService()
        {
            this.userManagement = UserManagement.Instance;
        }

        public static UserService Instance { get { return lazy.Value; } }
        public String signup(string username, string password, string _state, string _city, string _street, string _apartmentNum, string phone)
        {
            return userManagement.SignUp(username, password, new Address(_state, _city, _street, _apartmentNum), phone);
        }

        public String login(string username, string password, string guestusername)
        {
            return userManagement.LogIn(username, password, guestusername);
        }

        public string logout(string username)
        {
            return userManagement.Logout(username);
        }
    }
}
