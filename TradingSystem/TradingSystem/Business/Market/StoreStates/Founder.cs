using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingSystem.DAL;
using static TradingSystem.Business.Market.StoreStates.Manager;

namespace TradingSystem.Business.Market.StoreStates
{
    public class Founder : Appointer
    {

        

        public string Username { get => username; set => username = value; }
        public Store S { get => s; set => s = value; }

        public override MemberState getM() { return m; }

        private Founder(MemberState m, Store s)
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


      

    }
}
