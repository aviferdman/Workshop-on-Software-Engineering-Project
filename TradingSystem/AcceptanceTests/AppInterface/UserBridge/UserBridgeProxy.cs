﻿using System.Collections.Generic;

using AcceptanceTests.AppInterface.Data;

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

        public bool LogOut()
        {
            if (RealBridge == null)
            {
                return false;
            }

            bool success = RealBridge.LogOut();
            if (success)
            {
                SystemContext!.LoggedInUser = null;
            }
            return success;
        }
    }
}