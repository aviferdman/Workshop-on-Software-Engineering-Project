using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.UserManagement;
using System.Threading.Tasks;
namespace TradingSystem.DAL
{
    class UsersDAL
    {
        ProxyMarketContext proxyMarketContext = ProxyMarketContext.Instance;
        public static UsersDAL Instance { get { return _lazy.Value; } }

        private static readonly Lazy<UsersDAL>
       _lazy =
       new Lazy<UsersDAL>
           (() => new UsersDAL());
        //returns null if not exist
        public async Task<DataUser> GetDataUser(string username)
        {
            return await proxyMarketContext.GetDataUser(username);
        }

        //returns false if not successfull
        public async Task<bool> AddDataUser(DataUser u)
        {
            return await proxyMarketContext.AddDataUser(u);
        }

        //returns false if not successfull
        public async Task<bool> RemoveDataUser(string username)
        {
            return await proxyMarketContext.RemoveDataUser(username);
        }

        public void TearDown()
        {
            proxyMarketContext.UserTearDown();
        }

        public async Task AddNewMemberState(string username)
        {
            await proxyMarketContext.AddNewMemberState(username);
        }

        public async Task AddNewShoppingCart(string username)
        {
            await proxyMarketContext.AddNewShoppingCart(username);
        }
    }
}
