using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<StoreData> CreateStoreAsync
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
            Store store = await marketStores.CreateStore(name, username, card, address);
            if (store == null)
                return null;
            return new StoreData(store);
        }

        public async Task<ICollection<HistoryData>> GetStoreHistory(string username, Guid storeId)
        {
            ICollection<IHistory> history = await marketStores.GetStoreHistory(username, storeId);
            ICollection<HistoryData> ret = new HashSet<HistoryData>(); 
            foreach (var h in history)
            {
                ret.Add(new HistoryData(h));
            }
            return ret;
        }

        public async Task<ICollection<StoreData>> FindStoresByName(string name)
        {
            ICollection<Store> stores = await marketStores.GetStoresByName(name);
            ICollection<StoreData> dataStores = new LinkedList<StoreData>();
            foreach (Store s in stores)
            {
                dataStores.Add(new StoreData(s));
            }
            return dataStores;
        }

        public async Task<StoreData> getStoreById(Guid pid)
        {
            Store s=await marketStores.GetStoreById(pid);
            if (s!=null)
                return new StoreData((Store)s);
            return null;
        }
    }
}
