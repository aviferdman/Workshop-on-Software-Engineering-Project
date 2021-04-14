using System;
using System.Collections.Generic;
using System.Text;

using AcceptanceTests.AppInterface.Data;

using TradingSystem.Service;

namespace AcceptanceTests.AppInterface.UserBridge
{
    public class UserBridgeAdapter : IUserBridge
    {
        private readonly SystemContext systemContext;
        private readonly UserService userService;
        private readonly MarketUserService marketUserService;

        private UserBridgeAdapter(SystemContext systemContext, UserService userService, MarketUserService marketService)
        {
            this.systemContext = systemContext;
            this.userService = userService;
            this.marketUserService = marketService;
        }

        public static UserBridgeAdapter New(SystemContext systemContext)
        {
            return new UserBridgeAdapter(systemContext, UserService.Instance, MarketUserService.Instance);
        }

        /// <summary>
        /// Determines whether we can inteferface with the system,
        /// either as a guest or as a member.
        /// </summary>
        public bool IsConnected => Username != null;

        public string? Username { get => systemContext.TokenUsername; set => systemContext.TokenUsername = value; }

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
            marketUserService.RemoveGuest(Username);
            Username = null;
        }

        public bool AssureSignUp(UserInfo signupInfo)
        {
            throw new InvalidOperationException($"Should not call this method, use {nameof(SignUp)} method instead.");
        }

        public bool SignUp(UserInfo signupInfo)
        {
            string result = userService.Signup
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
            bool success = userService.Login(loginInfo.Username, loginInfo.Password, Username) == "success";
            if (success)
            {
                Username = loginInfo.Username;
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

            Username = guestUsername;
            return true;
        }

        private string LogoutCore()
        {
            return userService.Logout(Username);
        }

        private bool IdentifyAsGuestCore()
        {
            Username = marketUserService.AddGuest();
            return IsUsernameValid();
        }

        private bool IsUsernameValid()
        {
            return IsUsernameValid(Username);
        }
        private static bool IsUsernameValid(string? username)
        {
            return !string.IsNullOrWhiteSpace(username);
        }
    }
}
