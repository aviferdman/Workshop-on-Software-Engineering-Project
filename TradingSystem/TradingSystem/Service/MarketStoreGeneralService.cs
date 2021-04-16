using System;
using System.Collections.Generic;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class MarketStoreGeneralService
    {
        private static readonly Lazy<MarketStoreGeneralService> instanceLazy = new Lazy<MarketStoreGeneralService>(() => new MarketStoreGeneralService(), true);

        private readonly MarketStores marketStores;

        private MarketStoreGeneralService()
        {
            marketStores = MarketStores.Instance;
        }

        public static MarketStoreGeneralService Instance => instanceLazy.Value;

        public StoreData CreateStore
        (
            string name,
            string username,
            int accountNumber,
            int branch,
            string state,
            string city,
            string street,
            string apartmentNum
        )
        {
            var bankAccount = new BankAccount(accountNumber, branch);
            var address = new Address(state, city, street, apartmentNum);
            Store store = marketStores.CreateStore(name, username, bankAccount, address);
            return new StoreData(store);
        }

        public ICollection<HistoryData> GetStoreHistory(string username, Guid storeId)
        {
            ICollection<IHistory> history = marketStores.GetStoreHistory(username, storeId);
            ICollection<HistoryData> ret = new HashSet<HistoryData>(); 
            foreach (var h in history)
            {
                ret.Add(new HistoryData(h));
            }
            return ret;
        }

        public ICollection<StoreData> FindStoresByName(string name)
        {
            ICollection<Store> stores = marketStores.GetStoresByName(name);
            ICollection<StoreData> dataStores = new LinkedList<StoreData>();
            foreach (Store s in stores)
            {
                dataStores.Add(new StoreData(s));
            }
            return dataStores;
        }
    }
}
