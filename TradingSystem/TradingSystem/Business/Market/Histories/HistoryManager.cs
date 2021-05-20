using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market.Histories;
using TradingSystem.DAL;

namespace TradingSystem.Business.Market
{
    class HistoryManager
    {
        private static readonly Lazy<HistoryManager>
        _lazy =
        new Lazy<HistoryManager>
            (() => new HistoryManager());

        public HistoryManager()
        {
        }
        public static HistoryManager Instance { get { return _lazy.Value; } }

        public async void AddHistory(TransactionStatus history)
        {
            await MarketDAL.Instance.AddHistory(history);
        }

        public async Task<ICollection<IHistory>> GetAllHistories()
        {
            ICollection<TransactionStatus> transactionStatuses = await MarketDAL.Instance.getAllHistories();
            List<IHistory> histories = new List<IHistory>();
            foreach(TransactionStatus t in transactionStatuses)
            {
                histories.Add(new UserHistory(t));
                histories.Add(new StoreHistory(t));
            }
            return histories;
        }

        public async Task<ICollection<IHistory>> GetUserHistory(string username)
        {
            ICollection<TransactionStatus> transactionStatuses = await MarketDAL.Instance.getUserHistories(username);
            List<IHistory> histories = new List<IHistory>();
            foreach (TransactionStatus t in transactionStatuses)
            {
                histories.Add(new UserHistory(t));
            }
            return histories;
        }

        public async Task<ICollection<IHistory>> GetStoreHistory(Guid storeId)
        {
            ICollection<TransactionStatus> transactionStatuses = await MarketDAL.Instance.getStoreHistories(storeId);
            List<IHistory> histories = new List<IHistory>();
            foreach (TransactionStatus t in transactionStatuses)
            {
                histories.Add(new StoreHistory(t));
            }
            return histories;
        }
    }
}
