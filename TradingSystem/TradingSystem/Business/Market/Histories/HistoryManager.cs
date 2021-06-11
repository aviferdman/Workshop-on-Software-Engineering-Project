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
    public class HistoryManager
    {

        MarketDAL m;
        public HistoryManager(MarketDAL m)
        {
            this.m = m;
        }
      

        public async void AddHistory(TransactionStatus history)
        {
            await m.AddHistory(history);
        }

        public async Task<ICollection<IHistory>> GetAllHistories()
        {
            ICollection<TransactionStatus> transactionStatuses = await m.getAllHistories();
            List<IHistory> histories = new List<IHistory>();
            foreach(TransactionStatus t in transactionStatuses)
            {
                //histories.Add(new UserHistory(t));
                histories.Add(new StoreHistory(t));
            }
            return histories;
        }

        public async Task<ICollection<IHistory>> GetUserHistory(string username)
        {
            ICollection<TransactionStatus> transactionStatuses = await m.getUserHistories(username);
            List<IHistory> histories = new List<IHistory>();
            foreach (TransactionStatus t in transactionStatuses)
            {
                histories.Add(new UserHistory(t));
            }
            return histories;
        }

        public async Task<ICollection<IHistory>> GetStoreHistory(Guid storeId)
        {
            ICollection<TransactionStatus> transactionStatuses = await m.getStoreHistories(storeId);
            List<IHistory> histories = new List<IHistory>();
            foreach (TransactionStatus t in transactionStatuses)
            {
                histories.Add(new StoreHistory(t));
            }
            return histories;
        }
    }
}
