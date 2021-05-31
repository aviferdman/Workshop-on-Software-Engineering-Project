using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.DAL;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StoreStates
{
    public class Founder : Appointer
    {        
        [NotMapped]
        public string Username { get => username; set => username = value; }
        
        public override MemberState getM() { return m; }

        private Founder(MemberState m, Store s): base()
        {
            this.username = m.UserId;
            this.m = m;
            this.sid = sid;
            this.s = s;
        }

        public static Founder makeFounder(MemberState m, Store s)
        {
            return new Founder(m, s);
        }

        public Founder()
        {
        }
    }
}
