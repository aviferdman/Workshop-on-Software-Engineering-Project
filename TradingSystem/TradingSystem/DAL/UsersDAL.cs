using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.UserManagement;
using System.Threading.Tasks;
using TradingSystem.Business.Market;

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

        //returns false if not successfull
        public  Statistics getStatis(DateTime date)
        {
            return  proxyMarketContext.getStatis(date);
        }

        public void TearDown()
        {
            proxyMarketContext.UserTearDown();
        }

        public async Task AddNewMemberState(string username)
        {
            await proxyMarketContext.AddNewMemberState(username);
        }

        internal async Task AddNewAdminState(string username, string password, string phone)
        {
            await proxyMarketContext.AddNewAdminState(username, password, phone);
        }

        public async Task AddNewShoppingCart(string username)
        {
            await proxyMarketContext.AddNewShoppingCart(username);
        }
    }
}
