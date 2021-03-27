using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace TradingSystem.Business.UserManagement
{
    class UserManagement
    {
        private ConcurrentDictionary<string, DataUser> dataUsers;
        private Authentication authentication;
        private static readonly Lazy<UserManagement>
        lazy =
        new Lazy<UserManagement>
            (() => new UserManagement());

        public static UserManagement Instance { get { return lazy.Value; } }

        private UserManagement()
        {
            dataUsers = new ConcurrentDictionary<string, DataUser>();
        }
        //use case 2 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/21

        //using concurrent dictionary try add if usename already exist
        //than fail and return error message otherwise return success
        public string SignUp(string username, string password, string address)
        {
            if(dataUsers.TryAdd(username, new DataUser(username, password, address)))
                return "success";
            return "username is already taken please choose a different one";
        }
    }
}
