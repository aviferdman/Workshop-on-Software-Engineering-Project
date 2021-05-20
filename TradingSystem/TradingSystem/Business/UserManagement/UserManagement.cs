using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Market;
using TradingSystem.DAL;
using System.Threading.Tasks;
namespace TradingSystem.Business.UserManagement
{
    //this class tests are in UserMangmentTests
    public class UserManagement
    {
        private UsersDAL usersDAL= UsersDAL.Instance;
        private static readonly Lazy<UserManagement>
        lazy =
        new Lazy<UserManagement>
            (() => new UserManagement());

        public static UserManagement Instance { get { return lazy.Value; } }


        internal int GetUserAge(string username)
        {
            throw new NotImplementedException();
        }

        public void tearDown()
        {
            usersDAL.TearDown();
        }

        private UserManagement()
        {
            
        }

       

        //use case 1 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/11
        //using concurrent dictionary try add if usename already exist
        //than fail and return error message otherwise return success
        public async Task<string> SignUp( string username, string password, Address address, string phone)
        {
            if (username == null)
                return "username cannot be null";
            if (await usersDAL.AddDataUser(new DataUser(username, password, address, phone)))
            {
                await usersDAL.AddNewMemberState(username);
                await usersDAL.AddNewShoppingCart(username);
                return "success";
            }
                
            return "username: "+username+" is already taken please choose a different one";
        }

        
        public async Task<string> LogIn(string username, string password)
        {
            DataUser u = await usersDAL.GetDataUser(username);
            if (u!=null)
            {
                if (u.password.Equals(password))
                {
                    if (u.isLoggedin)
                        return "user is already logged in";
                    else
                    {
                        u.isLoggedin = true;
                        return "success";
                    }
                        
                }

                else 
                    return "the password doesn't match username: " + username;
            }
                
            return "username: " + username + " doesn't exist in the system";
        }

        //for unit test
        public async Task<bool> DeleteUser(string username)
        {
            DataUser u=null;
            return await usersDAL.RemoveDataUser(username);
        }

        private async Task<Result<String>> getUserPhone(string username)
        {
            DataUser u = await usersDAL.GetDataUser(username);
            if (u != null)
            {
                return new Result<string>(u.phone, false, "");
            }
            return new Result<string>(null , true, "username doesn't exist");
        }

        private async Task<Result<Address>> getUserAddress(string username)
        {
            DataUser u = await usersDAL.GetDataUser(username);
            if (u != null)
            {
                return new Result<Address>(u.address, false, "");
            }
            return new Result<Address>(null, true, "username doesn't exist");
        }

        
        public async  Task<bool> Logout(string username)
        {
            if (username == null)
                return false;
            DataUser u = await usersDAL.GetDataUser(username);
            if (u != null)
            {
                if (u.isLoggedin == false)
                    return false;
                u.isLoggedin = false;
                return true;
            }
            return false;
        }

        public async Task<bool> isMember(string username)
        {
            return (await usersDAL.GetDataUser(username))!=null;
        }

        public async Task<bool> isLoggedIn(string username)
        {
            DataUser u = await usersDAL.GetDataUser(username);
            if (u != null)
                return false;
            return u.isLoggedin;
        }


    }
}
