using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Delivery;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Payment;
using TradingSystem.DAL;

namespace TradingSystem.Service
{
    public class MarketGeneralService
    {
       
        private readonly MarketUsers marketUsers;
        private readonly MarketStores marketStores;
        private ProxyMarketContext proxyMarketContext;

        public MarketGeneralService(MarketUsers marketUsers, MarketStores marketStores, ProxyMarketContext proxyMarketContext)
        {
            this.marketUsers = marketUsers;
            this.marketStores = marketStores;
            this.proxyMarketContext = proxyMarketContext;
        }


        public void tearDown()
        {
            marketStores.tearDown();
            marketUsers.tearDown();
        }
        public void ActivateDebugMode(Mock<ExternalDeliverySystem> deliverySystem, Mock<ExternalPaymentSystem> paymentSystem, bool debugMode = false)
        {
            marketStores.ActivateDebugMode(deliverySystem, paymentSystem, debugMode);
        }

        public void SetDbDebugMode(bool debugMode = true)
        {
            proxyMarketContext.IsDebug = debugMode;
        }

        public async System.Threading.Tasks.Task<ICollection<HistoryData>> GetAllHistoryAsync(string username)
        {
            ICollection<IHistory> histories = await marketUsers.GetAllHistory(username);
            ICollection<HistoryData> ret = new HashSet<HistoryData>();
            foreach (IHistory his in histories)
            {
                ret.Add(new HistoryData(his));
            }
            return ret;
        }
    }
}
