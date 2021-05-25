using System;
using System.Configuration;
using System.Threading.Tasks;
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

        public void tearDown()
        {
            userManagement.tearDown();
        }

        public async Task<string> SignupAsync(string guestusername, string username, string password,string phone)
        {
            string ans= await userManagement.SignUp(username, password, phone);
            return ans;
                
        }

        public async Task<bool> isMember(string username)
        {
            return await userManagement.isMember(username);
        }
        public async Task<bool> isLoggedIn(string username)
        {
            return await userManagement .isLoggedIn(username);
        }


    }
}
