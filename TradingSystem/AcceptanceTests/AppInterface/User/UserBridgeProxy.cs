using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.User
{
    public class UserBridgeProxy : IUserBridge
    {
        private readonly IUserBridge? realBridge;

        public UserBridgeProxy(IUserBridge? realBridge)
        {
            this.realBridge = realBridge;
        }

        public bool SignUp(string username, string password)
        {
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
