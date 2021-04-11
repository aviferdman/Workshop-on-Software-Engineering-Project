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
        private IMarket marketo; 
        private static readonly Lazy<UserManagement>
        lazy =
        new Lazy<UserManagement>
            (() => new UserManagement());

        public static UserManagement Instance { get { return lazy.Value; } }

        public IMarket Marketo { get => marketo; set => marketo = value; }

        private UserManagement()
        {
            dataUsers = new ConcurrentDictionary<string, DataUser>();
            marketo = Market.Market.Instance;
        }
        //use case 1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/11
        /// 
        //using concurrent dictionary try add if usename already exist
        //than fail and return error message otherwise return success
        public string SignUp(string username, string password, Address address, string phone)
        {
            if (dataUsers.TryAdd(username, new DataUser(username, password, address, phone)))
                    return "success";
            return "username: "+username+" is already taken please choose a different one";
        }

        //use case 2 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/21
        public string LogIn(string username, string password, string guestusername)
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
                        if(!marketo.AddMember(username,guestusername, u.Id))
                            return "a problem has occurred";
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
            marketo.RemoveGuest(username);
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

        //use case 3 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/51
        public bool Logout(string username)
        {
            DataUser u = null;
            if (dataUsers.TryGetValue(username, out u))
            {
                if (u.IsLoggedin == false)
                    return false;
                u.IsLoggedin = false;
                return marketo.logout(username);
            }
            return false;
        }

        public Guid getIdByUsername(string username)
        {
            DataUser u = null;
            if (dataUsers.TryGetValue(username, out u))
            {
                return u.Id;
            }
            throw new UnauthorizedAccessException();
        }


    }
}
