using System;
using System.Configuration;
using TradingSystem.Business.Market;
using TradingSystem.Business.UserManagement;
using TradingSystem.Notifications;
using TradingSystem.PublisherComponent;

namespace TradingSystem.Service
{
    public class UserService
    {
        private static readonly Lazy<UserService> instanceLazy = new Lazy<UserService> (() => new UserService(), true);

        private readonly UserManagement userManagement;

        private UserService()
        {
            this.userManagement = UserManagement.Instance;
        }

        public static UserService Instance => instanceLazy.Value;

        public string Signup(string guestusername, string username, string password, string _state, string _city, string _street, string _apartmentNum, string phone, bool isAdmin)
        {
            string ans= userManagement.SignUp(username, password, new Address(_state, _city, _street, _apartmentNum), phone, isAdmin);
            PublisherManagement.Instance.EventNotification(username, EventType.RegisterEvent, ConfigurationManager.AppSettings[ans]);
            return ans;
                
        }

        public bool isMember(string username)
        {
            return userManagement.isMember(username);
        }
        public bool isLoggedIn(string username)
        {
            return userManagement.isLoggedIn(username);
        }


    }
}
