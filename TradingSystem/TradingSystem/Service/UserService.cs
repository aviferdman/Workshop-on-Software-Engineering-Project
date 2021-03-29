using System;
using System.Collections.Generic;
using System.Text;
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
        public String signup(string username, string password, string address)
        {
            return userManagement.SignUp(username, password, address);
        }

        public String login(string username, string password)
        {
            return userManagement.LogIn(username, password);
        }
    }
}
