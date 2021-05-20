using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public class AdministratorState : MemberState
    {
        private string _userId;
        private ICollection<IHistory> _userHistory;
        public AdministratorState(string username) : base(username)
        {
            _userId = username;
        }

        public override Task<ICollection<IHistory>> GetAllHistory()
        {
            return HistoryManager.Instance.GetAllHistories();
        }

        //Use case 41 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/67
        public override Task<ICollection<IHistory>> GetStoreHistory(Store store)
        {
            return store.GetStoreHistory(_userId);
        }

        //use case 40 : https://github.com/aviferdman/Workshop-on-Software-Engineering-Project/issues/66
        public override Task<ICollection<IHistory>> GetUserHistory(string username)
        {
            return HistoryManager.Instance.GetUserHistory(username);
        }
    }
}
