using System;
using System.Collections.Generic;
using System.Text;

namespace AcceptanceTests.AppInterface.User
{
    public interface IUserBridge
    {
        // TODO:
        // Find out how to make the password safer when invoking this method.
        // Find out how the developers used a safe password protocol and use it's client.
        bool SignUp(string username, string password);

        bool Login(string username, string password); 
    }
}
