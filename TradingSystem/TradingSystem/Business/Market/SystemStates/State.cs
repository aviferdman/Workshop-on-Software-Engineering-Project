using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.Business.Interfaces;
using TradingSystem.Business.Market;

namespace TradingSystem.Business.Market
{
    public abstract class State
    {
        public static readonly Transaction _transaction = Transaction.Instance;
    public string username { get; set; }
        public State(string username)
        {
            this.username = username;
        }
        public abstract void AddHistory(IHistory history);

        public abstract Task<ICollection<IHistory>> GetUserHistory(string username);

        public abstract Task<ICollection<IHistory>> GetStoreHistory(Store store);

        public abstract Task<ICollection<IHistory>> GetAllHistory();

        public Transaction GetTransaction()
        {
            return _transaction;
        }

    }
}
