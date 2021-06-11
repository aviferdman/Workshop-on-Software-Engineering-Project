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
        private readonly UserManagement userManagement;

        public UserService(UserManagement userManagement)
        {
            this.userManagement = userManagement;
        }

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
