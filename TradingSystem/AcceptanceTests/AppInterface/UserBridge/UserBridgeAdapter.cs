using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.Data;

using TradingSystem.Service;

namespace AcceptanceTests.AppInterface.UserBridge
{
    public class UserBridgeAdapter : IUserBridge
    {
        private string? username;
        private UserService userService;
        private MarketService marketService;

        private UserBridgeAdapter(UserService userService, MarketService marketService)
        {
            this.userService = userService;
            this.marketService = marketService;
        }

        public static UserBridgeAdapter New()
        {
            return new UserBridgeAdapter(UserService.Instance, new MarketService());
        }

        /// <summary>
        /// Determines whether we can inteferface with the system,
        /// either as a guest or as a member.
        /// </summary>
        public bool IsConnected => username != null;

        public bool Connect()
        {
            return IsConnected || IdentifyAsGuestCore();
        }

        public bool Disconnect()
        {
            if (!IsConnected)
            {
                return true;
            }

            DisconnectCore();
            return true;
        }

        private void DisconnectCore()
        {
            marketService.RemoveGuest(username);
            username = null;
        }

        public bool AssureSignUp(UserInfo signupInfo)
        {
            throw new InvalidOperationException("Should not call this method, use SignUp method instead.");
        }

        public bool SignUp(UserInfo signupInfo)
        {
            string result = userService.signup
            (
                signupInfo.Username,
                signupInfo.Password,
                signupInfo.Address.State,
                signupInfo.Address.City,
                signupInfo.Address.Street,
                signupInfo.Address.ApartmentNum,
                signupInfo.PhoneNumber
            );
            return result == "success";
        }

        public bool Login(UserInfo loginInfo)
        {
            bool success = userService.login(loginInfo.Username, loginInfo.Password, username) == "success";
            if (success)
            {
                username = loginInfo.Username;
            }

            return success;
        }

        public bool LogOut()
        {
            string guestUsername = LogoutCore();
            if (!IsUsernameValid(guestUsername))
            {
                return false;
            }

            username = guestUsername;
            return true;
        }

        private string LogoutCore()
        {
            return userService.logout(username);
        }

        private bool IdentifyAsGuestCore()
        {
            username = marketService.AddGuest();
            return IsUsernameValid();
        }

        private bool IsUsernameValid()
        {
            return IsUsernameValid(username);
        }
        private static bool IsUsernameValid(string? username)
        {
            return !string.IsNullOrWhiteSpace(username);
        }
    }
}
