using System;

using AcceptanceTests.AppInterface.Data;

using TradingSystem.Service;

namespace AcceptanceTests.AppInterface.UserBridge
{
    public class UserBridgeAdapter : BridgeAdapterBase, IUserBridge
    {
        private readonly UserService userService;
        private readonly MarketUserService marketUserService;

        private UserBridgeAdapter
        (
            SystemContext systemContext,
            UserService userService,
            MarketUserService marketService
        )
            : base(systemContext)
        {
            this.userService = userService;
            this.marketUserService = marketService;
        }

        public static UserBridgeAdapter New(SystemContext systemContext)
        {
            return new UserBridgeAdapter(systemContext, UserService.Instance, MarketUserService.Instance);
        }

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
            string result = userService.SignupAsync
            (
                Username,
                signupInfo.Username,
                signupInfo.Password,
                signupInfo.PhoneNumber
            ).Result;
            return result == "success";
        }
        
        public bool Login(UserInfo loginInfo)
        {
            return Connect() && LoginCore(loginInfo);
        }

        private bool LoginCore(UserInfo loginInfo)
        {
            bool success = marketUserService.loginAsync(loginInfo.Username, loginInfo.Password, Username).Result == "success";
            if (success)
            {
                Username = loginInfo.Username;
            }

            return success;
        }

        public bool AssureLogin(UserInfo loginInfo)
        {
            throw new InvalidOperationException($"Should not call this method, use {nameof(Login)} method instead.");
        }

        public bool Logout()
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
            return marketUserService.logoutAsync(Username).Result;
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

        public void tearDown()
        {
            userService.tearDown();
        }
    }
}
