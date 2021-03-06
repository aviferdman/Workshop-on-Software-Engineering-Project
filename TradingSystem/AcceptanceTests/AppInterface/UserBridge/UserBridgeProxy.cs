using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface.UserBridge
{
    public class UserBridgeProxy : ProxyBase<IUserBridge>, IUserBridge
    {
        // The key lookup is by username. We also want to know the password that the user registered with.
        private readonly IDictionary<UserInfo, UserInfo> registeredUsers;

        public UserBridgeProxy() : this(null) { }
        public UserBridgeProxy(IUserBridge? realBridge) : base(realBridge)
        {
            registeredUsers = new Dictionary<UserInfo, UserInfo>();
        }

        public override IUserBridge Bridge => this;

        public bool Connect()
        {
            return RealBridge != null && RealBridge.Connect();
        }

        public bool Disconnect()
        {
            return RealBridge != null && RealBridge.Disconnect();
        }

        public bool AssureSignUp(UserInfo signupInfo)
        {
            if (registeredUsers.TryGetValue(signupInfo, out UserInfo? registeredUser))
            {
                if (registeredUser.Password != signupInfo.Password)
                {
                    throw new TestUserRegisterMismatchException(signupInfo.Username);
                }

                return true;
            }

            return SignUp(signupInfo);
        }

        public bool SignUp(UserInfo signupInfo)
        {
            if (RealBridge == null)
            {
                return false;
            }

            bool success = RealBridge.SignUp(signupInfo);
            if (success && !registeredUsers.ContainsKey(signupInfo))
            {
                registeredUsers.Add(signupInfo, signupInfo);
            }
            return success;
        }

        public bool Login(UserInfo loginInfo)
        {
            if (RealBridge == null)
            {
                return false;
            }

            bool success = RealBridge.Login(loginInfo);
            if (success)
            {
                SystemContext!.LoggedInUser = loginInfo;
            }
            return success;
        }

        public bool AssureLogin(UserInfo loginInfo)
        {
            UserInfo? loggedInUser = SystemContext!.LoggedInUser;
            if (loggedInUser != null)
            {
                if (loggedInUser.Equals(loginInfo))
                {
                    return true;
                }
                if (!Logout())
                {
                    return false;
                }
            }

            return Login(loginInfo);
        }

        public bool Logout()
        {
            if (RealBridge == null)
            {
                return false;
            }

            bool success = RealBridge.Logout();
            if (success)
            {
                SystemContext!.LoggedInUser = null;
            }
            return success;
        }

        public void tearDown()
        {
            if (RealBridge == null)
            {
                RealBridge.tearDown();
            }
        }
    }
}
