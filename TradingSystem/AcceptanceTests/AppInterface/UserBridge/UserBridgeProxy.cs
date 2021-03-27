using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.UserBridge
{
    public class UserBridgeProxy : ProxyBase<IUserBridge>, IUserBridge
    {
        // The key lookup is by username. We also want to know the password that the user registered with.
        private readonly IDictionary<UserInfo, UserInfo> registeredUsers;

        public UserBridgeProxy(IUserBridge? realBridge) : base(realBridge)
        {
            registeredUsers = new Dictionary<UserInfo, UserInfo>();
        }

        public override IUserBridge Bridge => this;

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
            if (!registeredUsers.ContainsKey(signupInfo))
            {
                registeredUsers.Add(signupInfo, signupInfo);
            }
            if (RealBridge == null)
            {
                return true;
            }

            return RealBridge.SignUp(signupInfo);
        }

        public bool Login(UserInfo loginInfo)
        {
            return RealBridge == null || RealBridge.Login(loginInfo);
        }

        public bool LogOut()
        {
            return RealBridge == null || RealBridge.LogOut();
        }
    }
}
