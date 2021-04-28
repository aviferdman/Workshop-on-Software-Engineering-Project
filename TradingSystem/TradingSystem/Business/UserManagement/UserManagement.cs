using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.UserManagement
{
    //this class tests are in UserMangmentTests
    public class UserManagement
    {
        private ConcurrentDictionary<string, DataUser> dataUsers;
        private ConcurrentDictionary<string, RegisteredAdmin> admins;
        private static readonly Lazy<UserManagement>
        lazy =
        new Lazy<UserManagement>
            (() => new UserManagement());

        public static UserManagement Instance { get { return lazy.Value; } }

        public ConcurrentDictionary<string, DataUser> DataUsers { get => dataUsers; set => dataUsers = value; }

        internal int GetUserAge(string username)
        {
            throw new NotImplementedException();
        }

        private UserManagement()
        {
            dataUsers = new ConcurrentDictionary<string, DataUser>();
            admins = new ConcurrentDictionary<string, RegisteredAdmin>();
            RegisteredAdmin admin = new RegisteredAdmin("DEFULT_ADMIN", "ADMIN", new Address("Israel", "Beer Sheva", "lala", "5"), "0501234566");
            admin.IsAdmin = true;
            dataUsers.TryAdd("DEFULT_ADMIN",admin);
            admins.TryAdd("DEFULT_ADMIN", admin);
        }

        //use case 1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/11
        //using concurrent dictionary try add if usename already exist
        //than fail and return error message otherwise return success
        public string SignUp( string username, string password, Address address, string phone, bool isAdmin)
        {
            if(isAdmin)
            {
                if (dataUsers.TryAdd(username, new RegisteredAdmin(username, password, address, phone)))
                    return "success";
            }
            else
            {
                if (dataUsers.TryAdd(username, new DataUser(username, password, address, phone)))
                    return "success";
            }
            
            return "username: "+username+" is already taken please choose a different one";
        }

        
        public string LogIn(string username, string password)
        {
            DataUser u = null;
            if (dataUsers.TryGetValue(username, out u))
            {
                if (u.Password.Equals(password))
                {
                    if (u.IsLoggedin)
                        return "user is already logged in";
                    else
                    {
                        u.IsLoggedin = true;
                        return "success";
                    }
                        
                }

                else 
                    return "the password doesn't match username: " + username;
            }
                
            return "username: " + username + " doesn't exist in the system";
        }

        //for unit test
        public bool DeleteUser(string username)
        {
            DataUser u=null;
            return dataUsers.TryRemove(username, out u);
        }

        private Result<String> getUserPhone(string username)
        {
            DataUser u = null;
            if (dataUsers.TryGetValue(username, out u))
            {
                return new Result<string>(u.Phone, false, "");
            }
            return new Result<string>(null , true, "username doesn't exist");
        }

        private Result<Address> getUserAddress(string username)
        {
            DataUser u = null;
            if (dataUsers.TryGetValue(username, out u))
            {
                return new Result<Address>(u.Address, false, "");
            }
            return new Result<Address>(null, true, "username doesn't exist");
        }

        
        public bool Logout(string username)
        {
            DataUser u = null;
            if (dataUsers.TryGetValue(username, out u))
            {
                if (u.IsLoggedin == false)
                    return false;
                u.IsLoggedin = false;
                return true;
            }
            return false;
        }

        public bool isMember(string username)
        {
            return dataUsers.ContainsKey(username);
        }

        public bool isLoggedIn(string username)
        {
            DataUser u;
            if (!dataUsers.TryGetValue(username, out u))
                return false;
            return u.IsLoggedin;
        }


    }
}
