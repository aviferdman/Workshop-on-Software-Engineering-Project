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
            string cardNumber,
            string month,
            string year,
            string holderName,
            string cvv, 
            string holderId,
            string state,
            string city,
            string street,
            string apartmentNum,
            string zip
        )
        {
            var card = new CreditCard(cardNumber, month, year, holderName, cvv, holderId);
            var address = new Address(state, city, street, apartmentNum, zip);
            Store store = marketStores.CreateStore(name, username, card, address);
            if (store == null)
                return null;
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

        public StoreData getStoreById(Guid pid)
        {
            IStore s;
            if (marketStores.Stores.TryGetValue(pid,out s))
                return new StoreData((Store)s);
            return null;
        }
    }
}
