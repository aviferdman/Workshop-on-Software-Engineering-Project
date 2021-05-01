using AcceptanceTests.AppInterface.Data;

namespace AcceptanceTests.AppInterface.UserBridge
{
    public interface IUserBridge
    {
        bool Connect();
        bool Disconnect();

        // TODO:
        // Find out how to make the password safer when invoking this method.
        // Find out how the developers used a safe password protocol and use it's client.

        /// <summary>
        /// Sends a sign up request if the username isn't registered already.
        /// Otherwise, checks that the password given matches the password given in his sign up.
        /// 
        /// This method helps writing setup for tests so that the sign up function
        /// can be executed multiple times without failing the tests.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool AssureSignUp(UserInfo signupInfo);

        /// <summary>
        /// Sends a sign up request with the specified username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool SignUp(UserInfo signupInfo);

        bool Login(UserInfo loginInfo);

        /// <summary>
        /// Logins into the specified user using the password.
        /// If necessary logs out of the currently logged-in user.
        /// </summary>
        /// <param name="loginInfo">The info to login with</param>
        /// <returns>Whether logging-in was successful</returns>
        bool AssureLogin(UserInfo loginInfo);
        void tearDown();
        bool Logout();
    }
}
