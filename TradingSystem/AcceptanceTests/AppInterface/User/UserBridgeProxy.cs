using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.User
{
    public class UserBridgeProxy : IUserBridge
    {
        private readonly IUserBridge? realBridge;
        private readonly IDictionary<string, string> registeredUsers;

        public UserBridgeProxy(IUserBridge? realBridge)
        {
            this.realBridge = realBridge;
            registeredUsers = new Dictionary<string, string>();
        }

        public bool AssureSignUp(string username, string password)
        {
            if (registeredUsers.TryGetValue(username, out string? registeredPassword))
            {
                if (registeredPassword != password)
                {
                    throw new TestUserRegisterMismatchException(username);
                }

                return true;
            }

            return SignUp(username, password);
        }

        public bool SignUp(string username, string password)
        {
            if (!registeredUsers.ContainsKey(username))
            {
                registeredUsers.Add(username, password);
            }
            if (realBridge == null)
            {
                return true;
            }

            return realBridge.SignUp(username, password);
        }

        public bool Login(string username, string password)
        {
            if (realBridge == null)
            {
                return true;
            }

            return realBridge.Login(username, password);
        }

        public bool LogOut()
        {
            if (realBridge == null)
            {
                return true;
            }

            return realBridge.LogOut();
        }
    }
}
