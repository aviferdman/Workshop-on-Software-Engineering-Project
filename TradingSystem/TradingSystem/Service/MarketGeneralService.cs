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
        private static readonly Lazy<MarketGeneralService> instanceLazy = new Lazy<MarketGeneralService>(() => new MarketGeneralService(), true);

        private readonly MarketUsers marketUsers;
        private readonly MarketStores marketStores;

        private MarketGeneralService()
        {
            marketUsers = MarketUsers.Instance;
            marketStores = MarketStores.Instance;
        }

        public static MarketGeneralService Instance => instanceLazy.Value;

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
            ProxyMarketContext.Instance.IsDebug = debugMode;
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
