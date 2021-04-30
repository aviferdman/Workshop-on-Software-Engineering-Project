using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingSystem.Business.Interfaces;

namespace TradingSystem.Business.Market
{
    class HistoryManager
    {
        private ICollection<IHistory> histories;
        private static readonly Lazy<HistoryManager>
        _lazy =
        new Lazy<HistoryManager>
            (() => new HistoryManager());

        public HistoryManager()
        {
            histories = new HashSet<IHistory>();
        }
        public static HistoryManager Instance { get { return _lazy.Value; } }

        public void AddUserHistory(IHistory history)
        {
            histories.Add(history);
        }

        public ICollection<IHistory> GetAllHistories()
        {
            return histories;
        }

        public ICollection<IHistory> GetUserHistory(string username)
        {
            return histories.Where(h => h.GetPaymentStatus().Username.Equals(username)).ToList();
        }
    }
}
