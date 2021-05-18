using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.UserManagement;
using System.Threading.Tasks;
using TradingSystem.Business.Market;
namespace TradingSystem.DAL
{
    class ProxyMarketContext
    {
        private bool isDebug;

        private ConcurrentDictionary<string, DataUser> dataUsers;
        private ConcurrentDictionary<string, RegisteredAdmin> admins;
        public bool IsDebug { get => isDebug; set => isDebug = value; }

        private MarketContext marketContext = MarketContext.Instance;
        public static ProxyMarketContext Instance { get { return _lazy.Value; } }

        private static readonly Lazy<ProxyMarketContext>
       _lazy =
       new Lazy<ProxyMarketContext>
           (() => new ProxyMarketContext());

        public ProxyMarketContext()
        {
            isDebug = false;
            dataUsers = new ConcurrentDictionary<string, DataUser>();
            admins = new ConcurrentDictionary<string, RegisteredAdmin>();
            RegisteredAdmin admin = new RegisteredAdmin("DEFUALT_ADMIN", "ADMIN", new Address("Israel", "Beer Sheva", "lala", "5", "1111111"), "0501234566");
            dataUsers.TryAdd("DEFUALT_ADMIN", admin);
            admins.TryAdd("DEFUALT_ADMIN", admin);
        }

        internal void UserTearDown()
        {
            dataUsers = new ConcurrentDictionary<string, DataUser>();
        }

        public async Task<DataUser> GetDataUser(string username)
        {
            if (isDebug)
            {
                DataUser u = null;
                dataUsers.TryGetValue(username, out u);
                return u;
            }
            try
            {
                return await marketContext.GetDataUser(username);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<bool> RemoveDataUser(string username)
        {
            if (isDebug)
            {
                DataUser u = null;
                return dataUsers.TryRemove(username, out u);
            }
            try
            {
                return await marketContext.RemoveDataUser(username);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> AddDataUser(DataUser u)
        {
            if (isDebug)
            {
                return dataUsers.TryAdd(u.username, u);
            }
            try
            {
                return await marketContext.AddDataUser(u);
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
