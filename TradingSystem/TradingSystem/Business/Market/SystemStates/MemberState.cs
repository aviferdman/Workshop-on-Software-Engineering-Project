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

        
        public HashSet<Founder> founderPrems { get; set; }
        public HashSet<Owner> ownerPrems { get; set; }
        public HashSet<Manager> managerPrems { get; set; }
        public HashSet<Manager> managerAppointments { get; set; }
        public HashSet<Owner> ownerAppointments { get; set; }
        public MemberState(string userId) : base(userId)
        {
            founderPrems = new HashSet<Founder>();
            ownerPrems = new HashSet<Owner>();
            managerPrems = new HashSet<Manager>();
            managerAppointments = new HashSet<Manager>();
            ownerAppointments = new HashSet<Owner>();
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
