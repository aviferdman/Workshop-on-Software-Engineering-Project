using System;
using System.Collections.Generic;

using TradingSystem.Business.Market;

namespace TradingSystem.Service
{
    public class MarketStoreGeneralService
    {
        private readonly MarketStores marketStores;

        public MarketStoreGeneralService()
        {
            marketStores = MarketStores.Instance;
        }

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

        public HistoryData GetStoreHistory(string username, Guid storeId)
        {
            StoreHistory history = marketStores.GetStoreHistory(username, storeId);
            return new HistoryData(history);
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
