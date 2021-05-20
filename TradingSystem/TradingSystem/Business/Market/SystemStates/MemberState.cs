using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.Histories;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Notifications;

namespace TradingSystem.Business.Market
{
    public class MemberState : State
    {

        public string username { get; set; }
        private ConcurrentDictionary<Store, Founder> founderPrems { get; set; }
        private ConcurrentDictionary<Store, Owner> ownerPrems { get; set; }
        private ConcurrentDictionary<Store, Manager> managerPrems { get; set; }
        private ConcurrentDictionary<string, Manager> managerAppointments { get; set; }
        private ConcurrentDictionary<string, Owner> ownerAppointments { get; set; }
        public MemberState(string userId) : base()
        {
            this.username = userId;
            founderPrems = new ConcurrentDictionary<Store, Founder>();
            ownerPrems = new ConcurrentDictionary<Store, Owner>();
            managerPrems = new ConcurrentDictionary<Store, Manager>();
            managerAppointments = new ConcurrentDictionary<string, Manager>();
            ownerAppointments = new ConcurrentDictionary<string, Owner>();
        }

        public string UserId { get => username; set => username = value; }

        public override Task<ICollection<IHistory>> GetAllHistory()
        {
            throw new UnauthorizedAccessException();
        }

        public override Task<ICollection<IHistory>> GetStoreHistory(Store store)
        {
            throw new UnauthorizedAccessException();
        }

        public override void AddHistory(IHistory history)
        {
            HistoryManager.Instance.AddHistory(((UserHistory)history)._transactionStatus);
        }

        public async override Task<ICollection<IHistory>> GetUserHistory(string username)
        {
            return await HistoryManager.Instance.GetUserHistory(username);
        }


    }
}
