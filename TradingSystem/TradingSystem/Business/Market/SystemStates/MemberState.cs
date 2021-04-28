using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;
using TradingSystem.Business.Market.StoreStates;
using TradingSystem.Notifications;

namespace TradingSystem.Business.Market
{
    public class MemberState : State
    {

        private string _userId;
        private ICollection<IHistory> _userHistory;
        private ConcurrentDictionary<Store,Founder> founderPrems;
        private ConcurrentDictionary<Store, Owner> ownerPrems;
        private ConcurrentDictionary<Store, Manager> managerPrems;
        private ConcurrentDictionary<string, Manager> managerAppointments;
        private ConcurrentDictionary<string, Owner> ownerAppointments;
        private object prem_lock;
        public MemberState(string userId, ICollection<IHistory> userHistory) : base()
        {
            this._userId = userId;
            this.prem_lock = new object();
            this._userHistory = userHistory;
            founderPrems = new ConcurrentDictionary<Store, Founder>();
            ownerPrems = new ConcurrentDictionary<Store, Owner>();
            managerPrems = new ConcurrentDictionary<Store, Manager>();
            managerAppointments = new ConcurrentDictionary<string, Manager>();
            ownerAppointments = new ConcurrentDictionary<string, Owner>();
        }

        public string UserId { get => _userId; set => _userId = value; }
        public ConcurrentDictionary<Store, Founder> FounderPrems { get => founderPrems; set => founderPrems = value; }
        public ConcurrentDictionary<Store, Owner> OwnerPrems { get => ownerPrems; set => ownerPrems = value; }
        public ConcurrentDictionary<Store, Manager> ManagerPrems { get => managerPrems; set => managerPrems = value; }
        public object Prem_lock { get => prem_lock; set => prem_lock = value; }
        public ConcurrentDictionary<string, Manager> ManagerAppointments { get => managerAppointments; set => managerAppointments = value; }
        public ConcurrentDictionary<string, Owner> OwnerAppointments { get => ownerAppointments; set => ownerAppointments = value; }

        public override ICollection<IHistory> GetAllHistory()
        {
            throw new UnauthorizedAccessException();
        }

        public override ICollection<IHistory> GetStoreHistory(Store store)
        {
            throw new UnauthorizedAccessException();
        }

        public override ICollection<IHistory> GetUserHistory(string username)
        {
            return _userHistory;
        }

        public bool isStaff(Store s)
        {
            return founderPrems.ContainsKey(s) || managerPrems.ContainsKey(s) || ownerPrems.ContainsKey(s);
        }

    }
}
